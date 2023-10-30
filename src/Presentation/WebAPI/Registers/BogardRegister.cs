using Application;
using MediatR;

namespace WebAPI.Registers
{
    public class BogardRegister : IWebApplicationBuilderRegister
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {
            builder.Services.AddAutoMapper(typeof(Program), typeof(AssemblyReference));
            builder.Services.AddMediatR(typeof(Program), typeof(AssemblyReference));
        }
    }
}