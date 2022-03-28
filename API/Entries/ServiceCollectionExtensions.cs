using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Model;
using Model.Entries;
using MongoDB.Driver;

namespace VocabularyAPI.Entries
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEntries(this IServiceCollection services)
        {
            services.AddSingleton(provider =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                var connectionString = configuration["ConnectionString"];
                var client = new MongoClient(connectionString);
                var database = client.GetDatabase("vocabulary");
                var collection = database.GetCollection<Entry>("entries");
                return collection;
            });
            services.AddSingleton<IVocabulary, Vocabulary>();
            services.AddSingleton<IEntriesService, EntriesService>();

            return services;
        }
    }
}
