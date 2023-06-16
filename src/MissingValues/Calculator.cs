using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MissingValues
{
	/// <summary>
	/// Provides multiple methods for various mathematical functions.
	/// </summary>
	public static partial class Calculator
	{
		/// <summary>
		/// Represents the ratio of a line segment cut into two pieces of different lengths 
		/// such that the ratio of the whole segment to that of the longer segment is equal 
		/// to the ratio of the longer segment to the shorter segment.
		/// </summary>
		public const double GoldenRatio = 1.618034d;

		/// <summary>
		/// Returns the number of digits in a given binary integer when represented in a specified <paramref name="numberBase"/>.
		/// </summary>
		/// <typeparam name="T">Binary integer data type.</typeparam>
		/// <param name="number">Binary integer to count.</param>
		/// <param name="numberBase">Base the binary integer is represented as.</param>
		/// <returns>Number of digits in a given binary integer when represented in a specified base.</returns>
		public static int CountDigits<T>(T number, T numberBase)
			where T : struct, IBinaryInteger<T>
		{
			int count = 0;
			do
			{
				number /= numberBase;
				++count;
			}
			while (number != T.Zero);
			return count;
		}

		/// <summary>
		/// Returns a specified integer raise to the specific power.
		/// </summary>
		/// <typeparam name="T">Binary integer data type.</typeparam>
		/// <param name="x">A binary integer to be raised to a power.</param>
		/// <param name="y">A binary integer that specifies a power.</param>
		/// <returns>The integer <paramref name="x"/> raised to the power of <paramref name="y"/>.</returns>
		public static T Pow<T>(T x, T y)
			where T : struct, IBinaryInteger<T>
		{
			T result = T.One;
			T i = T.Zero;

			if (T.IsNegative(i))
			{
				return result;
			}

			for (; i < y; i++)
			{
				result *= x;
			}

			return result;
		}

		/// <summary>
		/// Returns the greatest common divisor (GCD) of two numbers.
		/// </summary>
		/// <typeparam name="T">Number data type</typeparam>
		/// <param name="a">The first number.</param>
		/// <param name="b">The second number.</param>
		/// <returns>The greatest common divisor of <paramref name="a"/> and <paramref name="b"/>.</returns>
		public static T GreatestCommonDivisor<T>(T a, T b)
			where T : struct, INumber<T>
		{
			if (a == T.Zero)
			{
				return T.Abs(b);
			}

			while (b != T.Zero)
			{
				var remainer = a % b;
				a = b;
				b = remainer;
			}

			return T.Abs(a);
		}

		/// <summary>
		/// Returns the Fibonacci number at a given index.
		/// </summary>
		/// <param name="index">The index of the Fibonacci number to retrieve.</param>
		/// <returns>The Fibonacci number at index <paramref name="index"/></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int GetFibonacciNumber(int index)
		{
			return (int)((System.Math.Pow(GoldenRatio, index) - System.Math.Pow(1 - GoldenRatio, index)) / System.Math.Sqrt(5));
		}
		/// <summary>
		/// Returns an array containing a sequence of Fibonacci numbers up to a specified count.
		/// </summary>
		/// <param name="count">The number of Fibonacci numbers to generate in the sequence.</param>
		/// <returns>An array containing a sequence of Fibonacci numbers up to a specified <paramref name="count"/>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int[] GetFibonacciSequence(int count)
		{
			Span<int> output = stackalloc int[count];
			output[0] = 0;

			for (int i = 1; i < count; i++)
			{
				output[i] = GetFibonacciNumber(i);
			}

			return output.ToArray();
		}

		/// <summary>
		/// Returns the Lucas number at a given index.
		/// </summary>
		/// <param name="index">The index of the Lucas number to retrieve.</param>
		/// <returns>The Lucas number at index <paramref name="index"/></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int GetLucasNumber(int index)
		{
			return (int)((System.Math.Pow(GoldenRatio, index) + System.Math.Pow(-GoldenRatio, -index)));
		}
		/// <summary>
		/// Returns an array containing a sequence of Lucas numbers up to a specified count.
		/// </summary>
		/// <param name="count">The number of Lucas numbers to generate in the sequence.</param>
		/// <returns>An array containing a sequence of Lucas numbers up to a specified <paramref name="count"/>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int[] GetLucasSequence(int count)
		{
			Span<int> output = stackalloc int[count];
			output[0] = 0;

			for (int i = 1; i < count; i++)
			{
				output[i] = GetLucasNumber(i);
			}

			return output.ToArray();
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
		/// <summary>
		/// Produces the full product of two unsigned 256-bit numbers.
		/// </summary>
		/// <param name="left">First number to multiply.</param>
		/// <param name="right">Second number to multiply.</param>
		/// <param name="lower">The low 256-bit of the product of the specified numbers.</param>
		/// <returns>The high 256-bit of the product of the specified numbers.</returns>
		public static UInt256 BigMul(UInt256 left, UInt256 right, out UInt256 lower)
		{
			// Adaptation of algorithm for multiplication
			// of 32-bit unsigned integers described
			// in Hacker's Delight by Henry S. Warren, Jr. (ISBN 0-201-91465-4), Chapter 8
			// Basically, it's an optimized version of FOIL method applied to
			// low and high dwords of each operand

			UInt256 al = left.Lower;
			UInt256 ah = left.Upper;

			UInt256 bl = right.Lower;
			UInt256 bh = right.Upper;

			UInt256 mull = al * bl;
			UInt256 t = ah * bl + mull.Upper;
			UInt256 tl = al * bh + t.Lower;

			lower = new UInt256(tl.Lower, mull.Lower);

			return ah * bh + t.Upper + tl.Upper;
		}

		/// <summary>
		/// Produces the full product of two signed 256-bit numbers.
		/// </summary>
		/// <param name="left">First number to multiply.</param>
		/// <param name="right">Second number to multiply.</param>
		/// <param name="lower">The low 256-bit of the product of the specified numbers.</param>
		/// <returns>The high 256-bit of the product of the specified numbers.</returns>
		public static Int256 BigMul(Int256 left, Int256 right, out Int256 low)
		{
			// This follows the same logic as is used in `long Math.BigMul(long, long, out long)`

			UInt256 upper = BigMul((UInt256)left, (UInt256)right, out UInt256 ulower);
			low = (Int256)ulower;
			return (Int256)(upper) - ((left >> 255) & right) - ((right >> 255) & left);
		}
	}
}