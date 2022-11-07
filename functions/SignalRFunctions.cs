using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace SignalRFunctions
{
    public static class SignalRFunctions
    {
        public static string storeid;
        public static string timeInterval;
        public static string ProductName;
        public static int ProductSellingRank;
        public static int SoldProductQuantity;
        public static int RemainProductQuantity;
        public static float ProductPrice;
        public static float ProductCost;
        public static float ProductDiscount;
        public static float ProductProfitPerItem;
        public static float ProductProfitPercentagePerItem;
        public static string ProductImageURL;
        public static int CustomerQuantityLastHour;
        public static int CustomerQuantityLastDay;
        public static int CustomerQuantityTotal;
        public static string ShelfProductNames;
        public static int ShelfItemQuantity;
        public static float ShelfRevenueLastHour;
        public static float ShelfRevenueLastDay;
        public static float ShelfRevenueTotal;
        public static float ShelfSoldItemQuantityLastHour;
        public static float ShelfSoldItemQuantityLastDay;
        public static float ShelfSoldItemQuantityTotal;
        public static float ShelfCostPerItem;
        public static float ShelfPricePerItem;
        public static float ConversionRateLastHour;
        public static float ConversionRateLastDay;
        public static float ConversionRateTotal;
        public static float ShelfProfitLastHour;
        public static float ShelfProfitLastDay;
        public static float ShelfProfitTotal;

        [FunctionName("negotiate")]
        public static SignalRConnectionInfo GetSignalRInfo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
            [SignalRConnectionInfo(HubName = "dttelemetry")] SignalRConnectionInfo connectionInfo)
        {
            return connectionInfo;
        }

        [FunctionName("broadcast")]
        public static Task SendMessage(
            [EventGridTrigger] EventGridEvent eventGridEvent,
            [SignalR(HubName = "dttelemetry")] IAsyncCollector<SignalRMessage> signalRMessages,
            ILogger log)
        {
            JObject eventGridData = (JObject)JsonConvert.DeserializeObject(eventGridEvent.Data.ToString());
            if (eventGridEvent.EventType.Contains("telemetry"))
            {
                var data = eventGridData.SelectToken("data");

                var telemetryMessage = new Dictionary<object, object>();
                foreach (JProperty property in data.Children())
                {
                    log.LogInformation(property.Name + " - " + property.Value);
                    telemetryMessage.Add(property.Name, property.Value);
                }
                return signalRMessages.AddAsync(
                new SignalRMessage
                {
                    Target = "TelemetryMessage",
                    Arguments = new[] { telemetryMessage }
                });
            }
            else
            {
                try
                {
                    storeid = eventGridEvent.Subject;
                    
                    var data = eventGridData.SelectToken("data");
                    var patch = data.SelectToken("patch");

                    var property = new Dictionary<object, object>
                    {
                        {"storeid", storeid }
                    };
                    return signalRMessages.AddAsync(
                        new SignalRMessage
                        {
                            Target = "PropertyMessage",
                            Arguments = new[] { property }
                        });
                }
                catch (Exception e)
                {
                    log.LogInformation(e.Message);
                    return null;
                }
            }

        }
    }
}