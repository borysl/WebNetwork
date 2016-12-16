using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebNetwork.ViewModels
{
    public class AssetNodeViewModel
    {
        [Column("name")]
        public string Label { get; set; }
        [Column("asset_id")]
        public int Id { get; set; }
        [Column("x")]
        public double X { get; set; }
        [Column("y")]
        public double Y { get; set; }
        [NotMapped]
        public int Size { get; set; } = 5;
        [NotMapped]
        public string Type { get; set; } = "square";
    }
}
