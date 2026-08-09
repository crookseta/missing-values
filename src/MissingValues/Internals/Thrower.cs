using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Text;
using System.Text.Json;

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
		internal enum ArithmeticOperation : byte
		{
			Addition, Subtraction, Multiplication, Division, Exponentiation
		}

		[DoesNotReturn]
		internal static void IntegerOverflow()
		{
			throw new OverflowException();
		}
		[DoesNotReturn]
		internal static void ArithmeticOverflow(ArithmeticOperation operation)
		{
			throw new OverflowException($"{operation} operation ended in overflow.");
		}
		[DoesNotReturn]
		internal static void DivideByZero()
		{
			throw new DivideByZeroException();
		}
		[DoesNotReturn]
		internal static void InvalidNaN<T>(T x)
			where T : IFloatingPointIeee754<T>
		{
			throw new ArithmeticException($"{nameof(x)} cannot be {NumberFormatInfo.CurrentInfo.NaNSymbol}");
		}
		[DoesNotReturn]
		internal static void InvalidFormat(char format)
		{
			throw new FormatException($"The format '{format}' is invalid.");
		}
		[DoesNotReturn]
		internal static void InvalidFormat(string format)
		{
			throw new FormatException($"The format '{format}' is invalid.");
		}
		[DoesNotReturn]
		internal static void InvalidJson<T>()
		{
			throw new FormatException($"Either the JSON value is not in a supported format, or is out of bounds for an {typeof(T)}.");
		}
		[DoesNotReturn]
		internal static void MinimumSignedAbsoluteValue<T>()
			where T : struct, IBinaryInteger<T>, IMinMaxValue<T>
		{
			throw new OverflowException($"Value {T.MinValue} is too large to be represented as a positive value.");
		}
		[DoesNotReturn]
		internal static void MinMaxError<T>(T min, T max)
			where T : struct, INumber<T>
		{
			throw new ArgumentException($"Minimum/Maximum values are invalid.\n{min} is greater than {max}");
		}
		[DoesNotReturn]
		internal static void NeedsNonNegative<T>()
			where T : struct, ISignedNumber<T>
		{
			throw new ArgumentException("Needs non-negative number.");
		}
		[DoesNotReturn]
		internal static void ParsingError<T>(string input, string extraContext = "")
			where T : IParsable<T>
		{
			throw new FormatException($"Could not parse '{input}' as {typeof(T)}.\n" + extraContext);
		}
		[DoesNotReturn]
		internal static void ParsingError<T>(ReadOnlySpan<byte> input, string extraContext = "")
			where T : IParsable<T>
		{
			ParsingError<T>(Encoding.UTF8.GetString(input), extraContext);
		}
		[DoesNotReturn]
		internal static void ParsingError<T>(string input, ParsingErrorType errorType)
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
		internal static void MustBeType<T>()
		{
			throw new NotSupportedException($"Parameter must be of type {typeof(T)}.\n");
		}
		[DoesNotReturn]
		internal static void NotSupported<TTo, TFrom>()
		{
			throw new NotSupportedException($"{typeof(TFrom)} cannot be represented as {typeof(TTo)}.\n");
		}
		[DoesNotReturn]
		internal static void NotSupported<T>()
		{
			throw new NotSupportedException($"{typeof(T)} is not supported.\n");
		}
		[DoesNotReturn]
		internal static void NotSupported()
		{
			throw new NotSupportedException("Operation not supported.\n");
		}
		[DoesNotReturn]
		internal static void ExpectedNumber(JsonTokenType actual)
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
