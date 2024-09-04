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
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void GetUpperAndLowerBits(UInt128 value, out ulong upper, out ulong lower)
		{
			lower = value.GetLowerBits();
			upper = value.GetUpperBits();
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void GetUpperAndLowerBits(Int128 value, out ulong upper, out ulong lower)
		{
			lower = value.GetLowerBits();
			upper = value.GetUpperBits();
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong GetUpperBits(this in UInt128 value)
		{
			return unchecked((ulong)(value >> 64));
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong GetUpperBits(this in Int128 value)
		{
			return unchecked((ulong)(value >> 64));
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong GetLowerBits(this in UInt128 value)
		{
			return unchecked((ulong)(value));
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong GetLowerBits(this in Int128 value)
		{
			return unchecked((ulong)(value));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static int LeadingZeroCount(UInt128 value)
		{
			GetUpperAndLowerBits(value, out var upper, out var lower);

			if (upper == 0)
			{
				return 64 + BitOperations.LeadingZeroCount(lower);
			}
			return BitOperations.LeadingZeroCount(upper);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static int LeadingZeroCount(UInt256 value)
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
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static int LeadingZeroCount(Int256 value)
		{
			UInt128 upper = value.Upper;

			if (upper == 0)
			{
				return 128 + LeadingZeroCount(value.Lower);
			}
			return LeadingZeroCount(upper);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static int LeadingZeroCount(UInt512 value)
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
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static int LeadingZeroCount(Int512 value)
		{
			UInt256 upper = value.Upper;

			if (upper == 0)
			{
				return 256 + LeadingZeroCount(value.Lower);
			}
			return LeadingZeroCount(upper);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static UInt128 ReverseEndianness(UInt128 value)
		{
			GetUpperAndLowerBits(value, out var upper, out var lower);

			return new UInt128(BinaryPrimitives.ReverseEndianness(lower), BinaryPrimitives.ReverseEndianness(upper));
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static Int128 ReverseEndianness(Int128 value)
		{
			GetUpperAndLowerBits(value, out var upper, out var lower);

			return new Int128(BinaryPrimitives.ReverseEndianness(lower), BinaryPrimitives.ReverseEndianness(upper));
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static UInt256 ReverseEndianness(UInt256 value)
		{
			return new UInt256(ReverseEndianness(value.Lower), ReverseEndianness(value.Upper));
		}

		internal static Int256 ReverseEndianness(Int256 value)
		{
			return new(ReverseEndianness(value.Lower), ReverseEndianness(value.Upper));
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static UInt512 ReverseEndianness(UInt512 value)
		{
			return new UInt512(ReverseEndianness(value.Lower), ReverseEndianness(value.Upper));
		}

		internal static Int512 ReverseEndianness(Int512 value)
		{
			return new Int512(ReverseEndianness(value.Lower), ReverseEndianness(value.Upper));
		}

		internal static int CountDigits(UInt128 num)
		{
			UInt128 p10 = new UInt128(5421010862427522170UL, 687399551400673280UL);

			for (int i = 39; i > 0; i--)
			{
				if (num >= p10)
				{
					return i;
				}
				p10 /= 10UL;
			}

			return 1;
		}

		internal static int CountDigits<TUnsigned, TSigned>(in TUnsigned num)
			where TUnsigned : IFormattableUnsignedInteger<TUnsigned, TSigned>
			where TSigned : IFormattableInteger<TSigned>
		{
			return TUnsigned.CountDigits(in num);
		}

		internal static int CountDigits<T>(T num, T numberBase)
			where T : struct, IFormattableInteger<T>
		{
			int count = 0;

			if (num >= numberBase)
			{
				T basePow2 = numberBase;
				if (numberBase == T.Two)
				{
					basePow2 = T.TwoPow2;
				}
				else if (numberBase == T.Ten)
				{
					basePow2 = T.TenPow2;
				}
				else if (numberBase == T.Sixteen)
				{
					basePow2 = T.SixteenPow2;
				}

				if (num >= basePow2)
				{
					T basePow3 = basePow2;
					if (basePow2 == T.TwoPow2)
					{
						basePow3 = T.TwoPow3;
					}
					else if (basePow2 == T.TenPow2)
					{
						basePow3 = T.TenPow3;
					}
					else if (basePow2 == T.SixteenPow2)
					{
						basePow3 = T.SixteenPow3;
					}

					do
					{
						num /= basePow3;
						count += 3;
					} while (num > basePow2);
					while (num > numberBase)
					{
						num /= basePow2;
						count += 2;
					}
				}
				else
				{
					do
					{
						num /= basePow2;
						count += 2;
					} while (num > numberBase);
				}
				while (num != T.Zero)
				{
					num /= numberBase;
					++count;
				}
			}
			else
			{
				do
				{
					num /= numberBase;
					++count;
				} while (num != T.Zero);
			}

			return count;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static T DefaultConvert<T>(out bool result)
		{
			result = false;
			return default;
		}
	}
}