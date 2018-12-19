using Autofac;
using Autofac.Extras.DynamicProxy;
using SanFu.Commons;
using SanFu.IService;
using System;
using System.Collections.Generic;
using System.Text;

namespace SanFu.Service
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(this.ThisAssembly)
                .Where(t => t.IsAssignableTo<IBaseService>())
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                //.PropertiesAutowired();
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(AopInterceptor)); ;
        }
    }
}
