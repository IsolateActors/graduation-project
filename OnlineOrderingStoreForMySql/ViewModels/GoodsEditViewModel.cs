using Microsoft.AspNetCore.Http;
using OnlineOrderingStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineOrderingStore.ViewModels
{
    public class GoodsEditViewModel
    {
        public Goods Goods { get; set; }
        public IFormFile Photo { get; set; }
    }
}
