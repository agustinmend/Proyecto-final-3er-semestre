namespace Tiendaenlinea.Dominio.Entidades;

public class Direccion
{
    public int DireccionesID { get; set; }
    public int UsuarioID { get; set; }
    public string Pais { get; set; } = null!;
    public string Ciudad { get; set; } = null!;
    public string CodigoPostal { get; set; } = null!;
    public string DireccionDetalle { get; set; } = null!;
}
