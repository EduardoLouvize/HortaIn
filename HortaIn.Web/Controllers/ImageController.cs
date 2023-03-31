using HortaIn.BLL.Interfaces;
using HortaIn.BLL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace HortaIn.Web.Controllers
{

    public class ImageController : Controller
    {

        private readonly IAzureStorage _storage;

        public ImageController(IAzureStorage storage)
        {
            _storage = storage;
        }

        // GET: ImageController
        public async Task<ActionResult> Index()
        {
            //var loggedUser = HttpContext.Session.GetString("loggedUser");
            List<Image> imageList = new List<Image>();
            var sasUrl = string.Empty;

            var accessToken = HttpContext.Session.GetString("JWToken");

            if (accessToken != null)
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    using (var response = await httpClient.GetAsync($"https://localhost:3001/api/images/userimages"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();

                        imageList = JsonConvert.DeserializeObject<List<Image>>(apiResponse);

                    }
                }
                return View(imageList);
            }

            TempData["notLoggedIn"] = "Para acessar esta página é necessário estar logado.";
            return RedirectToAction("Signin", "Home");
        }


        [HttpGet]
        public async Task<IActionResult> UploadImage()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(Image blobImage)
        {
            Image image = new Image();
            var loggedUser = HttpContext.Session.GetString("loggedUser");
            var accessToken = HttpContext.Session.GetString("JWToken");
            if (accessToken != null)
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    image.ApplicationUserId = loggedUser;
                    IFormFile file = blobImage.file;
                    //StringContent stringContent = new StringContent(file, null, "image/*");

                    BlobResponseDto? responseTeste = await _storage.UploadAsync(file);

                    image.Uri = responseTeste.Blob.Uri;

                    StringContent stringContent = new StringContent(JsonConvert.SerializeObject(image), null, "application/json");



                    using (var response = await httpClient.PostAsync($"https://localhost:3001/api/images", stringContent))
                    {
                    }

                    return RedirectToAction("Index");
                }
            }
            TempData["notLoggedIn"] = "Para acessar esta página é necessário estar logado.";
            return RedirectToAction("Signin", "Home");
        }









































        [HttpGet]
        [Route("/image/deleteimage/{imageUri}")]
        public async Task<IActionResult> DeleteImage(string imageUri)
        {

            return View("DeleteImage", WebUtility.UrlDecode(imageUri));
        }

        [ActionName("DeleteImage")]
        [HttpPost]
        [Route("/image/deleteimage/{imageUri}")]
        public async Task<IActionResult> PostMethodDeleteImage(string imageUri)
        {
            var imageUrlDecoded = WebUtility.UrlDecode(imageUri);
            var imageName = imageUrlDecoded.Split("/").Last();

            var accessToken = HttpContext.Session.GetString("JWToken");

            await _storage.DeleteAsync(imageName);

            if (accessToken != null)
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    using (var getResponse = await httpClient.DeleteAsync($"https://localhost:3001/api/Images/{imageUri}"))
                    {
                        string apiGetReponse = await getResponse.Content.ReadAsStringAsync();

                    }
                    return RedirectToAction("Index");
                }
            }
            TempData["notLoggedIn"] = "Para acessar esta página é necessário estar logado.";
            return RedirectToAction("Signin", "Home");
        }



        //CRIADO VIA SCAFFOLDING

        // GET: ImageController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ImageController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ImageController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ImageController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ImageController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ImageController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ImageController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
