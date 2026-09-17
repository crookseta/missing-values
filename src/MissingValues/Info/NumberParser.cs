using MissingValues.Internals;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Text;

namespace MissingValues.Info
{
	internal static partial class NumberParser
	{
		internal readonly struct ParsingStatus
		{
			private const int SuccessValue = 0;
			private const int FailedValue = 1;
			private const int OverflowValue = 2;
			private const int UnderflowValue = 3;
			private const int PartialValue = 4;
			
			internal static ParsingStatus Success => new ParsingStatus(SuccessValue);
			internal static ParsingStatus Failed => new ParsingStatus(FailedValue);
			internal static ParsingStatus Overflow => new ParsingStatus(OverflowValue);
			internal static ParsingStatus Underflow => new ParsingStatus(OverflowValue);
			internal static ParsingStatus Partial => new ParsingStatus(PartialValue);

			private readonly int _status;

			private ParsingStatus(int status)
			{
				_status = status;
			}

			internal void Throw<T>(ReadOnlySpan<byte> utf8Input)
				where T : IParsable<T>, IMinMaxValue<T>
			{
				Throw<T>(new string(Encoding.UTF8.GetChars(utf8Input.ToArray())));
			}
			internal void Throw<T>(string input)
				where T : IParsable<T>, IMinMaxValue<T>
			{
				throw _status switch
				{
					OverflowValue => new OverflowException($"Could not parse '{input}' as {typeof(T)}.\nThe input is bigger than {T.MaxValue}"),
					UnderflowValue => new OverflowException($"Could not parse '{input}' as {typeof(T)}.\nThe input is smaller than {T.MinValue}"),
					_ => new FormatException($"Could not parse '{input}' as {typeof(T)}.\n"),
				};
			}
			
			internal bool IsSuccessful() => _status == SuccessValue;
			internal bool IsSuccessfulOrPartial() => _status is SuccessValue or PartialValue;
		}
		
		internal static int ConsumeTrailingNulls<TChar>(ReadOnlySpan<TChar> value, int index)
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			// For compatibility, we need to allow trailing nulls at the end of a number string
			var remainder = value.Slice(index);

