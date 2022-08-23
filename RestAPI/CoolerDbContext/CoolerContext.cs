using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using RestAPI.Model;

namespace RestAPI.CoolerDbContext
{
    public class CoolerContext : DbContext
    {
        public DbSet<Cooler> Coolers { get; set; }
        public CoolerContext(DbContextOptions<CoolerContext> options) : base(options)
        {

        }
    }
}
