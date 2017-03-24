using System;
using System.IO;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;

namespace elsign.Controllers
{
    public class HomeController : Controller
    {
        public IUrlHelper h;

        protected readonly IMemoryCache _cache;
        protected readonly IHostingEnvironment _environment;

        /*
        /// <summary>
        /// You will get a run-time error if you have multimple constructors for the Controller class:
        /// 
        /// InvalidOperationException: 
        /// Multiple constructors accepting all given argument types have been found in type 'elsign.Controllers.HomeController'.
        /// There should only be one applicable constructor.
        /// 
        /// </summary>
        public HomeController(IMemoryCache cache)
        {
            this._cache = cache;
        }

        public HomeController(IHostingEnvironment environment)
        {
            this._environment = environment;
        }
        */

        public HomeController(IMemoryCache cache, IHostingEnvironment environment)
        {
            this._cache = cache;
            this._environment = environment;
        }

        // Home/SDS with arguments
        [HttpPost]
        public async Task<IActionResult> SDS(ICollection<IFormFile> documentList, int? id)
        {
            // default action shall be to present the SDS menu
            ViewData["Message"] = "SDS is a service which eliminates large data transfers from your web service requests.";
            ViewData["Mode"] = "menu";

            try
            {
                if ((id != null) && (id > 0))
                {
                    switch (id)
                    {
                        case 1:  // a file has been selected for upload
                            ViewData["Message"] = "Upload a file ";
                            ViewData["Mode"] = "upload";

                            var uploads = Path.Combine(_environment.WebRootPath, "uploads");
                            foreach (var file in documentList)
                            {
                                if (file.Length > 0)
                                {
                                    // upload the file to our web server
                                    using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                                    {
                                        await file.CopyToAsync(fileStream);
                                    }
                                    ViewData["Filename"] = file.FileName;
                                }
                            }

                            // fetch the file name submitted by the form
                            // ViewData["Filename"] = Request.Query["documentName"];  // if method="GET"
                            ViewData["Filename"] = Request.Form["documentName"];  // if method="POST"
                            ViewData["DocumentID"] = DocumentMetadata.GetLastDocumentID(_cache);
                            break;
                        case 2:  // present files which may be available for download
                            ViewData["Message"] = "Downloading file ";
                            ViewData["Mode"] = "download";
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return View();
        }  // Action SDS with parameters

        /// <summary>
        /// Upload the document from the web server to Signicat's document server.
        /// </summary>
        /// <param name="selectedDocument"></param>
        public async void UploadDocumentToSignicat(string selectedDocument)
        {
            HttpClientHandler httpClientHandler;
            HttpResponseMessage response;
            string docId = null;
            string username = DocumentMetadata.DEMO_USERNAME;
            string password = DocumentMetadata.DEMO_PASSWORD;

            /*
            httpClientHandler = new HttpClientHandler { Credentials = new NetworkCredential(username, password) };
            using (HttpClient httpClient = new HttpClient(httpClientHandler))
            {
                HttpContent httpContent = new ByteArrayContent(System.IO.File.ReadAllBytes(selectedDocument));
                if (selectedDocument.ToLower().LastIndexOf(".pdf").Equals(selectedDocument.Length - 4))
                {
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                }
                else
                {
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
                }
                response = await httpClient.PostAsync("https://preprod.signicat.com/doc/demo/sds", httpContent);
                docId = await response.Content.ReadAsStringAsync();

                if (docId.Length > 0 && response.StatusCode.Equals(HttpStatusCode.Created))
                {
                    DocumentMetadata.StoreDocumentID(docId, _cache);
                    ViewData["DocumentID"] = docId;
                }
            }
            */

            return;
        }  // UploadDocumentToSignicat()

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Electronic Signatures with Signicat";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult LearnAspNet()
        {
            ViewData["Message"] = "Template Home Page.";

            return View();
        }

        public IActionResult UserLogin()
        {
            ViewData["Message"] = "Login service";

            return View();
        }

        public IActionResult Signature()
        {
            ViewData["Message"] = "Secure signature";

            return View();
        }

        // Home/SDS without arguments
        public IActionResult SDS()
        {
            // default action shall be to present the SDS menu
            ViewData["Message"] = "SDS is a service which eliminates large data transfers from your web service requests.";
            ViewData["Mode"] = "menu";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
