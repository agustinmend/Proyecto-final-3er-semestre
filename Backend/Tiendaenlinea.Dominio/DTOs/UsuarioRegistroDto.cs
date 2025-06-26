namespace Tiendaenlinea.Dominio.DTOs
{
    public class UsuarioRegistroDto
    {
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Contrasena { get; set; } = null!;
    }
}
