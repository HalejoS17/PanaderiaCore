using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PanaderiaCore.Models
{
    public class Compra
    {
        [Key]
        public int IdFactura { get; set; }

        public string ClienteEmail { get; set; }
        public DateTime FechaCompra { get; set; }
        public decimal Total { get; set; }

        public ICollection<DetalleCompra> Detalles { get; set; }
    }
}
