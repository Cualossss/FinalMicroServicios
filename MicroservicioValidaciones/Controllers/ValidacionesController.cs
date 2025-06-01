using Microsoft.AspNetCore.Mvc;
using MicroservicioValidaciones.DTOs;
using MicroservicioValidaciones.Services;

namespace MicroservicioValidaciones.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValidacionesController : ControllerBase
    {
        private readonly IValidacionService _validacionService;

        public ValidacionesController(IValidacionService validacionService)
        {
            _validacionService = validacionService;
        }

        [HttpPost]
        public async Task<IActionResult> Validar([FromBody] ValidacionRequest request)
        {
            var resultado = await _validacionService.ValidarSolicitudAsync(request);
            return Ok(new { resultado });
        }
    }
}
