using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues.Tests.Helpers
{
	internal static class NumberBaseHelper<TSelf>
		where TSelf : INumberBase<TSelf>
	{
		public static TSelf One => TSelf.One;
		public static int Radix => TSelf.Radix;
		public static TSelf Zero => TSelf.Zero;
		public static TSelf Abs(TSelf value) => TSelf.Abs(value);

		public static TSelf CreateChecked<TOther>(TOther value)
			where TOther : INumberBase<TOther> => TSelf.CreateChecked(value);
		public static TSelf CreateSaturating<TOther>(TOther value)
			where TOther : INumberBase<TOther> => TSelf.CreateSaturating(value);
		public static TSelf CreateTruncating<TOther>(TOther value)
			where TOther : INumberBase<TOther> => TSelf.CreateTruncating(value);

		public static bool IsCanonical(TSelf value) => TSelf.IsCanonical(value);
		public static bool IsComplexNumber(TSelf value) => TSelf.IsComplexNumber(value);
		public static bool IsEvenInteger(TSelf value) => TSelf.IsEvenInteger(value);
		public static bool IsFinite(TSelf value) => TSelf.IsFinite(value);
		public static bool IsImaginaryNumber(TSelf value) => TSelf.IsImaginaryNumber(value);
		public static bool IsInfinity(TSelf value) => TSelf.IsInfinity(value);
		public static bool IsInteger(TSelf value) => TSelf.IsInteger(value);
		public static bool IsNaN(TSelf value) => TSelf.IsNaN(value);
		public static bool IsNegative(TSelf value) => TSelf.IsNegative(value);
		public static bool IsNegativeInfinity(TSelf value) => TSelf.IsNegativeInfinity(value);
		public static bool IsNormal(TSelf value) => TSelf.IsNormal(value);
		public static bool IsOddInteger(TSelf value) => TSelf.IsOddInteger(value);
		public static bool IsPositive(TSelf value) => TSelf.IsPositive(value);
		public static bool IsPositiveInfinity(TSelf value) => TSelf.IsPositiveInfinity(value);
		public static bool IsRealNumber(TSelf value) => TSelf.IsRealNumber(value);
		public static bool IsSubnormal(TSelf value) => TSelf.IsSubnormal(value);
		public static bool IsZero(TSelf value) => TSelf.IsZero(value);

		public static TSelf MaxMagnitude(TSelf x, TSelf y) => TSelf.MaxMagnitude(x, y);
		public static TSelf MaxMagnitudeNumber(TSelf x, TSelf y) => TSelf.MaxMagnitudeNumber(x, y);
		public static TSelf MinMagnitude(TSelf x, TSelf y) => TSelf.MinMagnitude(x, y);
		public static TSelf MinMagnitudeNumber(TSelf x, TSelf y) => TSelf.MinMagnitudeNumber(x, y);

		public static TSelf Parse(string s, NumberStyles style, IFormatProvider? provider) => TSelf.Parse(s, style, provider);
		public static TSelf Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider) => TSelf.Parse(s, style, provider);

		public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out TSelf result) => TSelf.TryParse(s, style, provider, out result);
		public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out TSelf result) => TSelf.TryParse(s, style, provider, out result);
	}
}
