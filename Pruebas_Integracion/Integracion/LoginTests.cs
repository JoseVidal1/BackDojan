using System.Net;
using System.Net.Http.Json;
using DTOS;

namespace Pruebas.Integracion
{
    public class LoginTests : IClassFixture<TestApplicationFactory>
    {
        private readonly HttpClient _client;

        public LoginTests(TestApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Login_Admin_ReturnsOkWithToken()
        {
            var loginDTO = new UsuarioDTO
            {
                username = "adminTest",
                password = "12345"
            };
            var response = await _client.PostAsJsonAsync("/api/Usuario/login", loginDTO);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var data = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();

            Assert.True(data.ContainsKey("token"));
            Assert.NotNull(data["token"]);
        }

        [Fact]
        public async Task Login_CredencialesInvalidas_ReturnsUnauthorized()
        {
            // Arrange
            var loginDTO = new UsuarioDTO
            {
                username = "adminTest",
                password = "xxxxxxxx"  // contraseña incorrecta
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/Usuario/login", loginDTO);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
