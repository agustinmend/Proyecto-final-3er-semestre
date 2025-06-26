namespace Tiendaenlinea.Dominio.Entidades;

public class DetallePedido
{
    public int DetallePedidoID { get; set; }
    public int PedidoID { get; set; }
    public int ProductoID { get; set; }
    public int Cantidad { get; set; }
    public int PrecioUnitario { get; set; }
    
}
