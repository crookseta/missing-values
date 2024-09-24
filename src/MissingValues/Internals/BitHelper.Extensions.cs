using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues
{
	internal static partial class BitHelper
	{
		public static void GetUpperAndLowerBits(UInt128 value, out ulong upper, out ulong lower)
		{
			lower = value.GetLowerBits();
			upper = value.GetUpperBits();
		}
		public static void GetUpperAndLowerBits(Int128 value, out ulong upper, out ulong lower)
		{
			lower = value.GetLowerBits();
			upper = value.GetUpperBits();
		}

		public static ulong GetUpperBits(this in UInt128 value)
		{
			return unchecked((ulong)(value >> 64));
		}
		public static ulong GetUpperBits(this in Int128 value)
		{
			return unchecked((ulong)(value >> 64));
		}
		public static ulong GetLowerBits(this in UInt128 value)
		{
			return unchecked((ulong)(value));
		}
		public static ulong GetLowerBits(this in Int128 value)
		{
			return unchecked((ulong)(value));
		}

		internal static T DefaultConvert<T>(out bool result)
		{
			result = false;
			return default;
		}
	}
}
