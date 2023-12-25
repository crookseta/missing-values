using FluentAssertions.Execution;
using FluentAssertions.Numeric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues.Tests.Helpers
{
	public static class FluentAssertionsExtensions
	{
		private class QuadAssertions : NumericAssertions<Quad>
		{
            internal QuadAssertions(Quad value)
				: base(value)
            {
            }
        }
		private class NullableQuadAssertions : NullableNumericAssertions<Quad>
		{
            internal NullableQuadAssertions(Quad? value)
				: base(value)
            {
            }
        }

		public static NumericAssertions<Quad> Should(this Quad actualValue)
		{
			return new QuadAssertions(actualValue);
		}
		public static NullableNumericAssertions<Quad> Should(this Quad? actualValue)
		{
			return new NullableQuadAssertions(actualValue);
		}
		public static AndConstraint<NumericAssertions<Quad>> BeApproximately(this NumericAssertions<Quad> parent, Quad expectedValue, Quad precision, string because = "", params object[] becauseArgs)
		{
			if (Quad.IsNaN(expectedValue))
			{
				throw new ArgumentException("Cannot determine approximation of a Quad to NaN", nameof(expectedValue));
			}
			if (Quad.IsNegative(precision))
			{
				throw new ArgumentException("Cannot determine precision of a Quad if its negative", nameof(expectedValue));
			}

			if (Quad.IsPositiveInfinity(expectedValue))
			{
				FailIfDifferenceOutsidePrecision(Quad.IsPositiveInfinity(parent.Subject.Value), parent, expectedValue, precision,
				float.NaN, because, becauseArgs);
			}
			else if (Quad.IsNegativeInfinity(expectedValue))
			{
				FailIfDifferenceOutsidePrecision(Quad.IsNegativeInfinity(parent.Subject.Value), parent, expectedValue, precision,
				float.NaN, because, becauseArgs);
			}
			else
			{
				Quad actualDifference = Quad.Abs(expectedValue - parent.Subject.Value);

				FailIfDifferenceOutsidePrecision(actualDifference <= precision, parent, expectedValue, precision, actualDifference, because, becauseArgs);
			}

			return new AndConstraint<NumericAssertions<Quad>>(parent);
		}

		public static AndConstraint<NumericAssertions<Quad>> BeNaN(this NumericAssertions<Quad> parent, string because = "", params object[] becauseArgs)
		{
			bool condition = Quad.IsNaN(parent.Subject!.Value);

			Execute.Assertion
			.ForCondition(condition)
			.BecauseOf(because, becauseArgs)
			.FailWith("Expected {context:object} to be {0}.", Quad.NaN);

			return new AndConstraint<NumericAssertions<Quad>>(parent);
		}

		private static void FailIfDifferenceOutsidePrecision<T>(
			bool differenceWithinPrecision,
			NumericAssertions<T> parent, T expectedValue, T precision, T actualDifference,
			string because, object[] becauseArgs)
			where T : struct, IComparable<T>
		{
			Execute.Assertion
			.ForCondition(differenceWithinPrecision)
			.BecauseOf(because, becauseArgs)
			.FailWith("Expected {context:value} to approximate {1} +/- {2}{reason}, but {0} differed by {3}.",
				parent.Subject, expectedValue, precision, actualDifference);
		}

		public static AndConstraint<NumericAssertions<Quad>> BeBitwiseEquivalentTo(this NumericAssertions<Quad, NumericAssertions<Quad>> assertions, Quad expected, string because = "", params object[] becauseArgs)
		{
			bool condition;

			Quad actualValue = assertions.Subject is null ? default : (Quad)assertions.Subject;

			if (Quad.IsNaN(actualValue) && Quad.IsNaN(expected))
			{
				condition = true;
			}
			else
			{
				condition = Equals(Quad.QuadToUInt128Bits(actualValue), Quad.QuadToUInt128Bits(expected));
			}

			Execute.Assertion
			.ForCondition(condition)
			.BecauseOf(because, becauseArgs)
			.FailWith("Expected {context:object} to be equal to {0}{reason}, but found {1}.", expected, assertions.Subject);

			return new AndConstraint<NumericAssertions<Quad>>((NumericAssertions<Quad>)assertions);
		}
		public static AndConstraint<NumericAssertions<Quad>> NotBeBitwiseEquivalentTo(this NumericAssertions<Quad, NumericAssertions<Quad>> assertions, Quad expected, string because = "", params object[] becauseArgs)
		{
			bool condition;

			Quad actualValue = assertions.Subject is null ? default : (Quad)assertions.Subject;

			if (Quad.IsNaN(actualValue) && Quad.IsNaN(expected))
			{
				condition = true;
			}
			else
			{
				condition = Equals(Quad.QuadToUInt128Bits(actualValue), Quad.QuadToUInt128Bits(expected));
			}

			Execute.Assertion
			.ForCondition(!condition)
			.BecauseOf(because, becauseArgs)
			.FailWith("Expected {context:object} to be equal to {0}{reason}, but found {1}.", expected, assertions.Subject);

			return new AndConstraint<NumericAssertions<Quad>>((NumericAssertions<Quad>)assertions);
		}
	}
}
