using Humanizer;
using Microsoft.AspNetCore.Mvc;
using prjToDoList.Models;
using prjToDoList.ViewModels;
using System.Globalization;
using System.Text.Json;

namespace prjToDoList.Controllers
{
    public class TodoController : Controller
    {
        private readonly demoDBContext _db;
        public TodoController(demoDBContext db)
        {
            _db = db;
        }
        public IActionResult List()
        {
            if (HttpContext.Session.Keys.Contains("Login"))
            {
                string? jsonStr = HttpContext.Session.GetString("Login");
                if (jsonStr != null)
                {
                    tUser? loginUser = JsonSerializer.Deserialize<tUser>(jsonStr);
                    if (loginUser != null)
                    {
                        IEnumerable<tToDoItem> items = from todos in _db.tToDoItems
                                                       where todos.fUserId == loginUser.fUserId
                                                       orderby todos.fAddedDate
                                                       select todos;
                        List<ToDoItemViewModel> todoList = new List<ToDoItemViewModel>();
                        foreach (tToDoItem item in items)
                        {
                            string formattedDateString = item.fAddedDate.ToString("yyyy-MM-dd HH:mm:ssZ");
                            todoList.Add(new ToDoItemViewModel() { ToDoItem = item, fAddedDate = formattedDateString });
                        }

                        return View(todoList);
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
    }
}
