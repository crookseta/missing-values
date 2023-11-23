using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues.Internals
{
	internal interface IUtfCharacter<TSelf> :
		IEquatable<TSelf>,
		IEqualityOperators<TSelf, TSelf, bool>
		where TSelf : unmanaged, IUtfCharacter<TSelf>?
	{
		abstract static TSelf NullCharacter { get; }
		abstract static ReadOnlySpan<TSelf> Digits { get; }

		abstract static bool TryParseFormat(ReadOnlySpan<TSelf> s, IFormatProvider? formatProvider, out char format, out int precision);

		abstract static ReadOnlySpan<char> CastToCharSpan(ReadOnlySpan<TSelf> chars);
		abstract static ReadOnlySpan<byte> CastToByteSpan(ReadOnlySpan<TSelf> chars);

		abstract static ReadOnlySpan<TSelf> CastFromCharSpan(ReadOnlySpan<char> chars);
		abstract static ReadOnlySpan<TSelf> CastFromByteSpan(ReadOnlySpan<byte> chars);
		virtual static ReadOnlySpan<TSelf> NumberDecimalSeparator(NumberFormatInfo instance)
		{
			return TSelf.CastFromCharSpan(instance.NumberDecimalSeparator);
		}
		virtual static ReadOnlySpan<TSelf> NumberGroupSeparator(NumberFormatInfo instance)
		{
			return TSelf.CastFromCharSpan(instance.NumberGroupSeparator);
		}
		virtual static ReadOnlySpan<TSelf> NegativeSign(NumberFormatInfo instance)
		{
			return TSelf.CastFromCharSpan(instance.NegativeSign);
		}
		virtual static ReadOnlySpan<TSelf> NegativeInfinitySymbol(NumberFormatInfo instance)
		{
			return TSelf.CastFromCharSpan(instance.NegativeInfinitySymbol);
		}
		virtual static ReadOnlySpan<TSelf> NaNSymbol(NumberFormatInfo instance)
		{
			return TSelf.CastFromCharSpan(instance.NaNSymbol);
		}
		virtual static ReadOnlySpan<TSelf> PositiveSign(NumberFormatInfo instance)
		{
			return TSelf.CastFromCharSpan(instance.PositiveSign);
		}
		virtual static ReadOnlySpan<TSelf> PositiveInfinitySymbol(NumberFormatInfo instance)
		{
			return TSelf.CastFromCharSpan(instance.PositiveInfinitySymbol);
		}

		abstract static TSelf ToUpper(TSelf value);
		abstract static TSelf ToLower(TSelf value);
		abstract static bool IsWhiteSpace(TSelf value);

		abstract static bool Constains(ReadOnlySpan<TSelf> v1, ReadOnlySpan<TSelf> v2, StringComparison comparisonType);
		abstract static bool EndsWith(ReadOnlySpan<TSelf> v1, ReadOnlySpan<TSelf> v2, StringComparison comparisonType);
		abstract static bool StartsWith(ReadOnlySpan<TSelf> v1, ReadOnlySpan<TSelf> v2, StringComparison comparisonType);
		abstract static bool Equals(ReadOnlySpan<TSelf> v1, ReadOnlySpan<TSelf> v2, StringComparison comparisonType);

		abstract static explicit operator TSelf(char value);  
		abstract static explicit operator TSelf(byte value);  
	}
}
