using System;
namespace Tiendaenlinea.Dominio.DTOs
{
    public class ProductoListadoDto
    {
        public int ProductoID { get; set; }
        public string NombreProducto { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public int TiendaID { get; set; }
        public int CategoriaID { get; set; }
        public string Estado { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public string? ImagenUrl { get; set; }
    }
}