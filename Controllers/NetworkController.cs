using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using WebNetwork.Models;
using WebNetwork.ViewModels;

namespace WebNetwork.Controllers
{
    [Route("network")]
    public class NetworkController : Controller
    {
        private readonly NetworkContext _context;
        private IConfigurationRoot _config;

        public NetworkController(NetworkContext context, IConfigurationRoot config)
        {
            _context = context;
            _config = config;
        }

        [HttpGet("graph")]
        public IActionResult GetGraph()
        {
            IEnumerable<ServiceLayer> serviceLayers = _context.ServiceLayers.ToList();
            var serviceLayer = serviceLayers.Single(_ => _.Name == _config["ServiceLayer"]);

            IEnumerable<Asset> assets =
                _context.Assets.Include(_ => _.AssetPosition)
                    .Include(_ => _.Site);

            IEnumerable<Asset> assetFromServiceLayer = assets
                    .Where(_ => _.AssetPosition.ServiceLayerId == serviceLayer.Id);

            IEnumerable<Asset> assetFromFrame = assetFromServiceLayer.
                Where(_ => (-1000 < _.AssetPosition.X) && (_.AssetPosition.X <= 3000) && (-1000 < _.AssetPosition.Y) && (_.AssetPosition.Y <= 3000));

            var assetsVm = Mapper.Map<IEnumerable<AssetNodeViewModel>>(assetFromFrame).ToList();
            var assetIds = assetsVm.Select(_ => _.Id);

            IEnumerable<Service> services = _context.Services.ToList();

            var servicesVm = Mapper.Map<IEnumerable<ServiceEdgeViewModel>>(services.Where(_ => assetIds.Contains(_.InputAssetId)));

            return Ok(new GraphViewModel(assetsVm, servicesVm));
        }
    }
}
