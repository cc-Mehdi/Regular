using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Datalayer.Models
{
    public class Projects
    {
        [Key]
        public int Id { get; set; }

        public string PublicId { get; set; }

        [Display(Name = "عنوان پروژه")]
        [Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "مقدار {0} بیش از حد مجاز است")]
        [MinLength(3, ErrorMessage = "لطفا یک نام مناسب برای {0} انتخاب کنید")]
        [DataType(DataType.Text)]
        public string Title { get; set; }

        [Display(Name = "موسس")]
        public Users Owner { get; set; }

        [Display(Name = "موسس")]
        [ForeignKey("Owner")]
        public int OwnerId { get; set; }

        [Display(Name = "سازمان")]
        public Organizations Organization { get; set; }

        [Display(Name = "سازمان")]
        [ForeignKey("Organization")]
        public int OrganizationId { get; set; }

        [Display(Name = "لوگو پروژه")]
        [DataType(DataType.Text)]
        public string ImageName { get; set; }

        public int TasksStatusPercent { get; set; }
        public int TasksCount { get; set; }
    }
}
