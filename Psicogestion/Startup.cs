using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Psicogestion.Startup))]
namespace Psicogestion
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
