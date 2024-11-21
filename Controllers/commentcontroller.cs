using InfoApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Info.Api.Controllers
{
    // Defines the route for this controller as "api/comment"
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly AppDbContext _context;

        // Constructor to inject the AppDBContext dependency
        public CommentController(AppDbContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        // GET: api/comment
        [HttpGet]
        public async Task<ActionResult> GetAllComments()
        {
            return Ok(await _context.Comments.ToArrayAsync());
        }

        // GET: api/comment/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult> GetComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment);
        }

        // POST: api/comment
        [HttpPost]
        public async Task<ActionResult<Comment>> PostComment(Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetComment), new { id = comment.Id }, comment);
        }

        // PUT: api/comment/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> PutComment(int id, Comment comment)
        {
            if (id != comment.Id)
            {
                return BadRequest();
            }

            _context.Entry(comment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Comments.Any(c => c.Id == id))
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

        // DELETE: api/comment/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<Comment>> DeleteComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return comment;
        }

        // POST: api/comment/Delete?ids=1&ids=2&ids=3
        [HttpPost]
        [Route("Delete")]
        public async Task<ActionResult> DeleteMultiple([FromQuery] int[] ids)
        {
            var comments = new List<Comment>();

            foreach (var id in ids)
            {
                var comment = await _context.Comments.FindAsync(id);

                if (comment == null)
                {
                    return NotFound($"Comment with ID {id} not found.");
                }

                comments.Add(comment);
            }

            _context.Comments.RemoveRange(comments);
            await _context.SaveChangesAsync();

            return Ok(comments);
        }
    }
}
