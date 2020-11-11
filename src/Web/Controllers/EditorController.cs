using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Web.Controllers
{
    public class EditorController : BaseController
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public EditorController(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("UploadFiles")]
        [Produces("application/json")]
        public async Task<IActionResult> Post(List<IFormFile> files, string mceContent)
        {
            // Get the file from the POST request
            var theFile = HttpContext.Request.Form.Files.GetFile("file");

            // Get the server path, wwwroot
            string webRootPath = webHostEnvironment.WebRootPath;

            // Building the path to the uploads directory
            var fileRoute = Path.Combine(webRootPath, "uploads");

            // Get the mime type
            var mimeType = HttpContext.Request.Form.Files.GetFile("file").ContentType;

            // Get File Extension
            string extension = System.IO.Path.GetExtension(theFile.FileName);

            // Generate Random name.
            string name = Guid.NewGuid().ToString().Substring(0, 8) + extension;

            // Build the full path inclunding the file name
            string link = Path.Combine(fileRoute, name);

            // Create directory if it does not exist.
            FileInfo dir = new FileInfo(fileRoute);
            dir.Directory.Create();

            // Basic validation on mime types and file extension
            string[] fileMimetypes = { "text/plain", "application/msword", "application/x-pdf", "application/pdf", "application/json", "text/html", "image/gif", "image/jpeg", "image/pjpeg", "image/x-png", "image/png", "image/svg+xml" };
            string[] fileExt = { ".txt", ".pdf", ".doc", ".json", ".html", ".gif", ".jpeg", ".jpg", ".png", ".svg", ".blob" };

            try
            {
                if (Array.IndexOf(fileMimetypes, mimeType) >= 0 && (Array.IndexOf(fileExt, extension) >= 0))
                {
                    // Copy contents to memory stream.
                    Stream stream;
                    stream = new MemoryStream();
                    theFile.CopyTo(stream);
                    stream.Position = 0;
                    String serverPath = link;

                    // Save the file
                    using (FileStream writerFileStream = System.IO.File.Create(serverPath))
                    {
                        await stream.CopyToAsync(writerFileStream);
                        writerFileStream.Dispose();
                    }

                    // Return the file path as json
                    Hashtable fileUrl = new Hashtable();
                    fileUrl.Add("link", "/uploads/" + name);

                    return Json(files, fileUrl);
                }
                throw new ArgumentException("The file did not pass the validation");
            }

            catch (ArgumentException ex)
            {
                return Json(ex.Message);
            }
        }

        //    [HttpPost("UploadFiles")]
        //    [Produces("application/json")]
        //    public async Task<IActionResult> Post(List<IFormFile> files)
        //    {
        //        // Get the file from the POST request
        //        var theFile = HttpContext.Request.Form.Files.GetFile("file");

        //        // Get the server path, wwwroot
        //        string webRootPath = _hostingEnvironment.WebRootPath;

        //        // Building the path to the uploads directory
        //        var fileRoute = Path.Combine(webRootPath, "uploads");

        //        // Get the mime type
        //        var mimeType = HttpContext.Request.Form.Files.GetFile("file").ContentType;

        //        // Get File Extension
        //        string extension = System.IO.Path.GetExtension(theFile.FileName);

        //        // Generate Random name.
        //        string name = Guid.NewGuid().ToString().Substring(0, 8) + extension;

        //        // Build the full path inclunding the file name
        //        string link = Path.Combine(fileRoute, name);

        //        // Create directory if it does not exist.
        //        FileInfo dir = new FileInfo(fileRoute);
        //        dir.Directory.Create();

        //        // Basic validation on mime types and file extension
        //        string[] imageMimetypes = { "image/gif", "image/jpeg", "image/pjpeg", "image/x-png", "image/png", "image/svg+xml" };
        //        string[] imageExt = { ".gif", ".jpeg", ".jpg", ".png", ".svg", ".blob" };

        //        try
        //        {
        //            if (Array.IndexOf(imageMimetypes, mimeType) >= 0 && (Array.IndexOf(imageExt, extension) >= 0))
        //            {
        //                // Copy contents to memory stream.
        //                Stream stream;
        //                stream = new MemoryStream();
        //                theFile.CopyTo(stream);
        //                stream.Position = 0;
        //                String serverPath = link;

        //                // Save the file
        //                using (FileStream writerFileStream = System.IO.File.Create(serverPath))
        //                {
        //                    await stream.CopyToAsync(writerFileStream);
        //                    writerFileStream.Dispose();
        //                }

        //                // Return the file path as json
        //                Hashtable imageUrl = new Hashtable();
        //                imageUrl.Add("link", "/uploads/" + name);

        //                return Json(imageUrl);
        //            }
        //            throw new ArgumentException("The image did not pass the validation");
        //        }

        //        catch (ArgumentException ex)
        //        {
        //            return Json(ex.Message);
        //        }
        //    }
    }
}

