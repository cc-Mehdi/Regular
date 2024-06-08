using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Datalayer.Models
{
    public class Organizations_Users
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Organizations")]
        public int OrganizationId { get; set; }
        public Organizations Organization { get; set; }

        [ForeignKey("Users")]
        public int UserId { get; set; }
        public Users User { get; set; }

        [Display(Name = "وضعیت درخواست")]
        [DataType(DataType.Text)]
        public string InviteStatus { get; set; }

        [Display(Name = "تاریخ درخواست")]
        [DataType(DataType.DateTime)]
        public DateTime CreateInviteDate { get; set; }

        [Display(Name = "تاریخ پذیرش درخواست")]
        [DataType(DataType.DateTime)]
        public DateTime AcceptInviteDate { get; set; }
    }
}
