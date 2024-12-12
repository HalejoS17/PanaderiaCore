using System.ComponentModel.DataAnnotations;

namespace PanaderiaCore.Models
{
    public class Usuario
    {
        [Key]
        public string Email { get; set; }
        public string Nombre { get; set; }
        public string Rol { get; set; } // "Administrador" o "Usuario"
        public string Contraseña { get; set; }
    }

}
