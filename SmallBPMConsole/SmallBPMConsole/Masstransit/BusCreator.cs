using MassTransit;
using System;
using System.Collections.Generic;
using System.Text;
using MassTransit.RabbitMqTransport;
using SmallBPMConsole.Models;

namespace SmallBPMConsole.Masstransit
{
    public static class BusCreator
    {
        public static async void SendCommand(IBusControl bus, Uri sendToUri, Models.ProcessContext request)
        {
            var endPoint = await bus.GetSendEndpoint(sendToUri);
            await endPoint.Send(request);
            //Console.WriteLine($"send command:id={request.ProcessId},DateTime:{DateTime.Now}");
        }

        public static IBusControl CreateBus(ProcessConfig process,
            Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost> registrationAction = null)
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri(process.RabbitMqUri.Url), hst =>
                {
                    hst.Username(process.RabbitMqUri.User);
                    hst.Password(process.RabbitMqUri.Password);
                });

                registrationAction?.Invoke(cfg, host);
            });
        }
    }
}
