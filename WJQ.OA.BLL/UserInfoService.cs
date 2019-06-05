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

        //public override void SetCurrentDal()
        //{
        //    CurrentDal = this.CurrentDBSession.UserInfoDal;
        //}
    }
}
