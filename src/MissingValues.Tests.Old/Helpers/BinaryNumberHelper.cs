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
		/// <inheritdoc cref="IBinaryNumber{TSelf}.AllBitsSet"/>
		public static TSelf AllBitsSet => TSelf.AllBitsSet;
		/// <inheritdoc cref="IBinaryNumber{TSelf}.IsPow2(TSelf)"/>
		public static bool IsPow2(TSelf value) => TSelf.IsPow2(value);
		/// <inheritdoc cref="IBinaryNumber{TSelf}.Log2(TSelf)"/>
		public static TSelf Log2(TSelf value) => TSelf.Log2(value);
	}
}
