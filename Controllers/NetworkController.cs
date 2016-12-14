using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
            IEnumerable<Asset> assets = _context.Assets.ToList();
            IEnumerable<Service> services = _context.Services.ToList();
            IEnumerable<AssetPosition> assetPositions = _context.AssetPosition.ToList();
            IEnumerable<ServiceLayer> serviceLayers = _context.ServiceLayers.ToList();

            var serviceLayer = serviceLayers.Single(_ => _.Name == _config["ServiceLayer"]);

            IEnumerable<Site> sites = _context.Sites.ToList();

            foreach (var asset in assets)
            {
                asset.AssetPosition = assetPositions.Single(_ => _.AssetId == asset.Id);
                asset.Site = sites.Single(_ => _.Id == asset.SiteId);
            }

            var assetsVm = Mapper.Map<IEnumerable<AssetNodeViewModel>>(assets.Where(_ => _.AssetPosition.ServiceLayerId == serviceLayer.Id)).ToList();
            var assetIds = assetsVm.Select(_ => _.Id);
            var servicesVm = Mapper.Map<IEnumerable<ServiceEdgeViewModel>>(services.Where(_ => assetIds.Contains(_.InputAssetId)));

            return Ok(new GraphViewModel(assetsVm, servicesVm));
        }
    }
}
