using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VegetablePlatform.Models;
using System.IO;

namespace VegetablePlatform.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult ProductIndex(string name)
        {
            VisitorDataBaseEntities db = new VisitorDataBaseEntities();
            var Products= db.Product.Where(m => m.Pid.Contains(name)).ToList();
            return View("ProductIndex","_Layout",Products);
        }

        public ActionResult ReadDetail(string Pid)
        {
            VisitorDataBaseEntities db = new VisitorDataBaseEntities();
            try
            {
                var Product = db.Product.Where(m => m.Pid.Contains(Pid)).FirstOrDefault();
                string path = @"C:\MVC\slnVP\VegetablePlatform\Txt\" + Product.Detail;
                using (StreamReader sr = new StreamReader(path))
                {
                    string line = "";
                    line = sr.ReadToEnd();
                    ViewBag.Message = line;
                    return View("ProductDetail", "_Layout",Product);
                }
            }
            catch (Exception ex)
            {
                string expection = "找不到此產品，或此產品已經下架";
                ViewBag.Message = expection;
                return View("ProductDetail", "_Layout");
            }
        }

        public ActionResult AddCar(string Pid)
        {
            int UserId =(Session["Member"] as UserData).Id;
            return View();
        }
    }
}