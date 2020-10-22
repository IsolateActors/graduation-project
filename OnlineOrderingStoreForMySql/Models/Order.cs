using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineOrderingStore.Models
{
    public class Order
    {
        [DisplayName("订单ID")]
        public Guid Id { get; set; }

        [DisplayName("下单时间")]
        public DateTime CreateTime { get; set; }

        [DisplayName("收货人")]
        public string ConsigneeName { get; set; }//收货人名字

        [DisplayName("地址")]
        public string Address { get; set; }

        [DisplayName("电话")]
        public long Phone { get; set; }

        [DisplayName("支付金额")]
        public decimal Pay { get; set; }
        


        public IList<GoodsWithOrder> GoodsWithOrders { get; set; }

        public int OnlineUserId { get; set; }
        public OnlineUser OnlineUser { get; set; }

        //public Guid? ShoppingCartId { get; set; }
        //public ShoppingCart ShoppingCart { get; set; }
    }
}
