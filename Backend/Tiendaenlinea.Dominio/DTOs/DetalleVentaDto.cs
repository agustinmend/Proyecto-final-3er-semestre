namespace Tiendaenlinea.Dominio.DTOs
{
    public class DetalleVentaDto
    {
        public int NroFactura { get; set; }
        public DateTime FechaPedido { get; set; }
        public string EstadoPedido { get; set; } = "";
        public string DireccionEnvio { get; set; } = "";
        public int ProductoID { get; set; }
        public string NombreProducto { get; set; } = "";
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
        public string Tienda { get; set; } = "";
    }
}