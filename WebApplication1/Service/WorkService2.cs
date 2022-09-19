namespace WebApplication1.Service;

public class WorkService2:IWorkService
{
    public string BeginWork()
    {
        Console.WriteLine("执行WorkService2.BeginWork():");
        return "WorkService2开始工作。。。";
    }
}