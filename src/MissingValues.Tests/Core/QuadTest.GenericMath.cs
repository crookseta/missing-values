using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Float = MissingValues.Quad;

namespace MissingValues.Tests.Core
{
	public partial class QuadTest
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
		[Theory]
		[MemberData(nameof(GreaterThanTheoryData))]
		public static void op_GreaterThanTest(Float left, Float right, bool result)
		{
			ComparisonOperatorsHelper<Float, Float, bool>.GreaterThanOperation(left, right)
				.Should().Be(result);
		}
		[Theory]
		[MemberData(nameof(GreaterThanTheoryData))]
		[MemberData(nameof(EqualToTheoryData))]
		public static void op_GreaterThanOrEqualTest(Float left, Float right, bool result)
		{
			ComparisonOperatorsHelper<Float, Float, bool>.GreaterThanOrEqualOperation(left, right)
				.Should().Be(result);
		}
		[Theory]
		[MemberData(nameof(LessThanTheoryData))]
		public static void op_LessThanTest(Float left, Float right, bool result)
		{
			ComparisonOperatorsHelper<Float, Float, bool>.LessThanOperation(left, right)
				.Should().Be(result);
		}
		[Theory]
		[MemberData(nameof(LessThanTheoryData))]
		[MemberData(nameof(EqualToTheoryData))]
		public static void op_LessThanOrEqualTest(Float left, Float right, bool result)
		{
			ComparisonOperatorsHelper<Float, Float, bool>.LessThanOrEqualOperation(left, right)
				.Should().Be(result);
		}
		[Theory]
		[MemberData(nameof(EqualToTheoryData))]
		public static void op_EqualToTest(Float left, Float right, bool result)
		{
			EqualityOperatorsHelper<Float, Float, bool>.EqualityOperation(left, right)
				.Should().Be(result);
		}
		[Theory]
		[MemberData(nameof(NotEqualToTheoryData))]
		public static void op_NotEqualToTest(Float left, Float right, bool result)
		{
			EqualityOperatorsHelper<Float, Float, bool>.InequalityOperation(left, right)
				.Should().Be(result);
		}
		#endregion

		#region Identities
		[Fact]
		public static void AdditiveIdentityTest()
		{
			Assert.Equal(Zero, MathConstantsHelper.AdditiveIdentityHelper<Float, Float>());
		}

		[Fact]
		public static void MultiplicativeIdentityTest()
		{
			Assert.Equal(One, MathConstantsHelper.MultiplicativeIdentityHelper<Float, Float>());
		}
		#endregion

		#region IFloatingPoint
		[Theory]
		[MemberData(nameof(RoundAwayFromZeroTheoryData))]
		[MemberData(nameof(RoundToEvenTheoryData))]
		[MemberData(nameof(RoundToNegativeInfinityTheoryData))]
		[MemberData(nameof(RoundToPositiveInfinityTheoryData))]
		[MemberData(nameof(RoundToZeroTheoryData))]
		public static void RoundTest(Float self, int digits, MidpointRounding midpointRounding, Float result)
		{
			Float.Round(self, digits, midpointRounding).Should().Be(result);
		}
		#endregion

		#region IFloatingPointConstants
		[Fact]
		public static void ConstantPiTest()
		{
			Assert.Equal(Float.Pi, MathConstantsHelper.Pi<Float>());
		}
		[Fact]
		public static void ConstantTauTest()
		{
			Assert.Equal(Float.Tau, MathConstantsHelper.Tau<Float>());
		}
		[Fact]
		public static void ConstantETest()
		{
			Assert.Equal(Float.E, MathConstantsHelper.E<Float>());
		}
		#endregion

		#region IMinMaxValue
		[Fact]
		public static void MaxValueTest()
		{
			MaxValue.Should().Be(MathConstantsHelper.MaxValue<Float>());
		}

		[Fact]
		public static void MinValueTest()
		{
			MinValue.Should().Be(MathConstantsHelper.MinValue<Float>());
		}
		#endregion

		#region ISignedNumber
		[Fact]
		public static void NegativeOneTest()
		{
			MathConstantsHelper.NegativeOne<Float>().Should().Be(NegativeOne);
		}
		#endregion

