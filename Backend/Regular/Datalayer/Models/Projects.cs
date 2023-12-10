using System.ComponentModel.DataAnnotations;

namespace DataLayer.Models
{
    public class Projects
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "نام پروژه")]
        [Required(ErrorMessage = "لطفا {0} را صحیح وارد کنید")]
        [MaxLength(300, ErrorMessage = "مقدار {0} بیش از حد مجاز طولانی است")]
        [DataType(DataType.Text)]
        public string ProjectName { get; set; }
    }
}
