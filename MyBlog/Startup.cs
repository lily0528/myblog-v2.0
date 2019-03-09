using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyBlog.Startup))]
namespace MyBlog
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
