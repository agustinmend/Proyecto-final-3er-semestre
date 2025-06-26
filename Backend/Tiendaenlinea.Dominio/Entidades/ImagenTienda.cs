using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Tiendaenlinea.Dominio.Entidades
{
    public class ImagenTienda
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("tiendaID")]
        public int TiendaID { get; set; }

        [BsonElement("url")]
        public string Url { get; set; } = string.Empty;
    }
}