using System.ComponentModel.DataAnnotations;

namespace Datalayer.Models
{
    public class Organization_Project
    {
        [Key]
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public int ProjectId { get; set; }
    }
}
