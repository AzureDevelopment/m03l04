using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System;

namespace AzureDev.Function
{
    public class PaymentStatusFunction
    {
        [FunctionName("CheckPaymentStatus")]
        public bool CheckPaymentStatus([ActivityTrigger] string name, ILogger log)
        {
            Random gen = new Random();
            int prob = gen.Next(2);
            return prob <= 1;
        }
    }
}