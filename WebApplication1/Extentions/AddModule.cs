using System.Reflection;
using System.Runtime.Loader;
using Autofac;
using Microsoft.Extensions.DependencyModel;
using WebApplication1.Service;

namespace WebApplication1.Extentions;

public class ApiModule: Autofac.Module
{
     protected override void Load(ContainerBuilder builder)
     {
          // 获取所有创建的项目Lib
          var libs = DependencyContext.Default
               .CompileLibraries
               .Where(x => !x.Serviceable && x.Type == "project").ToList();
 
          // 将lib转成Assembly
          List<Assembly> assemblies = new();
          foreach (var lib in libs)
          {
               assemblies.Add(AssemblyLoadContext.Default
                    .LoadFromAssemblyName(new AssemblyName(lib.Name)));
          }
 
          // 反射获取其中所有的被接口修饰的类型，并区分生命周期
          builder.RegisterAssemblyTypes(assemblies.ToArray())
               .Where(t => t.IsAssignableTo<IDependency>() && !t.IsAbstract)
               .AsSelf()
               .AsImplementedInterfaces()
               .InstancePerLifetimeScope()
               // .InstancePerDependency()
               .PropertiesAutowired();

          builder.RegisterType<WorkService>().Named<IWorkService>("work1")
               .InstancePerMatchingLifetimeScope("my-request");
          builder.RegisterType<WorkService2>().Named<IWorkService>("work2")
               .InstancePerMatchingLifetimeScope("my-request2");

     }
}