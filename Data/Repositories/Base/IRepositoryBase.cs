using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Phonebook.Data.Repositories.Base
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll(bool trackChanges);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges);
        Task Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}