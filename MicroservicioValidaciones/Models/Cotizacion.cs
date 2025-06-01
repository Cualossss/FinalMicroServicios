using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MicroservicioValidaciones.Models
{
    public class Cotizacion
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Placa { get; set; } = string.Empty;
        public string CedulaCliente { get; set; } = string.Empty;
        public string Resultado { get; set; } = string.Empty;
    }
}
