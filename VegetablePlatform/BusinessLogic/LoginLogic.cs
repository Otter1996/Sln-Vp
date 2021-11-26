using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VegetablePlatform.Models;

namespace VegetablePlatform.BusinessLogic
{
    public class LoginLogic
    {
        public UserData IsSuccessLogin(string UserId, string UserPassword)
        {
            UserDataBaseEntitiesEntities db = new UserDataBaseEntitiesEntities();
            var member = db.UserData.Where(m => m.account == UserId && m.password == UserPassword).FirstOrDefault();
            if (member == null)
            {
                return null;
            }
            return member;
        }
    }
}