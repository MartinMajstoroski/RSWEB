﻿using Microsoft.AspNetCore.Mvc;

namespace ProektRS.Controllers
{
    public class HelloWorldController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
