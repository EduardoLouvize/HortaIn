using HortaIn.API.JWTBearerConfiguration;
using HortaIn.BLL.Models;
using HortaIn.DAL.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HortaIn.API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtBearerTokenSettings jwtBearerTokenSettings;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext _context;

        public AuthController(IOptions<JwtBearerTokenSettings> jwtTokenOptions, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.jwtBearerTokenSettings = jwtTokenOptions.Value;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserDetails userDetails)
        {
            if (!ModelState.IsValid || userDetails == null)
            {
                return new BadRequestObjectResult(new { Message = "Registro de usuário falhou" });
            }

            var identityUser = new ApplicationUser() { UserName = userDetails.UserName, Email = userDetails.Email };
            var emailInUse = await userManager.FindByEmailAsync(userDetails.Email);
            if (emailInUse != null)
            {
               return Conflict("Email already in use"); 
            }
            var result = await userManager.CreateAsync(identityUser, userDetails.Password);

            if (result.Succeeded)
            {
                var dictionary = new ModelStateDictionary();
                foreach (IdentityError error in result.Errors)
                {
                    dictionary.AddModelError(error.Code, error.Description);
                    return new BadRequestObjectResult(new { Message = "Registro de usuário falhou", Errors = dictionary });
                }
            return Ok(new { Message = "Usuário registrado com sucesso" });

            }
            return BadRequest();

        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginCredentials credentials)
        {
            ApplicationUser identityUser;
            if (!ModelState.IsValid
                || credentials == null
                || (identityUser = await ValidateUser(credentials)) == null)
            {
                return Unauthorized(new { Message = "Login failed" });
            }

            var token = GenerateToken(identityUser);
            return Ok(token);
        }

        [HttpPost]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            // Well, What do you want to do here ?
            // Wait for token to get expired OR
            // Maintain token cache and invalidate the tokens after logout method is called
            return Ok(new { Token = "", Message = "Logged Out" });
        }


        private async Task<ApplicationUser> ValidateUser(LoginCredentials credentials)
        {
            var identityUser = await userManager.FindByEmailAsync(credentials.Email);
            if (identityUser != null)
            {
                var result = userManager.PasswordHasher.VerifyHashedPassword(identityUser, identityUser.PasswordHash, credentials.Password);
                return result == PasswordVerificationResult.Failed ? null : identityUser;
            }

            return null;
        }

        private object GenerateToken(ApplicationUser identityUser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtBearerTokenSettings.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, identityUser.UserName.ToString()),
                    new Claim(ClaimTypes.Email, identityUser.Email)
                }),

                Expires = DateTime.UtcNow.AddSeconds(jwtBearerTokenSettings.ExpiryTimeInSeconds),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = jwtBearerTokenSettings.Audience,
                Issuer = jwtBearerTokenSettings.Issuer
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{email}")]
        //public async Task<IActionResult> PutProduct(LoginCredentials credentials)
        //{
        //    var identityUser = await userManager.FindByEmailAsync(credentials.Email);


        //    _context.Entry(identityUser).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!UserExists(credentials.Email))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return CreatedAtAction("GetUser", new { email = identityUser.Email }, identityUser);
        //}


        [HttpGet("{email}")]
        public async Task<ActionResult<ApplicationUser>> GetUser(string email)
        {
            //var users = await userManager.Users.ToListAsync();
            var user = await userManager.Users.Where(e => e.Email == email).FirstOrDefaultAsync();
            //var user = await _context.Users.FindAsync(email);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        private bool UserExists(string email)
        {
            return _context.Users.Any(e => e.Email == email);
        }



        //Para testes retornando todos os usuários cadastrados
        [Authorize]
        [HttpGet]
        [Route("Users")]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetUsers()
        {
            
            return await userManager.Users.ToListAsync();
            
        }

    }
}
