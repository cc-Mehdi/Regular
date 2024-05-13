using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Datalayer.Models
{
    public class EmployeeInvites
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "وضعیت درخواست")]
        [DataType(DataType.Text)]
        public string InviteStatus { get; set; }

        [Display(Name = "سازمان")]
        [ForeignKey("Organizations")]
        public int OrganizationId { get; set; }

        [Display(Name = "سازمان")]
        public Organizations Organization { get; set; }

        [Display(Name = "کاربر")]
        [ForeignKey("Users")]
        public int UserId { get; set; }

        [Display(Name = "کاربر")]
        public Users User { get; set; }
    }
}
