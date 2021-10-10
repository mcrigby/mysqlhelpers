using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Microsoft.EntityFrameworkCore
{
    public abstract class MySqlDbContext : DbContext
    {
        public MySqlDbContext() : base() { }
        public MySqlDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreating(modelBuilder, null);
        }

        protected void OnModelCreating(ModelBuilder modelBuilder, Func<IMutableEntityType, string> getTableName = null)
        { 
            base.OnModelCreating(modelBuilder);

            if (getTableName == null) // If getTableName not set, return existing TableName so it doesn't get changed.
                getTableName = e => e.GetTableName();

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.SetTableName(getTableName(entity));

                foreach (var property in entity.GetProperties())
                {
                    if (property.ClrType == typeof(Guid))
                    {
                        property.SetValueConverter(new GuidToByteArrayInMySqlOrderConverter());
                        property.SetColumnType("varbinary(16)");
                    }

                    if (property.ClrType == typeof(Guid?))
                    {
                        property.SetValueConverter(new GuidNullableToByteArrayInMySqlOrderConverter());
                        property.SetColumnType("varbinary(16)");
                    }

                    if (property.ClrType == typeof(bool))
                        property.SetValueConverter(new BoolToZeroOneConverter<int>());
                }
            }
        }
    }
}
