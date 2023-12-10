using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Datalayer.Models
{
    public class Friends
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Users")]
        public int UserId1 { get; set; }
        public Users User1 { get; set; }

        [ForeignKey("Users")]
        public int UserId2 { get; set; }
        public Users User2 { get; set; }
    }
}
