using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WEB_CORE.Models;
using WEB_CORE.Repository.IRepository;

namespace WEB_CORE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IAccountRepository _accRepo;
        public HomeController(ILogger<HomeController> logger, IHttpClientFactory clientFactory, IAccountRepository accRepo)
        {
            _logger = logger;
            _clientFactory = clientFactory;
            _accRepo = accRepo;
        }

        //public async Task<IActionResult> Index()
        //{
        //    var request = new HttpRequestMessage(HttpMethod.Get, SD.NationalParkAPIPath);
        //    var client = _clientFactory.CreateClient();
        //    HttpResponseMessage response = await client.SendAsync(request);
        //    if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //    {
        //        var jsonString = await response.Content.ReadAsStringAsync();
        //        var obj = JsonConvert.DeserializeObject<IEnumerable<NationalPark>>(jsonString);
        //        return Json(obj);
        //    }

        //    return Json(new NationalPark());

        //}

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            User obj = new User();
            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(User obj)
        {
            User objUser = await _accRepo.LoginAsync(SD.AccountAPIPath, obj);
            if(obj.Token ==null)
            {
                return View();
            }
            HttpContext.Session.SetString("JWToken", objUser.Token);
            return RedirectToAction("Home");
        }

    }
}
