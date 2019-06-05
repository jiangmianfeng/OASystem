using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WJQ.OA.Model;

namespace WJQ.OA.IBLL
{
    public partial interface IRoleInfoService : IBaseService<RoleInfo>
    {
        bool SetRoleActionInfo(int roleId, List<int> actionIdList);
    }
}
