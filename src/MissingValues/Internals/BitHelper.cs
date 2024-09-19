using MissingValues.Info;
using MissingValues.Internals;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues
{
	internal static partial class BitHelper
	{
		public static void GetUpperAndLowerBits(UInt128 value, out ulong upper, out ulong lower)
		{
			lower = value.GetLowerBits();
			upper = value.GetUpperBits();
		}
		public static void GetUpperAndLowerBits(Int128 value, out ulong upper, out ulong lower)
		{
			lower = value.GetLowerBits();
			upper = value.GetUpperBits();
		}

		public static ulong GetUpperBits(this in UInt128 value)
		{
			return unchecked((ulong)(value >> 64));
		}
		public static ulong GetUpperBits(this in Int128 value)
		{
			return unchecked((ulong)(value >> 64));
		}
		public static ulong GetLowerBits(this in UInt128 value)
		{
			return unchecked((ulong)(value));
		}
		public static ulong GetLowerBits(this in Int128 value)
		{
			return unchecked((ulong)(value));
		}

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

		internal static T DefaultConvert<T>(out bool result)
		{
			result = false;
			return default;
		}
	}
}