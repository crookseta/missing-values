using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues.Tests.Helpers
{
	internal static class BinaryNumberHelper<TSelf>
		where TSelf : IBinaryNumber<TSelf>
	{
		public static TSelf AllBitsSet => TSelf.AllBitsSet;
		public static bool IsPow2(TSelf value) => TSelf.IsPow2(value);
		public static TSelf Log2(TSelf value) => TSelf.Log2(value);
	}
}
