using MicroservicioFasecolda.Data;
using MicroservicioFasecolda.Models;

namespace MicroservicioFasecolda.Services
{
    public class AccidenteService : IAccidenteService
    {
        private readonly AppDbContext _context;

        public AccidenteService(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Accidente> ObtenerPorPlaca(string placa)
        {
            return _context.Accidentes.Where(a => a.Placa == placa).ToList();
        }
    }
}
