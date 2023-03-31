using HortaIn.BLL.Interfaces;
using HortaIn.BLL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HortaIn.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlobsController : ControllerBase
    {
        private readonly IAzureStorage _storage;

        public BlobsController(IAzureStorage storage)
        {
            _storage = storage;
        }

        [HttpGet(nameof(Get))]
        public async Task<IActionResult> Get()
        {
            // Get all files at the Azure Storage Location and return them
            List<BlobDto>? files = await _storage.ListAsync();

            // Returns an empty array if no files are present at the storage container
            return StatusCode(StatusCodes.Status200OK, files);
        }

        [HttpPost(nameof(Upload))]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            BlobResponseDto? response = await _storage.UploadAsync(file);

            // Check if we got an error
            if (response.Error == true)
            {
                // We got an error during upload, return an error with details to the client
                return StatusCode(StatusCodes.Status500InternalServerError, response.Status);
            }
            else
            {
                // Return a success message to the client about successfull upload
                return StatusCode(StatusCodes.Status200OK, response);
            }
        }

        [HttpGet("{filename}")]
        public async Task<IActionResult> Download(string filename)
        {
            BlobDto? file = await _storage.DownloadAsync(filename);

            // Check if file was found
            if (file == null)
            {
                // Was not, return error message to client
                return StatusCode(StatusCodes.Status500InternalServerError, $"File {filename} could not be downloaded.");
            }
            else
            {
                // File was found, return it to client
                return File(file.Content, file.ContentType, file.Name);
            }
        }

        //[HttpGet("{filenames}")]
        //public async Task<List<IActionResult>> DownloadFiles(string[] filenames)
        //{
        //    List<BlobDto>? files = await _storage.DownloadFilesAsync(filenames);

        //    //// Check if file was found
        //    //foreach (var file in files)
        //    //{
        //    //    if (file.Name == null)
        //    //    {
        //    //        // Was not, return error message to client
        //    //        return StatusCode(StatusCodes.Status500InternalServerError, $"File {file.Name} could not be downloaded.");
        //    //    }
        //    //    else
        //    //    {
        //    //        // File was found, return it to client

        //    //        return List<File(file.Content, file.ContentType, file.Name)>;
        //    //    }
        //    //}

        //    var result = JsonConvert.SerializeObject(files);

        //    return result;
            
        //}

        [HttpDelete("filename")]
        public async Task<IActionResult> Delete(string filename)
        {
            BlobResponseDto response = await _storage.DeleteAsync(filename);

            // Check if we got an error
            if (response.Error == true)
            {
                // Return an error message to the client
                return StatusCode(StatusCodes.Status500InternalServerError, response.Status);
            }
            else
            {
                // File has been successfully deleted
                return StatusCode(StatusCodes.Status200OK, response.Status);
            }
        }
    }
}
