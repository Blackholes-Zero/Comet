using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SanFu.Repository.UnitOfWork
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// 提交更改
        /// </summary>
        /// <returns></returns>
        int SaveChanges();

        /// <summary>
        /// 提交更改异步
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// 开启事务
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// 提交事务
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// 回滚事务
        /// </summary>
        void RollBackTransaction();
    }
}