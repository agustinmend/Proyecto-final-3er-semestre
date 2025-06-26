using Tiendaenlinea.Dominio.Entidades;
using Tiendaenlinea.Dominio.DTOs;
public interface IImagenTiendaRepository
{
    Task<string?> ObtenerUrlPorTiendaIDAsync(int tiendaID);
    Dictionary<int, string> ObtenerTodas();
}