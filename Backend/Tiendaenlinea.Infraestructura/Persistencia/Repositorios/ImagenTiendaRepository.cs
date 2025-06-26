using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Tiendaenlinea.Dominio.Entidades;
using Tiendaenlinea.Dominio.Repositorios;
namespace Tiendaenlinea.Infraestructura.Persistencia.Repositorios
{
    public class ImagenTiendaRepository : IImagenTiendaRepository
    {
        private readonly IMongoCollection<ImagenTienda> _coleccion;

        public ImagenTiendaRepository(IConfiguration config)
        {
            var cliente = new MongoClient(config.GetConnectionString("MongoDB"));
            var db = cliente.GetDatabase("TiendaOnline");
            _coleccion = db.GetCollection<ImagenTienda>("imagenesTiendas");
        }

        public async Task<string?> ObtenerUrlPorTiendaIDAsync(int tiendaID)
        {
            var imagen = await _coleccion.Find(i => i.TiendaID == tiendaID).FirstOrDefaultAsync();
            return imagen?.Url;
        }

        public Dictionary<int, string> ObtenerTodas()
        {
            return _coleccion.Find(_ => true).ToList()
                             .ToDictionary(i => i.TiendaID, i => i.Url);
        }
    }
}