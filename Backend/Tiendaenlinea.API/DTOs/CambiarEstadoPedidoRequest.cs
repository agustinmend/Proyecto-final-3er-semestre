using System;
namespace Tiendaenlinea.API.DTOs
{
    public class CambiarEstadoPedidoRequest
    {
        public int PedidoID { get; set; }
        public string Estadonuevo { get; set; } = string.Empty;
    }
}
