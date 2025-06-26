using Tiendaenlinea.Dominio.DTOs;
namespace Tiendaenlinea.Dominio.Repositorios
{
    public interface IProductoRepository
    {
        int AgregarProducto(ProductoDto dto);
        List<ProductoListadoDto> ListarProductos();
        ProductoListadoDto? ObtenerPorID(int productoID);
        void ActualizarStock(int productoID, int stock);
        void ActualizarEstado(int productoID, string estado);
    }
}