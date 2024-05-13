using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Datalayer.Models
{
    public class Organization_Project
    {
        [Key]
        public int Id { get; set; }

        public Organizations Organization { get; set; }

        [ForeignKey("Organizations")]
        public int OrganizationId { get; set; }

        public Projects Project { get; set; }

        [ForeignKey("Projects")]
        public int ProjectId { get; set; }
    }
}
