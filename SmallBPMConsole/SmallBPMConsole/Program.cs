using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using MassTransit;
using SmallBPMConsole.Common;
using SmallBPMConsole.Consumers;
using SmallBPMConsole.Masstransit;
using SmallBPMConsole.Models;
using SmallBPMConsole.ProcessEngine;

namespace SmallBPMConsole
{
    class Program
    {
        /// <summary>
        /// VPS张三创建一个待入库流程：开始-创建待入库单-判断数目
        /// 若数目大于300，则报警通知李四（隔级上级）
        /// 若小于等于300，则通知王五（直属上级）
        /// 结束
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // 根据配置文件初始化队列监听
            var process = InitQueueListeners();
            // 开始执行流程
            int num = 400;
            Models.ProcessContext request = new Models.ProcessContext()
            {
                ProcessId = Guid.NewGuid().ToString(),
                Variables = new List<ProcessVariableData>()
                {
                    new ProcessVariableData(){Name = "Name",Value = "liuzhiwei"},
                    new ProcessVariableData(){Name = "Age",Value="20"},
                    new ProcessVariableData(){Name = "InStockNum",Value = num.ToString()}
                },
                ProcessConfig = process
            };
            var client = new ProcessEngine.ProcessEngine(request);
            client.Start();
            Console.WriteLine($"一个简单的业务流程引擎开始！入库数目：{num}");
        }

        private static ProcessConfig InitQueueListeners()
        {
            string applicationCode = "VPS";
            // 读取xml配置文件
            XDocument xmlDocument =
                XDocument.Load(AppDomain.CurrentDomain.BaseDirectory + $"ProcessConfiguration{applicationCode}.xml");
            ProcessConfig process = BuildConfig(xmlDocument);

            // 初始化监听
            CreateBusListener(process);

            return process;
        }

        /// <summary>
        /// 全部监听
        /// </summary>
        /// <param name="process"></param>
        private static void CreateBusListener(ProcessConfig process)
        {
            Dictionary<string, IConsumer<Models.ProcessContext>> dictionary =
                new Dictionary<string, IConsumer<Models.ProcessContext>>()
                {
                    {"flow_waitInStock", new WaitInStockConsumer()},
                    {"flow_inStock", new InStockConsumer()},
                    {"flow_gateway", new GatewayConsumer()},
                    {"flow_warning", new WarningServiceConsumer()},
                    {"flow_notify1", new NotifyServiceConsumer()},
                    {"flow_notify2", new NotifyServiceConsumer()},
                    {"flow_end", new EndConsumer()},
                };
            foreach (var item in dictionary)
            {
                var bus = BusCreator.CreateBus(process, (cfg, host) =>
                {
                    cfg.ReceiveEndpoint(host, item.Key, e =>
                    {
                        e.Instance(item.Value);
                        //e.QueueExpiration = TimeSpan.MaxValue;
                        //e.Durable = true;
                    });
                });
                bus.Start();
            }
        }
        
        private static ProcessConfig BuildConfig(XDocument xmlDocument)
        {
            string xmlString = xmlDocument.ToString();
            var result = XmlConverter.Deserialize<ProcessConfig>(xmlString);
            return result;
        }
    }
}
