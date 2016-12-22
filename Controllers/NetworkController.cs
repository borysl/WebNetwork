using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
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
        private readonly IConfigurationRoot _config;

        public class Rect
        {
            public Rect(double x1, double y1, double x2, double y2)
            {
                X1 = x1;
                Y1 = y1;
                X2 = x2;
                Y2 = y2;
            }

            public double X1 { get; set; }
            public double Y1 { get; set; }
            public double X2 { get; set; }
            public double Y2 { get; set; }
        }

        public NetworkController(NetworkContext context, IConfigurationRoot config)
        {
            _context = context;
            _config = config;
        }

        [HttpGet("graph")]
        public IActionResult GetGraph()
        {
            try
            {
                var graphVm = RetrieveGraph(null);
                return Ok(graphVm);
            }
            catch (Exception)
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet("graph/{x1:double}:{y1:double}x{x2:double}:{y2:double}")]
        public IActionResult GetGraph(double x1, double y1, double x2, double y2)
        {
            try
            {
                var rect = new Rect(x1, y1, x2, y2);
                var graphVm = RetrieveGraph(rect);
                return Ok(graphVm);
            }
            catch (Exception)
            {
                return BadRequest(ModelState);
            }
        }

        private GraphViewModel RetrieveGraph(Rect rect)
        {
            IEnumerable<AssetNodeViewModel> assetsVm;
            IEnumerable<ServiceEdgeViewModel> servicesVm;
            if (rect != null)
            {
                assetsVm = _context.Assets.FromSql("select * from get_frame_assets(@p0, @p1, @p2, @p3, @p4)", 1, rect.X1,
                    rect.Y1, rect.X2, rect.Y2).ToList();
            }
            else
            {
                assetsVm = _context.Assets.FromSql("select * from get_frame_assets(@p0)", 1).ToList();
            }

            if (rect != null)
            {
                servicesVm = _context.Services.FromSql("select * from get_frame_services(@p0, @p1, @p2, @p3, @p4)", 1, rect.X1,
                    rect.Y1, rect.X2, rect.Y2).ToList();
            }
            else
            {
                servicesVm = _context.Services.FromSql("select * from get_frame_services(@p0)", 1).ToList();
            }

            return new GraphViewModel(assetsVm, servicesVm);
        }
    }
}
