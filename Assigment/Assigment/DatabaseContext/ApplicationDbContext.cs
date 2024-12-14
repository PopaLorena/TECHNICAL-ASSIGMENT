﻿using Assigment.ModelsDao;
using Microsoft.EntityFrameworkCore;

namespace Assigment.DatabaseContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
       : base(options)
        {
        }
        public DbSet<UserDao> Users { get; set; } = null!;
        public DbSet<PartyDao> Parties { get; set; } = null!;
        public DbSet<ItemDao> Items { get; set; } = null!;
        public DbSet<ItemPartyDao> ItemParties { get; set; } = null!;
        public DbSet<ProposalDao> Proposals { get; set; } = null!;
        public DbSet<CounterProposalDao> CounterProposal { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           // Configure party-user relationship
            modelBuilder.Entity<PartyDao>()
               .HasMany(p => p.Users)
               .WithOne()
               .HasForeignKey(u => u.PartyId);

            // Configure party-item relationship
            modelBuilder.Entity<PartyDao>()
                .HasMany(p => p.ItemParties)
                .WithOne()
                .HasForeignKey(ip => ip.PartyId);

            modelBuilder.Entity<ItemDao>()
                .HasMany(p => p.ItemParties)
                .WithOne(i => i.Item)
                .HasForeignKey(ip => ip.ItemId);

            // Configure item-Proposal relationship

            modelBuilder.Entity<ItemDao>()
                .HasMany(i => i.Proposals)
                .WithOne()
                .HasForeignKey(p => p.ItemId);

            // Configure Proposal-user relationship
            modelBuilder.Entity<UserDao>()
                .HasMany(u => u.Proposals)
                .WithOne()
                .HasForeignKey(p => p.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Proposal-CounterProposal relationships
            modelBuilder.Entity<ProposalDao>()
                .HasMany(pr => pr.CounterProposals)
                .WithOne()
                .HasForeignKey(pr => pr.ProposalId);

            // Configure CounterProposal-user relationship
            modelBuilder.Entity<UserDao>()
                .HasMany(u => u.CounterProposals)
                .WithOne()
                .HasForeignKey(p => p.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
