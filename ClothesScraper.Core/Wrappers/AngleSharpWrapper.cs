using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Io;
using ClothesScraper.Core.Interfaces;

namespace ClothesScraper.Core.Wrappers
{
    public class AngleSharpWrapper : IAngleSharpWrapper
    {
        public async Task<IDocument> GetSearchResults(string url, IRequester requester = null)
        {
            if (requester == null)
            {
                requester = new DefaultHttpRequester();
            }

            var config = Configuration.Default.WithDefaultLoader().With(requester);
            var context = BrowsingContext.New(config);

            return await context.OpenAsync(url);
        }

        public async Task<IDocument> OpenAsync(string url, RequesterWrapper requester)
        {
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);

            return await context.OpenAsync(url);

            //return document.GetElementsByClassName("css-1anhqz4-ListingsContainer earci3d2");
        }
    }
}
