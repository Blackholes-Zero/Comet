using SanFu.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SanFu.IService
{
    public interface IAccountService:IBaseService
    {
        Task<AdminInfo> LoginAsync(string loginName,string passWord);

        Task<bool> AddAsync(AdminInfo admin);
    }
}
