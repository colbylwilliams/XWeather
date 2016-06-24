using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(XWeatherService.Startup))]

namespace XWeatherService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}