		#region INumber
		[Fact]
		public static void ClampTest()
		{
			NumberHelper<Float>.Clamp(Float.NegativeInfinity, One, Thousand)
				.Should().Be(One);
			NumberHelper<Float>.Clamp(Float.MinValue, One, Thousand)
				.Should().Be(One);
			NumberHelper<Float>.Clamp(NegativeOne, One, Thousand)
				.Should().Be(One);
			NumberHelper<Float>.Clamp(-GreatestSubnormal, One, Thousand)
				.Should().Be(One);
			NumberHelper<Float>.Clamp(-Float.Epsilon, One, Thousand)
				.Should().Be(One);
			NumberHelper<Float>.Clamp(NegativeZero, One, Thousand)
				.Should().Be(One);
			NumberHelper<Float>.Clamp(Float.NaN, One, Thousand)
				.Should().Be(Float.NaN);
			NumberHelper<Float>.Clamp(Zero, One, Thousand)
				.Should().Be(One);
			NumberHelper<Float>.Clamp(Float.Epsilon, One, Thousand)
				.Should().Be(One);
			NumberHelper<Float>.Clamp(GreatestSubnormal, One, Thousand)
				.Should().Be(One);
			NumberHelper<Float>.Clamp(One, One, Thousand)
				.Should().Be(One);
			NumberHelper<Float>.Clamp(Float.MaxValue, One, Thousand)
				.Should().Be(Thousand);
			NumberHelper<Float>.Clamp(Float.PositiveInfinity, One, Thousand)
				.Should().Be(Thousand);
		}
		[Fact]
		public static void CopySignTest()
		{
			NumberHelper<Float>.CopySign(One, NegativeOne)
				.Should().Be(NegativeOne);
			NumberHelper<Float>.CopySign(NegativeOne, One)
				.Should().Be(One);
			NumberHelper<Float>.CopySign(Thousand, NegativeOne)
				.Should().Be(NegativeThousand);
			NumberHelper<Float>.CopySign(NegativeHundred, NegativeOne)
				.Should().Be(NegativeHundred);
		}
		[Fact]
		public static void MaxTest()
		{
			NumberHelper<Float>.Max(Float.NegativeInfinity, One)
				.Should().Be(One);
			NumberHelper<Float>.Max(Float.MinValue, One)
				.Should().Be(One);
			NumberHelper<Float>.Max(NegativeOne, One)
				.Should().Be(One);
			NumberHelper<Float>.Max(-GreatestSubnormal, One)
				.Should().Be(One);
			NumberHelper<Float>.Max(-Float.Epsilon, One)
				.Should().Be(One);
			NumberHelper<Float>.Max(NegativeZero, One)
				.Should().Be(One);
			NumberHelper<Float>.Max(Float.NaN, One)
				.Should().Be(Float.NaN);
			NumberHelper<Float>.Max(Zero, One)
				.Should().Be(One);
			NumberHelper<Float>.Max(Float.Epsilon, One)
				.Should().Be(One);
			NumberHelper<Float>.Max(GreatestSubnormal, One)
				.Should().Be(One);
			NumberHelper<Float>.Max(One, One)
				.Should().Be(One);
			NumberHelper<Float>.Max(Float.MaxValue, One)
				.Should().Be(Float.MaxValue);
			NumberHelper<Float>.Max(Float.PositiveInfinity, One)
				.Should().Be(Float.PositiveInfinity);
		}
		[Fact]
		public static void MaxNumberTest()
		{
			NumberHelper<Float>.MaxNumber(Float.NegativeInfinity, One)
				.Should().Be(One);
			NumberHelper<Float>.MaxNumber(Float.MinValue, One)
				.Should().Be(One);
			NumberHelper<Float>.MaxNumber(NegativeOne, One)
				.Should().Be(One);
			NumberHelper<Float>.MaxNumber(-GreatestSubnormal, One)
				.Should().Be(One);
			NumberHelper<Float>.MaxNumber(-Float.Epsilon, One)
				.Should().Be(One);
			NumberHelper<Float>.MaxNumber(NegativeZero, One)
				.Should().Be(One);
			NumberHelper<Float>.MaxNumber(Float.NaN, One)
				.Should().Be(One);
			NumberHelper<Float>.MaxNumber(Zero, One)
				.Should().Be(One);
			NumberHelper<Float>.MaxNumber(Float.Epsilon, One)
				.Should().Be(One);
			NumberHelper<Float>.MaxNumber(GreatestSubnormal, One)
				.Should().Be(One);
			NumberHelper<Float>.MaxNumber(One, One)
				.Should().Be(One);
			NumberHelper<Float>.MaxNumber(Float.MaxValue, One)
				.Should().Be(Float.MaxValue);
			NumberHelper<Float>.MaxNumber(Float.PositiveInfinity, One)
				.Should().Be(Float.PositiveInfinity);
		}
		[Fact]
		public static void MinTest()
		{
			NumberHelper<Float>.Min(Float.NegativeInfinity, One)
				.Should().Be(Float.NegativeInfinity);
			NumberHelper<Float>.Min(Float.MinValue, One)
				.Should().Be(Float.MinValue);
			NumberHelper<Float>.Min(NegativeOne, One)
				.Should().Be(NegativeOne);
			NumberHelper<Float>.Min(-GreatestSubnormal, One)
				.Should().Be(-GreatestSubnormal);
			NumberHelper<Float>.Min(-Float.Epsilon, One)
				.Should().Be(-Float.Epsilon);
			NumberHelper<Float>.Min(NegativeZero, One)
				.Should().Be(NegativeZero);
			NumberHelper<Float>.Min(Float.NaN, One)
				.Should().Be(Float.NaN);
			NumberHelper<Float>.Min(Zero, One)
				.Should().Be(Zero);
			NumberHelper<Float>.Min(Float.Epsilon, One)
				.Should().Be(Float.Epsilon);
			NumberHelper<Float>.Min(GreatestSubnormal, One)
				.Should().Be(GreatestSubnormal);
			NumberHelper<Float>.Min(One, One)
				.Should().Be(One);
			NumberHelper<Float>.Min(Float.MaxValue, One)
				.Should().Be(One);
			NumberHelper<Float>.Min(Float.PositiveInfinity, One)
				.Should().Be(One);
		}
		[Fact]
		public static void MinNumberTest()
		{
			NumberHelper<Float>.MinNumber(Float.NegativeInfinity, One)
				.Should().Be(Float.NegativeInfinity);
			NumberHelper<Float>.MinNumber(Float.MinValue, One)
				.Should().Be(Float.MinValue);
			NumberHelper<Float>.MinNumber(NegativeOne, One)
				.Should().Be(NegativeOne);
			NumberHelper<Float>.MinNumber(-GreatestSubnormal, One)
				.Should().Be(-GreatestSubnormal);
			NumberHelper<Float>.MinNumber(-Float.Epsilon, One)
				.Should().Be(-Float.Epsilon);
			NumberHelper<Float>.MinNumber(NegativeZero, One)
				.Should().Be(NegativeZero);
			NumberHelper<Float>.MinNumber(Float.NaN, One)
				.Should().Be(One);
			NumberHelper<Float>.MinNumber(Zero, One)
				.Should().Be(Zero);
			NumberHelper<Float>.MinNumber(Float.Epsilon, One)
				.Should().Be(Float.Epsilon);
			NumberHelper<Float>.MinNumber(GreatestSubnormal, One)
				.Should().Be(GreatestSubnormal);
			NumberHelper<Float>.MinNumber(One, One)
				.Should().Be(One);
			NumberHelper<Float>.MinNumber(Float.MaxValue, One)
				.Should().Be(One);
			NumberHelper<Float>.MinNumber(Float.PositiveInfinity, One)
				.Should().Be(One);
		}
		[Fact]
		public static void SignTest()
		{
			NumberHelper<Float>.Sign(One)
				.Should().Be(1);
			NumberHelper<Float>.Sign(NegativeOne)
				.Should().Be(-1);
			NumberHelper<Float>.Sign(Ten)
				.Should().Be(1);
			NumberHelper<Float>.Sign(NegativeTen)
				.Should().Be(-1);
			NumberHelper<Float>.Sign(Zero)
				.Should().Be(0);
			NumberHelper<Float>.Sign(NegativeZero)
				.Should().Be(0);
		}
		#endregion

