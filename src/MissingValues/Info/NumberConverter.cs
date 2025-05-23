using MissingValues.Internals;
using System.Buffers;
using System.Globalization;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace MissingValues.Info
{
	internal class NumberConverter
	{
		public const int StackallocByteThreshold = 512;

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

		private static T ReadCore<T>(ref Utf8JsonReader reader)
			where T : struct, INumberBase<T>
		{
			int bufferLength = reader.HasValueSequence ? checked((int)reader.ValueSequence.Length) : reader.ValueSpan.Length;

			byte[]? rentedBuffer = null;
			Span<byte> buffer = bufferLength <= StackallocByteThreshold
				? stackalloc byte[StackallocByteThreshold]
				: (rentedBuffer = ArrayPool<byte>.Shared.Rent(bufferLength));

			int written = CopyValue(in reader, buffer);
			if (!TryParse(buffer[..written], out T result))
			{
				Thrower.InvalidFormat("Json");
			}

			if (rentedBuffer is not null)
			{
				ArrayPool<byte>.Shared.Return(rentedBuffer);
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
				Octo => 183466
			};
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
			Format(buffer, in value, out int written);
			writer.WriteRawValue(buffer[..written]);

			if (bufferArray is not null)
			{
				ArrayPool<byte>.Shared.Return(bufferArray);
			}
		}

		private static void Format<T>(
			Span<byte> destination,
			in T value,
			out int written)
			where T : struct, INumberBase<T>
		{
			value.TryFormat(destination, out written, ReadOnlySpan<char>.Empty, CultureInfo.InvariantCulture);
		}
		private static bool TryParse<T>(
			ReadOnlySpan<byte> s,
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
		internal sealed class OctoConverter : JsonConverter<Octo>
		{
			public override Octo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				if (reader.TokenType != JsonTokenType.Number)
				{
					Thrower.ExpectedNumber(reader.TokenType);
				}

				return ReadCore<Octo>(ref reader);
			}

			public override void Write(Utf8JsonWriter writer, Octo value, JsonSerializerOptions options)
			{
				WriteCore(writer, value);
			}
		}
	}
}
