using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Tiendaenlinea.Dominio.DTOs;
using Tiendaenlinea.Dominio.Entidades;
using Tiendaenlinea.Dominio.Repositorios;
using Tiendaenlinea.Aplicacion.Servicios;
using Tiendaenlinea.API.DTOs;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Tiendaenlinea.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TiendasController : ControllerBase
    {
        private readonly ITiendaRepository TiendaRepository;
        private readonly TiendaService _tiendaService;
        private readonly IImagenTiendaRepository _imagenTiendaRepository;

        public TiendasController(ITiendaRepository tiendasRepository , TiendaService tiendaService, IImagenTiendaRepository imagenTiendaRepository)
        {
            TiendaRepository = tiendasRepository;
            _tiendaService = tiendaService;
            _imagenTiendaRepository = imagenTiendaRepository;
        }

        [HttpPost("crear-tienda")]
        public async Task<IActionResult> CrearTienda([FromForm] CrearTiendaRequest request)
        {
            try
            {
                string? imagenUrl = null;

                if (request.Imagen != null)
                {
                    var nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(request.Imagen.FileName);
                    var rutaCarpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagenes");
                    Directory.CreateDirectory(rutaCarpeta);

                    var rutaArchivo = Path.Combine(rutaCarpeta, nombreArchivo);

                    using (var stream = new FileStream(rutaArchivo, FileMode.Create))
                    {
                        await request.Imagen.CopyToAsync(stream);
                    }

                    imagenUrl = $"{Request.Scheme}://{Request.Host}/imagenes/{nombreArchivo}";
                }

                var dto = new CrearTiendaDto
                {
                    NombreTienda = request.Nombre,
                    Descripcion = request.Descripcion,
                    UsuarioID = request.UsuarioID
                };

                int tiendaID = TiendaRepository.CrearTienda(dto);

                if (imagenUrl != null)
                {
                    await GuardarImagenTienda(tiendaID, imagenUrl);
                }

                return Ok(new { mensaje = "Tienda creada correctamente", tiendaID });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = "Error al crear tienda", error = ex.Message });
            }
        }
        private async Task GuardarImagenTienda(int tiendaID, string urlImagen)
        {
            var client = new MongoDB.Driver.MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("TiendaOnline");
            var coleccion = database.GetCollection<MongoDB.Bson.BsonDocument>("imagenesTiendas");

            var documento = new MongoDB.Bson.BsonDocument
            {
                {"tiendaID", tiendaID },
                {"url", urlImagen }
            };

            await coleccion.InsertOneAsync(documento);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerTiendaPorID(int id)
        {
            var tienda = await _tiendaService.ObtenerTiendaPorIDAsync(id);
            if (tienda == null)
                return NotFound(new { mensaje = "Tienda no encontrada." });

            return Ok(tienda);
        }

        [HttpGet("todas")]
        public IActionResult ObtenerTiendas()
        {
            var tiendas = TiendaRepository.ObtenerTodasLasTiendas();
            var imagenes = _imagenTiendaRepository.ObtenerTodas();

            var resultado = tiendas.Select(t => new
            {
                t.TiendaID,
                t.NombreTienda,
                t.Descripcion,
                t.FechaCreacion,
                ImagenUrl = imagenes.ContainsKey(t.TiendaID) ? imagenes[t.TiendaID] : null
            });

            return Ok(resultado);
        }

        [HttpGet("{tiendaID}/productos")]
        public IActionResult ObtenerProductosPorTienda(int tiendaID)
        {
            var productos = TiendaRepository.ObtenerProductosPorTienda(tiendaID);
            return Ok(productos);
        }

        [HttpGet("{tiendaID}/valoraciones")]
        public async Task<IActionResult> ObtenerValoracionesDeTienda(int tiendaID)
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("TiendaOnline");
            var coleccion = database.GetCollection<BsonDocument>("valoracionesTiendas");

            var filtro = Builders<BsonDocument>.Filter.Eq("tiendaID", tiendaID);
            var valoraciones = await coleccion.Find(filtro).ToListAsync();

            var lista = valoraciones.Select(doc => new {
                usuario = doc["usuario"].AsString,
                mensaje = doc["mensaje"].AsString,
                puntuacion = doc["puntuacion"].AsInt32
            });

            return Ok(lista);
        }

        [HttpPost("{tiendaID}/valoracion")]
        public async Task<IActionResult> AgregarValoracion(int tiendaID, [FromBody] ValoracionRequest request)
        {
            try
            {
                string nombreUsuario = TiendaRepository.ObtenerNombreUsuario(request.UsuarioID);

                var client = new MongoClient("mongodb://localhost:27017");
                var database = client.GetDatabase("TiendaOnline");
                var coleccion = database.GetCollection<BsonDocument>("valoracionesTiendas");

                var doc = new BsonDocument
                {
                    { "tiendaID", tiendaID },
                    { "usuario", nombreUsuario },
                    { "mensaje", request.Mensaje },
                    { "puntuacion", request.Puntuacion }
                };

                await coleccion.InsertOneAsync(doc);
                return Ok(new { mensaje = "Valoración registrada" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

    }
}
