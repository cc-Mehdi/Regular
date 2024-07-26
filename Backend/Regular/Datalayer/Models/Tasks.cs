using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Datalayer.Models
{
    public class Tasks
    {
        [Key]
        public int Id { get; set; }

        public string PublicId { get; set; }

        [Display(Name = "پروژه")]
        [ForeignKey("Projects")]
        public int ProjectId { get; set; }

        [Display(Name = "پروژه")]
        public Projects Project { get; set; }

        [Display(Name = "عنوان وظیفه")]
        [Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "مقدار {0} بیش از حد مجاز است")]
        [DataType(DataType.Text)]
        public string Title { get; set; }

        [Display(Name = "اولویت")]
        [Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "مقدار {0} بیش از حد مجاز است")]
        [DataType(DataType.Text)]
        public string Priority { get; set; }

        [Display(Name = "اختصاص یافته به")]
        [Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "مقدار {0} بیش از حد مجاز است")]
        [DataType(DataType.Text)]
        public string Assignto { get; set; }
        public int AssigntoId { get; set; }

        public int ReporterId { get; set; }

        [Display(Name = "زمان تخمینی")]
        [Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "مقدار {0} بیش از حد مجاز است")]
        [DataType(DataType.Text)]
        public string EstimateTime { get; set; }

        [Display(Name = "زمان باقی مانده")]
        [Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "مقدار {0} بیش از حد مجاز است")]
        [DataType(DataType.Text)]
        public string RemainingTime { get; set; }

        [Display(Name = "زمان ثبت شده")]
        [Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "مقدار {0} بیش از حد مجاز است")]
        [DataType(DataType.Text)]
        public string LoggedTime { get; set; }

        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
        [MaxLength(750, ErrorMessage = "مقدار {0} بیش از حد مجاز است")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "وضعیت وظیفه")]
        [Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "مقدار {0} بیش از حد مجاز است")]
        [DataType(DataType.Text)]
        public string TaskStatus { get; set; }

        [Display(Name = "نوع وظیفه")]
        [Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "مقدار {0} بیش از حد مجاز است")]
        [DataType(DataType.Text)]
        public string TaskType { get; set; }
    }
}
