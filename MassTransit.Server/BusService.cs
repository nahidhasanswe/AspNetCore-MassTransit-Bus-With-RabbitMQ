using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace MassTransit.Server
{
    public class BusService :
    IHostedService
    {
        private readonly IBusControl _busControl;

        public BusService(IServiceProvider provider)
        {
            using(var scope = provider.CreateScope())
            {
                _busControl = scope.ServiceProvider.GetRequiredService<IBusControl>();
            }
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            if (_busControl != null)
            {
                Console.WriteLine("Working....");
                await _busControl.StartAsync(cancellationToken);
            }
            else
            {
                Console.WriteLine("Bus Control is null");
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
           await _busControl.StopAsync(cancellationToken);
        }
    }
}
