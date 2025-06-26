namespace Tiendaenlinea.Dominio.Entidades;

public class Pedido
{
    public int PedidoID { get; set; }
    public int UsuarioID { get; set; }
    public int DireccionID { get; set; }
    public DateTime FechaPedido { get; set; }
    public string Estado { get; set; } = null!;

}
