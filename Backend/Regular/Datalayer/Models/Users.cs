using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Datalayer.Models
{
    public class Users
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "نام کامل")]
        [Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "مقدار {0} بیش از حد مجاز است")]
        [DataType(DataType.Text)]
        public string FullName { get; set; }

        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "مقدار {0} بیش از حد مجاز است")]
        [DataType(DataType.Text)]
        public string Username { get; set; }

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
        [MaxLength(1000, ErrorMessage = "مقدار {0} بیش از حد مجاز است")]
        [MinLength(6, ErrorMessage = "کلمه عبور باید بیشتر از 6 کاراکتر باشد")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "مقدار {0} بیش از حد مجاز است")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "تصویر پروفایل")]
        [DataType(DataType.Text)]
        public string ImageName { get; set; }

        [Display(Name = "سطح مهارت")]
        [Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "مقدار {0} بیش از حد مجاز است")]
        [DataType(DataType.Text)]
        public string Rank { get; set; }

        [Display(Name = "وضعیت")]
        [Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "مقدار {0} بیش از حد مجاز است")]
        [DataType(DataType.Text)]
        public string Status { get; set; }
    }
}
