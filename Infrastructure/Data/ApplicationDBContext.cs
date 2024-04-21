using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public partial class ApplicationDBContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
         
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ConnectorDetail>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ChargeCapacity)
                    .HasMaxLength(255)
                    .HasColumnName("chargeCapacity");

                entity.Property(e => e.ConnectorType)
                    .HasMaxLength(255)
                    .HasColumnName("connectorType");

                entity.Property(e => e.CustomerChargeLevel)
                    .HasMaxLength(10)
                    .HasColumnName("customerChargeLevel");

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

            modelBuilder.Entity<PaymentMethod>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.EPaymentAccept).HasColumnName("ePaymentAccept");

                entity.Property(e => e.EPaymentTypes).HasColumnName("ePaymentTypes");

                entity.Property(e => e.EvStationId).HasColumnName("evStationId");

                entity.Property(e => e.OtherPaymentAccept).HasColumnName("otherPaymentAccept");

                entity.Property(e => e.OtherPaymentTypes).HasColumnName("otherPaymentTypes");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
