using CZBK.ItcastOA.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WJQ.OA.BLL;
using WJQ.OA.IBLL;
using WJQ.OA.Model;

namespace WJQ.OA.WebApp.Controllers
{
    public class LoginController : Controller
    {
        IUserInfoService UserInfoService { get; set; }
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UserInfoLogin()
        {
            string validateCode = Session["ValidateImage"] != null ? Session["ValidateImage"].ToString() : String.Empty;
            if (string.IsNullOrEmpty(validateCode))
            {
                return Content("no:验证码错误");
            }
            Session["ValidateImage"] = null;
            string code = Request["validateCode"];
            if (!code.Equals(validateCode,StringComparison.InvariantCultureIgnoreCase))
            {
                return Content("no:验证码错误");
            }
            string userName = Request["loginName"] != null ? Request["loginName"] : String.Empty;
            string userPwd = Request["loginPwd"] != null ? Request["loginPwd"] : String.Empty;
            
            UserInfo userInfo = UserInfoService.LoadEntities(x => x.UName == userName && x.UPwd == userPwd).SingleOrDefault();

            if (userInfo!=null)
            {
                return Content("ok:登录成功");
                //Response.Redirect("/Home/Index");               
            }
            else
            {
                return Content("no:登录失败");
            }
        }
        public ActionResult ValidateLogin()
        {
            ValidateCode validate = new ValidateCode();
            string imageValidate = validate.CreateValidateCode(4);
            byte[] be = validate.CreateValidateGraphic(imageValidate);
            Session["ValidateImage"] = imageValidate;
            return File(be,"image/jpeg");
        }
    }
}