			var nullsToConsume = remainder.IndexOfAnyExcept(TChar.NullCharacter);
			return index + ((nullsToConsume >= 0) ? nullsToConsume : remainder.Length);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Vector512<byte> FromChar512(ReadOnlySpan<char> span)
		{
			var shortSpan = MemoryMarshal.Cast<char, ushort>(span);
			return Vector512.NarrowWithSaturation(Vector512.Create(shortSpan), Vector512.Create(shortSpan[Vector512<ushort>.Count..]));
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Vector256<byte> FromChar256(ReadOnlySpan<char> span)
		{
			var shortSpan = MemoryMarshal.Cast<char, ushort>(span);
			return Vector256.NarrowWithSaturation(Vector256.Create(shortSpan), Vector256.Create(shortSpan[Vector256<ushort>.Count..]));
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Vector128<byte> FromChar128(ReadOnlySpan<char> span)
		{
			var shortSpan = MemoryMarshal.Cast<char, ushort>(span);
			return Vector128.NarrowWithSaturation(Vector128.Create(shortSpan), Vector128.Create(shortSpan[Vector128<ushort>.Count..]));
		}

		private static bool TryParse8Chars(ulong chunk, out ulong result)
		{
			if ((((chunk + 0x4646_4646_4646_4646UL) | ~(chunk + 0x7676_7676_7676_7676UL)) & 0x8080_8080_8080_8080UL) != 0)
			{
				result = 0;
				return false;
			}

			ulong lower = (chunk & 0x0F00_0F00_0F00_0F00) >> 8;
			ulong upper = (chunk & 0x000F_000F_000F_000F) * 10;
			result = lower + upper;
			
			lower = (result & 0x00FF_0000_00FF_0000) >> 16;
			upper = (result & 0x0000_00FF_0000_00FF) * 100;
			result = lower + upper;
			
			lower = (result & 0x0000_FFFF_0000_0000) >> 32;
			upper = (result & 0x0000_0000_0000_FFFF) * 10000;
			result = lower + upper;
			
			return true;
		}
		
		private static bool TryParse16Chars(Vector128<byte> chunk, out ulong value)
		{
			// explanation for this algorithm: https://kholdstare.github.io/technical/2020/05/26/faster-integer-parsing.html
			var zeroes = Vector128.Create((byte)'0');

			if (Vector128.GreaterThanAny(chunk, Vector128.Create((byte)'9')) ||
			    Vector128.LessThanAny(chunk, zeroes))
			{
				value = 0;
				return false;
			}

			chunk -= zeroes;
			var mult = Vector128.Create((sbyte)10, 1, 10, 1, 10, 1, 10, 1, 10, 1, 10, 1, 10, 1, 10, 1);

			var chunk16 = Ssse3.MultiplyAddAdjacent(chunk, mult);
			var mult16 = Vector128.Create((short)100, 1, 100, 1, 100, 1, 100, 1);

			var chunk32 = Sse2.MultiplyAddAdjacent(chunk16, mult16);

			chunk16 = Sse41.PackUnsignedSaturate(chunk32, chunk32).AsInt16();
			mult16 = Vector128.Create((short)10000, 1, 10000, 1, 0, 0, 0, 0);
		
			chunk32 = Sse2.MultiplyAddAdjacent(chunk16, mult16);
		
			var chunk64 = chunk32.AsUInt64();
			ulong scalar = chunk64.ToScalar();
		
			value = ((scalar & 0xffffffff) * 100_000_000) + (scalar >> 32);
			return true;
		}
		
		private static bool TryParse32Chars(Vector256<byte> chunk, out ulong first, out ulong second)
		{
			var zeroes = Vector256.Create((byte)'0');

			if (Vector256.GreaterThanAny(chunk, Vector256.Create((byte)'9')) ||
			    Vector256.LessThanAny(chunk, zeroes))
			{
				first = 0;
				second = 0;
				return false;
			}
		
			chunk -= zeroes;
			var mult = Vector256.Create(
				(sbyte)10, 1, 10, 1, 10, 1, 10, 1, 10, 1, 10, 1, 10, 1, 10, 1, 
				10, 1, 10, 1, 10, 1, 10, 1, 10, 1, 10, 1, 10, 1, 10, 1);

			Vector256<short> chunk16 = Avx2.MultiplyAddAdjacent(chunk, mult);
			var mult16 = Vector256.Create(
				(short)100, 1, 100, 1, 100, 1, 100, 1, 
				100, 1, 100, 1, 100, 1, 100, 1);
		
			Vector256<int> chunk32 = Avx2.MultiplyAddAdjacent(chunk16, mult16);

			chunk16 = Avx2.PackUnsignedSaturate(chunk32, chunk32).AsInt16();
			mult16 = Vector256.Create(
				(short)10000, 1, 10000, 1, 0, 0, 0, 0
				, 10000, 1, 10000, 1, 0, 0, 0, 0);
			
			Vector256<int> result = Avx2.MultiplyAddAdjacent(chunk16, mult16);
			Vector256<ulong> result64 = result.AsUInt64();

			ulong lane0 = result64.GetElement(0);
			first = ((lane0 & 0xFFFF_FFFF) * 100_000_000) + (lane0 >> 32);

			ulong lane1 = result64.GetElement(2);
			second = ((lane1 & 0xFFFF_FFFF) * 100_000_000) + (lane1 >> 32);

			return true;
		}
		
		private static bool TryParse64Chars(Vector512<byte> chunk, out ulong first, out ulong second, out ulong third, out ulong fourth)
		{
			var zeroes = Vector512.Create((byte)'0');

			if (Vector512.GreaterThanAny(chunk, Vector512.Create((byte)'9')) ||
			    Vector512.LessThanAny(chunk, zeroes))
			{
				first = 0;
				second = 0;
				third = 0;
				fourth = 0;
				return false;
			}
		
			chunk -= zeroes;
			var mult = Vector512.Create(
				(sbyte)10, 1, 10, 1, 10, 1, 10, 1, 10, 1, 10, 1, 10, 1, 10, 1, 
				10, 1, 10, 1, 10, 1, 10, 1, 10, 1, 10, 1, 10, 1, 10, 1,
				10, 1, 10, 1, 10, 1, 10, 1, 10, 1, 10, 1, 10, 1, 10, 1, 
				10, 1, 10, 1, 10, 1, 10, 1, 10, 1, 10, 1, 10, 1, 10, 1
				);

			Vector512<short> chunk16 = Avx512BW.MultiplyAddAdjacent(chunk, mult);
			var mult16 = Vector512.Create(
				(short)100, 1, 100, 1, 100, 1, 100, 1, 
				100, 1, 100, 1, 100, 1, 100, 1,
				100, 1, 100, 1, 100, 1, 100, 1, 
				100, 1, 100, 1, 100, 1, 100, 1
				);
		
			Vector512<int> chunk32 = Avx512BW.MultiplyAddAdjacent(chunk16, mult16);

			chunk16 = Avx512BW.PackUnsignedSaturate(chunk32, chunk32).AsInt16();
			mult16 = Vector512.Create(
				(short)10000, 1, 10000, 1, 0, 0, 0, 0, 
				10000, 1, 10000, 1, 0, 0, 0, 0,
				10000, 1, 10000, 1, 0, 0, 0, 0, 
				10000, 1, 10000, 1, 0, 0, 0, 0
				);
			Vector512<int> result = Avx512BW.MultiplyAddAdjacent(chunk16, mult16);
			Vector512<ulong> result64 = result.AsUInt64();

			ulong lane = result64.GetElement(0);
			first = ((lane & 0xFFFF_FFFF) * 100_000_000) + (lane >> 32);

			lane = result64.GetElement(2);
			second = ((lane & 0xFFFF_FFFF) * 100_000_000) + (lane >> 32);

			lane = result64.GetElement(4);
			third = ((lane & 0xFFFF_FFFF) * 100_000_000) + (lane >> 32);

			lane = result64.GetElement(6);
			fourth = ((lane & 0xFFFF_FFFF) * 100_000_000) + (lane >> 32);

			return true;
		}
	}
}
