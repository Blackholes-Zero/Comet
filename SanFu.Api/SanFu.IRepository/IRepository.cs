using SanFu.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SanFu.IRepository
{
    public interface IRepository<T> : IBaseRepository where T : Entity
    {
        void Add(T entity);

        void AddAll(IEnumerable<T> entities);

        void Update(T entity);

        void Update(IEnumerable<T> entities);

        void Delete(T entity);

        void Delete(Expression<Func<T, bool>> where);

        void DeleteAll(IEnumerable<T> entities);

        void Clear();

        T GetById(long Id);

        T GetById(string Id);

        T Get(Expression<Func<T, bool>> where);

        IEnumerable<T> GetAll();

        IEnumerable<T> GetMany(Expression<Func<T, bool>> where);

        IEnumerable<T> GetAllLazy();

        void Addasync(T entity);

        Task<List<T>> GetAllasync(Expression<Func<T, bool>> where);

        Task<T> GetByIdasync(long Id);


        IQueryable<T> LoadPageItems<Tkey>(int pageIndex, int pageSize,out int total, Expression<Func<T, bool>> whereLambda, Func<T, Tkey> orderbyLambda, bool isAsc=true);
    }
}
