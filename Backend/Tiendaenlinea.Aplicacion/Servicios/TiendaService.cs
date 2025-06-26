using Tiendaenlinea.Dominio.DTOs;
using Tiendaenlinea.Dominio.Repositorios;
namespace Tiendaenlinea.Aplicacion.Servicios
{
    public class TiendaService
    {
        private readonly ITiendaRepository TiendaRepository;
        private readonly IImagenTiendaRepository ImagenTiendaRepository;

        public TiendaService(
            ITiendaRepository tiendaRepository,
            IImagenTiendaRepository imagenTiendaRepository)
        {
            TiendaRepository = tiendaRepository;
            ImagenTiendaRepository = imagenTiendaRepository;
        }

        public async Task<TiendaDto?> ObtenerTiendaPorIDAsync(int tiendaID)
        {
            var tienda = await TiendaRepository.ObtenerPorIDAsync(tiendaID);
            if (tienda == null) return null;

            var imagenUrl = await ImagenTiendaRepository.ObtenerUrlPorTiendaIDAsync(tiendaID);

            return new TiendaDto
            {
                UsuarioID = tienda.UsuarioID,
                TiendaID = tienda.TiendaID,
                Nombre = tienda.NombreTienda,
                Descripcion = tienda.Descripcion,
                ImagenUrl = imagenUrl
            };
        }
    }
}