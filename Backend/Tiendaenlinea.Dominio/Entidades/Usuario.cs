namespace Tiendaenlinea.Dominio.Entidades;

public class Usuario
{
    public int UsuarioID { get; set; }
    public string Nombre { get; set; } = null!;
    public string Apellido { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Contrasena { get; set; } = null!;
    public DateTime FechaRegistro { get; set; }
}
