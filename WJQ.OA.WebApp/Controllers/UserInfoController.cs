using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using WJQ.OA.BLL;
using WJQ.OA.IBLL;
using WJQ.OA.Model;
using static WJQ.OA.Common.EnumType;

namespace WJQ.OA.WebApp.Controllers
{
    public class UserInfoController : Controller
    {
        //IUserInfoService UserInfoService = new IUserInfoService();
        UserInfoService UserInfoService = new UserInfoService();
        // GET: UserInfo
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetUserInfoList()
        {
            int pageIndex = Request["page"]!=null?Convert.ToInt32(Request["page"]):1;
            int pageSize = Request["rows"] != null ? Convert.ToInt32(Request["rows"]) : 5;
            int pageCount;
            short IsDelete = (short)DeleteEnum.Normal;
            var UserList = UserInfoService.LoadPageEntities<int>(pageIndex, pageSize, out pageCount, c => c.DelFlag == IsDelete, c => c.ID, true);
            var temp = from u in UserList
                       select new
                       {
                           ID = u.ID,
                           UName = u.UName,
                           UPwd = u.UPwd,
                           Remark = u.Remark,
                           SubTime=u.SubTime
                       };
            return Json(new { rows = temp, total = pageCount });
        }
    }
}