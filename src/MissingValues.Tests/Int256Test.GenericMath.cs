using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues.Tests
{
	public partial class Int256Test
	{
		#region Readonly Variables
		private const double Int256MaxValueAsDouble = 57896044618658097711785492504343953926634992332820282019728792003956564819967.0;
		private const double Int256MinValueAsDouble = -57896044618658097711785492504343953926634992332820282019728792003956564819968.0d;

		private static readonly Int256 ByteMaxValue = new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_00FF));

		private static readonly Int256 Int16MaxValue = new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_7FFF));
		private static readonly Int256 Int16MinValue = new(new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_8000));

		private static readonly Int256 Int32MaxValue = new(new(0x0000_0000_0000_0000, 0x0000_0000_7FFF_FFFF));
		private static readonly Int256 Int32MinValue = new(new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_8000_0000));

		private static readonly Int256 Int64MaxValue = new(new(0x0000_0000_0000_0000, 0x7FFF_FFFF_FFFF_FFFF));
		private static readonly Int256 Int64MinValue = new(new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new(0xFFFF_FFFF_FFFF_FFFF, 0x8000_0000_0000_0000));

		private static readonly Int256 Int128MaxValue = new(new(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF));
		private static readonly Int256 Int128MinValue = new(new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new(0x8000_0000_0000_0000, 0x0000_0000_0000_0000));

		private static readonly Int256 NegativeOne = Int256.NegativeOne;
		private static readonly Int256 NegativeTwo = new(new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFE));
		private static readonly Int256 Zero = Int256.Zero;
		private static readonly Int256 One = Int256.One;
		private static readonly Int256 Two = new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0002));
		private static readonly Int256 MaxValue = Int256.MaxValue;
		private static readonly Int256 MaxValueMinusOne = new(new(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFE));
		private static readonly Int256 MinValue = Int256.MinValue;
		private static readonly Int256 MinValuePlusOne = new(new(0x8000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0001));
		#endregion

		#region Generic Math Operators
		[Fact]
		public static void op_AdditionTest()
		{
			Assert.Equal(One, MathOperatorsHelper.AdditionOperation<Int256, Int256, Int256>(Zero, 1));
			Assert.Equal(Two, MathOperatorsHelper.AdditionOperation<Int256, Int256, Int256>(One, 1));
			Assert.Equal(MinValue, MathOperatorsHelper.AdditionOperation<Int256, Int256, Int256>(MaxValue, 1));
			Assert.Equal(MinValuePlusOne, MathOperatorsHelper.AdditionOperation<Int256, Int256, Int256>(MinValue, 1));
			Assert.Equal(Zero, MathOperatorsHelper.AdditionOperation<Int256, Int256, Int256>(NegativeOne, 1));
		}
		[Fact]
		public static void op_CheckedAdditionTest()
		{
			Assert.Equal(One, MathOperatorsHelper.CheckedAdditionOperation<Int256, Int256, Int256>(Zero, 1));
			Assert.Equal(Two, MathOperatorsHelper.CheckedAdditionOperation<Int256, Int256, Int256>(Int256.One, 1));
			Assert.Equal(MinValuePlusOne, MathOperatorsHelper.CheckedAdditionOperation<Int256, Int256, Int256>(MinValue, 1));
			Assert.Equal(Zero, MathOperatorsHelper.CheckedAdditionOperation<Int256, Int256, Int256>(NegativeOne, 1));

			Assert.Throws<OverflowException>(() => MathOperatorsHelper.CheckedAdditionOperation<Int256, Int256, Int256>(MaxValue, 1));
		}
		[Fact]
		public static void op_IncrementTest()
		{
			MathOperatorsHelper.IncrementOperation<Int256>(Zero).Should().Be(One);
			MathOperatorsHelper.IncrementOperation<Int256>(One).Should().Be(Two);
			MathOperatorsHelper.IncrementOperation<Int256>(MinValue).Should().Be(MinValuePlusOne);
			MathOperatorsHelper.IncrementOperation<Int256>(MaxValueMinusOne).Should().Be(MaxValue);
			MathOperatorsHelper.IncrementOperation<Int256>(MaxValue).Should().Be(MinValue);
		}
		[Fact]
		public static void op_CheckedIncrementTest()
		{
			MathOperatorsHelper.CheckedIncrementOperation<Int256>(Zero).Should().Be(One);
			MathOperatorsHelper.CheckedIncrementOperation<Int256>(One).Should().Be(Two);
			MathOperatorsHelper.CheckedIncrementOperation<Int256>(MinValue).Should().Be(MinValuePlusOne);
			MathOperatorsHelper.CheckedIncrementOperation<Int256>(MaxValueMinusOne).Should().Be(MaxValue);

			Assert.Throws<OverflowException>(() => MathOperatorsHelper.CheckedIncrementOperation<Int256>(MaxValue));
		}
		[Fact]
		public static void op_SubtractionTest()
		{
			Assert.Equal(NegativeOne, MathOperatorsHelper.SubtractionOperation<Int256, Int256, Int256>(Zero, 1));
			Assert.Equal(One, MathOperatorsHelper.SubtractionOperation<Int256, Int256, Int256>(Two, 1));
			Assert.Equal(MaxValue, MathOperatorsHelper.SubtractionOperation<Int256, Int256, Int256>(MinValue, 1));
			Assert.Equal(MaxValueMinusOne, MathOperatorsHelper.SubtractionOperation<Int256, Int256, Int256>(MaxValue, 1));
			Assert.Equal(NegativeTwo, MathOperatorsHelper.SubtractionOperation<Int256, Int256, Int256>(NegativeOne, 1));
		}
		[Fact]
		public static void op_CheckedSubtractionTest()
		{
			Assert.Equal(NegativeOne, MathOperatorsHelper.CheckedSubtractionOperation<Int256, Int256, Int256>(Zero, 1));
			Assert.Equal(One, MathOperatorsHelper.CheckedSubtractionOperation<Int256, Int256, Int256>(Two, 1));
			Assert.Equal(MaxValueMinusOne, MathOperatorsHelper.CheckedSubtractionOperation<Int256, Int256, Int256>(MaxValue, 1));
			Assert.Equal(NegativeTwo, MathOperatorsHelper.CheckedSubtractionOperation<Int256, Int256, Int256>(NegativeOne, 1));

			Assert.Throws<OverflowException>(() => MathOperatorsHelper.CheckedSubtractionOperation<Int256, Int256, Int256>(MinValue, 1));
		}
		[Fact]
		public static void op_DecrementTest()
		{
			MathOperatorsHelper.DecrementOperation<Int256>(Two).Should().Be(One);
			MathOperatorsHelper.DecrementOperation<Int256>(One).Should().Be(Zero);
			MathOperatorsHelper.DecrementOperation<Int256>(MinValuePlusOne).Should().Be(MinValue);
			MathOperatorsHelper.DecrementOperation<Int256>(MaxValue).Should().Be(MaxValueMinusOne);
			MathOperatorsHelper.DecrementOperation<Int256>(MinValue).Should().Be(MaxValue);
		}
		[Fact]
		public static void op_CheckedDecrementTest()
		{
			MathOperatorsHelper.CheckedDecrementOperation<Int256>(Two).Should().Be(One);
			MathOperatorsHelper.CheckedDecrementOperation<Int256>(One).Should().Be(Zero);
			MathOperatorsHelper.CheckedDecrementOperation<Int256>(MinValuePlusOne).Should().Be(MinValue);
			MathOperatorsHelper.CheckedDecrementOperation<Int256>(MaxValue).Should().Be(MaxValueMinusOne);

			Assert.Throws<OverflowException>(() => MathOperatorsHelper.CheckedDecrementOperation<Int256>(MinValue));
		}
		[Fact]
		public static void op_MultiplyTest()
		{
			Assert.Equal(One, MathOperatorsHelper.MultiplicationOperation<Int256, Int256, Int256>(One, One));
			Assert.Equal(Two, MathOperatorsHelper.MultiplicationOperation<Int256, Int256, Int256>(Two, One));
			Assert.Equal(NegativeTwo, MathOperatorsHelper.MultiplicationOperation<Int256, Int256, Int256>(Two, NegativeOne));
			Assert.Equal(MinValuePlusOne, MathOperatorsHelper.MultiplicationOperation<Int256, Int256, Int256>(MaxValue, NegativeOne));
			Assert.Equal(MinValue, MathOperatorsHelper.MultiplicationOperation<Int256, Int256, Int256>(MinValue, NegativeOne));
		}
		[Fact]
		public static void op_CheckedMultiplyTest()
		{
			Assert.Equal(One, MathOperatorsHelper.CheckedMultiplicationOperation<Int256, Int256, Int256>(One, One));
			Assert.Equal(Two, MathOperatorsHelper.CheckedMultiplicationOperation<Int256, Int256, Int256>(Two, One));
			Assert.Equal(NegativeTwo, MathOperatorsHelper.CheckedMultiplicationOperation<Int256, Int256, Int256>(Two, NegativeOne));
			Assert.Equal(MinValuePlusOne, MathOperatorsHelper.CheckedMultiplicationOperation<Int256, Int256, Int256>(MaxValue, NegativeOne));

			Assert.Throws<OverflowException>(() => MathOperatorsHelper.CheckedMultiplicationOperation<Int256, Int256, Int256>(MinValue, NegativeOne));
		}
		[Fact]
		public static void op_DivisionTest()
		{
			Assert.Equal(new Int256(new(0x3FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)), MathOperatorsHelper.DivisionOperation<Int256, Int256, Int256>(MaxValue, Two));
			Assert.Equal(new Int256(new(0xC000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0001)), MathOperatorsHelper.DivisionOperation<Int256, Int256, Int256>(MaxValueMinusOne, NegativeTwo));
			Assert.Equal(One, MathOperatorsHelper.DivisionOperation<Int256, Int256, Int256>(MaxValue, MaxValue));
			Assert.Equal(NegativeOne, MathOperatorsHelper.DivisionOperation<Int256, Int256, Int256>(MaxValue, MinValuePlusOne));

			Assert.Throws<OverflowException>(() => MathOperatorsHelper.DivisionOperation<Int256, Int256, Int256>(MinValue, NegativeOne));
		}
		[Fact]
		public static void op_CheckedDivisionTest()
		{
			Assert.Equal(new Int256(new(0x3FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)), MathOperatorsHelper.CheckedDivisionOperation<Int256, Int256, Int256>(MaxValue, Two));
			Assert.Equal(new Int256(new(0xC000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0001)), MathOperatorsHelper.CheckedDivisionOperation<Int256, Int256, Int256>(MaxValueMinusOne, NegativeTwo));
			Assert.Equal(One, MathOperatorsHelper.CheckedDivisionOperation<Int256, Int256, Int256>(MaxValue, MaxValue));
			Assert.Equal(NegativeOne, MathOperatorsHelper.CheckedDivisionOperation<Int256, Int256, Int256>(MaxValue, MinValuePlusOne));

			Assert.Throws<OverflowException>(() => MathOperatorsHelper.CheckedDivisionOperation<Int256, Int256, Int256>(MinValue, NegativeOne));
		}
		[Fact]
		public static void op_ModulusTest()
		{
			MathOperatorsHelper.ModulusOperation<Int256, Int256, Int256>(Two, Two).Should().Be(Zero);
			MathOperatorsHelper.ModulusOperation<Int256, Int256, Int256>(One, Two).Should().NotBe(Zero);
			MathOperatorsHelper.ModulusOperation<Int256, Int256, Int256>(MaxValue, new(10U)).Should().Be(7);
			MathOperatorsHelper.ModulusOperation<Int256, Int256, Int256>(MinValue, new(10U)).Should().Be(-8);

			Assert.Throws<DivideByZeroException>(() => MathOperatorsHelper.ModulusOperation<Int256, Int256, Int256>(One, Zero));
		}

		[Fact]
		public static void op_BitwiseAndTest()
		{
			BitwiseOperatorsHelper<Int256, Int256, Int256>.BitwiseAndOperation(Zero, 1U).Should().Be(Zero);
			BitwiseOperatorsHelper<Int256, Int256, Int256>.BitwiseAndOperation(One, 1U).Should().Be(One);
			BitwiseOperatorsHelper<Int256, Int256, Int256>.BitwiseAndOperation(MaxValue, 1U).Should().Be(One);
			BitwiseOperatorsHelper<Int256, Int256, Int256>.BitwiseAndOperation(MinValue, 1U).Should().Be(Zero);
			BitwiseOperatorsHelper<Int256, Int256, Int256>.BitwiseAndOperation(NegativeOne, 1U).Should().Be(One);
		}
		[Fact]
		public static void op_BitwiseOrTest()
		{
			BitwiseOperatorsHelper<Int256, Int256, Int256>.BitwiseOrOperation(Zero, 1U)
				.Should().Be(One);
			BitwiseOperatorsHelper<Int256, Int256, Int256>.BitwiseOrOperation(One, 1U)
				.Should().Be(One);
			BitwiseOperatorsHelper<Int256, Int256, Int256>.BitwiseOrOperation(MaxValue, 1U)
				.Should().Be(MaxValue);
			BitwiseOperatorsHelper<Int256, Int256, Int256>.BitwiseOrOperation(MinValue, 1U)
				.Should().Be(new(new(0x8000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0001)));
			BitwiseOperatorsHelper<Int256, Int256, Int256>.BitwiseOrOperation(NegativeOne, 1U)
				.Should().Be(new(new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)));
		}
		[Fact]
		public static void op_ExclusiveOrTest()
		{
			BitwiseOperatorsHelper<Int256, Int256, Int256>.ExclusiveOrOperation(Zero, 1U)
				.Should().Be(One);
			BitwiseOperatorsHelper<Int256, Int256, Int256>.ExclusiveOrOperation(One, 1U)
				.Should().Be(Zero);
			BitwiseOperatorsHelper<Int256, Int256, Int256>.ExclusiveOrOperation(MaxValue, 1U)
				.Should().Be(new(new(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFE)));
			BitwiseOperatorsHelper<Int256, Int256, Int256>.ExclusiveOrOperation(MinValue, 1U)
				.Should().Be(new(new(0x8000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0001)));
			BitwiseOperatorsHelper<Int256, Int256, Int256>.ExclusiveOrOperation(NegativeOne, 1U)
				.Should().Be(new(new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFE)));
		}
		[Fact]
		public static void op_OnesComplementTest()
		{
			BitwiseOperatorsHelper<Int256, Int256, Int256>.OnesComplementOperation(Zero)
				.Should().Be(new(new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)));
			BitwiseOperatorsHelper<Int256, Int256, Int256>.OnesComplementOperation(One)
				.Should().Be(new(new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFE)));
			BitwiseOperatorsHelper<Int256, Int256, Int256>.OnesComplementOperation(MaxValue)
				.Should().Be(new(new(0x8000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000)));
			BitwiseOperatorsHelper<Int256, Int256, Int256>.OnesComplementOperation(MinValue)
				.Should().Be(new(new(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)));
			BitwiseOperatorsHelper<Int256, Int256, Int256>.OnesComplementOperation(NegativeOne)
				.Should().Be(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000)));
		}

		[Fact]
		public static void op_LeftShiftTest()
		{
			ShiftOperatorsHelper<Int256, int, Int256>.LeftShiftOperation(One, 1)
				.Should().Be(Two);
			ShiftOperatorsHelper<Int256, int, Int256>.LeftShiftOperation(MaxValue, 1)
				.Should().Be(NegativeTwo);
			ShiftOperatorsHelper<Int256, int, Int256>.LeftShiftOperation(MinValue, 1)
				.Should().Be(Zero);
			ShiftOperatorsHelper<Int256, int, Int256>.LeftShiftOperation(NegativeOne, 1)
				.Should().Be(NegativeTwo);
		}
		[Fact]
		public static void op_RightShiftTest()
		{
			ShiftOperatorsHelper<Int256, int, Int256>.RightShiftOperation(Zero, 1)
				.Should().Be(Zero);
			ShiftOperatorsHelper<Int256, int, Int256>.RightShiftOperation(One, 1)
				.Should().Be(Zero);
			ShiftOperatorsHelper<Int256, int, Int256>.RightShiftOperation(MaxValue, 1)
				.Should().Be(new(new(0x3FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)));
			ShiftOperatorsHelper<Int256, int, Int256>.RightShiftOperation(MinValue, 1)
				.Should().Be(new(new(0xC000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000)));
			ShiftOperatorsHelper<Int256, int, Int256>.RightShiftOperation(NegativeOne, 1)
				.Should().Be(NegativeOne);
		}
		[Fact]
		public static void op_UnsignedRightShiftTest()
		{
			ShiftOperatorsHelper<Int256, int, Int256>.UnsignedRightShiftOperation(Zero, 1)
				.Should().Be(Zero);
			ShiftOperatorsHelper<Int256, int, Int256>.UnsignedRightShiftOperation(One, 1)
				.Should().Be(Zero);
			ShiftOperatorsHelper<Int256, int, Int256>.UnsignedRightShiftOperation(MaxValue, 1)
				.Should().Be(new(new(0x3FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)));
			ShiftOperatorsHelper<Int256, int, Int256>.UnsignedRightShiftOperation(MinValue, 1)
				.Should().Be(new(new(0x4000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000)));
			ShiftOperatorsHelper<Int256, int, Int256>.UnsignedRightShiftOperation(NegativeOne, 1)
				.Should().Be(MaxValue);
		}

		[Fact]
		public static void op_EqualityTest()
		{
			EqualityOperatorsHelper<Int256, Int256, bool>.EqualityOperation(Zero, 1U).Should().BeFalse();
			EqualityOperatorsHelper<Int256, Int256, bool>.EqualityOperation(One, 1U).Should().BeTrue();
			EqualityOperatorsHelper<Int256, Int256, bool>.EqualityOperation(MaxValue, 1U).Should().BeFalse();
			EqualityOperatorsHelper<Int256, Int256, bool>.EqualityOperation(MinValue, 1U).Should().BeFalse();
			EqualityOperatorsHelper<Int256, Int256, bool>.EqualityOperation(NegativeOne, 1U).Should().BeFalse();
		}
		[Fact]
		public static void op_InequalityTest()
		{
			EqualityOperatorsHelper<Int256, Int256, bool>.InequalityOperation(Zero, 1U).Should().BeTrue();
			EqualityOperatorsHelper<Int256, Int256, bool>.InequalityOperation(One, 1U).Should().BeFalse();
			EqualityOperatorsHelper<Int256, Int256, bool>.InequalityOperation(MaxValue, 1U).Should().BeTrue();
			EqualityOperatorsHelper<Int256, Int256, bool>.InequalityOperation(MinValue, 1U).Should().BeTrue();
			EqualityOperatorsHelper<Int256, Int256, bool>.InequalityOperation(NegativeOne, 1U).Should().BeTrue();
		}

		[Fact]
		public static void op_GreaterThanTest()
		{
			ComparisonOperatorsHelper<Int256, Int256, bool>.GreaterThanOperation(Zero, 1U).Should().BeFalse();
			ComparisonOperatorsHelper<Int256, Int256, bool>.GreaterThanOperation(One, 1U).Should().BeFalse();
			ComparisonOperatorsHelper<Int256, Int256, bool>.GreaterThanOperation(MaxValue, 1U).Should().BeTrue();
			ComparisonOperatorsHelper<Int256, Int256, bool>.GreaterThanOperation(MinValue, 1U).Should().BeFalse();
			ComparisonOperatorsHelper<Int256, Int256, bool>.GreaterThanOperation(NegativeOne, 1U).Should().BeFalse();
		}
		[Fact]
		public static void op_GreaterThanOrEqualTest()
		{
			ComparisonOperatorsHelper<Int256, Int256, bool>.GreaterThanOrEqualOperation(Zero, 1U).Should().BeFalse();
			ComparisonOperatorsHelper<Int256, Int256, bool>.GreaterThanOrEqualOperation(One, 1U).Should().BeTrue();
			ComparisonOperatorsHelper<Int256, Int256, bool>.GreaterThanOrEqualOperation(MaxValue, 1U).Should().BeTrue();
			ComparisonOperatorsHelper<Int256, Int256, bool>.GreaterThanOrEqualOperation(MinValue, 1U).Should().BeFalse();
			ComparisonOperatorsHelper<Int256, Int256, bool>.GreaterThanOrEqualOperation(NegativeOne, 1U).Should().BeFalse();
		}
		[Fact]
		public static void op_LessThanTest()
		{
			ComparisonOperatorsHelper<Int256, Int256, bool>.LessThanOperation(Zero, 1U).Should().BeTrue();
			ComparisonOperatorsHelper<Int256, Int256, bool>.LessThanOperation(One, 1U).Should().BeFalse();
			ComparisonOperatorsHelper<Int256, Int256, bool>.LessThanOperation(MaxValue, 1U).Should().BeFalse();
			ComparisonOperatorsHelper<Int256, Int256, bool>.LessThanOperation(MinValue, 1U).Should().BeTrue();
			ComparisonOperatorsHelper<Int256, Int256, bool>.LessThanOperation(NegativeOne, 1U).Should().BeTrue();
		}
		[Fact]
		public static void op_LessThanOrEqualTest()
		{
			ComparisonOperatorsHelper<Int256, Int256, bool>.LessThanOrEqualOperation(Zero, 1U).Should().BeTrue();
			ComparisonOperatorsHelper<Int256, Int256, bool>.LessThanOrEqualOperation(One, 1U).Should().BeTrue();
			ComparisonOperatorsHelper<Int256, Int256, bool>.LessThanOrEqualOperation(MaxValue, 1U).Should().BeFalse();
			ComparisonOperatorsHelper<Int256, Int256, bool>.LessThanOrEqualOperation(MinValue, 1U).Should().BeTrue();
			ComparisonOperatorsHelper<Int256, Int256, bool>.LessThanOrEqualOperation(NegativeOne, 1U).Should().BeTrue();
		}
		#endregion

		#region Identities
		[Fact]
		public static void AdditiveIdentityTest()
		{
			Assert.Equal(Zero, MathConstantsHelper.AdditiveIdentityHelper<Int256, Int256>());
		}

		[Fact]
		public static void MultiplicativeIdentityTest()
		{
			Assert.Equal(One, MathConstantsHelper.MultiplicativeIdentityHelper<Int256, Int256>());
		} 
		#endregion

		#region IBinaryInteger
		[Fact]
		public static void DivRemTest()
		{
			Assert.Equal((Zero, Zero), BinaryIntegerHelper<Int256>.DivRem(Zero, Two));
			Assert.Equal((Zero, One), BinaryIntegerHelper<Int256>.DivRem(One, Two));
			Assert.Equal((new Int256(new(0x3FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)), One), BinaryIntegerHelper<Int256>.DivRem(MaxValue, 2));
			Assert.Equal((new Int256(new(0xC000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000)), Zero), BinaryIntegerHelper<Int256>.DivRem(MinValue, 2));
			Assert.Equal((Zero, NegativeOne), BinaryIntegerHelper<Int256>.DivRem(NegativeOne, 2));
		}

		[Fact]
		public static void LeadingZeroCountTest()
		{
			Assert.Equal(0x100, BinaryIntegerHelper<Int256>.LeadingZeroCount(Zero));
			Assert.Equal(0xfF, BinaryIntegerHelper<Int256>.LeadingZeroCount(One));
			Assert.Equal(0x01, BinaryIntegerHelper<Int256>.LeadingZeroCount(MaxValue));
			Assert.Equal(0x00, BinaryIntegerHelper<Int256>.LeadingZeroCount(MinValue));
			Assert.Equal(0x00, BinaryIntegerHelper<Int256>.LeadingZeroCount(NegativeOne));
		}

		[Fact]
		public static void PopCountTest()
		{
			Assert.Equal(0x00, BinaryIntegerHelper<Int256>.PopCount(Zero));
			Assert.Equal(0x01, BinaryIntegerHelper<Int256>.PopCount(One));
			Assert.Equal(0xFF, BinaryIntegerHelper<Int256>.PopCount(MaxValue));
			Assert.Equal(0x01, BinaryIntegerHelper<Int256>.PopCount(MinValue));
			Assert.Equal(0x100, BinaryIntegerHelper<Int256>.PopCount(NegativeOne));
		}

		[Fact]
		public static void RotateLeftTest()
		{
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000)), BinaryIntegerHelper<Int256>.RotateLeft(Zero, 1));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0002)), BinaryIntegerHelper<Int256>.RotateLeft(One, 1));
			Assert.Equal(new(new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFE)), BinaryIntegerHelper<Int256>.RotateLeft(MaxValue, 1));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0001)), BinaryIntegerHelper<Int256>.RotateLeft(MinValue, 1));
			Assert.Equal(new(new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)), BinaryIntegerHelper<Int256>.RotateLeft(NegativeOne, 1));
		}

		[Fact]
		public static void RotateRightTest()
		{
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000)), BinaryIntegerHelper<Int256>.RotateRight(Zero, 1));
			Assert.Equal(new(new(0x8000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000)), BinaryIntegerHelper<Int256>.RotateRight(One, 1));
			Assert.Equal(new(new(0xBFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)), BinaryIntegerHelper<Int256>.RotateRight(MaxValue, 1));
			Assert.Equal(new(new(0x4000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000)), BinaryIntegerHelper<Int256>.RotateRight(MinValue, 1));
			Assert.Equal(new(new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)), BinaryIntegerHelper<Int256>.RotateRight(NegativeOne, 1));
		}

		[Fact]
		public static void TrailingZeroCountTest()
		{
			Assert.Equal(0x100, BinaryIntegerHelper<Int256>.TrailingZeroCount(Zero));
			Assert.Equal(0x00, BinaryIntegerHelper<Int256>.TrailingZeroCount(One));
			Assert.Equal(0x00, BinaryIntegerHelper<Int256>.TrailingZeroCount(MaxValue));
			Assert.Equal(0xFF, BinaryIntegerHelper<Int256>.TrailingZeroCount(MinValue));
			Assert.Equal(0x00, BinaryIntegerHelper<Int256>.TrailingZeroCount(NegativeOne));
		}

		[Fact]
		public static void TryReadBigEndianInt128Test()
		{
			Int256 result;

			Assert.True(BinaryIntegerHelper<Int256>.TryReadBigEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: false, out result));
			Assert.Equal(new Int128(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), result);

			Assert.True(BinaryIntegerHelper<Int256>.TryReadBigEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 }, isUnsigned: false, out result));
			Assert.Equal(new Int128(0x0000_0000_0000_0000, 0x0000_0000_0000_0001), result);

			Assert.True(BinaryIntegerHelper<Int256>.TryReadBigEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80 }, isUnsigned: false, out result));
			Assert.Equal(new Int128(0x0000_0000_0000_0000, 0x0000_0000_0000_0080), result);

			Assert.True(BinaryIntegerHelper<Int256>.TryReadBigEndian(new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: false, out result));
			Assert.Equal(new Int128(0x0100_0000_0000_0000, 0x0000_0000_0000_0000), result);

			Assert.True(BinaryIntegerHelper<Int256>.TryReadBigEndian(new byte[] { 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, isUnsigned: false, out result));
			Assert.Equal(new Int128(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), result);

			Assert.True(BinaryIntegerHelper<Int256>.TryReadBigEndian(new byte[] { 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: false, out result));
			Assert.Equal(new Int128(0x8000_0000_0000_0000, 0x0000_0000_0000_0000), result);

			Assert.True(BinaryIntegerHelper<Int256>.TryReadBigEndian(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F }, isUnsigned: false, out result));
			Assert.Equal(new Int128(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FF7F), result);

			Assert.True(BinaryIntegerHelper<Int256>.TryReadBigEndian(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, isUnsigned: false, out result));
			Assert.Equal(new Int128(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), result);
		}

		[Fact]
		public static void TryReadBigEndianUInt128Test()
		{
			Int256 result;

			Assert.True(BinaryIntegerHelper<Int256>.TryReadBigEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: true, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000)), result);

			Assert.True(BinaryIntegerHelper<Int256>.TryReadBigEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 }, isUnsigned: true, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0001)), result);

			Assert.True(BinaryIntegerHelper<Int256>.TryReadBigEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80 }, isUnsigned: true, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0080)), result);

			Assert.True(BinaryIntegerHelper<Int256>.TryReadBigEndian(new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: true, out result));
			Assert.Equal(new(new(0x0100_0000_0000_0000, 0x0000_0000_0000_0000)), result);

			Assert.True(BinaryIntegerHelper<Int256>.TryReadBigEndian(new byte[] { 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, isUnsigned: true, out result));
			Assert.Equal(new(new(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)), result);

			Assert.True(BinaryIntegerHelper<Int256>.TryReadBigEndian(new byte[] { 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: true, out result));
			Assert.Equal(new(new(0x8000_0000_0000_0000, 0x0000_0000_0000_0000)), result);

			Assert.True(BinaryIntegerHelper<Int256>.TryReadBigEndian(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F }, isUnsigned: true, out result));
			Assert.Equal(new(new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FF7F)), result);

			Assert.True(BinaryIntegerHelper<Int256>.TryReadBigEndian(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, isUnsigned: true, out result));
			Assert.Equal(new(new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)), result);
		}

		[Fact]
		public static void TryReadLittleEndianInt128Test()
		{
			Int256 result;

			Assert.True(BinaryIntegerHelper<Int256>.TryReadLittleEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: false, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000)), result);

			Assert.True(BinaryIntegerHelper<Int256>.TryReadLittleEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 }, isUnsigned: false, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0100_0000_0000_0000, 0x0000_0000_0000_0000)), result);

			Assert.True(BinaryIntegerHelper<Int256>.TryReadLittleEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80 }, isUnsigned: false, out result));
			Assert.Equal(new(new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new(0x8000_0000_0000_0000, 0x0000_0000_0000_0000)), result);

			Assert.True(BinaryIntegerHelper<Int256>.TryReadLittleEndian(new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: false, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0001)), result);

			Assert.True(BinaryIntegerHelper<Int256>.TryReadLittleEndian(new byte[] { 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, isUnsigned: false, out result));
			Assert.Equal(new(new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FF7F)), result);

			Assert.True(BinaryIntegerHelper<Int256>.TryReadLittleEndian(new byte[] { 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: false, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0080)), result);

			Assert.True(BinaryIntegerHelper<Int256>.TryReadLittleEndian(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F }, isUnsigned: false, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)), result);

			Assert.True(BinaryIntegerHelper<Int256>.TryReadLittleEndian(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, isUnsigned: false, out result));
			Assert.Equal(new(new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)), result);
		}

		[Fact]
		public static void TryReadLittleEndianInt192Test()
		{
			Int256 result;

			Assert.True(BinaryIntegerHelper<Int256>.TryReadLittleEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: false, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000)), result);

			Assert.True(BinaryIntegerHelper<Int256>.TryReadLittleEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 }, isUnsigned: false, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0100_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000)), result);

			Assert.True(BinaryIntegerHelper<Int256>.TryReadLittleEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80 }, isUnsigned: false, out result));
			Assert.Equal(new(new(0xFFFF_FFFF_FFFF_FFFF, 0x8000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000)), result);

			Assert.True(BinaryIntegerHelper<Int256>.TryReadLittleEndian(new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: false, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0001)), result);

			Assert.True(BinaryIntegerHelper<Int256>.TryReadLittleEndian(new byte[] { 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, isUnsigned: false, out result));
			Assert.Equal(new(new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FF7F)), result);

			Assert.True(BinaryIntegerHelper<Int256>.TryReadLittleEndian(new byte[] { 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: false, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0080)), result);

			Assert.True(BinaryIntegerHelper<Int256>.TryReadLittleEndian(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F }, isUnsigned: false, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x7FFF_FFFF_FFFF_FFFF), new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)), result);

			Assert.True(BinaryIntegerHelper<Int256>.TryReadLittleEndian(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, isUnsigned: false, out result));
			Assert.Equal(new(new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)), result);
		}
		
		[Fact]
		public static void TryReadLittleEndianInt256Test()
		{
			Int256 result;

			Assert.True(BinaryIntegerHelper<Int256>.TryReadLittleEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: false, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000)), result);

			Assert.True(BinaryIntegerHelper<Int256>.TryReadLittleEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 }, isUnsigned: false, out result));
			Assert.Equal(new(new(0x0100_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000)), result);

			Assert.True(BinaryIntegerHelper<Int256>.TryReadLittleEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80 }, isUnsigned: false, out result));
			Assert.Equal(new(new(0x8000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000)), result);

			Assert.True(BinaryIntegerHelper<Int256>.TryReadLittleEndian(new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: false, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0001)), result);

			Assert.True(BinaryIntegerHelper<Int256>.TryReadLittleEndian(new byte[] { 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, isUnsigned: false, out result));
			Assert.Equal(new(new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FF7F)), result);

			Assert.True(BinaryIntegerHelper<Int256>.TryReadLittleEndian(new byte[] { 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: false, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0080)), result);

			Assert.True(BinaryIntegerHelper<Int256>.TryReadLittleEndian(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F }, isUnsigned: false, out result));
			Assert.Equal(new(new(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)), result);

			Assert.True(BinaryIntegerHelper<Int256>.TryReadLittleEndian(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, isUnsigned: false, out result));
			Assert.Equal(new(new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)), result);
		}

		[Fact]
		public static void TryReadLittleEndianUInt128Test()
		{
			Int256 result;

			Assert.True(BinaryIntegerHelper<Int256>.TryReadLittleEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: true, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000)), result);

			Assert.True(BinaryIntegerHelper<Int256>.TryReadLittleEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 }, isUnsigned: true, out result));
			Assert.Equal(new(new(0x0100_0000_0000_0000, 0x0000_0000_0000_0000)), result);

			Assert.True(BinaryIntegerHelper<Int256>.TryReadLittleEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80 }, isUnsigned: true, out result));
			Assert.Equal(new(new(0x8000_0000_0000_0000, 0x0000_0000_0000_0000)), result);

			Assert.True(BinaryIntegerHelper<Int256>.TryReadLittleEndian(new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: true, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0001)), result);

			Assert.True(BinaryIntegerHelper<Int256>.TryReadLittleEndian(new byte[] { 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, isUnsigned: true, out result));
			Assert.Equal(new(new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FF7F)), result);

			Assert.True(BinaryIntegerHelper<Int256>.TryReadLittleEndian(new byte[] { 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: true, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0080)), result);

			Assert.True(BinaryIntegerHelper<Int256>.TryReadLittleEndian(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F }, isUnsigned: true, out result));
			Assert.Equal(new(new(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)), result);

			Assert.True(BinaryIntegerHelper<Int256>.TryReadLittleEndian(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, isUnsigned: true, out result));
			Assert.Equal(new(new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)), result);
		}

		[Fact]
		public static void TryReadLittleEndianUInt192Test()
		{
			Int256 result;

			Assert.True(BinaryIntegerHelper<Int256>.TryReadLittleEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: true, out result));
			Assert.Equal(new(new UInt128(0x0000_0000_0000_0000, 0x0000_0000_0000_0000)), result);

			Assert.True(BinaryIntegerHelper<Int256>.TryReadLittleEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 }, isUnsigned: true, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0100_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000)), result);

			Assert.True(BinaryIntegerHelper<Int256>.TryReadLittleEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80 }, isUnsigned: true, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x8000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000)), result);

			Assert.True(BinaryIntegerHelper<Int256>.TryReadLittleEndian(new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: true, out result));
			Assert.Equal(new(new UInt128(0x0000_0000_0000_0000, 0x0000_0000_0000_0001)), result);

			Assert.True(BinaryIntegerHelper<Int256>.TryReadLittleEndian(new byte[] { 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, isUnsigned: true, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0xFFFF_FFFF_FFFF_FFFF), new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FF7F)), result);

			Assert.True(BinaryIntegerHelper<Int256>.TryReadLittleEndian(new byte[] { 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: true, out result));
			Assert.Equal(new(new UInt128(0x0000_0000_0000_0000, 0x0000_0000_0000_0080)), result);

			Assert.True(BinaryIntegerHelper<Int256>.TryReadLittleEndian(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F }, isUnsigned: true, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x7FFF_FFFF_FFFF_FFFF), new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)), result);

			Assert.True(BinaryIntegerHelper<Int256>.TryReadLittleEndian(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, isUnsigned: true, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0xFFFF_FFFF_FFFF_FFFF), new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)), result);
		}

		[Fact]
		public static void TryReadLittleEndianUInt256Test()
		{
			Int256 result;

			Assert.True(BinaryIntegerHelper<Int256>.TryReadLittleEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: true, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000)), result);

			Assert.True(BinaryIntegerHelper<Int256>.TryReadLittleEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 }, isUnsigned: true, out result));
			Assert.Equal(new(new(0x0100_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000)), result);

			Assert.False(BinaryIntegerHelper<Int256>.TryReadLittleEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80 }, isUnsigned: true, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000)), result);

			Assert.True(BinaryIntegerHelper<Int256>.TryReadLittleEndian(new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: true, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0001)), result);

			Assert.False(BinaryIntegerHelper<Int256>.TryReadLittleEndian(new byte[] { 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, isUnsigned: true, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000)), result);

			Assert.True(BinaryIntegerHelper<Int256>.TryReadLittleEndian(new byte[] { 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: true, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0080)), result);

			Assert.True(BinaryIntegerHelper<Int256>.TryReadLittleEndian(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F }, isUnsigned: true, out result));
			Assert.Equal(new(new(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)), result);

			Assert.False(BinaryIntegerHelper<Int256>.TryReadLittleEndian(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, isUnsigned: true, out result));
			Assert.Equal(new(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000)), result);
		}

		[Fact]
		public static void GetByteCountTest()
		{
			Assert.Equal(32, BinaryIntegerHelper<Int256>.GetByteCount(Zero));
			Assert.Equal(32, BinaryIntegerHelper<Int256>.GetByteCount(One));
			Assert.Equal(32, BinaryIntegerHelper<Int256>.GetByteCount(MaxValue));
			Assert.Equal(32, BinaryIntegerHelper<Int256>.GetByteCount(MinValue));
			Assert.Equal(32, BinaryIntegerHelper<Int256>.GetByteCount(NegativeOne));
		}

		[Fact]
		public static void GetShortestBitLengthTest()
		{
			Assert.Equal(0x00, BinaryIntegerHelper<Int256>.GetShortestBitLength(Zero));
			Assert.Equal(0x01, BinaryIntegerHelper<Int256>.GetShortestBitLength(One));
			Assert.Equal(0xFF, BinaryIntegerHelper<Int256>.GetShortestBitLength(MaxValue));
			Assert.Equal(0x100, BinaryIntegerHelper<Int256>.GetShortestBitLength(MinValue));
			Assert.Equal(0x01, BinaryIntegerHelper<Int256>.GetShortestBitLength(NegativeOne));
		}

		[Fact]
		public static void TryWriteBigEndianTest()
		{
			Span<byte> destination = stackalloc byte[32];
			int bytesWritten = 0;

			Assert.True(BinaryIntegerHelper<Int256>.TryWriteBigEndian(Zero, destination, out bytesWritten));
			Assert.Equal(32, bytesWritten);
			Assert.Equal(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, destination.ToArray());

			Assert.True(BinaryIntegerHelper<Int256>.TryWriteBigEndian(One, destination, out bytesWritten));
			Assert.Equal(32, bytesWritten);
			Assert.Equal(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 }, destination.ToArray());

			Assert.True(BinaryIntegerHelper<Int256>.TryWriteBigEndian(MaxValue, destination, out bytesWritten));
			Assert.Equal(32, bytesWritten);
			Assert.Equal(new byte[] { 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, destination.ToArray());

			Assert.True(BinaryIntegerHelper<Int256>.TryWriteBigEndian(MinValue, destination, out bytesWritten));
			Assert.Equal(32, bytesWritten);
			Assert.Equal(new byte[] { 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, destination.ToArray());

			Assert.True(BinaryIntegerHelper<Int256>.TryWriteBigEndian(NegativeOne, destination, out bytesWritten));
			Assert.Equal(32, bytesWritten);
			Assert.Equal(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, destination.ToArray());

			Assert.False(BinaryIntegerHelper<Int256>.TryWriteBigEndian(default, Span<byte>.Empty, out bytesWritten));
			Assert.Equal(0, bytesWritten);
			Assert.Equal(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, destination.ToArray());
		}

		[Fact]
		public static void TryWriteLittleEndianTest()
		{
			Span<byte> destination = stackalloc byte[32];
			int bytesWritten = 0;

			Assert.True(BinaryIntegerHelper<Int256>.TryWriteLittleEndian(Zero, destination, out bytesWritten));
			Assert.Equal(32, bytesWritten);
			Assert.Equal(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, destination.ToArray());

			Assert.True(BinaryIntegerHelper<Int256>.TryWriteLittleEndian(One, destination, out bytesWritten));
			Assert.Equal(32, bytesWritten);
			Assert.Equal(new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, destination.ToArray());

			Assert.True(BinaryIntegerHelper<Int256>.TryWriteLittleEndian(MaxValue, destination, out bytesWritten));
			Assert.Equal(32, bytesWritten);
			Assert.Equal(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F }, destination.ToArray());

			Assert.True(BinaryIntegerHelper<Int256>.TryWriteLittleEndian(MinValue, destination, out bytesWritten));
			Assert.Equal(32, bytesWritten);
			Assert.Equal(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80 }, destination.ToArray());

			Assert.True(BinaryIntegerHelper<Int256>.TryWriteLittleEndian(NegativeOne, destination, out bytesWritten));
			Assert.Equal(32, bytesWritten);
			Assert.Equal(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, destination.ToArray());

			Assert.False(BinaryIntegerHelper<Int256>.TryWriteLittleEndian(default, Span<byte>.Empty, out bytesWritten));
			Assert.Equal(0, bytesWritten);
			Assert.Equal(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, destination.ToArray());
		}
		#endregion

		#region IBinaryNumber
		[Fact]
		public static void AllBitsSetTest()
		{
			Assert.Equal(BinaryNumberHelper<Int256>.AllBitsSet, ~Zero);
		}
		[Fact]
		public static void IsPow2Test()
		{
			Assert.True(BinaryNumberHelper<Int256>.IsPow2(new(0x100)));
			Assert.True(BinaryNumberHelper<Int256>.IsPow2(new(0x1_0000)));
			Assert.True(BinaryNumberHelper<Int256>.IsPow2(new(0x1_0000_0000)));
			Assert.True(BinaryNumberHelper<Int256>.IsPow2(new(new(0x1, 0x0000_0000_0000_0000))));
			Assert.True(BinaryNumberHelper<Int256>.IsPow2(new(0x1, new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000))));
		}
		[Fact]
		public static void Log2Test()
		{
			Assert.Equal(8, BinaryNumberHelper<Int256>.Log2(new(0x100)));
			Assert.Equal(16, BinaryNumberHelper<Int256>.Log2(new(0x1_0000)));
			Assert.Equal(32, BinaryNumberHelper<Int256>.Log2(new(0x1_0000_0000)));
			Assert.Equal(64, BinaryNumberHelper<Int256>.Log2(new(new(0x1, 0x0000_0000_0000_0000))));
			Assert.Equal(128, BinaryNumberHelper<Int256>.Log2(new(0x1, new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000))));
		}
		#endregion

		#region IMinMaxValue
		[Fact]
		public static void MaxValueTest()
		{
			MaxValue.Should().Be(MathConstantsHelper.MaxValue<Int256>());
		}

		[Fact]
		public static void MinValueTest()
		{
			MinValue.Should().Be(MathConstantsHelper.MinValue<Int256>());
		}
		#endregion

		#region INumber
		[Fact]
		public static void ClampTest()
		{
			NumberHelper<Int256>.Clamp(MaxValueMinusOne, Int128MaxValue, MaxValue).Should().Be(MaxValueMinusOne);
			NumberHelper<Int256>.Clamp(MinValue, 0, MaxValue).Should().Be(0);
			NumberHelper<Int256>.Clamp(MaxValue, MinValue, 0).Should().Be(0);

			Assert.Throws<MathematicalException>(() => NumberHelper<Int256>.Clamp(MinValue, MaxValue, 0));
		}
		[Fact]
		public static void CopySignTest()
		{
			NumberHelper<Int256>.CopySign(MaxValue, NegativeOne).Should().Be(MinValuePlusOne);
			NumberHelper<Int256>.CopySign(MaxValue, One).Should().Be(MaxValue);
			NumberHelper<Int256>.CopySign(NegativeTwo, One).Should().Be(Two);
		}
		[Fact]
		public static void MaxTest()
		{
			NumberHelper<Int256>.Max(MaxValue, MinValue).Should().Be(MaxValue);
			NumberHelper<Int256>.Max(One, NegativeTwo).Should().Be(One);
			NumberHelper<Int256>.Max(Two, NegativeOne).Should().Be(Two);
			NumberHelper<Int256>.Max(NegativeOne, NegativeTwo).Should().Be(NegativeOne);
		}
		[Fact]
		public static void MaxNumberTest()
		{
			NumberHelper<Int256>.MaxNumber(MaxValue, MinValue).Should().Be(MaxValue);
			NumberHelper<Int256>.MaxNumber(One, NegativeTwo).Should().Be(One);
			NumberHelper<Int256>.MaxNumber(Two, NegativeOne).Should().Be(Two);
			NumberHelper<Int256>.MaxNumber(NegativeOne, NegativeTwo).Should().Be(NegativeOne);
		}
		[Fact]
		public static void MinTest()
		{
			NumberHelper<Int256>.Min(MaxValue, MinValue).Should().Be(MinValue);
			NumberHelper<Int256>.Min(One, NegativeTwo).Should().Be(NegativeTwo);
			NumberHelper<Int256>.Min(Two, NegativeOne).Should().Be(NegativeOne);
			NumberHelper<Int256>.Min(NegativeOne, NegativeTwo).Should().Be(NegativeTwo);
		}
		[Fact]
		public static void MinNumberTest()
		{
			NumberHelper<Int256>.MinNumber(MaxValue, MinValue).Should().Be(MinValue);
			NumberHelper<Int256>.MinNumber(One, NegativeTwo).Should().Be(NegativeTwo);
			NumberHelper<Int256>.MinNumber(Two, NegativeOne).Should().Be(NegativeOne);
			NumberHelper<Int256>.MinNumber(NegativeOne, NegativeTwo).Should().Be(NegativeTwo);
		}
		[Fact]
		public static void SignTest()
		{
			NumberHelper<Int256>.Sign(MinValue).Should().Be(-1);
			NumberHelper<Int256>.Sign(MaxValue).Should().Be(1);
			NumberHelper<Int256>.Sign(Int256.Zero).Should().Be(0);
		}
		#endregion

		#region INumberBase
		[Fact]
		public static void AbsTest()
		{
			NumberBaseHelper<Int256>.Abs(MinValuePlusOne).Should().Be(MaxValue);
			NumberBaseHelper<Int256>.Abs(NegativeTwo).Should().Be(Two);
			NumberBaseHelper<Int256>.Abs(NegativeOne).Should().Be(One);
			NumberBaseHelper<Int256>.Abs(One).Should().Be(One);

			Assert.Throws<ArgumentException>(() => NumberBaseHelper<Int256>.Abs(MinValue));
		}
		[Fact]
		public static void CreateCheckedToInt256Test()
		{
			NumberBaseHelper<Int256>.CreateChecked(byte.MaxValue).Should().Be(ByteMaxValue);
			NumberBaseHelper<Int256>.CreateChecked(short.MaxValue).Should().Be(Int16MaxValue);
			NumberBaseHelper<Int256>.CreateChecked(int.MaxValue).Should().Be(Int32MaxValue);
			NumberBaseHelper<Int256>.CreateChecked(long.MaxValue).Should().Be(Int64MaxValue);
			NumberBaseHelper<Int256>.CreateChecked(Int128.MaxValue).Should().Be(Int128MaxValue);
			NumberBaseHelper<Int256>.CreateChecked(Int256MaxValueAsDouble).Should().Be(MaxValue);

			NumberBaseHelper<Int256>.CreateChecked(byte.MinValue).Should().Be(Zero);
			NumberBaseHelper<Int256>.CreateChecked(short.MinValue).Should().Be(Int16MinValue);
			NumberBaseHelper<Int256>.CreateChecked(int.MinValue).Should().Be(Int32MinValue);
			NumberBaseHelper<Int256>.CreateChecked(long.MinValue).Should().Be(Int64MinValue);
			NumberBaseHelper<Int256>.CreateChecked(Int128.MinValue).Should().Be(Int128MinValue);
			NumberBaseHelper<Int256>.CreateChecked(Int256MinValueAsDouble).Should().Be(MinValue);
		}
		[Fact]
		public static void CreateSaturatingToInt256Test()
		{
			NumberBaseHelper<Int256>.CreateSaturating(byte.MaxValue).Should().Be(ByteMaxValue);
			NumberBaseHelper<Int256>.CreateSaturating(short.MaxValue).Should().Be(Int16MaxValue);
			NumberBaseHelper<Int256>.CreateSaturating(int.MaxValue).Should().Be(Int32MaxValue);
			NumberBaseHelper<Int256>.CreateSaturating(long.MaxValue).Should().Be(Int64MaxValue);
			NumberBaseHelper<Int256>.CreateSaturating(Int128.MaxValue).Should().Be(Int128MaxValue);
			NumberBaseHelper<Int256>.CreateSaturating(Int256MaxValueAsDouble).Should().Be(MaxValue);

			NumberBaseHelper<Int256>.CreateSaturating(byte.MinValue).Should().Be(Zero);
			NumberBaseHelper<Int256>.CreateSaturating(short.MinValue).Should().Be(Int16MinValue);
			NumberBaseHelper<Int256>.CreateSaturating(int.MinValue).Should().Be(Int32MinValue);
			NumberBaseHelper<Int256>.CreateSaturating(long.MinValue).Should().Be(Int64MinValue);
			NumberBaseHelper<Int256>.CreateSaturating(Int128.MinValue).Should().Be(Int128MinValue);
			NumberBaseHelper<Int256>.CreateSaturating(Int256MinValueAsDouble).Should().Be(MinValue);
		}
		[Fact]
		public static void CreateTruncatingToInt256Test()
		{
			NumberBaseHelper<Int256>.CreateTruncating(byte.MaxValue).Should().Be(ByteMaxValue);
			NumberBaseHelper<Int256>.CreateTruncating(short.MaxValue).Should().Be(Int16MaxValue);
			NumberBaseHelper<Int256>.CreateTruncating(int.MaxValue).Should().Be(Int32MaxValue);
			NumberBaseHelper<Int256>.CreateTruncating(long.MaxValue).Should().Be(Int64MaxValue);
			NumberBaseHelper<Int256>.CreateTruncating(Int128.MaxValue).Should().Be(Int128MaxValue);
			NumberBaseHelper<Int256>.CreateTruncating(Int256MaxValueAsDouble).Should().Be(MaxValue);

			NumberBaseHelper<Int256>.CreateTruncating(byte.MinValue).Should().Be(Zero);
			NumberBaseHelper<Int256>.CreateTruncating(short.MinValue).Should().Be(Int16MinValue);
			NumberBaseHelper<Int256>.CreateTruncating(int.MinValue).Should().Be(Int32MinValue);
			NumberBaseHelper<Int256>.CreateTruncating(long.MinValue).Should().Be(Int64MinValue);
			NumberBaseHelper<Int256>.CreateTruncating(Int128.MinValue).Should().Be(Int128MinValue);
			NumberBaseHelper<Int256>.CreateTruncating(Int256MinValueAsDouble).Should().Be(MinValue);
		}

		[Fact]
		public static void CreateCheckedFromInt256Test()
		{
			NumberBaseHelper<byte>.CreateChecked(ByteMaxValue).Should().Be(byte.MaxValue);
			NumberBaseHelper<short>.CreateChecked(Int16MaxValue).Should().Be(short.MaxValue);
			NumberBaseHelper<int>.CreateChecked(Int32MaxValue).Should().Be(int.MaxValue);
			NumberBaseHelper<long>.CreateChecked(Int64MaxValue).Should().Be(long.MaxValue);
			NumberBaseHelper<Int128>.CreateChecked(Int128MaxValue).Should().Be(Int128.MaxValue);
			NumberBaseHelper<double>.CreateChecked(MaxValue).Should().Be(Int256MaxValueAsDouble);

			NumberBaseHelper<byte>.CreateChecked(Zero).Should().Be(byte.MinValue);
			NumberBaseHelper<short>.CreateChecked(Int16MinValue).Should().Be(short.MinValue);
			NumberBaseHelper<int>.CreateChecked(Int32MinValue).Should().Be(int.MinValue);
			NumberBaseHelper<long>.CreateChecked(Int64MinValue).Should().Be(long.MinValue);
			NumberBaseHelper<Int128>.CreateChecked(Int128MinValue).Should().Be(Int128.MinValue);
			NumberBaseHelper<double>.CreateChecked(MinValue).Should().Be(Int256MinValueAsDouble);
		}
		[Fact]
		public static void CreateSaturatingFromInt256Test()
		{
			NumberBaseHelper<byte>.CreateSaturating(ByteMaxValue).Should().Be(byte.MaxValue);
			NumberBaseHelper<short>.CreateSaturating(Int16MaxValue).Should().Be(short.MaxValue);
			NumberBaseHelper<int>.CreateSaturating(Int32MaxValue).Should().Be(int.MaxValue);
			NumberBaseHelper<long>.CreateSaturating(Int64MaxValue).Should().Be(long.MaxValue);
			NumberBaseHelper<Int128>.CreateSaturating(Int128MaxValue).Should().Be(Int128.MaxValue);
			NumberBaseHelper<double>.CreateSaturating(MaxValue).Should().Be(Int256MaxValueAsDouble);

			NumberBaseHelper<byte>.CreateSaturating(Zero).Should().Be(byte.MinValue);
			NumberBaseHelper<short>.CreateSaturating(Int16MinValue).Should().Be(short.MinValue);
			NumberBaseHelper<int>.CreateSaturating(Int32MinValue).Should().Be(int.MinValue);
			NumberBaseHelper<long>.CreateSaturating(Int64MinValue).Should().Be(long.MinValue);
			NumberBaseHelper<Int128>.CreateSaturating(Int128MinValue).Should().Be(Int128.MinValue);
			NumberBaseHelper<double>.CreateSaturating(MinValue).Should().Be(Int256MinValueAsDouble);
		}
		[Fact]
		public static void CreateTruncatingFromInt256Test()
		{
			NumberBaseHelper<byte>.CreateTruncating(ByteMaxValue).Should().Be(byte.MaxValue);
			NumberBaseHelper<short>.CreateTruncating(Int16MaxValue).Should().Be(short.MaxValue);
			NumberBaseHelper<int>.CreateTruncating(Int32MaxValue).Should().Be(int.MaxValue);
			NumberBaseHelper<long>.CreateTruncating(Int64MaxValue).Should().Be(long.MaxValue);
			NumberBaseHelper<Int128>.CreateTruncating(Int128MaxValue).Should().Be(Int128.MaxValue);
			NumberBaseHelper<double>.CreateTruncating(MaxValue).Should().Be(Int256MaxValueAsDouble);

			NumberBaseHelper<byte>.CreateTruncating(Zero).Should().Be(byte.MinValue);
			NumberBaseHelper<short>.CreateTruncating(Int16MinValue).Should().Be(short.MinValue);
			NumberBaseHelper<int>.CreateTruncating(Int32MinValue).Should().Be(int.MinValue);
			NumberBaseHelper<long>.CreateTruncating(Int64MinValue).Should().Be(long.MinValue);
			NumberBaseHelper<Int128>.CreateTruncating(Int128MinValue).Should().Be(Int128.MinValue);
			NumberBaseHelper<double>.CreateTruncating(MinValue).Should().Be(Int256MinValueAsDouble);
		}

		[Fact]
		public static void IsCanonicalTest()
		{
			NumberBaseHelper<Int256>.IsCanonical(default(Int256)).Should().BeTrue();
		}

		[Fact]
		public static void IsComplexNumberTest()
		{
			NumberBaseHelper<Int256>.IsComplexNumber(default(Int256)).Should().BeFalse();
		}

		[Fact]
		public static void IsEvenIntegerTest()
		{
			NumberBaseHelper<Int256>.IsEvenInteger(One).Should().BeFalse();
			NumberBaseHelper<Int256>.IsEvenInteger(Two).Should().BeTrue();
		}

		[Fact]
		public static void IsFiniteTest()
		{
			NumberBaseHelper<Int256>.IsFinite(default(Int256)).Should().BeTrue();
		}

		[Fact]
		public static void IsImaginaryNumberTest()
		{
			NumberBaseHelper<Int256>.IsImaginaryNumber(default(Int256)).Should().BeFalse();
		}

		[Fact]
		public static void IsInfinityTest()
		{
			NumberBaseHelper<Int256>.IsInfinity(default(Int256)).Should().BeFalse();
		}

		[Fact]
		public static void IsIntegerTest()
		{
			NumberBaseHelper<Int256>.IsInteger(default(Int256)).Should().BeTrue();
		}

		[Fact]
		public static void IsNaNTest()
		{
			NumberBaseHelper<Int256>.IsNaN(default(Int256)).Should().BeFalse();
		}

		[Fact]
		public static void IsNegativeTest()
		{
			NumberBaseHelper<Int256>.IsNegative(One).Should().BeFalse();
			NumberBaseHelper<Int256>.IsNegative(NegativeOne).Should().BeTrue();
		}

		[Fact]
		public static void IsNegativeInfinityTest()
		{
			NumberBaseHelper<Int256>.IsNegativeInfinity(One).Should().BeFalse();
		}

		[Fact]
		public static void IsNormalTest()
		{
			NumberBaseHelper<Int256>.IsNormal(Zero).Should().BeFalse();
			NumberBaseHelper<Int256>.IsNormal(One).Should().BeTrue();
		}

		[Fact]
		public static void IsOddIntegerTest()
		{
			NumberBaseHelper<Int256>.IsOddInteger(One).Should().BeTrue();
			NumberBaseHelper<Int256>.IsOddInteger(Two).Should().BeFalse();
		}

		[Fact]
		public static void IsPositiveTest()
		{
			NumberBaseHelper<Int256>.IsPositive(One).Should().BeTrue();
			NumberBaseHelper<Int256>.IsPositive(NegativeOne).Should().BeFalse();
		}

		[Fact]
		public static void IsPositiveInfinityTest()
		{
			NumberBaseHelper<Int256>.IsPositiveInfinity(One).Should().BeFalse();
		}

		[Fact]
		public static void IsRealNumberTest()
		{
			NumberBaseHelper<Int256>.IsRealNumber(default).Should().BeTrue();
		}

		[Fact]
		public static void IsSubnormalTest()
		{
			NumberBaseHelper<Int256>.IsSubnormal(default).Should().BeFalse();
		}

		[Fact]
		public static void IsZeroTest()
		{
			NumberBaseHelper<Int256>.IsZero(default).Should().BeTrue();
			NumberBaseHelper<Int256>.IsZero(Zero).Should().BeTrue();
			NumberBaseHelper<Int256>.IsZero(One).Should().BeFalse();
			NumberBaseHelper<Int256>.IsZero(NegativeOne).Should().BeFalse();
		}

		[Fact]
		public static void MaxMagnitudeTest()
		{
			NumberBaseHelper<Int256>.MaxMagnitude(MaxValue, MinValue).Should().Be(MinValue);
			NumberBaseHelper<Int256>.MaxMagnitude(One, NegativeTwo).Should().Be(NegativeTwo);
			NumberBaseHelper<Int256>.MaxMagnitude(Two, NegativeOne).Should().Be(Two);
			NumberBaseHelper<Int256>.MaxMagnitude(NegativeOne, NegativeTwo).Should().Be(NegativeTwo);
		}

		[Fact]
		public static void MaxMagnitudeNumberTest()
		{
			NumberBaseHelper<Int256>.MaxMagnitudeNumber(MaxValue, MinValue).Should().Be(MinValue);
			NumberBaseHelper<Int256>.MaxMagnitudeNumber(One, NegativeTwo).Should().Be(NegativeTwo);
			NumberBaseHelper<Int256>.MaxMagnitudeNumber(Two, NegativeOne).Should().Be(Two);
			NumberBaseHelper<Int256>.MaxMagnitudeNumber(NegativeOne, NegativeTwo).Should().Be(NegativeTwo);
		}

		[Fact]
		public static void MinMagnitudeTest()
		{
			NumberBaseHelper<Int256>.MinMagnitude(MaxValue, MinValue).Should().Be(MaxValue);
			NumberBaseHelper<Int256>.MinMagnitude(One, NegativeTwo).Should().Be(One);
			NumberBaseHelper<Int256>.MinMagnitude(Two, NegativeOne).Should().Be(NegativeOne);
			NumberBaseHelper<Int256>.MinMagnitude(NegativeOne, NegativeTwo).Should().Be(NegativeOne);
		}

		[Fact]
		public static void MinMagnitudeNumberTest()
		{
			NumberBaseHelper<Int256>.MinMagnitudeNumber(MaxValue, MinValue).Should().Be(MaxValue);
			NumberBaseHelper<Int256>.MinMagnitudeNumber(One, NegativeTwo).Should().Be(One);
			NumberBaseHelper<Int256>.MinMagnitudeNumber(Two, NegativeOne).Should().Be(NegativeOne);
			NumberBaseHelper<Int256>.MinMagnitudeNumber(NegativeOne, NegativeTwo).Should().Be(NegativeOne);
		}

		[Fact]
		public void ParseTest()
		{
			NumberBaseHelper<Int256>.Parse("57896044618658097711785492504343953926634992332820282019728792003956564819967", System.Globalization.NumberStyles.Integer, CultureInfo.CurrentCulture)
				.Should().Be(Int256MaxValue)
				.And.BeRankedEquallyTo(Int256MaxValue);
			NumberBaseHelper<Int256>.Parse("7FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", System.Globalization.NumberStyles.HexNumber, CultureInfo.CurrentCulture)
				.Should().Be(Int256MaxValue)
				.And.BeRankedEquallyTo(Int256MaxValue);
			NumberBaseHelper<Int256>.Parse("-57896044618658097711785492504343953926634992332820282019728792003956564819968", System.Globalization.NumberStyles.Integer, CultureInfo.CurrentCulture)
				.Should().Be(Int256MinValue)
				.And.BeRankedEquallyTo(Int256MinValue);
			NumberBaseHelper<Int256>.Parse("8000000000000000000000000000000000000000000000000000000000000000", System.Globalization.NumberStyles.HexNumber, CultureInfo.CurrentCulture)
				.Should().Be(Int256MinValue)
				.And.BeRankedEquallyTo(Int256MinValue);

			Assert.Throws<FormatException>(() => NumberBaseHelper<Int256>.Parse("115792089237316195423570985008687907853269984665640564039457584007913129639936", System.Globalization.NumberStyles.Integer, CultureInfo.CurrentCulture));
		}

		[Fact]
		public void TryParseTest()
		{
			NumberBaseHelper<Int256>.TryParse("57896044618658097711785492504343953926634992332820282019728792003956564819967", System.Globalization.NumberStyles.Integer, CultureInfo.CurrentCulture, out Int256 parsedValue)
				.Should().BeTrue();
			parsedValue.Should().Be(Int256MaxValue)
				.And.BeRankedEquallyTo(Int256MaxValue);
			NumberBaseHelper<Int256>.TryParse("7FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", System.Globalization.NumberStyles.HexNumber, CultureInfo.CurrentCulture, out parsedValue)
				.Should().BeTrue();
			parsedValue.Should().Be(Int256MaxValue)
				.And.BeRankedEquallyTo(Int256MaxValue);
			NumberBaseHelper<Int256>.TryParse("-57896044618658097711785492504343953926634992332820282019728792003956564819968", System.Globalization.NumberStyles.Integer, CultureInfo.CurrentCulture, out parsedValue)
				.Should().BeTrue();
			parsedValue.Should().Be(Int256MinValue)
				.And.BeRankedEquallyTo(Int256MinValue);
			NumberBaseHelper<Int256>.TryParse("8000000000000000000000000000000000000000000000000000000000000000", System.Globalization.NumberStyles.HexNumber, CultureInfo.CurrentCulture, out parsedValue)
				.Should().BeTrue();
			parsedValue.Should().Be(Int256MinValue)
				.And.BeRankedEquallyTo(Int256MinValue);

			NumberBaseHelper<Int256>.TryParse("115792089237316195423570985008687907853269984665640564039457584007913129639936", System.Globalization.NumberStyles.Integer, CultureInfo.CurrentCulture, out parsedValue)
				.Should().BeFalse();
			parsedValue.Should().Be(default);
		}
		#endregion

		#region ISignedNumber
		[Fact]
		public static void NegativeOneTest()
		{
			MathConstantsHelper.NegativeOne<Int256>().Should().Be(NegativeOne);	
		}
		#endregion
	}
}
