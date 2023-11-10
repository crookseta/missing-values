using FluentAssertions.Execution;
using FluentAssertions.Numeric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues.Tests.Helpers
{
	public static class FluentAssertionsExtensions
	{
		public static AndConstraint<ComparableTypeAssertions<Quad>> BeBitwiseEquivalentTo(this ComparableTypeAssertions<Quad, ComparableTypeAssertions<Quad>> assertions, Quad expected, string because = "", params object[] becauseArgs)
		{
			bool condition;

			if (Quad.IsNaN((Quad)assertions.Subject) && Quad.IsNaN(expected))
			{
				condition = true;
			}
			else
			{
				condition = Equals(Quad.QuadToUInt128Bits((Quad)assertions.Subject), Quad.QuadToUInt128Bits(expected));
			}

			Execute.Assertion
			.ForCondition(condition)
			.BecauseOf(because, becauseArgs)
			.FailWith("Expected {context:object} to be equal to {0}{reason}, but found {1}.", expected, assertions.Subject);

			return new AndConstraint<ComparableTypeAssertions<Quad>>((ComparableTypeAssertions<Quad>)assertions);
		}
		public static AndConstraint<ComparableTypeAssertions<Quad>> NotBeBitwiseEquivalentTo(this ComparableTypeAssertions<Quad, ComparableTypeAssertions<Quad>> assertions, Quad expected, string because = "", params object[] becauseArgs)
		{
			bool condition;

			if (Quad.IsNaN((Quad)assertions.Subject) && Quad.IsNaN(expected))
			{
				condition = true;
			}
			else
			{
				condition = Equals(Quad.QuadToUInt128Bits((Quad)assertions.Subject), Quad.QuadToUInt128Bits(expected));
			}

			Execute.Assertion
			.ForCondition(!condition)
			.BecauseOf(because, becauseArgs)
			.FailWith("Expected {context:object} to be equal to {0}{reason}, but found {1}.", expected, assertions.Subject);

			return new AndConstraint<ComparableTypeAssertions<Quad>>((ComparableTypeAssertions<Quad>)assertions);
		}
	}
}
