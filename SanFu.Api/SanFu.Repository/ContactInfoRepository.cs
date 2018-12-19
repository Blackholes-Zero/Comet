using SanFu.Entities;
using SanFu.IRepository;
using SanFu.Repository.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Text;

namespace SanFu.Repository
{
    public class ContactInfoRepository : EfRepositoryBase<ContactInfo>, IContactInfoRepository
    {
        public ContactInfoRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}
