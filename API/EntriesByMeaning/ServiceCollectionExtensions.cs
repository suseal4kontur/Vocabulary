using Microsoft.Extensions.DependencyInjection;

namespace VocabularyAPI.EntriesByMeaning
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEntryByMeaning(this IServiceCollection services)
        {
            services.AddSingleton<IEntriesByMeaningService, EntriesByMeaningService>();

            return services;
        }
    }
}
