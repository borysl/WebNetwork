using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebNetwork.Models
{
    [Table("ntw_service")]
    public class Service
    {
        [Column("service_id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("input_asset_id")]
        public int InputAssetId { get; set; }
        [Column("output_asset_id")]
        public int OutputAssetId { get; set; }
        [Column("service_layer_id")]
        public int ServiceLayerId { get; set; }
    }
}
