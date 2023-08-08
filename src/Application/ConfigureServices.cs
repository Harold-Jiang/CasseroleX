using System.Reflection;
using CasseroleX.Application.Common.Behaviours;
using CasseroleX.Application.Common.Models;
using CasseroleX.Application.Public.Commands;
using CasseroleX.Domain.Entities;
using CasseroleX.Domain.Entities.Role;
using FluentValidation;
using MediatR;

namespace Microsoft.Extensions.DependencyInjection;
public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        services.AddAutoMapper(assembly);
        services.AddValidatorsFromAssembly(assembly);

        services.AddMediatR(options => options.RegisterServicesFromAssembly(assembly));


        services.AddTransient(typeof(IRequestHandler<MultiCommand<UserRule>, Result>), typeof(MultiCommandHandler<UserRule>));
        services.AddTransient(typeof(IRequestHandler<MultiCommand<RolePermissions>, Result>), typeof(MultiCommandHandler<RolePermissions>));

        services.AddTransient(typeof(IRequestHandler<SortCommand<UserRule>, Result>), typeof(SortCommandHandler<UserRule>));
        services.AddTransient(typeof(IRequestHandler<SortCommand<RolePermissions>, Result>), typeof(SortCommandHandler<RolePermissions>));

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>)); 
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));

        return services;
    }
}
