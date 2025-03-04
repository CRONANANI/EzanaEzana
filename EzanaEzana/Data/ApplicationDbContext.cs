using Ezana.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Ezana.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<InvestmentPreference> InvestmentPreferences { get; set; } = null!;
        public DbSet<GRPVModel> GRPVModels { get; set; } = null!;
        public DbSet<Friendship> Friendships { get; set; } = null!;
        public DbSet<Message> Messages { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure relationships
            builder.Entity<InvestmentPreference>()
                .HasOne(p => p.User)
                .WithOne(u => u.InvestmentPreference)
                .HasForeignKey<InvestmentPreference>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.Entity<GRPVModel>()
                .HasOne(g => g.User)
                .WithMany()
                .HasForeignKey(g => g.UserId)
                .OnDelete(DeleteBehavior.Cascade);
                
            // Configure complex types
            builder.Entity<GRPVModel>()
                .Property(g => g.GrowthFactors)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<Dictionary<string, decimal>>(v) ?? new Dictionary<string, decimal>());
                    
            builder.Entity<GRPVModel>()
                .Property(g => g.RiskFactors)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<Dictionary<string, decimal>>(v) ?? new Dictionary<string, decimal>());
                    
            builder.Entity<GRPVModel>()
                .Property(g => g.ProfitabilityFactors)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<Dictionary<string, decimal>>(v) ?? new Dictionary<string, decimal>());
                    
            builder.Entity<GRPVModel>()
                .Property(g => g.ValuationFactors)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<Dictionary<string, decimal>>(v) ?? new Dictionary<string, decimal>());
                    
            // Configure Friendship relationships
            builder.Entity<Friendship>()
                .HasOne(f => f.Requester)
                .WithMany(u => u.SentFriendRequests)
                .HasForeignKey(f => f.RequesterId)
                .OnDelete(DeleteBehavior.Restrict);
                
            builder.Entity<Friendship>()
                .HasOne(f => f.Addressee)
                .WithMany(u => u.ReceivedFriendRequests)
                .HasForeignKey(f => f.AddresseeId)
                .OnDelete(DeleteBehavior.Restrict);
                
            // Configure Message relationships
            builder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);
                
            builder.Entity<Message>()
                .HasOne(m => m.Recipient)
                .WithMany(u => u.ReceivedMessages)
                .HasForeignKey(m => m.RecipientId)
                .OnDelete(DeleteBehavior.Restrict);
                
            // Add unique index for PublicUsername
            builder.Entity<ApplicationUser>()
                .HasIndex(u => u.PublicUsername)
                .IsUnique();
        }
    }
} 