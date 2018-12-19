using SanFu.Entities;
using SanFu.IRepository;
using SanFu.Repository.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Text;

namespace WechatBusiness.Repository
{
    public class AdminInfoRepository : EfRepositoryBase<AdminInfo>, IAdminInfoRepository
    {
        public AdminInfoRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}