using Microsoft.AspNetCore.Mvc;
using Tiendaenlinea.Dominio.DTOs;
using Tiendaenlinea.API.DTOs;
using Tiendaenlinea.Dominio.Repositorios;
using Tiendaenlinea.Dominio.Servicios;

namespace Tiendaenlinea.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DireccionController : ControllerBase
    {
        private readonly IDireccionService direccionService;

        public DireccionController(IDireccionService productoRepository)
        {
            direccionService = productoRepository;
        }

        [HttpPost("agregar-direccion")]
        public IActionResult AgregarDireccion([FromBody] AgregarDireccionDto dto)
        {
            try
            {
                var direccionId = direccionService.AgregarDireccion(dto);
                return Ok(new { mensaje = "Dirección agregada", DireccionID = direccionId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
