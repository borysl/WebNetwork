using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebNetwork.Models
{
    // [Table("ntw_service")]
    public class Service
    {
        [Column("service_id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        public Asset InputAsset { get; set; }
        public Asset OutputAsset { get; set; }
    }
}
