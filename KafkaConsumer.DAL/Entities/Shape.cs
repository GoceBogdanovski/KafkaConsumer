using System.ComponentModel.DataAnnotations;

namespace KafkaConsumer.DAL.Entities
{
    public class Shape
    {
        [Key]
        [Required]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public decimal Weight { get; set; }
    }
}
