using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Team1Blog.Startup))]
namespace Team1Blog
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
