using System.Buffers.Binary;
using MissingValues.Internals;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace MissingValues.Info;

internal interface INumberFormat
{
	abstract static bool CanRound { get; }
	abstract static bool IsSupported<TNumber>() where TNumber : struct, IFormattableNumber<TNumber>;
	abstract static int GetDefaultDecimalDigits(NumberFormatInfo info);
	abstract static int GetRoundingPosition(ref NumberInfo number, ref int nMaxDigits);
	abstract static void Format<TChar>(ref ValueListBuilder<TChar> vlb, ref NumberInfo number,
	int nMaxDigits, bool isUpper, NumberFormatInfo info) where TChar : unmanaged, IUtfCharacter<TChar>;
}

internal readonly struct CurrencyFormat : INumberFormat
{
	internal static string[] PosCurrencyFormats =>
	[
		"$#", "#$", "$ #", "# $"
	];

	internal static string[] NegCurrencyFormats =>
	[
		"($#)", "-$#", "$-#", "$#-",
		"(#$)", "-#$", "#-$", "#$-",
		"-# $", "-$ #", "# $-", "$ #-",
		"$ -#", "#- $", "($ #)", "(# $)",
		"$- #"
	];

	static bool INumberFormat.CanRound => true;

	static int INumberFormat.GetDefaultDecimalDigits(NumberFormatInfo info)
	{
		return info.CurrencyDecimalDigits;
	}

	static bool INumberFormat.IsSupported<TNumber>()
	{
		return true;
	}

	static void INumberFormat.Format<TChar>(ref ValueListBuilder<TChar> vlb, ref NumberInfo number, int nMaxDigits, bool isUpper, NumberFormatInfo info)
	{
		ReadOnlySpan<char> fmt = number.IsNegative ?
			NegCurrencyFormats[(info.CurrencyNegativePattern)] :
			PosCurrencyFormats[(info.CurrencyPositivePattern)];

		Span<TChar> currencyDecimalSeparator = stackalloc TChar[TChar.GetLength(info.CurrencyDecimalSeparator)];
		Span<TChar> currencyGroupSeparator = stackalloc TChar[TChar.GetLength(info.CurrencyGroupSeparator)];
		Span<TChar> negativeSign = stackalloc TChar[TChar.GetLength(info.NegativeSign)];
		Span<TChar> currencySymbol = stackalloc TChar[TChar.GetLength(info.CurrencySymbol)];

		TChar.Copy(info.CurrencyDecimalSeparator, currencyDecimalSeparator);
		TChar.Copy(info.CurrencyGroupSeparator, currencyGroupSeparator);
		TChar.Copy(info.NegativeSign, negativeSign);
		TChar.Copy(info.CurrencySymbol, currencySymbol);

		foreach (var ch in fmt)
		{
			switch (ch)
			{
				case '#':
					NumberFormatter.FormatGroupedNumeric(ref vlb, ref number, nMaxDigits, info.CurrencyGroupSizes, currencyDecimalSeparator, currencyGroupSeparator);
					break;
				case '-':
					vlb.Append(negativeSign);
					break;
				case '$':
					vlb.Append(currencySymbol);
					break;
				default:
					vlb.Append((TChar)ch);
					break;
			}
		}
	}

	static int INumberFormat.GetRoundingPosition(ref NumberInfo number, ref int nMaxDigits)
	{
		return number.Scale + nMaxDigits;
	}
}
internal readonly struct EngineeringFormat : INumberFormat
{
	static bool INumberFormat.CanRound => true;

	static int INumberFormat.GetDefaultDecimalDigits(NumberFormatInfo info)
	{
		return 6;
	}

	static bool INumberFormat.IsSupported<TNumber>()
	{
		return TNumber.IsBinaryInteger();
	}

	static void INumberFormat.Format<TChar>(ref ValueListBuilder<TChar> vlb, ref NumberInfo number, int nMaxDigits, bool isUpper, NumberFormatInfo info)
	{
		Span<TChar> numberDecimalSeparator = stackalloc TChar[TChar.GetLength(info.NumberDecimalSeparator)];

		TChar.Copy(info.NumberDecimalSeparator, numberDecimalSeparator);

		if (number.IsNegative)
		{
			Span<TChar> negativeSign = stackalloc TChar[TChar.GetLength(info.NegativeSign)];
			TChar.Copy(info.NegativeSign, negativeSign);

			vlb.Append(negativeSign);
		}

		int index = 0;
		Span<byte> digits = number.Digits;

		vlb.Append((TChar)(digits[index] != 0 ? (char)digits[index] : '0'));
		index++;

		if (nMaxDigits != 1)
		{
			vlb.Append(numberDecimalSeparator);
		}

		while (--nMaxDigits > 0)
		{
			vlb.Append((TChar)(digits[index] != 0 ? (char)digits[index] : '0'));
			index++;
		}

		int e = digits[0] == 0 ? 0 : number.Scale - 1;
		NumberFormatter.FormatExponent(ref vlb, info, e, isUpper ? 'E' : 'e', 3, true);
	}

	static int INumberFormat.GetRoundingPosition(ref NumberInfo number, ref int nMaxDigits)
	{
		return ++nMaxDigits;
	}
}
internal readonly struct FixedFormat : INumberFormat
{
	static bool INumberFormat.CanRound => true;

	static int INumberFormat.GetDefaultDecimalDigits(NumberFormatInfo info)
	{
		return info.NumberDecimalDigits;
	}

	static bool INumberFormat.IsSupported<TNumber>()
	{
		return true;
	}

	static void INumberFormat.Format<TChar>(ref ValueListBuilder<TChar> vlb, ref NumberInfo number, int nMaxDigits, bool isUpper, NumberFormatInfo info)
	{
		if (number.IsNegative)
		{
			Span<TChar> negativeSign = stackalloc TChar[TChar.GetLength(info.NegativeSign)];
			TChar.Copy(info.NegativeSign, negativeSign);

			vlb.Append(negativeSign);
		}

		Span<TChar> numberDecimalSeparator = stackalloc TChar[TChar.GetLength(info.NumberDecimalSeparator)];
		Span<TChar> numberGroupSeparator = stackalloc TChar[TChar.GetLength(info.NumberGroupSeparator)];

		TChar.Copy(info.NumberDecimalSeparator, numberDecimalSeparator);
		TChar.Copy(info.NumberGroupSeparator, numberGroupSeparator);

		NumberFormatter.FormatGroupedNumeric(ref vlb, ref number, nMaxDigits, null, numberDecimalSeparator, numberGroupSeparator);
	}

	static int INumberFormat.GetRoundingPosition(ref NumberInfo number, ref int nMaxDigits)
	{
		return number.Scale + nMaxDigits;
	}
}
internal readonly struct NumericFormat : INumberFormat
{
	internal static readonly string[] NegNumberFormats =
	[
		"(#)", "-#", "- #", "#-", "# -",
	];

	static bool INumberFormat.CanRound => true;

	static int INumberFormat.GetDefaultDecimalDigits(NumberFormatInfo info)
	{
		return info.NumberDecimalDigits;
	}

	static bool INumberFormat.IsSupported<TNumber>()
	{
		return true;
	}

	static void INumberFormat.Format<TChar>(ref ValueListBuilder<TChar> vlb, ref NumberInfo number, int nMaxDigits, bool isUpper, NumberFormatInfo info)
	{
		ReadOnlySpan<char> fmt = number.IsNegative ?
			NegNumberFormats[(info.NumberNegativePattern)] :
			['#'];


		Span<TChar> numberDecimalSeparator = stackalloc TChar[TChar.GetLength(info.NumberDecimalSeparator)];
		Span<TChar> numberGroupSeparator = stackalloc TChar[TChar.GetLength(info.NumberGroupSeparator)];
		Span<TChar> negativeSign = stackalloc TChar[TChar.GetLength(info.NegativeSign)];

		TChar.Copy(info.NumberDecimalSeparator, numberDecimalSeparator);
		TChar.Copy(info.NumberGroupSeparator, numberGroupSeparator);
		TChar.Copy(info.NegativeSign, negativeSign);

		foreach (var ch in fmt)
		{
			switch (ch)
			{
				case '#':
					NumberFormatter.FormatGroupedNumeric(ref vlb, ref number, nMaxDigits, info.NumberGroupSizes, numberDecimalSeparator, numberGroupSeparator);
					break;
				case '-':
					vlb.Append(negativeSign);
					break;
				default:
					vlb.Append((TChar)ch);
					break;
			}
		}
	}

	static int INumberFormat.GetRoundingPosition(ref NumberInfo number, ref int nMaxDigits)
	{
		return number.Scale + nMaxDigits;
	}
}

