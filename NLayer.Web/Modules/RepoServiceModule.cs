﻿using Autofac;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Repository;
using NLayer.Repository.Repositories;
using NLayer.Repository.UnitOfWork;
using NLayer.Service.Mapping;
using NLayer.Service.Services;
using System.Reflection;
using Module = Autofac.Module;

namespace NLayer.Web.Modules
{
    public class RepoServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(Service<>)).As(typeof(IService<>)).InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            //ilk bir assemblyleri alalım.
            var apiAssembly = Assembly.GetExecutingAssembly(); //üzerinde olduğumuz asseblyi al api.
            var repoAssembly = Assembly.GetAssembly(typeof(AppDbContext)); //repo katmanındaki herhangi bir classı versem de olur.
            var serviceAssembly = Assembly.GetAssembly(typeof(MapProfile));

            builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly).Where(x => x.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly).Where(x => x.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<ProductServiceWithNoCaching>().As<IProductService>().InstancePerLifetimeScope();

            //InstancePerLifetimeScope => Asp.netteki scope methoduna karşılık geliyor. Yani bir request başlayıp bitene kadar aynı instance kullansın
            //InstancePerDependency = > Transient'e karşılık geliyor. Her seferinde o interface nerde geçildiyse her seferinde yeni bir instance oluşturur.
        }
    }
}