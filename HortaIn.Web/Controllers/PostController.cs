using HortaIn.BLL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace HortaIn.Web.Controllers
{

    public class PostController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var loggedUser = HttpContext.Session.GetString("loggedUser");
            List<Post> postsList = new List<Post>();

            var accessToken = HttpContext.Session.GetString("JWToken");

            if (accessToken != null)
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    using (var response = await httpClient.GetAsync($"https://localhost:3001/api/posts/userposts/{loggedUser}"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();

                        postsList = JsonConvert.DeserializeObject<List<Post>>(apiResponse);
                    }
                }
                return View(postsList);
            }

            TempData["notLoggedIn"] = "Para acessar esta página é necessário estar logado.";
            return RedirectToAction("Signin", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> CreatePost()
        {
            return View();

        }


        [HttpPost]
        public async Task<IActionResult> CreatePost(Post post)
        {
            var accessToken = HttpContext.Session.GetString("JWToken");
            var loggerUser = HttpContext.Session.GetString("loggedUser");

            post.ApplicationUserId = loggerUser;

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(post), Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync("https://localhost:3001/api/posts", stringContent);


                return RedirectToAction("Index");
            }

        }


        [HttpGet]
        [Route("/post/editpost/{postId}")]
        public async Task<IActionResult> EditPost(int postId)
        {
            Post post = new Post();

            var accessToken = HttpContext.Session.GetString("JWToken");
            if (accessToken != null)
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    using (var getResponse = await httpClient.GetAsync($"https://localhost:3001/api/Posts/{postId}"))
                    {
                        string apiGetReponse = await getResponse.Content.ReadAsStringAsync();

                        post = JsonConvert.DeserializeObject<Post>(apiGetReponse);


                    }

                    return View(post);
                }
            }
            TempData["notLoggedIn"] = "Para acessar esta página é necessário estar logado.";
            return RedirectToAction("Signin", "Home");
        }


        [HttpPost]
        public async Task<IActionResult> EditPost(Post post)
        {
            var loggedUser = HttpContext.Session.GetString("loggedUser");
            var accessToken = HttpContext.Session.GetString("JWToken");
            if (accessToken != null)
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    post.ApplicationUserId = loggedUser;

                    StringContent stringContent = new StringContent(JsonConvert.SerializeObject(post), Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PutAsync($"https://localhost:3001/api/Posts/{post.Id}", stringContent))
                    {
                        string apiReponse = await response.Content.ReadAsStringAsync();

                        post = JsonConvert.DeserializeObject<Post>(apiReponse);
                    }
                    //}

                    //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);


                    return RedirectToAction("Index");
                }
            }
            TempData["notLoggedIn"] = "Para acessar esta página é necessário estar logado.";
            return RedirectToAction("Signin", "Home");
        }

        [HttpGet]
        [Route("/post/deletepost/{postId}")]
        public async Task<IActionResult> DeletePost(int postId)
        {
            Post post = new Post();

            var accessToken = HttpContext.Session.GetString("JWToken");
            if (accessToken != null)
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    using (var getResponse = await httpClient.GetAsync($"https://localhost:3001/api/Posts/{postId}"))
                    {
                        string apiGetReponse = await getResponse.Content.ReadAsStringAsync();

                        post = JsonConvert.DeserializeObject<Post>(apiGetReponse);


                    }
                    return View(post);
                }
            }
            TempData["notLoggedIn"] = "Para acessar esta página é necessário estar logado.";
            return RedirectToAction("Signin", "Home");
        }

        [ActionName("DeletePost")]
        [HttpPost]
        [Route("/post/deletepost/{postId}")]
        public async Task<IActionResult> PostMethodDeletePost(int postId)
        {
            var accessToken = HttpContext.Session.GetString("JWToken");
            if (accessToken != null)
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    using (var getResponse = await httpClient.DeleteAsync($"https://localhost:3001/api/Posts/{postId}"))
                    {
                        string apiGetReponse = await getResponse.Content.ReadAsStringAsync();

                    }
                    return RedirectToAction("Index");
                }
            }
            TempData["notLoggedIn"] = "Para acessar esta página é necessário estar logado.";
            return RedirectToAction("Signin", "Home");
        }


    }
}
