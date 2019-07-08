using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using SmallBPMConsole.Models;

namespace SmallBPMConsole.Consumers
{
    /// <summary>
    /// 入库后判断分支
    /// </summary>
    public class GatewayConsumer : IConsumer<Models.ProcessContext>
    {
        public async Task Consume(ConsumeContext<Models.ProcessContext> context)
        {
            // 消息里面的入库数目
            var inStockNum = context.Message.Variables.FirstOrDefault(t => t.Name == "InStockNum")?.Value;
            // 为了区分两条分支，如果在流程中则还是两个分支实现
            if (Convert.ToInt32(inStockNum) > 300)
            {
                context.Message.Variables.Add(new ProcessVariableData() {Name = "Leader",Value = "lisi"});
            }
            else
            {
                context.Message.Variables.Add(new ProcessVariableData() { Name = "Leader", Value = "wangwu" });
            }
            
            await Console.Out.WriteLineAsync(
                $"Gateway Consume Message: {context.Message.ProcessId},DateTime:{DateTime.Now}," +
                $"InStockNum:{inStockNum}");
            // 发送下一步的消息：看看走哪个分支
            Models.ProcessContext request = new Models.ProcessContext()
            {
                ProcessId = context.Message.ProcessId,
                ProcessConfig = context.Message.ProcessConfig,
                CurrentSequenceFlow = context.Message.CurrentSequenceFlow,
                Variables = context.Message.Variables
            };
            var client = new ProcessEngine.ProcessEngine(request);
            client.DoNext();
        }
    }
}
