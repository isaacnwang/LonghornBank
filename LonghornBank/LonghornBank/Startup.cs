using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LonghornBank.Startup))]
namespace LonghornBank
{
    public partial class Startup
    {
        //public void Configuration(IAppBuilder app)
        //{
        //    ConfigureAuth(app);
        //}
    }
}
