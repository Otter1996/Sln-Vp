using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VegetablePlatform.Controllers
{
    public class CompanyController : Controller
    {

        public ActionResult ContactUs()
        {
            return View("ContactUs");
        }

        public ActionResult CommonQuestion()
        {
            return View("CommonQuestion");
        }
    }
}