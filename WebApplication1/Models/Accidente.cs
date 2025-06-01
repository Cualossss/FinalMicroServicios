using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MicroservicioFasecolda.Models
{
    public class Accidente
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Placa { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string Severidad { get; set; } = string.Empty;
    }
}
