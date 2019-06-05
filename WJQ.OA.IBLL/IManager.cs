 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WJQ.OA.Model;
using WJQ.OA.Model.UserSeach;

namespace WJQ.OA.IBLL
{
	
	public partial interface IActionInfoService : IBaseService<ActionInfo>
    {
       
    }   
	
	public partial interface IDepartmentService : IBaseService<Department>
    {
       
    }   
	
	public partial interface IR_UserInfo_ActionInfoService : IBaseService<R_UserInfo_ActionInfo>
    {
       
    }

    public partial interface IRoleInfoService : IBaseService<RoleInfo>
    {
        
    }

    public partial interface IUserInfoService : IBaseService<UserInfo>
    {
       
    }   
	
}