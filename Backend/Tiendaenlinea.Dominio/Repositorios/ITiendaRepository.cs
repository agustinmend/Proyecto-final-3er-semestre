using Tiendaenlinea.Dominio.Entidades;
using Tiendaenlinea.Dominio.DTOs;

namespace Tiendaenlinea.Dominio.Repositorios
{
    public interface ITiendaRepository
    {
        public int CrearTienda(CrearTiendaDto dto);
        public Task<Tienda?> ObtenerPorIDAsync(int tiendaID);
        List<Tienda> ObtenerTodasLasTiendas();
        List<ProductoListadoDto> ObtenerProductosPorTienda(int tiendaID);
        string ObtenerNombreUsuario(int usuarioId);
    }
}