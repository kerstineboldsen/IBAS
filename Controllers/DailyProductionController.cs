using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DailyProduction.Model;
using Azure.Data.Tables;
using Azure;

namespace IbasAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DailyProductionController : ControllerBase
    {
        private static TableClient tableClient; private List<DailyProductionDTO> _productionRepo; private readonly ILogger<DailyProductionController> _logger;

        public DailyProductionController(ILogger<DailyProductionController> logger)
        {
            _logger = logger; 
            var serviceUri = "https://ibastestgroupmodul2.table.core.windows.net/IBASProduktion2022"; 
            var tableName = "IBASProduktion2022"; 
            var accountName = "ibastestgroupmodul2";
            var storageAccountKey = "cGQuHp6HLYQR2SyuS93AgHULcETK6F5XlgcY2ZjZyoFFsptyvpwuq/ryYK6HWVPr02duW+QyY7o6+AStPQcklA==";

            tableClient = new TableClient(new Uri(serviceUri), tableName, new TableSharedKeyCredential(accountName, storageAccountKey));
        }


        //skal returnere en liste af DailyPorduction DTO´er 
        //starter med at lave en almindelig liste og kører igennem alle dem der er i query. 
        //opretter er nyt dto hvor hver entity.
        //enity er en dynamisk klasse. 
        //Model enum.ToObject er nogle tal og er en måde at ændre tal til en streng.
        //Items Produced = den skal over i item produced 
        //Dette gøres for alle elementer i listen og den laver grafen
        //Det er ligesom Dapper som mapper SQL statement om til kode

        [HttpGet]
        public IEnumerable<DailyProductionDTO> Get()
        {
            var _productionrepo = new List<DailyProductionDTO>();
            Pageable<TableEntity> entities = tableClient.Query<TableEntity>();
            foreach (TableEntity entity in entities)
            {
                var dto = new DailyProductionDTO
                {
                    Date = DateTime.Parse(entity.RowKey),
                    Model = (BikeModel)Enum.ToObject(typeof(BikeModel), Int32.Parse(entity.PartitionKey)),
                    ItemsProduced = (int)entity.GetInt32("itemsProduced")
                };
                _productionrepo.Add(dto);
            }
            return _productionrepo;
        }
    }
}


/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DailyProduction.Model;
using Azure;
//tilføjer Azure nudget til projektet 
using Azure.Data.Tables;

namespace IbasAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DailyProductionController : ControllerBase
    {

        private List<DailyProductionDTO> _productionRepo;
        private readonly ILogger<DailyProductionController> _logger;
        private TableClient client = new TableClient("DefaultEndpointsProtocol=https;AccountName=ibastestgroupmodul2;AccountKey=cGQuHp6HLYQR2SyuS93AgHULcETK6F5XlgcY2ZjZyoFFsptyvpwuq/ryYK6HWVPr02duW+QyY7o6+AStPQcklA==;EndpointSuffix=core.windows.net", "officeSupplies");
        TableServiceClient serviceClient = new TableServiceClient("DefaultEndpointsProtocol=https;AccountName=ibastestgroupmodul2;AccountKey=cGQuHp6HLYQR2SyuS93AgHULcETK6F5XlgcY2ZjZyoFFsptyvpwuq/ryYK6HWVPr02duW+QyY7o6+AStPQcklA==;EndpointSuffix=core.windows.net");


        public DailyProductionController(ILogger<DailyProductionController> logger)
        {
            this.tableClient = new TableClient(
            new Uri(serviceUri),
            tableName,
            new TableSharedKeyCredential(accountName, storageAccountKey));

            
            string MyPK = "PartitionKey";
            string MyRK = "RowKey";
            string filter = TableOdataFilter.Create($"PartitionKey eq {MyPK} or RowKey eq {MyRK}");

            //udføre en forespørgsel op imod tabellen - svare til at lave en SQL statement.
            //TableEntity er en indbygget entity som kan bygge tabellen op med dynamisk liste
            Pageable<TableEntity> entities = this.tableClient.Query<TableEntity>();

            foreach (TableEntity entity in entities)
            {
                Console.WriteLine($"{entity.GetString("Product")}: {entity.GetInteger("Count")}");
            }
        }

        //skal returnere en liste af DailyPorduction DTO´er 
        //starter med at lave en almindelig liste og kører igennem alle dem der er i query. 
        //opretter er nyt dto hvor hver entity.
        //enity er en dynamisk klasse. 
        //Model enum.ToObject er nogle tal og er en måde at ændre tal til en streng.
        //Items Produced = den skal over i item produced 
        //Dette gøres for alle elementer i listen og den laver grafen
        //Det er ligesom Dapper som mapper SQL statement om til kode
        [HttpGet]
        public IEnumerable<DailyProductionDTO> Get()
        {
            var _productionRepo = new List<DailyProductionDTO>();
            Pageable<TableEntity> entities = this.tableClient.Query<TableEntity>();

            foreach (TableEntityentityinentities)
            {
                var dto = new DailyProductionDTO {
                    Date = DateTime.Parse(entity.RowKey),
                    Model = (BikeModel)Enum.ToObject(typeof(BikeModel), Int32.Parse(entity.PartitionKey)),
                    ItemsProduced = (int)entity.GetInt32("itemsProduced")
                 };
                _productionRepo.Add(dto);
            }
            return _productionRepo;
        }

    }
}
*/