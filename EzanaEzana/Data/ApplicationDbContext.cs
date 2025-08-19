using EzanaEzana.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace EzanaEzana.Data
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
        
        // Plaid models
        public DbSet<PlaidItem> PlaidItems { get; set; } = null!;
        public DbSet<PlaidAccount> PlaidAccounts { get; set; } = null!;
        public DbSet<PlaidTransaction> PlaidTransactions { get; set; } = null!;
        public DbSet<PlaidInstitution> PlaidInstitutions { get; set; } = null!;

        // Achievement and Trophy System
        public DbSet<Achievement> Achievements { get; set; } = null!;
        public DbSet<UserAchievement> UserAchievements { get; set; } = null!;

        // Community System
        public DbSet<CommunityThread> CommunityThreads { get; set; } = null!;
        public DbSet<ThreadComment> ThreadComments { get; set; } = null!;
        public DbSet<ThreadLike> ThreadLikes { get; set; } = null!;
        public DbSet<CommentLike> CommentLikes { get; set; } = null!;
        public DbSet<ThreadView> ThreadViews { get; set; } = null!;

        // Portfolio and Investment System
        public DbSet<Portfolio> Portfolios { get; set; } = null!;
        public DbSet<PortfolioHolding> PortfolioHoldings { get; set; } = null!;
        public DbSet<PortfolioTransaction> PortfolioTransactions { get; set; } = null!;
        public DbSet<Watchlist> Watchlists { get; set; } = null!;
        public DbSet<WatchlistItem> WatchlistItems { get; set; } = null!;

        // Market Data and Research System
        public DbSet<MarketData> MarketData { get; set; } = null!;
        public DbSet<MarketDataHistory> MarketDataHistory { get; set; } = null!;
        public DbSet<CompanyProfile> CompanyProfiles { get; set; } = null!;
        public DbSet<CompanyFinancial> CompanyFinancials { get; set; } = null!;
        public DbSet<CompanyNews> CompanyNews { get; set; } = null!;
        public DbSet<EconomicIndicator> EconomicIndicators { get; set; } = null!;
        public DbSet<EconomicIndicatorHistory> EconomicIndicatorHistory { get; set; } = null!;

        // User Activity and Community Management
        public DbSet<UserActivity> UserActivities { get; set; } = null!;
        public DbSet<CommunityMembership> CommunityMemberships { get; set; } = null!;
        public DbSet<UserNotification> UserNotifications { get; set; } = null!;
        public DbSet<UserPreference> UserPreferences { get; set; } = null!;
        public DbSet<UserStatistics> UserStatistics { get; set; } = null!;

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
                
            // Configure Plaid relationships
            builder.Entity<PlaidItem>()
                .HasOne(i => i.User)
                .WithMany()
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.Entity<PlaidAccount>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.Entity<PlaidAccount>()
                .HasOne(a => a.Item)
                .WithMany(i => i.Accounts)
                .HasForeignKey(a => a.PlaidItemId)
                .HasPrincipalKey(i => i.PlaidItemId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.Entity<PlaidTransaction>()
                .HasOne(t => t.Account)
                .WithMany(a => a.Transactions)
                .HasForeignKey(t => t.AccountId)
                .OnDelete(DeleteBehavior.Cascade);
                
            // Add unique constraints for Plaid
            builder.Entity<PlaidItem>()
                .HasIndex(i => i.PlaidItemId)
                .IsUnique();
                
            builder.Entity<PlaidAccount>()
                .HasIndex(a => a.PlaidAccountId)
                .IsUnique();
                
            builder.Entity<PlaidTransaction>()
                .HasIndex(t => t.PlaidTransactionId)
                .IsUnique();
                
            builder.Entity<PlaidInstitution>()
                .HasIndex(i => i.PlaidInstitutionId)
                .IsUnique();
        }
    }
} 