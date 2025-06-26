using Tiendaenlinea.Dominio.DTOs;
namespace Tiendaenlinea.Dominio.Repositorios
{
    public interface IPedidoRepository
    {
        public List<DetalleVentaDto> ObtenerPedidosPorUsuario(int usuarioID);
        void CambiarEstado(int pedidoID, string estadoNuevo);
    }
}