using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyEPA.Startup))]
namespace MyEPA
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
