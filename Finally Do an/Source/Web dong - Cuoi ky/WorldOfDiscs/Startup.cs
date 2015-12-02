using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WorldOfDiscs.Startup))]
namespace WorldOfDiscs
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            
        }
    }
}
