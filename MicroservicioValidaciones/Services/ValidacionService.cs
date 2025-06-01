using MicroservicioValidaciones.DTOs;
using MicroservicioValidaciones.Models;
using MicroservicioValidaciones.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Text.Json;

namespace MicroservicioValidaciones.Services
{
    public class ValidacionService : IValidacionService
    {
        private readonly IMongoCollection<Cotizacion> _cotizaciones;
        private readonly HttpClient _httpClient;

        public ValidacionService(IOptions<MongoDbSettings> settings, IMongoClient mongoClient, HttpClient httpClient)
        {
            var db = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _cotizaciones = db.GetCollection<Cotizacion>(settings.Value.CotizacionesCollectionName);
            _httpClient = httpClient;
        }

        public async Task<string> ValidarSolicitudAsync(ValidacionRequest request)
        {
            var url = $"https://localhost:7006/api/accidentes/{request.Placa}";
            string resultado;

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                resultado = "rechazada";
                await RegistrarCotizacionAsync(request, resultado);
                return resultado;
            }

            var contenido = await response.Content.ReadAsStringAsync();
            var accidentes = JsonSerializer.Deserialize<List<AccidenteDto>>(contenido, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<AccidenteDto>();

            int puntaje = 0;
            foreach (var acc in accidentes)
            {
                if (acc.Severidad == "solo latas") puntaje += 100;
                else if (acc.Severidad == "heridos") puntaje += 200;
                else if (acc.Severidad == "muertos") puntaje += 300;
            }

            resultado = puntaje >= 400 ? "rechazada" : "aprobada";

            await RegistrarCotizacionAsync(request, resultado);
            return resultado;
        }

        private async Task RegistrarCotizacionAsync(ValidacionRequest request, string resultado)
        {
            var cot = new Cotizacion
            {
                CedulaCliente = request.CedulaCliente,
                Placa = request.Placa,
                Resultado = resultado
            };

            await _cotizaciones.InsertOneAsync(cot);
        }

        private class AccidenteDto
        {
            public string Placa { get; set; } = string.Empty;
            public DateTime Fecha { get; set; }
            public string Severidad { get; set; } = string.Empty;
        }
    }
}
