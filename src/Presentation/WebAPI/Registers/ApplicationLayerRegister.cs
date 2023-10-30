using Application.Services;

namespace WebAPI.Registers
{
    public class ApplicationLayerRegister : IWebApplicationBuilderRegister
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IdentityService>();
        }
    }
}