namespace Tiendaenlinea.API.DTOs
{
    public class ActualizarEstadoRequest
    {
        public int ProductoID { get; set; }
        public string Estado { get; set; } = string.Empty;
    }
}
