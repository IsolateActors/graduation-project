using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineOrderingStore.Models
{
    public class StoreType
    {
        public int ID { get; set; }

        [DisplayName("店铺类型")]
        public string TypeName { get; set; }

        [DisplayName("添加时间")]
        public DateTime CreateTime { get; set; }

        [DisplayName("修改时间")]
        public DateTime EditTime { get; set; }

        public ICollection<StoreUser> StoreUsers { get; set; }

        

    }
}
