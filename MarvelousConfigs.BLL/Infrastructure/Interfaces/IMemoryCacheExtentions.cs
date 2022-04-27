namespace MarvelousConfigs.BLL.Infrastructure
{
    public interface IMemoryCacheExtentions
    {
        Task RefreshConfigByServiceId(int id);
        Task SetMemoryCache();
    }
}