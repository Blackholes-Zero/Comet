using NetCore.Framework;
using SanFu.Entities;
using SanFu.IRepository;
using SanFu.IService;
using SanFu.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Castle.Core.Configuration;
using SanFu.Commons.AppSettings;
using Microsoft.Extensions.Options;

namespace SanFu.Service
{
    public class AccountService : BaseService, IAccountService
    {
        private IAdminInfoRepository _adminInfoRepository;
        private IUnitOfWork _unitOfWork;
        private IOptions<ApiAccessSettings> _accessSettings;

        public AccountService(IAdminInfoRepository adminInfoRepository, IUnitOfWork unitOfWork, IOptions<ApiAccessSettings> accessSettings)
        {
            this._adminInfoRepository = adminInfoRepository;
            this._unitOfWork = unitOfWork;
            this._accessSettings = accessSettings;
        }
        public async Task<bool> AddAsync(AdminInfo admin)
        {
            admin.SaltKey = admin.SaltKey == null ? Guid.NewGuid() : admin.SaltKey;
            admin.PassWord = EncryptDecrypt.EncryptMD5(EncryptDecrypt.EncryptMD5Salt(admin.PassWord, admin.SaltKey).ToUpper());
            admin.Mobile = EncryptDecrypt.Encrypt3DES(admin.Mobile, _accessSettings.Value.Key);
            _adminInfoRepository.Addasync(admin);
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }

        public async Task<AdminInfo> LoginAsync(string loginName, string passWord)
        {
            var listModel = await _adminInfoRepository.GetAllasync(p => p.LoginName == loginName || p.Mobile == EncryptDecrypt.Encrypt3DES(loginName, _accessSettings.Value.Key));
            if (listModel.Any())
            {
                var model = listModel.FirstOrDefault();
                var saltKey = model.SaltKey;
                var passMd5 = EncryptDecrypt.EncryptMD5(EncryptDecrypt.EncryptMD5Salt(passWord, saltKey).ToUpper());
                if (passMd5 == model.PassWord)
                {
                    return model;
                }
            }
            return new AdminInfo();
        }
    }
}
