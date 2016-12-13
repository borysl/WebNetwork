using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using WebNetwork.Models;
using WebNetwork.ViewModels;

namespace WebNetwork.Controllers
{
    [Route("network")]
    public class NetworkController : Controller
    {
        private readonly NetworkContext _context;

        public NetworkController(NetworkContext context)
        {
            _context = context;
        }

        [HttpGet("graph")]
        public IActionResult GetGraph()
        {
            IEnumerable<Asset> assets = _context.Assets.ToList();
            IEnumerable<Service> services = _context.Services.ToList();

            var assetsVm = Mapper.Map<IEnumerable<AssetNodeViewModel>>(assets);
            var servicesVm = Mapper.Map<IEnumerable<ServiceEdgeViewModel>>(services);

            return Ok(new GraphViewModel(assetsVm, servicesVm));
        }
    }
}
