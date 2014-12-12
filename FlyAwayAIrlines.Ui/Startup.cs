using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FlyAwayAIrlines.Ui.Startup))]
namespace FlyAwayAIrlines.Ui
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
