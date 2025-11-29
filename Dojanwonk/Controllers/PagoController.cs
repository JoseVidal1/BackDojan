using BLL;
using DAL.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dojanwonk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagoController : ControllerBase
    {

    private readonly IServicePago servicePago;
        public PagoController(IServicePago servicePago)
        {
            this.servicePago = servicePago;
        }
        [HttpPut]
        [Authorize(Policy = "AdminORecepcion")]
        public async Task<IActionResult> ActualizarPago(Pago pago)
        {
            var actualizado = await servicePago.ActualizarPago(pago);
            return Ok(actualizado);
        }
        [HttpGet]
        [Authorize(Policy = "AdminORecepcion")]
        public async Task<ActionResult<IEnumerable<Pago>>> LeerPagos()
        {
            return Ok(await servicePago.Leer());
        }
        [HttpPost("generarPagos")]
        [AllowAnonymous]
        public async Task<IActionResult> GenerarPagos()
        {
            await servicePago.GenerarPagos();
            return Ok("Pagos generados.");
        }
    }
}
