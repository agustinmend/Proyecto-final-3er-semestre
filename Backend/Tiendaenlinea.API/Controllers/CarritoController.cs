using Microsoft.AspNetCore.Mvc;
using Tiendaenlinea.Dominio.DTOs;
using Tiendaenlinea.Dominio.Servicios;

namespace Tiendaenlinea.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarritoController : ControllerBase
    {
        private readonly ICarritoService carritoService;

        public CarritoController(ICarritoService carritoService)
        {
            this.carritoService = carritoService;
        }

        [HttpPost("agregar")]
        public IActionResult AgregarAlCarrito([FromBody] AgregarAlCarritoDto dto)
        {
            carritoService.AgregarProducto(dto);
            return Ok(new { mensaje = "Producto agregado al carrito" });
        }

        [HttpGet("usuario/{usuarioId}")]
        public IActionResult ObtenerCarrito(int usuarioId)
        {
            var productos = carritoService.ObtenerCarrito(usuarioId);
            return Ok(productos);
        }

        [HttpPost("vaciar/{usuarioId}")]
        public IActionResult VaciarCarrito(int usuarioId)
        {
            carritoService.VaciarCarrito(usuarioId);
            return Ok(new { mensaje = "Carrito vaciado" });
        }

        [HttpPost("confirmar-compra")]
        public IActionResult ConfirmarCompra([FromBody] ConfirmarCompraDto dto)
        {
            try
            {
                var pedidoId = carritoService.ConfirmarCompra(dto);
                return Ok(new { mensaje = "Compra confirmada", pedidoId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

    }
}
