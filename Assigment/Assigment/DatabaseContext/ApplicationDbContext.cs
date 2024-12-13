using Assigment.Models;
using Microsoft.EntityFrameworkCore;

namespace Assigment.DatabaseContext
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Party> Parties { get; set; } = null!;
        public DbSet<Item> Items { get; set; } = null!;
        public DbSet<ItemParty> ItemParties { get; set; } = null!;
        public DbSet<Proposal> Proposals { get; set; } = null!;
        public DbSet<ProposalResponse> ProposalResponses { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure many-to-many relationship for Item and Party
            modelBuilder.Entity<ItemParty>()
                .HasKey(ip => new { ip.ItemId, ip.PartyId });

            modelBuilder.Entity<ItemParty>()
                .HasOne(ip => ip.Item)
                .WithMany(i => i.ItemParties)
                .HasForeignKey(ip => ip.ItemId);

            modelBuilder.Entity<ItemParty>()
                .HasOne(ip => ip.Party)
                .WithMany(p => p.Items)
                .HasForeignKey(ip => ip.PartyId);

            // Configure Proposal relationships
            modelBuilder.Entity<Proposal>()
                .HasOne(p => p.Item)
                .WithMany(i => i.Proposals)
                .HasForeignKey(p => p.ItemId);

            modelBuilder.Entity<Proposal>()
                .HasOne(p => p.CreatedByUser)
                .WithMany()
                .HasForeignKey(p => p.CreatedByUserId);

            // Configure ProposalResponse relationships
            modelBuilder.Entity<ProposalResponse>()
                .HasOne(pr => pr.Proposal)
                .WithMany(p => p.Responses)
                .HasForeignKey(pr => pr.ProposalId);

            modelBuilder.Entity<ProposalResponse>()
                .HasOne(pr => pr.Party)
                .WithMany()
                .HasForeignKey(pr => pr.PartyId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
