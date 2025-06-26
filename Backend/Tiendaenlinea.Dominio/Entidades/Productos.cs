namespace Tiendaenlinea.Dominio.Entidades;

public class Producto
{
    public int ProductoID { get; set; }
    public int TiendaID { get; set; }
    public int CategoriaID { get; set; }
    public string NombreProducto { get; set; } = null!;
    public string Descripcion { get; set; } = null!;
    public int Stock { get; set; }
    public string Estado { get; set; } = null!;
    public DateTime FechaPublicacion { get; set; }
}
