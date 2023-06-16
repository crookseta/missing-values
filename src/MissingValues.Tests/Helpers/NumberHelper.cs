using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues.Tests.Helpers
{
	internal static class NumberHelper<TSelf>
		where TSelf : INumber<TSelf>
	{
		public static TSelf Clamp(TSelf value, TSelf min, TSelf max) => TSelf.Clamp(value, min, max);
		public static TSelf CopySign(TSelf value, TSelf sign) => TSelf.CopySign(value, sign);
		public static TSelf Max(TSelf x, TSelf y) => TSelf.Max(x, y);
		public static TSelf MaxNumber(TSelf x, TSelf y) => TSelf.MaxNumber(x, y);
		public static TSelf Min(TSelf x, TSelf y) => TSelf.Min(x, y);
		public static TSelf MinNumber(TSelf x, TSelf y) => TSelf.MinNumber(x, y);
		public static int Sign(TSelf value) => TSelf.Sign(value);
	}
}
