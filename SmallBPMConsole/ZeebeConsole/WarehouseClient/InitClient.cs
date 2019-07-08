using System;
using System.Collections.Generic;
using System.Text;
using Lunz.MP.Warehouse.ApiGateway.Clients;

namespace ZeebeConsole.WarehouseClient
{
    public static class InitClient
    {
        public static InStockClient InStockClient;
        public static OutStockClient OutStockClient;
        public static CheckJobClient CheckJobClient;
        public static CheckResultClient CheckResultClient;
        public static CheckInfoClient CheckInfoClient;
        public static CheckDetailClient CheckDetailClient;
        public static DeliveryClient DeliveryClient;
        public static WaitInStockClient WaitInStockClient;
        public static WaitOutStockClient WaitOutStockClient;
        public static StockManagementClient StockManagementClient;
        public static AllocationClient AllocationClient;
        public static MoveStockClient MoveStockClient;
        public static DataDictionaryClient DataDictionaryClient;
        public static StorespaceClient StorespaceClient;
        public static Lunz.MP.Warehouse.ApiGateway.Clients.WarehouseClient WarehouseClient;
        public static LockManagementClient LockManagementClient;
        public static InoutStockLogClient InoutStockLogClient;
        public static RelateManageClient RelateManageClient;
        public static RelevanceShipperClient RelevanceShipperClient;
        public static void Initialize()
        {
            var baseUrl = "http://ware-internal-api.fat.lunz.cn";
            IdentitySettings identitySettings = new IdentitySettings(
                "http://identity-fat.lunz.cn",
                "warhouse-mp-internal-client",
                "NrAnpOFTNaKbAJ20FG9gcN7IlkXqt7iq",
                "warhouse-mp-internal-api");
            InStockClient = new InStockClient(baseUrl)
            {
                IdentitySettings = identitySettings,
            };
            OutStockClient = new OutStockClient(baseUrl)
            {
                IdentitySettings = identitySettings
            };
            CheckJobClient = new CheckJobClient(baseUrl)
            {
                IdentitySettings = identitySettings
            };
            CheckResultClient = new CheckResultClient(baseUrl)
            {
                IdentitySettings = identitySettings
            };
            CheckInfoClient = new CheckInfoClient(baseUrl)
            {
                IdentitySettings = identitySettings
            };
            CheckDetailClient = new CheckDetailClient(baseUrl)
            {
                IdentitySettings = identitySettings
            };
            DeliveryClient = new DeliveryClient(baseUrl)
            {
                IdentitySettings = identitySettings
            };
            WaitInStockClient = new WaitInStockClient(baseUrl)
            {
                IdentitySettings = identitySettings
            };
            WaitOutStockClient = new WaitOutStockClient(baseUrl)
            {
                IdentitySettings = identitySettings
            };
            StockManagementClient = new StockManagementClient(baseUrl)
            {
                IdentitySettings = identitySettings
            };
            AllocationClient = new AllocationClient(baseUrl)
            {
                IdentitySettings = identitySettings
            };
            MoveStockClient = new MoveStockClient(baseUrl)
            {
                IdentitySettings = identitySettings
            };
            DataDictionaryClient = new DataDictionaryClient(baseUrl)
            {
                IdentitySettings = identitySettings
            };
            StorespaceClient = new StorespaceClient(baseUrl)
            {
                IdentitySettings = identitySettings
            };
            WarehouseClient = new Lunz.MP.Warehouse.ApiGateway.Clients.WarehouseClient(baseUrl)
            {
                IdentitySettings = identitySettings
            };
            LockManagementClient = new LockManagementClient(baseUrl)
            {
                IdentitySettings = identitySettings
            };
            InoutStockLogClient = new InoutStockLogClient(baseUrl)
            {
                IdentitySettings = identitySettings
            };
            RelateManageClient = new RelateManageClient(baseUrl)
            {
                IdentitySettings = identitySettings
            };
            RelevanceShipperClient = new RelevanceShipperClient(baseUrl)
            {
                IdentitySettings = identitySettings
            };
        }
    }
}
