using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NowaAplikacja.Startup))]
namespace NowaAplikacja
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
