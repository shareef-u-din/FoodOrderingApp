namespace WebAPI.Registers
{
    public interface IWebApplicationBuilderRegister : IRegister
    {
        void RegisterServices(WebApplicationBuilder builder);
    }
}
