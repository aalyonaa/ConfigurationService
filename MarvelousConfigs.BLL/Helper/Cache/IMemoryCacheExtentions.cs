namespace MarvelousConfigs.BLL.Cache
{
    public interface IMemoryCacheExtentions
    {
        Task RefreshConfigByServiceId(int id);
        Task SetMemoryCache();
    }
}