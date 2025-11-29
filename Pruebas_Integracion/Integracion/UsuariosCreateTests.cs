using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
namespace Pruebas.Integracion
{
    public class UsuariosCreateTests : IClassFixture<TestApplicationFactory>
    {
        private readonly HttpClient _client;

        public UsuariosCreateTests(TestApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CrearUsuario_Admin_ReturnsCreated()
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
                new Claim(ClaimTypes.Role, "Administrador") // ⭐ NECESARIO para que pase la autorización
                },
                signingCredentials: credentials
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", jwt);
            var nuevoUsuario = new
            {
                Cc = "123456789",
                Nombres = "Usuario",
                Apellidos = "Prueba",
                UserName = "usuarioPrueba",
                Contraseña = "12345",
                Rol = "Instructor",
                Telefono = "3001112233",
                Correo = "prueba@correo.com",
                Direccion = "Calle 123"
            };

            var content = new StringContent(
                JsonSerializer.Serialize(nuevoUsuario),
                Encoding.UTF8,
                "application/json");

            //--------------------------------------------
            // ⭐ 3. EJECUTAR el POST
            //--------------------------------------------
            var response = await _client.PostAsync("/api/usuario", content);

            //--------------------------------------------
            // ⭐ 4. VALIDAR resultado
            //--------------------------------------------
            Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
        }
    }
}
