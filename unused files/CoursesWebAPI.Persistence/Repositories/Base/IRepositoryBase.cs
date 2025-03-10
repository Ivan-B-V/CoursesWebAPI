﻿using CoursesWebAPI.Core.Common;
using System.Linq.Expressions;

namespace CoursesWebAPI.Persistence.Repositories.Base
{
    internal interface IRepositoryBase<T> where T : Entity
    {
        public IQueryable<T> FindAll(bool trackChanges);
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges);
        public void Add(T entity);
        public void Update(T entity);
        public void Delete(T entity);
    }
}
