using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace WebNetwork.Models
{
    public class NetworkContext : DbContext
    {
        private readonly IConfigurationRoot _config;

        public NetworkContext(IConfigurationRoot config)
        {
            _config = config;
        }

        public DbSet<Asset> Assets { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceLayer> ServiceLayers { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<AssetPosition> AssetPosition { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseNpgsql(
                _config["ConnectionString"]);
        }
    }
}
