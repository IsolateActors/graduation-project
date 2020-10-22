using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineOrderingStore.Models
{
    public class Goods
    {
        public Guid Id { get; set; }

        [DisplayName("商品名称")]
        [Required(ErrorMessage = "请输入商品名称！")]
        [StringLength(10, MinimumLength = 2, ErrorMessage = "必须在2-10个字符之间！")]
        public string Name { get; set; }

        [DisplayName("单价")]
        [Required(ErrorMessage = "请输入商品价格！")]
        public decimal Price { get; set; }

        [DisplayName("库存")]
        [Required(ErrorMessage = "请输入商品剩余量！")]
        public int Stock { get; set; } //库存
        [DisplayName("发布时间")]
        public DateTime ReleaseTime { get; set; }
        [DisplayName("修改时间")]
        public DateTime EditTime { get; set; }
        [DisplayName("相片")]
        public string PhotoPath { get; set; }

        [Required(ErrorMessage = "请选择商品分类！")]
        public Guid GoodsTypeId { get; set; }
        [DisplayName("商品分类")]
        public GoodsType GoodsType { get; set; }
        public int StoreUserId { get; set; }
        public StoreUser StoreUser { get; set; }

        public IList<ShoppingCart> ShoppingCarts { get; set; }
        public IList<GoodsWithOrder> GoodsWithOrders { get; set; }
    }
}
