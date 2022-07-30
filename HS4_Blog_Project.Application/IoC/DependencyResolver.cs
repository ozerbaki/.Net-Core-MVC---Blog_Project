using Autofac;
using AutoMapper;
using HS4_Blog_Project.Application.AutoMapper;
using HS4_Blog_Project.Application.Services.AppUserService;
using HS4_Blog_Project.Application.Services.Postservice;
using HS4_Blog_Project.Domain.Repositories;
using HS4_Blog_Project.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS4_Blog_Project.Application.IoC
{
    //Ioc : Inversion of Control
    //Nuget : Autofac.Extensions.DependencyInjection yüklüyoruz
    //using Autofac; ekliyoruz, system.Reflection değil !!!

    //Asp.Net Core içinde bıilt-in IOC container var. 3 tane Life Manager sunarlar. AddSingleton, AddScoped, AddTransient
    //3. parti IOC container kullanacağız. İç içe life managment yapmak için kullanıyoruz.
    public class DependencyResolver:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AppUserRepository>().As<IAppUserRepository>().InstancePerLifetimeScope();
            builder.RegisterType<GenreRepository>().As<IGenreRepository>().InstancePerLifetimeScope();
            builder.RegisterType<PostRepository>().As<IPostRepository>().InstancePerLifetimeScope();
            builder.RegisterType<AuthorRepository>().As<IAuthorRepository>().InstancePerLifetimeScope();
            builder.RegisterType<AppUserRepository>().As<IAppUserRepository>().InstancePerLifetimeScope();

            #region Service Registration
            builder.RegisterType<PostService>().As<IPostService>().InstancePerLifetimeScope();
            builder.RegisterType<AppUserService>().As<IAppUserService>().InstancePerLifetimeScope();
            #endregion

            builder.RegisterType<Mapper>().As<IMapper>().InstancePerLifetimeScope();


            //Bağımlılığa ne sebep oluyorsa burada çözüyoruz.
            //AutoMapper, Fluent Validation

            // bu kısmı internetten bulup yapıştırdık.
            #region AutoMapper
            builder.Register(context => new MapperConfiguration(cfg =>
            {
                //Register Mapper Profile
                cfg.AddProfile<Mapping>(); //AutoMapper klasörünün altına eklediğimiz Mapping class'ını bağlıyoruz
            }
            )).AsSelf().SingleInstance();

            builder.Register(c =>
            {
                //This resolves a new context that can be used later.
                var context = c.Resolve<IComponentContext>();
                var config = context.Resolve<MapperConfiguration>();
                return config.CreateMapper(context.Resolve);
            })
            .As<IMapper>()
            .InstancePerLifetimeScope();
            #endregion

            base.Load(builder);
        }
    }
}
