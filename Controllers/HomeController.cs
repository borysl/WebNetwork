using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebNetwork.ViewModels;

namespace WebNetwork.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Sigma()
        {
            ViewData["Message"] = "Use Sigma.js package.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
