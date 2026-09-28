using System.ComponentModel.DataAnnotations;

namespace rideon.Models
{
    public class RegistroViewModel
    {
        [Required(ErrorMessage = "Esta información es obligatoria")]
        [StringLength(100)]
        public string strNombre { get; set; }

        [StringLength(100)]
        public string strApellidoPaterno { get; set; }

        [StringLength(100)]
        public string strApellidoMaterno { get; set; }

        [Required(ErrorMessage = "Esta información es obligatoria")]
        [EmailAddress(ErrorMessage = "Debe ingresar un correo válido")]
        public string strCorreo { get; set; }

        [Required(ErrorMessage = "Esta información es obligatoria")]
        [StringLength(18, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 18 caracteres")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[^A-Za-z\d]).{6,18}$",
            ErrorMessage = "La contraseña debe contener al menos una letra, un número y un carácter especial")]
        public string strContrasena { get; set; }

        [Compare("strContrasena", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmarContrasena { get; set; }

        [Required(ErrorMessage = "Esta información es obligatoria")]
        public int idSexo { get; set; }

        public DateTime? FechaNacimiento { get; set; }
    }

}
