using BLL;
using DAL.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dojanwonk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RangoController : ControllerBase
    {
        private readonly IServiceRango serviceRango;
        public RangoController(IServiceRango serviceRango)
        {
            this.serviceRango = serviceRango;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rango>>> Leer()
        {
            return Ok(await serviceRango.Leer());
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Rango>> Buscar(int id)
        {
            var buscado = await serviceRango.Buscar(id);
            if (buscado == null)
            {
                return NotFound();
            }
            return Ok(buscado);
        }
    }
}
