using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace rideon.Models
{
    public class UsuCatSexo
    {
        [Key]
        public int idSexo { get; set; }

        [Required(ErrorMessage = "La descripción del sexo es obligatoria")]
        [StringLength(50)]
        [DisplayName("Sexo")]
        public string strDescripcion { get; set; }
        [JsonIgnore]
        public ICollection<UsuUsuario>? Usuarios { get; set; } = new List<UsuUsuario>();
    }
}
