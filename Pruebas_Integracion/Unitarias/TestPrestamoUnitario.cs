using BLL;
using DAL;
using DAL.Modelos;
using Moq;
using Xunit;
namespace Pruebas.Unitarias
{
    public class ServicePrestamoAlquilarTests
    {
        [Fact]
        public async Task Alquilar_TodoCorrecto_RetornaTrue()
        {
            // Arrange
            var mockDBPrestamo = new Mock<IDBPrestamo>();
            var mockServiceArticulo = new Mock<IServiceArticulo>();
            var mockServiceEstudiante = new Mock<IServiceEstudiante>();

            // Estudiante activo
            var estudiante = new Estudiante
            {
                Id = "1044609182",
                Nombres = "Juan",
                Apellidos = "Pérez",
                estado = "Activo",
                Correo = "juan@test.com"
            };

            // Artículo con disponibilidad
            var articulo = new Articulo
            {
                Id = 1,
                Nombre = "Guantes",
                Cantidad = 10,
                Disponibles = 10
            };

            // Préstamo nuevo
            var prestamo = new Prestamo
            {
                EstudianteId = "1044609182",
                FechaPrestamo = DateOnly.FromDateTime(DateTime.Now),
                FechaDevolucion = DateOnly.FromDateTime(DateTime.Now.AddDays(7)),
                DetallePrestamos = new List<DetallePrestamo>
            {
                new DetallePrestamo
                {
                    IdArticulo = 1,
                    Cantidad = 2
                }
            }
            };

            // Setup mocks
            mockServiceEstudiante
                .Setup(s => s.Buscar("1044609182"))
                .ReturnsAsync(estudiante);

            mockServiceArticulo
                .Setup(s => s.Buscar(1))
                .ReturnsAsync(articulo);

            mockServiceArticulo
                .Setup(s => s.Actualizar(It.IsAny<Articulo>()))
                .ReturnsAsync(true);

            mockDBPrestamo
                .Setup(db => db.Leer())
                .ReturnsAsync(new List<Prestamo>()); // Sin préstamos previos

            mockDBPrestamo
                .Setup(db => db.Agregar(It.IsAny<Prestamo>()))
                .Returns(Task.CompletedTask);

            var service = new ServicePrestamo(
                mockDBPrestamo.Object,
                mockServiceArticulo.Object,
                mockServiceEstudiante.Object
            );

            // Act
            var resultado = await service.Alquilar(prestamo);

            // Assert
            Assert.True(resultado);
            Assert.Equal("En prestamo", prestamo.Estado);

            // Verificar que el artículo se actualizó correctamente
            mockServiceArticulo.Verify(
                s => s.Actualizar(It.Is<Articulo>(a => a.Disponibles == 8)),
                Times.Once
            );

            // Verificar que el préstamo se agregó
            mockDBPrestamo.Verify(
                db => db.Agregar(It.IsAny<Prestamo>()),
                Times.Once
            );
        }
    }
}