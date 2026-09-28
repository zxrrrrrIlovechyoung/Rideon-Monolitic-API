using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace rideon.Models
{
    public class CatTipoUsuario
    {
        [Key]
        public int idTipoUsuario { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [StringLength(50)]
        [DisplayName("Tipo de Usuario")]
        public string strDescripcion { get; set; }

        [JsonIgnore]
        public ICollection<UsuUsuario> Usuarios { get; set; } = new List<UsuUsuario>();
    }
}
