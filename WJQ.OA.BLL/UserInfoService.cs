using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WJQ.OA.IBLL;
using WJQ.OA.Model;

namespace WJQ.OA.BLL
{
    public class UserInfoService : BaseService<UserInfo>,IUserInfoService
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

        public override void SetCurrentDal()
        {
            CurrentDal = this.CurrentDBSession.UserInfoDal;
        }
    }
}
