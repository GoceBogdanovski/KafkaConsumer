using System.ComponentModel.DataAnnotations;

namespace KafkaConsumer.DAL.Entities
{
    public class Material
    {
        [Key]
        [Required]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
    }
}
