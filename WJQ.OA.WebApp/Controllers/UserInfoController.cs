using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using WJQ.OA.BLL;
using WJQ.OA.IBLL;
using WJQ.OA.Model;
using WJQ.OA.Model.UserSeach;
using static WJQ.OA.Common.EnumType;

namespace WJQ.OA.WebApp.Controllers
{
    public class UserInfoController : BaseController
    {
        //IUserInfoService UserInfoService = new IUserInfoService();
        IUserInfoService UserInfoService { get; set; }// = new UserInfoService();
        IRoleInfoService RoleInfoService { get; set; }
        IActionInfoService ActionInfoService { get; set; }
        IR_UserInfo_ActionInfoService R_UserInfo_ActionInfoService { get; set; }
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
            int pageCount=0;
            string userName = Request["userName"];
            string remark = Request["remark"];
            UserSeach userSeach = new UserSeach() {
                PageSize = pageSize,
                PageIndex=pageIndex,
                Remark=remark,
                TotalCount=pageCount,
                UserName=userName
            };

            short IsDelete = (short)DeleteEnum.Normal;
            var UserList = UserInfoService.SeachUserInfo(userSeach, IsDelete);
            //var UserList = UserInfoService.LoadPageEntities<int>(pageIndex, pageSize, out pageCount, c => c.DelFlag == IsDelete, c => c.ID, true);
            var temp = from u in UserList
                       select new
                       {
                           ID = u.ID,
                           UName = u.UName,
                           UPwd = u.UPwd,
                           Remark = u.Remark,
                           SubTime=u.SubTime
                       };
            return Json(new { rows = temp, total = userSeach.TotalCount },JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteUserInfo()
        {
            string UserId = Request["strId"];
            string[] UserIds = UserId.Split(',');
            List<int> list = new List<int>();
            foreach (var id in UserIds)
            {
                list.Add(Convert.ToInt32(id));
            }
            if (UserInfoService.DeleteUserListEntity(list))
            {
                return  Content("ok");
            }
            else
            {
                return  Content("no");
            }
        }
        [HttpPost]
        public ActionResult AddUserInfo(UserInfo userInfo)
        {
            userInfo.DelFlag = 0;
            userInfo.ModifiedOn = DateTime.Now;
            userInfo.SubTime = DateTime.Now;
            UserInfoService.AddEntity(userInfo);
            return Content("ok");
        }
        public ActionResult ShowUserInfo()
        {
            
            int id;
            if (!int.TryParse(Request["id"],out id))
            {
                return Json("no");
            }
            var user = UserInfoService.LoadEntities(x => x.ID == id).SingleOrDefault();
            JsonSerializerSettings setting = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            var userInfo = JsonConvert.SerializeObject(user, setting);
            return Json(userInfo, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateUserInfo(UserInfo userInfo)
        {
            userInfo.ModifiedOn = DateTime.Now;
            if (UserInfoService.EditEntity(userInfo))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        public ActionResult ShowUserRoleInfo()
        {
            int id = int.Parse(Request["id"]);
            var userInfo = UserInfoService.LoadEntities(u => u.ID == id).FirstOrDefault();
            ViewBag.UserInfo = userInfo;
            short delFlag =(short)DeleteEnum.Normal;
            var allRoleList = RoleInfoService.LoadEntities(u => u.DelFlag == delFlag).ToList();
            //查询一下要分配角色的用户以前具有了哪些角色编号
            var allUserRoleIdList = (from x in userInfo.RoleInfo
                                     select x.ID).ToList();
            ViewBag.AllRoleList = allRoleList;
            ViewBag.AllUserRoleIdList = allUserRoleIdList;
            return View();
        }
        public ActionResult SetUserRoleInfo()
        {
            int userId = int.Parse(Request["userId"]);
            string[] allKeys = Request.Form.AllKeys;//获取所有表单元素name属性值。
            List<int> roleIdList = new List<int>();
            foreach (var key in allKeys)
            {
                if (key.StartsWith("cba_"))
                {
                    string s = key.Replace("cba_", "");
                    roleIdList.Add(Convert.ToInt32(s));
                }
            }
            if (UserInfoService.SetUserRoleInfo(userId,roleIdList))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        public ActionResult ShowUserAction()
        {
            int userId = int.Parse(Request["userId"]);
            var userInfo = UserInfoService.LoadEntities(u => u.ID == userId).FirstOrDefault();
            ViewBag.UserInfo = userInfo;
            //获取所有的权限。
            short delFlag = (short)DeleteEnum.Normal;
            var allActionList = ActionInfoService.LoadEntities(a => a.DelFlag == delFlag).ToList();
            //获取要分配的用户已经有的权限。
            var allActionIdList = (from a in userInfo.R_UserInfo_ActionInfo
                                   select a).ToList();
            ViewBag.AllActionList = allActionList;
            ViewBag.AllActionIdList = allActionIdList;
            return View();
        }
        public ActionResult SetUserAction()
        {
            int actionId = Convert.ToInt32(Request["actionId"]);
            int userId = int.Parse(Request["userId"]);
            bool isPass = Request["isPass"] == "true" ? true : false;
            if (UserInfoService.SetUserRoleInfo(actionId,userId,isPass))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        public ActionResult ClearUserAction()
        {
            int actionId = Convert.ToInt32(Request["actionId"]);
            int userId = int.Parse(Request["userId"]);
            var userAction = R_UserInfo_ActionInfoService.LoadEntities(x => x.ActionInfoID == actionId && x.UserInfoID == userId).FirstOrDefault();
            if (userAction!=null)
            {
                if (R_UserInfo_ActionInfoService.DeleteEntity(userAction))
                {
                    return Content("ok:清除成功");
                }
                else
                {
                    return Content("no:清除失败");
                }
            }
            else
            {
                return Content("no:暂无数据");
            }
        }
    }
}