using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;

namespace AzureDev.Function
{
    public static class DurableFunctionsOrchestration
    {
        [FunctionName("OrchestrateOrderDelivery")]
        public static async Task<bool> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            Order order = context.GetInput<Order>();

            var customer = await context.CallActivityAsync<Customer>("GetCustomerDetailedInformations", order.CustomerId);
            for (int i = 0; i < 2 && !order.Paid; i++)
            {
                DateTime deadline = context.CurrentUtcDateTime.Add(TimeSpan.FromSeconds(5));
                await context.CreateTimer(deadline, CancellationToken.None);
                order.Paid = await context.CallActivityAsync<bool>("CheckPaymentStatus", order.Id);
            }
            if (order.Paid)
            {
                var invoiceUrl = await context.CallActivityAsync<string>("GenerateInvoice", order);
                var emailDetails = new EmailDetails() { OrderId = order.Id, CustomerEmail = customer.Email, InvoiceUrl = invoiceUrl };
                await context.CallActivityAsync<string>("SendInvoice", emailDetails);
                order.MarkCompleted();
            }
            else
            {
                //compensate
            }
            return order.Completed;
        }

        [FunctionName("OrchestrateOrderDelivery_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            // Function input comes from the request content.
            var Order = new Order()
            {
                Id = Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                OrderedItems = new List<Item>(){
                    new Item(){
                        Name="Drone",
                        Price=1000.00
                    },
                    new Item()
                    {
                        Name = "Propelers",
                        Price = 10.00
                    }
                }
            };
            string instanceId = await starter.StartNewAsync("OrchestrateOrderDelivery", null, Order);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}
