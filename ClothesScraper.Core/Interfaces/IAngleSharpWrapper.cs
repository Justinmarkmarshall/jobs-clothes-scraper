using AngleSharp.Dom;
using AngleSharp.Io;
using ClothesScraper.Core.Wrappers;

namespace ClothesScraper.Core.Interfaces
{
    public interface IAngleSharpWrapper
    {
        public Task<IDocument> GetSearchResults(string url, IRequester requester = null);
        public Task<IDocument> OpenAsync(string url, RequesterWrapper requester);
    }
}