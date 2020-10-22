using OnlineOrderingStore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineOrderingStore.ViewModels
{
    public class ShoppingCartForPayVIewModel
    {
        public IList<ShoppingCart> ShoppingCarts { get; set; }
        public IList<ShoppingBuyCount> ShoppingGoodsWithBuyCounts { get; set; }

        public string AllPay { get; set; }

        [Required(ErrorMessage = "请输入收件人名称！")]
        [StringLength(10, MinimumLength = 2, ErrorMessage = "必须在2-10个字符之间！")]
        public string ConsigneeName { get; set; }


        [Required(ErrorMessage = "请输入手机号！")]
        [Phone(ErrorMessage = "请输入正确的手机号！")]
        public string Phone { get; set; }


        [Required(ErrorMessage = "请输入地址！")]
        public string Address { get; set; }
    }
}
