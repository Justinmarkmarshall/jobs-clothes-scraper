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

            string url = $"https://www.nike.com/gb/w/mens-sale-shoes-3yaepznik1zy7ok";

            var document = await _angleSharpWrapper.GetSearchResults(url);

            var searchResults = document.GetElementsByClassName("product-grid__items");
            //var searchResults = document.GetElementsByClassName("product-grid__body");

            if (searchResults.Any())
            {
                await _efWrapper.SaveToDB(new ClothersScraper.DAL.Dtos.Audit() { Date = DateTime.Now });
                trainers = searchResults.Map();

                Dictionary<string, bool> stockCheck = new Dictionary<string, bool>();

                //SIZE CHECKER IS BROKEN, TO USE WITHOUT JUST TAKE OUT THE STOCK CHECK LOGIC
                foreach (var trainer in trainers)
                {
                    if (trainer.Link != null)
                    {
                        Dictionary<double, bool> sizeCheck = new Dictionary<double, bool>();

                        var sizeSearchResults = await _angleSharpWrapper.GetSearchResults(trainer.Link);

                        //not sure why these don't return when scraping the live site, they are present in the html.
                        //possibly more to traverse, and in the test doc I only have a snippet of html.
                        //but tried putting the whole html from the first page in, works in tests but not on live
                        var labels = sizeSearchResults.GetElementsByClassName("css-xf3ahq");
                        var inputs = sizeSearchResults.GetElementsByClassName("visually-hidden");

                        bool available = false;
                        double siz = 0.0F;

                        for (int i = 0; i < inputs.Count(); i++)
                        {
                            var input = inputs[i] as IHtmlInputElement;
                            available = !input.IsDisabled;

                            StringBuilder text = new StringBuilder(labels[i].InnerHtml);
                            text.Replace("UK ", "");
                            if (text.Length > 3)
                            {
                                text.Remove(3, text.Length-4);
                            }
                            text.Replace("()", "");
                            //siz = Convert.ToDouble(text.Replace("UK ", ""));
                            siz = Convert.ToDouble(text.ToString());

                            sizeCheck[siz] = available;

                            //if (siz == size) break;
                        }
                        
                        if (sizeCheck.ContainsKey(size))
                        {
                            stockCheck[trainer.Link] = sizeCheck[size];
                        }
                        else
                        {
                            stockCheck[trainer.Link] = false;
                        }                    
                    }
                   
                }

                await _efWrapper.SaveUniqueToDB(trainers.Map(stockCheck));

                //foreach (var trainer in searchResults[0].Children)
                //{
                //    var body = trainer.GetElementsByClassName("product-card__info")[0];

                //    var pictures = trainer.GetElementsByTagName("img");
                //    var images = new List<string>();
                //    foreach (IHtmlImageElement pic in pictures)
                //    {
                //        images.Add(!String.IsNullOrWhiteSpace(pic.Source) ? pic.Source : "");
                //    }
                //    var anchortags = trainer.GetElementsByTagName("a")[0] as IHtmlAnchorElement;
                //    var link = anchortags?.Href;
                //    var price = body.GetElementsByClassName("product-price is--current-price")[0].InnerHtml;
                //    var reducedFrom = body.GetElementsByClassName("product-price is--striked-out")[0].InnerHtml;
                //    var trainerName = body.GetElementsByClassName("product-card__title")[0].InnerHtml;

                //}
            }

            return trainers;

        }
    }
}