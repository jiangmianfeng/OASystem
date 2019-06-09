using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WJQ.OA.IBLL;
using WJQ.OA.Model;
using WJQ.OA.Model.UserSeach;

namespace WJQ.OA.BLL
{
    public partial class UserInfoService : BaseService<UserInfo>,IUserInfoService
    {
        public bool DeleteUserListEntity(List<int> list)
        {
            var Userlist = this.CurrentDBSession.UserInfoDal.LoadEntities(x => list.Contains(x.ID));
            foreach (var user in Userlist)
            {
                this.CurrentDBSession.UserInfoDal.DeleteEntity(user);
            }
            return this.CurrentDBSession.SaveChanges();
        }

        public IQueryable<UserInfo> SeachUserInfo(UserSeach userSeach,short delFlag)
        {
            var temp = this.CurrentDBSession.UserInfoDal.LoadEntities(x => x.DelFlag == delFlag);
            if (!string.IsNullOrEmpty(userSeach.UserName))
            {
                temp = temp.Where<UserInfo>(x => x.UName.Contains(userSeach.UserName));
            }
            if (!string.IsNullOrEmpty(userSeach.Remark))
            {
                temp = temp.Where<UserInfo>(x => x.Remark.Contains(userSeach.Remark));
            }
            userSeach.TotalCount = temp.Count();
            return temp.OrderBy<UserInfo,int>(x => x.ID).Skip<UserInfo>((userSeach.PageIndex - 1) * userSeach.PageSize).Take<UserInfo>(userSeach.PageSize);
        }

        public bool SetUserRoleInfo(int userId, List<int> roleIdList)
        {
            var userInfo = this.CurrentDBSession.UserInfoDal.LoadEntities(x => x.ID == userId).FirstOrDefault();
            if (userInfo!=null)
            {
                userInfo.RoleInfo.Clear();
                foreach(int roleId in roleIdList)
                {
                    var roleInfo = this.CurrentDBSession.RoleInfoDal.LoadEntities(x => x.ID == roleId).FirstOrDefault();
                    userInfo.RoleInfo.Add(roleInfo);
                }
                return this.CurrentDBSession.SaveChanges();
            }
            return false;
        }

        public bool SetUserRoleInfo(int actionId, int userId, bool isPass)
        {
            var userAction = this.CurrentDBSession.R_UserInfo_ActionInfoDal.LoadEntities(x => x.UserInfoID == userId&&x.ActionInfoID==actionId).FirstOrDefault();
            if (userAction==null)
            {
                R_UserInfo_ActionInfo userInfo_ActionInfo = new R_UserInfo_ActionInfo();
                userInfo_ActionInfo.IsPass = isPass;
                userInfo_ActionInfo.UserInfoID = userId;
                userInfo_ActionInfo.ActionInfoID = actionId;
                this.CurrentDBSession.R_UserInfo_ActionInfoDal.AddEntity(userInfo_ActionInfo);
            }
            else
            {
                userAction.IsPass = isPass;
                this.CurrentDBSession.R_UserInfo_ActionInfoDal.EditEntity(userAction);
            }
            return this.CurrentDBSession.SaveChanges();
        }


        //public override void SetCurrentDal()
        //{
        //    CurrentDal = this.CurrentDBSession.UserInfoDal;
        //}
    }
}
