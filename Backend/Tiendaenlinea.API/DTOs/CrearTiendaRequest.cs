using Microsoft.AspNetCore.Http;

namespace Tiendaenlinea.API.DTOs
{
    public class CrearTiendaRequest
    {
        public string Nombre { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public int UsuarioID { get; set; }
        public IFormFile? Imagen { get; set; }
    }
}
