using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
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
		[Flags]
		internal enum ParsingErrorType : byte
		{
			None = 0,
			InvalidTrailingWhiteSpace = 1,
			InvalidLeadingWhiteSpace = 2,
			InvalidCharacter = 4,
			InvalidHex = 8,
			InvalidBin = 16,
			StringTooBig = 32,
			ValueTooBig = 64,
			NotSupported = 128,
			InvalidWhiteSpace = InvalidTrailingWhiteSpace | InvalidLeadingWhiteSpace
		}
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
		public static void DivideByZero()
		{
			throw new DivideByZeroException();
		}
		[DoesNotReturn]
		public static void InvalidNaN()
		{
			throw new ArithmeticException($"{nameof(x)} cannot be {NumberFormatInfo.CurrentInfo.NaNSymbol}");
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
		public static void ParsingError<T>(string input, ParsingErrorType errorType)
			where T : IParsable<T>
		{
			StringBuilder extraContext = new StringBuilder();

			if (errorType.HasFlag(ParsingErrorType.InvalidWhiteSpace))
			{
				extraContext.AppendLine("String cannot contain whitespaces.");
			}
			else if (errorType.HasFlag(ParsingErrorType.InvalidTrailingWhiteSpace))
			{
				extraContext.AppendLine("String cannot contain trailing whitespaces.");
			}
			else if (errorType.HasFlag(ParsingErrorType.InvalidLeadingWhiteSpace))
			{
				extraContext.AppendLine("String cannot contain leading whitespaces.");
			}

			if (errorType.HasFlag(ParsingErrorType.InvalidCharacter))
			{
				extraContext.AppendLine("Invalid character found.");
			}

			if (errorType.HasFlag(ParsingErrorType.InvalidHex))
			{
				extraContext.AppendLine($"Hex character found, use {NumberStyles.AllowHexSpecifier} to parse hex values.");
			}

			if (errorType.HasFlag(ParsingErrorType.StringTooBig))
			{
				extraContext.AppendLine("String contains more characters than can be represented.");
			}

			if (errorType.HasFlag(ParsingErrorType.ValueTooBig))
			{
				extraContext.AppendLine("Value represented is too big.");
			}

			if (errorType.HasFlag(ParsingErrorType.NotSupported))
			{
				extraContext.AppendLine("Style not supported.");
			}

			throw new FormatException($"Could not parse '{input}' as {typeof(T)}.\n" + extraContext.ToString());
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
		public static void NotSupported()
		{
			throw new NotSupportedException($"Operation not supported.\n");
		}
		[DoesNotReturn]
		public static void ExpectedNumber(JsonTokenType actual)
		{
			throw new InvalidOperationException($"Expected {JsonTokenType.Number}, got {actual}.");
		}
		[DoesNotReturn]
		internal static void OutOfRange(string paramName)
		{
			throw new ArgumentOutOfRangeException(paramName);
		}
	}
}
