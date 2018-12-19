using SanFu.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SanFu.IService
{
    public interface IProductClassService: IBaseService
    {
        Task<bool> AddAsync(ProductClass model);

        Task<List<ProductClass>> GetAllAsync();

        Task<ProductClass> GetById(long Id);

        Task<bool> EditAsync(ProductClass model);
    }
}
