using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Lunz.MP.Warehouse.ApiGateway.Clients;
using Newtonsoft.Json;
using ZeebeConsole.WarehouseClient;

namespace ZeebeConsole
{
    public class Program
    {
        private static readonly string _zeebeUrl = "127.0.0.1:26500";
        /// <summary>
        /// 模拟入库场景
        /// 1.入库：新建入库单-->增加库存-->判断库存数目：>=300则预警；否则结束
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            InitClient.Initialize();
            Console.Write("\n输入入库数量\n");
            string stockNum = Console.ReadLine();//从控制台读入输入
            InStockScene(Convert.ToInt32(stockNum)).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 入库场景
        /// </summary>
        public static async Task InStockScene(int stocknum)
        {
            var processPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "instock-process.bpmn");
            var provider = new InStockProvider(_zeebeUrl, processPath);

            var mockInStockData = GetMockInStockData(stocknum);
            var mockStockData = GetMockStockData(stocknum);
            await provider.Send(new
            {
                InStockData = JsonConvert.SerializeObject(mockInStockData),
                StockData = JsonConvert.SerializeObject(mockStockData),
                stocknum = stocknum
            });

            await provider.JobWorkerCreator();
        }

        private static StockCreateOrUpdateCommand GetMockStockData(int stocknum)
        {
            StockCreateOrUpdateCommand command = new StockCreateOrUpdateCommand()
            {
                StockDetails = new List<StockDetailArgs>()
                {
                    new StockDetailArgs()
                    {
                        BatchNo = "liuzhiwei",
                        BatchNoOriginal = "liuzhiwei",
                        CargoesId = "liuzhiwei",
                        CargoesName = "liuzhiwei",
                        CargoesCode = "liuzhiwei",
                        ShipperId = "liuzhiwei",
                        ShipperName ="liuzhiwei",
                        StockNum = stocknum,
                        StorespaceId = "liuzhiwei",
                        StorespaceName = "liuzhiwei",
                        StorespaceCode = "liuzhiwei",
                        IsCodeSingle = false,//如果是true，则需要对GuNumber赋值
                        ExpirationTime = DateTime.MaxValue,
                        ProduceTime = DateTime.Now.AddYears(-4),
                        FromWarehouseName = "liuzhiwei",
                        FromWarehouseCode = "liuzhiwei",
                        FromWarehouseId = "liuzhiwei",
                        UnitName = "liuzhiwei",
                        UnitId = "liuzhiwei",
                        UpdatedAt = DateTime.Now,
                        CreatedAt = DateTime.Now,
                        CreatedById = "liuzhiwei",
                        UpdatedById = "liuzhiwei"
                    }
                }
            };
            return command;
        }

        private static InStockCreateCommand GetMockInStockData(int stocknum)
        {
            InStockCreateCommand command = new InStockCreateCommand();

            command.AuditStatus = AuditStatus.AuditPass;
            command.Code = "liuzhiwei";
            command.CreatedAt = DateTime.Now;
            command.CreatedById = "liuzhiwei";
            command.CreatedByName = "liuzhiwei";
            command.InStockType = BillTypes.InstockInit;
            command.PublishedAt = DateTime.Now.AddDays(1);
            command.PublishedById = "liuzhiwei";
            command.PublishedByName = "liuzhiwei";
            command.Remarks = "liuzhiwei";
            command.ShipperId = "liuzhiwei";
            command.ShipperName = "liuzhiwei";
            command.Status = BillStatus.Finished;
            command.SupplierId = "liuzhiwei";
            command.SupplierContact = "liuzhiwei";
            command.SupplierName = "liuzhiwei";
            command.SupplierTel = "liuzhiwei";
            command.TotalInTax = 100;
            command.TotalnoTax = 10;
            command.TotalTax = 111;

            command.InStockDetails = new List<InStockDetailCreateArgs>()
            {
                new InStockDetailCreateArgs()
                {
                        RelateId="liuzhiwei",
                        RelateCode="liuzhiwei",
                        RelateItemId="liuzhiwei",
                        CargoesId="liuzhiwei",
                        CargoesName="liuzhiwei",
                        CargoesCode="liuzhiwei",
                        BatchNoOriginal="liuzhiwei",
                        BatchNo="liuzhiwei",
                        GuNumber="liuzhiwei",
                        ProduceTime= DateTime.Now,
                        ExpirationTime= DateTime.Now.AddDays(5),
                        StoreSpaceId="liuzhiwei",
                        SpecId="liuzhiwei",
                        SpecName="liuzhiwei",
                        InStockNum=stocknum,
                        UnitId="liuzhiwei",
                        UnitName="liuzhiwei",
                        PriceNoTax=100,
                        PriceInTax=200,
                        TaxRate=300,
                        TaxAmount=400,
                        IsCodeSingle=false,
                        RelateType= BillTypes.InstockBackup
                }
            };

            return command;
        }
    }
}
