using MissingValues.Internals;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues.Internals
{
	internal static partial class NumberFormatter
	{
		private static bool TryFormatNumber<TNumber, TChar>(
			in TNumber value,
			Span<TChar> destination, 
			out int charsWritten, 
			ReadOnlySpan<char> format,
			NumberFormatInfo provider)
			where TNumber : struct, IFormattableNumber<TNumber>
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			if (TNumber.IsBinaryInteger())
			{
				return value switch
				{
					UInt256 n => TryFormatIntegerSlow(in n, destination, out charsWritten, format, provider),
					UInt512 n => TryFormatIntegerSlow(in n, destination, out charsWritten, format, provider),
					Int256 n => TryFormatIntegerSlow(in n, destination, out charsWritten, format, provider),
					Int512 n => TryFormatIntegerSlow(in n, destination, out charsWritten, format, provider),
					_ => throw new FormatException()
				};
				
			}
			else
			{
				double dValue = double.CreateChecked(value);

				if (typeof(TChar) == typeof(Utf16Char))
				{
					return dValue.TryFormat(TChar.CastToCharSpan(destination), out charsWritten, format, provider);
				}
#if NET8_0_OR_GREATER
				else if (typeof(TChar) == typeof(Utf8Char))
				{
					return dValue.TryFormat(TChar.CastToByteSpan(destination), out charsWritten, format, provider);
				}
#endif
				else
				{
					charsWritten = 0;
					return false;
				}
			}
		}

		private static bool TryFormatIntegerSlow<TInteger, TChar>(
			in TInteger value,
			Span<TChar> destination,
			out int charsWritten,
			ReadOnlySpan<char> format,
			NumberFormatInfo provider)
			where TInteger : struct, IFormattableInteger<TInteger>
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			Span<byte> integerBits = stackalloc byte[value.GetByteCount()];
			BigInteger integer = new BigInteger(integerBits, TInteger.IsNegative(value));
			charsWritten = 0;

			if (typeof(TChar) == typeof(Utf16Char))
			{
				return integer.TryFormat(TChar.CastToCharSpan(destination), out charsWritten, format, provider);
			}
#if NET8_0_OR_GREATER
			else if (typeof(TChar) == typeof(Utf8Char))
			{
				return ((IUtf8SpanFormattable)integer).TryFormat(TChar.CastToByteSpan(destination), out charsWritten, format, provider);
			}
#endif
			return false;
		}
	}
}