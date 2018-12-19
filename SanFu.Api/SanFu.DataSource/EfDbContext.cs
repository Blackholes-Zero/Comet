using Microsoft.EntityFrameworkCore;
using SanFu.Entities;
using System;

namespace SanFu.DataSource
{
    public class EfDbContext : DbContext
    {
        public EfDbContext(DbContextOptions<EfDbContext> options) : base(options)
        {
        }

        public virtual DbSet<AdminInfo> AdminUsers { get; set; }

        public virtual DbSet<ContactInfo> ContactInfo { get; set; }

        public virtual DbSet<ProductClass> ProductClass { get; set; }

        public virtual DbSet<Products> Products { get; set; }
    }
}
