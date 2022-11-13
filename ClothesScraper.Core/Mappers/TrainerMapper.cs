using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using ClothersScraper.DAL.Dtos;
using ClothesScraper.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesScraper.Core.Mappers
{
    public static class TrainerMapper
    {
        public static List<Trainer> Map(this IHtmlCollection<IElement> trainersDiv)
        {
            var lstReturn = new List<Trainer>();

            foreach (var trainer in trainersDiv[0].Children)
            {
                if (trainer.GetElementsByClassName("product-card__info").Any())
                {
                    var body = trainer.GetElementsByClassName("product-card__info")[0];
                    var link = "";
                    if (trainer.GetElementsByTagName("a").Any())
                    {
                        var anchor = trainer.GetElementsByTagName("a")[0] as IHtmlAnchorElement;
                        link = anchor?.Href;
                    }
                    lstReturn.Add(new Trainer()
                    {
                        Model = body.GetElementsByClassName("product-card__title")[0].InnerHtml,
                        Price = Convert.ToDouble(body.GetElementsByClassName("product-price is--striked-out")[0].InnerHtml.Replace("£", "")),
                        SalePrice = Convert.ToDouble(body.GetElementsByClassName("product-price is--current-price")[0].InnerHtml.Replace("£", "")),
                        Images = trainer.GetElementsByTagName("img").MapImages(),
                        Link = link
                    });
                };
            }

            return lstReturn;
            //var pictures = trainer.GetElementsByTagName("img");
            //var images = new List<string>();
            //foreach (IHtmlImageElement pic in pictures)
            //{
            //    images.Add(!String.IsNullOrWhiteSpace(pic.Source) ? pic.Source : "");
            //}
            //var anchortags = trainer.GetElementsByTagName("a")[0] as IHtmlAnchorElement;
            //var link = anchortags?.Href;
            //var price = body.GetElementsByClassName("product-price is--current-price")[0].InnerHtml;
            //var reducedFrom = body.GetElementsByClassName("product-price is--striked-out")[0].InnerHtml;
            //var trainerName = body.GetElementsByClassName("product-card__title")[0].InnerHtml;
        }

        public static List<Garment> Map(this List<Trainer> trainers)
        {
            var garments = new List<Garment>();

            foreach (var trainer in trainers)
            {
                var pic = "";

                if (trainer.Images.Any() && trainer.Images.Count > 1)
                {
                    pic = trainer.Images[1];
                }
                garments.Add(new Garment()
                {
                    Images = pic,
                    Price = trainer.SalePrice,
                    Model = trainer.Model,
                    Link = trainer.Link,
                    Date = DateTime.Now,
                });

            }

            return garments;
        }
        private static List<string> MapImages(this IHtmlCollection<IElement> pics)
        {
            var images = new List<string>();
            foreach (IHtmlImageElement pic in pics)
            {
                images.Add(!String.IsNullOrWhiteSpace(pic.Source) ? pic.Source : "");
            }
            return images;
        }

        private static string MapLinks(this IHtmlAnchorElement anchor)
        {
            return anchor.Href;
        }
    }
}
