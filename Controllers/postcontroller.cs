using InfoApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class PostController : ControllerBase
{
    private readonly AppDbContext _context;

    public PostController(AppDbContext context)
    {
        _context = context;
        _context.Database.EnsureCreated();
    }

    [HttpGet]
    public async Task<ActionResult> GetAllPosts()
    {
        return Ok(await _context.Posts.ToArrayAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetPost(int id)
    {
        var post = await _context.Posts.FindAsync(id);
        if (post == null)
        {
            return NotFound();
        }
        return Ok(post);
    }

    [HttpPost]
    public async Task<ActionResult<Post>> PostPost(Post post)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _context.Posts.Add(post);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPost), new { id = post.Id }, post);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> PutPost(int id, Post post)
    {
        if (id != post.Id)
        {
            return BadRequest();
        }

        _context.Entry(post).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Posts.Any(p => p.Id == id))
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

    [HttpDelete("{id}")]
    public async Task<ActionResult<Post>> DeletePost(int id)
    {
        var post = await _context.Posts.FindAsync(id);
        if (post == null)
        {
            return NotFound();
        }

        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();

        return post;
    }

    [HttpPost]
    [Route("Delete")]
    public async Task<ActionResult> DeleteMultiple([FromQuery] int[] ids)
    {
        var posts = await _context.Posts.Where(p => ids.Contains(p.Id)).ToListAsync();

        if (posts.Count != ids.Length)
        {
            return NotFound("Some posts were not found.");
        }

        _context.Posts.RemoveRange(posts);
        await _context.SaveChangesAsync();

        return Ok(posts);
    }
}
