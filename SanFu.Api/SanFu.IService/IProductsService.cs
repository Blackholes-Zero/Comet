using SanFu.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SanFu.IService
{
    public interface IProductsService : IBaseService
    {
        Task<bool> AddAsync(Products model);

        List<Products> GetPager(int pageIndex, int pageSize, out int total);

        Task<Products> GetById(long Id);

        Task<bool> EditAsync(Products contact);
    }
}
