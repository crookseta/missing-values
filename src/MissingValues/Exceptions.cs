using System;
using System.Runtime.Serialization;

namespace MissingValues
{
	[System.Serializable]
	public class MathematicalException : Exception
	{
		public MathematicalException() : base() { }
		public MathematicalException(string message) : base(message) { }
		public MathematicalException(string message, Exception innerException) : base(message, innerException) { }
		protected MathematicalException(SerializationInfo info, StreamingContext context) : base(info, context) { }


		internal static MathematicalException MinMaxException => throw new MathematicalException("Minimum/Maximum values are invalid.");
	}

	internal static class ExceptionThrowingHelper
	{
#if NETCOREAPP3_0_OR_GREATER
		public static FormatException ThrowFormatException<TOut>(string? format, FormatException? innerException = null, string extraContext = "")
#else
		public static FormatException ThrowFormatException<TOut>(string format, FormatException innerException = null, string extraContext = "")
#endif
		{
			return new FormatException($"The format '{format}' is invalid for {typeof(TOut)}.\n" + extraContext, innerException);
		}

#if NETCOREAPP3_0_OR_GREATER
		public static FormatException ThrowParsingException<TOut>(string? input, Exception? innerException = null, string extraContext = "")
#else
		public static FormatException ThrowParsingException<TOut>(string input, Exception innerException = null, string extraContext = "")
#endif
		{
			return new FormatException($"Could not parse '{input}' as {typeof(TOut)}.\n" + extraContext, innerException);
		}

#if NETCOREAPP3_0_OR_GREATER
		public static InvalidCastException ThrowConversionException<TOut>(Type input, Exception? innerException = null, string extraContext = "")
#else
		public static InvalidCastException ThrowConversionException<TOut>(Type input, Exception innerException = null, string extraContext = "")
#endif
		{
			return new InvalidCastException($"'{input}' cannot be converted to {typeof(TOut)}.\n" + extraContext, innerException);
		}
	}
}