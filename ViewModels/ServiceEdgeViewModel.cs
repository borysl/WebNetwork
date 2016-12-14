using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebNetwork.ViewModels
{
    public class ServiceEdgeViewModel
    {
        public int Id { get; set; }
        public int Source { get; set; }
        public int Target { get; set; }
        public string Name { get; set; }
    }
}
