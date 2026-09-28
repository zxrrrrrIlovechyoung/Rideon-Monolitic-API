using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace rideon.Models
{
    public class CatMarcaMoto
    {
        [Key]
        public int idMarca { get; set; }

        [DisplayName("Marca")]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [StringLength(50)]
        public string strMarca { get; set; }
        [JsonIgnore]
        public ICollection<CatMoto> Motos { get; set; } = new List<CatMoto>();
    }
}
