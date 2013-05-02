using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReadyPlayerSite.Models;
using System.Data.Entity;

namespace ReadyPlayerSite.Repository
{
    public interface IStorageContext<T> where T : IModel
    {
        IQueryable<T> Set();
        T FindByID(int id);
        void Add(T entity);
        void Remove(T entity);
        void Edit(T entity);
        void UpdateValues(T entity, T input);
        void SaveChanges();
        void Dispose();
    }
}
