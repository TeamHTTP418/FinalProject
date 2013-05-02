using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReadyPlayerSite.Models;
using System.Data.Entity;


namespace ReadyPlayerSite.Repository
{
    public class StorageContext<T> : IStorageContext<T> where T : IModel
    {
        DbContext context;


        public StorageContext(DbContext a_context)
        {
            context = a_context;
        }


        public IQueryable<T> Set()
        {
            return context.Set<T>();
        }


        public T FindByID(int anid)
        {
            IQueryable<T> list = context.Set<T>().Where(c => c.ID == anid);
            if (list.Count() > 0)
            {
                return list.First();
            }
            else
            {
                return null;
            }
        }


        public void Add(T entity)
        {
            context.Set<T>().Add(entity);
            context.Entry(entity).State = System.Data.EntityState.Added;
        }


        public void Remove(T entity)
        {
            context.Set<T>().Remove(entity);
        }


        public void Edit(T entity)
        {
            context.Entry(entity).State = System.Data.EntityState.Modified;
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }


        public void UpdateValues(T entity, T input)
        {
            context.Entry(entity).CurrentValues.SetValues(input);
        }

        public void Dispose()
        {
            context.Dispose();
        }

    }
}
