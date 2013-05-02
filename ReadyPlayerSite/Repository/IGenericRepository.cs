using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;

namespace ReadyPlayerSite.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        IQueryable<T> Where(Expression<Func<T, bool>> predicate);
        T Find(int id);
        void Add(T entity);
        void Remove(T entity);
        void Edit(T entity);
        void UpdateValues(T entity, T item);
        void SaveChanges();
        void Dispose();
    }
}
