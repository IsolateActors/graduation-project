using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineOrderingStore.Models
{
    public class OnlineUser
    {
        public int ID { get; set; }


        [Required(ErrorMessage = "请输入名称！")]
        [StringLength(10, MinimumLength = 2, ErrorMessage = "必须在2-10个字符之间！")]
        [DisplayName("名称")]
        public string Name { get; set; }

        [DisplayName("账户")]
        [Required(ErrorMessage = "请输入账号！")]
        [StringLength(11, MinimumLength = 8, ErrorMessage = "必须为8-11位数字！")]
        public string Account { get; set; }

        [DisplayName("密码")]
        [Required(ErrorMessage = "请输入密码！")]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "长度为6 - 15个字符！")]
        public string PassWord { get; set; }

        [DisplayName("注册时间")]
        [DataType(DataType.DateTime)]
        public DateTime RegisterTime { get; set; }

        [DisplayName("修改时间")]
        [DataType(DataType.DateTime)]
        public DateTime EditTime { get; set; }


        public IList<Order> Orders { get; set; }

    }
}
