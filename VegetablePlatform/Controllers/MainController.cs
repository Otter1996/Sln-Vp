using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using VegetablePlatform.Models;
using VegetablePlatform.BusinessLogic;
using System.Web.Security;
using System.Security.Principal;

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
            FormsAuthentication.SignOut();
            HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);
            return View("Main");
        }
        /// <summary>
        /// 登入機制
        /// </summary>
        [HttpPost]
        public ActionResult Login(string UserId, string UserPassword)
        {
            LoginLogic login = new LoginLogic();
            UserData LoginUser = login.IsSuccessLogin(UserId, HashFunction.Hashfun(UserPassword));
            if (LoginUser == null)
            {
                ViewBag.Message = "登入失敗，請重新輸入";
                return View("Login");
            }
            else
            {
                AuthenticationUserAndWriteIntoCookie(LoginUser);
                return RedirectToAction("Main");
            }
            
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
                if (post["Verify"] == member.UserActivation.Random)
                {
                    member.password = newpassword;
                    member.UserActivation.Random = null;
                    db.SaveChanges();

                    ViewBag.ChangePassword = "更改密碼成功!";
                    return View();
                }
                else
                {
                    ViewBag.ChangePassword = "輸入驗證碼錯誤，請重新傳送驗證碼。";
                    member.UserActivation.Random = null;
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
            var member = db.UserData.Where(m => m.account == userdata.account && HashFunction.Hashfun(m.password) == userdata.password).FirstOrDefault();
            if (member == null)
            {
                userdata.password = HashFunction.Hashfun(userdata.password);
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
            ViewBag.Message = "驗證失敗";
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
            UserDataBaseEntitiesEntities db = new UserDataBaseEntitiesEntities();
            var product = (from x in db.Product where x.Name.Contains(productname) select x);
            var productlist = product.ToList();
            return View("SearchResult", productlist);
        }
        /// <summary>
        /// 使用Authetication做出登入機制
        /// </summary>
        /// <param name="LoginUser"></param>
        public void AuthenticationUserAndWriteIntoCookie(UserData LoginUser)
        {
            DateTime DTnow = DateTime.Now;
            var authTicket = new FormsAuthenticationTicket(   // 登入成功，取得門票 (票證)。請自行填寫以下資訊。
                version: 1,   //版本號（Ver.）
                name: LoginUser.account, // ***自行放入資料（如：使用者帳號、真實名稱）
                issueDate: DTnow,  // 登入成功後，核發此票證的本機日期和時間（資料格式 DateTime）
                expiration: DTnow.AddDays(1),  //  "一天"內都有效（票證到期的本機日期和時間。）
                isPersistent: true,  // 記住我？ true or false（畫面上通常會用 CheckBox表示）

                userData: Convert.ToString(LoginUser.group),   // ***自行放入資料（如：會員權限、等級、群組） 
                                    // 與票證一起存放的使用者特定資料。
                                    // 需搭配 Global.asax設定檔 - Application_AuthenticateRequest事件。
                cookiePath: FormsAuthentication.FormsCookiePath
                );

            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket))
            {   // 重點！！避免 Cookie遭受攻擊、盜用或不當存取。請查詢關鍵字「」。
                HttpOnly = true  // 必須上網透過http才可以存取Cookie。不允許用戶端（寫前端程式）存取
                                 //Secure = true;      // 需要搭配https（SSL）才行。
            };

            if (authTicket.IsPersistent)
            {
                authCookie.Expires = authTicket.Expiration;    // Cookie過期日（票證到期的本機日期和時間。）
            }
            Response.Cookies.Add(authCookie);   // 完成 Cookie，寫入使用者的瀏覽器與設備中
        }

    }

   
}