internal interface IIntegerRadixConverter<TInteger>
	where TInteger : struct, IFormattableInteger<TInteger>
{
	static abstract NumberStyles AllowedStyles { get; }
	static abstract bool IsValidChar<TChar>(TChar ch) where TChar : unmanaged, IUtfCharacter<TChar>;
	static abstract TInteger FromChar<TChar>(TChar ch) where TChar : unmanaged, IUtfCharacter<TChar>;
	static abstract uint MaxDigitValue { get; }
	static abstract int MaxDigitCount { get; }
	static abstract int MaxUInt64DigitCount { get; }
	static abstract int BitsPerCharacter { get; }
	static abstract bool TryParse16Chars(Vector128<byte> chunk, ref TInteger value);
	static abstract bool TryParse32Chars(Vector256<byte> chunk, ref TInteger value);
	static abstract bool TryParse64Chars(Vector512<byte> chunk, ref TInteger value);
}
internal readonly struct HexConverter<TInteger> : IIntegerRadixConverter<TInteger>
	where TInteger : struct, IFormattableInteger<TInteger>
{
	static NumberStyles IIntegerRadixConverter<TInteger>.AllowedStyles => NumberStyles.HexNumber;
	static bool IIntegerRadixConverter<TInteger>.IsValidChar<TChar>(TChar ch) => TChar.IsHexDigit(ch);

	static TInteger IIntegerRadixConverter<TInteger>.FromChar<TChar>(TChar ch) => TInteger.GetHexValue((char)ch);

	static uint IIntegerRadixConverter<TInteger>.MaxDigitValue => 0xF;

	static int IIntegerRadixConverter<TInteger>.MaxDigitCount => TInteger.MaxHexDigits;

	static int IIntegerRadixConverter<TInteger>.MaxUInt64DigitCount => 16;

	static int IIntegerRadixConverter<TInteger>.BitsPerCharacter => 4;

	static bool IIntegerRadixConverter<TInteger>.TryParse16Chars(Vector128<byte> chunk, ref TInteger value)
	{
		// Fast vector validation and ASCII -> nibble conversion:
	    // Nibbles: '0'-'9' (0x30-0x39) -> 0-9
	    //          'A'-'F' (0x41-0x46) -> 10-15
	    //          'a'-'f' (0x61-0x66) -> 10-15
	    
	    var lowerChunk = chunk | Vector128.Create((byte)0x20);
	    
	    var isDigit = Vector128.GreaterThan(chunk, Vector128.Create((byte)('0' - 1))) &
	                  Vector128.LessThan(chunk, Vector128.Create((byte)('9' + 1)));
	                  
	    var isAlpha = Vector128.GreaterThan(lowerChunk, Vector128.Create((byte)('a' - 1))) &
	                  Vector128.LessThan(lowerChunk, Vector128.Create((byte)('f' + 1)));

	    if (!Vector128.AllWhereAllBitsSet(isDigit | isAlpha))
	    {
	        return false;
	    }

	    var digitOffset = Vector128.Create((byte)'0');
	    var alphaOffset = Vector128.Create((byte)('a' - 10));
	    var offset = Vector128.ConditionalSelect(isDigit, digitOffset, alphaOffset);
	    
	    var nibbles = (lowerChunk - offset);

	    var mult = Vector128.Create((sbyte)16, 1, 16, 1, 16, 1, 16, 1, 16, 1, 16, 1, 16, 1, 16, 1);
	    var bytes16 = Ssse3.MultiplyAddAdjacent(nibbles, mult); 

	    var packed = Sse2.PackUnsignedSaturate(bytes16, bytes16);

	    ulong scalar = packed.AsUInt64().ToScalar();
	    
	    value <<= 64;
	    value |= TInteger.CreateTruncating(BinaryPrimitives.ReverseEndianness(scalar));
	    return true;
	}

	static bool IIntegerRadixConverter<TInteger>.TryParse32Chars(Vector256<byte> chunk, ref TInteger value)
	{
		var lowerChunk = chunk | Vector256.Create((byte)0x20);
	    
		var isDigit = Vector256.GreaterThan(chunk, Vector256.Create((byte)('0' - 1))) &
		              Vector256.LessThan(chunk, Vector256.Create((byte)('9' + 1)));
	                  
		var isAlpha = Vector256.GreaterThan(lowerChunk, Vector256.Create((byte)('a' - 1))) &
		              Vector256.LessThan(lowerChunk, Vector256.Create((byte)('f' + 1)));

		if (!Vector256.AllWhereAllBitsSet(isDigit | isAlpha))
		{
			return false;
		}

		var digitOffset = Vector256.Create((byte)'0');
		var alphaOffset = Vector256.Create((byte)('a' - 10));
		var offset = Vector256.ConditionalSelect(isDigit, digitOffset, alphaOffset);
	    
		var nibbles = (lowerChunk - offset);

		var mult = Vector256.Create(
			(sbyte)16, 1, 16, 1, 16, 1, 16, 1, 16, 1, 16, 1, 16, 1, 16, 1,
			16, 1, 16, 1, 16, 1, 16, 1, 16, 1, 16, 1, 16, 1, 16, 1
			);
		var bytes16 = Avx2.MultiplyAddAdjacent(nibbles, mult); 

		var packed = Avx2.PackUnsignedSaturate(bytes16, bytes16);
		Vector256<ulong> result64 = packed.AsUInt64();
		
		value <<= 128;
		value |= TInteger.CreateTruncating(BinaryPrimitives.ReverseEndianness(result64.GetElement(0))) << 64;
		value |= TInteger.CreateTruncating(BinaryPrimitives.ReverseEndianness(result64.GetElement(2)));
		
		return true;
	}

	static bool IIntegerRadixConverter<TInteger>.TryParse64Chars(Vector512<byte> chunk, ref TInteger value)
	{
		var lowerChunk = chunk | Vector512.Create((byte)0x20);
	    
		var isDigit = Vector512.GreaterThan(chunk, Vector512.Create((byte)('0' - 1))) &
		              Vector512.LessThan(chunk, Vector512.Create((byte)('9' + 1)));
	                  
		var isAlpha = Vector512.GreaterThan(lowerChunk, Vector512.Create((byte)('a' - 1))) &
		              Vector512.LessThan(lowerChunk, Vector512.Create((byte)('f' + 1)));

		if (!Vector512.AllWhereAllBitsSet(isDigit | isAlpha))
		{
			return false;
		}

		var digitOffset = Vector512.Create((byte)'0');
		var alphaOffset = Vector512.Create((byte)('a' - 10));
		var offset = Vector512.ConditionalSelect(isDigit, digitOffset, alphaOffset);
	    
		var nibbles = (lowerChunk - offset);

		var mult = Vector512.Create(
			(sbyte)16, 1, 16, 1, 16, 1, 16, 1, 16, 1, 16, 1, 16, 1, 16, 1,
			16, 1, 16, 1, 16, 1, 16, 1, 16, 1, 16, 1, 16, 1, 16, 1,
			16, 1, 16, 1, 16, 1, 16, 1, 16, 1, 16, 1, 16, 1, 16, 1,
			16, 1, 16, 1, 16, 1, 16, 1, 16, 1, 16, 1, 16, 1, 16, 1
		);
		var bytes16 = Avx512BW.MultiplyAddAdjacent(nibbles, mult); 

		var packed = Avx512BW.PackUnsignedSaturate(bytes16, bytes16);
		Vector512<ulong> result64 = packed.AsUInt64();
		
		value <<= 256;
		value |= TInteger.CreateTruncating(BinaryPrimitives.ReverseEndianness(result64.GetElement(0))) << 192;
		value |= TInteger.CreateTruncating(BinaryPrimitives.ReverseEndianness(result64.GetElement(2))) << 128;
		value |= TInteger.CreateTruncating(BinaryPrimitives.ReverseEndianness(result64.GetElement(4))) << 64;
		value |= TInteger.CreateTruncating(BinaryPrimitives.ReverseEndianness(result64.GetElement(6)));
		
		return true;
	}
}
internal readonly struct BinConverter<TInteger> : IIntegerRadixConverter<TInteger>
	where TInteger : struct, IFormattableInteger<TInteger>
{
	static NumberStyles IIntegerRadixConverter<TInteger>.AllowedStyles => NumberStyles.BinaryNumber;
	static bool IIntegerRadixConverter<TInteger>.IsValidChar<TChar>(TChar ch) => ch == (TChar)'1' || ch == (TChar)'0';

	static TInteger IIntegerRadixConverter<TInteger>.FromChar<TChar>(TChar ch) => TInteger.GetDecimalValue((char)ch);

	static uint IIntegerRadixConverter<TInteger>.MaxDigitValue => 0b1;

	static int IIntegerRadixConverter<TInteger>.MaxDigitCount => TInteger.MaxBinaryDigits;

	static int IIntegerRadixConverter<TInteger>.MaxUInt64DigitCount => 64;

	static int IIntegerRadixConverter<TInteger>.BitsPerCharacter => 1;

	static bool IIntegerRadixConverter<TInteger>.TryParse16Chars(Vector128<byte> chunk, ref TInteger value)
	{
		var zeroes = Vector128.Create((byte)'0');

		if (Vector128.GreaterThanAny(chunk, Vector128.Create((byte)'1')) ||
		    Vector128.LessThanAny(chunk, zeroes))
		{
			return false;
		}

		var shifted = Vector128.ShiftLeft(chunk.AsInt16(), 7).AsByte();
		uint bitMask = shifted.ExtractMostSignificantBits();

		value <<= 16;
		value |= TInteger.CreateTruncating((ushort)(uint.ReverseBits(bitMask) >> 16));
		return true;
	}

	static bool IIntegerRadixConverter<TInteger>.TryParse32Chars(Vector256<byte> chunk, ref TInteger value)
	{
		var zeroes = Vector256.Create((byte)'0');

		if (Vector256.GreaterThanAny(chunk, Vector256.Create((byte)'1')) ||
		    Vector256.LessThanAny(chunk, zeroes))
		{
			return false;
		}

		var shifted = Vector256.ShiftLeft(chunk.AsInt16(), 7).AsByte();
		uint bitMask = shifted.ExtractMostSignificantBits();

		value <<= 32;
		value |= TInteger.CreateTruncating(uint.ReverseBits(bitMask));
		
		return true;
	}

	static bool IIntegerRadixConverter<TInteger>.TryParse64Chars(Vector512<byte> chunk, ref TInteger value)
	{
		var zeroes = Vector512.Create((byte)'0');

		if (Vector512.GreaterThanAny(chunk, Vector512.Create((byte)'1')) ||
		    Vector512.LessThanAny(chunk, zeroes))
		{
			return false;
		}

		var shifted = Vector512.ShiftLeft(chunk.AsInt16(), 7).AsByte();
		ulong bitMask = shifted.ExtractMostSignificantBits();
		
		value <<= 64;
		value |= TInteger.CreateTruncating(ulong.ReverseBits(bitMask));
		
		return true;
	}
}