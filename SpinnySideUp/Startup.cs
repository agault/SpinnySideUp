using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SpinnySideUp.Startup))]
namespace SpinnySideUp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
