using System;

namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion
{
    public sealed class GuidNullableToByteArrayInMySqlOrderConverter : ValueConverter<Guid?, byte[]>
    {
        public GuidNullableToByteArrayInMySqlOrderConverter(ConverterMappingHints mappingHints = null)
            : base(
                  g => ToByteArrayInMySQLOrder(g),
                  b => FromByteArrayInMySQLOrder(b),
                  mappingHints)
        {
        }

        public static ValueConverterInfo DefaultInfo { get; }
            = new ValueConverterInfo(typeof(Guid?), typeof(byte[]), 
                i => new GuidNullableToByteArrayInMySqlOrderConverter(i.MappingHints));

        public static Guid? FromByteArrayInMySQLOrder(byte[] value)
        {
            if (value == null)
                return null;

            return new Guid(GuidToByteArrayInMySqlOrderConverter.SwapGuidBytes(value));
        }
        public static byte[] ToByteArrayInMySQLOrder(Guid? value)
        {
            if (value == null)
                return null;

            return GuidToByteArrayInMySqlOrderConverter.SwapGuidBytes(value.Value.ToByteArray());
        }
    }
}
