using MicroservicioFasecolda.Models;
using Microsoft.Extensions.Options;
using MicroservicioFasecolda.Settings;
using MongoDB.Driver;

namespace MicroservicioFasecolda.Data
{
    public class AccidenteService
    {
        private readonly IMongoCollection<Accidente> _accidentes;

        public AccidenteService(IOptions<MongoDbSettings> settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _accidentes = database.GetCollection<Accidente>(settings.Value.AccidentesCollectionName);
        }

        public async Task<List<Accidente>> ObtenerPorPlacaAsync(string placa)
        {
            return await _accidentes.Find(a => a.Placa == placa).ToListAsync();
        }

        public async Task InsertarAccidenteAsync(Accidente acc)
        {
            await _accidentes.InsertOneAsync(acc);
        }
    }
}
