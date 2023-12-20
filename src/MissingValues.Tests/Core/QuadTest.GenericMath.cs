using MissingValues.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
		[Fact]
		public static void op_BitwiseAndTest()
		{
			BitwiseOperatorsHelper<Float, Float, Float>.BitwiseAndOperation(Zero, Values.CreateQuad(0x0, 0x1)).Should().Be(Zero);
			BitwiseOperatorsHelper<Float, Float, Float>.BitwiseAndOperation(Values.CreateQuad(0x0, 0x1), Values.CreateQuad(0x0, 0x1)).Should().Be(Values.CreateQuad(0x0, 0x1));
			BitwiseOperatorsHelper<Float, Float, Float>.BitwiseAndOperation(BinaryNumberHelper<Float>.AllBitsSet, Values.CreateQuad(0x0, 0x1)).Should().Be(Values.CreateQuad(0x0, 0x1));
		}
		[Fact]
		public static void op_BitwiseOrTest()
		{
			BitwiseOperatorsHelper<Float, Float, Float>.BitwiseOrOperation(Zero, Values.CreateQuad(0x0, 0x1))
				.Should().Be(Values.CreateQuad(0x0, 0x1));
			BitwiseOperatorsHelper<Float, Float, Float>.BitwiseOrOperation(Values.CreateQuad(0x0, 0x1), Values.CreateQuad(0x0, 0x1))
				.Should().Be(Values.CreateQuad(0x0, 0x1));
			BitwiseOperatorsHelper<Float, Float, Float>.BitwiseOrOperation(BinaryNumberHelper<Float>.AllBitsSet, Values.CreateQuad(0x0, 0x1))
				.Should().Be(BinaryNumberHelper<Float>.AllBitsSet);
		}
		[Fact]
		public static void op_ExclusiveOrTest()
		{
			BitwiseOperatorsHelper<Float, Float, Float>.ExclusiveOrOperation(Zero, Values.CreateQuad(0x0, 0x1))
				.Should().Be(Values.CreateQuad(0x0, 0x1));
			BitwiseOperatorsHelper<Float, Float, Float>.ExclusiveOrOperation(Values.CreateQuad(0x0, 0x1), Values.CreateQuad(0x0, 0x1))
				.Should().Be(Zero);
			BitwiseOperatorsHelper<Float, Float, Float>.ExclusiveOrOperation(BinaryNumberHelper<Float>.AllBitsSet, Values.CreateQuad(0x0, 0x1))
				.Should().Be(Values.CreateQuad(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFE));
		}
		[Fact]
		public static void op_OnesComplementTest()
		{
			BitwiseOperatorsHelper<Float, Float, Float>.OnesComplementOperation(Zero)
				.Should().Be(BinaryNumberHelper<Float>.AllBitsSet);
			BitwiseOperatorsHelper<Float, Float, Float>.OnesComplementOperation(Values.CreateQuad(0x0, 0x1))
				.Should().Be(Values.CreateQuad(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFE));
			BitwiseOperatorsHelper<Float, Float, Float>.OnesComplementOperation(BinaryNumberHelper<Float>.AllBitsSet)
				.Should().Be(Zero);
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
		public static void AcosTest()
		{
			GenericFloatingPointFunctions.Acos<Float>(Float.NaN)
				.Should()
				.BeBitwiseEquivalentTo(Float.NaN)
				.And.BeNaN();
			GenericFloatingPointFunctions.Acos<Float>(Two)
				.Should().BeNaN();
			GenericFloatingPointFunctions.Acos<Float>(NegativeTwo)
				.Should().BeNaN();
			GenericFloatingPointFunctions.Acos<Float>(Half)
				.Should().BeApproximately(Float.Pi / 3, Delta);
			GenericFloatingPointFunctions.Acos<Float>(One)
				.Should().BeApproximately(Float.Zero, Delta);
			GenericFloatingPointFunctions.Acos<Float>(NegativeOne)
				.Should().BeApproximately(Float.Pi, Delta);
		}
		[Fact]
		public static void AcoshTest()
		{
			GenericFloatingPointFunctions.Acosh<Float>(Two)
				.Should().BeApproximately(Values.CreateQuad(0x3FFF_5124_2719_8043, 0x49BE_684B_D018_8D53), Delta);
			GenericFloatingPointFunctions.Acosh<Float>(Half)
				.Should().BeNaN();
			GenericFloatingPointFunctions.Acosh<Float>(Zero)
				.Should().BeNaN();
			GenericFloatingPointFunctions.Acosh<Float>(NegativeOne)
				.Should().BeNaN();
		}
		[Fact]
		public static void AsinTest()
		{
			GenericFloatingPointFunctions.Asin<Float>(Half)
				.Should().BeApproximately(Values.CreateQuad(0x3FFE_0C15_2382_D736, 0x5846_5BB3_2E0F_567B), Delta)
				.And.BeApproximately(Float.Pi / Six, Delta);
			GenericFloatingPointFunctions.Asin<Float>(Two)
				.Should().Be(Float.NaN);
			GenericFloatingPointFunctions.Asin<Float>(One)
				.Should().BeApproximately(Float.Pi / Two, Delta);
			GenericFloatingPointFunctions.Asin<Float>(NegativeOne)
				.Should().BeApproximately(-Float.Pi / Two, Delta);
		}
		[Fact]
		public static void AsinhTest()
		{
			GenericFloatingPointFunctions.Asinh<Float>(Two)
				.Should().BeApproximately(Values.CreateQuad(0x3FFF_7192_1831_3D08, 0x72F8_E831_837F_0E95), Delta);
			GenericFloatingPointFunctions.Asinh<Float>(Zero)
				.Should().BeApproximately(Zero, Delta);
			GenericFloatingPointFunctions.Asinh<Float>(Values.CreateQuad(0xBFFF_8000_0000_0000, 0x0000_0000_0000_0000))
				.Should().BeApproximately(Values.CreateQuad(0x3FFF_31DC_0090_B63D, 0x8682_7E4B_AAAD_1909), Delta);
		}
		[Fact]
		public static void AtanTest()
		{
			GenericFloatingPointFunctions.Atan<Float>(Half)
				.Should().BeApproximately(Values.CreateQuad(0x3FFD_DAC6_7056_1BB4, 0xF1DE_7924_87B0_F0F3), Delta);
			GenericFloatingPointFunctions.Atan<Float>(Zero)
				.Should().BeApproximately(Zero, Delta);
			GenericFloatingPointFunctions.Atan<Float>(Float.PositiveInfinity)
				.Should().BeApproximately(Float.Pi / Two, Delta);
			GenericFloatingPointFunctions.Atan<Float>(Two)
				.Should().BeApproximately(Values.CreateQuad(0x3FFF_1B6E_192E_BBE4, 0x3F5A_7D44_566B_01A8), Delta);
		}
		[Fact]
		public static void Atan2Test()
		{
			FloatingPointIeee754<Float>.Atan2(Zero, Two)
				.Should().BeApproximately(Zero, Delta);
			FloatingPointIeee754<Float>.Atan2(Zero, Zero)
				.Should().BeApproximately(Zero, Delta);
			FloatingPointIeee754<Float>.Atan2(Zero, NegativeTwo)
				.Should().BeApproximately(Float.Pi, Delta);
			FloatingPointIeee754<Float>.Atan2(One, Two)
				.Should().BeApproximately(Values.CreateQuad(0x3FFD_DAC6_7056_1BB4, 0xF1DE_7924_87B0_F0F3), Delta);
			FloatingPointIeee754<Float>.Atan2(NegativeOne, Two)
				.Should().BeApproximately(Values.CreateQuad(0xBFFD_DAC6_7056_1BB4, 0xF1DE_7924_87B0_F0F3), Delta);
			FloatingPointIeee754<Float>.Atan2(One, NegativeTwo)
				.Should().BeApproximately(Values.CreateQuad(0x4000_56C6_E739_7F5A, 0xE130_A2BB_E272_574C), Delta);
		}
		[Fact]
		public static void AtanhTest()
		{
			GenericFloatingPointFunctions.Atanh<Float>(Two)
				.Should().Be(Float.NaN);
			GenericFloatingPointFunctions.Atanh<Float>(NegativeFour)
				.Should().Be(Float.NaN);
			GenericFloatingPointFunctions.Atanh<Float>(Zero)
				.Should().BeApproximately(Zero, Delta);
			GenericFloatingPointFunctions.Atanh<Float>(Half)
				.Should().BeApproximately(Values.CreateQuad(0x3FFE_193E_A7AA_D030, 0xA976_BA8D_B53A_D6E3), Delta);
		}
		[Fact]
		public static void BitDecrementTest()
		{
			FloatingPointIeee754<Float>.BitDecrement(One)
				.Should().Be(Values.CreateQuad(0x3FFEFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
			FloatingPointIeee754<Float>.BitDecrement(NegativeOne)
				.Should().Be(Values.CreateQuad(0xBFFF000000000000, 0x0000000000000001));
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
				.Should().Be(Values.CreateQuad(0x3FFF000000000000, 0x0000000000000001));
			FloatingPointIeee754<Float>.BitIncrement(NegativeOne)
				.Should().Be(Values.CreateQuad(0xBFFEFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
			FloatingPointIeee754<Float>.BitIncrement(NegativeZero)
				.Should().Be(Float.Epsilon);
			FloatingPointIeee754<Float>.BitIncrement(Float.NegativeInfinity)
				.Should().Be(Float.MinValue);
			FloatingPointIeee754<Float>.BitIncrement(Float.PositiveInfinity)
				.Should().Be(Float.PositiveInfinity);
		}
		[Fact]
		public static void CbrtTest()
		{
			GenericFloatingPointFunctions.Cbrt(Values.CreateQuad(0x4005_0000_0000_0000, 0x0000_0000_0000_0000))
				.Should().Be(Four);


			GenericFloatingPointFunctions.Cbrt(Zero)
				.Should().Be(Zero);
			GenericFloatingPointFunctions.Cbrt(-Zero)
				.Should().Be(-Zero);
			GenericFloatingPointFunctions.Cbrt(Float.PositiveInfinity)
				.Should().Be(Float.PositiveInfinity);
			GenericFloatingPointFunctions.Cbrt(NegativeFour)
				.Should().BeApproximately(Values.CreateQuad(0xBFFF_965F_EA53_D6E3, 0xC82B_0599_9AB4_3DC5), Delta);
			GenericFloatingPointFunctions.Cbrt(Float.NaN)
				.Should().Be(Float.NaN);
		}
		[Fact]
		public static void CosTest()
		{
			GenericFloatingPointFunctions.Cos(Zero)
				.Should().BeApproximately(One, Delta);
			GenericFloatingPointFunctions.Cos(Float.Pi / Two)
				.Should().BeApproximately(Zero, Delta);
			GenericFloatingPointFunctions.Cos(Float.Pi)
				.Should().BeApproximately(NegativeOne, Delta);
			GenericFloatingPointFunctions.Cos(Float.Pi * Two)
				.Should().BeApproximately(One, Delta);

			GenericFloatingPointFunctions.Cos(Float.NaN)
				.Should().Be(Float.NaN);
			GenericFloatingPointFunctions.Cos(Float.PositiveInfinity)
				.Should().Be(Float.NaN);
			GenericFloatingPointFunctions.Cos(Float.NegativeInfinity)
				.Should().Be(Float.NaN);
		}
		[Fact]
		public static void CoshTest()
		{
			GenericFloatingPointFunctions.Cosh(Zero)
				.Should().Be(One);
			GenericFloatingPointFunctions.Cosh(Two)
				.Should().BeApproximately(Values.CreateQuad(0x4000_E18F_A0DF_2D9B, 0xC293_27F7_1777_4D0C), Delta);
			GenericFloatingPointFunctions.Cosh(Five)
				.Should().BeApproximately(Values.CreateQuad(0x4005_28D6_FCBE_FF3A, 0x9C65_3333_916C_7D52), Delta);
			GenericFloatingPointFunctions.Cosh(NegativeFive)
				.Should().BeApproximately(Values.CreateQuad(0x4005_28D6_FCBE_FF3A, 0x9C65_3333_916C_7D52), Delta);
		}
		[Fact]
		public static void ExpTest()
		{
			GenericFloatingPointFunctions.Exp(Two)
				.Should().BeApproximately(Values.CreateQuad(0x4001_D8E6_4B8D_4DDA, 0xDCC3_3A3B_A206_B68B), Delta);
			GenericFloatingPointFunctions.Exp(NegativeHalf)
				.Should().BeApproximately(Values.CreateQuad(0x3FFE_368B_2FC6_F960, 0x9FE7_ACEB_46AA_619C), Delta);
			GenericFloatingPointFunctions.Exp(Values.CreateQuad(0x400C_7700_0000_0000, 0x0000_0000_0000_0000))
				.Should().Be(Float.PositiveInfinity);
			GenericFloatingPointFunctions.Exp(Values.CreateQuad(0xC00C_7700_0000_0000, 0x0000_0000_0000_0000))
				.Should().Be(Zero);
			GenericFloatingPointFunctions.Exp(Zero)
				.Should().Be(One);
			GenericFloatingPointFunctions.Exp(Float.PositiveInfinity)
				.Should().Be(Float.PositiveInfinity);
			GenericFloatingPointFunctions.Exp(Float.NaN)
				.Should().Be(Float.NaN);
			GenericFloatingPointFunctions.Exp(Float.NegativeInfinity)
				.Should().Be(Zero);
		}
		[Fact]
		public static void Exp10Test()
		{
			GenericFloatingPointFunctions.Exp10(Two)
				.Should().Be(Hundred);
			GenericFloatingPointFunctions.Exp10(Zero)
				.Should().Be(One);
			GenericFloatingPointFunctions.Exp10(Float.PositiveInfinity)
				.Should().Be(Float.PositiveInfinity);
			GenericFloatingPointFunctions.Exp10(Float.NaN)
				.Should().Be(Float.NaN);
			GenericFloatingPointFunctions.Exp10(Float.NegativeInfinity)
				.Should().Be(Zero);
		}
		[Fact]
		public static void Exp2Test()
		{
			GenericFloatingPointFunctions.Exp2(Two)
				.Should().Be(Four);
			GenericFloatingPointFunctions.Exp2(Zero)
				.Should().Be(One);
			GenericFloatingPointFunctions.Exp2(Float.PositiveInfinity)
				.Should().Be(Float.PositiveInfinity);
			GenericFloatingPointFunctions.Exp2(Float.NaN)
				.Should().Be(Float.NaN);
			GenericFloatingPointFunctions.Exp2(Float.NegativeInfinity)
				.Should().Be(Zero);
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
		public static void HypotTest()
		{
			GenericFloatingPointFunctions.Hypot(Hundred, Ten)
				.Should().BeApproximately(Values.CreateQuad(0x4005_91FE_B9F2_BF46, 0xC3A7_08A3_1212_49E7), Delta);
			GenericFloatingPointFunctions.Hypot(Float.PositiveInfinity, Float.NegativeInfinity)
				.Should().Be(Float.PositiveInfinity);
			GenericFloatingPointFunctions.Hypot(Float.NaN, Float.NaN)
				.Should().Be(Float.NaN);
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
			FloatingPointIeee754<Float>.ILogB(Values.CreateQuad(0x4009_0000_0000_0000, 0x0000_0000_0000_0000))
				.Should().Be(10);
			FloatingPointIeee754<Float>.ILogB(Values.CreateQuad(0x403F_0000_0000_0000, 0x0000_0000_0000_0000))
				.Should().Be(64);
			FloatingPointIeee754<Float>.ILogB(Values.CreateQuad(0xC03F_0000_0000_0000, 0x0000_0000_0000_0000))
				.Should().Be(64);
			FloatingPointIeee754<Float>.ILogB(Zero)
				.Should().Be(int.MinValue);
		}
		[Fact]
		public static void LogTest()
		{
			GenericFloatingPointFunctions.Log(Hundred)
				.Should().BeApproximately(Values.CreateQuad(0x4001_26BB_1BBB_5551, 0x582D_D4AD_AC57_05A6), Delta);
			GenericFloatingPointFunctions.Log(One)
				.Should().Be(Zero);
			GenericFloatingPointFunctions.Log(Zero)
				.Should().Be(Float.NegativeInfinity);
			GenericFloatingPointFunctions.Log(NegativeFive)
				.Should().Be(Float.NaN);
		}
		[Fact]
		public static void Log2Test()
		{
			GenericFloatingPointFunctions.Log2(Four)
				.Should().BeApproximately(Two, Delta);
			GenericFloatingPointFunctions.Log2(One)
				.Should().Be(Zero);
			GenericFloatingPointFunctions.Log2(Zero)
				.Should().Be(Float.NegativeInfinity);
			GenericFloatingPointFunctions.Log2(NegativeFive)
				.Should().Be(Float.NaN);
		}
		[Fact]
		public static void Log10Test()
		{
			GenericFloatingPointFunctions.Log10(Thousand)
				.Should().BeApproximately(Three, Delta);
			GenericFloatingPointFunctions.Log10(One)
				.Should().Be(Zero);
			GenericFloatingPointFunctions.Log10(Zero)
				.Should().Be(Float.NegativeInfinity);
			GenericFloatingPointFunctions.Log10(NegativeFive)
				.Should().Be(Float.NaN);
		}
		[Fact]
		public static void PowTest()
		{
			GenericFloatingPointFunctions.Pow<Float>(Three, Ten)
				.Should()
				.BeApproximately(Values.CreateQuad(0x400E_CD52_0000_0000, 0x0000_0000_0000_0000), Delta);
			GenericFloatingPointFunctions.Pow<Float>(Two, NegativeFour)
				.Should()
				.BeApproximately(Values.CreateQuad(0x3FFB_0000_0000_0000, 0x0000_0000_0000_0000), Delta);

			// Special Cases
			Float anything = 8, oddInt = 7, nonInt = 7.5d, greaterThanOne = GreaterThanOneSmallest, lessThanOne = LessThanOneLargest;

			GenericFloatingPointFunctions.Pow<Float>(anything, Zero)
				.Should()
				.Be(One);

			GenericFloatingPointFunctions.Pow<Float>(anything, One)
				.Should()
				.Be(anything);

			GenericFloatingPointFunctions.Pow<Float>(anything, Float.NaN)
				.Should()
				.Be(Float.NaN);

			GenericFloatingPointFunctions.Pow<Float>(One, Float.NaN)
				.Should()
				.Be(One);

			GenericFloatingPointFunctions.Pow<Float>(Float.NaN, anything)
				.Should()
				.Be(Float.NaN);

			GenericFloatingPointFunctions.Pow<Float>(greaterThanOne, Float.PositiveInfinity)
				.Should()
				.Be(Float.PositiveInfinity);

			GenericFloatingPointFunctions.Pow<Float>(greaterThanOne, Float.NegativeInfinity)
				.Should()
				.Be(Zero);

			GenericFloatingPointFunctions.Pow<Float>(lessThanOne, Float.PositiveInfinity)
				.Should()
				.Be(Zero);

			GenericFloatingPointFunctions.Pow<Float>(lessThanOne, Float.NegativeInfinity)
				.Should()
				.Be(Float.PositiveInfinity);

			GenericFloatingPointFunctions.Pow<Float>(One, Float.PositiveInfinity)
				.Should()
				.Be(One);

			GenericFloatingPointFunctions.Pow<Float>(One, Float.NegativeInfinity)
				.Should()
				.Be(One);

			GenericFloatingPointFunctions.Pow<Float>(NegativeOne, Float.PositiveInfinity)
				.Should()
				.Be(One);

			GenericFloatingPointFunctions.Pow<Float>(NegativeOne, Float.NegativeInfinity)
				.Should()
				.Be(One);

			GenericFloatingPointFunctions.Pow<Float>(Zero, anything)
				.Should()
				.Be(Zero);

			GenericFloatingPointFunctions.Pow<Float>(NegativeZero, anything)
				.Should()
				.Be(Zero);

			GenericFloatingPointFunctions.Pow<Float>(Zero, -anything)
				.Should()
				.Be(Float.PositiveInfinity);

			GenericFloatingPointFunctions.Pow<Float>(NegativeZero, -anything)
				.Should()
				.Be(Float.PositiveInfinity);

			GenericFloatingPointFunctions.Pow<Float>(NegativeZero, oddInt)
				.Should()
				.Be(-(GenericFloatingPointFunctions.Pow<Float>(Zero, oddInt)));

			GenericFloatingPointFunctions.Pow<Float>(Float.PositiveInfinity, anything)
				.Should()
				.Be(Float.PositiveInfinity);

			GenericFloatingPointFunctions.Pow<Float>(Float.PositiveInfinity, -anything)
				.Should()
				.Be(Zero);

			GenericFloatingPointFunctions.Pow<Float>(Float.NegativeInfinity, anything)
				.Should()
				.Be(GenericFloatingPointFunctions.Pow<Float>(NegativeZero, -anything));
			
			GenericFloatingPointFunctions.Pow<Float>(-anything, oddInt)
				.Should()
				.Be(GenericFloatingPointFunctions.Pow<Float>(NegativeOne, oddInt) * GenericFloatingPointFunctions.Pow<Float>(+anything, oddInt));

			GenericFloatingPointFunctions.Pow<Float>(-anything, nonInt)
				.Should()
				.Be(Float.NaN);
		}
		[Fact]
		public static void ReciprocalEstimateTest()
		{
			FloatingPointIeee754<Float>.ReciprocalEstimate(Two)
				.Should().Be(Half);
			FloatingPointIeee754<Float>.ReciprocalEstimate(Three)
				.Should().BeApproximately(Values.CreateQuad(0x3FFD_5555_5555_5555, 0x5555_165E_5289_24A5), Delta);
			FloatingPointIeee754<Float>.ReciprocalEstimate(Four)
				.Should().BeApproximately(Values.CreateQuad(0x3FFD_0000_0000_0000, 0x0000_0000_0000_0000), Delta);
		}
		[Fact]
		public static void RootNTest()
		{
			GenericFloatingPointFunctions.RootN(Values.CreateQuad(0x4005_4400_0000_0000, 0x0000_0000_0000_0000), 4)
				.Should().Be(Three);
			GenericFloatingPointFunctions.RootN(Values.CreateQuad(0x4005_0000_0000_0000, 0x0000_0000_0000_0000), 3)
				.Should().Be(Four)
				.And.Be(GenericFloatingPointFunctions.Cbrt(Values.CreateQuad(0x4005_0000_0000_0000, 0x0000_0000_0000_0000)));
			GenericFloatingPointFunctions.RootN(Hundred, 2)
				.Should().Be(Ten)
				.And.Be(GenericFloatingPointFunctions.Sqrt(Hundred));

			GenericFloatingPointFunctions.RootN(Zero, 2)
				.Should().Be(Zero);
			GenericFloatingPointFunctions.RootN(NegativeZero, 2)
				.Should().Be(NegativeZero);
			GenericFloatingPointFunctions.RootN(Float.PositiveInfinity, 2)
				.Should().Be(Float.PositiveInfinity);
			GenericFloatingPointFunctions.RootN(NegativeFour, 2)
				.Should().Be(Float.NaN);
			GenericFloatingPointFunctions.RootN(Float.NaN, 2)
				.Should().Be(Float.NaN);
		}
		[Fact]
		public static void ScaleBTest()
		{
			FloatingPointIeee754<Float>.ScaleB(Two, 3)
				.Should().Be(Values.CreateQuad(0x4003_0000_0000_0000, 0x0000_0000_0000_0000));
			FloatingPointIeee754<Float>.ScaleB(NegativeTwo, 3)
				.Should().Be(Values.CreateQuad(0xC003_0000_0000_0000, 0x0000_0000_0000_0000));
			FloatingPointIeee754<Float>.ScaleB(Zero, 6)
				.Should().Be(Zero);
			FloatingPointIeee754<Float>.ScaleB(Two, 30000)
				.Should().Be(Float.PositiveInfinity);
			FloatingPointIeee754<Float>.ScaleB(Two, -30000)
				.Should().Be(Zero);
		}
		[Fact]
		public static void SinTest()
		{
			GenericFloatingPointFunctions.Sin(Float.NaN)
				.Should().Be(Float.NaN);
			GenericFloatingPointFunctions.Sin(Float.PositiveInfinity)
				.Should().Be(Float.NaN);
			GenericFloatingPointFunctions.Sin(Float.NegativeInfinity)
				.Should().Be(Float.NaN);
		}
		[Fact]
		public static void SinCosTest()
		{
			Float sin, cos;

			(sin, cos) = GenericFloatingPointFunctions.SinCos(Zero);
			sin.Should().BeApproximately(Zero, Delta);
			cos.Should().BeApproximately(One, Delta);

			(sin, cos) = GenericFloatingPointFunctions.SinCos(Float.Pi);
			sin.Should().BeApproximately(Zero, Delta);
			cos.Should().BeApproximately(NegativeOne, Delta);
			
			(sin, cos) = GenericFloatingPointFunctions.SinCos(Float.Pi / Two);
			sin.Should().BeApproximately(One, Delta);
			cos.Should().BeApproximately(Zero, Delta);

			(sin, cos) = GenericFloatingPointFunctions.SinCos(Float.Pi * Two);
			sin.Should().BeApproximately(Zero, Delta);
			cos.Should().BeApproximately(One, Delta);
		}
		[Fact]
		public static void SinhTest()
		{
			GenericFloatingPointFunctions.Sinh(Two)
				.Should().BeApproximately(Values.CreateQuad(0x4000_D03C_F63B_6E19, 0xF6F3_4C80_2C96_2009), Delta);
			GenericFloatingPointFunctions.Sinh(Zero)
				.Should().Be(Zero);
		}
		[Fact]
		public static void SqrtTest()
		{
			GenericFloatingPointFunctions.Sqrt(Ten)
				.Should().BeApproximately(Values.CreateQuad(0x4000_94C5_83AD_A5B5, 0x2920_4A2B_C830_CD9C), Delta);
			GenericFloatingPointFunctions.Sqrt(Hundred)
				.Should().BeApproximately(Ten, Delta);

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
		}
		[Fact]
		public static void TanTest()
		{
			GenericFloatingPointFunctions.Tan(Float.NaN)
				.Should().Be(Float.NaN);
			GenericFloatingPointFunctions.Tan(Float.PositiveInfinity)
				.Should().Be(Float.NaN);
			GenericFloatingPointFunctions.Tan(Float.NegativeInfinity)
				.Should().Be(Float.NaN);
		}
		[Fact]
		public static void TanhTest()
		{
			GenericFloatingPointFunctions.Tanh(Two)
				.Should().BeApproximately(Values.CreateQuad(0x3FFE_ED95_05E1_BC3D, 0x3D33_C432_FC3E_8256), Delta);
			GenericFloatingPointFunctions.Tanh(Float.NaN)
				.Should().Be(Float.NaN);
			GenericFloatingPointFunctions.Tanh(Zero)
				.Should().Be(Zero);
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
			NumberBaseHelper<UInt128>.CreateChecked<Float>(TwoOver127).Should().Be(UInt128.Parse("170141183460469231731687303715884105728"));
			NumberBaseHelper<UInt256>.CreateChecked<Float>(TwoOver127).Should().Be(UInt256.Parse("170141183460469231731687303715884105728"));
			NumberBaseHelper<UInt512>.CreateChecked<Float>(TwoOver127).Should().Be(UInt512.Parse("170141183460469231731687303715884105728"));

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
			NumberBaseHelper<UInt128>.CreateSaturating<Float>(TwoOver127).Should().Be(UInt128.Parse("170141183460469231731687303715884105728"));
			NumberBaseHelper<UInt256>.CreateSaturating<Float>(TwoOver127).Should().Be(UInt256.Parse("170141183460469231731687303715884105728"));
			NumberBaseHelper<UInt512>.CreateSaturating<Float>(TwoOver127).Should().Be(UInt512.Parse("170141183460469231731687303715884105728"));

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
			NumberBaseHelper<UInt128>.CreateTruncating<Float>(TwoOver127).Should().Be(UInt128.Parse("170141183460469231731687303715884105728"));
			NumberBaseHelper<UInt256>.CreateTruncating<Float>(TwoOver127).Should().Be(UInt256.Parse("170141183460469231731687303715884105728"));
			NumberBaseHelper<UInt512>.CreateTruncating<Float>(TwoOver127).Should().Be(UInt512.Parse("170141183460469231731687303715884105728"));

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
			NumberBaseHelper<Float>.CreateChecked<UInt128>(UInt128.Parse("170141183460469231731687303715884105728"))
				.Should().Be(TwoOver127);
			NumberBaseHelper<Float>.CreateChecked<UInt256>(UInt256.Parse("170141183460469231731687303715884105728"))
				.Should().Be(TwoOver127);
			NumberBaseHelper<Float>.CreateChecked<UInt512>(UInt512.Parse("170141183460469231731687303715884105728"))
				.Should().Be(TwoOver127);

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
			NumberBaseHelper<Float>.CreateChecked<UInt128>(UInt128.Parse("170141183460469231731687303715884105728"))
				.Should().Be(TwoOver127);
			NumberBaseHelper<Float>.CreateChecked<UInt256>(UInt256.Parse("170141183460469231731687303715884105728"))
				.Should().Be(TwoOver127);
			NumberBaseHelper<Float>.CreateChecked<UInt512>(UInt512.Parse("170141183460469231731687303715884105728"))
				.Should().Be(TwoOver127);

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
			NumberBaseHelper<Float>.CreateChecked<UInt128>(UInt128.Parse("170141183460469231731687303715884105728"))
				.Should().Be(TwoOver127);
			NumberBaseHelper<Float>.CreateChecked<UInt256>(UInt256.Parse("170141183460469231731687303715884105728"))
				.Should().Be(TwoOver127);
			NumberBaseHelper<Float>.CreateChecked<UInt512>(UInt512.Parse("170141183460469231731687303715884105728"))
				.Should().Be(TwoOver127);

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
