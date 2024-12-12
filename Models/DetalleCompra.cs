using System.ComponentModel.DataAnnotations;

namespace PanaderiaCore.Models
{
    public class DetalleCompra
    {
        [Key]
        public int IdDetalle { get; set; }

        public int IdFactura { get; set; }
        public string ProductoNombre { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }

        // Relación con Compra
        public Compra Compra { get; set; }

        // Relación con Producto
        public Producto Producto { get; set; }
    }
}
