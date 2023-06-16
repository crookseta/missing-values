using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues.Tests
{
	public partial class UInt256Test
	{
		#region Readonly Variables
		private const double MaxValueAsDouble = 115792089237316195423570985008687907853269984665640564039457584007913129639935.0;

		private static readonly UInt256 ByteMaxValue = new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_00FF));
		private static readonly UInt256 UInt16MaxValue = new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_FFFF));
		private static readonly UInt256 UInt32MaxValue = new(new(0x0000_0000_0000_0000, 0x0000_0000_FFFF_FFFF));
		private static readonly UInt256 UInt64MaxValue = new(new(0x0000_0000_0000_0000, 0xFFFF_FFFF_FFFF_FFFF));
		private static readonly UInt256 UInt128MaxValue = new(new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF));

		private static readonly UInt256 Zero = new(new (0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new (0x0000_0000_0000_0000, 0x0000_0000_0000_0000));
		private static readonly UInt256 One = new(new (0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new (0x0000_0000_0000_0000, 0x0000_0000_0000_0001));
		private static readonly UInt256 Two = new(new (0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new (0x0000_0000_0000_0000, 0x0000_0000_0000_0002));

		private static readonly UInt256 MaxValue = new(new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF));
		private static readonly UInt256 MaxValueMinusOne = new(new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFE));
		private static readonly UInt256 MaxValueMinusTwo = new(new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFD));
		#endregion

		#region Generic Math Operators
		[Fact]
		public static void op_AdditionTest()
		{
			Assert.Equal(One, MathOperatorsHelper.AdditionOperation<UInt256, UInt256, UInt256>(Zero, One));
			Assert.Equal(Two, MathOperatorsHelper.AdditionOperation<UInt256, UInt256, UInt256>(One, One));
			Assert.Equal(Zero, MathOperatorsHelper.AdditionOperation<UInt256, UInt256, UInt256>(MaxValue, One));
		}
		[Fact]
		public static void op_CheckedAdditionTest()
		{
			Assert.Equal(One, MathOperatorsHelper.CheckedAdditionOperation<UInt256, UInt256, UInt256>(Zero, One));
			Assert.Equal(Two, MathOperatorsHelper.CheckedAdditionOperation<UInt256, UInt256, UInt256>(One, One));

			Assert.Throws<OverflowException>(() => MathOperatorsHelper.CheckedAdditionOperation<UInt256, UInt256, UInt256>(MaxValue, 1));
		}
		[Fact]	
		public static void op_IncrementTest()
		{
			MathOperatorsHelper.IncrementOperation<UInt256>(Zero).Should().Be(One);
			MathOperatorsHelper.IncrementOperation<UInt256>(One).Should().Be(Two);
			MathOperatorsHelper.IncrementOperation<UInt256>(MaxValueMinusTwo).Should().Be(MaxValueMinusOne);
			MathOperatorsHelper.IncrementOperation<UInt256>(MaxValueMinusOne).Should().Be(MaxValue);
			MathOperatorsHelper.IncrementOperation<UInt256>(MaxValue).Should().Be(Zero);
		}
		[Fact]	
		public static void op_CheckedIncrementTest()
		{
			MathOperatorsHelper.CheckedIncrementOperation<UInt256>(Zero).Should().Be(One);
			MathOperatorsHelper.CheckedIncrementOperation<UInt256>(One).Should().Be(Two);
			MathOperatorsHelper.CheckedIncrementOperation<UInt256>(MaxValueMinusTwo).Should().Be(MaxValueMinusOne);
			MathOperatorsHelper.CheckedIncrementOperation<UInt256>(MaxValueMinusOne).Should().Be(MaxValue);

			Assert.Throws<OverflowException>(() => MathOperatorsHelper.CheckedIncrementOperation<UInt256>(MaxValue));
		}
		[Fact]
		public static void op_SubtractionTest()
		{
			Assert.Equal(One, MathOperatorsHelper.SubtractionOperation<UInt256, UInt256, UInt256>(Two, One));
			Assert.Equal(MaxValueMinusOne, MathOperatorsHelper.SubtractionOperation<UInt256, UInt256, UInt256>(MaxValue, 1));
		}
		[Fact]
		public static void op_CheckedSubtractionTest()
		{
			Assert.Equal(One, MathOperatorsHelper.CheckedSubtractionOperation<UInt256, UInt256, UInt256>(Two, One));
			Assert.Equal(MaxValueMinusOne, MathOperatorsHelper.CheckedSubtractionOperation<UInt256, UInt256, UInt256>(MaxValue, 1));

			Assert.Throws<OverflowException>(() => MathOperatorsHelper.CheckedSubtractionOperation<UInt256, UInt256, UInt256>(Zero, 1));
		}
		[Fact]
		public static void op_DecrementTest()
		{
			MathOperatorsHelper.DecrementOperation<UInt256>(Two).Should().Be(One);
			MathOperatorsHelper.DecrementOperation<UInt256>(One).Should().Be(Zero);
			MathOperatorsHelper.DecrementOperation<UInt256>(MaxValueMinusOne).Should().Be(MaxValueMinusTwo);
			MathOperatorsHelper.DecrementOperation<UInt256>(MaxValue).Should().Be(MaxValueMinusOne);
			MathOperatorsHelper.DecrementOperation<UInt256>(Zero).Should().Be(MaxValue);
		}
		[Fact]
		public static void op_CheckedDecrementTest()
		{
			MathOperatorsHelper.CheckedDecrementOperation<UInt256>(Two).Should().Be(One);
			MathOperatorsHelper.CheckedDecrementOperation<UInt256>(One).Should().Be(Zero);
			MathOperatorsHelper.CheckedDecrementOperation<UInt256>(MaxValueMinusOne).Should().Be(MaxValueMinusTwo);
			MathOperatorsHelper.CheckedDecrementOperation<UInt256>(MaxValue).Should().Be(MaxValueMinusOne);

			Assert.Throws<OverflowException>(() => MathOperatorsHelper.CheckedDecrementOperation<UInt256>(Zero));
		}
		[Fact]
		public static void op_MultiplyTest()
		{
			Assert.Equal(One, MathOperatorsHelper.MultiplicationOperation<UInt256, UInt256, UInt256>(One, One));
			Assert.Equal(Two, MathOperatorsHelper.MultiplicationOperation<UInt256, UInt256, UInt256>(Two, One));
		}
		[Fact]
		public static void op_CheckedMultiplyTest()
		{
			Assert.Equal(One, MathOperatorsHelper.CheckedMultiplicationOperation<UInt256, UInt256, UInt256>(One, One));
			Assert.Equal(Two, MathOperatorsHelper.CheckedMultiplicationOperation<UInt256, UInt256, UInt256>(Two, One));

			Assert.Throws<OverflowException>(() => MathOperatorsHelper.CheckedMultiplicationOperation<UInt256, UInt256, UInt256>(MaxValue, Two));
		}
		[Fact]
		public static void op_DivisionTest()
		{
			Assert.Equal(new UInt256(new(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)), MathOperatorsHelper.DivisionOperation<UInt256, UInt256, UInt256>(MaxValue, Two));
			Assert.Equal(One, MathOperatorsHelper.DivisionOperation<UInt256, UInt256, UInt256>(MaxValue, MaxValue));

			Assert.Throws<DivideByZeroException>(() => MathOperatorsHelper.DivisionOperation<UInt256, UInt256, UInt256>(One, Zero));
		}
		[Fact]
		public static void op_CheckedDivisionTest()
		{
			Assert.Equal(new UInt256(new(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)), MathOperatorsHelper.CheckedDivisionOperation<UInt256, UInt256, UInt256>(MaxValue, Two));
			Assert.Equal(One, MathOperatorsHelper.CheckedDivisionOperation<UInt256, UInt256, UInt256>(MaxValue, MaxValue));

			Assert.Throws<DivideByZeroException>(() => MathOperatorsHelper.CheckedDivisionOperation<UInt256, UInt256, UInt256>(One, Zero));
		}
		[Fact]
		public static void op_ModulusTest()
		{
			MathOperatorsHelper.ModulusOperation<UInt256, UInt256, UInt256>(Two, Two).Should().Be(Zero);
			MathOperatorsHelper.ModulusOperation<UInt256, UInt256, UInt256>(One, Two).Should().NotBe(Zero);
			MathOperatorsHelper.ModulusOperation<UInt256, UInt256, UInt256>(MaxValue, new(10U)).Should().Be(5U);

			Assert.Throws<DivideByZeroException>(() => MathOperatorsHelper.ModulusOperation<UInt256, UInt256, UInt256>(One, Zero));
		}

		[Fact]
		public static void op_BitwiseAndTest()
		{
			BitwiseOperatorsHelper<UInt256, UInt256, UInt256>.BitwiseAndOperation(Zero, 1U).Should().Be(Zero);
			BitwiseOperatorsHelper<UInt256, UInt256, UInt256>.BitwiseAndOperation(One, 1U).Should().Be(One);
			BitwiseOperatorsHelper<UInt256, UInt256, UInt256>.BitwiseAndOperation(MaxValue, 1U).Should().Be(One);
		}
		[Fact]
		public static void op_BitwiseOrTest()
		{
			BitwiseOperatorsHelper<UInt256, UInt256, UInt256>.BitwiseOrOperation(Zero, 1U)
				.Should().Be(One);
			BitwiseOperatorsHelper<UInt256, UInt256, UInt256>.BitwiseOrOperation(One, 1U)
				.Should().Be(One);
			BitwiseOperatorsHelper<UInt256, UInt256, UInt256>.BitwiseOrOperation(MaxValue, 1U)
				.Should().Be(MaxValue);
		}
		[Fact]
		public static void op_ExclusiveOrTest()
		{
			BitwiseOperatorsHelper<UInt256, UInt256, UInt256>.ExclusiveOrOperation(Zero, 1U)
				.Should().Be(One);
			BitwiseOperatorsHelper<UInt256, UInt256, UInt256>.ExclusiveOrOperation(One, 1U)
				.Should().Be(Zero);
			BitwiseOperatorsHelper<UInt256, UInt256, UInt256>.ExclusiveOrOperation(MaxValue, 1U)
				.Should().Be(new(new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFE)));
		}
		[Fact]
		public static void op_OnesComplementTest()
		{
			BitwiseOperatorsHelper<UInt256, UInt256, UInt256>.OnesComplementOperation(Zero)
				.Should().Be(MaxValue);
			BitwiseOperatorsHelper<UInt256, UInt256, UInt256>.OnesComplementOperation(One)
				.Should().Be(new(new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFE)));
			BitwiseOperatorsHelper<UInt256, UInt256, UInt256>.OnesComplementOperation(MaxValue)
				.Should().Be(Zero);
		}

		[Fact]
		public static void op_LeftShiftTest()
		{
			ShiftOperatorsHelper<UInt256, int, UInt256>.LeftShiftOperation(Zero, 1)
				.Should().Be(Zero);
			ShiftOperatorsHelper<UInt256, int, UInt256>.LeftShiftOperation(One, 1)
				.Should().Be(Two);
			ShiftOperatorsHelper<UInt256, int, UInt256>.LeftShiftOperation(MaxValue, 1)
				.Should().Be(MaxValueMinusOne);
		}
		[Fact]
		public static void op_RightShiftTest()
		{
			ShiftOperatorsHelper<UInt256, int, UInt256>.RightShiftOperation(Zero, 1)
				.Should().Be(Zero);
			ShiftOperatorsHelper<UInt256, int, UInt256>.RightShiftOperation(One, 1)
				.Should().Be(Zero);
			ShiftOperatorsHelper<UInt256, int, UInt256>.RightShiftOperation(MaxValue, 1)
				.Should().Be(new(new(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)));
		}
		[Fact]
		public static void op_UnsignedRightShiftTest()
		{
			ShiftOperatorsHelper<UInt256, int, UInt256>.UnsignedRightShiftOperation(Zero, 1)
				.Should().Be(Zero);
			ShiftOperatorsHelper<UInt256, int, UInt256>.UnsignedRightShiftOperation(One, 1)
				.Should().Be(Zero);
			ShiftOperatorsHelper<UInt256, int, UInt256>.UnsignedRightShiftOperation(MaxValue, 1)
				.Should().Be(new(new(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)));
		}

		[Fact]
		public static void op_EqualityTest()
		{
			EqualityOperatorsHelper<UInt256, UInt256, bool>.EqualityOperation(Zero, 1U).Should().BeFalse();
			EqualityOperatorsHelper<UInt256, UInt256, bool>.EqualityOperation(One, 1U).Should().BeTrue();
			EqualityOperatorsHelper<UInt256, UInt256, bool>.EqualityOperation(MaxValue, 1U).Should().BeFalse();
		}
		[Fact]
		public static void op_InequalityTest()
		{
			EqualityOperatorsHelper<UInt256, UInt256, bool>.InequalityOperation(Zero, 1U).Should().BeTrue();
			EqualityOperatorsHelper<UInt256, UInt256, bool>.InequalityOperation(One, 1U).Should().BeFalse();
			EqualityOperatorsHelper<UInt256, UInt256, bool>.InequalityOperation(MaxValue, 1U).Should().BeTrue();
		}

		[Fact]
		public static void op_GreaterThanTest()
		{
			ComparisonOperatorsHelper<UInt256, UInt256, bool>.GreaterThanOperation(Zero, 1U).Should().BeFalse();
			ComparisonOperatorsHelper<UInt256, UInt256, bool>.GreaterThanOperation(One, 1U).Should().BeFalse();
			ComparisonOperatorsHelper<UInt256, UInt256, bool>.GreaterThanOperation(MaxValue, 1U).Should().BeTrue();
		}
		[Fact]
		public static void op_GreaterThanOrEqualTest()
		{
			ComparisonOperatorsHelper<UInt256, UInt256, bool>.GreaterThanOrEqualOperation(Zero, 1U).Should().BeFalse();
			ComparisonOperatorsHelper<UInt256, UInt256, bool>.GreaterThanOrEqualOperation(One, 1U).Should().BeTrue();
			ComparisonOperatorsHelper<UInt256, UInt256, bool>.GreaterThanOrEqualOperation(MaxValue, 1U).Should().BeTrue();
		}
		[Fact]
		public static void op_LessThanTest()
		{
			ComparisonOperatorsHelper<UInt256, UInt256, bool>.LessThanOperation(Zero, 1U).Should().BeTrue();
			ComparisonOperatorsHelper<UInt256, UInt256, bool>.LessThanOperation(One, 1U).Should().BeFalse();
			ComparisonOperatorsHelper<UInt256, UInt256, bool>.LessThanOperation(MaxValue, 1U).Should().BeFalse();
		}
		[Fact]
		public static void op_LessThanOrEqualTest()
		{
			ComparisonOperatorsHelper<UInt256, UInt256, bool>.LessThanOrEqualOperation(Zero, 1U).Should().BeTrue();
			ComparisonOperatorsHelper<UInt256, UInt256, bool>.LessThanOrEqualOperation(One, 1U).Should().BeTrue();
			ComparisonOperatorsHelper<UInt256, UInt256, bool>.LessThanOrEqualOperation(MaxValue, 1U).Should().BeFalse();
		}
		#endregion

		#region Identities
		[Fact]
		public static void AdditiveIdentityTest()
		{
			Assert.Equal(Zero, MathConstantsHelper.AdditiveIdentityHelper<UInt256, UInt256>());
		}

		[Fact]
		public static void MultiplicativeIdentityTest()
		{
			Assert.Equal(One, MathConstantsHelper.MultiplicativeIdentityHelper<UInt256, UInt256>());
		}
		#endregion

		#region IBinaryInteger
		[Fact]
		public static void DivRemTest()
		{
			Assert.Equal((Zero, Zero), BinaryIntegerHelper<UInt256>.DivRem(Zero, Two));
			Assert.Equal((Zero, One), BinaryIntegerHelper<UInt256>.DivRem(One, Two));
			Assert.Equal((new UInt256(new(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)), One), BinaryIntegerHelper<UInt256>.DivRem(MaxValue, 2));
		}

		[Fact]
		public static void LeadingZeroCountTest()
		{
			Assert.Equal(0x100U, BinaryIntegerHelper<UInt256>.LeadingZeroCount(Zero));
			Assert.Equal(0xFFU, BinaryIntegerHelper<UInt256>.LeadingZeroCount(One));
			Assert.Equal(0x0U, BinaryIntegerHelper<UInt256>.LeadingZeroCount(MaxValue));
		}

		[Fact]
		public static void PopCountTest()
		{
			Assert.Equal(0x00U, BinaryIntegerHelper<UInt256>.PopCount(Zero));
			Assert.Equal(0x01U, BinaryIntegerHelper<UInt256>.PopCount(One));
			Assert.Equal(0x100U, BinaryIntegerHelper<UInt256>.PopCount(MaxValue));
		}

		[Fact]
		public static void RotateLeftTest()
		{
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000)), BinaryIntegerHelper<UInt256>.RotateLeft(Zero, 1));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0002)), BinaryIntegerHelper<UInt256>.RotateLeft(One, 1));
			Assert.Equal(new(new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)), BinaryIntegerHelper<UInt256>.RotateLeft(MaxValue, 1));
		}

		[Fact]
		public static void RotateRightTest()
		{
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000)), BinaryIntegerHelper<UInt256>.RotateRight(Zero, 1));
			Assert.Equal(new(new(0x8000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000)), BinaryIntegerHelper<UInt256>.RotateRight(One, 1));
			Assert.Equal(new(new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)), BinaryIntegerHelper<UInt256>.RotateRight(MaxValue, 1));
		}

		[Fact]
		public static void TrailingZeroCountTest()
		{
			Assert.Equal(0x100U, BinaryIntegerHelper<UInt256>.TrailingZeroCount(Zero));
			Assert.Equal(0x00U, BinaryIntegerHelper<UInt256>.TrailingZeroCount(One));
			Assert.Equal(0x00U, BinaryIntegerHelper<UInt256>.TrailingZeroCount(MaxValue));
		}

		[Fact]
		public static void TryReadBigEndianInt128Test()
		{
			UInt256 result;

			Assert.True(BinaryIntegerHelper<UInt256>.TryReadBigEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: false, out result));
			Assert.Equal(new UInt128(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), result);

			Assert.True(BinaryIntegerHelper<UInt256>.TryReadBigEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 }, isUnsigned: false, out result));
			Assert.Equal(new UInt128(0x0000_0000_0000_0000, 0x0000_0000_0000_0001), result);

			Assert.True(BinaryIntegerHelper<UInt256>.TryReadBigEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80 }, isUnsigned: false, out result));
			Assert.Equal(new UInt128(0x0000_0000_0000_0000, 0x0000_0000_0000_0080), result);

			Assert.True(BinaryIntegerHelper<UInt256>.TryReadBigEndian(new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: false, out result));
			Assert.Equal(new UInt128(0x0100_0000_0000_0000, 0x0000_0000_0000_0000), result);

			Assert.True(BinaryIntegerHelper<UInt256>.TryReadBigEndian(new byte[] { 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, isUnsigned: false, out result));
			Assert.Equal(new UInt128(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), result);

			Assert.False(BinaryIntegerHelper<UInt256>.TryReadBigEndian(new byte[] { 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: false, out result));
			Assert.Equal(new UInt128(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), result);

			Assert.False(BinaryIntegerHelper<UInt256>.TryReadBigEndian(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F }, isUnsigned: false, out result));
			Assert.Equal(new UInt128(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), result);

			Assert.False(BinaryIntegerHelper<UInt256>.TryReadBigEndian(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, isUnsigned: false, out result));
			Assert.Equal(new UInt128(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), result);
		}

		[Fact]
		public static void TryReadBigEndianUInt128Test()
		{
			UInt256 result;

			Assert.True(BinaryIntegerHelper<UInt256>.TryReadBigEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: true, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000)), result);

			Assert.True(BinaryIntegerHelper<UInt256>.TryReadBigEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 }, isUnsigned: true, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0001)), result);

			Assert.True(BinaryIntegerHelper<UInt256>.TryReadBigEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80 }, isUnsigned: true, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0080)), result);

			Assert.True(BinaryIntegerHelper<UInt256>.TryReadBigEndian(new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: true, out result));
			Assert.Equal(new(new(0x0100_0000_0000_0000, 0x0000_0000_0000_0000)), result);

			Assert.True(BinaryIntegerHelper<UInt256>.TryReadBigEndian(new byte[] { 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, isUnsigned: true, out result));
			Assert.Equal(new(new(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)), result);

			Assert.True(BinaryIntegerHelper<UInt256>.TryReadBigEndian(new byte[] { 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: true, out result));
			Assert.Equal(new(new(0x8000_0000_0000_0000, 0x0000_0000_0000_0000)), result);

			Assert.True(BinaryIntegerHelper<UInt256>.TryReadBigEndian(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F }, isUnsigned: true, out result));
			Assert.Equal(new(new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FF7F)), result);

			Assert.True(BinaryIntegerHelper<UInt256>.TryReadBigEndian(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, isUnsigned: true, out result));
			Assert.Equal(new(new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)), result);
		}

		[Fact]
		public static void TryReadLittleEndianInt128Test()
		{
			UInt256 result;

			Assert.True(BinaryIntegerHelper<UInt256>.TryReadLittleEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: false, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000)), result);

			Assert.True(BinaryIntegerHelper<UInt256>.TryReadLittleEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 }, isUnsigned: false, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0100_0000_0000_0000, 0x0000_0000_0000_0000)), result);

			Assert.False(BinaryIntegerHelper<UInt256>.TryReadLittleEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80 }, isUnsigned: false, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000)), result);

			Assert.True(BinaryIntegerHelper<UInt256>.TryReadLittleEndian(new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: false, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0001)), result);

			Assert.False(BinaryIntegerHelper<UInt256>.TryReadLittleEndian(new byte[] { 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, isUnsigned: false, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000)), result);

			Assert.True(BinaryIntegerHelper<UInt256>.TryReadLittleEndian(new byte[] { 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: false, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0080)), result);

			Assert.True(BinaryIntegerHelper<UInt256>.TryReadLittleEndian(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F }, isUnsigned: false, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)), result);

			Assert.False(BinaryIntegerHelper<UInt256>.TryReadLittleEndian(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, isUnsigned: false, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000)), result);
		}

		[Fact]
		public static void TryReadLittleEndianInt192Test()
		{
			UInt256 result;

			Assert.True(BinaryIntegerHelper<UInt256>.TryReadLittleEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: false, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000)), result);

			Assert.True(BinaryIntegerHelper<UInt256>.TryReadLittleEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 }, isUnsigned: false, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0100_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000)), result);

			Assert.False(BinaryIntegerHelper<UInt256>.TryReadLittleEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80 }, isUnsigned: false, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000)), result);

			Assert.True(BinaryIntegerHelper<UInt256>.TryReadLittleEndian(new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: false, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0001)), result);

			Assert.False(BinaryIntegerHelper<UInt256>.TryReadLittleEndian(new byte[] { 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, isUnsigned: false, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000)), result);

			Assert.True(BinaryIntegerHelper<UInt256>.TryReadLittleEndian(new byte[] { 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: false, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0080)), result);

			Assert.True(BinaryIntegerHelper<UInt256>.TryReadLittleEndian(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F }, isUnsigned: false, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x7FFF_FFFF_FFFF_FFFF), new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)), result);

			Assert.False(BinaryIntegerHelper<UInt256>.TryReadLittleEndian(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, isUnsigned: false, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000)), result);
		}

		[Fact]
		public static void TryReadLittleEndianInt256Test()
		{
			UInt256 result;

			Assert.True(BinaryIntegerHelper<UInt256>.TryReadLittleEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: false, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000)), result);

			Assert.True(BinaryIntegerHelper<UInt256>.TryReadLittleEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 }, isUnsigned: false, out result));
			Assert.Equal(new(new(0x0100_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000)), result);

			Assert.False(BinaryIntegerHelper<UInt256>.TryReadLittleEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80 }, isUnsigned: false, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000)), result);

			Assert.True(BinaryIntegerHelper<UInt256>.TryReadLittleEndian(new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: false, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0001)), result);

			Assert.False(BinaryIntegerHelper<UInt256>.TryReadLittleEndian(new byte[] { 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, isUnsigned: false, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000)), result);

			Assert.True(BinaryIntegerHelper<UInt256>.TryReadLittleEndian(new byte[] { 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: false, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0080)), result);

			Assert.True(BinaryIntegerHelper<UInt256>.TryReadLittleEndian(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F }, isUnsigned: false, out result));
			Assert.Equal(new(new(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)), result);

			Assert.False(BinaryIntegerHelper<UInt256>.TryReadLittleEndian(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, isUnsigned: false, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000)), result);
		}

		[Fact]
		public static void TryReadLittleEndianUInt128Test()
		{
			UInt256 result;

			Assert.True(BinaryIntegerHelper<UInt256>.TryReadLittleEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: true, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000)), result);

			Assert.True(BinaryIntegerHelper<UInt256>.TryReadLittleEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 }, isUnsigned: true, out result));
			Assert.Equal(new(new(0x0100_0000_0000_0000, 0x0000_0000_0000_0000)), result);

			Assert.True(BinaryIntegerHelper<UInt256>.TryReadLittleEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80 }, isUnsigned: true, out result));
			Assert.Equal(new(new(0x8000_0000_0000_0000, 0x0000_0000_0000_0000)), result);

			Assert.True(BinaryIntegerHelper<UInt256>.TryReadLittleEndian(new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: true, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0001)), result);

			Assert.True(BinaryIntegerHelper<UInt256>.TryReadLittleEndian(new byte[] { 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, isUnsigned: true, out result));
			Assert.Equal(new(new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FF7F)), result);

			Assert.True(BinaryIntegerHelper<UInt256>.TryReadLittleEndian(new byte[] { 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: true, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0080)), result);

			Assert.True(BinaryIntegerHelper<UInt256>.TryReadLittleEndian(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F }, isUnsigned: true, out result));
			Assert.Equal(new(new(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)), result);

			Assert.True(BinaryIntegerHelper<UInt256>.TryReadLittleEndian(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, isUnsigned: true, out result));
			Assert.Equal(new(new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)), result);
		}

		[Fact]
		public static void TryReadLittleEndianUInt192Test()
		{
			UInt256 result;

			Assert.True(BinaryIntegerHelper<UInt256>.TryReadLittleEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: true, out result));
			Assert.Equal(new(new UInt128(0x0000_0000_0000_0000, 0x0000_0000_0000_0000)), result);

			Assert.True(BinaryIntegerHelper<UInt256>.TryReadLittleEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 }, isUnsigned: true, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0100_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000)), result);

			Assert.True(BinaryIntegerHelper<UInt256>.TryReadLittleEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80 }, isUnsigned: true, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x8000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000)), result);

			Assert.True(BinaryIntegerHelper<UInt256>.TryReadLittleEndian(new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: true, out result));
			Assert.Equal(new(new UInt128(0x0000_0000_0000_0000, 0x0000_0000_0000_0001)), result);

			Assert.True(BinaryIntegerHelper<UInt256>.TryReadLittleEndian(new byte[] { 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, isUnsigned: true, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0xFFFF_FFFF_FFFF_FFFF), new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FF7F)), result);

			Assert.True(BinaryIntegerHelper<UInt256>.TryReadLittleEndian(new byte[] { 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: true, out result));
			Assert.Equal(new(new UInt128(0x0000_0000_0000_0000, 0x0000_0000_0000_0080)), result);

			Assert.True(BinaryIntegerHelper<UInt256>.TryReadLittleEndian(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F }, isUnsigned: true, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x7FFF_FFFF_FFFF_FFFF), new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)), result);

			Assert.True(BinaryIntegerHelper<UInt256>.TryReadLittleEndian(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, isUnsigned: true, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0xFFFF_FFFF_FFFF_FFFF), new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)), result);
		}

		[Fact]
		public static void TryReadLittleEndianUInt256Test()
		{
			UInt256 result;

			Assert.True(BinaryIntegerHelper<UInt256>.TryReadLittleEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: true, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000)), result);

			Assert.True(BinaryIntegerHelper<UInt256>.TryReadLittleEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 }, isUnsigned: true, out result));
			Assert.Equal(new(new(0x0100_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000)), result);

			Assert.True(BinaryIntegerHelper<UInt256>.TryReadLittleEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80 }, isUnsigned: true, out result));
			Assert.Equal(new(new(0x8000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000)), result);

			Assert.True(BinaryIntegerHelper<UInt256>.TryReadLittleEndian(new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: true, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0001)), result);

			Assert.True(BinaryIntegerHelper<UInt256>.TryReadLittleEndian(new byte[] { 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, isUnsigned: true, out result));
			Assert.Equal(new(new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FF7F)), result);

			Assert.True(BinaryIntegerHelper<UInt256>.TryReadLittleEndian(new byte[] { 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: true, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0080)), result);

			Assert.True(BinaryIntegerHelper<UInt256>.TryReadLittleEndian(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F }, isUnsigned: true, out result));
			Assert.Equal(new(new(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)), result);

			Assert.True(BinaryIntegerHelper<UInt256>.TryReadLittleEndian(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, isUnsigned: true, out result));
			Assert.Equal(new(new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)), result);
		}

		[Fact]
		public static void GetByteCountTest()
		{
			Assert.Equal(32, BinaryIntegerHelper<UInt256>.GetByteCount(Zero));
			Assert.Equal(32, BinaryIntegerHelper<UInt256>.GetByteCount(One));
			Assert.Equal(32, BinaryIntegerHelper<UInt256>.GetByteCount(MaxValue));
		}

		[Fact]
		public static void GetShortestBitLengthTest()
		{
			Assert.Equal(0x00, BinaryIntegerHelper<UInt256>.GetShortestBitLength(Zero));
			Assert.Equal(0x01, BinaryIntegerHelper<UInt256>.GetShortestBitLength(One));
			Assert.Equal(0x100, BinaryIntegerHelper<UInt256>.GetShortestBitLength(MaxValue));
		}

		[Fact]
		public static void TryWriteBigEndianTest()
		{
			Span<byte> destination = stackalloc byte[32];
			int bytesWritten = 0;

			Assert.True(BinaryIntegerHelper<UInt256>.TryWriteBigEndian(Zero, destination, out bytesWritten));
			Assert.Equal(32, bytesWritten);
			Assert.Equal(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, destination.ToArray());

			Assert.True(BinaryIntegerHelper<UInt256>.TryWriteBigEndian(One, destination, out bytesWritten));
			Assert.Equal(32, bytesWritten);
			Assert.Equal(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 }, destination.ToArray());

			Assert.True(BinaryIntegerHelper<UInt256>.TryWriteBigEndian(MaxValue, destination, out bytesWritten));
			Assert.Equal(32, bytesWritten);
			Assert.Equal(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, destination.ToArray());

			Assert.False(BinaryIntegerHelper<UInt256>.TryWriteBigEndian(default, Span<byte>.Empty, out bytesWritten));
			Assert.Equal(0, bytesWritten);
			Assert.Equal(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, destination.ToArray());
		}

		[Fact]
		public static void TryWriteLittleEndianTest()
		{
			Span<byte> destination = stackalloc byte[32];
			int bytesWritten = 0;

			Assert.True(BinaryIntegerHelper<UInt256>.TryWriteLittleEndian(Zero, destination, out bytesWritten));
			Assert.Equal(32, bytesWritten);
			Assert.Equal(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, destination.ToArray());

			Assert.True(BinaryIntegerHelper<UInt256>.TryWriteLittleEndian(One, destination, out bytesWritten));
			Assert.Equal(32, bytesWritten);
			Assert.Equal(new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, destination.ToArray());

			Assert.True(BinaryIntegerHelper<UInt256>.TryWriteLittleEndian(MaxValue, destination, out bytesWritten));
			Assert.Equal(32, bytesWritten);
			Assert.Equal(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, destination.ToArray());

			Assert.False(BinaryIntegerHelper<UInt256>.TryWriteLittleEndian(default, Span<byte>.Empty, out bytesWritten));
			Assert.Equal(0, bytesWritten);
			Assert.Equal(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, destination.ToArray());
		}
		#endregion

		#region IBinaryNumber
		[Fact]
		public static void AllBitsSetTest()
		{
			Assert.Equal(BinaryNumberHelper<UInt256>.AllBitsSet, ~Zero);
		}
		[Fact]
		public static void IsPow2Test()
		{
			Assert.True(BinaryNumberHelper<UInt256>.IsPow2(new(0x100)));
			Assert.True(BinaryNumberHelper<UInt256>.IsPow2(new(0x1_0000)));
			Assert.True(BinaryNumberHelper<UInt256>.IsPow2(new(0x1_0000_0000)));
			Assert.True(BinaryNumberHelper<UInt256>.IsPow2(new(new(0x1, 0x0000_0000_0000_0000))));
			Assert.True(BinaryNumberHelper<UInt256>.IsPow2(new(0x1, new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000))));
		}
		[Fact]
		public static void Log2Test()
		{
			Assert.Equal(8U, BinaryNumberHelper<UInt256>.Log2(new(0x100)));
			Assert.Equal(16U, BinaryNumberHelper<UInt256>.Log2(new(0x1_0000)));
			Assert.Equal(32U, BinaryNumberHelper<UInt256>.Log2(new(0x1_0000_0000)));
			Assert.Equal(64U, BinaryNumberHelper<UInt256>.Log2(new(new(0x1, 0x0000_0000_0000_0000))));
			Assert.Equal(128U, BinaryNumberHelper<UInt256>.Log2(new(0x1, new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000))));
		}
		#endregion

		#region IMinMaxValue
		[Fact]
		public static void MaxValueTest()
		{
			MaxValue.Should().Be(MathConstantsHelper.MaxValue<UInt256>());
		}

		[Fact]
		public static void MinValueTest()
		{
			Zero.Should().Be(MathConstantsHelper.MinValue<UInt256>());
		}
		#endregion

		#region INumber
		[Fact]
		public static void ClampTest()
		{
			NumberHelper<UInt256>.Clamp(MaxValueMinusOne, UInt128MaxValue, MaxValue).Should().Be(MaxValueMinusOne);
			NumberHelper<UInt256>.Clamp(Zero, Two, MaxValue).Should().Be(Two);
			NumberHelper<UInt256>.Clamp(MaxValue, Zero, One).Should().Be(One);

			Assert.Throws<MathematicalException>(() => NumberHelper<UInt256>.Clamp(Zero, MaxValue, Zero));
		}
		[Fact]
		public static void CopySignTest()
		{
			NumberHelper<UInt256>.CopySign(MaxValue, One).Should().Be(MaxValue);
		}
		[Fact]
		public static void MaxTest()
		{
			NumberHelper<UInt256>.Max(MaxValue, Two).Should().Be(MaxValue);
			NumberHelper<UInt256>.Max(One, Zero).Should().Be(One);
			NumberHelper<UInt256>.Max(Two, Zero).Should().Be(Two);
			NumberHelper<UInt256>.Max(Two, One).Should().Be(Two);
		}
		[Fact]
		public static void MaxNumberTest()
		{
			NumberHelper<UInt256>.MaxNumber(MaxValue, Zero).Should().Be(MaxValue);
			NumberHelper<UInt256>.MaxNumber(One, Zero).Should().Be(One);
			NumberHelper<UInt256>.MaxNumber(Two, Zero).Should().Be(Two);
		}
		[Fact]
		public static void MinTest()
		{
			NumberHelper<UInt256>.Min(MaxValue, Zero).Should().Be(Zero);
			NumberHelper<UInt256>.Min(One,	Zero).Should().Be(Zero);
			NumberHelper<UInt256>.Min(Two, Zero).Should().Be(Zero);
		}
		[Fact]
		public static void MinNumberTest()
		{
			NumberHelper<UInt256>.MinNumber(MaxValue, Zero).Should().Be(Zero);
			NumberHelper<UInt256>.MinNumber(One,Zero).Should().Be(Zero);
			NumberHelper<UInt256>.MinNumber(Two, Zero).Should().Be(Zero);
		}
		[Fact]
		public static void SignTest()
		{
			NumberHelper<UInt256>.Sign(MaxValue).Should().Be(1);
			NumberHelper<UInt256>.Sign(UInt256.Zero).Should().Be(0);
		}
		#endregion

		#region INumberBase
		[Fact]
		public static void AbsTest()
		{
			NumberBaseHelper<UInt256>.Abs(One).Should().Be(One);
		}
		[Fact]
		public static void CreateCheckedToUInt256Test()
		{
			NumberBaseHelper<UInt256>.CreateChecked(byte.MaxValue).Should().Be(ByteMaxValue);
			NumberBaseHelper<UInt256>.CreateChecked(ushort.MaxValue).Should().Be(UInt16MaxValue);
			NumberBaseHelper<UInt256>.CreateChecked(uint.MaxValue).Should().Be(UInt32MaxValue);
			NumberBaseHelper<UInt256>.CreateChecked(ulong.MaxValue).Should().Be(UInt64MaxValue);
			NumberBaseHelper<UInt256>.CreateChecked(UInt128.MaxValue).Should().Be(UInt128MaxValue);
			NumberBaseHelper<UInt256>.CreateChecked(MaxValueAsDouble).Should().Be(MaxValue);

			NumberBaseHelper<UInt256>.CreateChecked(byte.MinValue).Should().Be(Zero);
			NumberBaseHelper<UInt256>.CreateChecked(ushort.MinValue).Should().Be(Zero);
			NumberBaseHelper<UInt256>.CreateChecked(uint.MinValue).Should().Be(Zero);
			NumberBaseHelper<UInt256>.CreateChecked(ulong.MinValue).Should().Be(Zero);
			NumberBaseHelper<UInt256>.CreateChecked(UInt128.MinValue).Should().Be(Zero);
		}
		[Fact]
		public static void CreateSaturatingToUInt256Test()
		{
			NumberBaseHelper<UInt256>.CreateSaturating(byte.MaxValue).Should().Be(ByteMaxValue);
			NumberBaseHelper<UInt256>.CreateSaturating(ushort.MaxValue).Should().Be(UInt16MaxValue);
			NumberBaseHelper<UInt256>.CreateSaturating(uint.MaxValue).Should().Be(UInt32MaxValue);
			NumberBaseHelper<UInt256>.CreateSaturating(ulong.MaxValue).Should().Be(UInt64MaxValue);
			NumberBaseHelper<UInt256>.CreateSaturating(UInt128.MaxValue).Should().Be(UInt128MaxValue);
			NumberBaseHelper<UInt256>.CreateSaturating(MaxValueAsDouble).Should().Be(MaxValue);

			NumberBaseHelper<UInt256>.CreateSaturating(byte.MinValue).Should().Be(Zero);
			NumberBaseHelper<UInt256>.CreateSaturating(ushort.MinValue).Should().Be(Zero);
			NumberBaseHelper<UInt256>.CreateSaturating(uint.MinValue).Should().Be(Zero);
			NumberBaseHelper<UInt256>.CreateSaturating(ulong.MinValue).Should().Be(Zero);
			NumberBaseHelper<UInt256>.CreateSaturating(UInt128.MinValue).Should().Be(Zero);
		}
		[Fact]
		public static void CreateTruncatingToUInt256Test()
		{
			NumberBaseHelper<UInt256>.CreateTruncating(byte.MaxValue).Should().Be(ByteMaxValue);
			NumberBaseHelper<UInt256>.CreateTruncating(ushort.MaxValue).Should().Be(UInt16MaxValue);
			NumberBaseHelper<UInt256>.CreateTruncating(uint.MaxValue).Should().Be(UInt32MaxValue);
			NumberBaseHelper<UInt256>.CreateTruncating(ulong.MaxValue).Should().Be(UInt64MaxValue);
			NumberBaseHelper<UInt256>.CreateTruncating(UInt128.MaxValue).Should().Be(UInt128MaxValue);
			NumberBaseHelper<UInt256>.CreateTruncating(MaxValueAsDouble).Should().Be(MaxValue);

			NumberBaseHelper<UInt256>.CreateTruncating(byte.MinValue).Should().Be(Zero);
			NumberBaseHelper<UInt256>.CreateTruncating(ushort.MinValue).Should().Be(Zero);
			NumberBaseHelper<UInt256>.CreateTruncating(uint.MinValue).Should().Be(Zero);
			NumberBaseHelper<UInt256>.CreateTruncating(ulong.MinValue).Should().Be(Zero);
			NumberBaseHelper<UInt256>.CreateTruncating(UInt128.MinValue).Should().Be(Zero);
		}

		[Fact]
		public static void CreateCheckedFromUInt256Test()
		{
			NumberBaseHelper<byte>.CreateChecked(ByteMaxValue).Should().Be(byte.MaxValue);
			NumberBaseHelper<ushort>.CreateChecked(UInt16MaxValue).Should().Be(ushort.MaxValue);
			NumberBaseHelper<uint>.CreateChecked(UInt32MaxValue).Should().Be(uint.MaxValue);
			NumberBaseHelper<ulong>.CreateChecked(UInt64MaxValue).Should().Be(ulong.MaxValue);
			NumberBaseHelper<UInt128>.CreateChecked(UInt128MaxValue).Should().Be(UInt128.MaxValue);
			NumberBaseHelper<double>.CreateChecked(MaxValue).Should().Be(MaxValueAsDouble);

			NumberBaseHelper<byte>.CreateChecked(Zero).Should().Be(byte.MinValue);
			NumberBaseHelper<ushort>.CreateChecked(Zero).Should().Be(ushort.MinValue);
			NumberBaseHelper<uint>.CreateChecked(Zero).Should().Be(uint.MinValue);
			NumberBaseHelper<ulong>.CreateChecked(Zero).Should().Be(ulong.MinValue);
			NumberBaseHelper<UInt128>.CreateChecked(Zero).Should().Be(UInt128.MinValue);
		}
		[Fact]
		public static void CreateSaturatingFromUInt256Test()
		{
			NumberBaseHelper<byte>.CreateSaturating(ByteMaxValue).Should().Be(byte.MaxValue);
			NumberBaseHelper<ushort>.CreateSaturating(UInt16MaxValue).Should().Be(ushort.MaxValue);
			NumberBaseHelper<uint>.CreateSaturating(UInt32MaxValue).Should().Be(uint.MaxValue);
			NumberBaseHelper<ulong>.CreateSaturating(UInt64MaxValue).Should().Be(ulong.MaxValue);
			NumberBaseHelper<UInt128>.CreateSaturating(UInt128MaxValue).Should().Be(UInt128.MaxValue);
			NumberBaseHelper<double>.CreateSaturating(MaxValue).Should().Be(MaxValueAsDouble);

			NumberBaseHelper<byte>.CreateSaturating(Zero).Should().Be(byte.MinValue);
			NumberBaseHelper<ushort>.CreateSaturating(Zero).Should().Be(ushort.MinValue);
			NumberBaseHelper<uint>.CreateSaturating(Zero).Should().Be(uint.MinValue);
			NumberBaseHelper<ulong>.CreateSaturating(Zero).Should().Be(ulong.MinValue);
			NumberBaseHelper<UInt128>.CreateSaturating(Zero).Should().Be(UInt128.MinValue);
		}
		[Fact]
		public static void CreateTruncatingFromUInt256Test()
		{
			NumberBaseHelper<byte>.CreateTruncating(ByteMaxValue).Should().Be(byte.MaxValue);
			NumberBaseHelper<ushort>.CreateTruncating(UInt16MaxValue).Should().Be(ushort.MaxValue);
			NumberBaseHelper<uint>.CreateTruncating(UInt32MaxValue).Should().Be(uint.MaxValue);
			NumberBaseHelper<ulong>.CreateTruncating(UInt64MaxValue).Should().Be(ulong.MaxValue);
			NumberBaseHelper<UInt128>.CreateTruncating(UInt128MaxValue).Should().Be(UInt128.MaxValue);
			NumberBaseHelper<double>.CreateTruncating(MaxValue).Should().Be(MaxValueAsDouble);

			NumberBaseHelper<byte>.CreateTruncating(Zero).Should().Be(byte.MinValue);
			NumberBaseHelper<ushort>.CreateTruncating(Zero).Should().Be(ushort.MinValue);
			NumberBaseHelper<uint>.CreateTruncating(Zero).Should().Be(uint.MinValue);
			NumberBaseHelper<ulong>.CreateTruncating(Zero).Should().Be(ulong.MinValue);
			NumberBaseHelper<UInt128>.CreateTruncating(Zero).Should().Be(UInt128.MinValue);
		}

		[Fact]
		public static void IsCanonicalTest()
		{
			NumberBaseHelper<UInt256>.IsCanonical(default(UInt256)).Should().BeTrue();
		}

		[Fact]
		public static void IsComplexNumberTest()
		{
			NumberBaseHelper<UInt256>.IsComplexNumber(default(UInt256)).Should().BeFalse();
		}

		[Fact]
		public static void IsEvenIntegerTest()
		{
			NumberBaseHelper<UInt256>.IsEvenInteger(One).Should().BeFalse();
			NumberBaseHelper<UInt256>.IsEvenInteger(Two).Should().BeTrue();
		}

		[Fact]
		public static void IsFiniteTest()
		{
			NumberBaseHelper<UInt256>.IsFinite(default(UInt256)).Should().BeTrue();
		}

		[Fact]
		public static void IsImaginaryNumberTest()
		{
			NumberBaseHelper<UInt256>.IsImaginaryNumber(default(UInt256)).Should().BeFalse();
		}

		[Fact]
		public static void IsInfinityTest()
		{
			NumberBaseHelper<UInt256>.IsInfinity(default(UInt256)).Should().BeFalse();
		}

		[Fact]
		public static void IsIntegerTest()
		{
			NumberBaseHelper<UInt256>.IsInteger(default(UInt256)).Should().BeTrue();
		}

		[Fact]
		public static void IsNaNTest()
		{
			NumberBaseHelper<UInt256>.IsNaN(default(UInt256)).Should().BeFalse();
		}

		[Fact]
		public static void IsNegativeTest()
		{
			NumberBaseHelper<UInt256>.IsNegative(One).Should().BeFalse();
		}

		[Fact]
		public static void IsNegativeInfinityTest()
		{
			NumberBaseHelper<UInt256>.IsNegativeInfinity(One).Should().BeFalse();
		}

		[Fact]
		public static void IsNormalTest()
		{
			NumberBaseHelper<UInt256>.IsNormal(Zero).Should().BeFalse();
			NumberBaseHelper<UInt256>.IsNormal(One).Should().BeTrue();
		}

		[Fact]
		public static void IsOddIntegerTest()
		{
			NumberBaseHelper<UInt256>.IsOddInteger(One).Should().BeTrue();
			NumberBaseHelper<UInt256>.IsOddInteger(Two).Should().BeFalse();
		}

		[Fact]
		public static void IsPositiveTest()
		{
			NumberBaseHelper<UInt256>.IsPositive(One).Should().BeTrue();
		}

		[Fact]
		public static void IsPositiveInfinityTest()
		{
			NumberBaseHelper<UInt256>.IsPositiveInfinity(One).Should().BeFalse();
		}

		[Fact]
		public static void IsRealNumberTest()
		{
			NumberBaseHelper<UInt256>.IsRealNumber(default).Should().BeTrue();
		}

		[Fact]
		public static void IsSubnormalTest()
		{
			NumberBaseHelper<UInt256>.IsSubnormal(default).Should().BeFalse();
		}

		[Fact]
		public static void IsZeroTest()
		{
			NumberBaseHelper<UInt256>.IsZero(default).Should().BeTrue();
			NumberBaseHelper<UInt256>.IsZero(Zero).Should().BeTrue();
			NumberBaseHelper<UInt256>.IsZero(One).Should().BeFalse();
		}

		[Fact]
		public static void MaxMagnitudeTest()
		{
			NumberBaseHelper<UInt256>.MaxMagnitude(MaxValue, Zero).Should().Be(MaxValue);
			NumberBaseHelper<UInt256>.MaxMagnitude(One, Zero).Should().Be(One);
			NumberBaseHelper<UInt256>.MaxMagnitude(Two, Zero).Should().Be(Two);
		}

		[Fact]
		public static void MaxMagnitudeNumberTest()
		{
			NumberBaseHelper<UInt256>.MaxMagnitudeNumber(MaxValue, Zero).Should().Be(MaxValue);
			NumberBaseHelper<UInt256>.MaxMagnitudeNumber(One, Zero).Should().Be(One);
			NumberBaseHelper<UInt256>.MaxMagnitudeNumber(Two, Zero).Should().Be(Two);
		}

		[Fact]
		public static void MinMagnitudeTest()
		{
			NumberBaseHelper<UInt256>.MinMagnitude(MaxValue, MaxValueMinusOne).Should().Be(MaxValueMinusOne);
			NumberBaseHelper<UInt256>.MinMagnitude(One, Zero).Should().Be(Zero);
			NumberBaseHelper<UInt256>.MinMagnitude(Two, Zero).Should().Be(Zero);
		}

		[Fact]
		public static void MinMagnitudeNumberTest()
		{
			NumberBaseHelper<UInt256>.MinMagnitudeNumber(MaxValue, MaxValueMinusOne).Should().Be(MaxValueMinusOne);
			NumberBaseHelper<UInt256>.MinMagnitudeNumber(One, Zero).Should().Be(Zero);
			NumberBaseHelper<UInt256>.MinMagnitudeNumber(Two, Zero).Should().Be(Zero);
		}

		[Fact]
		public void ParseTest()
		{
			NumberBaseHelper<UInt256>.Parse("115792089237316195423570985008687907853269984665640564039457584007913129639935", System.Globalization.NumberStyles.Integer, CultureInfo.CurrentCulture)
				.Should().Be(MaxValue)
				.And.BeRankedEquallyTo(MaxValue);
			NumberBaseHelper<UInt256>.Parse("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", System.Globalization.NumberStyles.HexNumber, CultureInfo.CurrentCulture)
				.Should().Be(MaxValue)
				.And.BeRankedEquallyTo(MaxValue);

			Assert.Throws<FormatException>(() => NumberBaseHelper<UInt256>.Parse("115792089237316195423570985008687907853269984665640564039457584007913129639936", System.Globalization.NumberStyles.Integer, CultureInfo.CurrentCulture));
		}

		[Fact]
		public void TryParseTest()
		{
			NumberBaseHelper<UInt256>.TryParse("115792089237316195423570985008687907853269984665640564039457584007913129639935", System.Globalization.NumberStyles.Integer, CultureInfo.CurrentCulture, out UInt256 parsedValue)
				.Should().BeTrue();
			parsedValue.Should().Be(MaxValue)
				.And.BeRankedEquallyTo(MaxValue);
			NumberBaseHelper<UInt256>.TryParse("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", System.Globalization.NumberStyles.HexNumber, CultureInfo.CurrentCulture, out parsedValue)
				.Should().BeTrue();
			parsedValue.Should().Be(MaxValue)
				.And.BeRankedEquallyTo(MaxValue);

			NumberBaseHelper<UInt256>.TryParse("115792089237316195423570985008687907853269984665640564039457584007913129639936", System.Globalization.NumberStyles.Integer, CultureInfo.CurrentCulture, out parsedValue)
				.Should().BeFalse();
			parsedValue.Should().Be(default);
		}
		#endregion
	}
}
