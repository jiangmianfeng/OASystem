using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WJQ.OA.Common;
using WJQ.OA.Model;

namespace WJQ.OA.WebApp.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            bool isSucess = false;
            if (Request.Cookies["sessionId"] != null)
            {
                string sessionId = Request.Cookies["sessionId"].Value;
                Object obj = MemcacheHelper.Get(sessionId);
                if (obj!=null)
                {
                    UserInfo userInfo = SerializeHelper.DeserializeToObject<UserInfo>(obj.ToString());
                    isSucess = true;
                    MemcacheHelper.Set(sessionId, obj, DateTime.Now.AddMinutes(20));
                }
                if (!isSucess)
                {
                    filterContext.Result = Redirect("/Login/Index");
                }               
            }
            //if (Session["UserInfo"]==null)
            //{
            //    //filterContext.HttpContext.Response.Redirect("/Login/Index");
            //    filterContext.Result = Redirect("/Login/Index");
            //}
        }
    }
}