using System.Text;

namespace MissingValues.Tests;

public static class Values
{
	public static TFloat CreateFloat<TFloat>(params ReadOnlySpan<ulong> bits)
	{
		if (typeof(TFloat) == typeof(Quad) && bits.Length == 2)
		{
			return (TFloat)(object)Quad.UInt128BitsToQuad(new UInt128(bits[0], bits[1]));
		}
		if (typeof(TFloat) == typeof(Octo) && bits.Length == 4)
		{
			return (TFloat)(object)Octo.UInt256BitsToOcto(new UInt256(bits[0], bits[1], bits[2], bits[3]));
		}

		throw new InvalidOperationException($"{typeof(TFloat)} does not match the bits({bits.Length}): {StringifyBits(bits)}");

		static string StringifyBits(ReadOnlySpan<ulong> bits)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append($"0x{bits[0]:X16}");
			for (int i = 1; i < bits.Length; i++)
			{
				sb.Append($"_{bits[i]:X16}");
			}

			return sb.ToString();
		}
	}
}