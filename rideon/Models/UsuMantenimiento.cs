using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rideon.Models
{
    public class UsuMantenimiento
    {
        [Key]
        public int idMantenimientoUsuario { get; set; }

        [Required]
        public int idUsuario { get; set; }

        [ForeignKey(nameof(idUsuario))]
        [InverseProperty("UsuMantenimientos")]
        public UsuUsuario Usuario { get; set; }

        [Required]
        public int idMotoUsuario { get; set; }  

        [ForeignKey(nameof(idMotoUsuario))]
        [InverseProperty(nameof(UsuUsuarioMoto.Mantenimientos))]
        public UsuUsuarioMoto Moto { get; set; } 

        [Required]
        public int idCatMantenimiento { get; set; }

        [ForeignKey(nameof(idCatMantenimiento))]
        [InverseProperty("MantenimientosUsuario")]
        public CatMantenimiento CatMantenimiento { get; set; }

        [DisplayName("Fecha")]
        public DateTime dtFecha { get; set; } = DateTime.Now;

        [StringLength(255)]
        [DisplayName("Notas")]
        public string strNotas { get; set; }


    }
}
