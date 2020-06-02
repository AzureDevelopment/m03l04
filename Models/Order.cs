using System.Collections.Generic;
using System;
namespace AzureDev.Function
{
    public class Order
    {
        public Guid Id;
        public Guid CustomerId;
        public IEnumerable<Item> OrderedItems;
        public bool Paid;

        public bool Completed;

        internal void MarkCompleted()
        {
            Completed = true;
        }
    }
}