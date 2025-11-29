using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Xunit;
namespace Pruebas.Integracion
{
    public class ArticulosCreateTests : IClassFixture<TestApplicationFactory>
    {
        private readonly HttpClient _client;

        public ArticulosCreateTests(TestApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CrearArticulo_Admin_ReturnsCreated()
        {
            //--------------------------------------------
            // ⭐ 1. GENERAR TOKEN JWT CON ROL ADMINISTRADOR
            //--------------------------------------------
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

            //--------------------------------------------
            // ⭐ 2. PREPARAR el cuerpo del POST
            //--------------------------------------------
            var nuevoArticulo = new
            {
                Nombre = "Casco de protección",
                Cantidad = 15,
                Disponibles = 15
            };

            var content = new StringContent(
                JsonSerializer.Serialize(nuevoArticulo),
                Encoding.UTF8,
                "application/json");

            //--------------------------------------------
            // ⭐ 3. EJECUTAR el POST
            //--------------------------------------------
            var response = await _client.PostAsync("/api/articulo", content);

            //--------------------------------------------
            // ⭐ 4. VALIDAR resultado
            //--------------------------------------------
            var responseContent = await response.Content.ReadAsStringAsync();

            // ⭐ Si falla, mostrar el error para debugging
            if (response.StatusCode != System.Net.HttpStatusCode.Created)
            {
                throw new Exception($"Expected Created but got {response.StatusCode}. Response: {responseContent}");
            }

            Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);

            // ⭐ Validar que la respuesta contiene el artículo creado
            Assert.False(string.IsNullOrEmpty(responseContent));

            // ⭐ Deserializar y validar campos específicos
            var articuloCreado = JsonSerializer.Deserialize<JsonElement>(responseContent);
            Assert.True(articuloCreado.TryGetProperty("nombre", out var nombre));
            Assert.Equal("Casco de protección", nombre.GetString());
            Assert.True(articuloCreado.TryGetProperty("cantidad", out var cantidad));
            Assert.Equal(15, cantidad.GetInt32());
            Assert.True(articuloCreado.TryGetProperty("disponibles", out var disponibles));
            Assert.Equal(15, disponibles.GetInt32());
        }

        [Fact]
        public async Task CrearArticulo_SinAutorizacion_ReturnsUnauthorized()
        {
            //--------------------------------------------
            // ⭐ NO agregar token JWT
            //--------------------------------------------
            var nuevoArticulo = new
            {
                Nombre = "Protector bucal",
                Cantidad = 30,
                Disponibles = 30
            };

            var content = new StringContent(
                JsonSerializer.Serialize(nuevoArticulo),
                Encoding.UTF8,
                "application/json");

            var response = await _client.PostAsync("/api/articulo", content);

            //--------------------------------------------
            // ⭐ Debe retornar Unauthorized (401)
            //--------------------------------------------
            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task CrearArticulo_RolInstructor_ReturnsForbidden()
        {
            //--------------------------------------------
            // ⭐ Token con rol NO autorizado
            //--------------------------------------------
            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("mi-clave-super-secreta-de-32bytes"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "DojankwonAPI",
                audience: "DojankwonClient",
                expires: DateTime.Now.AddHours(1),
                claims: new[]
                {
                new Claim(ClaimTypes.Role, "Instructor")
                },
                signingCredentials: credentials
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", jwt);

            var nuevoArticulo = new
            {
                Nombre = "Vendas",
                Cantidad = 50,
                Disponibles = 50
            };

            var content = new StringContent(
                JsonSerializer.Serialize(nuevoArticulo),
                Encoding.UTF8,
                "application/json");

            var response = await _client.PostAsync("/api/articulo", content);

            //--------------------------------------------
            // ⭐ Debe retornar Forbidden (403)
            //--------------------------------------------
            Assert.Equal(System.Net.HttpStatusCode.Forbidden, response.StatusCode);
        }

    }
}