using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PanaderiaCore.Data;
using PanaderiaCore.Models;
using System.Linq;

namespace PanaderiaCore.Controllers
{
    public class ProductoController : Controller
    {
        private readonly PanaderiaDbContext _context;

        public ProductoController(PanaderiaDbContext context)
        {
            _context = context;
        }

        // Listar productos
        public IActionResult Index()
        {
            var productos = _context.Productos.ToList();
            return View(productos);
        }

        // Crear producto
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Producto producto)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        // Asegúrate de que FechaCreacion esté inicializado
                        if (producto.FechaCreacion == default)
                        {
                            producto.FechaCreacion = DateTime.Now;
                        }

                        // Agregar el producto
                        _context.Productos.Add(producto);
                        _context.SaveChanges();

                        // Agregar el producto al inventario
                        var inventario = new Inventario
                        {
                            ProductoNombre = producto.Nombre,
                            CantidadDisponible = 0, // Por defecto en 0, puede ser modificado
                            UltimaActualizacion = DateTime.Now
                        };

                        _context.Inventarios.Add(inventario);
                        _context.SaveChanges();

                        // Confirmar la transacción
                        transaction.Commit();
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        ModelState.AddModelError("", $"Error al guardar el producto: {ex.Message}");
                    }
                }
            }
            return View(producto);
        }


        // Editar producto
        public IActionResult Edit(string id)
        {
            var producto = _context.Productos.Find(id);
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }

        [HttpPost]
        public IActionResult Edit(Producto producto)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        // Solo actualiza las propiedades principales del producto
                        var productoExistente = _context.Productos
                            .FirstOrDefault(p => p.Nombre == producto.Nombre);

                        if (productoExistente == null)
                        {
                            ModelState.AddModelError("", "El producto no existe.");
                            return View(producto);
                        }

                        productoExistente.Precio = producto.Precio;
                        productoExistente.Categoria = producto.Categoria;
                        productoExistente.Descripcion = producto.Descripcion;

                        _context.SaveChanges();
                        transaction.Commit();

                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        ModelState.AddModelError("", $"Error al guardar los cambios: {ex.Message}");
                    }
                }
            }
            return View(producto);
        }


        // Detalles del producto
        public IActionResult Details(string id)
        {
            var producto = _context.Productos.Find(id);
            if (producto == null) return NotFound();
            return View(producto);
        }

        // Eliminar producto
        public IActionResult Delete(string id)
        {
            var producto = _context.Productos.Find(id);
            if (producto == null) return NotFound();
            return View(producto);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(string id)
        {
            var producto = _context.Productos.Find(id);
            if (producto != null)
            {
                try
                {
                    _context.Productos.Remove(producto);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error al eliminar el producto: {ex.Message}");
                }
            }
            else
            {
                ModelState.AddModelError("", "Producto no encontrado.");
            }

            return RedirectToAction("Index");
        }


    }
}
