using AngleSharp.Dom;
using AngleSharp.Io;
using ClothersScraper.DAL.Dtos;
using ClothersScraper.DAL.Wrappers;
using ClothesScraper.Core;
using ClothesScraper.Core.Interfaces;
using ClothesScraper.Core.Models;
using ClothesScraper.Core.Wrappers;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ClothesScraper.Tests
{
    public class WhenScrapingNikeForTrainers
    {

        private string responseFromNike = "";
        private NikeScraper _nikeScraper;
        private List<Trainer> trainers = new List<Trainer>();
        private Mock<IAngleSharpWrapper>? _angleSharpWrapper;
        private AngleSharpWrapper _angleSharpWrap;
        private Mock<IEFWrapper>? _efWrapper;
        private IDocument nikeTrainersResponse;
        private IDocument nikeTrainerResponse;
        private const string nikeMenSaleShoesUrl = "https://www.nike.com/gb/w/mens-sale-shoes-3yaepznik1zy7ok";


        [SetUp]
        public void Setup()
        {
            _efWrapper = new Mock<IEFWrapper>();
            _angleSharpWrap = new AngleSharpWrapper();
            _angleSharpWrapper = new Mock<IAngleSharpWrapper>();
            _nikeScraper = new NikeScraper(_angleSharpWrapper.Object, _efWrapper.Object);
        }

        [Test]
        public async Task WhenScrapingNikeForTrainersShouldPopulateTrainersAndLogToAuditAndSaveToDB()
        {
            Given_response_from_Nike();
            await When_scraping_trainers_and_trainer_page();
            Should_save_unique_trainers_to_db_and_log_to_audit();
        }

        [Test]
        public async Task Given_specific_size_then_shoes_should_be_available_in_that_size()
        {
            Given_response_from_Nike();
            await When_scraping_trainers_and_trainer_page();
            Should_save_trainers_of_specific_size();
        }

        private void Should_save_trainers_of_specific_size()
        {
            Assert.Greater(trainers.Count, 1);
            Mock.Get(_efWrapper.Object).Verify(x => x.SaveToDB(It.IsAny<Audit>()), Times.AtLeastOnce);
            Mock.Get(_efWrapper.Object).Verify(x => x.SaveUniqueToDB(It.IsAny<List<Garment>>()), Times.AtLeastOnce);
        }

        private void Should_save_unique_trainers_to_db_and_log_to_audit()
        {
            Assert.Greater(trainers.Count, 1);
            Mock.Get(_efWrapper.Object).Verify(x => x.SaveToDB(It.IsAny<Audit>()), Times.AtLeastOnce);
            Mock.Get(_efWrapper.Object).Verify(x => x.SaveUniqueToDB(It.IsAny<List<Garment>>()), Times.AtLeastOnce);
        }

        private async void Given_response_from_Nike()
        {
            nikeTrainersResponse = await GetFakeDocument(DocType.NikeTrainersResponse);
        }

        private async Task When_scraping_trainers_and_trainer_page()
        {
            _angleSharpWrapper?.Setup(r => r.GetSearchResults(It.Is<string>(s => s == nikeMenSaleShoesUrl), It.IsAny<IRequester>()))
                .ReturnsAsync(nikeTrainersResponse);
            nikeTrainerResponse = await GetFakeDocument(DocType.NikeTrainerResponse);
            _angleSharpWrapper?.Setup(r => r.GetSearchResults(It.Is<string>(s => s != nikeMenSaleShoesUrl), It.IsAny<IRequester>()))
                .ReturnsAsync(nikeTrainerResponse);
            trainers = await _nikeScraper.GetSaleTrainersResponseFromNike();

        }

        private async Task<IDocument> GetFakeDocument(DocType type)
        {
            var requesterMock = GetFakeRequesterMock(type);
            return await _angleSharpWrap.GetSearchResults("http://askjdkaj", requesterMock.Object);

        }

        private Mock<RequesterWrapper> GetFakeRequesterMock(DocType type)
        {
            var mockResponse = new Mock<IResponse>();
            mockResponse.Setup(x => x.Address).Returns(new Url("fakeAddress"));
            mockResponse.Setup(x => x.Headers).Returns(new Dictionary<string, string>());
            mockResponse.Setup(_ => _.Content).Returns(LoadFakeDocumentFromField(type));

            var mockFakeRequester = new Mock<RequesterWrapper>();
            mockFakeRequester.Setup(_ => _.RequestAsync(It.IsAny<Request>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockResponse.Object);
            mockFakeRequester.Setup(x => x.SupportsProtocol(It.IsAny<string>())).Returns(true);
            return mockFakeRequester;
        }

        private Stream LoadFakeDocumentFromField(DocType type)
        {
            var doc = "";

            switch (type)
            {
                case DocType.NikeTrainersResponse:
                    doc = "HtmlDocData/NikeTrainersResponse.html";
                    break;
                case DocType.NikeTrainerResponse:
                    doc = "HtmlDocData/NikeTrainerResponse.html";
                    break;
                default:
                    break;
            }

            var address = Path.Combine(Directory.GetCurrentDirectory(), $"{doc}");

            using (FileStream fileStream = File.OpenRead(Path.Combine(Directory.GetCurrentDirectory(), $"{doc}")))
            {
                MemoryStream memoryStream = new MemoryStream();
                memoryStream.SetLength(fileStream.Length);
                fileStream.Read(memoryStream.GetBuffer(), 0, (int)fileStream.Length);

                return memoryStream;
            }
        }
    }
}