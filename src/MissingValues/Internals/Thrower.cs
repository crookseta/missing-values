using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MissingValues.Internals
{
	/// <summary>
	/// Helper class for guard clauses.
	/// </summary>
	internal static class Thrower
	{
		internal enum ArithmethicOperation : byte
		{
			Addition, Subtraction, Multiplication, Division
		}

		[DoesNotReturn]
		public static void IntegerOverflow()
		{
			throw new OverflowException();
		}
		[DoesNotReturn]
		public static void ArithmethicOverflow(ArithmethicOperation operation)
		{
			throw new OverflowException($"{operation} operation ended in overflow.");
		}
		[DoesNotReturn]
		public static void InvalidFormat(string format)
		{
			throw new FormatException($"The format '{format}' is invalid.");
		}
		[DoesNotReturn]
		public static void InvalidJson<T>()
		{
			throw new FormatException($"Either the JSON value is not in a supported format, or is out of bounds for an {typeof(T)}.");
		}
		[DoesNotReturn]
		public static void MinimumSignedAbsoluteValue<T>()
			where T : struct, IBinaryInteger<T>, IMinMaxValue<T>
		{
			throw new OverflowException($"Value {T.MinValue} is too large to be represented as a positive value.");
		}
		[DoesNotReturn]
		public static void MinMaxError<T>(T min, T max)
			where T : struct, INumber<T>
		{
			throw new ArgumentException($"Minimum/Maximum values are invalid.\n{min} is greater than {max}");
		}
		[DoesNotReturn]
		public static void NeedsNonNegative<T>()
			where T : struct, ISignedNumber<T>
		{
			throw new ArgumentException("Needs non-negative number.");
		}
		[DoesNotReturn]
		public static void ParsingError<T>(string input, string extraContext = "")
			where T : IParsable<T>
		{
			throw new FormatException($"Could not parse '{input}' as {typeof(T)}.\n" + extraContext);
		}
		[DoesNotReturn]
		public static void MustBeType<T>()
		{
			throw new NotSupportedException($"Parameter must be of type {typeof(T)}.\n");
		}
		[DoesNotReturn]
		public static void NotSupported<TTo, TFrom>()
		{
			throw new NotSupportedException($"{typeof(TFrom)} cannot be represented as {typeof(TTo)}.\n");
		}
		[DoesNotReturn]
		public static void ExpectedNumber(JsonTokenType actual)
		{
			throw new InvalidOperationException($"Expected {JsonTokenType.Number}, got {actual}.");
		}
	}
}
