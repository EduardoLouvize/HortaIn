using HortaIn.API.JWTBearerConfiguration;
using HortaIn.BLL.Models;
using HortaIn.DAL.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace HortaIn.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly JwtBearerTokenSettings jwtBearerTokenSettings;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext _context;

        public PostsController(IOptions<JwtBearerTokenSettings> jwtTokenOptions, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _context = context;
            jwtBearerTokenSettings = jwtTokenOptions.Value;
            userManager = userManager;
        }

        // GET: api/posts/2
        [HttpGet("userposts/{userId}")]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts(string userId)
        {            
            var posts = await _context.Posts.Where(p => p.ApplicationUserId == userId).ToListAsync();

            if (posts == null)
            {
                return NotFound();
            }

            return posts;
        }

        // GET: api/posts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPost(int id)
        {
            var post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            return post;
        }

        // POST: api/posts
        [HttpPost]
        public async Task<ActionResult<Post>> CreatePost(Post post)
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPost), new { id = post.Id }, post);
        }

        // PUT: api/posts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(int id, Post post)
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
                if (!PostExists(id))
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

        // DELETE: api/posts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}
