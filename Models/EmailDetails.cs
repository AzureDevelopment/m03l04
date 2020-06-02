using System;

namespace AzureDev.Function
{
    public class EmailDetails
    {
        public EmailDetails()
        {
        }

        public Guid OrderId { get; set; }
        public string CustomerEmail { get; set; }
        public string InvoiceUrl { get; set; }
    }
}
