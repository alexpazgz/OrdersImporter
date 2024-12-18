using Businnes.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        #region Ctor
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        #endregion

        #region DbSet
        public DbSet<OnlineOrder> OnlineOrder { get; set; }
        public DbSet<Link> Link { get; set; }

        #endregion

        #region methods
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OnlineOrder>(entity =>
            {
                entity.ToTable("OnlineOrder");
                entity.HasIndex(x => x.OrderId);

                entity.Property(e => e.OrderId).UseIdentityColumn(1L, 1);
                entity.HasKey(e => e.OrderId);
                entity.Property(e => e.OrderId).HasColumnName("OrderId").ValueGeneratedOnAdd().HasColumnType("bigint");

                entity.Property(e => e.Uuid).HasColumnName("Uuid").HasMaxLength(50).IsUnicode(false).IsRequired();
                entity.Property(e => e.Uuid).HasColumnName("Uuid").HasMaxLength(50).IsUnicode(false).IsRequired();
                entity.Property(e => e.Id).HasColumnName("Id").HasMaxLength(20).IsUnicode(false).IsRequired();
                entity.Property(e => e.Region).HasColumnName("Region").HasMaxLength(50).IsUnicode(false).IsRequired();
                entity.Property(e => e.Country).HasColumnName("Country").HasMaxLength(50).IsUnicode(false).IsRequired();
                entity.Property(e => e.ItemType).HasColumnName("ItemType").HasMaxLength(50).IsUnicode(false).IsRequired();
                entity.Property(e => e.SalesChannel).HasColumnName("SalesChannel").HasMaxLength(50).IsUnicode(false).IsRequired();
                entity.Property(e => e.Priority).HasColumnName("Priority").HasMaxLength(1).IsUnicode(false).IsRequired();
                entity.Property(e => e.Date).HasColumnName("Date").HasMaxLength(10).IsUnicode(false).IsRequired();
                entity.Property(e => e.ShipDate).HasColumnName("ShipDate").HasMaxLength(10).IsUnicode(false);
                entity.Property(e => e.UnitsSold).HasColumnName("UnitsSold").HasColumnType("float").IsRequired();
                entity.Property(e => e.UnitPrice).HasColumnName("UnitPrice").HasColumnType("decimal").HasPrecision(18, 2).IsRequired();
                entity.Property(e => e.UnitCost).HasColumnName("UnitCost").HasColumnType("decimal").HasPrecision(18, 2).IsRequired();
                entity.Property(e => e.TotalRevenue).HasColumnName("TotalRevenue").HasColumnType("decimal").HasPrecision(18, 2).IsRequired();
                entity.Property(e => e.TotalCost).HasColumnName("TotalCost").HasColumnType("decimal").HasPrecision(18, 2).IsRequired();
                entity.Property(e => e.TotalProfit).HasColumnName("TotalProfit").HasColumnType("decimal").HasPrecision(18, 2).IsRequired();

                entity.Navigation(e => e.Link);
            });

            modelBuilder.Entity<Link>(entity =>
            {
                entity.ToTable("Link");
                entity.HasIndex(x => x.OrderId);

                entity.HasKey(e => e.OrderId);
                entity.Property(e => e.OrderId).HasColumnName("OrderId").HasColumnType("bigint");
                entity.Property(e => e.Self).HasColumnName("Self").HasMaxLength(100).IsUnicode(false).IsRequired();


                entity.HasOne(s => s.OnlineOrder)
                    .WithOne(ad => ad.Link)
                    .HasForeignKey<Link>(s => s.OrderId);

                entity.Navigation(e => e.OnlineOrder);
            });
        }
    }
}
