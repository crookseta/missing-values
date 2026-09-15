using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace MissingValues.Internals;

internal static class NumberFormatInfoExtensions
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ReadOnlySpan<TChar> FromString<TChar>(string value)
		where TChar : unmanaged, IUtfCharacter<TChar>
	{
		if (typeof(TChar) == typeof(Utf8Char))
		{
			return Unsafe.BitCast<ReadOnlySpan<byte>, ReadOnlySpan<TChar>>(Encoding.UTF8.GetBytes(value));
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

		internal ReadOnlySpan<TChar> PositiveSignTChar<TChar>()
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			return FromString<TChar>(info.PositiveSign);
		}
		internal ReadOnlySpan<TChar> NegativeSignTChar<TChar>()
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			return FromString<TChar>(info.NegativeSign);
		}
	}
}