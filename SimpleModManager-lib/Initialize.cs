using System;
using Microsoft.Extensions.DependencyInjection;

namespace SimpleModManager_lib
{
    public static class Initialize
    {
        private static IServiceProvider CreateServiceProvider()
        {
            return new ServiceCollection()
                // Add different services needed here
                .BuildServiceProvider();
            
        }
        
        public static IServiceProvider ServiceProvider { get; } = CreateServiceProvider();
    }
}