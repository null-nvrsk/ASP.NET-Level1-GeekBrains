using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Shop()
        {
            return View("Shop");
        }

        public IActionResult PorductDetails()
        {
            return View("PorductDetails");
        }

        public IActionResult Checkout()
        {
            return View("Checkout");
        }

        public IActionResult Cart()
        {
            return View("Cart");
        }
        public IActionResult Login()
        {
            return View("Login");
        }

        public IActionResult Blog()
        {
            return View("Blog");
        }
        
        public IActionResult BlogSingle()
        {
            return View("BlogSingle");
        }    

        public IActionResult ContactUs()
        {
            return View("ContactUs");
        }             
        
        public IActionResult NotFound404()
        {
            return View("NotFound404");
        }


    }
}