using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EntityFramework1Ikub.Startup))]
namespace EntityFramework1Ikub
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
