using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using WJQ.OA.Model;

namespace WJQ.OA.DaFactory
{
    public class DalFactory
    {
        public static DbContext CreateDbContext()
        {
            DbContext dbContext = (DbContext)CallContext.GetData("DbContext");
            if (dbContext == null)
            {
                dbContext = new OAEntities();
                CallContext.SetData("DbContext", dbContext);
            }
            return dbContext;
        }

    }
}
