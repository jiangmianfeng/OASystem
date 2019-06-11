using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WJQ.OA.Common;
using WJQ.OA.IBLL;
using WJQ.OA.Model;

namespace WJQ.OA.WebApp.Controllers
{
    public class HomeController : BaseController
    {
        IUserInfoService UserInfoService { get; set; }
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult HomePage()
        {
            return View();
        }
        /// <summary>
        /// 1: 可以按照用户---角色---权限这条线找出登录用户的权限，放在一个集合中。
        /// 2：可以按照用户---权限这条线找出用户的权限，放在一个集合中。
        /// 3：将这两个集合合并成一个集合。
        /// 4：把禁止的权限从总的集合中清除。
        /// 5：将总的集合中的重复权限清除。
        /// 6：把过滤好的菜单权限生成JSON返回。
        /// </summary>
        /// <returns></returns>
        public ActionResult GetMenu()
        {
            var userInfo = UserInfoService.LoadEntities(x => x.ID == LoginUser.ID).FirstOrDefault();
            var userRole = userInfo.RoleInfo;
            short ActionType = (short)EnumType.ActionEnum.MenuType;
            var userRoleAction = (from r in userRole
                                  from a in r.ActionInfo
                                  where a.ActionTypeEnum==ActionType
                                  select a).ToList();
            var userAction = from a in userInfo.R_UserInfo_ActionInfo
                             select a.ActionInfo;
            var userMenuAction = (from a in userAction
                                  where a.ActionTypeEnum == ActionType
                                  select a).ToList();
            userRoleAction.AddRange(userMenuAction);
            var errorAction = (from a in userInfo.R_UserInfo_ActionInfo
                               where a.IsPass == false
                               select a.ActionInfoID).ToList();
            var loginUser = userRoleAction.Where(x =>!errorAction.Contains(x.ID));

            var allLoginUser = loginUser.Distinct(new EqualityComparer());

            var data = from u in allLoginUser
                       select new
                       {
                           icon = u.MenuIcon,
                           title = u.ActionInfoName,
                           url = u.Url
                       };
            return Json(data,JsonRequestBehavior.AllowGet);
        }
    }
}