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
    public class UserInfoController : Controller
    {
        //IUserInfoService UserInfoService = new IUserInfoService();
        IUserInfoService UserInfoService { get; set; }// = new UserInfoService();
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
            var userInfo = UserInfoService.LoadEntities(x => x.ID == id).SingleOrDefault();
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
    }
}