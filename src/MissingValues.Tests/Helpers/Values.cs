using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues.Tests.Helpers
{
	internal static class Values
	{
		public static Quad CreateQuad(ulong upper, ulong lower) => Quad.UInt128BitsToQuad(new UInt128(upper, lower));
	}
}
