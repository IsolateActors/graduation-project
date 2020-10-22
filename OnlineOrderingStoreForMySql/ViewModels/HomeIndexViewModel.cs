using OnlineOrderingStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineOrderingStore.ViewModels
{
    public class HomeIndexViewModel
    {
        public List<Goods> Goods { get; set; }
        public string StoreTypeName { get; set; }
        public string StoreName { get; set; }
        public string GoodsTypeName { get; set; }
        public string SearchingString { get; set; }
    }
}
