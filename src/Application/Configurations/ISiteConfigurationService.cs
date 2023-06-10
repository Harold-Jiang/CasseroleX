namespace CasseroleX.Application.Configurations;

public interface ISiteConfigurationService
{
    Task<T> GetConfiguration<T>() where T : IConfig,new();
}
