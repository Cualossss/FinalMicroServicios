using Xunit;
using Moq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using MicroservicioValidaciones.Data;
using MicroservicioValidaciones.Services;
using MicroservicioValidaciones.DTOs;
using MicroservicioValidaciones.Models;
using Moq.Protected;

namespace MicroservicioValidaciones.Tests.Services
{
    public class ValidacionServiceTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);
            return context;
        }

        private HttpClient GetFakeHttpClient(string jsonResponse, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var handler = new Mock<HttpMessageHandler>();

            handler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new StringContent(jsonResponse)
                });

            return new HttpClient(handler.Object);
        }

        [Fact]
        public async Task ValidarSolicitud_DeberiaAprobarSiPuntajeMenorA400()
        {
            // Arrange
            var accidentes = new[]
            {
                new { Placa = "abc123", Fecha = "2023-01-01", Severidad = "solo latas" }, // 100 pts
                new { Placa = "abc123", Fecha = "2023-02-01", Severidad = "heridos" }    // 200 pts
            };

            var json = JsonSerializer.Serialize(accidentes);
            var httpClient = GetFakeHttpClient(json);
            var db = GetDbContext();
            var service = new ValidacionService(db, httpClient);

            var request = new ValidacionRequest
            {
                CedulaCliente = "123",
                Placa = "abc123"
            };

            // Act
            var resultado = await service.ValidarSolicitudAsync(request);

            // Assert
            Assert.Equal("aprobada", resultado);
            Assert.Single(db.Cotizaciones);
            Assert.Equal("aprobada", db.Cotizaciones.First().Resultado);
        }

        [Fact]
        public async Task ValidarSolicitud_DeberiaRechazarSiPuntajeMayorIgual400()
        {
            var accidentes = new[]
            {
                new { Placa = "abc123", Fecha = "2023-01-01", Severidad = "muertos" }, // 300 pts
                new { Placa = "abc123", Fecha = "2023-02-01", Severidad = "heridos" }  // 200 pts
            };

            var json = JsonSerializer.Serialize(accidentes);
            var httpClient = GetFakeHttpClient(json);
            var db = GetDbContext();
            var service = new ValidacionService(db, httpClient);

            var request = new ValidacionRequest
            {
                CedulaCliente = "456",
                Placa = "abc123"
            };

            var resultado = await service.ValidarSolicitudAsync(request);

            Assert.Equal("rechazada", resultado);
            Assert.Single(db.Cotizaciones);
            Assert.Equal("rechazada", db.Cotizaciones.First().Resultado);
        }

        [Fact]
        public async Task ValidarSolicitud_DeberiaRechazarSiNoHayRespuesta()
        {
            var httpClient = GetFakeHttpClient("[]", HttpStatusCode.NotFound);
            var db = GetDbContext();
            var service = new ValidacionService(db, httpClient);

            var request = new ValidacionRequest
            {
                CedulaCliente = "789",
                Placa = "noexiste"
            };

            var resultado = await service.ValidarSolicitudAsync(request);

            Assert.Equal("rechazada", resultado);
            Assert.Single(db.Cotizaciones);
            Assert.Equal("rechazada", db.Cotizaciones.First().Resultado);
        }
    }
}
