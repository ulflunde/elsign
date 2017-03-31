using System;
using System.IO;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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

        private string _selectedDocumentOnServer = null;

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


        private async Task UploadFileFromClientToServer(ICollection<IFormFile> documentList)
        {
            string filename = "";
            var uploads = Path.Combine(_environment.WebRootPath, "uploads");

            foreach (var file in documentList)
            {
                if (file.FileName.Length > 0 && file.Length > 0)  // file.Length is the size of the file in bytes
                {
                    // cancel the operation if the file size is > 4 MB
                    if (file.Length < 4000000)  
                    {
                        // upload the file to our web server
                        filename = file.FileName;
                        if (filename.EndsWith(".pdf") || filename.EndsWith(".txt"))
                        {
                            _selectedDocumentOnServer = Path.Combine(uploads, filename);
                            using (var fileStream = new FileStream(_selectedDocumentOnServer, FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                            }
                            ViewData["Filesize"] = Convert.ToInt32(file.Length);  // if this value is set, we go ahead with the upload
                        }
                        else
                        {
                            ViewData["errors"] += "Only PDF and TXT documents are accepted. If ";
                            ViewData["errors"] += filename + " is a text file, please change the filename extension to .TXT";
                        }
                    }
                    else
                    {
                        ViewData["errors"] = "File too big. (" + file.Length.ToString() + " bytes)";
                    }
                }
            }

            // assume that we only need to remember the last file uploaded
            DocumentMetadata.StoreFilename(filename, _cache);

            return;
        }  // UploadFileFromClientToServer()


        /// <summary>
        /// Upload the document from the web server to Signicat's document server.
        /// </summary>
        /// <returns></returns>
        private async Task UploadFileFromServerToSignicat()
        {
            HttpClientHandler httpClientHandler;
            HttpResponseMessage response;
            string username = DocumentService.DEMO_USERNAME;
            string password = DocumentService.DEMO_PASSWORD;
            string docId = null;  // fetched from Signicat, see below

            httpClientHandler = new HttpClientHandler { Credentials = new NetworkCredential(username, password) };
            using (HttpClient httpClient = new HttpClient(httpClientHandler))
            {
                HttpContent httpContent = new ByteArrayContent(System.IO.File.ReadAllBytes(_selectedDocumentOnServer));
                if (_selectedDocumentOnServer.ToLower().LastIndexOf(".pdf").Equals(_selectedDocumentOnServer.Length - 4))
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

            return;
        }  // UploadFileFromServerToSignicat()


        /// <summary>
        /// Home/SDS with arguments.
        /// </summary>
        /// <param name="documentList">input-filelist from the HTML form</param>
        /// <param name="id">subpage identifier</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SDS(ICollection<IFormFile> documentList, int? id)
        {
            // default action shall be to present the SDS menu
            ViewData["Message"] = "SDS is a service which eliminates large data transfers from your web service requests.";
            ViewData["Mode"] = "menu";
            ViewData["errors"] = "";

            try
            {
                if ((id != null) && (id > 0))
                {
                    switch (id)
                    {
                        case 1:  // a file has been selected for upload
                            ViewData["Message"] = "Upload a file ";
                            ViewData["Mode"] = "upload";

                            // fetch the file name submitted by the form

                            // 1. using form with action (not asp-action) and method="GET"
                            // DocumentMetadata.StoreFilename(Request.Query["documentList"]);

                            // 2. using form with action (not asp-action) and method="POST"
                            // DocumentMetadata.StoreFilename(Request.Form["documentList"]);

                            // 3. using form with asp-action (not action) and method="POST"
                            await UploadFileFromClientToServer(documentList);  // also sets ViewData["filesize"]
                            if (ViewData["Filesize"] != null)
                            {
                                await UploadFileFromServerToSignicat();
                                ViewData["Filename"] = DocumentMetadata.GetLastFilename(_cache);
                                ViewData["DocumentID"] = DocumentMetadata.GetLastDocumentID(_cache);
                            }

                            if (ViewData["Filename"] != null && ViewData["Filesize"] != null && ViewData["DocumentID"] != null)
                            {
                                DocumentMetadata.AddToStoredDocuments(_cache, (string) ViewData["Filename"], (int) ViewData["Filesize"], (string) ViewData["DocumentID"]);
                            }
                            else
                            {
                                ViewData["Message"] = "Uploading failed.";
                            }
                            break;
                        case 2:  // present files which may be available for download
                            ViewData["Message"] = "Please choose a file for download:";
                            ViewData["Mode"] = "download";
                            ViewData["Filelist"] = DocumentMetadata.GetDocumentlist(_cache);
                            if ((string[]) ViewData["Filelist"] == null)
                            {
                                ViewData["Message"] = "No files have been uploaded.";
                            }
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
        /// When pressing the Submit-button, this page is invoked first, and then the page specified by the form definition.
        /// </summary>
        /// <param name="selectedDocument"></param>
        public void StoreDocumentMetadata(string selectedDocument)
        {
            string copy = selectedDocument;
            return;
        }  // 

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
            ViewData["Filelist"] = DocumentMetadata.GetDocumentlist(_cache);
            if ((string[]) ViewData["Filelist"] != null)
            {
                ViewData["Message"] = "Select a document to sign:";
            }
            else
            {
                ViewData["Message"] = "No files have been uploaded.";
            }

            return View();
        }

        // Home/SDS without arguments
        public IActionResult SDS()
        {
            // default action for the SDS menu choice (if no subpage is specified) shall be to present the SDS menu
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
