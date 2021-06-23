using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using VegetablePlatform.Models;
using VegetablePlatform.BusinessLogic;

namespace VegetablePlatform.Controllers
{
    public class MainController : Controller
    {
        /// <summary>
        /// 產生Main頁面
        /// </summary>
        public ActionResult Main()
        {
            return View();
        }
        /// <summary>
        /// 產生Login頁面
        /// </summary>
        public ActionResult Login()
        {
            return View();
        }
        /// <summary>
        /// 產生Register頁面
        /// </summary>
        /// <returns></returns>
        public ActionResult Register()
        {
            return View("Register");
        }
        /// <summary>
        /// Session清除後返回首頁
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            Session.Clear();
            return View("Main");
        }
        /// <summary>
        /// 登入機制
        /// </summary>
        [HttpPost]
        public ActionResult Login(string UserId, string UserPassword)
        {
            LoginLogic login = new LoginLogic();
            bool state = login.IsSuccessLogin(UserId, UserPassword);
            if (state == false)
            {
                ViewBag.Message = "登入失敗，請重新輸入";
                return View("Login");
            }
            return RedirectToAction("Main");
        }
        public ActionResult ForgetPassword()
        {
            return View("ForgetPassword");
        }
        /// <summary>
        /// 驗證驗證碼的方法
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        [HttpPost]
        [MultiButton("Confirm")]
        public ActionResult Confirm(FormCollection post)
        {
            UserDataBaseEntitiesEntities db = new UserDataBaseEntitiesEntities();
            string email = post["Email"];
            string account = post["Account"];
            string newpassword = post["Password"];

            var member = db.UserData
                .Where(m => m.account == account && m.email == email).FirstOrDefault();
            if (member != null)
            {
                if (post["Verify"] == member.UserActivation1.Random)
                {
                    member.password = newpassword;
                    member.UserActivation1.Random = null;
                    db.SaveChanges();

                    ViewBag.ChangePassword = "更改密碼成功!";
                    return View();
                }
                else
                {
                    ViewBag.ChangePassword = "輸入驗證碼錯誤，請重新傳送驗證碼。";
                    member.UserActivation1.Random = null;
                    return View();
                }

            }
            else
            {
                ViewBag.ChangePassword = "找不到此用戶，或輸入Email不相符，請重新再試。";
                return View();
            }
        }
        /// <summary>
        /// 忘記密碼之Email驗證碼功能
        /// </summary>
        /// <param name="post">Email</param>
        [HttpPost]
        [MultiButton("Verify")]
        public ActionResult SendEmail(FormCollection post)
        {

            string Email = post["Email"];
            string subject = "此為系統自動發送信件，請勿直接回覆。";

            Random VerifyNumber = new Random();

            UserDataBaseEntitiesEntities db = new UserDataBaseEntitiesEntities();

            var member = db.UserActivation
                .Where(m => m.email == Email).FirstOrDefault();

            if (member != null)
            {
                member.Random = Convert.ToString(VerifyNumber.Next(99999));
                string body = "親愛的顧客您好，以下為您的Email驗證碼:【 " + member.Random + " 】，請在10分鐘內前往填寫驗證碼。";
                WebMail.Send(Email, subject, body, null, null, null, true, null, null, null, null, null);
                ViewBag.msg = "Email成功傳送，請至信箱擷取驗證碼...";
                return View("ForgetPassword");
            }
            else
            {
                ViewBag.msg = "Email傳送失敗，請聯絡管理員進行了解(開玩笑的不要找我)";
                return View("ForgetPassword");
            }

        }
        /// <summary>
        /// 使用者點選註冊按鈕就會Call此方法
        /// </summary>
        /// <param name="userdata">欲註冊的完整資料</param>
        /// <returns>返回註冊頁並夾帶ViewBag</returns>
        [HttpPost]
        public ActionResult Register(UserData userdata)
        {
            UserDataBaseEntitiesEntities db = new UserDataBaseEntitiesEntities();
            var member = db.UserData.Where(m => m.account == userdata.account && m.password == userdata.password).FirstOrDefault();
            if (member == null)
            {
                db.UserData.Add(userdata);
                SendEmail(userdata.email);
                db.SaveChanges();
                ViewBag.Message = "註冊成功，以發送Email至您的信箱，請前往驗證Email";
                return View("Register");
            }
            ViewBag.Message = "此帳號已經有人使用，註冊失敗。";
            return View("Register");
        }
        /// <summary>
        /// 產生驗證訊息的View
        /// </summary>
        /// <returns>Activation的View</returns>
        public ActionResult Activation()
        {
            ViewBag.Message = "驗證失敗，就跟我的人生一樣";
            if (RouteData.Values["id"] != null)
            {
                Guid activationCode = new Guid(RouteData.Values["id"].ToString());
                UserDataBaseEntitiesEntities db = new UserDataBaseEntitiesEntities();
                UserActivation userActivation = db.UserActivation.Where(p => p.ActivationCode == activationCode).FirstOrDefault();
                if (userActivation != null)
                {
                    ViewBag.Message = "驗證成功";
                    UserData user = db.UserData.Where(m => m.email == userActivation.email).FirstOrDefault();
                    user.emailverify = true;
                    userActivation.ActivationCode = null;
                    db.SaveChanges();
                }
            }

            return View("Activation");
        }
        /// <summary>
        /// 寄出激活Email
        /// </summary>
        /// <param name="email">欲註冊使用者輸入的email</param>
        public void SendEmail(string email)
        { 
            /*紀錄Email認證信的activationCode並寫進DB*/
            Guid activationCode = Guid.NewGuid();
            UserDataBaseEntitiesEntities db = new UserDataBaseEntitiesEntities();
            db.UserActivation.Add(new UserActivation { email = Convert.ToString(email), ActivationCode = activationCode });
            db.SaveChanges();

            /*發送信件*/
            string body = "<br /><br />親愛的顧客您好，恭喜你完成註冊濰濰植物網，現在只要點選以下網址就能完成Email認證:";
            body += "<br /><a href = '" + string.Format("{0}://{1}/Main/Activation/{2}", Request.Url.Scheme, Request.Url.Authority, activationCode) + "'>Click here to activate your account.</a>";
            body += "<br /><br />Thanks";
            WebMail.Send(email,"此為系統直接發送信件，請勿回復",body, null, null, null, true, null, null, null, null, null);
        }
        /// <summary>
        /// 搜尋產品
        /// </summary>
        /// <param name="productname"></param>
        /// <returns>返回產品陣列</returns>
        [HttpGet]
        public ActionResult Search(string productname)
        {
            VisitorDataBaseEntities db = new VisitorDataBaseEntities();
            var product = (from x in db.Product where x.Name.Contains(productname) select x);
            var productlist = product.ToList();
            return View("SearchResult", productlist);
        }

    }

   
}