using ClothesScraper.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesScraper.Core.Interfaces
{
    public interface INikeScraper
    {
        public Task<List<Trainer>> GetSaleTrainersResponseFromNike(int size = 11);
    }
}
