using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using SmallBPMConsole.Models;

namespace SmallBPMConsole.Consumers
{
    public class EndConsumer : IConsumer<Models.ProcessContext>
    {
        public async Task Consume(ConsumeContext<Models.ProcessContext> context)
        {
            await Console.Out.WriteLineAsync(
                $"End Consume Message: {context.Message.ProcessId}，流程结束了！！！");
        }
    }
}
