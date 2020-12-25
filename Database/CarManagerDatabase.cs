using Microsoft.EntityFrameworkCore;
using automanager.Models.Database;

namespace automanager.Database 
{
    public class CarManagerDatabase : DbContext
    {
        public DbSet<Vehicle> Vehicles { get; set; }

        public DbSet<GasFillingEntry> FillingEntries { get; set; }

        public CarManagerDatabase(DbContextOptions<CarManagerDatabase> options)
            :base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);
        }
    }
}
