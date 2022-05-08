using System.ComponentModel.DataAnnotations;

namespace ClothersScraper.DAL.Dtos
{
    public class Audit
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
    }
}
