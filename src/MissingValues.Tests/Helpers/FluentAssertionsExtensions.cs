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
		private class OctoAssertions : NumericAssertions<Octo>
		{
            internal OctoAssertions(Octo value)
				: base(value)
            {
            }
        }
		private class NullableOctoAssertions : NullableNumericAssertions<Octo>
		{
            internal NullableOctoAssertions(Octo? value)
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
		public static NumericAssertions<Octo> Should(this Octo actualValue)
		{
			return new OctoAssertions(actualValue);
		}
		public static NullableNumericAssertions<Octo> Should(this Octo? actualValue)
		{
			return new NullableOctoAssertions(actualValue);
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
				Quad.NaN, because, becauseArgs);
			}
			else if (Quad.IsNegativeInfinity(expectedValue))
			{
				FailIfDifferenceOutsidePrecision(Quad.IsNegativeInfinity(parent.Subject.Value), parent, expectedValue, precision,
				Quad.NaN, because, becauseArgs);
			}
			else
			{
				Quad actualDifference = Quad.Abs(expectedValue - parent.Subject.Value);

				FailIfDifferenceOutsidePrecision(actualDifference <= precision, parent, expectedValue, precision, actualDifference, because, becauseArgs);
			}

			return new AndConstraint<NumericAssertions<Quad>>(parent);
		}
		public static AndConstraint<NumericAssertions<Octo>> BeApproximately(this NumericAssertions<Octo> parent, Octo expectedValue, Octo precision, string because = "", params object[] becauseArgs)
		{
			if (Octo.IsNaN(expectedValue))
			{
				throw new ArgumentException("Cannot determine approximation of a Quad to NaN", nameof(expectedValue));
			}
			if (Octo.IsNegative(precision))
			{
				throw new ArgumentException("Cannot determine precision of a Quad if its negative", nameof(expectedValue));
			}

			if (Octo.IsPositiveInfinity(expectedValue))
			{
				FailIfDifferenceOutsidePrecision(Octo.IsPositiveInfinity(parent.Subject.Value), parent, expectedValue, precision,
				Octo.NaN, because, becauseArgs);
			}
			else if (Octo.IsNegativeInfinity(expectedValue))
			{
				FailIfDifferenceOutsidePrecision(Octo.IsNegativeInfinity(parent.Subject.Value), parent, expectedValue, precision,
				Octo.NaN, because, becauseArgs);
			}
			else
			{
				Octo actualDifference = Octo.Abs(expectedValue - parent.Subject.Value);

				FailIfDifferenceOutsidePrecision(actualDifference <= precision, parent, expectedValue, precision, actualDifference, because, becauseArgs);
			}

			return new AndConstraint<NumericAssertions<Octo>>(parent);
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
		public static AndConstraint<NumericAssertions<Octo>> BeNaN(this NumericAssertions<Octo> parent, string because = "", params object[] becauseArgs)
		{
			bool condition = Octo.IsNaN(parent.Subject!.Value);

			Execute.Assertion
			.ForCondition(condition)
			.BecauseOf(because, becauseArgs)
			.FailWith("Expected {context:object} to be {0}.", Octo.NaN);

			return new AndConstraint<NumericAssertions<Octo>>(parent);
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
		public static AndConstraint<NumericAssertions<Octo>> BeBitwiseEquivalentTo(this NumericAssertions<Octo, NumericAssertions<Octo>> assertions, Octo expected, string because = "", params object[] becauseArgs)
		{
			bool condition;

			Octo actualValue = assertions.Subject is null ? default : (Octo)assertions.Subject;

			if (Octo.IsNaN(actualValue) && Octo.IsNaN(expected))
			{
				condition = true;
			}
			else
			{
				condition = Equals(Octo.OctoToUInt256Bits(actualValue), Octo.OctoToUInt256Bits(expected));
			}

			Execute.Assertion
			.ForCondition(condition)
			.BecauseOf(because, becauseArgs)
			.FailWith("Expected {context:object} to be equal to {0}{reason}, but found {1}.", expected, assertions.Subject);

			return new AndConstraint<NumericAssertions<Octo>>((NumericAssertions<Octo>)assertions);
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
		public static AndConstraint<NumericAssertions<Octo>> NotBeBitwiseEquivalentTo(this NumericAssertions<Octo, NumericAssertions<Octo>> assertions, Octo expected, string because = "", params object[] becauseArgs)
		{
			bool condition;

			Octo actualValue = assertions.Subject is null ? default : (Octo)assertions.Subject;

			if (Octo.IsNaN(actualValue) && Octo.IsNaN(expected))
			{
				condition = true;
			}
			else
			{
				condition = Equals(Octo.OctoToUInt256Bits(actualValue), Octo.OctoToUInt256Bits(expected));
			}

			Execute.Assertion
			.ForCondition(!condition)
			.BecauseOf(because, becauseArgs)
			.FailWith("Expected {context:object} to be equal to {0}{reason}, but found {1}.", expected, assertions.Subject);

			return new AndConstraint<NumericAssertions<Octo>>((NumericAssertions<Octo>)assertions);
		}
	}
}
