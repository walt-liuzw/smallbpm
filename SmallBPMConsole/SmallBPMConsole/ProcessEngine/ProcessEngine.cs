using MassTransit;
using SmallBPMConsole.Masstransit;
using System;
using System.Collections.Generic;
using System.Linq;
using SmallBPMConsole.Models;

namespace SmallBPMConsole.ProcessEngine
{
    public class ProcessEngine: IProcessEngine
    {
        private IBusControl _bus;
        public ProcessContext Context { get; set; }

        public ProcessEngine(ProcessContext request)
        {
            Context = request;
        }

        public void Start()
        {
            _bus = BusCreator.CreateBus(Context.ProcessConfig);
            _bus.Start();

            // 根据process配置，找到第一个start
            var startId = Context.ProcessConfig.StartEvent.Id;
            var startFlow = Context.ProcessConfig.SequenceFlow.FirstOrDefault(t => t.SourceRef == startId);
            if (startFlow == null)
            {
                return;
            }
            Context.CurrentSequenceFlow = startFlow;
            var sendToUri = new Uri($"{Context.ProcessConfig.RabbitMqUri.Url}{startFlow?.Id}");
            BusCreator.SendCommand(_bus, sendToUri, Context);
        }

        public void DoNext()
        {
            _bus = BusCreator.CreateBus(Context.ProcessConfig);
            _bus.Start();

            // 找到当前的节点的下一个节点执行(TODO:目标的服务可能是列表)
            var nextFlows = Context.ProcessConfig.SequenceFlow.Where(t => t.SourceRef == Context.CurrentSequenceFlow?.TargetRef).ToList();
            if (!nextFlows.Any())
            {
                return;
            }

            var node = GetNextNode(nextFlows);
            Context.CurrentSequenceFlow = node;
            var sendToUri = new Uri($"{Context.ProcessConfig.RabbitMqUri.Url}{node.Id}");
            BusCreator.SendCommand(_bus, sendToUri, Context);

        }

        private SequenceFlow GetNextNode(List<SequenceFlow> nextFlows)
        {
            if (nextFlows.Count == 1)
            {
                return nextFlows.FirstOrDefault();
            }
            else
            {
                // 根据条件选择具体一个节点
                foreach (var node in nextFlows)
                {
                    if (node.ConditionExpression != null)
                    {
                        var gatewayValue = Context.Variables.FirstOrDefault(t => t.Name == node.ConditionExpression.Name)?.Value;
                        if (node.ConditionExpression.Type == "GE" && Convert.ToInt32(gatewayValue) >= Convert.ToInt32(node.ConditionExpression.Value))
                        {
                            return node;
                        }

                        if (node.ConditionExpression.Type == "L" && Convert.ToInt32(gatewayValue) < Convert.ToInt32(node.ConditionExpression.Value))
                        {
                            return node;
                        }
                    }
                    else
                    {
                        return node;
                    }
                }
                return nextFlows.FirstOrDefault();
            }
        }
    }
}
