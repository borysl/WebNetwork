using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebNetwork.Models
{
    [Table("sd_service_layer")]
    public class ServiceLayer
    {
        [Column("service_layer_id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("level")]
        public int Level { get; set; }
        [Column("description")]
        public string Description { get; set; }
    }
}
