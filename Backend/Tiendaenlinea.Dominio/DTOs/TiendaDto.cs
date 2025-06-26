namespace Tiendaenlinea.Dominio.DTOs
{
    public class TiendaDto
    {
        public int UsuarioID { get; set; }
        public int TiendaID { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string? ImagenUrl { get; set; }
    }
}