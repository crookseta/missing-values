using System;
using System.Collections.Generic;
using System.Diagnostics;
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
		abstract static ReadOnlySpan<TSelf> Digits { get; }

		abstract static ReadOnlySpan<TSelf> NumberDecimalSeparator(NumberFormatInfo instance);
		abstract static ReadOnlySpan<TSelf> NumberGroupSeparator(NumberFormatInfo instance);
		abstract static ReadOnlySpan<TSelf> NegativeSign(NumberFormatInfo instance);

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
