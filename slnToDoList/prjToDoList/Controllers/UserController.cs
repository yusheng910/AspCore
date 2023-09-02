using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using prjToDoList.Models;
using prjToDoList.ViewModels;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace prjToDoList.Controllers
{
    public class UserController : Controller
    {
        private readonly demoDBContext _db;
        public UserController(demoDBContext db)
        {
            _db = db;
        }

        public IActionResult List(KeywordViewModel kv)
        {
            string? keyword = kv.Keyword;
            IEnumerable<tUser>? users = null;
            if (string.IsNullOrEmpty(keyword))
            {
                users = from u in _db.tUsers
                        select u;
            }
            else
            {
                users = from u in _db.tUsers.Where(user => user.fUserName.Contains(keyword) ||
                        user.fEmail.Contains(keyword))
                        select u;
            }
            List<UserViewModel> userList = new List<UserViewModel>();
            foreach (tUser user in users)
            {
                userList.Add(new UserViewModel() { User = user });
            }

            return View(userList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(tUser userParam)
        {
            if (String.IsNullOrEmpty(userParam.fUserName) || String.IsNullOrEmpty(userParam.fPassword) || String.IsNullOrEmpty(userParam.fEmail))
            {
                return BadRequest("Something wrong with the information you provided.");
            }
            tUser? userSameMail = _db.tUsers.FirstOrDefault(u => u.fEmail == userParam.fEmail);
            tUser? userSameName = _db.tUsers.FirstOrDefault(u => u.fUserName == userParam.fUserName);

            if (userSameMail != null)
            {
                return BadRequest("The mail has already registered our service.");
            }
            if (userSameName != null)
            {
                return BadRequest("The user name has already been taken.");
            }


            string encryptedPwd = CommonFn.ComputeSHA256Hash(userParam.fPassword);
            userParam.fPassword = encryptedPwd;
            _db.Add(userParam);
            _db.SaveChanges();
            return RedirectToAction("Login", "Home");
        }

        public IActionResult Edit()
        {
            if (HttpContext.Session.Keys.Contains("Login"))
            {
                string? jsonStr = HttpContext.Session.GetString("Login");
                if (jsonStr != null)
                {
                    tUser? loginUser = JsonSerializer.Deserialize<tUser>(jsonStr);
                    if (loginUser != null)
                    {
                        return View(new UserViewModel() { User = loginUser });
                    }

                }
                return
                    RedirectToAction("Login", "Home");

            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        [HttpPost]
        public IActionResult Edit(UserViewModel userParam)
        {
            tUser? user = _db.tUsers.FirstOrDefault(u => u.fUserId == userParam.fUserId);
            if (user != null)
            {
                if (!String.IsNullOrEmpty(userParam.fUserName) || !String.IsNullOrEmpty(userParam.oldPassword) || !String.IsNullOrEmpty(userParam.fPassword))
                {
                    string oldPwdEncoded = CommonFn.ComputeSHA256Hash(userParam.oldPassword);
                    if (oldPwdEncoded.Equals(user.fPassword))
                    {
                        user.fUserName = userParam.fUserName;
                        user.fPassword = CommonFn.ComputeSHA256Hash(userParam.fPassword);
                        user.fMobile = userParam.fMobile;
                        _db.SaveChanges();
                    }
                    else
                    {
                        return BadRequest("The old password doesn't match");
                    }
                }
            }
            return RedirectToAction("List");
        }
    }
}
