using Microsoft.AspNetCore.Mvc;
using prjToDoList.Models;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace prjToDoList.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckLoginStatusAPIController : ControllerBase
    {
        // GET: api/<CheckLoginStatusAPIController>
        [HttpGet]
        public IActionResult Get()
        {

            if (HttpContext.Session.Keys.Contains("Login"))
            {
                string? jsonStr = HttpContext.Session.GetString("Login");
                if (jsonStr != null)
                {
                    tUser? loginUser = JsonSerializer.Deserialize<tUser>(jsonStr);
                    if (loginUser != null)
                    {
                        return Ok(new { status = "login" });
                    }

                }
                return BadRequest(new { status = "No user found" });

            }
            else
            {
                return BadRequest(new { status = "No user found" });
            }
        }
    }
}
