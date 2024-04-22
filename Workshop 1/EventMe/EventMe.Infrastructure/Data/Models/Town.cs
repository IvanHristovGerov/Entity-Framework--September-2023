using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace EventMe.Infrastructure.Data.Models
{
    /// <summary>
    /// Town of the event
    /// </summary>
    [Comment("Town")]
    public class Town
    {
        /// <summary>
        /// Id of the town
        /// </summary>
        [Key]
        [Comment("Id of the town")]
        public int Id { get; set; }

        /// <summary>
        /// Name of the town
        /// </summary>
        [Required]
        [Comment("Name of the town")]
        [MaxLength(50)]
        public string Name { get; set; } = null!;
    }
}
