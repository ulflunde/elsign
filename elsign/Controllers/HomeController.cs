using System;
using System.Collections.Generic;
using System.Linq;
// using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace elsign.Controllers
{
    public class HomeController : Controller
    {
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
            ViewData["Message"] = "SDS is a service which eliminates large data transfers from your web service requests.";
            ViewData["Mode"] = "menu";
            try
            {
                if ((id != null) && (id > 0))
                {
                    switch(id)
                    {
                        case 1:
                            ViewData["Message"] = "Upload a file ";
                            ViewData["Mode"] = "upload";
                            // ViewData["Filename"] = Request.Query["documentName"];  // if method="GET"
                            ViewData["Filename"] = Request.Form["documentName"];  // if method="POST"
                            break;
                        case 2:
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
