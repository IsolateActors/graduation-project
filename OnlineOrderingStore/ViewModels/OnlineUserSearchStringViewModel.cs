using OnlineOrderingStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineOrderingStore.ViewModels
{
    public class OnlineUserSearchStringViewModel
    {
        public List<OnlineUser> OnlineUsers { get; set; }
        public string SearchString { get; set; }
    }
}