		#region INumberBase
		[Fact]
		public static void OneTest()
		{
			Assert.Equal(One, NumberBaseHelper<Float>.One);
		}
		[Fact]
		public static void ZeroTest()
		{
			Assert.Equal(Zero, NumberBaseHelper<Float>.Zero);
		}
		[Fact]
		public static void RadixTest()
		{
			Assert.Equal(Radix, NumberBaseHelper<Float>.Radix);
		}
		[Fact]
		public static void AbsTest()
		{
			NumberBaseHelper<Float>.Abs(One).Should().Be(One);
			NumberBaseHelper<Float>.Abs(NegativeOne).Should().Be(One);
			NumberBaseHelper<Float>.Abs(NegativeHalf).Should().Be(Half);
			NumberBaseHelper<Float>.Abs(NegativeQuarter).Should().Be(Quarter);
			NumberBaseHelper<Float>.Abs(NegativeZero).Should().Be(Zero);
			NumberBaseHelper<Float>.Abs(Float.NegativeInfinity).Should().Be(Float.PositiveInfinity);
		}
		[Fact]
		public static void CreateCheckedFromQuadTest()
		{
			NumberBaseHelper<byte>.CreateChecked<Float>(ByteMaxValue).Should().Be(byte.MaxValue);
			NumberBaseHelper<short>.CreateChecked<Float>(Int16MaxValue).Should().Be(short.MaxValue);
			NumberBaseHelper<int>.CreateChecked<Float>(Int32MaxValue).Should().Be(int.MaxValue);
			NumberBaseHelper<long>.CreateChecked<Float>(Int64MaxValue).Should().Be(long.MaxValue);
			NumberBaseHelper<UInt128>.CreateChecked<Float>(UInt128MaxValue).Should().Be(UInt128.MaxValue);
			NumberBaseHelper<UInt256>.CreateChecked<Float>(UInt256MaxValue).Should().Be(UInt256.MaxValue);
			NumberBaseHelper<UInt512>.CreateChecked<Float>(UInt512MaxValue).Should().Be(UInt512.MaxValue);

			NumberBaseHelper<Half>.CreateChecked<Float>(Half).Should().Be((Half)0.5f);
			NumberBaseHelper<float>.CreateChecked<Float>(Half).Should().Be(0.5f);
			NumberBaseHelper<double>.CreateChecked<Float>(Half).Should().Be(0.5d);
		}
		[Fact]
		public static void CreateSaturatingFromQuadTest()
		{
			NumberBaseHelper<byte>.CreateSaturating<Float>(ByteMaxValue).Should().Be(byte.MaxValue);
			NumberBaseHelper<short>.CreateSaturating<Float>(Int16MaxValue).Should().Be(short.MaxValue);
			NumberBaseHelper<int>.CreateSaturating<Float>(Int32MaxValue).Should().Be(int.MaxValue);
			NumberBaseHelper<long>.CreateSaturating<Float>(Int64MaxValue).Should().Be(long.MaxValue);
			NumberBaseHelper<UInt128>.CreateSaturating<Float>(UInt128MaxValue).Should().Be(UInt128.MaxValue);
			NumberBaseHelper<UInt256>.CreateSaturating<Float>(UInt256MaxValue).Should().Be(UInt256.MaxValue);
			NumberBaseHelper<UInt512>.CreateSaturating<Float>(UInt512MaxValue).Should().Be(UInt512.MaxValue);

			NumberBaseHelper<Half>.CreateSaturating<Float>(Half).Should().Be((Half)0.5f);
			NumberBaseHelper<float>.CreateSaturating<Float>(Half).Should().Be(0.5f);
			NumberBaseHelper<double>.CreateSaturating<Float>(Half).Should().Be(0.5d);
		}
		[Fact]
		public static void CreateTruncatingFromQuadTest()
		{
			NumberBaseHelper<byte>.CreateTruncating<Float>(ByteMaxValue).Should().Be(byte.MaxValue);
			NumberBaseHelper<short>.CreateTruncating<Float>(Int16MaxValue).Should().Be(short.MaxValue);
			NumberBaseHelper<int>.CreateTruncating<Float>(Int32MaxValue).Should().Be(int.MaxValue);
			NumberBaseHelper<long>.CreateTruncating<Float>(Int64MaxValue).Should().Be(long.MaxValue);
			NumberBaseHelper<UInt128>.CreateTruncating<Float>(UInt128MaxValue).Should().Be(UInt128.MaxValue);
			NumberBaseHelper<UInt256>.CreateTruncating<Float>(UInt256MaxValue).Should().Be(UInt256.MaxValue);
			NumberBaseHelper<UInt512>.CreateTruncating<Float>(UInt512MaxValue).Should().Be(UInt512.MaxValue);

			NumberBaseHelper<Half>.CreateTruncating<Float>(Half).Should().Be((Half)0.5f);
			NumberBaseHelper<float>.CreateTruncating<Float>(Half).Should().Be(0.5f);
			NumberBaseHelper<double>.CreateTruncating<Float>(Half).Should().Be(0.5d);
		}
		[Fact]
		public static void CreateCheckedToQuadTest()
		{
			NumberBaseHelper<Float>.CreateChecked<byte>(byte.MaxValue).Should().Be(ByteMaxValue);
			NumberBaseHelper<Float>.CreateChecked<short>(short.MaxValue).Should().Be(Int16MaxValue);
			NumberBaseHelper<Float>.CreateChecked<int>(int.MaxValue).Should().Be(Int32MaxValue);
			NumberBaseHelper<Float>.CreateChecked<long>(long.MaxValue).Should().Be(Int64MaxValue);
			NumberBaseHelper<Float>.CreateChecked<UInt128>(UInt128.MaxValue).Should().Be(UInt128MaxValue);
			NumberBaseHelper<Float>.CreateChecked<UInt256>(UInt256.MaxValue).Should().Be(UInt256MaxValue);
			NumberBaseHelper<Float>.CreateChecked<UInt512>(UInt512.MaxValue).Should().Be(UInt512MaxValue);

			NumberBaseHelper<Float>.CreateChecked<Half>((Half)0.5f).Should().Be(Half);
			NumberBaseHelper<Float>.CreateChecked<float>(0.5f).Should().Be(Half);
			NumberBaseHelper<Float>.CreateChecked<double>(0.5d).Should().Be(Half);
		}
		[Fact]
		public static void CreateSaturatingToQuadTest()
		{
			NumberBaseHelper<Float>.CreateChecked<byte>(byte.MaxValue).Should().Be(ByteMaxValue);
			NumberBaseHelper<Float>.CreateChecked<short>(short.MaxValue).Should().Be(Int16MaxValue);
			NumberBaseHelper<Float>.CreateChecked<int>(int.MaxValue).Should().Be(Int32MaxValue);
			NumberBaseHelper<Float>.CreateChecked<long>(long.MaxValue).Should().Be(Int64MaxValue);
			NumberBaseHelper<Float>.CreateChecked<UInt128>(UInt128.MaxValue).Should().Be(UInt128MaxValue);
			NumberBaseHelper<Float>.CreateChecked<UInt256>(UInt256.MaxValue).Should().Be(UInt256MaxValue);
			NumberBaseHelper<Float>.CreateChecked<UInt512>(UInt512.MaxValue).Should().Be(UInt512MaxValue);

			NumberBaseHelper<Float>.CreateChecked<Half>((Half)0.5f).Should().Be(Half);
			NumberBaseHelper<Float>.CreateChecked<float>(0.5f).Should().Be(Half);
			NumberBaseHelper<Float>.CreateChecked<double>(0.5d).Should().Be(Half);
		}
		[Fact]
		public static void CreateTruncatingToQuadTest()
		{
			NumberBaseHelper<Float>.CreateChecked<byte>(byte.MaxValue).Should().Be(ByteMaxValue);
			NumberBaseHelper<Float>.CreateChecked<short>(short.MaxValue).Should().Be(Int16MaxValue);
			NumberBaseHelper<Float>.CreateChecked<int>(int.MaxValue).Should().Be(Int32MaxValue);
			NumberBaseHelper<Float>.CreateChecked<long>(long.MaxValue).Should().Be(Int64MaxValue);
			NumberBaseHelper<Float>.CreateChecked<UInt128>(UInt128.MaxValue).Should().Be(UInt128MaxValue);
			NumberBaseHelper<Float>.CreateChecked<UInt256>(UInt256.MaxValue).Should().Be(UInt256MaxValue);
			NumberBaseHelper<Float>.CreateChecked<UInt512>(UInt512.MaxValue).Should().Be(UInt512MaxValue);

			NumberBaseHelper<Float>.CreateChecked<Half>((Half)0.5f).Should().Be(Half);
			NumberBaseHelper<Float>.CreateChecked<float>(0.5f).Should().Be(Half);
			NumberBaseHelper<Float>.CreateChecked<double>(0.5d).Should().Be(Half);
		}
		[Fact]
		public static void IsCanonicalTest()
		{
			NumberBaseHelper<Float>.IsCanonical(One).Should().BeTrue();
		}
		[Fact]
		public static void IsComplexNumberTest()
		{
			NumberBaseHelper<Float>.IsComplexNumber(One).Should().BeFalse();
		}
		[Fact]
		public static void IsEvenIntegerTest()
		{
			NumberBaseHelper<Float>.IsEvenInteger(Half).Should().BeFalse();
			NumberBaseHelper<Float>.IsEvenInteger(One).Should().BeFalse();
			NumberBaseHelper<Float>.IsEvenInteger(Two).Should().BeTrue();
			NumberBaseHelper<Float>.IsEvenInteger(Three).Should().BeFalse();
			NumberBaseHelper<Float>.IsEvenInteger(Four).Should().BeTrue();
			NumberBaseHelper<Float>.IsEvenInteger(NegativeOne).Should().BeFalse();
			NumberBaseHelper<Float>.IsEvenInteger(NegativeTwo).Should().BeTrue();
			NumberBaseHelper<Float>.IsEvenInteger(NegativeThree).Should().BeFalse();
			NumberBaseHelper<Float>.IsEvenInteger(NegativeFour).Should().BeTrue();
		}
		[Fact]
		public static void IsFiniteTest()
		{
			NumberBaseHelper<Float>.IsFinite(One).Should().BeTrue();
			NumberBaseHelper<Float>.IsFinite(NegativeOne).Should().BeTrue();
			NumberBaseHelper<Float>.IsFinite(Float.NaN).Should().BeFalse();
			NumberBaseHelper<Float>.IsFinite(Float.PositiveInfinity).Should().BeFalse();
			NumberBaseHelper<Float>.IsFinite(Float.NegativeInfinity).Should().BeFalse();
		}
		[Fact]
		public static void IsImaginaryNumberTest()
		{
			NumberBaseHelper<Float>.IsImaginaryNumber(One).Should().BeFalse();
		}
		[Fact]
		public static void IsInfinityTest()
		{
			NumberBaseHelper<Float>.IsInfinity(One).Should().BeFalse();
			NumberBaseHelper<Float>.IsInfinity(NegativeOne).Should().BeFalse();
			NumberBaseHelper<Float>.IsInfinity(Float.NaN).Should().BeFalse();
			NumberBaseHelper<Float>.IsInfinity(Float.PositiveInfinity).Should().BeTrue();
			NumberBaseHelper<Float>.IsInfinity(Float.NegativeInfinity).Should().BeTrue();
		}
		[Fact]
		public static void IsIntegerTest()
		{
			NumberBaseHelper<Float>.IsInteger(Quarter).Should().BeFalse();
			NumberBaseHelper<Float>.IsInteger(Half).Should().BeFalse();
			NumberBaseHelper<Float>.IsInteger(Thousand).Should().BeTrue();
			NumberBaseHelper<Float>.IsInteger(One).Should().BeTrue();
			NumberBaseHelper<Float>.IsInteger(GreaterThanOneSmallest).Should().BeFalse();
			NumberBaseHelper<Float>.IsInteger(SmallestSubnormal).Should().BeFalse();
			NumberBaseHelper<Float>.IsInteger(NegativeOne).Should().BeTrue();
			NumberBaseHelper<Float>.IsInteger(NegativeThousand).Should().BeTrue();
			NumberBaseHelper<Float>.IsInteger(NegativeHalf).Should().BeFalse();
			NumberBaseHelper<Float>.IsInteger(NegativeQuarter).Should().BeFalse();
			NumberBaseHelper<Float>.IsInteger(Float.NaN).Should().BeFalse();
			NumberBaseHelper<Float>.IsInteger(Float.PositiveInfinity).Should().BeFalse();
			NumberBaseHelper<Float>.IsInteger(Float.NegativeInfinity).Should().BeFalse();
		}
		[Fact]
		public static void IsNaNTest()
		{
			NumberBaseHelper<Float>.IsNaN(One).Should().BeFalse();
			NumberBaseHelper<Float>.IsNaN(NegativeOne).Should().BeFalse();
			NumberBaseHelper<Float>.IsNaN(Float.NaN).Should().BeTrue();
			NumberBaseHelper<Float>.IsNaN(Float.PositiveInfinity).Should().BeFalse();
			NumberBaseHelper<Float>.IsNaN(Float.NegativeInfinity).Should().BeFalse();
		}
		[Fact]
		public static void IsNegativeTest()
		{
			NumberBaseHelper<Float>.IsNegative(One).Should().BeFalse();
			NumberBaseHelper<Float>.IsNegative(GreatestSubnormal).Should().BeFalse();
			NumberBaseHelper<Float>.IsNegative(Float.PositiveInfinity).Should().BeFalse();
			NumberBaseHelper<Float>.IsNegative(Float.NaN).Should().BeTrue();
			NumberBaseHelper<Float>.IsNegative(NegativeOne).Should().BeTrue();
			NumberBaseHelper<Float>.IsNegative(Float.NegativeInfinity).Should().BeTrue();
		}
		[Fact]
		public static void IsNegativeInfinityTest()
		{
			NumberBaseHelper<Float>.IsNegativeInfinity(One).Should().BeFalse();
			NumberBaseHelper<Float>.IsNegativeInfinity(NegativeOne).Should().BeFalse();
			NumberBaseHelper<Float>.IsNegativeInfinity(Float.NaN).Should().BeFalse();
			NumberBaseHelper<Float>.IsNegativeInfinity(Float.PositiveInfinity).Should().BeFalse();
			NumberBaseHelper<Float>.IsNegativeInfinity(Float.NegativeInfinity).Should().BeTrue();
		}
		[Fact]
		public static void IsNormalTest()
		{
			NumberBaseHelper<Float>.IsNormal(GreatestSubnormal).Should().BeFalse();
			NumberBaseHelper<Float>.IsNormal(SmallestSubnormal).Should().BeFalse();
			NumberBaseHelper<Float>.IsNormal(MaxValue).Should().BeTrue();
			NumberBaseHelper<Float>.IsNormal(MinValue).Should().BeTrue();
			NumberBaseHelper<Float>.IsNormal(One).Should().BeTrue();
		}
		[Fact]
		public static void IsOddIntegerTest()
		{
			NumberBaseHelper<Float>.IsOddInteger(Half).Should().BeFalse();
			NumberBaseHelper<Float>.IsOddInteger(One).Should().BeTrue();
			NumberBaseHelper<Float>.IsOddInteger(Two).Should().BeFalse();
			NumberBaseHelper<Float>.IsOddInteger(Three).Should().BeTrue();
			NumberBaseHelper<Float>.IsOddInteger(Four).Should().BeFalse();
			NumberBaseHelper<Float>.IsOddInteger(NegativeOne).Should().BeTrue();
			NumberBaseHelper<Float>.IsOddInteger(NegativeTwo).Should().BeFalse();
			NumberBaseHelper<Float>.IsOddInteger(NegativeThree).Should().BeTrue();
			NumberBaseHelper<Float>.IsOddInteger(NegativeFour).Should().BeFalse();
		}
		[Fact]
		public static void IsPositiveTest()
		{
			NumberBaseHelper<Float>.IsPositive(One).Should().BeTrue();
			NumberBaseHelper<Float>.IsPositive(GreatestSubnormal).Should().BeTrue();
			NumberBaseHelper<Float>.IsPositive(Float.PositiveInfinity).Should().BeTrue();
			NumberBaseHelper<Float>.IsPositive(Float.NaN).Should().BeFalse();
			NumberBaseHelper<Float>.IsPositive(NegativeOne).Should().BeFalse();
			NumberBaseHelper<Float>.IsPositive(Float.NegativeInfinity).Should().BeFalse();
		}
		[Fact]
		public static void IsPositiveInfinityTest()
		{
			NumberBaseHelper<Float>.IsPositiveInfinity(One).Should().BeFalse();
			NumberBaseHelper<Float>.IsPositiveInfinity(NegativeOne).Should().BeFalse();
			NumberBaseHelper<Float>.IsPositiveInfinity(Float.NaN).Should().BeFalse();
			NumberBaseHelper<Float>.IsPositiveInfinity(Float.PositiveInfinity).Should().BeTrue();
			NumberBaseHelper<Float>.IsPositiveInfinity(Float.NegativeInfinity).Should().BeFalse();
		}
		[Fact]
		public static void IsRealNumberTest()
		{
			NumberBaseHelper<Float>.IsRealNumber(GreatestSubnormal).Should().BeTrue();
			NumberBaseHelper<Float>.IsRealNumber(MaxValue).Should().BeTrue();
			NumberBaseHelper<Float>.IsRealNumber(NegativeThousand).Should().BeTrue();
			NumberBaseHelper<Float>.IsRealNumber(Float.PositiveInfinity).Should().BeTrue();
			NumberBaseHelper<Float>.IsRealNumber(Float.NegativeInfinity).Should().BeTrue();
			NumberBaseHelper<Float>.IsRealNumber(Float.NaN).Should().BeFalse();
		}
		[Fact]
		public static void IsSubnormalTest()
		{
			NumberBaseHelper<Float>.IsSubnormal(GreatestSubnormal).Should().BeTrue();
			NumberBaseHelper<Float>.IsSubnormal(SmallestSubnormal).Should().BeTrue();
			NumberBaseHelper<Float>.IsSubnormal(MaxValue).Should().BeFalse();
			NumberBaseHelper<Float>.IsSubnormal(MinValue).Should().BeFalse();
			NumberBaseHelper<Float>.IsSubnormal(One).Should().BeFalse();
		}
		[Fact]
		public static void IsZeroTest()
		{
			NumberBaseHelper<Float>.IsZero(One).Should().BeFalse();
			NumberBaseHelper<Float>.IsZero(Float.Epsilon).Should().BeFalse();
			NumberBaseHelper<Float>.IsZero(Zero).Should().BeTrue();
			NumberBaseHelper<Float>.IsZero(NegativeZero).Should().BeTrue();
		}
		[Fact]
		public static void MaxMagnitudeTest()
		{
			NumberBaseHelper<Float>.MaxMagnitude(Float.NegativeInfinity, One)
				.Should().Be(Float.NegativeInfinity);
			NumberBaseHelper<Float>.MaxMagnitude(Float.MinValue, One)
				.Should().Be(Float.MinValue);
			NumberBaseHelper<Float>.MaxMagnitude(NegativeOne, One)
				.Should().Be(One);
			NumberBaseHelper<Float>.MaxMagnitude(-GreatestSubnormal, One)
				.Should().Be(One);
			NumberBaseHelper<Float>.MaxMagnitude(-Float.Epsilon, One)
				.Should().Be(One);
			NumberBaseHelper<Float>.MaxMagnitude(NegativeZero, One)
				.Should().Be(One);
			NumberBaseHelper<Float>.MaxMagnitude(Float.NaN, One)
				.Should().Be(Float.NaN);
			NumberBaseHelper<Float>.MaxMagnitude(Zero, One)
				.Should().Be(One);
			NumberBaseHelper<Float>.MaxMagnitude(Float.Epsilon, One)
				.Should().Be(One);
			NumberBaseHelper<Float>.MaxMagnitude(GreatestSubnormal, One)
				.Should().Be(One);
			NumberBaseHelper<Float>.MaxMagnitude(One, One)
				.Should().Be(One);
			NumberBaseHelper<Float>.MaxMagnitude(Float.MaxValue, One)
				.Should().Be(Float.MaxValue);
			NumberBaseHelper<Float>.MaxMagnitude(Float.PositiveInfinity, One)
				.Should().Be(Float.PositiveInfinity);
		}
		[Fact]
		public static void MaxMagnitudeNumberTest()
		{
			NumberBaseHelper<Float>.MaxMagnitudeNumber(Float.NegativeInfinity, One)
				.Should().Be(Float.NegativeInfinity);
			NumberBaseHelper<Float>.MaxMagnitudeNumber(Float.MinValue, One)
				.Should().Be(Float.MinValue);
			NumberBaseHelper<Float>.MaxMagnitudeNumber(NegativeOne, One)
				.Should().Be(One);
			NumberBaseHelper<Float>.MaxMagnitudeNumber(-GreatestSubnormal, One)
				.Should().Be(One);
			NumberBaseHelper<Float>.MaxMagnitudeNumber(-Float.Epsilon, One)
				.Should().Be(One);
			NumberBaseHelper<Float>.MaxMagnitudeNumber(NegativeZero, One)
				.Should().Be(One);
			NumberBaseHelper<Float>.MaxMagnitudeNumber(Float.NaN, One)
				.Should().Be(One);
			NumberBaseHelper<Float>.MaxMagnitudeNumber(Zero, One)
				.Should().Be(One);
			NumberBaseHelper<Float>.MaxMagnitudeNumber(Float.Epsilon, One)
				.Should().Be(One);
			NumberBaseHelper<Float>.MaxMagnitudeNumber(GreatestSubnormal, One)
				.Should().Be(One);
			NumberBaseHelper<Float>.MaxMagnitudeNumber(One, One)
				.Should().Be(One);
			NumberBaseHelper<Float>.MaxMagnitudeNumber(Float.MaxValue, One)
				.Should().Be(Float.MaxValue);
			NumberBaseHelper<Float>.MaxMagnitudeNumber(Float.PositiveInfinity, One)
				.Should().Be(Float.PositiveInfinity);
		}
		[Fact]
		public static void MinMagnitudeTest()
		{
			NumberBaseHelper<Float>.MinMagnitude(Float.NegativeInfinity, One)
				.Should().Be(One);
			NumberBaseHelper<Float>.MinMagnitude(Float.MinValue, One)
				.Should().Be(One);
			NumberBaseHelper<Float>.MinMagnitude(NegativeOne, One)
				.Should().Be(NegativeOne);
			NumberBaseHelper<Float>.MinMagnitude(-GreatestSubnormal, One)
				.Should().Be(-GreatestSubnormal);
			NumberBaseHelper<Float>.MinMagnitude(-Float.Epsilon, One)
				.Should().Be(-Float.Epsilon);
			NumberBaseHelper<Float>.MinMagnitude(NegativeZero, One)
				.Should().Be(NegativeZero);
			NumberBaseHelper<Float>.MinMagnitude(Float.NaN, One)
				.Should().Be(Float.NaN);
			NumberBaseHelper<Float>.MinMagnitude(Zero, One)
				.Should().Be(Zero);
			NumberBaseHelper<Float>.MinMagnitude(Float.Epsilon, One)
				.Should().Be(Float.Epsilon);
			NumberBaseHelper<Float>.MinMagnitude(GreatestSubnormal, One)
				.Should().Be(GreatestSubnormal);
			NumberBaseHelper<Float>.MinMagnitude(One, One)
				.Should().Be(One);
			NumberBaseHelper<Float>.MinMagnitude(Float.MaxValue, One)
				.Should().Be(One);
			NumberBaseHelper<Float>.MinMagnitude(Float.PositiveInfinity, One)
				.Should().Be(One);
		}
		[Fact]
		public static void MinMagnitudeNumberTest()
		{
			NumberBaseHelper<Float>.MinMagnitudeNumber(Float.NegativeInfinity, One)
				.Should().Be(One);
			NumberBaseHelper<Float>.MinMagnitudeNumber(Float.MinValue, One)
				.Should().Be(One);
			NumberBaseHelper<Float>.MinMagnitudeNumber(NegativeOne, One)
				.Should().Be(NegativeOne);
			NumberBaseHelper<Float>.MinMagnitudeNumber(-GreatestSubnormal, One)
				.Should().Be(-GreatestSubnormal);
			NumberBaseHelper<Float>.MinMagnitudeNumber(-Float.Epsilon, One)
				.Should().Be(-Float.Epsilon);
			NumberBaseHelper<Float>.MinMagnitudeNumber(NegativeZero, One)
				.Should().Be(NegativeZero);
			NumberBaseHelper<Float>.MinMagnitudeNumber(Float.NaN, One)
				.Should().Be(One);
			NumberBaseHelper<Float>.MinMagnitudeNumber(Zero, One)
				.Should().Be(Zero);
			NumberBaseHelper<Float>.MinMagnitudeNumber(Float.Epsilon, One)
				.Should().Be(Float.Epsilon);
			NumberBaseHelper<Float>.MinMagnitudeNumber(GreatestSubnormal, One)
				.Should().Be(GreatestSubnormal);
			NumberBaseHelper<Float>.MinMagnitudeNumber(One, One)
				.Should().Be(One);
			NumberBaseHelper<Float>.MinMagnitudeNumber(Float.MaxValue, One)
				.Should().Be(One);
			NumberBaseHelper<Float>.MinMagnitudeNumber(Float.PositiveInfinity, One)
				.Should().Be(One);
		}
		#endregion
	}
}
