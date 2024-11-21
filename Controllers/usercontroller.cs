using InfoApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Info.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult> GetAllUsers()
        {
            return Ok(await _context.Users.ToArrayAsync());
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // POST: api/users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Users.Any(u => u.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        // POST: api/users/Delete?ids=1&ids=2&ids=3
        [HttpPost]
        [Route("Delete")]
        public async Task<ActionResult> DeleteMultiple([FromQuery] int[] ids)
        {
            var users = await _context.Users.Where(u => ids.Contains(u.Id)).ToListAsync();

            if (users.Count != ids.Length)
            {
                return NotFound("Some users were not found.");
            }

            _context.Users.RemoveRange(users);
            await _context.SaveChangesAsync();

            return Ok(users);
        }
    }
}
