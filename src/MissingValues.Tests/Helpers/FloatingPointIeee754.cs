using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues.Tests.Helpers
{
	internal static class FloatingPointIeee754<TSelf>
		where TSelf : IFloatingPointIeee754<TSelf>
	{
		public static TSelf Epsilon => TSelf.Epsilon;
		public static TSelf NaN => TSelf.NaN;
		public static TSelf NegativeInfinity => TSelf.NegativeInfinity;
		public static TSelf NegativeZero => TSelf.NegativeZero;
		public static TSelf PositiveInfinity => TSelf.PositiveInfinity;
		public static TSelf Atan2(TSelf y, TSelf x) => TSelf.Atan2(y, x);
		public static TSelf Atan2Pi(TSelf y, TSelf x) => TSelf.Atan2Pi(y, x);
		public static TSelf BitDecrement(TSelf x) => TSelf.BitDecrement(x);
		public static TSelf BitIncrement(TSelf x) => TSelf.BitIncrement(x);
		public static TSelf FusedMultiplyAdd(TSelf left, TSelf right, TSelf addend) => TSelf.FusedMultiplyAdd(left, right, addend);
		public static TSelf Ieee754Remainder(TSelf left, TSelf right) => TSelf.Ieee754Remainder(left, right);
		public static int ILogB(TSelf x) => TSelf.ILogB(x);
		public static TSelf ReciprocalEstimate(TSelf x) => TSelf.ReciprocalEstimate(x);
		public static TSelf ReciprocalSqrtEstimate(TSelf x) => TSelf.ReciprocalSqrtEstimate(x);
		public static TSelf ScaleB(TSelf x, int n) => TSelf.ScaleB(x, n);
	}
}
