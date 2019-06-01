using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WJQ.OA.Dal;
using WJQ.OA.IDal;
using WJQ.OA.Model;

namespace WJQ.OA.DaFactory
{
    public partial class DBSession: IDbSession
    {
        //OAEntities Db = new OAEntities();
        public DbContext Db
        {
            get
            {
                return DalFactory.CreateDbContext();
            }
        }
        //public IUserInfoDal _UserInfoDal;
        //public IUserInfoDal UserInfoDal
        //{
        //    get
        //    {
        //        if (_UserInfoDal==null)
        //        {
        //            //_UserInfoDal = new UserInfoDal();
        //            _UserInfoDal = AbstractFactory.CreateUserInfoDal();
        //        }
        //        return _UserInfoDal;
        //    }
        //    set
        //    {
        //        _UserInfoDal = value;
        //    }
        //}
        public bool SaveChanges()
        {
            return Db.SaveChanges()>0;
        }
    }
}
