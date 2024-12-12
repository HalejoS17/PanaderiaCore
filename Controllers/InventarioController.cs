using Microsoft.AspNetCore.Mvc;
using PanaderiaCore.Data;
using PanaderiaCore.Models;
using System.Linq;

namespace PanaderiaCore.Controllers
{
    public class InventarioController : Controller
    {
        private readonly PanaderiaDbContext _context;

        public InventarioController(PanaderiaDbContext context)
        {
            _context = context;
        }

        public IActionResult GestionInventario()
        {
            var inventarios = _context.Inventarios.ToList();
            return View(inventarios);
        }

        public IActionResult EditInventario(string id)
        {
            var inventario = _context.Inventarios.FirstOrDefault(i => i.ProductoNombre == id);
            if (inventario == null)
            {
                return NotFound();
            }
            return View(inventario);
        }

        [HttpPost]
        public IActionResult EditInventario(Inventario inventario)
        {
            if (!ModelState.IsValid)
            {
                var errores = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                TempData["Error"] = "Errores en los datos proporcionados: " + string.Join(", ", errores);
                return View(inventario);
            }

            try
            {
                var inventarioExistente = _context.Inventarios.FirstOrDefault(i => i.ProductoNombre == inventario.ProductoNombre);

                if (inventarioExistente != null)
                {
                    int cantidadAnterior = inventarioExistente.CantidadDisponible;
                    int cantidadNueva = inventario.CantidadDisponible;
                    int diferencia = cantidadNueva - cantidadAnterior;

                    if (diferencia > 0)
                    {
                        var reabastecimiento = new Reabastecimiento
                        {
                            ProductoNombre = inventario.ProductoNombre,
                            Cantidad = diferencia,
                            Fecha = DateTime.Now
                        };
                        _context.Reabastecimientos.Add(reabastecimiento);
                    }

                    inventarioExistente.CantidadDisponible = cantidadNueva;
                    inventarioExistente.UltimaActualizacion = DateTime.Now;

                    _context.SaveChanges();
                    TempData["Success"] = "Cambios guardados exitosamente.";
                    return RedirectToAction("GestionInventario");
                }
                else
                {
                    TempData["Error"] = "El inventario no existe en la base de datos.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al guardar cambios: {ex.Message}";
            }

            return View(inventario);
        }

        public IActionResult ReabastecimientosPorRango(DateTime? fechaInicio, DateTime? fechaFin)
        {
            if (!fechaInicio.HasValue || !fechaFin.HasValue)
            {
                return View();
            }

            var reabastecimientos = _context.Reabastecimientos
                .Where(r => r.Fecha >= fechaInicio && r.Fecha <= fechaFin)
                .GroupBy(r => r.ProductoNombre)
                .Select(g => new ReabastecimientoViewModel
                {
                    ProductoNombre = g.Key,
                    VecesReabastecido = g.Count(),
                    TotalCantidad = g.Sum(r => r.Cantidad)
                })
                .ToList();

            return View(reabastecimientos);
        }
    }

    public class ReabastecimientoViewModel
    {
        public string ProductoNombre { get; set; }
        public int VecesReabastecido { get; set; }
        public int TotalCantidad { get; set; }
    }
}