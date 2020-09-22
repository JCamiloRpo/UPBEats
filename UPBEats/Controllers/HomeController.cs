using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
        private static Usuario usuario; //Variable para saber los datos del usuario
        private static bool registro = false; //Variable para saber cuando el usuario esta registrado en la plataforma
        private static bool ingreso = false; //Variable para saber cuando el usuario ingresó en la plataforma
        //Accesores de la variable ingreso
        public static bool getIngreso { get => ingreso; }
        public static void setIngreso(bool val)
        {
            ingreso = val;
        }
        //Accesores de la variable registro
        public static bool getRegistro { get => registro; }
        public static void setRegistro(bool val)
        {
            registro = val;
        }
        //Accesores de la variable usuario
        public static Usuario getUsuario { get => usuario; }
        public static void setUsuario(Usuario val)
        {
            usuario = val;
        }

        public HomeController(UPBEatsContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            bool ingreso = getIngreso, registro = getRegistro;
            //Si el usuario ya ingresó y esta registrado
            if (ingreso && registro)//Codigo que requiere la pagina en especifico
            {
                IdUsuario();//Buscar el Id del usuario ingresado
                return View();
            }
            //Si ingresó y no se ha registrado
            else if (ingreso)
            {
                setIngreso(false);
            }
            return RedirectToAction("Principal", "Home");
        }

        public IActionResult About()
        {
            bool ingreso = getIngreso, registro = getRegistro;
            //Si el usuario ya ingresó y esta registrado
            if (ingreso && registro)
            {
                ViewData["Message"] = "Your application description page.";
                return View();
            }
            //Si ingresó y no se ha registrado
            else if (ingreso)
            {
                setIngreso(false);
            }
            return RedirectToAction("Principal", "Home");
        }

        public IActionResult Contact()
        {
            bool ingreso = getIngreso, registro = getRegistro;
            //Si el usuario ya ingresó y esta registrado
            if (ingreso && registro)
            {
                ViewData["Message"] = "Your contact page.";
                return View();
            }
            //Si ingresó y no se ha registrado
            else if (ingreso)
            {
                setIngreso(false);
            }
            return RedirectToAction("Principal", "Home");
        }

        public IActionResult Privacy()
        {
            bool ingreso = getIngreso, registro = getRegistro;
            //Si el usuario ya ingresó y esta registrado
            if (ingreso && registro)
            {
                return View();
            }
            //Si ingresó y no se ha registrado
            else if (ingreso)
            {
                setIngreso(false);
            }
            return RedirectToAction("Principal", "Home");
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
            bool ingreso = getIngreso, registro = getRegistro;
            //Si el usuario tiene la sesion iniciada ingresa a la pagina de inicio
            if (ingreso)
            {
                //Validar si ya está registrado
                var usuario = _context.Usuario.FirstOrDefault(m => m.Correo == User.Identity.Name);
                if (usuario == null)
                {
                    setRegistro(false);
                    return RedirectToAction("Create", "Usuarios");
                }


                setRegistro(true);
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public void Ingresar()
        {
            setIngreso(true);
        }

        [HttpPost]
        public void Salir()
        {
            setIngreso(false);
        }

        private void IdUsuario()
        {
            var usuario = _context.Usuario.FirstOrDefault(m => m.Correo == User.Identity.Name);
            if (usuario != null)
            {
                setUsuario(usuario);
            }
        }

    }
}
