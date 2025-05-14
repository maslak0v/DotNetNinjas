

namespace FinancialTracker.Services.AuthorizeApi.Presentation
{
    public static class DIPresentation
    {
        public static IServiceCollection AddPresentationLayer(
                this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            //services.AddScoped<ICurrentUserService, CurrentUserService>();
            return services;
        }
    }

}
