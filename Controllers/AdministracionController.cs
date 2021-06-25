using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SIS_QSF.Models;

namespace SIS_QSF.Controllers
{
    public class AdministracionController : Controller
    {
        private readonly ILogger<AdministracionController> _logger;

        public AdministracionController(ILogger<AdministracionController> logger)
        {
            _logger = logger;
        }

        public IActionResult Solicitudes()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
