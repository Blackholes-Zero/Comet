using Microsoft.Extensions.Options;
using NetCore.Framework;
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
    public class ContactInfoService : BaseService, IContactInfoService
    {
        private IContactInfoRepository _repository;
        private IUnitOfWork _unitOfWork;
        private IOptions<ApiAccessSettings> _accessSettings;

        public ContactInfoService(IContactInfoRepository repository, IUnitOfWork unitOfWork, IOptions<ApiAccessSettings> accessSettings)
        {
            this._repository = repository;
            this._unitOfWork = unitOfWork;
            this._accessSettings = accessSettings;
        }

        public async Task<bool> AddAsync(ContactInfo contact)
        {
            contact.Mobile= EncryptDecrypt.Encrypt3DES(contact.Mobile, _accessSettings.Value.Key);
            contact.HotwireTelephone = EncryptDecrypt.Encrypt3DES(contact.HotwireTelephone, _accessSettings.Value.Key);
            contact.Email = EncryptDecrypt.Encrypt3DES(contact.Email, _accessSettings.Value.Key);
            contact.QQ = EncryptDecrypt.Encrypt3DES(contact.QQ, _accessSettings.Value.Key);

            this._repository.Addasync(contact);
            int result= await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> EditAsync(ContactInfo contact)
        {
            contact.Mobile = EncryptDecrypt.Encrypt3DES(contact.Mobile, _accessSettings.Value.Key);
            contact.HotwireTelephone = EncryptDecrypt.Encrypt3DES(contact.HotwireTelephone, _accessSettings.Value.Key);
            contact.Email = EncryptDecrypt.Encrypt3DES(contact.Email, _accessSettings.Value.Key);
            contact.QQ = EncryptDecrypt.Encrypt3DES(contact.QQ, _accessSettings.Value.Key);

            this._repository.Update(contact);
            int result = await _unitOfWork.SaveChangesAsync();
            return result > 0;

        }

        public async Task<List<ContactInfo>> GetAllAsync()
        {
            var list= await _repository.GetAllasync(p => p.Id > 0);
            return list;
        }

        public async Task<ContactInfo> GetById(long Id)
        {
            var contact = await _repository.GetByIdasync(Id);
            contact.Mobile = EncryptDecrypt.Decrypt3DES(contact.Mobile, _accessSettings.Value.Key);
            contact.HotwireTelephone = EncryptDecrypt.Decrypt3DES(contact.HotwireTelephone, _accessSettings.Value.Key);
            contact.Email = EncryptDecrypt.Decrypt3DES(contact.Email, _accessSettings.Value.Key);
            contact.QQ = EncryptDecrypt.Decrypt3DES(contact.QQ, _accessSettings.Value.Key);
            return contact;
        }
    }
}
