using Microsoft.Extensions.DependencyInjection;
using NameBasedAuthorizeViewComponent.Interfaces;
using TanvirArjel.Blazor.DependencyInjection;

namespace NameBasedAuthorizeViewComponent;

public static class AuthorizeComponentServiceExtensions
{
    public static IServiceCollection AddNameBasedAuthorizeComponent<T>(this IServiceCollection services) where T : class, INameBasedAuthorizationHelper
    {
        services.AddComponents();
        services.AddScoped<INameBasedAuthorizationHelper, T>();
        return services;
    }
}