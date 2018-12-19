
using Microsoft.EntityFrameworkCore;
using SanFu.DataSource;
using SanFu.Entities;
using SanFu.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SanFu.Repository.RepositoryBase
{
    public class EfRepositoryBase<T> : IRepository<T> where T : Entity
    {
        private EfDbContext dataContext;
        private DbSet<T> dbSet;

        protected IDatabaseFactory DbFactory
        {
            private set;
            get;
        }

        public EfDbContext DataContext
        {
            get { return dataContext ?? (dataContext = DbFactory.GetEfDbContext()); }
        }

        public EfRepositoryBase(IDatabaseFactory databaseFactory)
        {
            DbFactory = databaseFactory;
            dbSet = DataContext.Set<T>();
        }

        public virtual async void Add(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        //新增方法
        public virtual void AddAll(IEnumerable<T> entities)
        {
            dbSet.AddRangeAsync(entities);
        }

        public virtual void Update(T entity)
        {
            dbSet.Attach(entity);
            dataContext.Entry(entity).State = EntityState.Modified;
        }

        //新增方法
        public virtual void Update(IEnumerable<T> entities)
        {
            foreach (T obj in entities)
            {
                dbSet.Attach(obj);
                dataContext.Entry(obj).State = EntityState.Modified;
            }
        }

        public virtual void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = dbSet.Where<T>(where).AsEnumerable();
            dbSet.RemoveRange(objects);
        }

        //新增方法
        public virtual void DeleteAll(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }

        public virtual void Clear()
        {
            throw new NotImplementedException();
        }

        public virtual T GetById(long id)
        {
            return dbSet.Find(id);
        }

        public virtual T GetById(string id)
        {
            return dbSet.Find(id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return dbSet.ToList();
        }

        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where).ToList();
        }

        public T Get(Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where).FirstOrDefault<T>();
        }

        public virtual IEnumerable<T> GetAllLazy()
        {
            return dbSet;
        }

        public virtual async void Addasync(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        public virtual async Task<List<T>> GetAllasync(Expression<Func<T, bool>> where)
        {
            return await dbSet.Where(where).ToListAsync<T>();
        }

        public virtual async Task<T> GetByIdasync(long id)
        {
            return await dbSet.FindAsync(id);
        }

        /// <summary>
        /// 分页查询 + 条件查询 + 排序
        /// </summary>
        /// <typeparam name="Tkey">泛型</typeparam>
        /// <param name="pageSize">每页大小</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="total">总数量</param>
        /// <param name="whereLambda">查询条件</param>
        /// <param name="orderbyLambda">排序条件</param>
        /// <param name="isAsc">是否升序</param>
        /// <returns>IQueryable 泛型集合</returns>
        public virtual IQueryable<T> LoadPageItems<Tkey>( int pageIndex,int pageSize, out int total, Expression<Func<T, bool>> whereLambda, Func<T, Tkey> orderbyLambda, bool isAsc=true)
        {
            total = dbSet.Where(whereLambda).Count();
            if (isAsc)
            {
                var temp = dbSet.Where(whereLambda)
                             .OrderBy<T, Tkey>(orderbyLambda)
                             .Skip(pageSize * (pageIndex - 1))
                             .Take(pageSize);
                return temp.AsQueryable();
            }
            else
            {
                var temp = dbSet.Where(whereLambda)
                           .OrderByDescending<T, Tkey>(orderbyLambda)
                           .Skip(pageSize * (pageIndex - 1))
                           .Take(pageSize);
                return temp.AsQueryable();
            }
        }

    }
}