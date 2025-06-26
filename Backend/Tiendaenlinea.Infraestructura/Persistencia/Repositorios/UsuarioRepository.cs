using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Tiendaenlinea.Dominio.Entidades;
using Tiendaenlinea.Dominio.Repositorios;

namespace Tiendaenlinea.Infraestructura.Persistencia.Repositorios
{
	public class UsuarioRepository : IUsuarioRepository
	{
		private readonly string ConnectionString;
		
		public UsuarioRepository(string connectionString)
		{
			ConnectionString = connectionString;
		}

        public int RegistrarUsuario(Usuario usuario)
        {
            try
            {
                int usuarioID;
                using (var connection = new SqlConnection(ConnectionString))
                {
                    var command = new SqlCommand("RegistrarUsuario", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                    command.Parameters.AddWithValue("@Apellido", usuario.Apellido);
                    command.Parameters.AddWithValue("@Email", usuario.Email);
                    command.Parameters.AddWithValue("@Contrasena", usuario.Contrasena);

                    var outputParam = new SqlParameter("@UsuarioID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(outputParam);

                    connection.Open();
                    command.ExecuteNonQuery();

                    usuarioID = Convert.ToInt32(outputParam.Value);
                }

                return usuarioID;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al registrar usuario: " + ex.Message);
            }
        }

        public Usuario? ObtenerPorEmail(string email)
		{
			using (var connection = new SqlConnection(ConnectionString))
			{
				var command = new SqlCommand("Select * from Usuarios where Email = @Email", connection);
				command.Parameters.AddWithValue("@Email", email);
				connection.Open();
				using (var reader = command.ExecuteReader())
				{
					if (reader.Read())
					{
						return new Usuario
						{
							UsuarioID = Convert.ToInt32(reader["UsuarioID"]),
							Nombre = reader["Nombre"].ToString(),
							Apellido = reader["Apellido"].ToString(),
							Email = reader["Email"].ToString(),
							Contrasena = reader["Contrasena"].ToString(),
                            FechaRegistro = Convert.ToDateTime(reader["FechaRegistro"])
                        };
					}
				}
			}
			return null;
		}
	
	}

}
