using ClothersScraper.DAL.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ClothersScraper.DAL.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<Audit>? Audit { get; set; }

        public DbSet<Garment>? Garment { get; set; }
    }
}
