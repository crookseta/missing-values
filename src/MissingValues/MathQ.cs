using MissingValues.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues
{
	/// <summary>
	/// Provides constants and static methods for trigonometric, logarithmic, and other common mathematical functions.
	/// </summary>
	public static partial class MathQ
	{
		public static int CountDigits<T>(T num, T numberBase)
			where T : struct, IBinaryInteger<T>
		{
			int count = 0;
			do
			{
				num /= numberBase;
				++count;
			}
			while (num != T.Zero);
			return count;
		}

		internal static T GetGoldenRatio<T>()
			where T : unmanaged, IFloatingPoint<T>
		{
			T ratio;

			if (typeof(T) == typeof(Half))
			{
				ratio = (T)(object)(Half)1.61803400516510009765625f;
			}
			else if (typeof(T) == typeof(float))
			{
				ratio = (T)(object)1.61803400516510009765625f;
			}
			else if (typeof(T) == typeof(double))
			{
				ratio = (T)(object)1.6180339887498949025257388711906969547271d;
			}
			else if (typeof(T) == typeof(decimal))
			{
				ratio = (T)(object)1.61803398874989484820458683436563811772030917980576286213544862270526046281890244970720720418939M;
			}
			else if (typeof(T) == typeof(Quad))
			{
				ratio = (T)(object)Quad.UInt128BitsToQuad(new UInt128(0x3FFF_9E37_79B9_7F4A, 0x7C15_F39C_C060_5CEE));
			}
			else
			{
				ratio = T.CreateChecked(1.61803400516510009765625);
			}

			return ratio;
		}

		public static T GreatestCommonDivisor<T>(T x, T y)
			where T : struct, IBinaryInteger<T>
		{
			if (T.IsZero(x))
			{
				return T.Abs(y);
			}

			while (!T.IsZero(y))
			{
				T remainder = x % y;
				x = y;
				y = remainder;
			}

			return T.Abs(x);
		}

		public static T Pow<T>(T x, T y) 
			where T : struct, IBinaryInteger<T>
		{
			T result = T.One;
			T i = T.Zero;

			if (T.IsNegative(i))
			{
				return i;
			}

			for (; i < y; i++)
			{
				result *= x;
			}

			return result;
		}
	}
}
