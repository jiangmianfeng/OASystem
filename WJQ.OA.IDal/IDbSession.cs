using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJQ.OA.IDal
{
    public interface IDbSession
    {
        DbContext Db { get; }
        IUserInfoDal UserInfoDal { get; set; }
        bool SaveChanges();
    }
}
