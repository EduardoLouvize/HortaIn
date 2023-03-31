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
using System.Net;

namespace HortaIn.API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly JwtBearerTokenSettings jwtBearerTokenSettings;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext _context;

        public ImagesController(IOptions<JwtBearerTokenSettings> jwtTokenOptions, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _context = context;
            jwtBearerTokenSettings = jwtTokenOptions.Value;
            userManager = userManager;
        }

        // GET: api/images
        [HttpGet("userimages")]
        public async Task<ActionResult<IEnumerable<Image>>> GetImagesd()
        {
           
            var images = await _context.Images.ToListAsync();

            if (images == null)
            {
                return NotFound();
            }

            return images;
        }

        // GET: api/images/2
        [HttpGet("userimages/{userId}")]
        public async Task<ActionResult<IEnumerable<Image>>> GetImagesByUserId(string userId)
        {            
            
            var images = await _context.Images.Where(p => p.ApplicationUserId == userId).ToListAsync();

            if (images == null)
            {
                return NotFound();
            }

            return images;
        }

        // GET: api/Images/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Image>> GetImage(int id)
        {
            var image = await _context.Images.FindAsync(id);

            if (image == null)
            {
                return NotFound();
            }

            return image;
        }

        // Image: api/Images
        [HttpPost]
        public async Task<ActionResult<Image>> CreateImage(Image image)
        {
            _context.Images.Add(image);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetImage), new { id = image.Id }, image);
        }

        // PUT: api/Images/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateImage(int id, Image image)
        {
            if (id != image.Id)
            {
                return BadRequest();
            }

            _context.Entry(image).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImageExists(id))
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












































        // DELETE: api/Images/5
        [HttpDelete("{imageUri}")]
        public async Task<IActionResult> DeleteImage(string imageUri)
        {
            string imageUrlDecoded = WebUtility.UrlDecode(imageUri);
            var image = await _context.Images.FirstAsync(i => i.Uri == imageUrlDecoded);
            if (image == null)
            {
                return NotFound();
            }

            _context.Images.Remove(image);
            await _context.SaveChangesAsync();

            return NoContent();
        }





































        private bool ImageExists(int id)
        {
            return _context.Images.Any(e => e.Id == id);
        }
    }
}
