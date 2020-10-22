using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineOrderingStore.Models
{
    public class ShoppingCart
    {
        public Guid Id { get; set; }

        [DisplayName("购买件数")]
        public int BuyCount { get; set; }

        public Guid GoodsID { get; set; }
        public Goods Goods { get; set; }

        public int OnlineUserId { get; set; }
        public OnlineUser OnlineUser { get; set; }
    }
}
