using System;
using System.Collections.Generic;
using System.Globalization;
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

		#region IBinaryFloatingPointIEEE
		[Fact]
		public static void AllBitsSetTest()
		{
			BinaryNumberHelper<Float>.AllBitsSet.Should().Be(~Zero);
		}
		[Fact]
		public static void IsPow2Test()
		{
			BinaryNumberHelper<Float>.IsPow2(Half).Should().BeTrue();
			BinaryNumberHelper<Float>.IsPow2(One).Should().BeTrue();
			BinaryNumberHelper<Float>.IsPow2(Two).Should().BeTrue();
			BinaryNumberHelper<Float>.IsPow2(Three).Should().BeFalse();
			BinaryNumberHelper<Float>.IsPow2(NegativeTwo).Should().BeFalse();
		}
		#endregion

		#region IFloatingPointIEEE
		[Fact]
		public static void EpsilonTest()
		{
			FloatingPointIeee754<Float>.Epsilon.Should().Be(Float.Epsilon);
			MathOperatorsHelper.AdditionOperation<Float, Float, Float>(FloatingPointIeee754<Float>.Epsilon, NumberBaseHelper<Float>.Zero)
				.Should().NotBe(Float.Zero);
		}
		[Fact]
		public static void NaNTest()
		{
			FloatingPointIeee754<Float>.NaN
				.Should().Be(Float.NaN)
				.And.BeNaN();
			NumberBaseHelper<Float>.IsNaN(FloatingPointIeee754<Float>.NaN).Should().BeTrue();
		}
		[Fact]
		public static void NegativeInfinityTest()
		{
			FloatingPointIeee754<Float>.NegativeInfinity.Should().Be(Float.NegativeInfinity);
			NumberBaseHelper<Float>.IsInfinity(FloatingPointIeee754<Float>.NegativeInfinity).Should().BeTrue();
			NumberBaseHelper<Float>.IsNegativeInfinity(FloatingPointIeee754<Float>.NegativeInfinity).Should().BeTrue();
			NumberBaseHelper<Float>.IsPositiveInfinity(FloatingPointIeee754<Float>.NegativeInfinity).Should().BeFalse();
		}
		[Fact]
		public static void PositiveInfinityTest()
		{
			FloatingPointIeee754<Float>.PositiveInfinity.Should().Be(Float.PositiveInfinity);
			NumberBaseHelper<Float>.IsInfinity(FloatingPointIeee754<Float>.PositiveInfinity).Should().BeTrue();
			NumberBaseHelper<Float>.IsNegativeInfinity(FloatingPointIeee754<Float>.PositiveInfinity).Should().BeFalse();
			NumberBaseHelper<Float>.IsPositiveInfinity(FloatingPointIeee754<Float>.PositiveInfinity).Should().BeTrue();
		}
		[Fact]
		public static void BitDecrementTest()
		{
			FloatingPointIeee754<Float>.BitDecrement(One)
				.Should().Be(Values.CreateFloat<Float>(0x3FFFEFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
			FloatingPointIeee754<Float>.BitDecrement(NegativeOne)
				.Should().Be(Values.CreateFloat<Float>(0xBFFFF00000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000001));
			FloatingPointIeee754<Float>.BitDecrement(Zero)
				.Should().Be(-Float.Epsilon);
			FloatingPointIeee754<Float>.BitDecrement(Float.NegativeInfinity)
				.Should().Be(Float.NegativeInfinity);
			FloatingPointIeee754<Float>.BitDecrement(Float.PositiveInfinity)
				.Should().Be(Float.MaxValue);
		}
		[Fact]
		public static void BitIncrementTest()
		{
			FloatingPointIeee754<Float>.BitIncrement(One)
				.Should().Be(Values.CreateFloat<Float>(0x3FFFF00000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000001));
			FloatingPointIeee754<Float>.BitIncrement(NegativeOne)
				.Should().Be(Values.CreateFloat<Float>(0xBFFFEFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
			FloatingPointIeee754<Float>.BitIncrement(NegativeZero)
				.Should().Be(Float.Epsilon);
			FloatingPointIeee754<Float>.BitIncrement(Float.NegativeInfinity)
				.Should().Be(Float.MinValue);
			FloatingPointIeee754<Float>.BitIncrement(Float.PositiveInfinity)
				.Should().Be(Float.PositiveInfinity);
		}
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
		public static void ILogBTest()
		{
			FloatingPointIeee754<Float>.ILogB(Values.CreateFloat<Float>(0x4000_9000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000))
				.Should().Be(10);
			FloatingPointIeee754<Float>.ILogB(Values.CreateFloat<Float>(0x4003_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000))
				.Should().Be(64);
			FloatingPointIeee754<Float>.ILogB(Values.CreateFloat<Float>(0x4007_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000))
				.Should().Be(128);
			FloatingPointIeee754<Float>.ILogB(Values.CreateFloat<Float>(0xC003_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000))
				.Should().Be(64);
			FloatingPointIeee754<Float>.ILogB(Zero)
				.Should().Be(int.MinValue);
		}
		[Fact]
		public static void ReciprocalEstimateTest()
		{
			FloatingPointIeee754<Float>.ReciprocalEstimate(Two)
				.Should().Be(Half);
			FloatingPointIeee754<Float>.ReciprocalEstimate(Four)
				.Should().BeApproximately(Values.CreateFloat<Float>(0x3FFF_D000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), Delta);
		}
		[Fact]
		public static void ScaleBTest()
		{
			FloatingPointIeee754<Float>.ScaleB(Two, 3)
				.Should().Be(Values.CreateFloat<Float>(0x4000_3000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000));
			FloatingPointIeee754<Float>.ScaleB(NegativeTwo, 3)
				.Should().Be(Values.CreateFloat<Float>(0xC000_3000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000));
			FloatingPointIeee754<Float>.ScaleB(Zero, 6)
				.Should().Be(Zero);
			FloatingPointIeee754<Float>.ScaleB(Two, 300000)
				.Should().Be(Float.PositiveInfinity);
			FloatingPointIeee754<Float>.ScaleB(Two, -300000)
				.Should().Be(Zero);
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
			MathConstantsHelper.NegativeOne<Float>().Should().Be(-One);
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
			NumberBaseHelper<Int128>.CreateChecked<Float>(Int128MaxValue).Should().Be(Int128.MaxValue);
			NumberBaseHelper<UInt256>.CreateChecked<Float>(TwoOver255)
				.Should().Be(UInt256.Parse("57896044618658097711785492504343953926634992332820282019728792003956564819968"));
			NumberBaseHelper<UInt512>.CreateChecked<Float>(TwoOver511)
				.Should()
				.Be(UInt512.Parse("6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042048"));

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
			NumberBaseHelper<Int128>.CreateSaturating<Float>(Int128MaxValue).Should().Be(Int128.MaxValue);
			NumberBaseHelper<UInt256>.CreateSaturating<Float>(TwoOver255)
				.Should().Be(UInt256.Parse("57896044618658097711785492504343953926634992332820282019728792003956564819968"));
			NumberBaseHelper<UInt512>.CreateSaturating<Float>(TwoOver511)
				.Should()
				.Be(UInt512.Parse("6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042048"));

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
			NumberBaseHelper<Int128>.CreateTruncating<Float>(Int128MaxValue).Should().Be(Int128.MaxValue);
			NumberBaseHelper<UInt256>.CreateTruncating<Float>(TwoOver255)
				.Should().Be(UInt256.Parse("57896044618658097711785492504343953926634992332820282019728792003956564819968"));
			NumberBaseHelper<UInt512>.CreateTruncating<Float>(TwoOver511)
				.Should()
				.Be(UInt512.Parse("6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042048"));

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
			NumberBaseHelper<Float>.CreateChecked<Int128>(Int128.MaxValue).Should().Be(Int128MaxValue);
			NumberBaseHelper<Float>.CreateChecked<UInt256>(UInt256.Parse("57896044618658097711785492504343953926634992332820282019728792003956564819968"))
				.Should().Be(TwoOver255);
			NumberBaseHelper<Float>.CreateChecked<UInt512>(UInt512.Parse("6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042048"))
				.Should().Be(TwoOver511);

			NumberBaseHelper<Float>.CreateChecked<Half>((Half)0.5f).Should().Be(Half);
			NumberBaseHelper<Float>.CreateChecked<float>(0.5f).Should().Be(Half);
			NumberBaseHelper<Float>.CreateChecked<double>(0.5d).Should().Be(Half);
		}
		[Fact]
		public static void CreateSaturatingToQuadTest()
		{
			NumberBaseHelper<Float>.CreateSaturating<byte>(byte.MaxValue).Should().Be(ByteMaxValue);
			NumberBaseHelper<Float>.CreateSaturating<short>(short.MaxValue).Should().Be(Int16MaxValue);
			NumberBaseHelper<Float>.CreateSaturating<int>(int.MaxValue).Should().Be(Int32MaxValue);
			NumberBaseHelper<Float>.CreateSaturating<long>(long.MaxValue).Should().Be(Int64MaxValue);
			NumberBaseHelper<Float>.CreateSaturating<Int128>(Int128.MaxValue).Should().Be(Int128MaxValue);
			NumberBaseHelper<Float>.CreateSaturating<UInt256>(UInt256.Parse("57896044618658097711785492504343953926634992332820282019728792003956564819968"))
				.Should().Be(TwoOver255);
			NumberBaseHelper<Float>.CreateSaturating<UInt512>(UInt512.Parse("6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042048"))
				.Should().Be(TwoOver511);

			NumberBaseHelper<Float>.CreateSaturating<Half>((Half)0.5f).Should().Be(Half);
			NumberBaseHelper<Float>.CreateSaturating<float>(0.5f).Should().Be(Half);
			NumberBaseHelper<Float>.CreateSaturating<double>(0.5d).Should().Be(Half);
		}
		[Fact]
		public static void CreateTruncatingToQuadTest()
		{
			NumberBaseHelper<Float>.CreateTruncating<byte>(byte.MaxValue).Should().Be(ByteMaxValue);
			NumberBaseHelper<Float>.CreateTruncating<short>(short.MaxValue).Should().Be(Int16MaxValue);
			NumberBaseHelper<Float>.CreateTruncating<int>(int.MaxValue).Should().Be(Int32MaxValue);
			NumberBaseHelper<Float>.CreateTruncating<long>(long.MaxValue).Should().Be(Int64MaxValue);
			NumberBaseHelper<Float>.CreateTruncating<Int128>(Int128.MaxValue).Should().Be(Int128MaxValue);
			NumberBaseHelper<Float>.CreateTruncating<UInt256>(UInt256.Parse("57896044618658097711785492504343953926634992332820282019728792003956564819968"))
				.Should().Be(TwoOver255);
			NumberBaseHelper<Float>.CreateTruncating<UInt512>(UInt512.Parse("6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042048"))
				.Should().Be(TwoOver511);

			NumberBaseHelper<Float>.CreateTruncating<Half>((Half)0.5f).Should().Be(Half);
			NumberBaseHelper<Float>.CreateTruncating<float>(0.5f).Should().Be(Half);
			NumberBaseHelper<Float>.CreateTruncating<double>(0.5d).Should().Be(Half);
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
		[Theory]
		[MemberData(nameof(TryParseTheoryData))]
		public void ParseTest(string s, bool success, Float output)
		{
			if (success)
			{
				NumberBaseHelper<Float>.Parse(s, NumberStyles.Float, NumberFormatInfo.CurrentInfo)
					.Should().Be(output);
			}
			else
			{
				Assert.Throws<FormatException>(() => NumberBaseHelper<Float>.Parse(s, NumberStyles.Float, NumberFormatInfo.CurrentInfo));
			}
		}

		[Theory]
		[MemberData(nameof(TryParseTheoryData))]
		public void TryParseTest(string s, bool success, Float output)
		{
			Float result;

			NumberBaseHelper<Float>.TryParse(s, NumberStyles.Float, NumberFormatInfo.CurrentInfo, out result).Should().Be(success);
			result.Should().Be(output);
		}

		[Theory]
		[MemberData(nameof(TryParseTheoryData))]
		public void ParseUtf8Test(string s, bool success, Float output)
		{
			byte[] utf8 = Encoding.UTF8.GetBytes(s);

			if (success)
			{
				NumberBaseHelper<Float>.Parse(utf8, NumberStyles.Float, NumberFormatInfo.CurrentInfo)
					.Should().Be(output);
			}
			else
			{
				Assert.Throws<FormatException>(() => NumberBaseHelper<Float>.Parse(utf8, NumberStyles.Float, NumberFormatInfo.CurrentInfo));
			}
		}

		[Theory]
		[MemberData(nameof(TryParseTheoryData))]
		public void TryParseUtf8Test(string s, bool success, Float output)
		{
			ReadOnlySpan<byte> utf8 = Encoding.UTF8.GetBytes(s);
			Float result;

			NumberBaseHelper<Float>.TryParse(utf8, NumberStyles.Float, NumberFormatInfo.CurrentInfo, out result).Should().Be(success);
			result.Should().Be(output);
		}
		#endregion
	}
}
