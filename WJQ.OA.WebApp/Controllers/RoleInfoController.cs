using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WJQ.OA.BLL;
using WJQ.OA.Model;
using static WJQ.OA.Common.EnumType;

namespace WJQ.OA.WebApp.Controllers
{
    public class RoleInfoController : BaseController
    {
        private RoleInfoService RoleInfoService { get; set; }
        // GET: RoleInfo
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetRoleInfoList()
        {
            int pageSize = Request["rows"] != null ? Convert.ToInt32(Request["rows"]) : 5;
            int pageIndex = Request["page"] != null ?Convert.ToInt32(Request["page"]) : 1;
            int totalCount = 0;
            short isDelete =(short)DeleteEnum.Normal;
            var RoleList = RoleInfoService.LoadPageEntities(pageIndex, pageSize, out totalCount, x => x.DelFlag == isDelete, x => x.ID, true);
            var temp = from u in RoleList
                       select new
                       {
                           ID=u.ID,
                           RoleName=u.RoleName,
                           Sort=u.Sort,
                           Remark=u.Remark,
                           SubTime=u.SubTime
                       };
            return Json(new { rows = temp, total = totalCount }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ShowAddInfo()
        {
            return View();
        }
        public ActionResult AddRoleInfo(RoleInfo role)
        {
            role.SubTime = DateTime.Now;
            role.ModifiedOn = "2018-09-09";
            role.DelFlag = 0;
            RoleInfoService.AddEntity(role);
            return Content("ok");
        }
    }
}