using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReadyPlayerSite.Models;

namespace ReadyPlayerSite.Repository
{
    public class GenericRepository<T> : IGenericRepository<T>
        where T : IModel
    {

        public GenericRepository(IStorageContext<T> storage)
        {
            this._entities = storage;
        }

        //This is the storage context where the data will be stored.
        private IStorageContext<T> _entities;
        public IStorageContext<T> Context
        {
            get { return _entities; }
            set { _entities = value; }
        }
        public virtual IQueryable<T> GetAll()
        {

            IQueryable<T> query = _entities.Set();
            return query;
        }
        public IQueryable<T> Where(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {

            IQueryable<T> query = _entities.Set().Where(predicate);
            return query;
        }
         public T Find(int anid)
        {
            return _entities.FindByID(anid);
        }
        public virtual void Add(T entity)
        {
            _entities.Add(entity);
        }
         public virtual void Remove(T entity)
        {
            _entities.Remove(entity);
        }
        public virtual void Edit(T entity)
        {
            _entities.Edit(entity);
        }
        public virtual void SaveChanges()
        {
            _entities.SaveChanges();
        }
         public virtual void UpdateValues(T entity, T item)
        {
            _entities.UpdateValues(entity, item);
        }
         public virtual void Dispose()
         {
             _entities.Dispose();
         }
    }







}
