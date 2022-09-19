namespace WebApplication1.Service;

public class UserService:IUserService
{
    public string GetUserName()
    {
        Console.WriteLine("执行GetUserName():");
        return "返回一个用户名;";
    }
}