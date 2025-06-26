using System.Data;
using System.Data.SqlClient;
using Tiendaenlinea.Dominio.DTOs;
using Tiendaenlinea.Dominio.Repositorios;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Tiendaenlinea.Infraestructura.Persistencia.Repositorios
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly string _connectionString;

        public ProductoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int AgregarProducto(ProductoDto dto)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("agregarproducto", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@TiendaID", dto.TiendaID);
            command.Parameters.AddWithValue("@CategoriaID", dto.CategoriaID);
            command.Parameters.AddWithValue("@NombreProducto", dto.NombreProducto);
            command.Parameters.AddWithValue("@Descripcion", dto.Descripcion);
            command.Parameters.AddWithValue("@Precio", dto.Precio);
            command.Parameters.AddWithValue("@Stock", dto.Stock);
            command.Parameters.AddWithValue("@Estado", dto.Estado);

            connection.Open();
            var resultado = command.ExecuteScalar();
            return Convert.ToInt32(resultado);
        }
        public List<ProductoListadoDto> ListarProductos()
        {
            var productos = new List<ProductoListadoDto>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("SELECT * FROM Productos", connection);

            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var producto = new ProductoListadoDto
                {
                    ProductoID = Convert.ToInt32(reader["ProductoID"]),
                    TiendaID = Convert.ToInt32(reader["TiendaID"]),
                    CategoriaID = Convert.ToInt32(reader["CategoriaID"]),
                    NombreProducto = reader["NombreProducto"].ToString()!,
                    Descripcion = reader["Descripcion"].ToString()!,
                    Precio = Convert.ToDecimal(reader["Precio"]),
                    Stock = Convert.ToInt32(reader["Stock"]),
                    FechaPublicacion = Convert.ToDateTime(reader["FechaPublicacion"]),
                    Estado = reader["Estado"].ToString()!,
                    ImagenUrl = null
                };
                productos.Add(producto);
            }

            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("TiendaOnline");
            var coleccion = database.GetCollection<BsonDocument>("imagenesProductos");

            foreach (var producto in productos)
            {
                var filtro = Builders<BsonDocument>.Filter.Eq("productoID", producto.ProductoID);
                var doc = coleccion.Find(filtro).FirstOrDefault();
                if (doc != null && doc.Contains("url"))
                {
                    producto.ImagenUrl = doc["url"].AsString;
                }
            }

            return productos;
        }

        public ProductoListadoDto? ObtenerPorID(int productoID)
        {
            ProductoListadoDto? producto = null;

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("SELECT * FROM Productos WHERE ProductoID = @ProductoID", connection);
            command.Parameters.AddWithValue("@ProductoID", productoID);

            connection.Open();
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                producto = new ProductoListadoDto
                {
                    ProductoID = Convert.ToInt32(reader["ProductoID"]),
                    TiendaID = Convert.ToInt32(reader["TiendaID"]),
                    CategoriaID = Convert.ToInt32(reader["CategoriaID"]),
                    NombreProducto = reader["NombreProducto"].ToString()!,
                    Descripcion = reader["Descripcion"].ToString()!,
                    Precio = Convert.ToDecimal(reader["Precio"]),
                    Stock = Convert.ToInt32(reader["Stock"]),
                    FechaPublicacion = Convert.ToDateTime(reader["FechaPublicacion"]),
                    Estado = reader["Estado"].ToString()!,
                    ImagenUrl = null
                };
            }

            if (producto != null)
            {
                var client = new MongoClient("mongodb://localhost:27017");
                var database = client.GetDatabase("TiendaOnline");
                var coleccion = database.GetCollection<BsonDocument>("imagenesProductos");

                var filtro = Builders<BsonDocument>.Filter.Eq("productoID", producto.ProductoID);
                var doc = coleccion.Find(filtro).FirstOrDefault();
                if (doc != null && doc.Contains("url"))
                {
                    producto.ImagenUrl = doc["url"].AsString;
                }
            }

            return producto;
        }

        public void ActualizarStock(int productoID, int stock)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("actualizarstockproducto", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProductoID", productoID);
                    command.Parameters.AddWithValue("@Stock", stock);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void ActualizarEstado(int productoID, string estado)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("inactivarproducto", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ProductoID", productoID);
                command.Parameters.AddWithValue("@Estado", estado);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

    }
}
