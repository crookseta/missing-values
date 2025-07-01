using MissingValues.Internals;
using System.Buffers.Binary;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MissingValues
{
	internal static partial class BitHelper
	{
		internal static int LeadingZeroCount(in UInt256 value)
		{
			if (value.Part3 != 0)
			{
				return BitOperations.LeadingZeroCount(value.Part3);
			}
			if (value.Part2 != 0)
			{
				return 64 + BitOperations.LeadingZeroCount(value.Part2);
			}
			if (value.Part1 != 0)
			{
				return 128 + BitOperations.LeadingZeroCount(value.Part1);
			}
			return 192 + BitOperations.LeadingZeroCount(value.Part0);
		}
		internal static int LeadingZeroCount(in Int256 value)
		{
			if (value.Part3 != 0)
			{
				return BitOperations.LeadingZeroCount(value.Part3);
			}
			if (value.Part2 != 0)
			{
				return 64 + BitOperations.LeadingZeroCount(value.Part2);
			}
			if (value.Part1 != 0)
			{
				return 128 + BitOperations.LeadingZeroCount(value.Part1);
			}
			return 192 + BitOperations.LeadingZeroCount(value.Part0);
		}
		internal static int LeadingZeroCount(in UInt512 value)
		{
			if (value.Part7 != 0)
			{
				return BitOperations.LeadingZeroCount(value.Part7);
			}
			if (value.Part6 != 0)
			{
				return 64 + BitOperations.LeadingZeroCount(value.Part6);
			}
			if (value.Part5 != 0)
			{
				return 128 + BitOperations.LeadingZeroCount(value.Part5);
			}
			if (value.Part4 != 0)
			{
				return 192 + BitOperations.LeadingZeroCount(value.Part4);
			}
			if (value.Part3 != 0)
			{
				return 256 + BitOperations.LeadingZeroCount(value.Part3);
			}
			if (value.Part2 != 0)
			{
				return 320 + BitOperations.LeadingZeroCount(value.Part2);
			}
			if (value.Part1 != 0)
			{
				return 384 + BitOperations.LeadingZeroCount(value.Part1);
			}
			return 448 + BitOperations.LeadingZeroCount(value.Part0);
		}
		internal static int LeadingZeroCount(in Int512 value)
		{
			if (value.Part7 != 0)
			{
				return BitOperations.LeadingZeroCount(value.Part7);
			}
			if (value.Part6 != 0)
			{
				return 64 + BitOperations.LeadingZeroCount(value.Part6);
			}
			if (value.Part5 != 0)
			{
				return 128 + BitOperations.LeadingZeroCount(value.Part5);
			}
			if (value.Part4 != 0)
			{
				return 192 + BitOperations.LeadingZeroCount(value.Part4);
			}
			if (value.Part3 != 0)
			{
				return 256 + BitOperations.LeadingZeroCount(value.Part3);
			}
			if (value.Part2 != 0)
			{
				return 320 + BitOperations.LeadingZeroCount(value.Part2);
			}
			if (value.Part1 != 0)
			{
				return 384 + BitOperations.LeadingZeroCount(value.Part1);
			}
			return 448 + BitOperations.LeadingZeroCount(value.Part0);
		}

		internal static int Log2(in UInt256 value)
		{
			if (value.Part3 != 0)
			{
				return 192 + BitOperations.Log2(value.Part3);
			}
			if (value.Part2 != 0)
			{
				return 128 + BitOperations.Log2(value.Part2);
			}
			if (value.Part1 != 0)
			{
				return 64 + BitOperations.Log2(value.Part1);
			}
			return BitOperations.Log2(value.Part0);
		}
		internal static int Log2(in Int256 value)
		{
			if ((long)value.Part3 < 0)
			{
				Thrower.NeedsNonNegative<Int256>();
			}

			if (value.Part3 != 0)
			{
				return 192 + BitOperations.Log2(value.Part3);
			}
			if (value.Part2 != 0)
			{
				return 128 + BitOperations.Log2(value.Part2);
			}
			if (value.Part1 != 0)
			{
				return 64 + BitOperations.Log2(value.Part1);
			}
			return BitOperations.Log2(value.Part0);
		}
		internal static int Log2(in UInt512 value)
		{
			if (value.Part7 != 0)
			{
				return 448 + BitOperations.Log2(value.Part7);
			}
			if (value.Part6 != 0)
			{
				return 384 + BitOperations.Log2(value.Part6);
			}
			if (value.Part5 != 0)
			{
				return 320 + BitOperations.Log2(value.Part5);
			}
			if (value.Part4 != 0)
			{
				return 256 + BitOperations.Log2(value.Part4);
			}
			if (value.Part3 != 0)
			{
				return 192 + BitOperations.Log2(value.Part3);
			}
			if (value.Part2 != 0)
			{
				return 128 + BitOperations.Log2(value.Part2);
			}
			if (value.Part1 != 0)
			{
				return 64 + BitOperations.Log2(value.Part1);
			}
			return BitOperations.Log2(value.Part0);
		}
		internal static int Log2(in Int512 value)
		{
			if ((long)value.Part7 < 0)
			{
				Thrower.NeedsNonNegative<Int512>();
			}

			if (value.Part7 != 0)
			{
				return 448 + BitOperations.Log2(value.Part7);
			}
			if (value.Part6 != 0)
			{
				return 384 + BitOperations.Log2(value.Part6);
			}
			if (value.Part5 != 0)
			{
				return 320 + BitOperations.Log2(value.Part5);
			}
			if (value.Part4 != 0)
			{
				return 256 + BitOperations.Log2(value.Part4);
			}
			if (value.Part3 != 0)
			{
				return 192 + BitOperations.Log2(value.Part3);
			}
			if (value.Part2 != 0)
			{
				return 128 + BitOperations.Log2(value.Part2);
			}
			if (value.Part1 != 0)
			{
				return 64 + BitOperations.Log2(value.Part1);
			}
			return BitOperations.Log2(value.Part0);
		}

		internal static int PopCount(in UInt256 value)
		{
			return BitOperations.PopCount(value.Part0) + BitOperations.PopCount(value.Part1) + BitOperations.PopCount(value.Part2) + BitOperations.PopCount(value.Part3);
		}
		internal static int PopCount(in Int256 value)
		{
			return BitOperations.PopCount(value.Part0) + BitOperations.PopCount(value.Part1) + BitOperations.PopCount(value.Part2) + BitOperations.PopCount(value.Part3);
		}
		internal static int PopCount(in UInt512 value)
		{
			return BitOperations.PopCount(value.Part0) + BitOperations.PopCount(value.Part1) + BitOperations.PopCount(value.Part2) + BitOperations.PopCount(value.Part3)
			+ BitOperations.PopCount(value.Part4) + BitOperations.PopCount(value.Part5) + BitOperations.PopCount(value.Part6) + BitOperations.PopCount(value.Part7);
		}
		internal static int PopCount(in Int512 value)
		{
			return BitOperations.PopCount(value.Part0) + BitOperations.PopCount(value.Part1) + BitOperations.PopCount(value.Part2) + BitOperations.PopCount(value.Part3)
			+ BitOperations.PopCount(value.Part4) + BitOperations.PopCount(value.Part5) + BitOperations.PopCount(value.Part6) + BitOperations.PopCount(value.Part7);
		}

		internal static UInt256 ReverseEndianness(in UInt256 value)
		{
			return new(BinaryPrimitives.ReverseEndianness(value.Part0), BinaryPrimitives.ReverseEndianness(value.Part1), BinaryPrimitives.ReverseEndianness(value.Part2), BinaryPrimitives.ReverseEndianness(value.Part3));
		}
		internal static Int256 ReverseEndianness(in Int256 value)
		{
			return new(BinaryPrimitives.ReverseEndianness(value.Part0), BinaryPrimitives.ReverseEndianness(value.Part1), BinaryPrimitives.ReverseEndianness(value.Part2), BinaryPrimitives.ReverseEndianness(value.Part3));
		}
		internal static UInt512 ReverseEndianness(in UInt512 value)
		{
			return new(
				BinaryPrimitives.ReverseEndianness(value.Part0), BinaryPrimitives.ReverseEndianness(value.Part1), BinaryPrimitives.ReverseEndianness(value.Part2), BinaryPrimitives.ReverseEndianness(value.Part3),
				BinaryPrimitives.ReverseEndianness(value.Part4), BinaryPrimitives.ReverseEndianness(value.Part5), BinaryPrimitives.ReverseEndianness(value.Part6), BinaryPrimitives.ReverseEndianness(value.Part7)
				);
		}
		internal static Int512 ReverseEndianness(in Int512 value)
		{
			return new(
				BinaryPrimitives.ReverseEndianness(value.Part0), BinaryPrimitives.ReverseEndianness(value.Part1), BinaryPrimitives.ReverseEndianness(value.Part2), BinaryPrimitives.ReverseEndianness(value.Part3),
				BinaryPrimitives.ReverseEndianness(value.Part4), BinaryPrimitives.ReverseEndianness(value.Part5), BinaryPrimitives.ReverseEndianness(value.Part6), BinaryPrimitives.ReverseEndianness(value.Part7)
				);
		}

		internal static int TrailingZeroCount(in UInt256 value)
		{
			if (value.Part0 != 0)
			{
				return BitOperations.TrailingZeroCount(value.Part0);
			}
			if (value.Part1 != 0)
			{
				return 64 + BitOperations.TrailingZeroCount(value.Part1);
			}
			if (value.Part2 != 0)
			{
				return 128 + BitOperations.TrailingZeroCount(value.Part2);
			}
			return 192 + BitOperations.TrailingZeroCount(value.Part3);
		}
		internal static int TrailingZeroCount(in Int256 value)
		{
			if (value.Part0 != 0)
			{
				return BitOperations.TrailingZeroCount(value.Part0);
			}
			if (value.Part1 != 0)
			{
				return 64 + BitOperations.TrailingZeroCount(value.Part1);
			}
			if (value.Part2 != 0)
			{
				return 128 + BitOperations.TrailingZeroCount(value.Part2);
			}
			return 192 + BitOperations.TrailingZeroCount(value.Part3);
		}
		internal static int TrailingZeroCount(in UInt512 value)
		{
			if (value.Part0 != 0)
			{
				return BitOperations.TrailingZeroCount(value.Part0);
			}
			if (value.Part1 != 0)
			{
				return 64 + BitOperations.TrailingZeroCount(value.Part1);
			}
			if (value.Part2 != 0)
			{
				return 128 + BitOperations.TrailingZeroCount(value.Part2);
			}
			if (value.Part3 != 0)
			{
				return 192 + BitOperations.TrailingZeroCount(value.Part3);
			}
			if (value.Part4 != 0)
			{
				return 256 + BitOperations.TrailingZeroCount(value.Part4);
			}
			if (value.Part5 != 0)
			{
				return 320 + BitOperations.TrailingZeroCount(value.Part5);
			}
			if (value.Part6 != 0)
			{
				return 384 + BitOperations.TrailingZeroCount(value.Part6);
			}
			return 448 + BitOperations.TrailingZeroCount(value.Part7);
		}
		internal static int TrailingZeroCount(in Int512 value)
		{
			if (value.Part0 != 0)
			{
				return BitOperations.TrailingZeroCount(value.Part0);
			}
			if (value.Part1 != 0)
			{
				return 64 + BitOperations.TrailingZeroCount(value.Part1);
			}
			if (value.Part2 != 0)
			{
				return 128 + BitOperations.TrailingZeroCount(value.Part2);
			}
			if (value.Part3 != 0)
			{
				return 192 + BitOperations.TrailingZeroCount(value.Part3);
			}
			if (value.Part4 != 0)
			{
				return 256 + BitOperations.TrailingZeroCount(value.Part4);
			}
			if (value.Part5 != 0)
			{
				return 320 + BitOperations.TrailingZeroCount(value.Part5);
			}
			if (value.Part6 != 0)
			{
				return 384 + BitOperations.TrailingZeroCount(value.Part6);
			}
			return 448 + BitOperations.TrailingZeroCount(value.Part7);
		}
		
		internal static int GetTrimLength(in UInt256 value)
		{
			if (value.Part3 != 0)
			{
				return 4;
			}
			if (value.Part2 != 0)
			{
				return 3;
			}
			if (value.Part1 != 0)
			{
				return 2;
			}

			return 1;
		}
		internal static int GetTrimLength(in Int256 value)
		{
			if (value.Part3 != 0)
			{
				return 4;
			}
			if (value.Part2 != 0)
			{
				return 3;
			}
			if (value.Part1 != 0)
			{
				return 2;
			}

			return 1;
		}
		internal static int GetTrimLength(in UInt512 value)
		{
			if (value.Part7 != 0)
			{
				return 8;
			}
			if (value.Part6 != 0)
			{
				return 7;
			}
			if (value.Part5 != 0)
			{
				return 6;
			}
			if (value.Part4 != 0)
			{
				return 5;
			}
			if (value.Part3 != 0)
			{
				return 4;
			}
			if (value.Part2 != 0)
			{
				return 3;
			}
			if (value.Part1 != 0)
			{
				return 2;
			}

			return 1;
		}
		internal static int GetTrimLength(in Int512 value)
		{
			if (value.Part7 != 0)
			{
				return 8;
			}
			if (value.Part6 != 0)
			{
				return 7;
			}
			if (value.Part5 != 0)
			{
				return 6;
			}
			if (value.Part4 != 0)
			{
				return 5;
			}
			if (value.Part3 != 0)
			{
				return 4;
			}
			if (value.Part2 != 0)
			{
				return 3;
			}
			if (value.Part1 != 0)
			{
				return 2;
			}

			return 1;
		}

		internal static void Write<T>(Span<ulong> destination, in T value)
			where T : unmanaged, IBigInteger<T>
		{
			Debug.Assert(((typeof(T) == typeof(UInt256) || typeof(T) == typeof(Int256)) && destination.Length == 4) || ((typeof(T) == typeof(UInt512) || typeof(T) == typeof(Int512)) && destination.Length == 8));
			
#if BIGENDIAN
			ref byte dest = ref Unsafe.As<ulong, byte>(ref MemoryMarshal.GetReference(destination));
			Unsafe.WriteUnaligned(ref Unsafe.Add(ref dest, sizeof(ulong) * 0), ulong.CreateTruncating(value));
			Unsafe.WriteUnaligned(ref Unsafe.Add(ref dest, sizeof(ulong) * 1), ulong.CreateTruncating(value >>> 64));
			Unsafe.WriteUnaligned(ref Unsafe.Add(ref dest, sizeof(ulong) * 2), ulong.CreateTruncating(value >>> 128));
			Unsafe.WriteUnaligned(ref Unsafe.Add(ref dest, sizeof(ulong) * 3), ulong.CreateTruncating(value >>> 192));
			if (typeof(T) == typeof(UInt512) || typeof(T) == typeof(Int512))
			{
				Unsafe.WriteUnaligned(ref Unsafe.Add(ref dest, sizeof(ulong) * 4), ulong.CreateTruncating(value >>> 256));
				Unsafe.WriteUnaligned(ref Unsafe.Add(ref dest, sizeof(ulong) * 5), ulong.CreateTruncating(value >>> 320));
				Unsafe.WriteUnaligned(ref Unsafe.Add(ref dest, sizeof(ulong) * 6), ulong.CreateTruncating(value >>> 384));
				Unsafe.WriteUnaligned(ref Unsafe.Add(ref dest, sizeof(ulong) * 7), ulong.CreateTruncating(value >>> 448));
			}
#else
			Unsafe.WriteUnaligned(ref Unsafe.As<ulong, byte>(ref MemoryMarshal.GetReference(destination)), value);
#endif
		}
		
		internal static T Read<T>(ReadOnlySpan<ulong> source)
			where T : unmanaged, IBigInteger<T>
		{
			Debug.Assert(((typeof(T) == typeof(UInt256) || typeof(T) == typeof(Int256)) && source.Length == 4) || ((typeof(T) == typeof(UInt512) || typeof(T) == typeof(Int512)) && source.Length == 8));
			
#if BIGENDIAN
			T result = T.CreateTruncating(source[0]);
			result |= T.CreateTruncating(source[1]) << 64;
			result |= T.CreateTruncating(source[2]) << 128;
			result |= T.CreateTruncating(source[3]) << 192;
			if (typeof(T) == typeof(UInt512) || typeof(T) == typeof(Int512))
			{
				result |= T.CreateTruncating(source[4]) << 256;
				result |= T.CreateTruncating(source[5]) << 320;
				result |= T.CreateTruncating(source[6]) << 384;
				result |= T.CreateTruncating(source[7]) << 448;
			}
			return result;
#else
			return Unsafe.ReadUnaligned<T>(in Unsafe.As<ulong, byte>(ref MemoryMarshal.GetReference(source)));
#endif
		}
	}
}