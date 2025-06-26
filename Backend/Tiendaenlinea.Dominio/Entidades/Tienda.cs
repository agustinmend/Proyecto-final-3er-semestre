namespace Tiendaenlinea.Dominio.Entidades;

public class Tienda
{
    public int TiendaID { get; set; }
    public int UsuarioID { get; set; }
    public string NombreTienda { get; set; } = null!;
    public string Descripcion { get; set; } = null!;
    public DateTime FechaCreacion { get; set; }
}
