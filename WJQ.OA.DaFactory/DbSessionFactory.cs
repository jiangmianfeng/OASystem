using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using WJQ.OA.IDal;

namespace WJQ.OA.DaFactory
{
    public class DbSessionFactory
    {
        public static IDbSession CreateDbSession()
        {
            IDbSession dbSession = (IDbSession)CallContext.GetData("DbSession");
            if (dbSession==null)
            {
                dbSession =new DaFactory.DBSession();
                CallContext.SetData("DbSession", dbSession);
            }
            return dbSession;
        }
    }
}
