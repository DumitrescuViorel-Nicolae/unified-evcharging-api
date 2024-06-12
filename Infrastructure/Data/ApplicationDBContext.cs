using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public partial class ApplicationDBContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }
        public DbSet<EVStation> EVstations { get; set; }
        public DbSet<RegisteredCompany> RegisteredCompanies { get; set; }
        public DbSet<PaymentMethod> PaymentMethod { get; set; }
        public DbSet<ConnectorDetail> ConnectorDetail { get; set; }
        public DbSet<ConnectorStatus> ConnectorStatus { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
         
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasOne(rt => rt.User)
                       .WithMany(u => u.RefreshTokens)
                       .HasForeignKey(rt => rt.UserId)
                       .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ConnectorDetail>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.HasMany(e => e.ConnectorStatuses).WithOne(e => e.ConnectorDetails);

                entity.Property(e => e.ChargeCapacity)
                    .HasMaxLength(255)
                    .HasColumnName("chargeCapacity");

                entity.Property(e => e.ConnectorType)
                    .HasMaxLength(255)
                    .HasColumnName("connectorType");

                entity.Property(e => e.CustomerChargeLevel)
                    .HasMaxLength(10)
                    .HasColumnName("customerChargeLevel");

                entity.Property(e => e.Price)
                    .HasMaxLength(10)
                    .HasColumnName("price");

                entity.Property(e => e.CustomerConnectorName)
                    .HasMaxLength(255)
                    .HasColumnName("customerConnectorName");

                entity.Property(e => e.EvStationId).HasColumnName("evStationId");

                entity.Property(e => e.MaxPowerLevel).HasColumnName("maxPowerLevel");

                entity.Property(e => e.Pay).HasColumnName("pay");

                entity.Property(e => e.SupplierName)
                    .HasMaxLength(255)
                    .HasColumnName("supplierName");
            });
          
        modelBuilder.Entity<ConnectorStatus>(entity =>
            {
                entity.ToTable("ConnectorStatus");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ConnectorDetailsId).HasColumnName("connectorDetailsId");

                entity.Property(e => e.PhysicalReference)
                    .HasMaxLength(10)
                    .HasColumnName("physicalReference");

                entity.Property(e => e.State)
                    .HasMaxLength(50)
                    .HasColumnName("state");
            });

            modelBuilder.Entity<EVStation>(entity =>
            {
                entity.ToTable("EVStations");
                entity.HasOne(e => e.PaymentMethod).WithOne(e => e.EVStation);
                entity.HasMany(e => e.ConnectorDetail).WithOne(e => e.EVStation);

                entity.HasOne(e => e.Company)
                      .WithMany(c => c.EVStations)
                      .HasForeignKey(e => e.CompanyId);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Brand)
                    .HasMaxLength(255)
                    .HasColumnName("brand");

                entity.Property(e => e.City)
                    .HasMaxLength(255)
                    .HasColumnName("city");

                entity.Property(e => e.Country)
                    .HasMaxLength(255)
                    .HasColumnName("country");

                entity.Property(e => e.Latitude)
                    .HasColumnType("decimal(9, 6)")
                    .HasColumnName("latitude");

                entity.Property(e => e.Longitude)
                    .HasColumnType("decimal(9, 6)")
                    .HasColumnName("longitude");

                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .HasColumnName("phone");

                entity.Property(e => e.Street)
                    .HasMaxLength(255)
                    .HasColumnName("street");

                entity.Property(e => e.TotalNumberOfConnectors).HasColumnName("totalNumberOfConnectors");

                entity.Property(e => e.Website)
                    .HasMaxLength(255)
                    .HasColumnName("website");
            });

            modelBuilder.Entity<RegisteredCompany>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.UserId)
                      .HasColumnName("UserId")
                      .IsRequired();

                entity.Property(e => e.CompanyName)
                      .HasMaxLength(255)
                      .HasColumnName("CompanyName");

                entity.Property(e => e.StripeAccountID)
                      .HasMaxLength(255)
                      .HasColumnName("StripeAccountID");

                entity.Property(e => e.RegistrationNumber)
                      .HasMaxLength(255)
                      .HasColumnName("RegistrationNumber");

                entity.Property(e => e.TaxNumber)
                      .HasMaxLength(255)
                      .HasColumnName("TaxNumber");

                entity.Property(e => e.Country)
                      .HasMaxLength(255)
                      .HasColumnName("Country");

                entity.Property(e => e.City)
                      .HasMaxLength(255)
                      .HasColumnName("City");

                entity.Property(e => e.StreetName)
                      .HasMaxLength(255)
                      .HasColumnName("StreetName");

                entity.Property(e => e.ZipCode)
                      .HasMaxLength(10)
                      .HasColumnName("ZipCode");
            });

            modelBuilder.Entity<PaymentMethod>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.EPaymentAccept).HasColumnName("ePaymentAccept");

                entity.Property(e => e.EPaymentTypes).HasColumnName("ePaymentTypes");

                entity.Property(e => e.EvStationId).HasColumnName("evStationId");

                entity.Property(e => e.OtherPaymentAccept).HasColumnName("otherPaymentAccept");

                entity.Property(e => e.OtherPaymentTypes).HasColumnName("otherPaymentTypes");
            });

            modelBuilder.Entity<PaymentTransaction>(entity =>
            {
                entity.ToTable("PaymentTransactions");

                entity.HasKey(e => e.TransactionId).HasName("transaction_id");

                entity.Property(e => e.TransactionId)
                    .HasColumnName("transaction_id")
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(e => e.Amount)
                    .HasColumnName("amount")
                    .HasColumnType("DECIMAL(10, 2)")
                    .IsRequired();

                entity.Property(e => e.Currency)
                    .HasColumnName("currency")
                    .HasMaxLength(3)
                    .IsRequired();

                entity.Property(e => e.PaymentMethodBrand)
                    .HasColumnName("payment_method_brand")
                    .HasMaxLength(20);

                entity.Property(e => e.PaymentMethodLast4)
                    .HasColumnName("payment_method_last4")
                    .HasMaxLength(4);

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(e => e.ReceiptUrl)
                    .HasColumnName("receipt_url")
                    .HasMaxLength(255);

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("datetime");
            });


            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
