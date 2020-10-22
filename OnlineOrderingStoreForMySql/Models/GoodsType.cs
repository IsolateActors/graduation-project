using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace OnlineOrderingStore.Models
{
    public class GoodsType
    {
        public Guid ID { get; set; }

        [DisplayName("分类")]
        public string GoodsTypeName { get; set; }

        [DisplayName("添加时间")]
        public DateTime CreateTime { get; set; }

        [DisplayName("修改时间")]
        public DateTime EditTime { get; set; }

        public IList<Goods> Goods { get; set; }
        public int StoreUserId { get; set; }
        public StoreUser StoreUser { get; set; }
    }
}
