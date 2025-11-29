using DAL.Modelos;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
namespace Pruebas.Integracion
{
    public class EstudiantesCreateTests : IClassFixture<TestApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly TestApplicationFactory _factory;

        public EstudiantesCreateTests(TestApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("mi-clave-super-secreta-de-32bytes"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: "DojankwonAPI",
                audience: "DojankwonClient",
                expires: DateTime.Now.AddHours(1),
                claims: new[]
                {
                new Claim(ClaimTypes.Role, "Administrador")
                },
                signingCredentials: creds
            );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", jwt);
        }

        [Fact]
        public async Task CrearEstudiante_Admin_ReturnsCreated()
        {
            var estudiante = new Estudiante
            {
                Id = "1044609183",
                Nombres = "Juan",
                Apellidos = "Pérez",
                Telefono = "3205550000",
                Correo = "carlos@test.com",
                Direccion = "Calle 22",
                Eps = "Sura",
                IdRango = 1,
                FechaNacimiento = new DateOnly(2010, 1, 1)
            };

            var response = await _client.PostAsJsonAsync("/api/Estudiante", estudiante);

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Expected Created but got BadRequest. Response: {errorContent}");
            }

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<DbDojankwonContext>();

            var creado = db.Estudiantes.FirstOrDefault(e => e.Id == "1044609183");
            Assert.NotNull(creado);

            var pago = db.Pagos.FirstOrDefault(p => p.IdEstudiante == "1044609183");
            Assert.NotNull(pago);
            Assert.Equal("pagado", pago.Estado);
        }
    }
}