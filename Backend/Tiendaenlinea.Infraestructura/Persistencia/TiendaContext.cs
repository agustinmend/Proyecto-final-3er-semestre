using Microsoft.EntityFrameworkCore;
using Tiendaenlinea.Dominio.Entidades;

namespace Tiendaenlinea.Infraestructura.Persistencia
{ 
public class TiendaContext : DbContext
{
	public TiendaContext(DbContextOptions<TiendaContext> options) : base(options)
	{
	}
	public DbSet<Usuario> Usuarios { get; set; }
	public DbSet<Tienda> Tiendas { get; set; }
	public DbSet<Producto> Productos { get; set; }
	public DbSet<Categoria> Categorias { get; set; }
	public DbSet<Pedido> Pedidos { get; set; }
	public DbSet<DetallePedido> DestallePedidos { get; set; }
	public DbSet<Direccion> Direcciones { get; set; }

	}
}
