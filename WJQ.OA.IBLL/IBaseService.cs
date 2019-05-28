using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WJQ.OA.IDal;

namespace WJQ.OA.IBLL
{
    public interface IBaseService<T> where T:class,new()
    {
        IDbSession CurrentDBSession { get; }
        IBase<T> CurrentDal { get; set; }
        IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda);
        IQueryable<T> LoadPageEntities<s>(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, s>> orderByLambda, bool isAsc);
        bool EditEntity(T entity);
        bool DeleteEntity(T entity);
        T AddEntity(T entity);
    }
}
