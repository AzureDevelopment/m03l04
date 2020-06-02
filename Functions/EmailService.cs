using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Mail;

namespace AzureDev.Function
{
    public class EmailService
    {

        [FunctionName("SendInvoice")]
        public void SendEmail([ActivityTrigger] EmailDetails emailDetails, [SendGrid(ApiKey = "SendGridKeyAppSettingName")] out SendGridMessage message, ILogger log)
        {
            message = new SendGridMessage();
            message.AddTo(new EmailAddress("bartek@gruba.it"));
            message.AddContent("text/html", $"Hi, thank you for your order no . Your invoce can be found under this link: {emailDetails.InvoiceUrl}");
            message.Subject = "Thanks for your order, your invoice is waiting for you";
            message.From = new EmailAddress(emailDetails.CustomerEmail);
        }
    }
}