using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Tiendaenlinea.Dominio.Entidades;
using Tiendaenlinea.Dominio.Repositorios;
using Tiendaenlinea.Dominio.DTOs;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Bson;


namespace Tiendaenlinea.Infraestructura.Persistencia.Repositorios
{
	public class TiendaRepository : ITiendaRepository
	{
        private readonly string ConnectionString;
        public TiendaRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public int CrearTienda(CrearTiendaDto dto)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("CrearTienda", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@NombreTienda", dto.NombreTienda);
                    command.Parameters.AddWithValue("@Descripcion", dto.Descripcion);
                    command.Parameters.AddWithValue("@UsuarioID", dto.UsuarioID);

                    var result = command.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
        }
        public async Task<Tienda?> ObtenerPorIDAsync(int tiendaID)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();

                var query = @"SELECT TiendaID, UsuarioID, NombreTienda, Descripcion, FechaCreacion 
                      FROM Tiendas 
                      WHERE TiendaID = @TiendaID";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TiendaID", tiendaID);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Tienda
                            {
                                TiendaID = reader.GetInt32(0),
                                UsuarioID = reader.GetInt32(1),
                                NombreTienda = reader.GetString(2),
                                Descripcion = reader.GetString(3),
                                FechaCreacion = reader.GetDateTime(4)
                            };
                        }
                    }
                }
            }

            return null;
        }

        public List<Tienda> ObtenerTodasLasTiendas()
        {
            var tiendas = new List<Tienda>();
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();

            using var command = new SqlCommand("SELECT TiendaID, NombreTienda, Descripcion, FechaCreacion FROM Tiendas", connection);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                tiendas.Add(new Tienda
                {
                    TiendaID = reader.GetInt32(0),
                    NombreTienda = reader.GetString(1),
                    Descripcion = reader.GetString(2),
                    FechaCreacion = reader.GetDateTime(3)
                });
            }

            return tiendas;
        }
        public List<ProductoListadoDto> ObtenerProductosPorTienda(int tiendaID)
        {
            var productos = new List<ProductoListadoDto>();

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT ProductoID, NombreProducto, Descripcion, Precio, Stock, CategoriaID, Estado FROM Productos WHERE TiendaID = @TiendaID", connection))
                {
                    command.Parameters.AddWithValue("@TiendaID", tiendaID);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var producto = new ProductoListadoDto
                            {
                                ProductoID = reader.GetInt32(reader.GetOrdinal("ProductoID")),
                                NombreProducto = reader.GetString(reader.GetOrdinal("NombreProducto")),
                                Descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                                Precio = reader.GetDecimal(reader.GetOrdinal("Precio")),
                                Stock = reader.GetInt32(reader.GetOrdinal("Stock")),
                                CategoriaID = reader.GetInt32(reader.GetOrdinal("CategoriaID")),
                                Estado = reader.GetString(reader.GetOrdinal("Estado")),
                                ImagenUrl = null
                            };

                            productos.Add(producto);
                        }
                    }
                }
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

        public string ObtenerNombreUsuario(int usuarioId)
        {
            using var connection = new SqlConnection(ConnectionString);
            using var command = new SqlCommand("SELECT Nombre FROM Usuarios WHERE UsuarioID = @id", connection);
            command.Parameters.AddWithValue("@id", usuarioId);
            connection.Open();
            var resultado = command.ExecuteScalar();
            return resultado?.ToString() ?? "Anónimo";
        }


    }
}
