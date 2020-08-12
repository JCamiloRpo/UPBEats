using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(UPBEats.Startup))]
namespace UPBEats
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
