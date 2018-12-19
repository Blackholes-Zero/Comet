using Autofac;
using Autofac.Extras.DynamicProxy;
using SanFu.Commons;
using SanFu.DataSource;
using SanFu.IRepository;
using SanFu.Repository.RepositoryBase;
using SanFu.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;


namespace SanFu.Repository
{
    public class RepositoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EfUnitOfWork<EfDbContext>>()
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();
            builder.RegisterType<DatabaseFactory>()
                .As<IDatabaseFactory>()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(this.ThisAssembly)
                .Where(t => t.IsAssignableTo<IBaseRepository>())
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                //.PropertiesAutowired()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(AopInterceptor)); ;
        }
    }
}