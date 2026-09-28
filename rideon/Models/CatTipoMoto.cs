using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace rideon.Models
{
    public class CatTipoMoto
    {
        [Key]
        public int idTipoMoto { get; set; }

        [DisplayName("Tipo de Moto")]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [StringLength(50)]
        public string strTipoMoto { get; set; }
        [JsonIgnore]
        public ICollection<CatMoto> Motos { get; set; } = new List<CatMoto>();
    }
}
