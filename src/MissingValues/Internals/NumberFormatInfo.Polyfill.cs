using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace MissingValues.Internals;

internal static class NumberFormatInfoExtensions
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ReadOnlySpan<TChar> FromString<TChar>(string value, Span<TChar> storage)
		where TChar : unmanaged, IUtfCharacter<TChar>
	{
		if (typeof(TChar) == typeof(Utf8Char))
		{
			if (!Encoding.UTF8.TryGetBytes(value, TChar.CastToByteSpan(storage), out int written))
			{
				return Unsafe.BitCast<ReadOnlySpan<byte>, ReadOnlySpan<TChar>>(Encoding.UTF8.GetBytes(value));
			}
			return storage[..written];
		}

		Debug.Assert(typeof(TChar) == typeof(Utf16Char));
		return Unsafe.BitCast<ReadOnlySpan<char>, ReadOnlySpan<TChar>>(value.AsSpan());
	}
	
	extension(NumberFormatInfo info)
	{
		internal bool AllowHyphenDuringParsing()
		{
			string negativeSign = info.NegativeSign;
			return negativeSign.Length == 1 &&
			       negativeSign[0] switch
			       {
				       '\u2012' or         // Figure Dash
				       '\u207B' or         // Superscript Minus
				       '\u208B' or         // Subscript Minus
				       '\u2212' or         // Minus Sign
				       '\u2796' or         // Heavy Minus Sign
				       '\uFE63' or         // Small Hyphen-Minus
				       '\uFF0D' => true,   // Fullwidth Hyphen-Minus
				       _ => false
			       };
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal ReadOnlySpan<TChar> PositiveSignTChar<TChar>(Span<TChar> storage)
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			return FromString(info.PositiveSign, storage);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal ReadOnlySpan<TChar> NegativeSignTChar<TChar>(Span<TChar> storage)
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			return FromString(info.NegativeSign, storage);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal ReadOnlySpan<TChar> PositiveInfinitySymbolTChar<TChar>(Span<TChar> storage)
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			return FromString(info.PositiveInfinitySymbol, storage);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal ReadOnlySpan<TChar> NegativeInfinitySymbolTChar<TChar>(Span<TChar> storage)
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			return FromString(info.NegativeInfinitySymbol, storage);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal ReadOnlySpan<TChar> NaNSymbolTChar<TChar>(Span<TChar> storage)
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			return FromString(info.NaNSymbol, storage);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal ReadOnlySpan<TChar> NumberDecimalSeparatorTChar<TChar>(Span<TChar> storage)
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			return FromString(info.NumberDecimalSeparator, storage);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal ReadOnlySpan<TChar> NumberGroupSeparatorTChar<TChar>(Span<TChar> storage)
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			return FromString(info.NumberGroupSeparator, storage);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal ReadOnlySpan<TChar> CurrencyDecimalSeparatorTChar<TChar>(Span<TChar> storage)
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			return FromString(info.CurrencyDecimalSeparator, storage);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal ReadOnlySpan<TChar> CurrencyGroupSeparatorTChar<TChar>(Span<TChar> storage)
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			return FromString(info.CurrencyGroupSeparator, storage);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal ReadOnlySpan<TChar> CurrencySymbolTChar<TChar>(Span<TChar> storage)
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			return FromString(info.CurrencySymbol, storage);
		}
	}
}