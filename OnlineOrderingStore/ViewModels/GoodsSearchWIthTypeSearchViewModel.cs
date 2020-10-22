using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineOrderingStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineOrderingStore.ViewModels
{
    public class GoodsSearchWIthTypeSearchViewModel
    {
        public List<Goods> Goods { get; set; }
        public SelectList GoodsTypes { get; set; }
        public string SearchString { get; set; }
        public string GoodsTypeName { get; set; }
    }
}
