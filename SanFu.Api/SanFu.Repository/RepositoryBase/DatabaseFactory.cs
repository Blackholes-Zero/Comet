using SanFu.DataSource;
using System;
using System.Collections.Generic;
using System.Text;

namespace SanFu.Repository.RepositoryBase
{
    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        private EfDbContext dataContext;

        public DatabaseFactory(EfDbContext efDbContext)
        {
            this.dataContext = efDbContext;
        }

        public EfDbContext GetEfDbContext()
        {
            return dataContext ?? throw new ArgumentNullException(nameof(dataContext));
        }

        public override void DisposeCore()
        {
            if (dataContext != null)
                dataContext.Dispose();
        }
    }
}