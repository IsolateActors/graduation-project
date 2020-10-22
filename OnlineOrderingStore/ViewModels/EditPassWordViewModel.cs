using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineOrderingStore.ViewModels
{
    public class EditPassWordViewModel
    {
        [Required(ErrorMessage = "请输入旧密码")]
        [Display(Name = "旧密码")]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "长度为6 - 15个字符！")]
        public string OldPassWord { get; set; }

        [Required(ErrorMessage = "请输入新密码")]
        [Display(Name = "新密码")]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "长度为6 - 15个字符！")]
        public string NewPassWord { get; set; }

        [Required(ErrorMessage = "请重复输入新密码")]
        [Display(Name = "重复新密码")]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "长度为6 - 15个字符！")]
        public string RepeatPassword { get; set; }
    }
}
