using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using VegetablePlatform.Models;

namespace VegetablePlatform.BusinessLogic
{
    public class LoginLogic
    {
        public bool IsSuccessLogin(string UserId, string UserPassword)
        {
            UserDataBaseEntitiesEntities db = new UserDataBaseEntitiesEntities();
            var member = db.UserData.Where(m => m.account == UserId && m.password == UserPassword).FirstOrDefault();
            if (member == null)
            {
                return false;
            }
            System.Web.HttpContext.Current.Session["WelCome"] = member.name + "，歡迎光臨";
            System.Web.HttpContext.Current.Session["Member"] = member;
            return true;
        }
    }

    /// <summary>
    /// 自定義不同按鈕選擇Action
    /// </summary>
    public class MultiButtonAttribute : ActionNameSelectorAttribute
    {
        public string Name { get; set; }
        public MultiButtonAttribute(string name)
        {
            this.Name = name;
        }
        public override bool IsValidName(ControllerContext controllerContext,
            string actionName, System.Reflection.MethodInfo methodInfo)
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                return false;
            }
            return controllerContext.HttpContext.Request.Form.AllKeys.Contains(this.Name);
        }
    }
}