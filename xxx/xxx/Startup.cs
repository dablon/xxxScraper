using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(xxx.Startup))]
namespace xxx
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
