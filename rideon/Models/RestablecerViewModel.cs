using System.ComponentModel.DataAnnotations;

namespace rideon.Models
{
    public class RestablecerViewModel
    {
        [Required]
        [EmailAddress]
        public string Correo { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(18, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 18 caracteres")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[^A-Za-z\d]).{6,18}$",
            ErrorMessage = "La contraseña debe contener al menos una letra, un número y un carácter especial")]
        [DataType(DataType.Password)]
        public string NuevaContrasena { get; set; }
    }
}
