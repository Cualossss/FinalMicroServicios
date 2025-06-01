using Xunit;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using MicroservicioFasecolda.Data;
using MicroservicioFasecolda.Models;
using MicroservicioFasecolda.Services;

namespace MicroservicioFasecolda.Tests.Services
{
    public class AccidenteServiceTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // aislado por prueba
                .Options;

            var context = new AppDbContext(options);

            context.Accidentes.AddRange(
                new Accidente { Placa = "abc123", Fecha = DateTime.Now, Severidad = "muertos" },
                new Accidente { Placa = "abc123", Fecha = DateTime.Now, Severidad = "solo latas" },
                new Accidente { Placa = "xyz789", Fecha = DateTime.Now, Severidad = "heridos" }
            );

            context.SaveChanges();
            return context;
        }

        [Fact]
        public void ObtenerPorPlaca_DebeRetornarAccidentesCorrectos()
        {
            // Arrange
            var context = GetDbContext();
            var service = new AccidenteService(context);

            // Act
            var resultado = service.ObtenerPorPlaca("abc123").ToList();

            // Assert
            Assert.Equal(2, resultado.Count);
            Assert.All(resultado, a => Assert.Equal("abc123", a.Placa));
        }

        [Fact]
        public void ObtenerPorPlaca_SiNoExisteRetornaListaVacia()
        {
            var context = GetDbContext();
            var service = new AccidenteService(context);

            var resultado = service.ObtenerPorPlaca("noexiste");

            Assert.Empty(resultado);
        }
    }
}
