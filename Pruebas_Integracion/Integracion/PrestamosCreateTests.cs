using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Xunit;
namespace Pruebas.Integracion
{
    public class PrestamosCreateTests : IClassFixture<TestApplicationFactory>
    {
        private readonly HttpClient _client;

        public PrestamosCreateTests(TestApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CrearPrestamo_AdminConArticulosDisponibles_ReturnsCreated()
        {
            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("mi-clave-super-secreta-de-32bytes"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "DojankwonAPI",
                audience: "DojankwonClient",
                expires: DateTime.Now.AddHours(1),
                claims: new[]
                {
                new Claim(ClaimTypes.Role, "Administrador")
                },
                signingCredentials: credentials
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", jwt);
            var nuevoPrestamo = new
            {
                EstudianteId = "1044609182",
                FechaPrestamo = DateOnly.FromDateTime(DateTime.Now),
                FechaDevolucion = DateOnly.FromDateTime(DateTime.Now.AddDays(7)),
                Estado = "Activo",
                DetallePrestamos = new[]
                {
                new
                {
                    IdArticulo = 1,
                    Cantidad = 2
                }
            }
            };
            var content = new StringContent(
                JsonSerializer.Serialize(nuevoPrestamo),
                Encoding.UTF8,
                "application/json");
            var response = await _client.PostAsync("/api/prestamo", content);
            Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.False(string.IsNullOrEmpty(responseContent));
            var prestamoCreado = JsonSerializer.Deserialize<JsonElement>(responseContent);
            Assert.True(prestamoCreado.TryGetProperty("estudianteId", out _));
            Assert.True(prestamoCreado.TryGetProperty("estado", out _));
        }

        [Fact]
        public async Task CrearPrestamo_CantidadExcedida_ReturnsBadRequest()
        {
            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("mi-clave-super-secreta-de-32bytes"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "DojankwonAPI",
                audience: "DojankwonClient",
                expires: DateTime.Now.AddHours(1),
                claims: new[]
                {
                new Claim(ClaimTypes.Role, "Administrador")
                },
                signingCredentials: credentials
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", jwt);
            var nuevoPrestamo = new
            {
                EstudianteId = "1044609182",
                FechaPrestamo = DateOnly.FromDateTime(DateTime.Now),
                FechaDevolucion = DateOnly.FromDateTime(DateTime.Now.AddDays(7)),
                Estado = "Activo",
                DetallePrestamos = new[]
                {
                new
                {
                    IdArticulo = 1,
                    Cantidad = 100
                }
            }
            };

            var content = new StringContent(
                JsonSerializer.Serialize(nuevoPrestamo),
                Encoding.UTF8,
                "application/json");

            var response = await _client.PostAsync("/api/prestamo", content);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}