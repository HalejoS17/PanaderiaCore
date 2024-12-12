namespace PanaderiaCore.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Linq;
    using PanaderiaCore.Data;
    using PanaderiaCore.Models;
    using System.Collections.Generic;

    public class AdminController : Controller
    {
        private readonly PanaderiaDbContext _context;

        public AdminController(PanaderiaDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            var rankingProductos = _context.DetalleCompras
                .GroupBy(d => d.ProductoNombre)
                .Select(g => new
                {
                    Producto = g.Key,
                    TotalCantidad = g.Sum(d => d.Cantidad)
                })
                .OrderByDescending(p => p.TotalCantidad)
                .ToList();

            ViewBag.RankingProductos = rankingProductos;

            return View();
        }

        public IActionResult ProductosMasYMenosVendidos()
        {
            var masVendido = _context.DetalleCompras
                .GroupBy(d => d.ProductoNombre)
                .OrderByDescending(g => g.Sum(d => d.Cantidad))
                .Select(g => new { Producto = g.Key, TotalCantidad = g.Sum(d => d.Cantidad) })
                .FirstOrDefault();

            var menosVendido = _context.DetalleCompras
                .GroupBy(d => d.ProductoNombre)
                .OrderBy(g => g.Sum(d => d.Cantidad))
                .Select(g => new { Producto = g.Key, TotalCantidad = g.Sum(d => d.Cantidad) })
                .FirstOrDefault();

            return View(new { MasVendido = masVendido, MenosVendido = menosVendido });
        }

        public IActionResult CompararPopularidad(string productoNombre)
        {
            if (string.IsNullOrEmpty(productoNombre))
            {
                ViewBag.Productos = _context.Productos.ToList();
                return View();
            }

            var totalVendido = _context.DetalleCompras
                .GroupBy(d => d.ProductoNombre)
                .Select(g => new { Producto = g.Key, TotalCantidad = g.Sum(d => d.Cantidad) })
                .OrderByDescending(x => x.TotalCantidad)
                .ToList();

            if (!totalVendido.Any())
            {
                ViewBag.Productos = _context.Productos.ToList();
                ViewBag.Error = "No hay datos de ventas para comparar.";
                return View();
            }

            var masVendido = totalVendido.First();
            var menosVendido = totalVendido.Last();

            var productoSeleccionado = totalVendido.FirstOrDefault(p => p.Producto == productoNombre);

            if (productoSeleccionado == null)
            {
                ViewBag.Productos = _context.Productos.ToList();
                ViewBag.Error = $"El producto '{productoNombre}' no tiene ventas registradas.";
                return View();
            }

            decimal porcentajeVsMas = 0;
            decimal porcentajeVsMenos = 0;

            if (productoSeleccionado.Producto == masVendido.Producto)
            {
                porcentajeVsMenos = Math.Round(100 * ((productoSeleccionado.TotalCantidad - menosVendido.TotalCantidad) / (decimal)menosVendido.TotalCantidad), 2);
            }
            else if (productoSeleccionado.Producto == menosVendido.Producto)
            {
                porcentajeVsMas = Math.Round(100 * ((masVendido.TotalCantidad - productoSeleccionado.TotalCantidad) / (decimal)masVendido.TotalCantidad), 2);
            }
            else
            {
                porcentajeVsMas = Math.Round(100 * ((masVendido.TotalCantidad - productoSeleccionado.TotalCantidad) / (decimal)masVendido.TotalCantidad), 2);
                porcentajeVsMenos = Math.Round(100 * ((productoSeleccionado.TotalCantidad - menosVendido.TotalCantidad) / (decimal)menosVendido.TotalCantidad), 2);
            }

            var ranking = totalVendido.IndexOf(productoSeleccionado) + 1;

            ViewBag.Productos = _context.Productos.ToList();

            return View(new
            {
                ProductoSeleccionado = productoSeleccionado,
                MasVendido = masVendido,
                MenosVendido = menosVendido,
                PorcentajeVsMas = porcentajeVsMas,
                PorcentajeVsMenos = porcentajeVsMenos,
                Ranking = ranking,
                TotalProductos = totalVendido.Count
            });
        }

        public IActionResult ProductosPorRango(DateTime? fechaInicio, DateTime? fechaFin)
        {
            if (!fechaInicio.HasValue || !fechaFin.HasValue)
            {
                ViewBag.Error = "Por favor selecciona un rango de fechas.";
                return View();
            }

            var productos = _context.DetalleCompras
                .Where(d => d.Compra.FechaCompra >= fechaInicio && d.Compra.FechaCompra <= fechaFin)
                .GroupBy(d => d.ProductoNombre)
                .Select(g => new
                {
                    Producto = g.Key,
                    TotalCantidad = g.Sum(d => d.Cantidad)
                })
                .ToList();

            var masVendido = productos.OrderByDescending(p => p.TotalCantidad).FirstOrDefault();
            var menosVendido = productos.OrderBy(p => p.TotalCantidad).FirstOrDefault();

            if (masVendido == null || menosVendido == null)
            {
                ViewBag.Error = "No hay datos disponibles en el rango seleccionado.";
                return View();
            }

            var viewModel = new RangoFechasViewModel
            {
                FechaInicio = fechaInicio.Value,
                FechaFin = fechaFin.Value,
                MasVendido = masVendido,
                MenosVendido = menosVendido
            };

            return View(viewModel);
        }

    }
}
