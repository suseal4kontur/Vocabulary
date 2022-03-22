using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace VocabularyAPI.Mapping
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMapping(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(e => e.AddProfile(new MappingProfile()));
            services.AddSingleton(mappingConfig.CreateMapper());

            return services;
        }
    }
}
