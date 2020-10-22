using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OnlineOrderingStore.ViewModels
{
    public class GoodsCreateViewModel
    {
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

        [DisplayName("相片")]
        public IFormFile Photo { get; set; }

        [DisplayName("商品分类")]
        [Required(ErrorMessage = "请选择商品分类！")]
        public Guid GoodsTypeId { get; set; }
        
        
    }
}
