namespace Tiendaenlinea.API.DTOs
{
    public class ValoracionRequest
    {
        public int UsuarioID { get; set; }
        public string Mensaje { get; set; } = "";
        public int Puntuacion { get; set; }
    }
}
