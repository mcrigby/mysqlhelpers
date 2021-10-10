using System;

namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion
{
    public sealed class GuidToByteArrayInMySqlOrderConverter : ValueConverter<Guid, byte[]>
    {
        public GuidToByteArrayInMySqlOrderConverter(ConverterMappingHints mappingHints = null)
            : base(
                  g => ToByteArrayInMySQLOrder(g),
                  b => FromByteArrayInMySQLOrder(b),
                  mappingHints)
        {
        }

        public static ValueConverterInfo DefaultInfo { get; }
            = new ValueConverterInfo(typeof(Guid), typeof(byte[]), 
                i => new GuidToByteArrayInMySqlOrderConverter(i.MappingHints));

        public static Guid FromByteArrayInMySQLOrder(byte[] value)
        {
            return new Guid(GuidToByteArrayInMySqlOrderConverter.SwapGuidBytes(value));
        }
        public static byte[] ToByteArrayInMySQLOrder(Guid value)
        {
            return GuidToByteArrayInMySqlOrderConverter.SwapGuidBytes(value.ToByteArray());
        }

        internal static byte[] SwapGuidBytes(byte[] value)
        {
            if (value == null || value.Length != _guidLength)
                throw new ArgumentOutOfRangeException("Value must be 16 bytes long.");

            var result = new byte[_guidLength];
            for (int i = 0; i < _guidLength; i++)
            {
                var j = _guidByteOrder[i];
                result[i] = value[j];
            }

            return result;
        }

        private static readonly int[] _guidByteOrder =
                new[] { 3, 2, 1, 0, 5, 4, 7, 6, 8, 9, 10, 11, 12, 13, 14, 15 };
        private const byte _guidLength = 16;
    }
}
