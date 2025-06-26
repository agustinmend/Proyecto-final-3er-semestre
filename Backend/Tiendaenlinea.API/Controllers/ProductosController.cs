using Microsoft.AspNetCore.Mvc;
using Tiendaenlinea.Dominio.DTOs;
using Tiendaenlinea.API.DTOs;
using Tiendaenlinea.Dominio.Repositorios;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Tiendaenlinea.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly IProductoRepository _productoRepository;

        public ProductosController(IProductoRepository productoRepository)
        {
            _productoRepository = productoRepository;
        }

        [HttpPost("agregar-producto")]
        public async Task<IActionResult> AgregarProducto([FromForm] AgregarProductoRequest request)
        {
            try
            {
                string? imagenUrl = null;

                if (request.Imagen != null)
                {
                    var nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(request.Imagen.FileName);
                    var rutaCarpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagenes_productos");
                    Directory.CreateDirectory(rutaCarpeta);

                    var rutaArchivo = Path.Combine(rutaCarpeta, nombreArchivo);

                    using (var stream = new FileStream(rutaArchivo, FileMode.Create))
                    {
                        await request.Imagen.CopyToAsync(stream);
                    }

                    imagenUrl = $"{Request.Scheme}://{Request.Host}/imagenes_productos/{nombreArchivo}";
                }

                var productoDto = new ProductoDto
                {
                    NombreProducto = request.NombreProducto,
                    Descripcion = request.Descripcion,
                    Precio = request.Precio,
                    Stock = request.Stock,
                    CategoriaID = request.CategoriaID,
                    TiendaID = request.TiendaID,
                    Estado = request.Estado,
                    ImagenUrl = imagenUrl
                };
                Console.WriteLine($"🟡 TiendaID recibido: {request.TiendaID}");
                int productoID = _productoRepository.AgregarProducto(productoDto);

                if (imagenUrl != null)
                {
                    await GuardarImagenProducto(productoID, imagenUrl);
                }

                return Ok(new { mensaje = "Producto agregado correctamente", productoID });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    mensaje = "Error al agregar producto",
                    error = ex.Message,
                    detalles = ex.InnerException?.Message,
                    pila = ex.StackTrace
                });
            }
        }

        private async Task GuardarImagenProducto(int productoID, string urlImagen)
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("TiendaOnline");
            var coleccion = database.GetCollection<BsonDocument>("imagenesProductos");

            var documento = new BsonDocument
            {
                { "productoID", productoID },
                { "url", urlImagen }
            };

            await coleccion.InsertOneAsync(documento);
        }
        [HttpGet("listar")]
        public IActionResult ListarProductos()
        {
            var productos = _productoRepository.ListarProductos();
            return Ok(productos);
        }

        [HttpPost("actualizar-stock")]
        public IActionResult ActualizarStock([FromBody] ActualizarStockDto dto)
        {
            try
            {
                _productoRepository.ActualizarStock(dto.ProductoID, dto.Stock);
                return Ok(new { mensaje = "Stock actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPut("actualizar-estado")]
        public IActionResult ActualizarEstadoProducto([FromBody] ActualizarEstadoRequest dto)
        {
            try
            {
                _productoRepository.ActualizarEstado(dto.ProductoID, dto.Estado);
                return Ok(new { mensaje = "Estado actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

    }
}
