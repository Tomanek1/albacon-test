﻿using AlbaconTest.Services.Infrastructure.Models;
using AlbaconTest.Services.Services;
using AlbaconTest.Services.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AlbaconTest.Services.Infrastructure
{
    public static class DependencyInjection
    {
        public static void RegisterServices(this IServiceCollection services )
        {
            services.AddScoped<IDatastoreService, DatastoreService>();

            services.AddSingleton<InMemoryStorage>();

            services.AddOptions<Option>().BindConfiguration("Options");
        }

    }
}
