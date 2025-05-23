﻿using IndependedTrees.DAL.Models.Journal;
using IndependedTrees.DAL.Models.Tree;
using Microsoft.EntityFrameworkCore;

namespace IndependedTrees.DAL
{
    public class IndependedTreesDbContext : DbContext
    {
        public DbSet<JournalRecord> Journal { get; set; }
        public DbSet<TreeNode> TreeNodes { get; set; }

        public IndependedTreesDbContext() : base() { }

        public IndependedTreesDbContext(DbContextOptions<IndependedTreesDbContext> options)
           : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<JournalRecord>()
                .HasKey(x => x.EventId);
        }
    }
}
