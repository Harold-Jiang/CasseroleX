namespace CasseroleX.Application.Configurations;

public interface ISiteConfigurationService
{
    Task<T> GetConfigurationAsync<T>() where T : IConfig,new();
}
