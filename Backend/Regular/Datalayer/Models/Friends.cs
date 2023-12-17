using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models
{
    public class Friends
    {
        [Key]
        public int Id { get; set; }


        public int UserId1 { get; set; }

        [Display(Name = "کد همکار")]
        public int UserId2 { get; set; }

        [ForeignKey("UserId2")]
        [Display(Name = "همکار")]
        public Users User2 { get; set; }

    }
}
