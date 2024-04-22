using EventMe.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EventMe.Infrastructure.Data
{
    /// <summary>
    /// Context of the Database
    /// </summary>
    public class EventMeDbContext : DbContext

    {
        /// <summary>
        /// Database context constructor
        /// </summary>
        /// <param name="options"></param>
        public EventMeDbContext(DbContextOptions<EventMeDbContext> options)
            :base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //QueryFilter - винаги когато правим заявка към тази таблиза този фитър ще се прилага и ще имаме само активните евенти.
            modelBuilder.Entity<Event>()
                .HasQueryFilter(e => e.IsActive);
        }
        /// <summary>
        /// Events
        /// </summary>
        public DbSet<Event> Events { get; set; } = null!;

        /// <summary>
        /// Towns
        /// </summary>
        public DbSet<Town> Towns { get; set; }=null!;

        /// <summary>
        /// Addresses
        /// </summary>
        public DbSet<Address> Addresses { get; set; } = null!;


    }
}
