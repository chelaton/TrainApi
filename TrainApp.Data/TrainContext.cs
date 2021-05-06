using Microsoft.EntityFrameworkCore;


namespace TrainApp.Data
{
    public class TrainContext : DbContext
    {
        public TrainContext(DbContextOptions<TrainContext> options)
        : base(options)
        {

        }
        public DbSet<Entities.Train> Trains { get; set; }
        public DbSet<Entities.Wagon> Wagons { get; set; }
        public DbSet<Entities.Chair> Chairs { get; set; }

    }
}
