namespace Tiendaenlinea.Dominio.DTOs
{
    public class CrearTiendaDto
    {
        public int UsuarioID { get; set; }
        public string NombreTienda { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
    }
}