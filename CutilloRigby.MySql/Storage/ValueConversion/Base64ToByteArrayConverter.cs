using System;

namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public sealed class Base64ToByteArrayConverter : ValueConverter<Base64, byte[]>
{
    public Base64ToByteArrayConverter(ConverterMappingHints mappingHints = null)
        : base(b => b.ByteValue, b => new Base64(b), mappingHints)
    {
    }

    public static ValueConverterInfo DefaultInfo { get; }
        = new ValueConverterInfo(typeof(Base64), typeof(byte[]),
            i => new Base64ToByteArrayConverter(i.MappingHints));
}
