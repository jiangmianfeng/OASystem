using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WJQ.OA.DaFactory;
using WJQ.OA.IDal;

namespace WJQ.OA.BLL
{
    public abstract class BaseService<T> where T: class,new()
    {
        public IDbSession CurrentDBSession
        {
            get
            {
                //return new DBSession();
                return DbSessionFactory.CreateDbSession();
            }
        }
        public IBase<T> CurrentDal { get; set; }
        public abstract void SetCurrentDal();
        public BaseService()
        {
            SetCurrentDal();
        }
        public IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda)
        {
           return CurrentDal.LoadEntities(whereLambda);
        }
        public IQueryable<T> LoadPageEntities<s>(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, s>> orderByLambda, bool isAsc)
        {
            return CurrentDal.LoadPageEntities<s>(pageIndex, pageSize,out totalCount, whereLambda, orderByLambda, isAsc);
        }
        public bool EditEntity(T entity)
        {
            CurrentDal.EditEntity(entity);
            return CurrentDBSession.SaveChanges();
        }
        public bool DeleteEntity(T entity)
        {
            CurrentDal.DeleteEntity(entity);
            return CurrentDBSession.SaveChanges();
        }
        public T AddEntity(T entity)
        {
            CurrentDal.AddEntity(entity);
            CurrentDBSession.SaveChanges();
            return entity;
        }
    }
}
