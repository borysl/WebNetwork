using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebNetwork.ViewModels
{
    public class GraphViewModel
    {
        private readonly IEnumerable<AssetNodeViewModel> _assetsVm;
        private readonly IEnumerable<ServiceEdgeViewModel> _servicesVm;

        public GraphViewModel(IEnumerable<AssetNodeViewModel> assetsVm, IEnumerable<ServiceEdgeViewModel> servicesVm)
        {
            _assetsVm = assetsVm;
            _servicesVm = servicesVm;
        }

        public IEnumerable<AssetNodeViewModel> Nodes => _assetsVm;

        public IEnumerable<ServiceEdgeViewModel> Edges => _servicesVm;
    }
}
