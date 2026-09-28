using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace rideon.Models
{
    public class CatRecomendacionesMoto
    {
        [Key]
        public int idRecomendacion { get; set; }

        [Required(ErrorMessage = "La moto es obligatoria")]
        [DisplayName("Moto")]
        public int idMoto { get; set; } 

        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(150)]
        [DisplayName("Título de la recomendación")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [StringLength(500)]
        [DisplayName("Descripción")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El kilometraje recomendado es obligatorio")]
        [Range(0, int.MaxValue, ErrorMessage = "El kilometraje debe ser mayor o igual a 0")]
        [DisplayName("Kilometraje recomendado")]
        public int KmRecomendado { get; set; }

 
        [JsonIgnore]
        [ValidateNever]    
        public CatMoto Moto { get; set; }
    }
}
