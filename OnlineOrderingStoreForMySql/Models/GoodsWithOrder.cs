using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineOrderingStore.Models
{
    public class GoodsWithOrder
    {
        public Guid Id { get; set; }

        [DisplayName("购买数量")]
        public int BuyCount { get; set; }

        [DisplayName("配送时间")]
        public DateTime DeliveredTime { get; set; }//配送时间
        public bool Delivered { get; set; }

        public Guid GoodsId { get; set; }
        public Goods Goods { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
    }
}
