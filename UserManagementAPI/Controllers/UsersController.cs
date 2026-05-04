using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Models;

namespace UserManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private static List<User> _users = new List<User>();

        [HttpGet] public IActionResult GetAll() => Ok(_users); // GET

        [HttpPost]
        public IActionResult Create(User user) // POST
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _users.Add(user);
            return Ok(user);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, User user) // PUT
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingUser = _users.FirstOrDefault(u => u.Id == id);
            if (existingUser == null)
                return NotFound();

            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            return Ok(existingUser);
        }

        [HttpDelete("{id}")] public IActionResult Delete(int id) => NoContent(); // DELETE
    }
}
