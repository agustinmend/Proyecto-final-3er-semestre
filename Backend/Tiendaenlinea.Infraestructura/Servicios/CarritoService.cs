using System.Collections.Concurrent;
using System.Data;
using System.Data.SqlClient;
using Tiendaenlinea.Dominio.DTOs;
using Tiendaenlinea.Dominio.Servicios;
using Tiendaenlinea.Dominio.Repositorios;
using Tiendaenlinea.Dominio.Entidades;
namespace Tiendaenlinea.Infraestructura.Servicios
{
    public class CarritoService : ICarritoService
    {
        private readonly IProductoRepository _productoRepository;
        private readonly string _connectionString;

        public CarritoService(IProductoRepository productoRepository, string connectionString)
        {
            _productoRepository = productoRepository;
            _connectionString = connectionString;
        }

        public void AgregarProducto(AgregarAlCarritoDto producto)
        {
            if (!CarritoMemoria.Carritos.ContainsKey(producto.UsuarioID))
            {
                CarritoMemoria.Carritos[producto.UsuarioID] = new List<AgregarAlCarritoDto>();
            }

            var carritoUsuario = CarritoMemoria.Carritos[producto.UsuarioID];

            var productoExistente = carritoUsuario.FirstOrDefault(p => p.ProductoID == producto.ProductoID);



            if (productoExistente != null)
            {
                productoExistente.Cantidad += producto.Cantidad;
                Console.WriteLine($"➕ Se aumentó la cantidad del producto {producto.ProductoID} en el carrito del usuario {producto.UsuarioID}");
            }
            else
            {
                carritoUsuario.Add(producto);
                Console.WriteLine($"🛒 Producto nuevo agregado al carrito del usuario {producto.UsuarioID}");
            }

            Console.WriteLine($"🧺 Total productos ahora: {carritoUsuario.Count}");
        }

        public List<ProductoEnCarritoDto> ObtenerCarrito(int usuarioId)
        {
            Console.WriteLine($"🔍 Buscando carrito del usuario {usuarioId}");
            Console.WriteLine($"¿Existe?: {CarritoMemoria.Carritos.ContainsKey(usuarioId)}");
            var resultado = new List<ProductoEnCarritoDto>();

            if (CarritoMemoria.Carritos.TryGetValue(usuarioId, out var productos))
            {
                foreach (var p in productos)
                {
                    var productoDb = _productoRepository.ObtenerPorID(p.ProductoID);
                    if (productoDb != null)
                    {
                        resultado.Add(new ProductoEnCarritoDto
                        {
                            ProductoID = p.ProductoID,
                            NombreProducto = productoDb.NombreProducto,
                            Descripcion = productoDb.Descripcion,
                            ImagenUrl = productoDb.ImagenUrl ?? "",
                            PrecioUnitario = p.PrecioUnitario,
                            Cantidad = p.Cantidad
                        });
                    }
                }
            }

            return resultado;
        }


        public void VaciarCarrito(int usuarioId)
        {
            CarritoMemoria.Carritos.TryRemove(usuarioId, out _);
        }

        public int ConfirmarCompra(ConfirmarCompraDto dto)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var command = new SqlCommand("confirmarcompra", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@UsuarioID", dto.UsuarioID);
            command.Parameters.AddWithValue("@DireccionID", dto.DireccionID);

            var table = new DataTable();
            table.Columns.Add("ProductoID", typeof(int));
            table.Columns.Add("Cantidad", typeof(int));
            table.Columns.Add("PrecioUnitario", typeof(decimal));

            foreach (var item in dto.Carrito)
            {
                table.Rows.Add(item.ProductoID, item.Cantidad, item.PrecioUnitario);
            }

            var carritoParam = command.Parameters.AddWithValue("@Carrito", table);
            carritoParam.SqlDbType = SqlDbType.Structured;
            carritoParam.TypeName = "Carrito";

            var pedidoId = 0;

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    pedidoId = reader.GetInt32(reader.GetOrdinal("PedidoID"));
                }
            }

            CarritoMemoria.Carritos.TryRemove(dto.UsuarioID, out _);

            return pedidoId;
        }

    }
}
