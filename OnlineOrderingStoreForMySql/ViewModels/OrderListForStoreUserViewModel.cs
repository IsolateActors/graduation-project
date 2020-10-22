using OnlineOrderingStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineOrderingStore.ViewModels
{
    public class OrderListForStoreUserViewModel
    {
        public IList<Order> DeliveredOrder { get; set; }
        public IList<Order> NotDeliveredOrder { get; set; }
    }
}
