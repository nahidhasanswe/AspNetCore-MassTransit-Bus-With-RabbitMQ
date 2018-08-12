using Autofac;
using Common.Messages;
using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MassTransit.Server
{
    class Program
    {
        private const int delay = 1000;
        private static ManualResetEvent resetEvent = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            StartMassTransit(serviceCollection).GetAwaiter().GetResult();
        }

        private static async Task StartMassTransit(IServiceCollection services)
        {
            services.AddScoped<RequestHandler>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<RequestHandler>();
            });


            services.AddSingleton<IBusControl>(context =>
            {
                var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    var host = cfg.Host(new Uri("rabbitmq://192.168.1.105/"), h =>
                    {
                        h.Username("admin");
                        h.Password("a");
                    });

                    cfg.ReceiveEndpoint(host, "demo", e =>
                    {
                        e.LoadFrom(context);
                    });
                });

                return bus;
            });

            services.AddSingleton<IBus>(provider => provider.GetRequiredService<IBusControl>());

            
            IBusControl busControl = services.BuildServiceProvider().GetRequiredService<IBusControl>();

            try
            {

                await busControl.StartAsync();
                Console.WriteLine("Start MassTransit Server");
                await Task.Factory.StartNew(async () => await Handle());
                Console.CancelKeyPress += (sender, eventArgs) => resetEvent.Set();
                resetEvent.WaitOne();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                await busControl.StopAsync();
                Console.WriteLine("STOP");
            }
        }

        static async Task Handle()
        {
            await Task.Delay(delay);
            await Handle();
        }
    }
}
