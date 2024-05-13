using System.ComponentModel.DataAnnotations;

namespace Datalayer.Models
{
    public class User_Project
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProjectId { get; set; }
    }
}
