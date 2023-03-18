using Kiwify.Core.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kiwify.Core.Data
{
    public class DataContext : DbContext
    {
        protected DataContext()
        {
        }

        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
    }
}
