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

namespace SanFu.Service
{
    public class ProductClassService : BaseService, IProductClassService
    {
        private IProductClassRepository _repository;
        private IUnitOfWork _unitOfWork;
        private IOptions<ApiAccessSettings> _accessSettings;

        public ProductClassService(IProductClassRepository repository, IUnitOfWork unitOfWork, IOptions<ApiAccessSettings> accessSettings)
        {
            this._repository = repository;
            this._unitOfWork = unitOfWork;
            this._accessSettings = accessSettings;
        }

        public async Task<bool> AddAsync(ProductClass model)
        {
            this._repository.Addasync(model);
            int result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> EditAsync(ProductClass model)
        {
            this._repository.Update(model);
            int result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }

        public async Task<List<ProductClass>> GetAllAsync()
        {
            var list = await _repository.GetAllasync(p => p.Id > 0);
            return list;
        }

        public async Task<ProductClass> GetById(long Id)
        {
            var contact = await _repository.GetByIdasync(Id);
            return contact;
        }
    }
}
