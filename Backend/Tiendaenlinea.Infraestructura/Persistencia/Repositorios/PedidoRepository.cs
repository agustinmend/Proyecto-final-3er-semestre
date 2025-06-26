using System.Data;
using System.Data.SqlClient;
using Tiendaenlinea.Dominio.DTOs;
using Tiendaenlinea.Dominio.Repositorios;

namespace Tiendaenlinea.Infraestructura.Persistencia.Repositorios
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly string _connectionString;

        public PedidoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<DetalleVentaDto> ObtenerPedidosPorUsuario(int usuarioID)
        {
            var resultado = new List<DetalleVentaDto>();

            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var query = "SELECT * FROM mostrar_detalle_venta WHERE ID_Cliente = @UsuarioID";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UsuarioID", usuarioID);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                resultado.Add(new DetalleVentaDto
                {
                    NroFactura = reader.GetInt32(reader.GetOrdinal("Nro_factura")),
                    FechaPedido = reader.GetDateTime(reader.GetOrdinal("FechaPedido")),
                    EstadoPedido = reader.GetString(reader.GetOrdinal("Estado_pedido")),
                    DireccionEnvio = reader.GetString(reader.GetOrdinal("Direccion_envio")),
                    ProductoID = reader.GetInt32(reader.GetOrdinal("ProductoID")),
                    NombreProducto = reader.GetString(reader.GetOrdinal("NombreProducto")),
                    Cantidad = reader.GetInt32(reader.GetOrdinal("Cantidad")),
                    PrecioUnitario = reader.GetDecimal(reader.GetOrdinal("PrecioUnitario")),
                    Subtotal = reader.GetDecimal(reader.GetOrdinal("Subtotal")),
                    Tienda = reader.GetString(reader.GetOrdinal("Tienda"))
                });
            }

            return resultado;
        }

        public void CambiarEstado(int pedidoID, string estadoNuevo)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("cambiarestadopedido", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PedidoID", pedidoID);
                command.Parameters.AddWithValue("@Estadonuevo", estadoNuevo);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

    }
}
