using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebNetwork.Models;
using WebNetwork.ViewModels;

namespace WebNetwork.Controllers
{
    /// <summary>
    /// Network controller that creates 
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Route("network")]
    public class NetworkController : Controller
    {
        private readonly NetworkContext _context;

        private class Rect
        {
            public Rect(double xTopLeft, double yTopLeft, double xBottomRight, double yBottomRight)
            {
                XTopLeft = xTopLeft;
                YTopLeft = yTopLeft;
                XBottomRight = xBottomRight;
                YBottomRight = yBottomRight;
            }

            public double XTopLeft { get; }
            public double YTopLeft { get; }
            public double XBottomRight { get; }
            public double YBottomRight { get; }
        }

        /// <summary>
        /// Instanciate Network controller
        /// </summary>
        /// <param name="context">DB context for Network</param>
        /// <param name="config">Configuration class to fetch settings</param>
        public NetworkController(NetworkContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get entire graph with a single call
        /// </summary>
        /// <returns></returns>
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


        /// <summary>
        /// Get frame of the network.
        /// </summary>
        /// <param name="xTopLeft">Coordinate X of the top left point.</param>
        /// <param name="yTopLeft">Coordinate Y of the top left point.</param>
        /// <param name="xBottomRight">Corrdinate X of the bottom right point.</param>
        /// <param name="yBottomRight">Coordinate Y of the bottom right point.</param>
        /// <returns></returns>
        [HttpGet("graph/{xTopLeft:double}:{yTopLeft:double}x{xBottomRight:double}:{yBottomRight:double}")]
        public IActionResult GetGraph(double xTopLeft, double yTopLeft, double xBottomRight, double yBottomRight)
        {
            try
            {
                var rect = new Rect(xTopLeft, yTopLeft, xBottomRight, yBottomRight);
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
                assetsVm = _context.Assets.FromSql("select * from get_frame_assets(@p0, @p1, @p2, @p3, @p4)", 1, rect.XTopLeft,
                    rect.YTopLeft, rect.XBottomRight, rect.YBottomRight).ToList();
            }
            else
            {
                assetsVm = _context.Assets.FromSql("select * from get_frame_assets(@p0)", 1).ToList();
            }

            if (rect != null)
            {
                servicesVm = _context.Services.FromSql("select * from get_frame_services(@p0, @p1, @p2, @p3, @p4)", 1, rect.XTopLeft,
                    rect.YTopLeft, rect.XBottomRight, rect.YBottomRight).ToList();
            }
            else
            {
                servicesVm = _context.Services.FromSql("select * from get_frame_services(@p0)", 1).ToList();
            }

            return new GraphViewModel(assetsVm, servicesVm);
        }
    }
}
