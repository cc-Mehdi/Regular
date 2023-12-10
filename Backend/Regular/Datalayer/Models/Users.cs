using System.ComponentModel.DataAnnotations;

namespace Datalayer.Models
{
    public class Users
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را صحیح وارد کنید")]
        [MaxLength(300, ErrorMessage = "مقدار {0} بیش از حد مجاز طولانی است")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را صحیح وارد کنید")]
        [MaxLength(300, ErrorMessage = "مقدار {0} بیش از حد مجاز طولانی است")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفا {0} را صحیح وارد کنید")]
        [MaxLength(200, ErrorMessage = "مقدار {0} بیش از حد مجاز طولانی است")]
        [DataType(DataType.Text)]
        public string Username { get; set; }

        [Display(Name = "نام")]
        [Required(ErrorMessage = "لطفا {0} را صحیح وارد کنید")]
        [MaxLength(150, ErrorMessage = "مقدار {0} بیش از حد مجاز طولانی است")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را صحیح وارد کنید")]
        [MaxLength(150, ErrorMessage = "مقدار {0} بیش از حد مجاز طولانی است")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }
    }
}
