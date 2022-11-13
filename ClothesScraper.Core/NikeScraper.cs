using AngleSharp.Html.Dom;
using ClothersScraper.DAL.Wrappers;
using ClothesScraper.Core.Interfaces;
using ClothesScraper.Core.Mappers;
using ClothesScraper.Core.Models;
using System.Text;

namespace ClothesScraper.Core
{
    public class NikeScraper : INikeScraper
    {
        IAngleSharpWrapper _angleSharpWrapper;
        IEFWrapper _efWrapper;

        public NikeScraper(IAngleSharpWrapper angleSharpWrapper, IEFWrapper efWrapper)
        {
            _efWrapper = efWrapper;
            _angleSharpWrapper = angleSharpWrapper;
        }

        public async Task<List<Trainer>> GetSaleTrainersResponseFromNike(int size = 11)
        {
            var trainers = new List<Trainer>();
            string url = $"https://www.nike.com/gb/w/mens-sale-shoes-1m67gz3yaepznik1zy7ok";
            var document = await _angleSharpWrapper.GetSearchResults(url);
            var searchResults = document.GetElementsByClassName("product-grid__items");           
            if (searchResults.Any())
            {               
                trainers = searchResults.Map();
            }
            await _efWrapper.SaveUniqueToDB(trainers.Map());
            return trainers;

        }

        public async Task<List<Trainer>> GetSaleClothesResponseFromNike()
        {
            var trainers = new List<Trainer>();
            string url = $"https://www.nike.com/gb/w/mens-sale-clothing-3yaepz5ewkdz6ymx6znik1";
            var document = await _angleSharpWrapper.GetSearchResults(url);
            var searchResults = document.GetElementsByClassName("product-grid__items");           
            if (searchResults.Any())
            {

                trainers = searchResults.Map();               
            }
            await _efWrapper.SaveUniqueToDB(trainers.Map());
            return trainers;
        }

        public async Task<List<Trainer>> GetSaleCottonClothesResponseFromNike()
        {
            var trainers = new List<Trainer>();
            string url = $"https://www.nike.com/gb/w/mens-sale-organic-cotton-clothing-1w47rz3yaepz5ewkdz6ymx6znik1";
            var document = await _angleSharpWrapper.GetSearchResults(url);
            var searchResults = document.GetElementsByClassName("product-grid__items");           
            if (searchResults.Any())
            {
                trainers = searchResults.Map();
            }
            await _efWrapper.SaveUniqueToDB(trainers.Map());
            return trainers;
        }
    }
}