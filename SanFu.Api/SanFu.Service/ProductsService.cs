using Microsoft.Extensions.Options;
using SanFu.Commons.AppSettings;
using SanFu.Entities;
using SanFu.IRepository;
using SanFu.IService;
using SanFu.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace SanFu.Service
{
    public class ProductsService : BaseService, IProductsService
    {
        private IProductsRepository _repository;
        private IUnitOfWork _unitOfWork;
        private IOptions<ApiAccessSettings> _accessSettings;

        public ProductsService(IProductsRepository repository, IUnitOfWork unitOfWork, IOptions<ApiAccessSettings> accessSettings)
        {
            this._repository = repository;
            this._unitOfWork = unitOfWork;
            this._accessSettings = accessSettings;
        }

        public async Task<bool> AddAsync(Products model)
        {
            this._repository.Addasync(model);
            int result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> EditAsync(Products model)
        {
            this._repository.Update(model);
            int result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }

        public List<Products> GetPager(int pageIndex, int pageSize,out int total)
        {
            var list = _repository.LoadPageItems(pageIndex, pageSize, out total, p => p.Id > 0, p => p.Id).ToList();
            return list;
        }

        public async Task<Products> GetById(long Id)
        {
            var model = await _repository.GetByIdasync(Id);
            return model;
        }
    }
}
