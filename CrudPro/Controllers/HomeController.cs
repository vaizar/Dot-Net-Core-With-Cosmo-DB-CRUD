using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CrudPro.Models;
using CrudPro.Context;
using System.Net.Http;

namespace CrudPro.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<User> result = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44357/api");
                var postTask = client.GetAsync("api/Users/get");
                postTask.Wait();

                var res = postTask.Result;
                if (res.IsSuccessStatusCode)
                {
                    //res.Content.ReadAsStringAsync<User>();
                    return View();
                }
            }

            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return RedirectToPage("Error.cshtml");
        }

        public IActionResult Privacy()
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
