namespace rideon.Models.DTOs
{
    public class RegisterDto
    {
        public string strNombre { get; set; }
        public string strApellidoPaterno { get; set; }
        public string strApellidoMaterno { get; set; }
        public string strCorreo { get; set; }
        public string strTelefono { get; set; }
        public int idSexo { get; set; }
        public int idTipoUsuario { get; set; }
        public string strContrasena { get; set; }
        public DateTime? FechaNacimiento { get; set; }
    }

}
