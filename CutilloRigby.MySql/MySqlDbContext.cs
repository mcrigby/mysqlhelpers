using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Microsoft.EntityFrameworkCore;

public abstract class MySqlDbContext : DbContext
{
    public MySqlDbContext() : base() { }
    public MySqlDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreating(modelBuilder, null);
    }

    protected void OnModelCreating(ModelBuilder modelBuilder, Func<IMutableEntityType, string> getTableName)
    {
        base.OnModelCreating(modelBuilder);

        getTableName ??= e => e.GetTableName();

        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            entity.SetTableName(getTableName(entity));

            entity.AddCustomTypedClrProperties(typeof(Base64), ConfigureBase64Property);

            entity.ConfigureEntityPropertiesOfClrType(typeof(Guid), ConfigureGuidProperty);
            entity.ConfigureEntityPropertiesOfClrType(typeof(Guid?), ConfigureGuidNullableProperty);
            entity.ConfigureEntityPropertiesOfClrType(typeof(bool), ConfigureBooleanProperty);
        }
    }

    private static void ConfigureBase64Property(IMutableProperty property)
    {
        property.SetValueConverter(Base64ToByteArrayConverter);
        property.SetColumnType("varbinary(max)");

        var maxLengthAttribute = (MaxLengthAttribute)property.PropertyInfo
            .GetCustomAttributes(typeof(MaxLengthAttribute), false)
            .FirstOrDefault();

        if (maxLengthAttribute == null)
            return;

        var maxLengthValue = maxLengthAttribute.Length.Base64DecrytedLength();
        property.SetAnnotation("MaxLength", maxLengthValue);
        property.SetColumnType($"varbinary({maxLengthValue})");
    }
    private static void ConfigureGuidProperty(IMutableProperty property)
    {
        property.SetValueConverter(GuidToByteArrayInMySqlOrderConverter);
        property.SetColumnType(GuidColumnType);
    }
    private static void ConfigureGuidNullableProperty(IMutableProperty property)
    {
        property.SetValueConverter(GuidNullableToByteArrayInMySqlOrderConverter);
        property.SetColumnType(GuidColumnType);
    }
    private static void ConfigureBooleanProperty(IMutableProperty property)
    {
        property.SetValueConverter(BoolToZeroOneConverter);
    }

    protected const string GuidColumnType = "varbinary(16)";

    internal static readonly GuidToByteArrayInMySqlOrderConverter GuidToByteArrayInMySqlOrderConverter = new GuidToByteArrayInMySqlOrderConverter();
    internal static readonly GuidNullableToByteArrayInMySqlOrderConverter GuidNullableToByteArrayInMySqlOrderConverter = new GuidNullableToByteArrayInMySqlOrderConverter();
    internal static readonly BoolToZeroOneConverter<int> BoolToZeroOneConverter = new BoolToZeroOneConverter<int>();
    internal static readonly Base64ToByteArrayConverter Base64ToByteArrayConverter = new Base64ToByteArrayConverter();
}
