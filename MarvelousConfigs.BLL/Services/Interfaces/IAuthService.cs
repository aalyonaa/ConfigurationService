
namespace MarvelousConfigs.BLL.Services
{
    public interface IAuthService
    {
        string GetToken(string email, string pass);
    }
}