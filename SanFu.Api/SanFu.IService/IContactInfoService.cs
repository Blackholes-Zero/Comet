using SanFu.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SanFu.IService
{
    public interface IContactInfoService:IBaseService
    {
        Task<bool> AddAsync(ContactInfo contact);

        Task<List<ContactInfo>> GetAllAsync();

        Task<ContactInfo> GetById(long Id);

        Task<bool> EditAsync(ContactInfo contact);
    }
}
