using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Datalayer.Models
{
    public class Users_Users
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Users")]
        public int SenderUserId { get; set; }
        public Users SenderUser { get; set; }

        [ForeignKey("Users")]
        public int ReceiverUserId { get; set; }
        public Users ReceiverUser { get; set; }

        [Display(Name = "تاریخ درخواست")]
        [DataType(DataType.DateTime)]
        public DateTime CreateInviteDate { get; set; }

        [Display(Name = "تاریخ پذیرش درخواست")]
        [DataType(DataType.DateTime)]
        public DateTime AcceptInviteDate { get; set; }
    }
}
