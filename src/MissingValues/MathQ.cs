using MissingValues.Internals;
using System.Runtime.CompilerServices;

namespace MissingValues
{
	/// <summary>
	/// Provides constants and static methods for trigonometric, logarithmic, and other common mathematical functions.
	/// </summary>
	public static partial class MathQ
	{
		/// <summary>
		/// Produces the full product of two unsigned 128-bit numbers.
		/// </summary>
		/// <param name="a">The first number to multiply.</param>
		/// <param name="b">The second number to multiply.</param>
		/// <returns>The full product of the specified numbers.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static UInt256 BigMul(UInt128 a, UInt128 b)
		{
			UInt128 high = Calculator.BigMul(a, b, out var low);
			return new UInt256(high, low);
		}

		/// <summary>
		/// Produces the full product of two 128-bit numbers.
		/// </summary>
		/// <param name="a">The first number to multiply.</param>
		/// <param name="b">The second number to multiply.</param>
		/// <returns>The full product of the specified numbers.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Int256 BigMul(Int128 a, Int128 b)
		{
			UInt128 high = Calculator.BigMul((UInt128)a, (UInt128)b, out var low);
			return new Int256(high, low);
		}

		/// <summary>
		/// Produces the full product of two unsigned 256-bit numbers.
		/// </summary>
		/// <param name="a">The first number to multiply.</param>
		/// <param name="b">The second number to multiply.</param>
		/// <returns>The full product of the specified numbers.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static UInt512 BigMul(UInt256 a, UInt256 b)
		{
			UInt256 high = UInt256.BigMul(a, b, out var low);
			return new UInt512(high, low);
		}

		/// <summary>
		/// Produces the full product of two 256-bit numbers.
		/// </summary>
		/// <param name="a">The first number to multiply.</param>
		/// <param name="b">The second number to multiply.</param>
		/// <returns>The full product of the specified numbers.</returns>

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Int512 BigMul(Int256 a, Int256 b)
		{
			UInt256 high = UInt256.BigMul((UInt256)a, (UInt256)b, out var low);
			return new Int512(high, low);
		}
	}
}
