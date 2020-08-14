using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UPBEats.Data;
using UPBEats.Models;

namespace UPBEats.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly UPBEatsContext _context;
        public static bool registro=false;

        public HomeController(UPBEatsContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [AllowAnonymous]
        public IActionResult Principal()
        {
            //Si el usuario tiene la sesion iniciada ingresa a la pagina de inicio
            if (User.Identity.IsAuthenticated)
            {
                //Validar si ya está registrado
                var usuario = _context.Usuario.FirstOrDefault(m => m.Correo == User.Identity.Name);
                if (usuario == null)
                    return Redirect("Usuarios/Create");

                registro = true;
                return View("Index");
            }
            registro = false;
            return View();
        }
    }
}
