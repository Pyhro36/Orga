using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Orga.Models;

namespace Orga.Controllers {
    public class HomeController : Controller {

        public int MyProperty { get; set; }
        private readonly ILogger<HomeController> _logger;

        private readonly IConfiguration _configuration;

        public HomeController (ILogger<HomeController> logger, IConfiguration configuration) {
            _configuration = configuration;
            _logger = logger;
            MyProperty = 2;
        }

        public IActionResult Index () {
            return View (_configuration.GetValue<string>("ASPNETCORE_ENVIRONMENT"));
        }
        
        public IActionResult Privacy () {
            return View ();
        }

        [ResponseCache (Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error () {
            return View (new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}