using Autofac;
using Autofac.Extensions.DependencyInjection;
using WebApplication1.Extentions;
using WebApplication1.Service;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
       
        //方式一：模组化
        containerBuilder.RegisterModule<ApiModule>();
        //方式二：指定具体依赖关系
        // container.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
        // var container = containerBuilder.Build();
        //
        // using (var scope=container.BeginLifetimeScope("my-request"))
        // {
        //     var workService = scope.Resolve<IWorkService>();
        //     workService.BeginWork();
        // }
      
       
    });
// Add services to the container.
 
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();