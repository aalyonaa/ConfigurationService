namespace MarvelousConfigs.BLL.AuthRequestClient
{
    public interface IAuthRequestClient
    {
        Task<bool> GetRestResponse(string token);
    }
}