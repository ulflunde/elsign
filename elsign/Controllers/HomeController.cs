using System;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Mvc;

namespace elsign.Controllers
{
    public class HomeController : Controller
    {
        public IUrlHelper h;

        private IMemoryCache _cache;

        public HomeController(IMemoryCache cache)
        {
            _cache = cache;
        }

        public string UploadDocumentToSignicat()
        {
            string docId = "thisisafakedocumentID";
            DocumentMetadata.StoreDocumentID(docId, _cache);
            return docId;
        }

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

        // Home/SDS
        public IActionResult SDS(int? id)
        {
            // default action shall be to present the SDS menu
            ViewData["Message"] = "SDS is a service which eliminates large data transfers from your web service requests.";
            ViewData["Mode"] = "menu";

            try
            {
                if ((id != null) && (id > 0))
                {
                    switch(id)
                    {
                        case 1:  // a file has been selected for upload
                            ViewData["Message"] = "Upload a file ";
                            ViewData["Mode"] = "upload";

                            // fetch the file name submitted by the form
                            /* ViewData["Filename"] = Request.Query["documentName"];  // if method="GET" */
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
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
