using MicroservicioFasecolda.Models;

namespace MicroservicioFasecolda.Services
{
    public interface IAccidenteService
    {
        IEnumerable<Accidente> ObtenerPorPlaca(string placa);
    }
}
