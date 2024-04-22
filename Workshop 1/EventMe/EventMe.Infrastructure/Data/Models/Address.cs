using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventMe.Infrastructure.Data.Models
{
    /// <summary>
    /// Location/Place of the event
    /// </summary>
    [Comment("Location/Place of the event")]
    public class Address
    {
        /// <summary>
        /// Id of the place
        /// </summary>
        [Key]
        [Comment("Location/Place of the event")]
        public int Id { get; set; }

        /// <summary>
        /// Id of the town
        /// </summary>
        [Required]
        [Comment("Id of the town")]
        public int TownId { get; set; }

        /// <summary>
        /// Address of the place
        /// </summary>
        [Required]
        [Comment("Address of the place")]
        [MaxLength(100)]
        public string StreetAddress { get; set; } = null!;

        /// <summary>
        /// Town
        /// </summary>
        [Required]
        [Comment("Town")]
        [ForeignKey(nameof(TownId))]
        public Town Town { get; set; } = null!;
    }
}
