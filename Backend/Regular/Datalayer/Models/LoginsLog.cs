using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Datalayer.Models
{
    public class LoginsLog
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Users")]
        public int UserId { get; set; }
        public Users User { get; set; }
        public string LoginToken { get; set; }
        public bool IsSignOut { get; set; }
        public DateTime LogTime { get; set; }
    }
}
