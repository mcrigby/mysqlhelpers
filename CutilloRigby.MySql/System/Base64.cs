namespace System;

public readonly struct Base64 : IEquatable<Base64>
{
    private readonly string _value;

    public Base64(string value)
    {
        _value = null;
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(value), "value must be specified and have a value.");

        value = value
            .RemoveFileSafeBase64Substitutions()
            .EnsureBase64Length();

        if (!value.IsBase64String())
            throw new ArgumentException("value is not a valid Base64 string.", nameof(value));

        _value = value;
    }
    public Base64(byte[] value)
    {
        _value = null;
        if (value == null || value.Length == 0)
            throw new ArgumentNullException("value must be specified and have a value.", nameof(value));

        _value = Convert.ToBase64String(value);
    }

    public string Value
    {
        get
        {
            if (string.IsNullOrWhiteSpace(_value))
                return null;

            return _value;
        }
    }
    public string FileSafeValue { get => _value?.MakeFileSafeBase64Substitutions(); }
    public byte[] ByteValue
    {
        get
        {
            if (string.IsNullOrWhiteSpace(_value))
                return null;

            return Convert.FromBase64String(_value);
        }
    }

    public override string ToString()
    {
        return _value;
    }

    public static implicit operator string(Base64 base64) => base64.Value;
    public static implicit operator byte[](Base64 base64) => base64.ByteValue;

    public static implicit operator Base64(string value)
    {
        return new Base64(value);
    }
    public static implicit operator Base64(byte[] value)
    {
        return new Base64(value);
    }


    public bool Equals(Base64 other)
    {
        return _value == other._value;
    }

    public override bool Equals(object obj)
    {
        return obj is Base64 value && Equals(value);
    }

    public static bool operator ==(Base64 left, Base64 right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Base64 left, Base64 right)
    {
        return !(left == right);
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }
}
