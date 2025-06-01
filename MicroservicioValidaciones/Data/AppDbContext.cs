using Microsoft.EntityFrameworkCore;
using MicroservicioValidaciones.Models;

namespace MicroservicioValidaciones.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Cotizacion> Cotizaciones => Set<Cotizacion>();
    }
}
