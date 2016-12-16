using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebNetwork.ViewModels;

namespace WebNetwork.Models
{
    public class NetworkContext : DbContext
    {
        private readonly IConfigurationRoot _config;

        public NetworkContext(IConfigurationRoot config)
        {
            _config = config;
        }

        public DbSet<AssetNodeViewModel> Assets { get; set; }
        public DbSet<ServiceEdgeViewModel> Services { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseNpgsql(
                _config["ConnectionString"]);
        }
    }
}
