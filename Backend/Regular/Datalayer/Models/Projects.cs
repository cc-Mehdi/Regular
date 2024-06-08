using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Datalayer.Models
{
    public class Projects
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "عنوان پروژه")]
        [Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "مقدار {0} بیش از حد مجاز است")]
        [DataType(DataType.Text)]
        public string Title { get; set; }

        [Display(Name = "موسس")]
        public Users Owner { get; set; }

        [Display(Name = "موسس")]
        [ForeignKey("Users")]
        public int OwnerId { get; set; }

        [Display(Name = "سازمان")]
        [Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "مقدار {0} بیش از حد مجاز است")]
        [DataType(DataType.Text)]
        public string Organization { get; set; }

        public int OrganizationId { get; set; }

        [Display(Name = "لوگو پروژه")]
        [DataType(DataType.Text)]
        public string ImageName { get; set; }

        public int TasksStatusPercent { get; set; }
        public int TasksCount { get; set; }
    }
}
