using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Datalayer.Models
{
    public class Organizations
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "عنوان سازمان")]
        [Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "مقدار {0} بیش از حد مجاز است")]
        [DataType(DataType.Text)]
        public string Title { get; set; }


        [Display(Name = "موسس")]
        [ForeignKey("Users")]
        public int OwnerId { get; set; }
        public Users Owner { get; set; }

        [Display(Name = "لوگو سازمان")]
        [DataType(DataType.Text)]
        public string ImageName { get; set; }
    }
}
