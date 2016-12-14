﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebNetwork.Models
{
    [Table("ntw_asset")]
    public class Asset
    {
        [Column("name")]
        public string Name { get; set; }

        [Column("asset_id")]
        public int Id { get; set; }

        [NotMapped]
        public double X { get; set; }

        [NotMapped]
        public double Y { get; set; }

        [NotMapped]
        public double Latitude { get; set; }

        [NotMapped]
        public double Longitude { get; set; }

        [Column("site_id")]
        public int SiteId { get; set; }

        [NotMapped]
        public Site Site { get; set; }

        [NotMapped]
        public AssetPosition AssetPosition { get; set; }
    }
}