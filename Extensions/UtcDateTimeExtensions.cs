namespace electro_shop_backend.Extensions
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
    using System;
    using System.Linq;

    public static class UtcDateTimeExtensions
    {
        public static void UseUtcDateTimeWithProviderAdjustment(this ModelBuilder modelBuilder, string providerName)
        {
            if (!providerName.Contains("Npgsql"))
                return;
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var dateProps = entityType.ClrType.GetProperties()
                    .Where(p => p.PropertyType == typeof(DateTime) || p.PropertyType == typeof(DateTime?));

                foreach (var prop in dateProps)
                {
                    
                    if (prop.PropertyType == typeof(DateTime))
                    {
                        var builder = modelBuilder.Entity(entityType.Name)
                            .Property(prop.Name)
                            .HasConversion(GetConverterForProviderNonNullable(providerName))
                            .HasColumnType("timestamp with time zone");
                    }
                    else // DateTime?
                    {
                        var builder = modelBuilder.Entity(entityType.Name)
                            .Property(prop.Name)
                            .HasConversion(GetConverterForProviderNullable(providerName))
                            .HasColumnType("timestamp with time zone");
                    }
                }
            }
        }

        private static ValueConverter<DateTime, DateTime> GetConverterForProviderNonNullable(string providerName)
        {
            return new ValueConverter<DateTime, DateTime>(
                v => v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime(),
                v => TimeZoneInfo.ConvertTimeBySystemTimeZoneId(v, "SE Asia Standard Time")
            );
        }

        private static ValueConverter<DateTime?, DateTime?> GetConverterForProviderNullable(string providerName)
        {
            return new ValueConverter<DateTime?, DateTime?>(
                v => v.HasValue ? (v.Value.Kind == DateTimeKind.Utc ? v.Value : v.Value.ToUniversalTime()) : v,
                v => v.HasValue ? TimeZoneInfo.ConvertTimeBySystemTimeZoneId(v.Value, "SE Asia Standard Time") : v
            );
        }
    }

}
