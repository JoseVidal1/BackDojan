using DAL;
using DAL.Modelos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class TestApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _dbName = $"TestDB_{Guid.NewGuid()}"; // ✅ Instancia única

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureServices(services =>
        {
            // 1. Eliminar DbContext real
            var descriptor = services.SingleOrDefault(d =>
                d.ServiceType == typeof(DbContextOptions<DbDojankwonContext>)
            );
            if (descriptor != null)
                services.Remove(descriptor);

            // 2. Crear un InMemory DB con nombre consistente
            services.AddDbContext<DbDojankwonContext>(options =>
                options.UseInMemoryDatabase(_dbName) // ✅ Usa la misma BD
            );

            // 3. Construir provider y hacer seed
            var serviceProvider = services.BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<DbDojankwonContext>();
                db.Database.EnsureCreated();

                // ==============================
                // SEED DE DATOS
                // ==============================
                db.Usuarios.Add(new Usuario
                {
                    Cc = "1044609182",
                    Nombres = "Admin",
                    Apellidos = "Test",
                    Correo = "admin@test.com",
                    Direccion = "Calle Falsa 123",
                    Telefono = "3001234567",
                    UserName = "adminTest",
                    Rol = "Administrador",
                    Contraseña = "12345"
                });

                db.Rangos.AddRange(
                    new Rango { Id = 1, Nombre = "Blanco" },
                    new Rango { Id = 2, Nombre = "Amarillo" },
                    new Rango { Id = 3, Nombre = "Verde" },
                    new Rango { Id = 4, Nombre = "Azul" },
                    new Rango { Id = 5, Nombre = "Rojo" },
                    new Rango { Id = 6, Nombre = "Negro" }
                );

                db.Grupos.AddRange(
                    new Grupo { Id = 1, Nombre = "5-6" },
                    new Grupo { Id = 2, Nombre = "6-7" },
                    new Grupo { Id = 3, Nombre = "7-8" }
                );

                db.Articulos.AddRange(
                    new Articulo
                    {
                        Id = 1,
                        Nombre = "Guantes de Taekwondo",
                        Cantidad = 10,
                        Disponibles = 10
                    },
                    new Articulo
                    {
                        Id = 2,
                        Nombre = "Peto de combate",
                        Cantidad = 5,
                        Disponibles = 5
                    }
                );

                db.Estudiantes.AddRange(
    new Estudiante
    {
        Id = "1044609182",
        Nombres = "Estudiante",
        Apellidos = "Test",
        Telefono = "3001234562",
        Correo = "estudiante@test.com",
        Direccion = "Calle Falsa 456",
        Eps = "Sanitas",
        IdRango = 1,
        FechaNacimiento = new DateOnly(2000, 1, 1),
        FechaRegistro = DateOnly.FromDateTime(DateTime.Now),
        estado = "Activo"
    },
    new Estudiante // ✅ Nuevo estudiante para otros tests
    {
        Id = "1044609180",
        Nombres = "Carlos",
        Apellidos = "López",
        Telefono = "3001234563",
        Correo = "carlos@test.com",
        Direccion = "Calle Nueva 789",
        Eps = "Sura",
        IdRango = 1,
        FechaNacimiento = new DateOnly(2005, 6, 15),
        FechaRegistro = DateOnly.FromDateTime(DateTime.Now),
        estado = "Activo"
    }
);

                db.SaveChanges();
            }
        });
    }
}


