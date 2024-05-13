using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Datalayer.Models
{
    public class User_Project
    {
        [Key]
        public int Id { get; set; }
        public Users User { get; set; }

        [ForeignKey("Users")]
        public int UserId { get; set; }

        public Projects Project { get; set; }

        [ForeignKey("Projects")]

        public int ProjectId { get; set; }
    }
}
