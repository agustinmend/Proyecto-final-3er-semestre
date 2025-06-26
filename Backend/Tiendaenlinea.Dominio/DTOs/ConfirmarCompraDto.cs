namespace Tiendaenlinea.Dominio.DTOs
{
    public class ConfirmarCompraDto
    {
        public int UsuarioID { get; set; }
        public int DireccionID { get; set; }
        public List<ItemCarritoDto> Carrito { get; set; } = new();
    }
}
