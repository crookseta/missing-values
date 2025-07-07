using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues.Benchmarks.Helpers;
internal static class RandomExtensions
{
	public static T NextInteger<T>(this Random random)
		where T : unmanaged, IBinaryInteger<T>
	{
		Span<byte> bytes = stackalloc byte[Unsafe.SizeOf<T>()];
		random.NextBytes(bytes);

		return Unsafe.ReadUnaligned<T>(ref MemoryMarshal.GetReference(bytes));
	}
	public static T NextInteger<T>(this Random random, T max)
		where T : unmanaged, IBinaryInteger<T>
	{
		return NextInteger(random, T.Zero, max);
	}
	public static T NextInteger<T>(this Random random, T min, T max)
		where T : unmanaged, IBinaryInteger<T>
	{
		ArgumentOutOfRangeException.ThrowIfGreaterThan(min, max);

		T result = NextInteger<T>(random);
		T ten = T.CreateTruncating(10);

		if (result > max)
		{
			do
			{
				result /= ten;
			} while (result > max);
		}
		else if (result < min)
		{
			if (T.IsNegative(result) && T.IsPositive(min))
			{
				result = -(++result);
			}
			do
			{
				result *= ten;
			} while (result < max);
		}

		return result;
	}

	public static T NextFloat<T>(this Random random)
		where T : unmanaged, IBinaryFloatingPointIeee754<T>
	{
		Debug.Assert(Unsafe.SizeOf<T>() <= Unsafe.SizeOf<UInt512>());
		int significandBitLength = T.PositiveInfinity.GetSignificandBitLength();

		return T.CreateTruncating(NextInteger<UInt512>(random) >> (512 - significandBitLength)) * (T.One / T.CreateTruncating(UInt512.One << significandBitLength));
	}

	public static T[] NextIntegerArray<T>(this Random random, int length)
		where T : unmanaged, IBinaryInteger<T>
	{
		T[] result = new T[length];

		for (int i = 0; i < length; i++)
		{
			result[i] = random.NextInteger<T>();
		}

		return result;
	}
	public static T[] NextIntegerArray<T>(this Random random, int length, T max)
		where T : unmanaged, IBinaryInteger<T>
	{
		T[] result = new T[length];

		for (int i = 0; i < length; i++)
		{
			result[i] = random.NextInteger(max);
		}

		return result;
	}
	public static T[] NextIntegerArray<T>(this Random random, int length, T min, T max)
		where T : unmanaged, IBinaryInteger<T>
	{
		T[] result = new T[length];

		for (int i = 0; i < length; i++)
		{
			result[i] = random.NextInteger(min, max);
		}

		return result;
	}
}
