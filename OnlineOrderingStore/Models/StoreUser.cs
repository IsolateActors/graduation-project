using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineOrderingStore.Models
{
    public class StoreUser
    {
        public int ID { get; set; }

        [DisplayName("店铺名称")]
        [Required(ErrorMessage = "请输入店铺名称！")]
        [StringLength(10, MinimumLength = 2, ErrorMessage ="必须在2-10个字符之间！")]
        public string StoreName { get; set; }

        [DisplayName("店铺账户")]
        [Required(ErrorMessage = "请输入账号！")]
        [StringLength(11, MinimumLength = 8, ErrorMessage = "必须为8-11位数字！")]
        public string Account { get; set; }

        [DisplayName("密码")]
        [Required(ErrorMessage = "请输入密码！")]
        [StringLength(15,MinimumLength = 6, ErrorMessage ="长度为6 - 15个字符！")]
        public string PassWord { get; set; }

        [DisplayName("创建时间")]
        [DataType(DataType.Date)]
        public DateTime RegisterTime { get; set; }

        [Required(ErrorMessage ="请选择类型！")]
        public int? StoreTypeID { get; set; }
        public StoreType StoreType { get; set; }

       
        public IList<GoodsType> GoodsTypes { get; set; }
        public IList<Goods> Goods { get; set; }
    }
}
