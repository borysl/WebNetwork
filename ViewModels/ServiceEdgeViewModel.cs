using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebNetwork.ViewModels
{
    public class ServiceEdgeViewModel
    {
        [Column("service_id")]
        public int Id { get; set; }
        [Column("input_asset_id")]
        public int Source { get; set; }
        [Column("output_asset_id")]
        public int Target { get; set; }
        [Column("name")]
        public string Label { get; set; }
    }
}
