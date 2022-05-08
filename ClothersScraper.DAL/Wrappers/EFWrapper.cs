using ClothersScraper.DAL.Data;
using ClothersScraper.DAL.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothersScraper.DAL.Wrappers
{
    public class EFWrapper : IEFWrapper
    {
        private DataContext _context;

        public EFWrapper(DataContext context)
        {
            _context = context;
        }

        public async Task SaveUniqueToDB(List<Garment> garments)
        {
            try
            {
                var unique = new List<Garment>();

                foreach (var garment in garments)
                {
                    if (!_context.Garment.Select(r => r.Link).ToList().Contains(garment.Link))
                    {
                        unique.Add(garment);
                    }
                }
                await _context.AddRangeAsync(unique);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
        }

        public async Task SaveToDB(Audit audit)
        {
            try
            {
                await _context.AddAsync(audit);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
