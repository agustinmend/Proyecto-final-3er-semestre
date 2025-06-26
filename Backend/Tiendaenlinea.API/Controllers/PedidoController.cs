using Microsoft.AspNetCore.Mvc;
using Tiendaenlinea.Dominio.DTOs;
using Tiendaenlinea.API.DTOs;
using Tiendaenlinea.Dominio.Repositorios;

namespace Tiendaenlinea.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoRepository _pedidoRepository;

        public PedidoController(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        [HttpGet("mis-pedidos/{usuarioId}")]
        public IActionResult ObtenerPedidos(int usuarioId)
        {
            try
            {
                var pedidos = _pedidoRepository.ObtenerPedidosPorUsuario(usuarioId);
                return Ok(pedidos);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("cambiar-estado")]
        public IActionResult CambiarEstadoPedido([FromBody] CambiarEstadoPedidoRequest dto)
        {
            try
            {
                _pedidoRepository.CambiarEstado(dto.PedidoID, dto.Estadonuevo);
                return Ok(new { mensaje = "Estado actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

    }
}
