using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace rideon.Models.ViewModels
{
    public class LoginViewModel
    {
        [DisplayName("Correo")]
        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "El correo no es válido")]
        public string Correo { get; set; }

        [DisplayName("Contraseña")]
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Contrasena { get; set; }
    }
}
