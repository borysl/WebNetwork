using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebNetwork.Models
{
    public class NetworkContext : DbContext
    {
        public NetworkContext()
        {
            
        }

        public DbSet<Asset> Assets { get; set; }
        public DbSet<Service> Services { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseNpgsql(
                "User ID=postgres;Password=123qwe,./;Server=localhost;Port=5435;Database=nms_5000_a;Pooling=true;");
        }
    }
}
