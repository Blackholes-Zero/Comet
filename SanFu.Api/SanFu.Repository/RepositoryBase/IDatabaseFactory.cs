using SanFu.DataSource;
using System;
using System.Collections.Generic;
using System.Text;
namespace SanFu.Repository.RepositoryBase
{
    public interface IDatabaseFactory : IDisposable
    {
        EfDbContext GetEfDbContext();
    }
}