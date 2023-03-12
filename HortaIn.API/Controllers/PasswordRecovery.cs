using HortaIn.API.JWTBearerConfiguration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using HortaIn.DAL.Data; 
using HortaIn.DAL.utils; 
using HortaIn.BLL.Models;
namespace HortaIn.API.Controllers
{
    
    [Route("api/change-password")]
    [ApiController]
    public class PasswordRecoveryController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly PasswordChangeContext _context;
        public PasswordRecoveryController(IOptions<JwtBearerTokenSettings> jwtTokenOptions, UserManager<IdentityUser> userManager,PasswordChangeContext context)
        {
            this.userManager = userManager;
            this._context = context;
        }

        [HttpPost]
        [Route("Request")]
        public async Task<IActionResult> RequestChange(string email)
        {   
            var accountFound = await userManager.FindByEmailAsync(email);
            if (accountFound == null)
            {
                return NotFound("conta não encontada");
            }
            var secret = SecretGenerator.Generate();
            var request = new PasswordRecovery{Secret = secret, Used= false,UserName= accountFound.ToString()};
            _context.Add(request);
            _context.SaveChanges();
            Mailer.Send(email,secret);
            return Ok("");
        }
        [HttpGet]
        [Route("Request/{secret}")]
        public async Task<IActionResult> Verify(string secret)
        {   
            var secretFound = _context.PasswordChange.FirstOrDefault(p => p.Secret == secret);
            if (secretFound == null)
            {
                return NotFound("Requisição não encontrada");
            }
            if (secretFound?.Used == true)
            {
                return Conflict("Requisição já utilizada");

            }
            return Ok("Requisição válida");
        }
        [HttpPost]
        [Route("Change/{secret}")]
          public async Task<IActionResult> Change(string secret, [FromBody] PassowordRecoveryDTO PassowordRecoveryDTO)
        {   
            var secretFound = _context.PasswordChange.FirstOrDefault(p => p.Secret == secret);
            if (secretFound == null)
            {
                return NotFound("Requisição não encontrada");
            }
            if (secretFound?.Used == true)
            {
                return Conflict("Requisição já utilizada");

            }
            var user = await userManager.FindByNameAsync(secretFound?.UserName);
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var result = await userManager.ResetPasswordAsync(user, token, PassowordRecoveryDTO.newPassword);
             if (result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    return BadRequest(new { Message = "error",Errors = error });
                }
            secretFound.Used = true;
            _context.SaveChanges();
            return Ok("Senha atualizada");
            }

         return StatusCode(500);
        }

}
}