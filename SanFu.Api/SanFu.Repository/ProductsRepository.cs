using SanFu.Entities;
using SanFu.IRepository;
using SanFu.Repository.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Text;

namespace SanFu.Repository
{
    public class ProductsRepository : EfRepositoryBase<Products>, IProductsRepository
    {
        public ProductsRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}
