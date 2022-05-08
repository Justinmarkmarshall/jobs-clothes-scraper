using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClothersScraper.DAL.Dtos
{
    [Table("Garment")]
    public class Garment
    {
        [Key]
        public int Id { get; set; }
        public string? Model { get; set; }
        public double Price { get; set; }
        public string? Images { get; set; }
        public string? Link { get; set; }
        public int Size { get; set; }
        public DateTime Date { get; set; }
    }
}
