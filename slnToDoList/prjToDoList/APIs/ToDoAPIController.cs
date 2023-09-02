using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using prjToDoList.Controllers;
using prjToDoList.Models;
using prjToDoList.ViewModels;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace prjToDoList.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoAPIController : ControllerBase
    {
        private readonly demoDBContext _db;
        private readonly ILogger<HomeController> _logger;
        public ToDoAPIController(ILogger<HomeController> logger, demoDBContext db)
        {
            _logger = logger;
            _db = db;
        }

        // GET api/<ToDoAPIController>/5
        [HttpGet("{id}")]
        public List<object> Get(int id)
        {
            IEnumerable<object> items = from todos in _db.tToDoItems
                                        where todos.fUserId == id
                                        select new // Creating a new object with only the desired fields
                                        {
                                            task = todos.fTaskContent,
                                            done = todos.fIsDone ? "Yes" : "Not Yet"
                                        };
            return items.ToList();
        }

        // POST api/<ToDoAPIController>
        /*
        FromBody reads data from json object, FromForm reads data from form-data
        Since FromBody get the json parameter from request, A class is required as a mediation to received data
        On the other hand FromFrom can get data from parameters directly      
        
        An Example Post API(FromForm):
        [HttpPost]
        public IActionResult Post([FromForm] string userId, [FromForm] string taskContent, [FromForm] string isDone)
        {
            bool success = int.TryParse(userId, out int Id);
            bool success2 = bool.TryParse(isDone, out bool IsDone);

            if (success && success2)
            {
                if (_db.tToDoItems.Any(todo => todo.fUserId == Id))
                {
                    var item = new tToDoItem { fUserId = Id, fTaskContent = taskContent, fIsDone = IsDone };
                    _db.tToDoItems.Add(item);
                    _db.SaveChanges();
                    return Ok(new { status = "Added" });
                }
                else
                {
                    return BadRequest(new { status = "Not valid user ID" });
                }
            }
            else
            {
                return BadRequest(new { status = "Connected but param types might be wrong" });
            }
        }
        */
        [HttpPost]
        public IActionResult Post([FromForm] string taskContent)
        {
            int? userId = null;
            try
            {
                if (HttpContext.Session.Keys.Contains("Login"))
                {
                    string? jsonStr = HttpContext.Session.GetString("Login");
                    if (jsonStr != null)
                    {
                        tUser? loginUser = JsonSerializer.Deserialize<tUser>(jsonStr);
                        if (loginUser != null)
                        {
                            userId = loginUser.fUserId;
                            DateTimeOffset addedDate = DateTimeOffset.UtcNow;
                            var item = new tToDoItem
                            {
                                fUserId = (int)userId,
                                fTaskContent = taskContent,
                                fIsDone = false,
                                fAddedDate = addedDate
                            };
                            _db.tToDoItems.Add(item);
                            _db.SaveChanges();
                            _logger.LogInformation("The task has been written to user: " + loginUser.fUserName);
                            return Ok(new { status = "Added", addedTime = addedDate, taskId = item.fTaskId });
                        }
                    }
                    return BadRequest(new { status = "Not valid request" });
                }
                else
                {
                    return BadRequest(new { status = "Not valid request" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                return BadRequest(new { status = "Web server error." });
            }


        }

        // PUT api/<ToDoAPIController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id)
        {
            if (HttpContext.Session.Keys.Contains("Login"))
            {
                string? jsonStr = HttpContext.Session.GetString("Login");
                if (jsonStr != null)
                {
                    tUser? loginUser = JsonSerializer.Deserialize<tUser>(jsonStr);
                    if (loginUser != null)
                    {
                        var todoToUpdate = _db.tToDoItems.FirstOrDefault(todo => todo.fTaskId == id
                                                                              && todo.fUserId == loginUser.fUserId);
                        if (todoToUpdate != null)
                        {
                            todoToUpdate.fIsDone = !todoToUpdate.fIsDone;
                            _db.SaveChanges();
                            return Ok(new { status = "Updated", isDone = todoToUpdate.fIsDone });
                        }
                        return NotFound(new { status = "Todo not found" });
                    }
                }
                return NotFound(new { status = "Not valid request" });
            }
            else
            {
                return BadRequest(new { status = "Not valid request" });
            }
        }

        // DELETE api/<ToDoAPIController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (HttpContext.Session.Keys.Contains("Login"))
            {
                string? jsonStr = HttpContext.Session.GetString("Login");
                if (jsonStr != null)
                {
                    tUser? loginUser = JsonSerializer.Deserialize<tUser>(jsonStr);
                    if (loginUser != null)
                    {
                        var todoToDelete = _db.tToDoItems.FirstOrDefault(todo => todo.fTaskId == id
                                                                              && todo.fUserId == loginUser.fUserId);
                        if (todoToDelete != null)
                        {
                            _db.tToDoItems.Remove(todoToDelete);
                            _db.SaveChanges();
                            return Ok(new { status = "Deleted" });
                        }
                        return NotFound(new { status = "Todo not found" });
                    }
                }
                return BadRequest(new { status = "Not valid request" });
            }
            else
            {
                return BadRequest(new { status = "Not valid request" });
            }
        }
    }
}
