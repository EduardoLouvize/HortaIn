using HortaIn.BLL.Models;
using HortaIn.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Http.Headers;

namespace HortaIn.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Signup()
        {
            return View();
        }

        public async Task<IActionResult> ListUsers()
        {
            List<UserDetails> userList = new List<UserDetails>();

            var accessToken = HttpContext.Session.GetString("JWToken");

            if (accessToken != null)
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    using (var response = await httpClient.GetAsync("https://localhost:3001/api/Auth/Users"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();

                        userList = JsonConvert.DeserializeObject<List<UserDetails>>(apiResponse);
                    }
                }
                return View(userList);
            }

            TempData["notLoggedIn"] = "Para acessar esta página é necessário estar logado.";
            return RedirectToAction("Signin");


        }

        [HttpPost]
        public async Task<IActionResult> Signup(UserDetails userDetails)
        {
            Regex validatedRegex = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");

            if (validatedRegex.IsMatch(userDetails.Password))
            {
                using (var httpClient = new HttpClient())
                {
                    StringContent stringContent = new StringContent(JsonConvert.SerializeObject(userDetails), Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync("https://localhost:3001/api/Auth/Register", stringContent);
                    if (response.StatusCode == HttpStatusCode.Conflict)
                    {
                        ViewBag.Message = "E-mail já utilizado.";
                        return View();
                    }


                    return RedirectToAction("Signin");
                }
            }

            else
            {
                ViewBag.Message = "Senha deve ter pelo menos uma letra maiúscula, uma letra minúscula e um caractere especial.";
                return View();
            }

        }

        public IActionResult Signin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Signin(LoginCredentials loginCredentials)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(loginCredentials), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("https://localhost:3001/api/Auth/Login", stringContent))
                {
                    //string token = await response.Content.ReadAsStringAsync();
                    string token = await response.Content.ReadAsStringAsync();


                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        TempData["unauthorizedMessage"] = "Email ou senha incorreto!";
                        return RedirectToAction("Signin");
                    }

                    HttpContext.Session.SetString("JWToken", token);
                }
            }

            TempData["unauthorizedMessage"] = "";
            return RedirectToAction("ListUsers");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {

            return View();
        }

        [ActionName("Logout")]
        [HttpPost]
        public async Task<IActionResult> LogoutPost()
        {
            using (var httpClient = new HttpClient())
            {


                using (var response = await httpClient.PostAsync("https://localhost:3001/api/Auth/Logout", null))
                {

                    string token = await response.Content.ReadAsStringAsync();

                    HttpContext.Session.Clear();
                }
            }

            //TempData["unauthorizedMessage"] = "";
            return RedirectToAction("Signin");
        }

        [HttpPost]
        public async Task<IActionResult> Recovery(RecoveryDTO RecoveryDTO)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(RecoveryDTO), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("https://localhost:3001/api/change-password/Request", stringContent))
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        TempData["OkMessage"] = "";
                        return RedirectToAction("Recovery");
                    }

                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        TempData["NotFoundMessage"] = "Email não encontrado";
                        return RedirectToAction("Recovery");
                    }

                    TempData["ServerErrorMessage"] = "Algo deu errado!";
                    return RedirectToAction("Recovery");
                }
            }
        }
        public IActionResult Recovery()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Change(PassowordRecoveryDTO dto)
        {
            Regex validatedRegex = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");
            if (validatedRegex.IsMatch(dto.newPassword) == false)
            {
                TempData["BadRequestMessage"] = "Senha deve ter pelo menos uma letra maiúscula, uma letra minúscula e um caractere especial.";
                return RedirectToAction("Change");
            }

            using (var httpClient = new HttpClient())
            {
                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync($"https://localhost:3001/api/change-password/Change/{dto.Secret}", stringContent))
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        TempData["OkMessage"] = "";
                        return RedirectToAction("Change");
                    }

                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        TempData["NotFoundMessage"] = "Link inválido";
                        return RedirectToAction("Change");
                    }

                    TempData["ServerErrorMessage"] = "Algo deu errado!";
                    return RedirectToAction("Change");
                }
            }




        }
        public IActionResult Change(string secret)
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}