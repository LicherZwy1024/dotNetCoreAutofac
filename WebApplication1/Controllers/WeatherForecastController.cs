using Autofac;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Service;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IUserService _userService;
    private readonly IComponentContext _componentContext;
    private  IWorkService? _workService;
    private readonly ILifetimeScope _scope;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IUserService userService,
        IComponentContext componentContext,
        ILifetimeScope scope)
    {
        _logger = logger;
        _userService = userService;
        _componentContext = componentContext;

        _scope = scope;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        _logger.LogInformation("test");
        _userService.GetUserName();
        IWorkService? _workServiceS0;
        using (var s0=_scope.BeginLifetimeScope())
        {
            using (var s=s0.BeginLifetimeScope("my-request"))
            {
                _workServiceS0=  s.ResolveNamed<IWorkService>("work1");
                _workServiceS0?.BeginWork();
            }
        }
        IWorkService? _workServiceSS;
        IWorkService? _workServiceSS2;
        using (var s=_scope.BeginLifetimeScope("my-request2"))
        {
            _workService=  s.ResolveNamed<IWorkService>("work2");
            _workService?.BeginWork();
            //_componentContext的生命周期中没有work2
            // var workService = _componentContext.ResolveNamed<IWorkService>("work2");
            // workService?.BeginWork();
            
            //相同作用域下的解析实例是一个
            using (var ss=s.BeginLifetimeScope())
            {
                _workServiceSS2=  ss.ResolveNamed<IWorkService>("work2");
                _workServiceSS2?.BeginWork();
            }
            //不同作用域下的解析实例不同
            using (var ss=s.BeginLifetimeScope("my-request"))
            {
                _workServiceSS=  ss.ResolveNamed<IWorkService>("work1");
                _workServiceSS?.BeginWork();
            }
        }
        _workService?.BeginWork();
        var resolveNamed = _componentContext.Resolve<UserService>();
        resolveNamed.GetUserName();
        
        //InstancePerLifetimeScope解析实例是一个
        Console.WriteLine($"userservice={_userService==resolveNamed}");
        //不同作用域下的解析实例不同
        Console.WriteLine($"workservice={_workServiceS0==_workServiceSS}");
        //相同作用域下的解析实例是一个
        Console.WriteLine($"workservice2={_workService==_workServiceSS2}");
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
}