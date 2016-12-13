using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebNetwork.Models
{
    [Table("ntw_asset_position")]
    public class AssetPosition
    {
        public Asset Asset { get; set; }
        public ServiceLayer ServiceLayer { get; set; }
        [Column("x")]
        public double X { get; set; }
        [Column("y")]
        public double Y { get; set; }
    }
}
