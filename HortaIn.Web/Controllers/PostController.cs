using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace HortaIn.Web.Controllers
{
    
    public class PostController : Controller
    {
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var accessToken = HttpContext.Session.GetString("JWToken");
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                //using (var response = await httpClient.GetAsync("https://localhost:3000/api/Products"))
                //{
                //    string apiResponse = await response.Content.ReadAsStringAsync();

                    
                //}
            }


            return View();
        }
    }
}
