using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Tiendaenlinea.Infraestructura.Persistencia;
using Tiendaenlinea.Dominio.Repositorios;
using Tiendaenlinea.Infraestructura.Persistencia.Repositorios;
using Tiendaenlinea.Aplicacion.Servicios;
using Tiendaenlinea.Dominio.Servicios;
using Tiendaenlinea.Infraestructura.Servicios;

var builder = WebApplication.CreateBuilder(args);

// Usamos solo una cadena de conexión
var connectionString = builder.Configuration.GetConnectionString("TiendaConnection");

// Inyectamos el repositorio
builder.Services.AddScoped<IUsuarioRepository>(provider =>
    new UsuarioRepository(connectionString));
builder.Services.AddScoped<ITiendaRepository>(provider =>
    new TiendaRepository(connectionString));
builder.Services.AddScoped<IProductoRepository>(provider =>
    new ProductoRepository(connectionString));

builder.Services.AddScoped<IPedidoRepository>(provider =>
    new PedidoRepository(connectionString));

builder.Services.AddScoped<ICarritoService>(provider =>
{
    var productoRepo = provider.GetRequiredService<IProductoRepository>();
    return new CarritoService(productoRepo, connectionString!);
});

builder.Services.AddScoped<IDireccionService>(provider =>
    new DireccionService(connectionString));

builder.Services.AddScoped<IImagenTiendaRepository, ImagenTiendaRepository>();
builder.Services.AddScoped<TiendaService>();

// Agregamos los servicios comunes
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuramos CORS para Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Configuramos EF Core con SQL Server
builder.Services.AddDbContext<TiendaContext>(options =>
{
    options.UseSqlServer(connectionString);
});

var app = builder.Build();

// Verificamos la conexión
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TiendaContext>();
    Console.WriteLine("✅ Conexión a la base de datos configurada correctamente.");
}

// Middleware
app.UseCors("AllowAngularApp");

app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers(); // Este es necesario si estás usando [ApiController]

app.Run();