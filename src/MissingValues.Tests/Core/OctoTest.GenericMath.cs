using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Float = MissingValues.Octo;

namespace MissingValues.Tests.Core
{
	public partial class OctoTest
	{
		#region Generic Math Operators
		[Theory]
		[MemberData(nameof(UnaryNegationOperationTheoryData))]
		public static void op_UnaryNegationTest(Float self, Float result)
		{
			MathOperatorsHelper.UnaryNegationOperation<Float, Float>(self)
				.Should().Be(result)
				.And.BeBitwiseEquivalentTo(result);
		}
		[Theory]
		[MemberData(nameof(AdditionOperationTheoryData))]
		public static void op_AdditionTest(Float left, Float right, Float result)
		{
			MathOperatorsHelper.AdditionOperation<Float, Float, Float>(left, right)
				.Should().Be(result)
				.And.BeBitwiseEquivalentTo(result);
		}
		[Theory]
		[MemberData(nameof(IncrementOperationTheoryData))]
		public static void op_IncrementTest(Float self, Float result)
		{
			MathOperatorsHelper.IncrementOperation<Float>(self)
				.Should().Be(result)
				.And.BeBitwiseEquivalentTo(result);
		}
		[Theory]
		[MemberData(nameof(SubtractionOperationTheoryData))]
		public static void op_SubtractionTest(Float left, Float right, Float result)
		{
			MathOperatorsHelper.SubtractionOperation<Float, Float, Float>(left, right)
				.Should().Be(result)
				.And.BeBitwiseEquivalentTo(result);
		}
		[Theory]
		[MemberData(nameof(DecrementOperationTheoryData))]
		public static void op_DecrementTest(Float self, Float result)
		{
			MathOperatorsHelper.DecrementOperation<Float>(self)
				.Should().Be(result)
				.And.BeBitwiseEquivalentTo(result);
		}
		[Theory]
		[MemberData(nameof(MultiplicationOperationTheoryData))]
		public static void op_MultiplicationTest(Float left, Float right, Float result)
		{
			MathOperatorsHelper.MultiplicationOperation<Float, Float, Float>(left, right)
				.Should().Be(result)
				.And.BeBitwiseEquivalentTo(result);
		}
		[Theory]
		[MemberData(nameof(DivisionOperationTheoryData))]
		public static void op_DivisionTest(Float left, Float right, Float result)
		{
			MathOperatorsHelper.DivisionOperation<Float, Float, Float>(left, right)
				.Should().Be(result)
				.And.BeBitwiseEquivalentTo(result);
		}
		#endregion

		#region IFloatingPointIEEE
		[Theory]
		[MemberData(nameof(FMATheoryData))]
		public static void FusedMultiplyAddTest(Float left, Float right, Float addend, Float result)
		{
			FloatingPointIeee754<Float>.FusedMultiplyAdd(left, right, addend)
				.Should().Be(result)
				.And.BeBitwiseEquivalentTo(result);
		}
		[Fact]
		public static void IeeeRemainderTest()
		{
			FloatingPointIeee754<Float>.Ieee754Remainder(Ten, Three).Should().Be(One);
			FloatingPointIeee754<Float>.Ieee754Remainder(Ten, Two).Should().Be(Zero);
			FloatingPointIeee754<Float>.Ieee754Remainder(NegativeTen, Three).Should().Be(NegativeOne);
			FloatingPointIeee754<Float>.Ieee754Remainder(NegativeTen, Two).Should().Be(NegativeZero);
			FloatingPointIeee754<Float>.Ieee754Remainder(NegativeTen, Zero).Should().Be(Float.NaN);
		}
		[Fact]
		public static void SqrtTest()
		{
			GenericFloatingPointFunctions.Sqrt(Zero)
				.Should().Be(Zero);
			GenericFloatingPointFunctions.Sqrt(-Zero)
				.Should().Be(-Zero);
			GenericFloatingPointFunctions.Sqrt(Float.PositiveInfinity)
				.Should().Be(Float.PositiveInfinity);
			GenericFloatingPointFunctions.Sqrt(NegativeFour)
				.Should().Be(Float.NaN);
			GenericFloatingPointFunctions.Sqrt(Float.NaN)
				.Should().Be(Float.NaN);

			GenericFloatingPointFunctions.Sqrt(Hundred)
				.Should().BeApproximately(Ten, Delta);
		}
		#endregion
	}
}
