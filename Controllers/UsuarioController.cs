namespace PanaderiaCore.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using System.Linq;
    using PanaderiaCore.Data;
    using PanaderiaCore.Models;
    using System.Security.Claims;

    [Authorize]
    public class UsuarioController : Controller
    {
        private readonly PanaderiaDbContext _context;

        public UsuarioController(PanaderiaDbContext context)
        {
            _context = context;
        }

        public IActionResult Catalogo()
        {
            var productos = _context.Productos.ToList();
            return View(productos);
        }

        public IActionResult Comprar(string id)
        {
            // Buscar el producto por su nombre
            var producto = _context.Productos.FirstOrDefault(p => p.Nombre == id);
            if (producto == null)
            {
                TempData["Error"] = "El producto no existe.";
                return RedirectToAction("Catalogo");
            }

            // Obtener el inventario del producto
            var inventario = _context.Inventarios.FirstOrDefault(i => i.ProductoNombre == id);
            if (inventario == null)
            {
                TempData["Error"] = "El producto no tiene inventario asociado.";
                return RedirectToAction("Catalogo");
            }

            ViewBag.InventarioDisponible = inventario.CantidadDisponible;
            return View(producto);
        }

        [HttpPost]
        public IActionResult ConfirmarCompra(DetalleCompra detalle)
        {
            // Recuperar el correo del usuario autenticado
            var clienteEmail = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(clienteEmail))
            {
                TempData["Error"] = "No se pudo identificar al cliente.";
                return RedirectToAction("Catalogo");
            }

            // Validar que el producto existe
            var producto = _context.Productos.FirstOrDefault(p => p.Nombre == detalle.ProductoNombre);
            if (producto == null)
            {
                TempData["Error"] = "El producto no existe.";
                return RedirectToAction("Catalogo");
            }

            // Validar que el inventario del producto existe
            var inventario = _context.Inventarios.FirstOrDefault(i => i.ProductoNombre == detalle.ProductoNombre);
            if (inventario == null)
            {
                TempData["Error"] = "El producto no tiene inventario disponible.";
                return RedirectToAction("Comprar", new { id = detalle.ProductoNombre });
            }

            // Validar que la cantidad solicitada no exceda la cantidad disponible
            if (detalle.Cantidad > inventario.CantidadDisponible)
            {
                TempData["Error"] = $"No puedes comprar más de {inventario.CantidadDisponible} unidades del producto.";
                return RedirectToAction("Comprar", new { id = detalle.ProductoNombre });
            }

            // Crear la compra
            var nuevaCompra = new Compra
            {
                ClienteEmail = clienteEmail,
                FechaCompra = DateTime.Now,
                Total = detalle.Cantidad * producto.Precio // Calcula el total basado en el precio y la cantidad
            };

            _context.Compras.Add(nuevaCompra);
            _context.SaveChanges(); // Guarda la compra para generar el IdFactura

            // Asociar el detalle de compra a la compra recién creada
            detalle.IdFactura = nuevaCompra.IdFactura;
            detalle.PrecioUnitario = producto.Precio;

            _context.DetalleCompras.Add(detalle);

            // Reducir el inventario
            inventario.CantidadDisponible -= detalle.Cantidad;
            inventario.UltimaActualizacion = DateTime.Now;

            _context.SaveChanges();

            TempData["Success"] = "Compra realizada exitosamente.";
            return RedirectToAction("Catalogo");
        }
    }
}
