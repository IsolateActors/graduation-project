using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineOrderingStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineOrderingStore.ViewModels
{
    public class StoreUserStoreTypeViewModel
    {
        public List<StoreUser> StoreUsers { get; set; }
        public SelectList StoreTypes { get; set; }
        public string SearchString { get; set; }
        public string StoreTypeName { get; set; }
    }
}
