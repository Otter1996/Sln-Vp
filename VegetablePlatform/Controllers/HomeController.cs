using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VegetablePlatform.Models;
using System.Data;
using System.Data.SqlClient;

namespace VegetablePlatform.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Vaild(FormCollection post)
        {
            string account = post["Account"];
            string password = post["Password"];
            string remindme = post["Remindme"];

            VisitorDataBaseEntities visitorDataBase = new VisitorDataBaseEntities();

            //類別VisitorData物件m 是否等於輸入參數account&password
            var member = visitorDataBase.VisitorData
                .Where(m => m.account == account && m.password == password).FirstOrDefault();

            if (member == null)
            {
                ViewBag.Message= "帳號或密碼錯誤，請重新登入!";
                return View("Index");
            }
            else
            {
                ViewBag.Message= "登入成功!";
                return RedirectToAction("Main","Main");
            }
        }

    }
}