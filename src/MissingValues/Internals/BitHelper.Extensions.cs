using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MissingValues.Internals;

namespace MissingValues
{
	internal static partial class BitHelper
	{
		extension(UInt128 uInt128)
		{
			public ulong Upper
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get => GetUpper(in uInt128);
			}

			public ulong Lower
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get => GetLower(in uInt128);
			}

			public static int CountDigits(UInt128 value)
			{
				var x = value | UInt128.One;
				int num1 = (int)UInt128.Log2(x) + 1;
				int num2 = (num1 * 1233) >> 12;
				return (x < Unsafe.Add(ref MemoryMarshal.GetArrayDataReference(Tables.Pow10Table), num2) ? num2 - 1 : num2) + 1;
			}
			
			#if !NET10_0_OR_GREATER
			public static UInt128 BigMul(UInt128 a, UInt128 b, out UInt128 lower)
			{
				// Adaptation of algorithm for multiplication
				// of 32-bit unsigned integers described
				// in Hacker's Delight by Henry S. Warren, Jr. (ISBN 0-201-91465-4), Chapter 8
				// Basically, it's an optimized version of FOIL method applied to
				// low and high dwords of each operand

				ulong al = a.Lower;
				ulong ah = a.Upper;

				ulong bl = b.Lower;
				ulong bh = b.Upper;

				UInt128 mull = Calculator.BigMul(al, bl);
				UInt128 t = Calculator.BigMul(ah, bl) + mull.Upper;
				UInt128 tl = Calculator.BigMul(al, bh) + t.Lower;

				lower = new UInt128(tl.Lower, mull.Lower);

				return Calculator.BigMul(ah, bh) + t.Upper + tl.Upper;
			}
			#endif
		}
		extension(Int128 int128)
		{
			public ulong Upper
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get => GetUpper(in int128);
			}

			public ulong Lower
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get => GetLower(in int128);
			}
		}
		
		public static void GetUpperAndLowerBits(UInt128 value, out ulong upper, out ulong lower)
		{
			lower = value.Lower;
			upper = value.Upper;
		}
		public static void GetUpperAndLowerBits(Int128 value, out ulong upper, out ulong lower)
		{
			lower = value.Lower;
			upper = value.Upper;
		}
		
		[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_upper")]
		private static extern ref ulong GetUpper(in UInt128 value);
		[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_lower")]
		private static extern ref ulong GetLower(in UInt128 value);
		
		[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_upper")]
		private static extern ref ulong GetUpper(in Int128 value);
		[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_lower")]
		private static extern ref ulong GetLower(in Int128 value);

		internal static T DefaultConvert<T>(out bool result)
		{
			result = false;
			return default;
		}
	}
}
