using Microsoft.AspNetCore.Mvc;
using prjToDoList.Models;
using prjToDoList.ViewModels;
using System.Diagnostics;
using System.Text.Json;

namespace prjToDoList.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly demoDBContext _db;

        public HomeController(ILogger<HomeController> logger, demoDBContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.Keys.Contains("Login"))
            {
                return View();
            }
            return RedirectToAction("Login");

        }

        public IActionResult Login()
        {
            if (HttpContext.Session.Keys.Contains("Login"))
            {
                string? jsonStr = HttpContext.Session.GetString("Login");
                if (jsonStr != null)
                {
                    tUser? loginUser = JsonSerializer.Deserialize<tUser>(jsonStr);
                    return RedirectToAction("List", "Todo");
                    // todo 有登入 Session 轉導至該 user to do list 畫面
                }

            }
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel login)
        {
            if (String.IsNullOrEmpty(login.loginAccount) || String.IsNullOrEmpty(login.loginPassword))
            {
                return View();
            }

            try
            {
                tUser? user = _db.tUsers.FirstOrDefault(u => u.fEmail.Equals(login.loginAccount)
                                && u.fPassword.Equals(CommonFn.ComputeSHA256Hash(login.loginPassword)));
                if (user != null)
                {
                    string jsonStr = "";
                    jsonStr = JsonSerializer.Serialize(user);
                    HttpContext.Session.SetString("Login", jsonStr);

                    _logger.LogInformation(user.fUserName + " has logging in the service");
                    return RedirectToAction("List", "Todo");
                }
            }catch(Exception ex) {

                _logger.LogError(ex, "An error occurred while processing the request.");
                return BadRequest("Web server error: " + ex.Message);
            }

            return View();

        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Login");
            return RedirectToAction("Login");
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