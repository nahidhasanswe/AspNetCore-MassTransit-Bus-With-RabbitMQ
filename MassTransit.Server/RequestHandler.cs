using Common.Messages;
using System;
using System.Threading.Tasks;

namespace MassTransit.Server
{
    public class RequestHandler : IConsumer<requestMessage>
    {
        public async Task Consume(ConsumeContext<requestMessage> context)
        {

            Console.WriteLine(context.Message.Message);

            var response = new ReplyMessage
            {
                Message = "Server Receive the messages : " + context.Message.Message
            };

            await context.RespondAsync<ReplyMessage>(response);
        }
    }
}
