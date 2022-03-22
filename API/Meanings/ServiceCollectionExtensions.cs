using Microsoft.Extensions.DependencyInjection;

namespace VocabularyAPI.Meanings
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMeanings(this IServiceCollection services)
        {
            services.AddSingleton<MeaningsService>();

            return services;
        }
    }
}
