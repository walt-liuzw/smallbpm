using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using SmallBPMConsole.Models;

namespace SmallBPMConsole.Consumers
{
    /// <summary>
    /// 待入库消费端
    /// </summary>
    public class WaitInStockConsumer : IConsumer<Models.ProcessContext>
    {
        public async Task Consume(ConsumeContext<Models.ProcessContext> context)
        {
            await Console.Out.WriteLineAsync(
                $"WaitInStock Service Consume Message: {context.Message.ProcessId},DateTime:{DateTime.Now}");

            await Task.Run(() =>
             {
                // 发送下一步的消息：入库(消息里面带着引擎信息的数据id)
                Models.ProcessContext request = new Models.ProcessContext()
                 {
                     ProcessId = context.Message.ProcessId,
                     ProcessConfig = context.Message.ProcessConfig,
                     CurrentSequenceFlow = context.Message.CurrentSequenceFlow,
                     Variables = context.Message.Variables
                 };
                 var client = new ProcessEngine.ProcessEngine(request);
                 client.DoNext();
             });
        }
    }
}
