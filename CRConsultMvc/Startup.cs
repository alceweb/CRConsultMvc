using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CRConsultMvc.Startup))]
namespace CRConsultMvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
