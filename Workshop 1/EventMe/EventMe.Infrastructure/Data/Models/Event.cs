using EventMe.Infrastructure.Data.Contracts;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventMe.Infrastructure.Data.Models
{
    /// <summary>
    /// Event
    /// </summary>
    [Comment("Event")]
    public class Event:IDeletable
    {
        /// <summary>
        /// Event Id
        /// </summary>
        [Comment("Event Id")]
        [Key]
        public  int Id { get; set; }

        /// <summary>
        /// Name of the event
        /// </summary>
        [Required]
        [Comment("Name of the event")]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Start of the event
        /// </summary>
        [Required]
        [Comment("Start of the event")]
        public DateTime Start { get; set; }

        /// <summary>
        /// End of the event
        /// </summary>
        [Required]
        [Comment("End of the event")]
        public DateTime End { get; set; }

        /// <summary>
        /// Place Id
        /// </summary>
        [Required]
        [Comment("Place Id")]
        public int PlaceId { get; set; }

        /// <summary>
        /// State is active
        /// </summary>
        [Required]
        [Comment("State is active")]
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Deleted date
        /// </summary>
        [Comment("Daleted date")]
        public DateTime? DeletedOn { get ; set; }

        /// <summary>
        /// Place of the event
        /// </summary>
        [Required]
        [Comment("Place of the event")]
        [ForeignKey(nameof(PlaceId))]
        public Address Place { get; set; } = null!;


       
    }
}
