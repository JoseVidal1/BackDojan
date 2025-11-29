using BLL;
using DAL;
using DAL.Modelos;
using DTOS;
using Moq;
using Xunit;

namespace Pruebas.Unitarias
{
    public class ServiceUsuarioLoginTests
    {
        [Fact]
        public async Task Login_CredencialesCorrectas_RetornaUsuario()
        {
            // Arrange
            var mockDBUsuario = new Mock<IDBUsuario>();

            var usuariosSimulados = new List<Usuario>
        {
            new Usuario
            {
                Cc = "1044609182",
                Nombres = "Admin",
                Apellidos = "Test",
                Correo = "admin@test.com",
                UserName = "adminTest",
                Contraseña = "12345",
                Rol = "Administrador"
            },
            new Usuario
            {
                Cc = "1044609183",
                Nombres = "Instructor",
                Apellidos = "Test",
                Correo = "instructor@test.com",
                UserName = "instructorTest",
                Contraseña = "password123",
                Rol = "Instructor"
            }
        };

            mockDBUsuario
                .Setup(repo => repo.Leer())
                .ReturnsAsync(usuariosSimulados);

            var serviceUsuario = new ServiceUsuario(mockDBUsuario.Object);

            var usuarioDTO = new UsuarioDTO
            {
                username = "adminTest",
                password = "12345"
            };

            // Act
            var resultado = await serviceUsuario.Login(usuarioDTO);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("adminTest", resultado.UserName);
            Assert.Equal("12345", resultado.Contraseña);
            Assert.Equal("Administrador", resultado.Rol);
            Assert.Equal("admin@test.com", resultado.Correo);

            // Verificar que el método Leer fue llamado exactamente una vez
            mockDBUsuario.Verify(repo => repo.Leer(), Times.Once);
        }
    }
}
