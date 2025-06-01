using Microsoft.AspNetCore.Mvc;
using MicroservicioFasecolda.Data;
using MicroservicioFasecolda.Models;

namespace MicroservicioFasecolda.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccidentesController : ControllerBase
    {
        private readonly AccidenteService _accidenteService;

        public AccidentesController(AccidenteService accidenteService)
        {
            _accidenteService = accidenteService;
        }

        [HttpGet("{placa}")]
        public async Task<IActionResult> GetPorPlaca(string placa)
        {
            var resultado = await _accidenteService.ObtenerPorPlacaAsync(placa);
            if (!resultado.Any())
                return NotFound(new { mensaje = "No hay accidentes registrados para esta placa." });

            return Ok(resultado);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Accidente acc)
        {
            await _accidenteService.InsertarAccidenteAsync(acc);
            return CreatedAtAction(nameof(GetPorPlaca), new { placa = acc.Placa }, acc);
        }
    }
}
