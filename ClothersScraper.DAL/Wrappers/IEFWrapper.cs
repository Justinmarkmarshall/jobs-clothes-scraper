using ClothersScraper.DAL.Dtos;

namespace ClothersScraper.DAL.Wrappers
{
    public interface IEFWrapper
    {
        Task SaveToDB(Audit audit);
        Task SaveUniqueToDB(List<Garment> garments);
    }
}