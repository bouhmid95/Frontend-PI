using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Frontend_PI.Startup))]
namespace Frontend_PI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
