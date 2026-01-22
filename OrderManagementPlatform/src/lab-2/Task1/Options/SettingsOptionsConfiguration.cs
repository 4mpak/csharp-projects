using Microsoft.Extensions.DependencyInjection;

namespace Task1.Options;

public static class SettingsOptionsConfiguration
{
    public static IServiceCollection ConfigureSettingsOptions(this IServiceCollection collection)
    {
        collection.AddOptions<SettingsOptions>().BindConfiguration("SettingsOptions");
        return collection;
    }
}