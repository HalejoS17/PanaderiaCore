using System.ComponentModel.DataAnnotations;

namespace PanaderiaCore.Models
{
    public class Inventario
    {
        [Key]
        public string ProductoNombre { get; set; }

        [Required(ErrorMessage = "La cantidad disponible es obligatoria.")]
        [Range(0, int.MaxValue, ErrorMessage = "La cantidad disponible debe ser mayor o igual a 0.")]
        public int CantidadDisponible { get; set; }

        public DateTime UltimaActualizacion { get; set; }
        public Producto? Producto { get; set; }
    }
}
