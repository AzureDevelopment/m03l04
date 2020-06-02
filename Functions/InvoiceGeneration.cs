using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using System;


namespace AzureDev.Function
{
    public class InvoiceGeneration
    {
        [FunctionName("GenerateInvoice")]
        public async Task<string> GenerateInvoice([ActivityTrigger] Order order, [Blob("invoices")] CloudBlobContainer invoicesContainer, ILogger log)
        {

            var blobName = order.Id.ToString() + ".txt";

            var invoiceBlob = invoicesContainer.GetBlockBlobReference(blobName);

            await invoiceBlob.UploadTextAsync($"Example invoice for order no. {order.Id}");
            return GetSasUri(invoiceBlob);
        }
        private string GetSasUri(CloudBlockBlob blob)
        {
            var sas = blob.GetSharedAccessSignature(new SharedAccessBlobPolicy()
            {
                Permissions = SharedAccessBlobPermissions.Read,
                SharedAccessStartTime = DateTimeOffset.UtcNow,
                SharedAccessExpiryTime = DateTimeOffset.UtcNow.AddDays(1),
            });
            return blob.StorageUri.PrimaryUri + sas;
        }
    }
}