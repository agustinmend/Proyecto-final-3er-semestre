using System.Collections.Concurrent;
using System.Data;
using System.Data.SqlClient;
using Tiendaenlinea.Dominio.DTOs;
using Tiendaenlinea.Dominio.Servicios;
using Tiendaenlinea.Dominio.Repositorios;
using Tiendaenlinea.Dominio.Entidades;
namespace Tiendaenlinea.Infraestructura.Servicios
{
    public class DireccionService : IDireccionService
    {
        private readonly string _connectionString;

        public DireccionService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int AgregarDireccion(AgregarDireccionDto dto)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var command = new SqlCommand("AgregarDireccion", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@UsuarioID", dto.UsuarioID);
            command.Parameters.AddWithValue("@Pais", dto.Pais);
            command.Parameters.AddWithValue("@Ciudad", dto.Ciudad);
            command.Parameters.AddWithValue("@CodigoPostal", dto.CodigoPostal);
            command.Parameters.AddWithValue("@DireccionDetalle", dto.DireccionDetalle);

            var direccionId = 0;

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    direccionId = Convert.ToInt32(reader["DireccionID"]);
                }
            }

            return direccionId;
        }

    }

}
