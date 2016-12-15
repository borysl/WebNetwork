using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public GraphViewModel RetrieveGraph(Rect rect)
        {
            IEnumerable<ServiceLayer> serviceLayers = _context.ServiceLayers.ToList();
            var serviceLayer = serviceLayers.Single(_ => _.Name == _config["ServiceLayer"]);

            IEnumerable<Asset> assets =
                _context.Assets.Include(_ => _.AssetPosition)
                    .Include(_ => _.Site);

            var assetFromServiceLayer = assets
                    .Where(_ => _.AssetPosition.ServiceLayerId == serviceLayer.Id);

            IEnumerable<Asset> assetsFromFrame;
            if (rect != null)
            {
                assetsFromFrame = assetFromServiceLayer.
                    Where(
                        _ =>
                            (rect.X1 <= _.AssetPosition.X) && (_.AssetPosition.X <= rect.X2) &&
                            (rect.Y1 <= _.AssetPosition.Y) && (_.AssetPosition.Y <= rect.Y2));
            }
            else
            {
                assetsFromFrame = assetFromServiceLayer;
            }

            var assetsVm = Mapper.Map<IEnumerable<AssetNodeViewModel>>(assetsFromFrame);

            var servicesFromServiceLayer = _context.Services.Include(_ => _.InputAsset).Include(_ => _.OutputAsset);
                //.Where(_ => _.InputAsset.AssetPosition.ServiceLayerId == serviceLayer.Id && _.OutputAsset.AssetPosition.ServiceLayerId == serviceLayer.Id);
            
            IEnumerable<Service> servicesFromFrame;

            if (rect != null)
            {
                servicesFromFrame = servicesFromServiceLayer
                    .Where(BuildContainsExpression<Service, Asset>(_ => _.InputAsset, assetsFromFrame));
            }
            else
            {
                servicesFromFrame = servicesFromServiceLayer.Where(_ => _.InputAsset.AssetPosition.ServiceLayerId == serviceLayer.Id);
            }
            
            var servicesVm = Mapper.Map<IEnumerable<ServiceEdgeViewModel>>(servicesFromFrame);

            return new GraphViewModel(assetsVm, servicesVm);
        }

        static Expression<Func<TElement, bool>> BuildContainsExpression<TElement, TValue>(
            Expression<Func<TElement, TValue>> valueSelector, IEnumerable<TValue> values)
        {
            if (null == valueSelector) { throw new ArgumentNullException("valueSelector"); }
            if (null == values) { throw new ArgumentNullException("values"); }
            ParameterExpression p = valueSelector.Parameters.Single();
            // p => valueSelector(p) == values[0] || valueSelector(p) == ...
            if (!values.Any())
            {
                return e => false;
            }
            var equals = values.Select(value => (Expression)Expression.Equal(valueSelector.Body, Expression.Constant(value, typeof(TValue))));
            var body = equals.Aggregate<Expression>((accumulate, equal) => Expression.Or(accumulate, equal));
            return Expression.Lambda<Func<TElement, bool>>(body, p);
        }
    }
}
