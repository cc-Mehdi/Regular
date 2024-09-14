using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Datalayer.Models
{
    public class Organizations
    {
        [Key]
        public int Id { get; set; }

        public string PublicId { get; set; }

        [Display(Name = "عنوان سازمان")]
        [Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
        [MinLength(3, ErrorMessage = "لطفا یک نام مناسب برای {0} انتخاب کنید")]
        [MaxLength(300, ErrorMessage = "مقدار {0} بیش از حد مجاز است")]
        [DataType(DataType.Text)]
        public string OrgTitle { get; set; }


        [Display(Name = "موسس")]
        [ForeignKey("Users")]
        public int OwnerId { get; set; }
        public Users Owner { get; set; }

        [Display(Name = "لوگو سازمان")]
        [DataType(DataType.Text)]
        public string ImageName { get; set; }
    }
}
