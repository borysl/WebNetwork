using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebNetwork.ViewModels
{
    public class AssetNodeViewModel
    {
        public string Label { get; set; }
        public int Id { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public int Size { get; set; } = 5;
        public string Type { get; set; } = "square";
    }
}
