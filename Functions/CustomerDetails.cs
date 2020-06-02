using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System;

namespace AzureDev.Function
{
    public class CustomerDetails
    {
        [FunctionName("GetCustomerDetailedInformations")]
        public static Task<Customer> GetCustomerDetailedInformations([ActivityTrigger] Guid customerId, ILogger log)
        {
            var customer = new Customer()
            {
                Id = customerId,
                FirstName = "Bartłomiej",
                LastName = "Glac",
                Email = "bartek@gruba.it"
            };

            return Task.FromResult(customer);
        }
    }
}