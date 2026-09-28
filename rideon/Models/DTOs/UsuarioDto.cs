namespace rideon.Models.DTOs
{
    public class UsuarioDto
    {
        public int idUsuario { get; set; }
        public string strNombre { get; set; }
        public string strApellidoPaterno { get; set; }
        public string strApellidoMaterno { get; set; }
        public string strCorreo { get; set; }
        public string strTelefono { get; set; }
        public int idSexo { get; set; }
        public int idTipoUsuario { get; set; }
        public DateTime dtFechaDeRegistro { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string? strFotoPerfil { get; set; }
    }

}
