namespace PanaderiaCore.Models
{
    using PanaderiaCore.Models;
    using System.ComponentModel.DataAnnotations;

    public class Producto
    {
        public Producto()
        {
            FechaCreacion = DateTime.Now;
        }

        [Key]
        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que 0.")]
        [DataType(DataType.Currency)]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria.")]
        public string Categoria { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public string Descripcion { get; set; }

        public DateTime FechaCreacion { get; set; }

        public Inventario? Inventario { get; set; }
        public ICollection<DetalleCompra> Detalles { get; set; } = new List<DetalleCompra>();

        public ICollection<Reabastecimiento> Reabastecimientos { get; set; } = new List<Reabastecimiento>(); // Nuevo

    }
}
