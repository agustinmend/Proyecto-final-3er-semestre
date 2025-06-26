using Tiendaenlinea.Dominio.Entidades;
using Tiendaenlinea.Dominio.DTOs;

namespace Tiendaenlinea.Dominio.Repositorios
{
    public interface IUsuarioRepository
    {
        int RegistrarUsuario(Usuario usuario);
        Usuario? ObtenerPorEmail(string email);
    }
}