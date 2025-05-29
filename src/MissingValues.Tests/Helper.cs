using System.Globalization;
using System.Numerics;

namespace MissingValues.Tests;

public static class Helper
{
    public static TSelf Abs<TSelf>(TSelf value)
        where TSelf : INumberBase<TSelf>
    {
        return TSelf.Abs(value);
    }
    public static bool IsCanonical<TSelf>(TSelf value)
        where TSelf : INumberBase<TSelf>
    {
        return TSelf.IsCanonical(value);
    }
    public static bool IsComplexNumber<TSelf>(TSelf value)
        where TSelf : INumberBase<TSelf>
    {
        return TSelf.IsComplexNumber(value);
    }
    public static bool IsEvenInteger<TSelf>(TSelf value)
        where TSelf : INumberBase<TSelf>
    {
        return TSelf.IsEvenInteger(value);
    }
    public static bool IsFinite<TSelf>(TSelf value)
        where TSelf : INumberBase<TSelf>
    {
        return TSelf.IsFinite(value);
    }
    public static bool IsImaginaryNumber<TSelf>(TSelf value)
        where TSelf : INumberBase<TSelf>
    {
        return TSelf.IsImaginaryNumber(value);
    }
    public static bool IsInfinity<TSelf>(TSelf value)
        where TSelf : INumberBase<TSelf>
    {
        return TSelf.IsInfinity(value);
    }
    public static bool IsInteger<TSelf>(TSelf value)
        where TSelf : INumberBase<TSelf>
    {
        return TSelf.IsInteger(value);
    }
    public static bool IsNaN<TSelf>(TSelf value)
        where TSelf : INumberBase<TSelf>
    {
        return TSelf.IsNaN(value);
    }
    public static bool IsNegative<TSelf>(TSelf value)
        where TSelf : INumberBase<TSelf>
    {
        return TSelf.IsNegative(value);
    }
    public static bool IsNegativeInfinity<TSelf>(TSelf value)
        where TSelf : INumberBase<TSelf>
    {
        return TSelf.IsNegativeInfinity(value);
    }
    public static bool IsNormal<TSelf>(TSelf value)
        where TSelf : INumberBase<TSelf>
    {
        return TSelf.IsNormal(value);
    }
    public static bool IsOddInteger<TSelf>(TSelf value)
        where TSelf : INumberBase<TSelf>
    {
        return TSelf.IsOddInteger(value);
    }
    public static bool IsPositive<TSelf>(TSelf value)
        where TSelf : INumberBase<TSelf>
    {
        return TSelf.IsPositive(value);
    }
    public static bool IsPositiveInfinity<TSelf>(TSelf value)
        where TSelf : INumberBase<TSelf>
    {
        return TSelf.IsPositiveInfinity(value);
    }
    public static bool IsRealNumber<TSelf>(TSelf value)
        where TSelf : INumberBase<TSelf>
    {
        return TSelf.IsRealNumber(value);
    }
    public static bool IsSubnormal<TSelf>(TSelf value)
        where TSelf : INumberBase<TSelf>
    {
        return TSelf.IsSubnormal(value);
    }
    public static bool IsZero<TSelf>(TSelf value)
        where TSelf : INumberBase<TSelf>
    {
        return TSelf.IsZero(value);
    }
    public static TSelf MaxMagnitude<TSelf>(TSelf x, TSelf y)
        where TSelf : INumberBase<TSelf>
    {
        return TSelf.MaxMagnitude(x, y);
    }
    public static TSelf MaxMagnitudeNumber<TSelf>(TSelf x, TSelf y)
        where TSelf : INumberBase<TSelf>
    {
        return TSelf.MaxMagnitudeNumber(x, y);
    }
    public static TSelf MinMagnitude<TSelf>(TSelf x, TSelf y)
        where TSelf : INumberBase<TSelf>
    {
        return TSelf.MinMagnitude(x, y);
    }
    public static TSelf MinMagnitudeNumber<TSelf>(TSelf x, TSelf y)
        where TSelf : INumberBase<TSelf>
    {
        return TSelf.MinMagnitudeNumber(x, y);
    }
    public static TSelf MultiplyAddEstimate<TSelf>(TSelf left, TSelf right, TSelf addend)
        where TSelf : INumberBase<TSelf>
    {
        return TSelf.MultiplyAddEstimate(left, right, addend);
    }
    public static TSelf Parse<TSelf>(string s, NumberStyles style, IFormatProvider? provider)
        where TSelf : INumberBase<TSelf>
    {
        return TSelf.Parse(s, style, provider);
    }
    public static TSelf Parse<TSelf>(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
        where TSelf : INumberBase<TSelf>
    {
        return TSelf.Parse(s, style, provider);
    }
    public static TSelf Parse<TSelf>(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider)
        where TSelf : INumberBase<TSelf>
    {
        return TSelf.Parse(utf8Text, style, provider);
    }
    public static bool TryParse<TSelf>(string s, NumberStyles style, IFormatProvider? provider, out TSelf result)
        where TSelf : INumberBase<TSelf>
    {
        return TSelf.TryParse(s, style, provider, out result);
    }
    public static bool TryParse<TSelf>(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out TSelf result)
        where TSelf : INumberBase<TSelf>
    {
        return TSelf.TryParse(s, style, provider, out result);
    }
    public static bool TryParse<TSelf>(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider, out TSelf result)
        where TSelf : INumberBase<TSelf>
    {
        return TSelf.TryParse(utf8Text, style, provider, out result);
    }
    public static TSelf Clamp<TSelf>(TSelf value, TSelf min, TSelf max)
        where TSelf : INumber<TSelf>
    {
        return TSelf.Clamp(value, min, max);
    }
    public static TSelf CopySign<TSelf>(TSelf value, TSelf sign)
        where TSelf : INumber<TSelf>
    {
        return TSelf.CopySign(value, sign);
    }
    public static TSelf Max<TSelf>(TSelf x, TSelf y)
        where TSelf : INumber<TSelf>
    {
        return TSelf.Max(x, y);
    }
    public static TSelf MaxNumber<TSelf>(TSelf x, TSelf y)
        where TSelf : INumber<TSelf>
    {
        return TSelf.MaxNumber(x, y);
    }
    public static TSelf Min<TSelf>(TSelf x, TSelf y)
        where TSelf : INumber<TSelf>
    {
        return TSelf.Min(x, y);
    }
    public static TSelf MinNumber<TSelf>(TSelf x, TSelf y)
        where TSelf : INumber<TSelf>
    {
        return TSelf.MinNumber(x, y);
    }
    public static int Sign<TSelf>(TSelf value)
        where TSelf : INumber<TSelf>
    {
        return TSelf.Sign(value);
    }
    public static bool IsPow2<TSelf>(TSelf value)
        where TSelf : IBinaryNumber<TSelf>
    {
        return TSelf.IsPow2(value);
    }
    public static TSelf Log2<TSelf>(TSelf value)
        where TSelf : IBinaryNumber<TSelf>
    {
        return TSelf.Log2(value);
    }
    public static (TSelf Quotient, TSelf Remainder) DivRem<TSelf>(TSelf left, TSelf right)
        where TSelf : IBinaryInteger<TSelf>
    {
        return TSelf.DivRem(left, right);
    }
    public static TSelf LeadingZeroCount<TSelf>(TSelf value)
        where TSelf : IBinaryInteger<TSelf>
    {
        return TSelf.LeadingZeroCount(value);
    }
    public static TSelf PopCount<TSelf>(TSelf value)
        where TSelf : IBinaryInteger<TSelf>
    {
        return TSelf.PopCount(value);
    }
    public static TSelf ReadBigEndian<TSelf>(ReadOnlySpan<byte> source, bool isUnsigned)
        where TSelf : IBinaryInteger<TSelf>
    {
        return TSelf.ReadBigEndian(source, isUnsigned);
    }
    public static TSelf ReadLittleEndian<TSelf>(ReadOnlySpan<byte> source, bool isUnsigned)
        where TSelf : IBinaryInteger<TSelf>
    {
        return TSelf.ReadLittleEndian(source, isUnsigned);
    }
    public static TSelf RotateLeft<TSelf>(TSelf value, int rotateAmount)
        where TSelf : IBinaryInteger<TSelf>
    {
        return TSelf.RotateLeft(value, rotateAmount);
    }
    public static TSelf RotateRight<TSelf>(TSelf value, int rotateAmount)
        where TSelf : IBinaryInteger<TSelf>
    {
        return TSelf.RotateRight(value, rotateAmount);
    }
    public static TSelf TrailingZeroCount<TSelf>(TSelf value)
        where TSelf : IBinaryInteger<TSelf>
    {
        return TSelf.TrailingZeroCount(value);
    }
    public static int GetByteCount<TSelf>(TSelf value)
        where TSelf : IBinaryInteger<TSelf>
    {
        return value.GetByteCount();
    }
    public static int GetShortestBitLength<TSelf>(TSelf value)
        where TSelf : IBinaryInteger<TSelf>
    {
        return value.GetShortestBitLength();
    }
    public static int WriteBigEndian<TSelf>(TSelf value, Span<byte> destination)
        where TSelf : IBinaryInteger<TSelf>
    {
        return value.WriteBigEndian(destination);
    }
    public static int WriteLittleEndian<TSelf>(TSelf value, Span<byte> destination)
        where TSelf : IBinaryInteger<TSelf>
    {
        return value.WriteLittleEndian(destination);
    }
}