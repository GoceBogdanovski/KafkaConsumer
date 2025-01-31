using KafkaConsumer.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace KafkaConsumer.DAL
{
    public class EntityDbContext : DbContext
    {
        public EntityDbContext(DbContextOptions<EntityDbContext> options) : base(options) { }

        public DbSet<Shape> Shapes { get; set; }
        public DbSet<Material> Materials { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("InMemoryDb");
        }
    }
}
