using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WJQ.OA.BLL;
using WJQ.OA.IBLL;
using WJQ.OA.Model;
using static WJQ.OA.Common.EnumType;

namespace WJQ.OA.WebApp.Controllers
{
    public class RoleInfoController : BaseController
    {
        private IRoleInfoService RoleInfoService { get; set; }
        private IActionInfoService ActionInfoService { get; set; }
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
        public ActionResult ShowRoleAction()
        {
            int id = int.Parse(Request["id"]);
            var roleInfo = RoleInfoService.LoadEntities(r => r.ID == id).FirstOrDefault();//获取要分配的权限的角色信息。
            ViewBag.RoleInfo = roleInfo;
            //获取所有的权限。
            short delFlag = (short)DeleteEnum.Normal;
            var actionInfoList = ActionInfoService.LoadEntities(a => a.DelFlag == delFlag).ToList();
            //要分配权限的角色以前有哪些权限。
            var actionIdList = (from a in roleInfo.ActionInfo
                                select a.ID).ToList();
            ViewBag.ActionInfoList = actionInfoList;
            ViewBag.ActionIdList = actionIdList;
            return View();
        }
        public ActionResult SetRoleAction()
        {
            int roleId = int.Parse(Request["roleId"]);//获取角色编号
            string[] allKeys = Request.Form.AllKeys;//获取所有表单元素name属性的值。
            List<int> list = new List<int>();
            foreach (string key in allKeys)
            {
                if (key.StartsWith("cba_"))
                {
                    string k = key.Replace("cba_", "");
                    list.Add(Convert.ToInt32(k));
                }
            }
            if (RoleInfoService.SetRoleActionInfo(roleId, list))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
    }
}