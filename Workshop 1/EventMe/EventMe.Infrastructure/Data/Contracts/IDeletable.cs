namespace EventMe.Infrastructure.Data.Contracts
{
    /// <summary>
    /// Entity which can be deleted
    /// </summary>
    public interface IDeletable
    {
        /// <summary>
        /// Record is active
        /// </summary>
        bool IsActive { get; set; }

        /// <summary>
        /// Deleted date
        /// </summary>
        public DateTime? DeletedOn { get; set; }
    }
}
