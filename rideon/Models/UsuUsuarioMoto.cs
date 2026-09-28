using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rideon.Models
{
    public class UsuUsuarioMoto
    {
        [Key]
        public int idUsuarioMoto { get; set; } 

        public int idUsuario { get; set; }

        [ForeignKey(nameof(idUsuario))]
        public UsuUsuario UsuUsuario { get; set; }
        public int? idMoto { get; set; }

        [ForeignKey(nameof(idMoto))]
        [InverseProperty("UsuUsuarioMotos")]
        public CatMoto? CatMoto { get; set; }
        [InverseProperty(nameof(UsuMantenimiento.Moto))]
        public ICollection<UsuMantenimiento> Mantenimientos { get; set; } = new List<UsuMantenimiento>();
        [Display(Name = "Placas")]
        [StringLength(10)]
        public string? strPlacas { get; set; }

        [Display(Name = "ODO total")]
        public int? odoTotal { get; set; }

        [Display(Name = "Último servicio")]
        public DateTime? fechaUltimoServicio { get; set; }
    }
}
