using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace rideon.Models
{
    public class UsuUsuario
    {
        [Key]
        public int idUsuario { get; set; }

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

        [StringLength(20, ErrorMessage = "El teléfono no puede tener más de 20 caracteres.")]
        public string? strTelefono { get; set; }

        // 🔗 Relaciones
        [Required(ErrorMessage = "Esta información es obligatoria")]
        public int idSexo { get; set; }

        [ForeignKey("idSexo")]
        [ValidateNever] 
        public UsuCatSexo UsuCatSexo { get; set; }

        [Required(ErrorMessage = "Esta información es obligatoria")]
        public int idTipoUsuario { get; set; }

        [ForeignKey("idTipoUsuario")]
        [ValidateNever]
        public CatTipoUsuario CatTipoUsuario { get; set; }

        [StringLength(18, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 18 caracteres")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[^A-Za-z\d]).{6,18}$",
    ErrorMessage = "La contraseña debe contener al menos una letra, un número y un carácter especial")]
        public string? strContrasena { get; set; }


        public bool activo { get; set; }
        public bool UsuValidacion { get; set; }

        [Required(ErrorMessage = "Esta información es obligatoria")]
        public DateTime dtFechaDeRegistro { get; set; }

        public DateTime? dtUltimoAcceso { get; set; }
        public DateTime? dtUltimaModificacion { get; set; }
        public DateTime? FechaNacimiento { get; set; }

        public string? strTokenRecuperacion { get; set; }
        public DateTime? dtExpToken { get; set; }

        public string? strFotoPerfil { get; set; }

        [ValidateNever]
        public ICollection<UsuUsuarioMoto> UsuUsuarioMotos { get; set; }

        [ValidateNever]
        public ICollection<UsuMantenimiento> UsuMantenimientos { get; set; }

        [NotMapped]
        public int[] SelectedMotos { get; set; }

        public UsuUsuario()
        {
            UsuUsuarioMotos = new List<UsuUsuarioMoto>();
            UsuMantenimientos = new List<UsuMantenimiento>();
            SelectedMotos = Array.Empty<int>();
        }
    }
}
