using SanFu.Entities;
using SanFu.IRepository;
using SanFu.Repository.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Text;

namespace SanFu.Repository
{
    public class ProductClassRepository : EfRepositoryBase<ProductClass>, IProductClassRepository
    {
        public ProductClassRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}
