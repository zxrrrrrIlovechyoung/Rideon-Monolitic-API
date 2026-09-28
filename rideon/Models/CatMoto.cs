using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace rideon.Models
{
    public class CatMoto
    {
        [Key]
        public int idMoto { get; set; }

        [DisplayName("Marca")]
        public int idMarca { get; set; }

        [ForeignKey(nameof(idMarca))]
        [ValidateNever]      // 👈 EVITA QUE SE VALIDE
        public CatMarcaMoto CatMarcaMoto { get; set; }

        [DisplayName("Tipo de Moto")]
        public int idTipoMoto { get; set; }

        [ForeignKey(nameof(idTipoMoto))]
        [ValidateNever]      // 👈 EVITA QUE SE VALIDE
        public CatTipoMoto CatTipoMoto { get; set; }

        // ========== Campos normales ==========
        [DisplayName("Modelo")]
        [Required(ErrorMessage = "Esta información es obligatoria")]
        [StringLength(50)]
        public string strModelo { get; set; }

        [DisplayName("Cilindraje")]
        [Required(ErrorMessage = "Esta información es obligatoria")]
        [StringLength(20)]
        public string strCilindraje { get; set; }

        [DisplayName("Foto")]
        public string imgFoto { get; set; }

        [DisplayName("Descripción")]
        [Required(ErrorMessage = "Esta información es obligatoria")]
        [StringLength(255)]
        public string strDescripcion { get; set; }

        [DisplayName("HP")]
        [Required(ErrorMessage = "Esta información es obligatoria")]
        [StringLength(50)]
        public string strHp { get; set; }

        [DisplayName("Nm")]
        [Required(ErrorMessage = "Esta información es obligatoria")]
        [StringLength(50)]
        public string strNm { get; set; }
        [NotMapped]
        public IFormFile foto { get; set; }

        [JsonIgnore]
        public ICollection<UsuUsuarioMoto> UsuUsuarioMotos { get; set; } = new List<UsuUsuarioMoto>();
        [JsonIgnore]
        public ICollection<CatRecomendacionesMoto> Recomendaciones { get; set; } = new List<CatRecomendacionesMoto>();
    }

}

