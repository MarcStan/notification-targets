using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationTargets.Notification;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace NotificationTargets
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false)
                .Build();

            var services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(configuration);
            services.Scan(scan =>
            {
                scan.FromAssemblyOf<INotificationService>()
                    .AddClasses(classes => classes.AssignableTo<INotificationService>())
                    .AsSelfWithInterfaces()
                    .WithSingletonLifetime();
            });
            services.AddSingleton<INotificationStrategy, AggregatedNotificationService>();
            services.AddSingleton<HttpClient>();

            var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (s, e) => cts.Cancel();

            // usage
            using (var serviceProvider = services.BuildServiceProvider())
            {
                var strategy = serviceProvider.GetRequiredService<INotificationStrategy>();

                await strategy.BroadcastNotificationAsync(new MessageModel
                {
                    Title = "Hello, world",
                    Message = "Broadcasting messages from .Net Core"
                }, cts.Token);
            }
        }
    }
}
