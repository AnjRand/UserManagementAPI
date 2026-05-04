using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Models;

namespace UserManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private static List<User> _users = new List<User>();

        [HttpGet] public IActionResult GetAll() => Ok(_users); // GET

        [HttpPost]
        public IActionResult Create(User user) // POST
        {
            _users.Add(user);
            return Ok(user);
        }

        [HttpPut("{id}")] public IActionResult Update(int id, User user) => Ok(); // PUT

        [HttpDelete("{id}")] public IActionResult Delete(int id) => NoContent(); // DELETE
    }
}
