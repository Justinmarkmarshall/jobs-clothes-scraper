using ClothersScraper.DAL.Data;
using ClothersScraper.DAL.Wrappers;
using ClothesScraper.Core;
using ClothesScraper.Core.Interfaces;
using ClothesScraper.Core.Wrappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClothesScraper
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var serviceCollection = new ServiceCollection()
                .AddDbContext<DataContext>(options =>
                {
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                })
                .AddScoped<DbContext, DataContext>()
                .AddTransient<INikeScraper, NikeScraper>()
                .AddTransient<IAngleSharpWrapper, AngleSharpWrapper>()
                .AddTransient<IEFWrapper, EFWrapper>()

                .BuildServiceProvider();

            var nikeScraper = serviceCollection.GetService<INikeScraper>();

            var results = await nikeScraper.GetSaleTrainersResponseFromNike();
        }
    }
}