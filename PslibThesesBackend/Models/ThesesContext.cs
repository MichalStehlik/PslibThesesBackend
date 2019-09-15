using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PslibThesesBackend.Models
{
    public class ThesesContext : DbContext
    {
        public ThesesContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Idea> Ideas { get; set; }
        public DbSet<IdeaGoal> IdeaGoals { get; set; }
    }
}
