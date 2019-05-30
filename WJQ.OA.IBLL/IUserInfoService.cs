using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WJQ.OA.Model;
using WJQ.OA.Model.UserSeach;

namespace WJQ.OA.IBLL
{
    public interface IUserInfoService:IBaseService<UserInfo>
    {
        bool DeleteUserListEntity(List<int> list);
        IQueryable<UserInfo> SeachUserInfo(UserSeach userSeach, short delFlag);
    }
}
