using System.ComponentModel.DataAnnotations;

namespace PanaderiaCore.Models
{
    public class Reabastecimiento
    {
        public int Id { get; set; }
        [StringLength(100)]
        public string ProductoNombre { get; set; }
        public int Cantidad { get; set; }
        public DateTime Fecha { get; set; }

        public Producto Producto { get; set; }
    }
}