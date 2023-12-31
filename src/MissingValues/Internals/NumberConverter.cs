using System;
using System.Buffers;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace MissingValues.Internals
{
	internal class NumberConverter
	{
		public const int StackallocByteThreshold = 256;
		public const int StackallocCharThreshold = StackallocByteThreshold / 2;

#if NET8_0_OR_GREATER
		internal static int CopyValue(in Utf8JsonReader reader, Span<byte> utf8Destination)
		{
			if (reader.ValueIsEscaped)
			{
				return reader.CopyString(utf8Destination);
			}

			int bytesWritten;

			if (reader.HasValueSequence)
			{
				ReadOnlySequence<byte> valueSequence = reader.ValueSequence;
				valueSequence.CopyTo(utf8Destination);
				bytesWritten = (int)valueSequence.Length;
			}
			else
			{
				ReadOnlySpan<byte> valueSpan = reader.ValueSpan;
				valueSpan.CopyTo(utf8Destination);
				bytesWritten = valueSpan.Length;
			}

			if (!Utf8.IsValid(utf8Destination.Slice(0, bytesWritten)))
			{
				Thrower.InvalidFormat("");
			}

			return bytesWritten;
		}
#else
		internal static int CopyValue(in Utf8JsonReader reader, Span<char> destination)
		{
			if (reader.ValueIsEscaped)
			{
				return reader.CopyString(destination);
			}

			scoped ReadOnlySpan<byte> unescapedSource;
			byte[]? rentedBuffer = null;
			int valueLength;

			if (reader.HasValueSequence)
			{
				ReadOnlySequence<byte> valueSequence = reader.ValueSequence;
				valueLength = checked((int)valueSequence.Length);

				Span<byte> intermediate = valueLength <= StackallocByteThreshold ?
						stackalloc byte[StackallocByteThreshold] :
						(rentedBuffer = ArrayPool<byte>.Shared.Rent(valueLength));

				valueSequence.CopyTo(intermediate);
				unescapedSource = intermediate.Slice(0, valueLength);
			}
			else
			{
				unescapedSource = reader.ValueSpan;
			}

			int charsWritten = Encoding.UTF8.GetChars(unescapedSource, destination);

			if (rentedBuffer != null)
			{
				new Span<byte>(rentedBuffer, 0, unescapedSource.Length).Clear();
				ArrayPool<byte>.Shared.Return(rentedBuffer);
			}

			return charsWritten;
		}
#endif

		private static T ReadCore<T>(ref Utf8JsonReader reader)
			where T : struct, INumberBase<T>
		{
			int bufferLength = reader.HasValueSequence ? checked((int)reader.ValueSequence.Length) : reader.ValueSpan.Length;

#if NET8_0_OR_GREATER
			byte[]? rentedBuffer = null;
			Span<byte> buffer = bufferLength <= StackallocByteThreshold
				? stackalloc byte[StackallocByteThreshold]
				: (rentedBuffer = ArrayPool<byte>.Shared.Rent(bufferLength));
#else
			char[]? rentedBuffer = null;
			Span<char> buffer = bufferLength <= StackallocCharThreshold
				? stackalloc char[StackallocCharThreshold]
				: (rentedBuffer = ArrayPool<char>.Shared.Rent(bufferLength));
#endif

			int written = CopyValue(in reader, buffer);
			if (!TryParse(buffer[..written], out T result))
			{
				Thrower.InvalidFormat("Json");
			}

			if (rentedBuffer is not null)
			{
#if NET8_0_OR_GREATER
                ArrayPool<byte>.Shared.Return(rentedBuffer);
#else
				ArrayPool<char>.Shared.Return(rentedBuffer);
#endif
			}

			return result;
		}
		private static void WriteCore<T>(Utf8JsonWriter writer, in T value)
			where T : struct, INumberBase<T>
		{
			int maxFormatLength = value switch
			{
				UInt256 => 78,
				Int256 => 77 + 2,
				UInt512 => 155,
				Int512 => 154 + 2,
				Quad => 11563,
			};
#if NET8_0_OR_GREATER
			byte[]? bufferArray = null;
			scoped Span<byte> buffer;

			if (maxFormatLength > StackallocByteThreshold)
			{
				bufferArray = ArrayPool<byte>.Shared.Rent(maxFormatLength);
				buffer = bufferArray;
			}
			else
			{
				buffer = stackalloc byte[maxFormatLength];
			}
#else
			char[]? bufferArray = null;
			scoped Span<char> buffer = stackalloc char[maxFormatLength];

			if (maxFormatLength > StackallocCharThreshold)
			{
				bufferArray = ArrayPool<char>.Shared.Rent(maxFormatLength);
				buffer = bufferArray;
			}
			else
			{
				buffer = stackalloc char[maxFormatLength];
			}
#endif
			Format(buffer, in value, out int written);
			writer.WriteRawValue(buffer[..written]);

			if (bufferArray is not null)
			{
#if NET8_0_OR_GREATER
				ArrayPool<byte>.Shared.Return(bufferArray);
#else
				ArrayPool<char>.Shared.Return(bufferArray);
#endif
			}
		}

		private static void Format<T>(
#if NET8_0_OR_GREATER
			Span<byte> destination, 
#else
			Span<char> destination, 
#endif
			in T value, 
			out int written) 
			where T : struct, INumberBase<T>
		{
			value.TryFormat(destination, out written, ReadOnlySpan<char>.Empty, CultureInfo.InvariantCulture);
		}
		private static bool TryParse<T>(
#if NET8_0_OR_GREATER
			ReadOnlySpan<byte> s, 
#else
			ReadOnlySpan<char> s,
#endif
			out T value
			)
			where T : struct, INumberBase<T>
		{
			return T.TryParse(s, CultureInfo.InvariantCulture, out value);
		}

		internal sealed class UInt256Converter : JsonConverter<UInt256>
		{
			public override UInt256 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				if (reader.TokenType != JsonTokenType.Number)
				{
					Thrower.ExpectedNumber(reader.TokenType);
				}

				return ReadCore<UInt256>(ref reader);
			}

			public override void Write(Utf8JsonWriter writer, UInt256 value, JsonSerializerOptions options)
			{
				WriteCore(writer, value);
			}
		}
		internal sealed class Int256Converter : JsonConverter<Int256>
		{
			public override Int256 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				if (reader.TokenType != JsonTokenType.Number)
				{
					Thrower.ExpectedNumber(reader.TokenType);
				}

				return ReadCore<Int256>(ref reader);
			}

			public override void Write(Utf8JsonWriter writer, Int256 value, JsonSerializerOptions options)
			{
				WriteCore(writer, value);
			}
		}
		internal sealed class UInt512Converter : JsonConverter<UInt512>
		{
			public override UInt512 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				if (reader.TokenType != JsonTokenType.Number)
				{
					Thrower.ExpectedNumber(reader.TokenType);
				}

				return ReadCore<UInt512>(ref reader);
			}

			public override void Write(Utf8JsonWriter writer, UInt512 value, JsonSerializerOptions options)
			{
				WriteCore(writer, value);
			}
		}
		internal sealed class Int512Converter : JsonConverter<Int512>
		{
			public override Int512 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				if (reader.TokenType != JsonTokenType.Number)
				{
					Thrower.ExpectedNumber(reader.TokenType);
				}

				return ReadCore<Int512>(ref reader);
			}

			public override void Write(Utf8JsonWriter writer, Int512 value, JsonSerializerOptions options)
			{
				WriteCore(writer, value);
			}
		}
		internal sealed class QuadConverter : JsonConverter<Quad>
		{
			public override Quad Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				if (reader.TokenType != JsonTokenType.Number)
				{
					Thrower.ExpectedNumber(reader.TokenType);
				}

				return ReadCore<Quad>(ref reader);
			}

			public override void Write(Utf8JsonWriter writer, Quad value, JsonSerializerOptions options)
			{
				WriteCore(writer, value);
			}
		}
	}
}
