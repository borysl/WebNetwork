using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebNetwork.Models
{
    // [Table("sd_site")]
    public class Site
    {
        [Column("site_id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("trigram")]
        public string Trigram { get; set; }
        [Column("gps_x")]
        public double GpsX { get; set; }
        [Column("gps_y")]
        public double GpsY { get; set; }
    }
}
