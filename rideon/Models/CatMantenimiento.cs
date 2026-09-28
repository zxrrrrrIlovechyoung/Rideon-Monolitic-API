using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace rideon.Models
{
    public class CatMantenimiento
    {
        [Key]
        public int idCatMantenimiento { get; set; }

        [Required, StringLength(100)]
        [DisplayName("Nombre del Mantenimiento")]
        public string Nombre { get; set; }

        [Required, StringLength(100)]
        [DisplayName("Descripción del Mantenimiento")]
        public string strDescripcion { get; set; }  

        [DisplayName("Kilómetros Recomendados")]
        public int? KmRecomendado { get; set; }
        [Required]
        public int MotoCatalogoId { get; set; }

        [ForeignKey(nameof(MotoCatalogoId))]
        [DisplayName("Moto del Catálogo")]
        public CatMoto? Moto { get; set; }  
        [JsonIgnore]
        [InverseProperty("CatMantenimiento")]
        public ICollection<UsuMantenimiento> MantenimientosUsuario { get; set; } = new List<UsuMantenimiento>();
    }
}
