using Microsoft.EntityFrameworkCore;
using MicroservicioFasecolda.Models;
using System.Collections.Generic;

namespace MicroservicioFasecolda.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Accidente> Accidentes => Set<Accidente>();
    }
}
