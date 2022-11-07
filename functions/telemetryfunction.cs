using Azure;
using Azure.Core.Pipeline;
using Azure.DigitalTwins.Core;
using Azure.Identity;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Collections.Generic;

namespace My.Function
{
    // This class processes telemetry events from IoT Hub, reads temperature of a device
    // and sets the "Temperature" property of the device with the value of the telemetry.
    public class telemetryfunction
    {
        private static readonly HttpClient httpClient = new HttpClient();
        private static string adtServiceUrl = Environment.GetEnvironmentVariable("ADT_SERVICE_URL");

        [FunctionName("telemetryfunction")]
        public async void Run([EventGridTrigger] EventGridEvent eventGridEvent, ILogger log)
        {
            try
            {
                // After this is deployed, you need to turn the Managed Identity Status to "On",
                // Grab Object Id of the function and assigned "Azure Digital Twins Owner (Preview)" role
                // to this function identity in order for this function to be authorized on ADT APIs.
                //Authenticate with Digital Twins
                var credentials = new DefaultAzureCredential();
                log.LogInformation(credentials.ToString());
                DigitalTwinsClient client = new DigitalTwinsClient(
                    new Uri(adtServiceUrl), credentials, new DigitalTwinsClientOptions
                    { Transport = new HttpClientTransport(httpClient) });
                log.LogInformation($"ADT service client connection created.");
                
                if (eventGridEvent != null && eventGridEvent.Data != null)
                {
                    JObject deviceMessage = (JObject)JsonConvert.DeserializeObject(eventGridEvent.Data.ToString());
                    string deviceId = (string)deviceMessage["systemProperties"]["iothub-connection-device-id"];
                    var ID = deviceMessage["body"]["storeid"];
                    var TimeInterval = deviceMessage["body"]["TimeInterval"];
                    var ProductName = deviceMessage["body"]["ProductName"];
                    var ProductSellingRank = deviceMessage["body"]["ProductSellingRank"];
                    var SoldProductQuantity = deviceMessage["body"]["SoldProductQuantity"];
                    var RemainProductQuantity = deviceMessage["body"]["RemainProductQuantity"];
                    var ProductPrice = deviceMessage["body"]["ProductPrice"];
                    var ProductCost = deviceMessage["body"]["ProductCost"];
                    var ProductDiscount = deviceMessage["body"]["ProductDiscount"];
                    var ProductProfitPerItem = deviceMessage["body"]["ProductProfitPerItem"];
                    var ProductProfitPercentagePerItem = deviceMessage["body"]["ProductProfitPercentagePerItem"];
                    var ProductImageURL = deviceMessage["body"]["ProductImageURL"];
                    var CustomerQuantityLastHour = deviceMessage["body"]["CustomerQuantityLastHour"];
                    var CustomerQuantityLastDay = deviceMessage["body"]["CustomerQuantityLastDay"];
                    var CustomerQuantityTotal = deviceMessage["body"]["CustomerQuantityTotal"];
                    var ShelfProductNames = deviceMessage["body"]["ShelfProductNames"];
                    var ShelfItemQuantity = deviceMessage["body"]["ShelfItemQuantity"];
                    var ShelfRevenueLastHour = deviceMessage["body"]["ShelfRevenueLastHour"];
                    var ShelfRevenueLastDay = deviceMessage["body"]["ShelfRevenueLastDay"];
                    var ShelfRevenueTotal = deviceMessage["body"]["ShelfRevenueTotal"];
                    var ShelfSoldItemQuantityLastHour = deviceMessage["body"]["ShelfSoldItemQuantityLastHour"];
                    var ShelfSoldItemQuantityLastDay = deviceMessage["body"]["ShelfSoldItemQuantityLastDay"];
                    var ShelfSoldItemQuantityTotal = deviceMessage["body"]["ShelfSoldItemQuantityTotal"];
                    var ShelfCostPerItem = deviceMessage["body"]["ShelfCostPerItem"];
                    var ShelfPricePerItem = deviceMessage["body"]["ShelfPricePerItem"];
                    var ConversionRateLastHour = deviceMessage["body"]["ConversionRateLastHour"];
                    var ConversionRateLastDay = deviceMessage["body"]["ConversionRateLastDay"];
                    var ConversionRateTotal = deviceMessage["body"]["ConversionRateTotal"];
                    var ShelfProfitLastHour = deviceMessage["body"]["ShelfProfitLastHour"];
                    var ShelfProfitLastDay = deviceMessage["body"]["ShelfProfitLastDay"];
                    var ShelfProfitTotal = deviceMessage["body"]["ShelfProfitTotal"];

                    log.LogInformation($"Device: {deviceId} Device Id is: {ID}");
                    log.LogInformation($"Device: {deviceId} Time interval is: {TimeInterval}");
                    log.LogInformation($"Device: {deviceId} ProductName is: {ProductName}");
                    log.LogInformation($"Device: {deviceId} ProductSellingRank is: {ProductSellingRank}");
                    log.LogInformation($"Device: {deviceId} SoldProductQuantity: {SoldProductQuantity}");
                    log.LogInformation($"Device: {deviceId} RemainProductQuantity is: {RemainProductQuantity}");
                    log.LogInformation($"Device: {deviceId} ProductPrice is: {ProductPrice}");
                    log.LogInformation($"Device: {deviceId} ProductCost is: {ProductCost}");
                    log.LogInformation($"Device: {deviceId} ProductDiscount is: {ProductDiscount}");
                    log.LogInformation($"Device: {deviceId} ProductProfitPerItem is: {ProductProfitPerItem}");
                    log.LogInformation($"Device: {deviceId} ProductProfitPercentagePerItem is: {ProductProfitPercentagePerItem}");
                    log.LogInformation($"Device: {deviceId} ProductImageURL is: {ProductImageURL}");
                    log.LogInformation($"Device: {deviceId} CustomerQuantityLastHour is: {CustomerQuantityLastHour}");
                    log.LogInformation($"Device: {deviceId} CustomerQuantityLastDay is: {CustomerQuantityLastDay}");
                    log.LogInformation($"Device: {deviceId} CustomerQuantityTotal is: {CustomerQuantityTotal}");
                    log.LogInformation($"Device: {deviceId} ShelfProductNames is: {ShelfProductNames}");
                    log.LogInformation($"Device: {deviceId} ShelfItemQuantity is: {ShelfItemQuantity}");
                    log.LogInformation($"Device: {deviceId} ShelfRevenueLastHour is: {ShelfRevenueLastHour}");
                    log.LogInformation($"Device: {deviceId} ShelfRevenueLastDay is: {ShelfRevenueLastDay}");
                    log.LogInformation($"Device: {deviceId} ShelfRevenueTotal is: {ShelfRevenueTotal}");
                    log.LogInformation($"Device: {deviceId} ShelfSoldItemQuantityLastHour is: {ShelfSoldItemQuantityLastHour}");
                    log.LogInformation($"Device: {deviceId} ShelfSoldItemQuantityLastDay is: {ShelfSoldItemQuantityLastDay}");
                    log.LogInformation($"Device: {deviceId} ShelfSoldItemQuantityTotal is: {ShelfSoldItemQuantityTotal}");
                    log.LogInformation($"Device: {deviceId} ShelfCostPerItem is: {ShelfCostPerItem}");
                    log.LogInformation($"Device: {deviceId} ShelfPricePerItem is: {ShelfPricePerItem}");
                    log.LogInformation($"Device: {deviceId} ConversionRateLastHour is: {ConversionRateLastHour}");
                    log.LogInformation($"Device: {deviceId} ConversionRateLastDay is: {ConversionRateLastDay}");
                    log.LogInformation($"Device: {deviceId} ConversionRateTotal is: {ConversionRateTotal}");
                    log.LogInformation($"Device: {deviceId} ShelfProfitLastHour is: {ShelfProfitLastHour}");
                    log.LogInformation($"Device: {deviceId} ShelfProfitLastDay is: {ShelfProfitLastDay}");
                    log.LogInformation($"Device: {deviceId} ShelfProfitTotal is: {ShelfProfitTotal}");
                    var updateProperty = new JsonPatchDocument();
                    var storeTelemetry = new Dictionary<string, Object>()
                    {
                        ["storeid"] = ID,
                        ["TimeInterval"] = TimeInterval,
                        ["ProductName"] = ProductName,
                        ["ProductSellingRank"] = ProductSellingRank,
                        ["SoldProductQuantity"] = SoldProductQuantity,
                        ["RemainProductQuantity"] = RemainProductQuantity,
                        ["ProductPrice"] = ProductPrice,
                        ["ProductCost"] = ProductCost,
                        ["ProductDiscount"] = ProductDiscount,
                        ["ProductProfitPerItem"] = ProductProfitPerItem,
                        ["ProductProfitPercentagePerItem"] = ProductProfitPercentagePerItem,
                        ["ProductImageURL"] = ProductImageURL,
                        ["CustomerQuantityLastHour"] = CustomerQuantityLastHour,
                        ["CustomerQuantityLastDay"] = CustomerQuantityLastDay,
                        ["CustomerQuantityTotal"] = CustomerQuantityTotal,
                        ["ShelfProductNames"] = ShelfProductNames,
                        ["ShelfItemQuantity"] = ShelfItemQuantity,
                        ["ShelfRevenueLastHour"] = ShelfRevenueLastHour,
                        ["ShelfRevenueLastDay"] = ShelfRevenueLastDay,
                        ["ShelfRevenueTotal"] = ShelfRevenueTotal,
                        ["ShelfSoldItemQuantityLastHour"] = ShelfSoldItemQuantityLastHour,
                        ["ShelfSoldItemQuantityLastDay"] = ShelfSoldItemQuantityLastDay,
                        ["ShelfSoldItemQuantityTotal"] = ShelfSoldItemQuantityTotal,
                        ["ShelfCostPerItem"] = ShelfCostPerItem,
                        ["ShelfPricePerItem"] = ShelfPricePerItem,
                        ["ConversionRateLastHour"] = ConversionRateLastHour,
                        ["ConversionRateLastDay"] = ConversionRateLastDay,
                        ["ConversionRateTotal"] = ConversionRateTotal,
                        ["ShelfProfitLastHour"] = ShelfProfitLastHour,
                        ["ShelfProfitLastDay"] = ShelfProfitLastDay,
                        ["ShelfProfitTotal"] = ShelfProfitTotal
                    };
                    updateProperty.AppendAdd("/storeid", ID.Value<string>());

                    log.LogInformation(updateProperty.ToString());
                    try
                    {
                        await client.PublishTelemetryAsync(deviceId, Guid.NewGuid().ToString(), JsonConvert.SerializeObject(storeTelemetry));
                    }
                    catch (Exception e)
                    {
                        log.LogInformation(e.Message);
                    }
                }
            }
            catch (Exception e)
            {
                log.LogInformation(e.Message);
            }
        }
    }
}