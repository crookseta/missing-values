using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues
{
	internal static class BitHelper
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void GetUpperAndLowerBits(ulong value, out uint upper, out uint lower)
		{
			lower = (uint)value;
			upper = (uint)(value >> 32);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void GetUpperAndLowerBits(UInt128 value, out ulong upper, out ulong lower)
		{
			lower = (ulong)value;
			upper = (ulong)(value >> 64);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void GetUpperAndLowerBits(Int128 value, out ulong upper, out ulong lower)
		{
			lower = (ulong)value;
			upper = (ulong)(value >> 64);
		}

		public static void GetDoubleParts(double dbl, out int sign, out int exp, out ulong man, out bool fFinite)
		{
			ulong bits = BitConverter.DoubleToUInt64Bits(dbl);

			sign = 1 - ((int)(bits >> 62) & 2);
			man = bits & 0x000FFFFFFFFFFFFF;
			exp = (int)(bits >> 52) & 0x7FF;
			if (exp == 0)
			{
				// Denormalized number.
				fFinite = true;
				if (man != 0)
					exp = -1074;
			}
			else if (exp == 0x7FF)
			{
				// NaN or Infinite.
				fFinite = false;
				exp = int.MaxValue;
			}
			else
			{
				fFinite = true;
				man |= 0x0010000000000000;
				exp -= 1075;
			}
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
			UInt128 upper = value.Upper;

			if (upper == 0)
			{
				return 128 + LeadingZeroCount(value.Lower);
			}
			return LeadingZeroCount(upper);
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
			UInt256 upper = value.Upper;

			if (upper == 0)
			{
				return 256 + LeadingZeroCount(value.Lower);
			}
			return LeadingZeroCount(upper);
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

		/// <summary>
		/// Produces the full product of two unsigned 128-bit numbers.
		/// </summary>
		/// <param name="a">First number to multiply.</param>
		/// <param name="b">Second number to multiply.</param>
		/// <param name="lower">The low 128-bit of the product of the specified numbers.</param>
		/// <returns>The high 128-bit of the product of the specified numbers.</returns>
		public static UInt128 BigMul(UInt128 a, UInt128 b, out UInt128 lower)
		{
			// Adaptation of algorithm for multiplication
			// of 32-bit unsigned integers described
			// in Hacker's Delight by Henry S. Warren, Jr. (ISBN 0-201-91465-4), Chapter 8
			// Basically, it's an optimized version of FOIL method applied to
			// low and high dwords of each operand

			UInt128 al = (ulong)a;
			UInt128 ah = (ulong)(a >> 64);

			UInt128 bl = (ulong)b;
			UInt128 bh = (ulong)(b >> 64);

			UInt128 mull = al * bl;
			UInt128 t = ah * bl + (ulong)(mull >> 64);
			UInt128 tl = al * bh + (ulong)t;

			lower = new UInt128((ulong)tl, (ulong)mull); ;

			return ah * bh + (ulong)(t >> 64) + (ulong)(tl >> 64);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static int SizeOf<T>()
			where T : struct
		{
			return Marshal.SizeOf(typeof(T));
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

		internal static void DangerousMakeTwosComplement(Span<uint> d)
		{
			if (d.Length > 0)
			{
				d[0] = unchecked(~d[0] + 1);

				int i = 1;

				// first do complement and +1 as long as carry is needed
				for (; d[i - 1] == 0 && i < d.Length; i++)
				{
					d[i] = unchecked(~d[i] + 1);
				}
				// now ones complement is sufficient
				for (; i < d.Length; i++)
				{
					d[i] = ~d[i];
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static T DefaultConvert<T>(out bool result)
		{
			result = false;
			return default;
		}
	}
}