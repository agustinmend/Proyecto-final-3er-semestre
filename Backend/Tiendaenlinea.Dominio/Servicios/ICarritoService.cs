using Tiendaenlinea.Dominio.DTOs;

namespace Tiendaenlinea.Dominio.Servicios
{
    public interface ICarritoService
    {
        void AgregarProducto(AgregarAlCarritoDto producto);
        List<ProductoEnCarritoDto> ObtenerCarrito(int usuarioId);
        void VaciarCarrito(int usuarioId);
        int ConfirmarCompra(ConfirmarCompraDto dto);
    }
}