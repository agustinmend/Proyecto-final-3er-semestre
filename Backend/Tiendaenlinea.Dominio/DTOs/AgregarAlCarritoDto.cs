namespace Tiendaenlinea.Dominio.DTOs
{
    public class AgregarAlCarritoDto
    {
        public int UsuarioID { get; set; }
        public int ProductoID { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}

