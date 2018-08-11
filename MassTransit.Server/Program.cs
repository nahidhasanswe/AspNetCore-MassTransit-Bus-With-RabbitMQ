using Common.Messages;
using System;
using System.Threading.Tasks;

namespace MassTransit.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            StartMassTransit().GetAwaiter().GetResult();
        }

        private static async Task StartMassTransit()
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri("rabbitmq://localhost/"), h => 
                {
                    h.Username("admin");
                    h.Password("a");
                });

                cfg.ReceiveEndpoint(host, "demo", e =>
                {
                    e.Consumer<RequestHandler>();
                });
            });

            await bus.StartAsync();
            try
            {
                Console.WriteLine("Working....");

                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                await bus.StopAsync();
            }
        }
    }
}
