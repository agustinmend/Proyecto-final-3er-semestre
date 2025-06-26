using Microsoft.AspNetCore.Mvc;
using Tiendaenlinea.Dominio.DTOs;
using Tiendaenlinea.Dominio.Entidades;
using Tiendaenlinea.Dominio.Repositorios;

namespace Tiendaenlinea.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UsuariosController : ControllerBase
	{
		private readonly IUsuarioRepository UsuarioRepository;

		public UsuariosController(IUsuarioRepository usuariosRepository)
		{
			UsuarioRepository = usuariosRepository;
		}

		[HttpPost("registrar")]
		public IActionResult Registrar([FromBody] UsuarioRegistroDto dto)
		{
			try
			{
				var nuevoUsuario = new Usuario
				{
					Nombre = dto.Nombre,
					Apellido = dto.Apellido,
					Email = dto.Email,
					Contrasena = dto.Contrasena
				};

				int usuarioID = UsuarioRepository.RegistrarUsuario(nuevoUsuario);
                Console.WriteLine("UsuarioID generado: " + usuarioID);
                return Ok(new
				{
					mensaje = "Usuario registrado correctamente",
					usuarioID = usuarioID
				});
			}
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

		[HttpPost("iniciar-sesion")]
		public IActionResult IniciarSesion([FromBody] UsuarioLoginDto dto)
		{
			var usuario = UsuarioRepository.ObtenerPorEmail(dto.Email);
			if (usuario == null || usuario.Contrasena != dto.Contrasena)
			{
				return Unauthorized(new { mensaje = "Este usuario u contrasena no nos suenan" });
			}
			return Ok(usuario);

		}
	}
}
