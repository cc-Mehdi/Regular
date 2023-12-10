using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models
{
    public class Tasks
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "عنوان وظیفه")]
        [Required(ErrorMessage = "لطفا {0} را صحیح وارد کنید")]
        [MaxLength(200, ErrorMessage = "مقدار {0} بیش از حد مجاز طولانی است")]
        [DataType(DataType.Text)]
        public string Title { get; set; }

        [ForeignKey("Projects")]
        public int ProjectId { get; set; }
        public Projects Project { get; set; }

        [ForeignKey("Users")]
        public int UserId { get; set; }
        public Users User { get; set; }

        [Display(Name = "نوع وظیفه")]
        [Required(ErrorMessage = "لطفا {0} را صحیح وارد کنید")]
        [MaxLength(100, ErrorMessage = "مقدار {0} بیش از حد مجاز طولانی است")]
        [DataType(DataType.Text)]
        public string TaskType { get; set; }

        [Display(Name = "زمان تقریبی")]
        [Required(ErrorMessage = "لطفا {0} را صحیح وارد کنید")]
        [MaxLength(50, ErrorMessage = "مقدار {0} بیش از حد مجاز طولانی است")]
        [DataType(DataType.Text)]
        public string EstimateTime { get; set; }

        [Display(Name = "زمان ثبت شده")]
        [Required(ErrorMessage = "لطفا {0} را صحیح وارد کنید")]
        [MaxLength(50, ErrorMessage = "مقدار {0} بیش از حد مجاز طولانی است")]
        [DataType(DataType.Text)]
        public string LogedTime { get; set; }

        [Display(Name = "وضعیت وظیفه")]
        [Required(ErrorMessage = "لطفا {0} را صحیح وارد کنید")]
        [MaxLength(100, ErrorMessage = "مقدار {0} بیش از حد مجاز طولانی است")]
        [DataType(DataType.Text)]
        public string TaskStatus { get; set; }

        [Display(Name = "سطح اولویت")]
        [Required(ErrorMessage = "لطفا {0} را صحیح وارد کنید")]
        [MaxLength(100, ErrorMessage = "مقدار {0} بیش از حد مجاز طولانی است")]
        [DataType(DataType.Text)]
        public string TaskPriority { get; set; }

        [Display(Name = "مشکل در نسخه")]
        [Required(ErrorMessage = "لطفا {0} را صحیح وارد کنید")]
        [MaxLength(50, ErrorMessage = "مقدار {0} بیش از حد مجاز طولانی است")]
        [DataType(DataType.Text)]
        public string BeforeVersion { get; set; }

        [Display(Name = "اصلاح در نسخه")]
        [Required(ErrorMessage = "لطفا {0} را صحیح وارد کنید")]
        [MaxLength(50, ErrorMessage = "مقدار {0} بیش از حد مجاز طولانی است")]
        [DataType(DataType.Text)]
        public string AfterVersion { get; set; }

        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "لطفا {0} را صحیح وارد کنید")]
        [MaxLength(500, ErrorMessage = "مقدار {0} بیش از حد مجاز طولانی است")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

    }
}
