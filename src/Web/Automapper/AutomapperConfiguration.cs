using AutoMapper;

namespace Web.Automapper
{
    public class AutomapperConfiguration
    {
        public static void RegisterMapping()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(typeof(Startup));
            });
            var configuration = new MapperConfiguration(cfg => cfg.AddMaps(typeof(Startup)));
        }
    }
}
