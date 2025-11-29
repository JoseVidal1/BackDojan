using BLL;
using DAL.Modelos;
using DTOS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System.Text;

namespace Dojanwonk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IServiceUsuario logica;
        private readonly TokenService tokenService;
        public UsuarioController(IServiceUsuario logica, TokenService tokenService)
        {
            this.logica = logica;
            this.tokenService = tokenService;
        }
        [HttpGet]
        [Authorize(Policy = "SoloAdmin")]
        public async Task<ActionResult<IEnumerable<Usuario>>> Leer()
        {
            return Ok(await logica.Leer());
        }
        [HttpPost]
        [Authorize(Policy = "SoloAdmin")]
        public async Task<ActionResult<Usuario>> Agregar(Usuario usuario)
        {
            try
            {
                if (await logica.Agregar(usuario))
                {
                    return StatusCode(StatusCodes.Status201Created, usuario);
                }
                return BadRequest("No se pudo agregar el usuario");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet("{cc}")]
        [Authorize(Policy = "SoloAdmin")]
        public async Task<ActionResult<Usuario>> Buscar(string cc)
        {
            var buscado = await logica.Buscar(cc);
            if (buscado == null)
            {
                return NotFound();
            }
            return Ok(buscado);
        }
        [HttpDelete("{cc}")]
        [Authorize(Policy = "SoloAdmin")]
        public async Task<ActionResult> Eliminar(string cc)
        {
            try
            {
                await logica.Eliminar(cc);
                return Ok("Usuario eliminado con exito");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Authorize(Policy = "SoloAdmin")]
        public async Task<ActionResult<Usuario>> Actualizar(Usuario usuario)
        {
            try
            {
                var actualizado = await logica.Actualizar(usuario);
                return Ok(actualizado);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UsuarioDTO usuarioDTO)
        {
            var usuario = await logica.Login(usuarioDTO);
            if (usuario == null)
                return Unauthorized("Credenciales incorrectas");
            try
            {
                var token = tokenService.GenerateToken(usuario);


                return Ok(new
                {
                    token,
                    username = usuario.UserName,
                    rol = usuario.Rol,
                    nombres = usuario.Nombres,
                    apellidos = usuario.Apellidos,
                    correo= usuario.Correo,
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest("Error al generar el token: " + e.Message);
            }
        }
    }
}
