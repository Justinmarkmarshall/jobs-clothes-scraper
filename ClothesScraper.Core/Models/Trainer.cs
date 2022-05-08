using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesScraper.Core.Models
{
    public class Trainer
    {
        public string? Model { get; set; }

        public double Price { get; set; }

        public double SalePrice { get; set; }

        public List<string>? Images { get; set; }

        public string? Link { get; set; }

        public int Size { get; set; }
    }
}
