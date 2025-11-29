using BLL;
using DAL;
using DAL.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dojanwonk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticuloController : ControllerBase
    {
        private readonly IServiceArticulo serviceArticulo;
        public ArticuloController(IServiceArticulo serviceArticulo)
        {
            this.serviceArticulo = serviceArticulo;
        }
        [HttpPost]
        [Authorize(Policy = "AdminORecepcion")]
        public async Task<ActionResult<Articulo>> Agregar(Articulo articulo)
        {
            try
            {
                if (await serviceArticulo.Agregar(articulo))
                {
                    return StatusCode(StatusCodes.Status201Created, articulo);
                }
                return BadRequest("No se pudo agregar el estudiante.");
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Authorize(Policy = "TodoStaff")]
        public async Task<ActionResult<IEnumerable<Articulo>>> Leer()
        {
            return Ok(await  serviceArticulo.Leer());
        }
        [HttpGet("{id}")]
        [Authorize(Policy = "TodoStaff")]
        public async Task<ActionResult<Articulo>> Buscar(int id)
        {
            var buscado = await serviceArticulo.Buscar(id);
            if(buscado == null)
            {
                return NotFound();
            }
            return Ok(buscado);
        }
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminORecepcion")]
        public async Task<ActionResult> Eliminar(int id)
        {
            try
            {
                await serviceArticulo.Eliminar(id);
                return Ok("Articulo eliminado con exito");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPut]
        [Authorize(Policy = "AdminORecepcion")]
        public async Task<ActionResult> Actualizar(Articulo articulo)
        {
            try
            {
                await serviceArticulo.Actualizar(articulo
                    );
                return Ok("Articulo eliminado con exito");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
