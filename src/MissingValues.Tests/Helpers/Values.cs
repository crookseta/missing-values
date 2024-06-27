using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues.Tests.Helpers
{
	internal static class Values
	{
		public static TFloat CreateFloat<TFloat>(params ulong[] bits)
			where TFloat : unmanaged, IBinaryFloatingPointIeee754<TFloat>
		{
			if (typeof(TFloat) == typeof(Quad) && bits.Length == 2)
			{
				return (TFloat)(object)Quad.UInt128BitsToQuad(new UInt128(bits[0], bits[1]));
			}
			else if (typeof(TFloat) == typeof(Octo) && bits.Length == 4)
			{
				return (TFloat)(object)Octo.UInt256BitsToOcto(new UInt256(bits[0], bits[1], bits[2], bits[3]));
			}
			else
			{
				throw new InvalidOperationException();
			}
		}
	}
}
