using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using System.Web.Mvc;
using TestTask;
using TestTask.Services;
using TestTask.Services.Interfaces;

public class AutofacConfig
{
    public static void ConfigureContainer()
    {
        var builder = new ContainerBuilder();
        builder.RegisterControllers(typeof(MvcApplication).Assembly);
        
        //services
        builder.RegisterType<MatrixService>().As<IMatrixService>();
        

        var container = builder.Build();
        DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
    }
}