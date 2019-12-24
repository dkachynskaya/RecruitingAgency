using BLL.Infrastructure;

namespace WebApi.Mapper
{
    public static class MapperInitializer
    {
        public static void Initialize()
        {
            AutoMapper.Mapper.Initialize(config =>
            {
                config.AddProfile<MapViewModels>();
                config.AddProfile<MapDTOModels>();
            });
        }
    }
}