using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WJQ.OA.Model;

namespace WJQ.OA.Dal
{
    public class BaseDal<T> where T:class ,new()
    {

        OAEntities db = new OAEntities();

        public T AddEntity(T entity)
        {
            db.Set<T>().Add(entity);
            db.SaveChanges();
            return entity;
        }

        public bool DeleteEntity(T entity)
        {
            db.Entry<T>(entity).State = EntityState.Deleted;
            return db.SaveChanges() > 0;
        }

        public bool EditEntity(T entity)
        {
            db.Entry<T>(entity).State = EntityState.Modified;
            return db.SaveChanges() > 0;
        }

        public IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda)
        {
            return db.Set<T>().Where(whereLambda);
        }

        public IQueryable<T> LoadPageEntities<s>(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, s>> orderByLambda, bool isAsc)
        {
            var temp = db.Set<T>().Where(whereLambda);
            totalCount = temp.Count();
            if (isAsc)
            {
                temp.OrderBy<T, s>(orderByLambda).Skip<T>((pageIndex - 1) * pageSize).Take(pageSize);
            }
            else
            {
                temp.OrderByDescending<T, s>(orderByLambda).Skip<T>((pageIndex - 1) * pageSize).Take(pageSize);
            }
            return temp;
        }
    }
}
