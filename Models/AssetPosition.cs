using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebNetwork.Models
{
    [Table("ntw_asset_position")]
    public class AssetPosition
    {
        [Key, ForeignKey("Asset"), Column("asset_id")]
        public int AssetId { get; set; }
        [Column("service_layer_id")]
        public int ServiceLayerId { get; set; }
        [Column("x")]
        public double X { get; set; }
        [Column("y")]
        public double Y { get; set; }
    }
}
