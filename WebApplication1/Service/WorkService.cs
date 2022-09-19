namespace WebApplication1.Service;

public class WorkService:IWorkService
{
    public string BeginWork()
    {
        Console.WriteLine("执行BeginWork():");
        return "开始工作。。。";
    }
}