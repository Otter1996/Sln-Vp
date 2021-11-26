using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace VegetablePlatform.BusinessLogic
{
    public enum Group
    {
        [Description("管理者")]
        Admin = 1,
        [Description("一般使用者")]
        User = 2,
    }
}