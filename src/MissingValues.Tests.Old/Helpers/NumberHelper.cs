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
		/// <inheritdoc cref="INumber{TSelf}.Clamp(TSelf, TSelf, TSelf)"/>
		public static TSelf Clamp(TSelf value, TSelf min, TSelf max) => TSelf.Clamp(value, min, max);
		/// <inheritdoc cref="INumber{TSelf}.CopySign(TSelf, TSelf)"/>
		public static TSelf CopySign(TSelf value, TSelf sign) => TSelf.CopySign(value, sign);
		/// <inheritdoc cref="INumber{TSelf}.Max(TSelf, TSelf)"/>
		public static TSelf Max(TSelf x, TSelf y) => TSelf.Max(x, y);
		/// <inheritdoc cref="INumber{TSelf}.MaxNumber(TSelf, TSelf)"/>
		public static TSelf MaxNumber(TSelf x, TSelf y) => TSelf.MaxNumber(x, y);
		/// <inheritdoc cref="INumber{TSelf}.Min(TSelf, TSelf)"/>
		public static TSelf Min(TSelf x, TSelf y) => TSelf.Min(x, y);
		/// <inheritdoc cref="INumber{TSelf}.MinNumber(TSelf, TSelf)"/>
		public static TSelf MinNumber(TSelf x, TSelf y) => TSelf.MinNumber(x, y);
		/// <inheritdoc cref="INumber{TSelf}.Sign(TSelf)"/>
		public static int Sign(TSelf value) => TSelf.Sign(value);
	}
}
