﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        public DbSet<IdeaOutline> IdeaOutlines { get; set; }
        public DbSet<IdeaTarget> IdeaTargets { get; set; }
        public DbSet<Target> Targets { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdeaGoal>().HasIndex(ig => new { ig.IdeaId, ig.Order }).IsUnique();
            modelBuilder.Entity<IdeaOutline>().HasIndex(io => new { io.IdeaId, io.Order }).IsUnique();
            modelBuilder.Entity<IdeaTarget>().HasIndex(it => new { it.IdeaId, it.TargetId }).IsUnique();
            #region IdeaTargetSeed
            modelBuilder.Entity<Target>().HasData(new Target { Id = 1, Text = "MP Lyceum", Color = Color.Yellow});
            modelBuilder.Entity<Target>().HasData(new Target { Id = 2, Text = "RP Lyceum", Color = Color.Orange });
            modelBuilder.Entity<Target>().HasData(new Target { Id = 3, Text = "MP IT", Color = Color.Red });
            modelBuilder.Entity<Target>().HasData(new Target { Id = 4, Text = "MP Strojírenství", Color = Color.Blue });
            modelBuilder.Entity<Target>().HasData(new Target { Id = 5, Text = "MP Elektrotechnika", Color = Color.Green });
            #endregion
        }
    }
}
