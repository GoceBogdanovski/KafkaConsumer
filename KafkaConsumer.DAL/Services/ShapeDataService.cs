using KafkaPocCommon.DTOs;

namespace KafkaConsumer.DAL.Services
{
    public class ShapeDataService
    {
        private readonly EntityDbContext _entityDbContext;

        public ShapeDataService(EntityDbContext entityDbContext)
        {
            _entityDbContext = entityDbContext;
        }

        public async Task<bool> Save(long id, ShapeDto input)
        {
            _entityDbContext.Shapes.Add(new Entities.Shape
            {
                Id = id,
                Name = input.Name,
                Color = input.Color,
                Weight = input.Weight
            });

            return await _entityDbContext.SaveChangesAsync() > 0;
        }
    }
}
