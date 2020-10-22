using OnlineOrderingStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineOrderingStore.ViewModels
{
    public class StoreUserGoodsWithOrderDetailsViewModel
    {
        public IList<GoodsWithOrder> DeliveredGoods { get; set; }
        public IList<GoodsWithOrder> NotDeliveredGoods { get; set; }
    }
}
