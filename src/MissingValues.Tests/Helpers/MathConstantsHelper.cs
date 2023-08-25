using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues.Tests.Helpers
{
	internal static class MathConstantsHelper
	{
		/// <inheritdoc cref="IAdditiveIdentity{TSelf, TResult}.AdditiveIdentity"/>
		public static TResult AdditiveIdentityHelper<TSelf, TResult>()
			where TSelf : IAdditiveIdentity<TSelf, TResult>
		{
			return TSelf.AdditiveIdentity;
		}
		/// <inheritdoc cref="IMultiplicativeIdentity{TSelf, TResult}.MultiplicativeIdentity"/>
		public static TResult MultiplicativeIdentityHelper<TSelf, TResult>()
			where TSelf : IMultiplicativeIdentity<TSelf, TResult>
		{
			return TSelf.MultiplicativeIdentity;
		}
		/// <inheritdoc cref="IMinMaxValue{TSelf}.MaxValue"/>
		public static TSelf MaxValue<TSelf>()
			where TSelf : IMinMaxValue<TSelf>
		{
			return TSelf.MaxValue;
		}
		/// <inheritdoc cref="IMinMaxValue{TSelf}.MinValue"/>
		public static TSelf MinValue<TSelf>()
			where TSelf : IMinMaxValue<TSelf>
		{
			return TSelf.MinValue;
		}
		/// <inheritdoc cref="ISignedNumber{TSelf}.NegativeOne"/>
		public static TSelf NegativeOne<TSelf>()
			where TSelf : ISignedNumber<TSelf>
		{
			return TSelf.NegativeOne;
		}
	}
}
