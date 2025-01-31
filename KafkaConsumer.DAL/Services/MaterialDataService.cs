using KafkaPocCommon.DTOs;

namespace KafkaConsumer.DAL.Services
{
    public class MaterialDataService
    {
        private readonly EntityDbContext _entityDbContext;

        public MaterialDataService(EntityDbContext entityDbContext)
        {
            _entityDbContext = entityDbContext;
        }

        public async Task<bool> Save(long id, MaterialDto input)
        {
            _entityDbContext.Materials.Add(new Entities.Material
            {
                Id = id,
                Name = input.Name,
                Color = input.Color,
            });

            return await _entityDbContext.SaveChangesAsync() > 0;
        }
    }
}
