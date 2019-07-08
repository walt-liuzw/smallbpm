using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lunz.MP.Warehouse.ApiGateway.Clients;
using Newtonsoft.Json;
using Zeebe.Client;
using Zeebe.Client.Api.Clients;
using Zeebe.Client.Api.Responses;
using Zeebe.Client.Api.Subscription;
using ZeebeConsole.WarehouseClient;

namespace ZeebeConsole
{
    public class InStockProvider
    {
        private string _path;
        private string _zeebeUrl;
        private static IZeebeClient _client;
        public InStockProvider(string zeebeUrl, string path)
        {
            _zeebeUrl = zeebeUrl;
            _path = path;
        }

        public async Task Send(dynamic variables)
        {
            // create zeebe client
            _client = ZeebeClient.NewZeebeClient(_zeebeUrl);
            // deploy
            var deployResponse = await _client.NewDeployCommand().AddResourceFile(_path).Send();
            // create workflow instance
            var workflowKey = deployResponse.Workflows[0].WorkflowKey;

            await _client
                .NewCreateWorkflowInstanceCommand()
                .WorkflowKey(workflowKey)
                .Variables(JsonConvert.SerializeObject(variables))
                .Send();
        }

        public Task JobWorkerCreator()
        {
            using (var signal = new EventWaitHandle(false, EventResetMode.AutoReset))
            {
                _client.NewWorker()
                    .JobType("createinstock")
                    .Handler(HandleInStockJob)
                    .MaxJobsActive(5)
                    .Name("createinstock")
                    .AutoCompletion()
                    .PollInterval(TimeSpan.FromSeconds(1))
                    .Timeout(TimeSpan.FromSeconds(10))
                    .Open();
                _client.NewWorker()
                    .JobType("modifystock")
                    .Handler(HandleStockJob)
                    .MaxJobsActive(5)
                    .Name("modifystock")
                    .AutoCompletion()
                    .PollInterval(TimeSpan.FromSeconds(1))
                    .Timeout(TimeSpan.FromSeconds(10))
                    .Open();
                _client.NewWorker()
                    .JobType("notify")
                    .Handler(HandleNotifyJob)
                    .MaxJobsActive(5)
                    .Name("notify")
                    .AutoCompletion()
                    .PollInterval(TimeSpan.FromSeconds(1))
                    .Timeout(TimeSpan.FromSeconds(10))
                    .Open();

                // blocks main thread, so that worker can run
                signal.WaitOne();
            }

            return Task.CompletedTask;
        }

        private static void HandleInStockJob(IJobClient jobClient, IJob job)
        {
            Console.WriteLine("————流程进入：新增入库模块");
            var variables = JsonConvert.DeserializeObject<Dictionary<string, object>>(job.Variables);
            var command = JsonConvert.DeserializeObject<InStockCreateCommand>(variables["InStockData"].ToString());
            InitClient.InStockClient.CreateAsync(command).GetAwaiter().GetResult();
            
        }
        private static void HandleStockJob(IJobClient jobClient, IJob job)
        {
            Console.WriteLine("————流程进入：修改库存模块");
            var variables = JsonConvert.DeserializeObject<Dictionary<string, object>>(job.Variables);
            var command = JsonConvert.DeserializeObject<StockCreateOrUpdateCommand>(variables["StockData"].ToString());
            InitClient.StockManagementClient.CreateOrUpdateAsync(command).GetAwaiter().GetResult();
        }
        private static void HandleNotifyJob(IJobClient jobClient, IJob job)
        {
            var variables = JsonConvert.DeserializeObject<Dictionary<string, object>>(job.Variables);
            Console.WriteLine($"————流程进入：因为数量：{variables["stocknum"]}>=300,所以触发报警");
        }
    }
}
