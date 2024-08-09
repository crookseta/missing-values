using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Int = MissingValues.Int512;

namespace MissingValues.Tests.Core
{
	public partial class Int512Test
	{
		#region Readonly Variables
		private const double Int512MaxValueAsDouble = 6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042047.0;
		private const double Int512MinValueAsDouble = -6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042048.0d;

		private static readonly Int ByteMaxValue = new(
			0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000,
			0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_00FF);

		private static readonly Int Int16MaxValue = new(
			0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000,
			0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_7FFF);
		private static readonly Int Int16MinValue = new(
			0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF,
			0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_8000);

		private static readonly Int Int32MaxValue = new(
			0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000,
			0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_7FFF_FFFF);
		private static readonly Int Int32MinValue = new(
			0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF,
			0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_8000_0000);

		private static readonly Int Int64MaxValue = new(
			0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000,
			0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x7FFF_FFFF_FFFF_FFFF);
		private static readonly Int Int64MinValue = new(
			0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF,
			0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0x8000_0000_0000_0000);

		private static readonly Int Int128MaxValue = new(
			0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000,
			0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);
		private static readonly Int Int128MinValue = new(
			0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF,
			0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0x8000_0000_0000_0000, 0x0000_0000_0000_0000);

		private static readonly Int Int256MaxValue = new(
			0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000,
			0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF
			);
		private static readonly Int Int256MinValue = new(
			0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF,
			0x8000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000
			);

		private static readonly Int NegativeOne = Int.NegativeOne;
		private static readonly Int NegativeTwo = new(
			0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF,
			0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFE);
		private static readonly Int Zero = Int.Zero;
		private static readonly Int One = Int.One;
		private static readonly Int Two = new(
			0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000,
			0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0002);
		private static readonly Int MaxValue = Int.MaxValue;
		private static readonly Int MaxValueMinusOne = new(
			0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF,
			0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFE);
		private static readonly Int MinValue = Int.MinValue;
		private static readonly Int MinValuePlusOne = new(
			0x8000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000,
			0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0001);
		#endregion

		#region Generic Math Operators
		[Fact]
		public static void op_AdditionTest()
		{
			Assert.Equal(One, MathOperatorsHelper.AdditionOperation<Int, Int, Int>(Zero, 1));
			Assert.Equal(Two, MathOperatorsHelper.AdditionOperation<Int, Int, Int>(One, 1));
			Assert.Equal(MinValue, MathOperatorsHelper.AdditionOperation<Int, Int, Int>(MaxValue, 1));
			Assert.Equal(MinValuePlusOne, MathOperatorsHelper.AdditionOperation<Int, Int, Int>(MinValue, 1));
			Assert.Equal(Zero, MathOperatorsHelper.AdditionOperation<Int, Int, Int>(NegativeOne, 1));
		}
		[Fact]
		public static void op_CheckedAdditionTest()
		{
			Assert.Equal(One, MathOperatorsHelper.CheckedAdditionOperation<Int, Int, Int>(Zero, 1));
			Assert.Equal(Two, MathOperatorsHelper.CheckedAdditionOperation<Int, Int, Int>(Int.One, 1));
			Assert.Equal(MinValuePlusOne, MathOperatorsHelper.CheckedAdditionOperation<Int, Int, Int>(MinValue, 1));
			Assert.Equal(Zero, MathOperatorsHelper.CheckedAdditionOperation<Int, Int, Int>(NegativeOne, 1));

			Assert.Throws<OverflowException>(() => MathOperatorsHelper.CheckedAdditionOperation<Int, Int, Int>(MaxValue, 1));
		}
		[Fact]
		public static void op_IncrementTest()
		{
			MathOperatorsHelper.IncrementOperation(Zero).Should().Be(One);
			MathOperatorsHelper.IncrementOperation(One).Should().Be(Two);
			MathOperatorsHelper.IncrementOperation(MinValue).Should().Be(MinValuePlusOne);
			MathOperatorsHelper.IncrementOperation(MaxValueMinusOne).Should().Be(MaxValue);
			MathOperatorsHelper.IncrementOperation(MaxValue).Should().Be(MinValue);
		}
		[Fact]
		public static void op_CheckedIncrementTest()
		{
			MathOperatorsHelper.CheckedIncrementOperation(Zero).Should().Be(One);
			MathOperatorsHelper.CheckedIncrementOperation(One).Should().Be(Two);
			MathOperatorsHelper.CheckedIncrementOperation(MinValue).Should().Be(MinValuePlusOne);
			MathOperatorsHelper.CheckedIncrementOperation(MaxValueMinusOne).Should().Be(MaxValue);

			Assert.Throws<OverflowException>(() => MathOperatorsHelper.CheckedIncrementOperation(MaxValue));
		}
		[Fact]
		public static void op_SubtractionTest()
		{
			Assert.Equal(NegativeOne, MathOperatorsHelper.SubtractionOperation<Int, Int, Int>(Zero, 1));
			Assert.Equal(One, MathOperatorsHelper.SubtractionOperation<Int, Int, Int>(Two, 1));
			Assert.Equal(MaxValue, MathOperatorsHelper.SubtractionOperation<Int, Int, Int>(MinValue, 1));
			Assert.Equal(MaxValueMinusOne, MathOperatorsHelper.SubtractionOperation<Int, Int, Int>(MaxValue, 1));
			Assert.Equal(NegativeTwo, MathOperatorsHelper.SubtractionOperation<Int, Int, Int>(NegativeOne, 1));
		}
		[Fact]
		public static void op_CheckedSubtractionTest()
		{
			Assert.Equal(NegativeOne, MathOperatorsHelper.CheckedSubtractionOperation<Int, Int, Int>(Zero, 1));
			Assert.Equal(One, MathOperatorsHelper.CheckedSubtractionOperation<Int, Int, Int>(Two, 1));
			Assert.Equal(MaxValueMinusOne, MathOperatorsHelper.CheckedSubtractionOperation<Int, Int, Int>(MaxValue, 1));
			Assert.Equal(NegativeTwo, MathOperatorsHelper.CheckedSubtractionOperation<Int, Int, Int>(NegativeOne, 1));

			Assert.Throws<OverflowException>(() => MathOperatorsHelper.CheckedSubtractionOperation<Int, Int, Int>(MinValue, 1));
		}
		[Fact]
		public static void op_DecrementTest()
		{
			MathOperatorsHelper.DecrementOperation(Two).Should().Be(One);
			MathOperatorsHelper.DecrementOperation(One).Should().Be(Zero);
			MathOperatorsHelper.DecrementOperation(MinValuePlusOne).Should().Be(MinValue);
			MathOperatorsHelper.DecrementOperation(MaxValue).Should().Be(MaxValueMinusOne);
			MathOperatorsHelper.DecrementOperation(MinValue).Should().Be(MaxValue);
		}
		[Fact]
		public static void op_CheckedDecrementTest()
		{
			MathOperatorsHelper.CheckedDecrementOperation(Two).Should().Be(One);
			MathOperatorsHelper.CheckedDecrementOperation(One).Should().Be(Zero);
			MathOperatorsHelper.CheckedDecrementOperation(MinValuePlusOne).Should().Be(MinValue);
			MathOperatorsHelper.CheckedDecrementOperation(MaxValue).Should().Be(MaxValueMinusOne);

			Assert.Throws<OverflowException>(() => MathOperatorsHelper.CheckedDecrementOperation(MinValue));
		}
		[Fact]
		public static void op_MultiplyTest()
		{
			Assert.Equal(One, MathOperatorsHelper.MultiplicationOperation<Int, Int, Int>(One, One));
			Assert.Equal(Two, MathOperatorsHelper.MultiplicationOperation<Int, Int, Int>(Two, One));
			Assert.Equal(NegativeTwo, MathOperatorsHelper.MultiplicationOperation<Int, Int, Int>(Two, NegativeOne));
			Assert.Equal(MinValuePlusOne, MathOperatorsHelper.MultiplicationOperation<Int, Int, Int>(MaxValue, NegativeOne));
			Assert.Equal(MinValue, MathOperatorsHelper.MultiplicationOperation<Int, Int, Int>(MinValue, NegativeOne));
		}
		[Fact]
		public static void op_CheckedMultiplyTest()
		{
			Assert.Equal(One, MathOperatorsHelper.CheckedMultiplicationOperation<Int, Int, Int>(One, One));
			Assert.Equal(Two, MathOperatorsHelper.CheckedMultiplicationOperation<Int, Int, Int>(Two, One));
			Assert.Equal(NegativeTwo, MathOperatorsHelper.CheckedMultiplicationOperation<Int, Int, Int>(Two, NegativeOne));
			Assert.Equal(MinValuePlusOne, MathOperatorsHelper.CheckedMultiplicationOperation<Int, Int, Int>(MaxValue, NegativeOne));

			Assert.Throws<OverflowException>(() => MathOperatorsHelper.CheckedMultiplicationOperation<Int, Int, Int>(MinValue, NegativeOne));
		}
		[Fact]
		public static void op_DivisionTest()
		{
			Assert.Equal(new Int(0x3FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), MathOperatorsHelper.DivisionOperation<Int, Int, Int>(MaxValue, Two));
			Assert.Equal(new Int(0xC000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0001), MathOperatorsHelper.DivisionOperation<Int, Int, Int>(MaxValueMinusOne, NegativeTwo));
			Assert.Equal(One, MathOperatorsHelper.DivisionOperation<Int, Int, Int>(MaxValue, MaxValue));
			Assert.Equal(NegativeOne, MathOperatorsHelper.DivisionOperation<Int, Int, Int>(MaxValue, MinValuePlusOne));

			Assert.Throws<OverflowException>(() => MathOperatorsHelper.DivisionOperation<Int, Int, Int>(MinValue, NegativeOne));
		}
		[Fact]
		public static void op_CheckedDivisionTest()
		{
			Assert.Equal(new Int(0x3FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), MathOperatorsHelper.CheckedDivisionOperation<Int, Int, Int>(MaxValue, Two));
			Assert.Equal(new Int(0xC000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0001), MathOperatorsHelper.CheckedDivisionOperation<Int, Int, Int>(MaxValueMinusOne, NegativeTwo));
			Assert.Equal(One, MathOperatorsHelper.CheckedDivisionOperation<Int, Int, Int>(MaxValue, MaxValue));
			Assert.Equal(NegativeOne, MathOperatorsHelper.CheckedDivisionOperation<Int, Int, Int>(MaxValue, MinValuePlusOne));

			Assert.Throws<OverflowException>(() => MathOperatorsHelper.CheckedDivisionOperation<Int, Int, Int>(MinValue, NegativeOne));
		}
		[Fact]
		public static void op_ModulusTest()
		{
			MathOperatorsHelper.ModulusOperation<Int, Int, Int>(Two, Two).Should().Be(Zero);
			MathOperatorsHelper.ModulusOperation<Int, Int, Int>(One, Two).Should().NotBe(Zero);
			MathOperatorsHelper.ModulusOperation<Int, Int, Int>(MaxValue, new(10U)).Should().Be(7);
			MathOperatorsHelper.ModulusOperation<Int, Int, Int>(MinValue, new(10U)).Should().Be(-8);

			Assert.Throws<DivideByZeroException>(() => MathOperatorsHelper.ModulusOperation<Int, Int, Int>(One, Zero));
		}

		[Fact]
		public static void op_BitwiseAndTest()
		{
			BitwiseOperatorsHelper<Int, Int, Int>.BitwiseAndOperation(Zero, 1U).Should().Be(Zero);
			BitwiseOperatorsHelper<Int, Int, Int>.BitwiseAndOperation(One, 1U).Should().Be(One);
			BitwiseOperatorsHelper<Int, Int, Int>.BitwiseAndOperation(MaxValue, 1U).Should().Be(One);
			BitwiseOperatorsHelper<Int, Int, Int>.BitwiseAndOperation(MinValue, 1U).Should().Be(Zero);
			BitwiseOperatorsHelper<Int, Int, Int>.BitwiseAndOperation(NegativeOne, 1U).Should().Be(One);
		}
		[Fact]
		public static void op_BitwiseOrTest()
		{
			BitwiseOperatorsHelper<Int, Int, Int>.BitwiseOrOperation(Zero, 1U)
				.Should().Be(One);
			BitwiseOperatorsHelper<Int, Int, Int>.BitwiseOrOperation(One, 1U)
				.Should().Be(One);
			BitwiseOperatorsHelper<Int, Int, Int>.BitwiseOrOperation(MaxValue, 1U)
				.Should().Be(MaxValue);
			BitwiseOperatorsHelper<Int, Int, Int>.BitwiseOrOperation(MinValue, 1U)
				.Should().Be(new(0x8000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0001));
			BitwiseOperatorsHelper<Int, Int, Int>.BitwiseOrOperation(NegativeOne, 1U)
				.Should().Be(new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF));
		}
		[Fact]
		public static void op_ExclusiveOrTest()
		{
			BitwiseOperatorsHelper<Int, Int, Int>.ExclusiveOrOperation(Zero, 1U)
				.Should().Be(One);
			BitwiseOperatorsHelper<Int, Int, Int>.ExclusiveOrOperation(One, 1U)
				.Should().Be(Zero);
			BitwiseOperatorsHelper<Int, Int, Int>.ExclusiveOrOperation(MaxValue, 1U)
				.Should().Be(new(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFE));
			BitwiseOperatorsHelper<Int, Int, Int>.ExclusiveOrOperation(MinValue, 1U)
				.Should().Be(new(0x8000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0001));
			BitwiseOperatorsHelper<Int, Int, Int>.ExclusiveOrOperation(NegativeOne, 1U)
				.Should().Be(new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFE));
		}
		[Fact]
		public static void op_OnesComplementTest()
		{
			BitwiseOperatorsHelper<Int, Int, Int>.OnesComplementOperation(Zero)
				.Should().Be(new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF));
			BitwiseOperatorsHelper<Int, Int, Int>.OnesComplementOperation(One)
				.Should().Be(new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFE));
			BitwiseOperatorsHelper<Int, Int, Int>.OnesComplementOperation(MaxValue)
				.Should().Be(new(0x8000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000));
			BitwiseOperatorsHelper<Int, Int, Int>.OnesComplementOperation(MinValue)
				.Should().Be(new(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF));
			BitwiseOperatorsHelper<Int, Int, Int>.OnesComplementOperation(NegativeOne)
				.Should().Be(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000));
		}

		[Fact]
		public static void op_LeftShiftTest()
		{
			ShiftOperatorsHelper<Int, int, Int>.LeftShiftOperation(One, 1)
				.Should().Be(Two);
			ShiftOperatorsHelper<Int, int, Int>.LeftShiftOperation(MaxValue, 1)
				.Should().Be(NegativeTwo);
			ShiftOperatorsHelper<Int, int, Int>.LeftShiftOperation(MinValue, 1)
				.Should().Be(Zero);
			ShiftOperatorsHelper<Int, int, Int>.LeftShiftOperation(NegativeOne, 1)
				.Should().Be(NegativeTwo);
		}
		[Fact]
		public static void op_RightShiftTest()
		{
			ShiftOperatorsHelper<Int, int, Int>.RightShiftOperation(Zero, 1)
				.Should().Be(Zero);
			ShiftOperatorsHelper<Int, int, Int>.RightShiftOperation(One, 1)
				.Should().Be(Zero);
			ShiftOperatorsHelper<Int, int, Int>.RightShiftOperation(MaxValue, 1)
				.Should().Be(new(0x3FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF));
			ShiftOperatorsHelper<Int, int, Int>.RightShiftOperation(MinValue, 1)
				.Should().Be(new(0xC000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000));
			ShiftOperatorsHelper<Int, int, Int>.RightShiftOperation(NegativeOne, 1)
				.Should().Be(NegativeOne);

			var actual = new Int(0x8000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000,
				0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

			ShiftOperatorsHelper<Int, int, Int>.RightShiftOperation(actual, 64 * 1)
				.Should().Be(new(
					0xFFFF_FFFF_FFFF_FFFF, 0x8000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000,
					0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000));
			ShiftOperatorsHelper<Int, int, Int>.RightShiftOperation(actual, 64 * 2)
				.Should().Be(new(
					0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0x8000_0000_0000_0000, 0x0000_0000_0000_0000,
					0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000));
			ShiftOperatorsHelper<Int, int, Int>.RightShiftOperation(actual, 64 * 3)
				.Should().Be(new(
					0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0x8000_0000_0000_0000,
					0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000));
			ShiftOperatorsHelper<Int, int, Int>.RightShiftOperation(actual, 64 * 4)
				.Should().Be(new(
					0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF,
					0x8000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000));
			ShiftOperatorsHelper<Int, int, Int>.RightShiftOperation(actual, 64 * 5)
				.Should().Be(new(
					0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF,
					0xFFFF_FFFF_FFFF_FFFF, 0x8000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000));
			ShiftOperatorsHelper<Int, int, Int>.RightShiftOperation(actual, 64 * 6)
				.Should().Be(new(
					0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF,
					0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0x8000_0000_0000_0000, 0x0000_0000_0000_0000));
			ShiftOperatorsHelper<Int, int, Int>.RightShiftOperation(actual, 64 * 7)
				.Should().Be(new(
					0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF,
					0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0x8000_0000_0000_0000));
		}
		[Fact]
		public static void op_UnsignedRightShiftTest()
		{
			ShiftOperatorsHelper<Int, int, Int>.UnsignedRightShiftOperation(Zero, 1)
				.Should().Be(Zero);
			ShiftOperatorsHelper<Int, int, Int>.UnsignedRightShiftOperation(One, 1)
				.Should().Be(Zero);
			ShiftOperatorsHelper<Int, int, Int>.UnsignedRightShiftOperation(MaxValue, 1)
				.Should().Be(new(0x3FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF));
			ShiftOperatorsHelper<Int, int, Int>.UnsignedRightShiftOperation(MinValue, 1)
				.Should().Be(new(0x4000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000));
			ShiftOperatorsHelper<Int, int, Int>.UnsignedRightShiftOperation(NegativeOne, 1)
				.Should().Be(MaxValue);
		}

		[Fact]
		public static void op_EqualityTest()
		{
			EqualityOperatorsHelper<Int, Int, bool>.EqualityOperation(Zero, 1U).Should().BeFalse();
			EqualityOperatorsHelper<Int, Int, bool>.EqualityOperation(One, 1U).Should().BeTrue();
			EqualityOperatorsHelper<Int, Int, bool>.EqualityOperation(MaxValue, 1U).Should().BeFalse();
			EqualityOperatorsHelper<Int, Int, bool>.EqualityOperation(MinValue, 1U).Should().BeFalse();
			EqualityOperatorsHelper<Int, Int, bool>.EqualityOperation(NegativeOne, 1U).Should().BeFalse();
		}
		[Fact]
		public static void op_InequalityTest()
		{
			EqualityOperatorsHelper<Int, Int, bool>.InequalityOperation(Zero, 1U).Should().BeTrue();
			EqualityOperatorsHelper<Int, Int, bool>.InequalityOperation(One, 1U).Should().BeFalse();
			EqualityOperatorsHelper<Int, Int, bool>.InequalityOperation(MaxValue, 1U).Should().BeTrue();
			EqualityOperatorsHelper<Int, Int, bool>.InequalityOperation(MinValue, 1U).Should().BeTrue();
			EqualityOperatorsHelper<Int, Int, bool>.InequalityOperation(NegativeOne, 1U).Should().BeTrue();
		}

		[Fact]
		public static void op_GreaterThanTest()
		{
			ComparisonOperatorsHelper<Int, Int, bool>.GreaterThanOperation(Zero, 1U).Should().BeFalse();
			ComparisonOperatorsHelper<Int, Int, bool>.GreaterThanOperation(One, 1U).Should().BeFalse();
			ComparisonOperatorsHelper<Int, Int, bool>.GreaterThanOperation(MaxValue, 1U).Should().BeTrue();
			ComparisonOperatorsHelper<Int, Int, bool>.GreaterThanOperation(MinValue, 1U).Should().BeFalse();
			ComparisonOperatorsHelper<Int, Int, bool>.GreaterThanOperation(NegativeOne, 1U).Should().BeFalse();
		}
		[Fact]
		public static void op_GreaterThanOrEqualTest()
		{
			ComparisonOperatorsHelper<Int, Int, bool>.GreaterThanOrEqualOperation(Zero, 1U).Should().BeFalse();
			ComparisonOperatorsHelper<Int, Int, bool>.GreaterThanOrEqualOperation(One, 1U).Should().BeTrue();
			ComparisonOperatorsHelper<Int, Int, bool>.GreaterThanOrEqualOperation(MaxValue, 1U).Should().BeTrue();
			ComparisonOperatorsHelper<Int, Int, bool>.GreaterThanOrEqualOperation(MinValue, 1U).Should().BeFalse();
			ComparisonOperatorsHelper<Int, Int, bool>.GreaterThanOrEqualOperation(NegativeOne, 1U).Should().BeFalse();
		}
		[Fact]
		public static void op_LessThanTest()
		{
			ComparisonOperatorsHelper<Int, Int, bool>.LessThanOperation(Zero, 1U).Should().BeTrue();
			ComparisonOperatorsHelper<Int, Int, bool>.LessThanOperation(One, 1U).Should().BeFalse();
			ComparisonOperatorsHelper<Int, Int, bool>.LessThanOperation(MaxValue, 1U).Should().BeFalse();
			ComparisonOperatorsHelper<Int, Int, bool>.LessThanOperation(MinValue, 1U).Should().BeTrue();
			ComparisonOperatorsHelper<Int, Int, bool>.LessThanOperation(NegativeOne, 1U).Should().BeTrue();
		}
		[Fact]
		public static void op_LessThanOrEqualTest()
		{
			ComparisonOperatorsHelper<Int, Int, bool>.LessThanOrEqualOperation(Zero, 1U).Should().BeTrue();
			ComparisonOperatorsHelper<Int, Int, bool>.LessThanOrEqualOperation(One, 1U).Should().BeTrue();
			ComparisonOperatorsHelper<Int, Int, bool>.LessThanOrEqualOperation(MaxValue, 1U).Should().BeFalse();
			ComparisonOperatorsHelper<Int, Int, bool>.LessThanOrEqualOperation(MinValue, 1U).Should().BeTrue();
			ComparisonOperatorsHelper<Int, Int, bool>.LessThanOrEqualOperation(NegativeOne, 1U).Should().BeTrue();
		}
		#endregion

		#region Identities
		[Fact]
		public static void AdditiveIdentityTest()
		{
			Assert.Equal(Zero, MathConstantsHelper.AdditiveIdentityHelper<Int, Int>());
		}

		[Fact]
		public static void MultiplicativeIdentityTest()
		{
			Assert.Equal(One, MathConstantsHelper.MultiplicativeIdentityHelper<Int, Int>());
		}
		#endregion

		#region IBinaryInteger
		[Fact]
		public static void DivRemTest()
		{
			Assert.Equal((Zero, Zero), BinaryIntegerHelper<Int>.DivRem(Zero, Two));
			Assert.Equal((Zero, One), BinaryIntegerHelper<Int>.DivRem(One, Two));
			Assert.Equal((new Int(0x3FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), One), BinaryIntegerHelper<Int>.DivRem(MaxValue, Two));
			Assert.Equal((new Int(0xC000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), Zero), BinaryIntegerHelper<Int>.DivRem(MinValue, Two));
			Assert.Equal((Zero, NegativeOne), BinaryIntegerHelper<Int>.DivRem(NegativeOne, 2));
		}

		[Fact]
		public static void LeadingZeroCountTest()
		{
			Assert.Equal(0x200, BinaryIntegerHelper<Int>.LeadingZeroCount(Zero));
			Assert.Equal(0x1FF, BinaryIntegerHelper<Int>.LeadingZeroCount(One));
			Assert.Equal(0x01, BinaryIntegerHelper<Int>.LeadingZeroCount(MaxValue));
			Assert.Equal(0x00, BinaryIntegerHelper<Int>.LeadingZeroCount(MinValue));
			Assert.Equal(0x00, BinaryIntegerHelper<Int>.LeadingZeroCount(NegativeOne));
		}

		[Fact]
		public static void PopCountTest()
		{
			Assert.Equal(0x00, BinaryIntegerHelper<Int>.PopCount(Zero));
			Assert.Equal(0x01, BinaryIntegerHelper<Int>.PopCount(One));
			Assert.Equal(0x1FF, BinaryIntegerHelper<Int>.PopCount(MaxValue));
			Assert.Equal(0x01, BinaryIntegerHelper<Int>.PopCount(MinValue));
			Assert.Equal(0x200, BinaryIntegerHelper<Int>.PopCount(NegativeOne));
		}

		[Fact]
		public static void RotateLeftTest()
		{
			Assert.Equal(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), BinaryIntegerHelper<Int>.RotateLeft(Zero, 1));
			Assert.Equal(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0002), BinaryIntegerHelper<Int>.RotateLeft(One, 1));
			Assert.Equal(new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFE), BinaryIntegerHelper<Int>.RotateLeft(MaxValue, 1));
			Assert.Equal(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0001), BinaryIntegerHelper<Int>.RotateLeft(MinValue, 1));
			Assert.Equal(new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), BinaryIntegerHelper<Int>.RotateLeft(NegativeOne, 1));
		}

		[Fact]
		public static void RotateRightTest()
		{
			Assert.Equal(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), BinaryIntegerHelper<Int>.RotateRight(Zero, 1));
			Assert.Equal(new(0x8000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), BinaryIntegerHelper<Int>.RotateRight(One, 1));
			Assert.Equal(new(0xBFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), BinaryIntegerHelper<Int>.RotateRight(MaxValue, 1));
			Assert.Equal(new(0x4000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), BinaryIntegerHelper<Int>.RotateRight(MinValue, 1));
			Assert.Equal(new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), BinaryIntegerHelper<Int>.RotateRight(NegativeOne, 1));
		}

		[Fact]
		public static void TrailingZeroCountTest()
		{
			Assert.Equal(0x200, BinaryIntegerHelper<Int>.TrailingZeroCount(Zero));
			Assert.Equal(0x00, BinaryIntegerHelper<Int>.TrailingZeroCount(One));
			Assert.Equal(0x00, BinaryIntegerHelper<Int>.TrailingZeroCount(MaxValue));
			Assert.Equal(0x1FF, BinaryIntegerHelper<Int>.TrailingZeroCount(MinValue));
			Assert.Equal(0x00, BinaryIntegerHelper<Int>.TrailingZeroCount(NegativeOne));
		}

		[Fact]
		public static void TryReadLittleEndianInt256Test()
		{
			Int result;

			Assert.True(BinaryIntegerHelper<Int>.TryReadLittleEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: false, out result));
			Assert.Equal(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), result);

			Assert.True(BinaryIntegerHelper<Int>.TryReadLittleEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 }, isUnsigned: false, out result));
			Assert.Equal(new(0x0100_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), result);

			Assert.True(BinaryIntegerHelper<Int>.TryReadLittleEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80 }, isUnsigned: false, out result));
			Assert.Equal(new(0x8000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), result);

			Assert.True(BinaryIntegerHelper<Int>.TryReadLittleEndian(new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: false, out result));
			Assert.Equal(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0001), result);

			Assert.True(BinaryIntegerHelper<Int>.TryReadLittleEndian(new byte[] { 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, isUnsigned: false, out result));
			Assert.Equal(new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FF7F), result);

			Assert.True(BinaryIntegerHelper<Int>.TryReadLittleEndian(new byte[] { 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: false, out result));
			Assert.Equal(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0080), result);

			Assert.True(BinaryIntegerHelper<Int>.TryReadLittleEndian(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F }, isUnsigned: false, out result));
			Assert.Equal(new(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), result);

			Assert.True(BinaryIntegerHelper<Int>.TryReadLittleEndian(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, isUnsigned: false, out result));
			Assert.Equal(new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), result);
		}

		[Fact]
		public static void TryReadLittleEndianUInt256Test()
		{
			Int result;

			Assert.True(BinaryIntegerHelper<Int>.TryReadLittleEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: true, out result));
			Assert.Equal(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), result);

			Assert.True(BinaryIntegerHelper<Int>.TryReadLittleEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 }, isUnsigned: true, out result));
			Assert.Equal(new(0x0100_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), result);

			Assert.False(BinaryIntegerHelper<Int>.TryReadLittleEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80 }, isUnsigned: true, out result));
			Assert.Equal(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), result);

			Assert.True(BinaryIntegerHelper<Int>.TryReadLittleEndian(new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: true, out result));
			Assert.Equal(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0001), result);

			Assert.False(BinaryIntegerHelper<Int>.TryReadLittleEndian(new byte[] { 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, isUnsigned: true, out result));
			Assert.Equal(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), result);

			Assert.True(BinaryIntegerHelper<Int>.TryReadLittleEndian(new byte[] { 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: true, out result));
			Assert.Equal(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0080), result);

			Assert.True(BinaryIntegerHelper<Int>.TryReadLittleEndian(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F }, isUnsigned: true, out result));
			Assert.Equal(new(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), result);

			Assert.False(BinaryIntegerHelper<Int>.TryReadLittleEndian(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, isUnsigned: true, out result));
			Assert.Equal(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), result);
		}

		[Fact]
		public static void GetByteCountTest()
		{
			Assert.Equal(64, BinaryIntegerHelper<Int>.GetByteCount(Zero));
			Assert.Equal(64, BinaryIntegerHelper<Int>.GetByteCount(One));
			Assert.Equal(64, BinaryIntegerHelper<Int>.GetByteCount(MaxValue));
			Assert.Equal(64, BinaryIntegerHelper<Int>.GetByteCount(MinValue));
			Assert.Equal(64, BinaryIntegerHelper<Int>.GetByteCount(NegativeOne));
		}

		[Fact]
		public static void GetShortestBitLengthTest()
		{
			Assert.Equal(0x00, BinaryIntegerHelper<Int>.GetShortestBitLength(Zero));
			Assert.Equal(0x01, BinaryIntegerHelper<Int>.GetShortestBitLength(One));
			Assert.Equal(0x1FF, BinaryIntegerHelper<Int>.GetShortestBitLength(MaxValue));
			Assert.Equal(0x200, BinaryIntegerHelper<Int>.GetShortestBitLength(MinValue));
			Assert.Equal(0x01, BinaryIntegerHelper<Int>.GetShortestBitLength(NegativeOne));
		}

		[Fact]
		public static void TryWriteBigEndianTest()
		{
			Span<byte> destination = stackalloc byte[64];
			int bytesWritten = 0;

			Assert.True(BinaryIntegerHelper<Int>.TryWriteBigEndian(Zero, destination, out bytesWritten));
			Assert.Equal(64, bytesWritten);
			Assert.Equal(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, destination.ToArray());

			Assert.True(BinaryIntegerHelper<Int>.TryWriteBigEndian(One, destination, out bytesWritten));
			Assert.Equal(64, bytesWritten);
			Assert.Equal(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 }, destination.ToArray());

			Assert.True(BinaryIntegerHelper<Int>.TryWriteBigEndian(MaxValue, destination, out bytesWritten));
			Assert.Equal(64, bytesWritten);
			Assert.Equal(new byte[] { 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, destination.ToArray());

			Assert.True(BinaryIntegerHelper<Int>.TryWriteBigEndian(MinValue, destination, out bytesWritten));
			Assert.Equal(64, bytesWritten);
			Assert.Equal(new byte[] { 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, destination.ToArray());

			Assert.True(BinaryIntegerHelper<Int>.TryWriteBigEndian(NegativeOne, destination, out bytesWritten));
			Assert.Equal(64, bytesWritten);
			Assert.Equal(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, destination.ToArray());

			Assert.False(BinaryIntegerHelper<Int>.TryWriteBigEndian(default, Span<byte>.Empty, out bytesWritten));
			Assert.Equal(0, bytesWritten);
			Assert.Equal(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, destination.ToArray());
		}

		[Fact]
		public static void TryWriteLittleEndianTest()
		{
			Span<byte> destination = stackalloc byte[64];
			int bytesWritten = 0;

			Assert.True(BinaryIntegerHelper<Int>.TryWriteLittleEndian(Zero, destination, out bytesWritten));
			Assert.Equal(64, bytesWritten);
			Assert.Equal(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, destination.ToArray());

			Assert.True(BinaryIntegerHelper<Int>.TryWriteLittleEndian(One, destination, out bytesWritten));
			Assert.Equal(64, bytesWritten);
			Assert.Equal(new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, destination.ToArray());

			Assert.True(BinaryIntegerHelper<Int>.TryWriteLittleEndian(MaxValue, destination, out bytesWritten));
			Assert.Equal(64, bytesWritten);
			Assert.Equal(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F }, destination.ToArray());

			Assert.True(BinaryIntegerHelper<Int>.TryWriteLittleEndian(MinValue, destination, out bytesWritten));
			Assert.Equal(64, bytesWritten);
			Assert.Equal(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80 }, destination.ToArray());

			Assert.True(BinaryIntegerHelper<Int>.TryWriteLittleEndian(NegativeOne, destination, out bytesWritten));
			Assert.Equal(64, bytesWritten);
			Assert.Equal(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, destination.ToArray());

			Assert.False(BinaryIntegerHelper<Int>.TryWriteLittleEndian(default, Span<byte>.Empty, out bytesWritten));
			Assert.Equal(0, bytesWritten);
			Assert.Equal(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, destination.ToArray());
		}
		#endregion

		#region IBinaryNumber
		[Fact]
		public static void AllBitsSetTest()
		{
			Assert.Equal(BinaryNumberHelper<Int>.AllBitsSet, ~Zero);
		}
		[Fact]
		public static void IsPow2Test()
		{
			Assert.True(BinaryNumberHelper<Int>.IsPow2(new(0x100)));
			Assert.True(BinaryNumberHelper<Int>.IsPow2(new(0x1_0000)));
			Assert.True(BinaryNumberHelper<Int>.IsPow2(new(0x1_0000_0000)));
			Assert.True(BinaryNumberHelper<Int>.IsPow2(new(0, new(0x1, 0x0000_0000_0000_0000))));
			Assert.True(BinaryNumberHelper<Int>.IsPow2(new(0x1, new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000))));
		}
		[Fact]
		public static void Log2Test()
		{
			Assert.Equal(8, BinaryNumberHelper<Int>.Log2(new(0x100)));
			Assert.Equal(16, BinaryNumberHelper<Int>.Log2(new(0x1_0000)));
			Assert.Equal(32, BinaryNumberHelper<Int>.Log2(new(0x1_0000_0000)));
			Assert.Equal(64, BinaryNumberHelper<Int>.Log2(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0001, 0x0000_0000_0000_0000)));
			Assert.Equal(128, BinaryNumberHelper<Int>.Log2(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0001, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000)));
		}
		#endregion

		#region IMinMaxValue
		[Fact]
		public static void MaxValueTest()
		{
			MaxValue.Should().Be(MathConstantsHelper.MaxValue<Int>());
		}

		[Fact]
		public static void MinValueTest()
		{
			MinValue.Should().Be(MathConstantsHelper.MinValue<Int>());
		}
		#endregion

		#region INumber
		[Fact]
		public static void ClampTest()
		{
			NumberHelper<Int>.Clamp(MaxValueMinusOne, Int128MaxValue, MaxValue).Should().Be(MaxValueMinusOne);
			NumberHelper<Int>.Clamp(MinValue, 0, MaxValue).Should().Be(0);
			NumberHelper<Int>.Clamp(MaxValue, MinValue, 0).Should().Be(0);

			Assert.Throws<ArgumentException>(() => NumberHelper<Int>.Clamp(MinValue, MaxValue, 0));
		}
		[Fact]
		public static void CopySignTest()
		{
			NumberHelper<Int>.CopySign(MaxValue, NegativeOne).Should().Be(MinValuePlusOne);
			NumberHelper<Int>.CopySign(MaxValue, One).Should().Be(MaxValue);
			NumberHelper<Int>.CopySign(NegativeTwo, One).Should().Be(Two);
		}
		[Fact]
		public static void MaxTest()
		{
			NumberHelper<Int>.Max(MaxValue, MinValue).Should().Be(MaxValue);
			NumberHelper<Int>.Max(One, NegativeTwo).Should().Be(One);
			NumberHelper<Int>.Max(Two, NegativeOne).Should().Be(Two);
			NumberHelper<Int>.Max(NegativeOne, NegativeTwo).Should().Be(NegativeOne);
		}
		[Fact]
		public static void MaxNumberTest()
		{
			NumberHelper<Int>.MaxNumber(MaxValue, MinValue).Should().Be(MaxValue);
			NumberHelper<Int>.MaxNumber(One, NegativeTwo).Should().Be(One);
			NumberHelper<Int>.MaxNumber(Two, NegativeOne).Should().Be(Two);
			NumberHelper<Int>.MaxNumber(NegativeOne, NegativeTwo).Should().Be(NegativeOne);
		}
		[Fact]
		public static void MinTest()
		{
			NumberHelper<Int>.Min(MaxValue, MinValue).Should().Be(MinValue);
			NumberHelper<Int>.Min(One, NegativeTwo).Should().Be(NegativeTwo);
			NumberHelper<Int>.Min(Two, NegativeOne).Should().Be(NegativeOne);
			NumberHelper<Int>.Min(NegativeOne, NegativeTwo).Should().Be(NegativeTwo);
		}
		[Fact]
		public static void MinNumberTest()
		{
			NumberHelper<Int>.MinNumber(MaxValue, MinValue).Should().Be(MinValue);
			NumberHelper<Int>.MinNumber(One, NegativeTwo).Should().Be(NegativeTwo);
			NumberHelper<Int>.MinNumber(Two, NegativeOne).Should().Be(NegativeOne);
			NumberHelper<Int>.MinNumber(NegativeOne, NegativeTwo).Should().Be(NegativeTwo);
		}
		[Fact]
		public static void SignTest()
		{
			NumberHelper<Int>.Sign(MinValue).Should().Be(-1);
			NumberHelper<Int>.Sign(MaxValue).Should().Be(1);
			NumberHelper<Int>.Sign(Int.Zero).Should().Be(0);
		}
		#endregion

		#region INumberBase
		[Fact]
		public static void AbsTest()
		{
			NumberBaseHelper<Int>.Abs(MinValuePlusOne).Should().Be(MaxValue);
			NumberBaseHelper<Int>.Abs(NegativeTwo).Should().Be(Two);
			NumberBaseHelper<Int>.Abs(NegativeOne).Should().Be(One);
			NumberBaseHelper<Int>.Abs(One).Should().Be(One);

			Assert.Throws<OverflowException>(() => NumberBaseHelper<Int>.Abs(MinValue));
		}
		[Fact]
		public static void CreateCheckedToInt512Test()
		{
			NumberBaseHelper<Int>.CreateChecked(byte.MaxValue).Should().Be(ByteMaxValue);
			NumberBaseHelper<Int>.CreateChecked(short.MaxValue).Should().Be(Int16MaxValue);
			NumberBaseHelper<Int>.CreateChecked(int.MaxValue).Should().Be(Int32MaxValue);
			NumberBaseHelper<Int>.CreateChecked(long.MaxValue).Should().Be(Int64MaxValue);
			NumberBaseHelper<Int>.CreateChecked(Int128.MaxValue).Should().Be(Int128MaxValue);
			NumberBaseHelper<Int>.CreateChecked((double)int.MaxValue).Should().Be(Int32MaxValue);

			NumberBaseHelper<Int>.CreateChecked(byte.MinValue).Should().Be(Zero);
			NumberBaseHelper<Int>.CreateChecked(short.MinValue).Should().Be(Int16MinValue);
			NumberBaseHelper<Int>.CreateChecked(int.MinValue).Should().Be(Int32MinValue);
			NumberBaseHelper<Int>.CreateChecked(long.MinValue).Should().Be(Int64MinValue);
			NumberBaseHelper<Int>.CreateChecked(Int128.MinValue).Should().Be(Int128MinValue);
			NumberBaseHelper<Int>.CreateChecked((double)int.MinValue).Should().Be(Int32MinValue);
		}
		[Fact]
		public static void CreateSaturatingToInt512Test()
		{
			NumberBaseHelper<Int>.CreateSaturating(byte.MaxValue).Should().Be(ByteMaxValue);
			NumberBaseHelper<Int>.CreateSaturating(short.MaxValue).Should().Be(Int16MaxValue);
			NumberBaseHelper<Int>.CreateSaturating(int.MaxValue).Should().Be(Int32MaxValue);
			NumberBaseHelper<Int>.CreateSaturating(long.MaxValue).Should().Be(Int64MaxValue);
			NumberBaseHelper<Int>.CreateSaturating(Int128.MaxValue).Should().Be(Int128MaxValue);
			NumberBaseHelper<Int>.CreateSaturating(Int512MaxValueAsDouble).Should().Be(MaxValue);

			NumberBaseHelper<Int>.CreateSaturating(byte.MinValue).Should().Be(Zero);
			NumberBaseHelper<Int>.CreateSaturating(short.MinValue).Should().Be(Int16MinValue);
			NumberBaseHelper<Int>.CreateSaturating(int.MinValue).Should().Be(Int32MinValue);
			NumberBaseHelper<Int>.CreateSaturating(long.MinValue).Should().Be(Int64MinValue);
			NumberBaseHelper<Int>.CreateSaturating(Int128.MinValue).Should().Be(Int128MinValue);
			NumberBaseHelper<Int>.CreateSaturating(Int512MinValueAsDouble).Should().Be(MinValue);
		}
		[Fact]
		public static void CreateTruncatingToInt512Test()
		{
			NumberBaseHelper<Int>.CreateTruncating(byte.MaxValue).Should().Be(ByteMaxValue);
			NumberBaseHelper<Int>.CreateTruncating(short.MaxValue).Should().Be(Int16MaxValue);
			NumberBaseHelper<Int>.CreateTruncating(int.MaxValue).Should().Be(Int32MaxValue);
			NumberBaseHelper<Int>.CreateTruncating(long.MaxValue).Should().Be(Int64MaxValue);
			NumberBaseHelper<Int>.CreateTruncating(Int128.MaxValue).Should().Be(Int128MaxValue);
			NumberBaseHelper<Int>.CreateTruncating(Int512MaxValueAsDouble).Should().Be(MaxValue);

			NumberBaseHelper<Int>.CreateTruncating(byte.MinValue).Should().Be(Zero);
			NumberBaseHelper<Int>.CreateTruncating(short.MinValue).Should().Be(Int16MinValue);
			NumberBaseHelper<Int>.CreateTruncating(int.MinValue).Should().Be(Int32MinValue);
			NumberBaseHelper<Int>.CreateTruncating(long.MinValue).Should().Be(Int64MinValue);
			NumberBaseHelper<Int>.CreateTruncating(Int128.MinValue).Should().Be(Int128MinValue);
			NumberBaseHelper<Int>.CreateTruncating(Int512MinValueAsDouble).Should().Be(MinValue);
		}

		[Fact]
		public static void CreateCheckedFromInt512Test()
		{
			NumberBaseHelper<byte>.CreateChecked(ByteMaxValue).Should().Be(byte.MaxValue);
			NumberBaseHelper<short>.CreateChecked(Int16MaxValue).Should().Be(short.MaxValue);
			NumberBaseHelper<int>.CreateChecked(Int32MaxValue).Should().Be(int.MaxValue);
			NumberBaseHelper<long>.CreateChecked(Int64MaxValue).Should().Be(long.MaxValue);
			NumberBaseHelper<Int128>.CreateChecked(Int128MaxValue).Should().Be(Int128.MaxValue);
			NumberBaseHelper<double>.CreateChecked(MaxValue).Should().Be(Int512MaxValueAsDouble);

			NumberBaseHelper<byte>.CreateChecked(Zero).Should().Be(byte.MinValue);
			NumberBaseHelper<short>.CreateChecked(Int16MinValue).Should().Be(short.MinValue);
			NumberBaseHelper<int>.CreateChecked(Int32MinValue).Should().Be(int.MinValue);
			NumberBaseHelper<long>.CreateChecked(Int64MinValue).Should().Be(long.MinValue);
			NumberBaseHelper<Int128>.CreateChecked(Int128MinValue).Should().Be(Int128.MinValue);
			NumberBaseHelper<double>.CreateChecked(MinValue).Should().Be(Int512MinValueAsDouble);
		}
		[Fact]
		public static void CreateSaturatingFromInt512Test()
		{
			NumberBaseHelper<byte>.CreateSaturating(ByteMaxValue).Should().Be(byte.MaxValue);
			NumberBaseHelper<short>.CreateSaturating(Int16MaxValue).Should().Be(short.MaxValue);
			NumberBaseHelper<int>.CreateSaturating(Int32MaxValue).Should().Be(int.MaxValue);
			NumberBaseHelper<long>.CreateSaturating(Int64MaxValue).Should().Be(long.MaxValue);
			NumberBaseHelper<Int128>.CreateSaturating(Int128MaxValue).Should().Be(Int128.MaxValue);
			NumberBaseHelper<double>.CreateSaturating(MaxValue).Should().Be(Int512MaxValueAsDouble);

			NumberBaseHelper<byte>.CreateSaturating(Zero).Should().Be(byte.MinValue);
			NumberBaseHelper<short>.CreateSaturating(Int16MinValue).Should().Be(short.MinValue);
			NumberBaseHelper<int>.CreateSaturating(Int32MinValue).Should().Be(int.MinValue);
			NumberBaseHelper<long>.CreateSaturating(Int64MinValue).Should().Be(long.MinValue);
			NumberBaseHelper<Int128>.CreateSaturating(Int128MinValue).Should().Be(Int128.MinValue);
			NumberBaseHelper<double>.CreateSaturating(MinValue).Should().Be(Int512MinValueAsDouble);
		}
		[Fact]
		public static void CreateTruncatingFromInt512Test()
		{
			NumberBaseHelper<byte>.CreateTruncating(ByteMaxValue).Should().Be(byte.MaxValue);
			NumberBaseHelper<short>.CreateTruncating(Int16MaxValue).Should().Be(short.MaxValue);
			NumberBaseHelper<int>.CreateTruncating(Int32MaxValue).Should().Be(int.MaxValue);
			NumberBaseHelper<long>.CreateTruncating(Int64MaxValue).Should().Be(long.MaxValue);
			NumberBaseHelper<Int128>.CreateTruncating(Int128MaxValue).Should().Be(Int128.MaxValue);
			NumberBaseHelper<double>.CreateTruncating(MaxValue).Should().Be(Int512MaxValueAsDouble);

			NumberBaseHelper<byte>.CreateTruncating(Zero).Should().Be(byte.MinValue);
			NumberBaseHelper<short>.CreateTruncating(Int16MinValue).Should().Be(short.MinValue);
			NumberBaseHelper<int>.CreateTruncating(Int32MinValue).Should().Be(int.MinValue);
			NumberBaseHelper<long>.CreateTruncating(Int64MinValue).Should().Be(long.MinValue);
			NumberBaseHelper<Int128>.CreateTruncating(Int128MinValue).Should().Be(Int128.MinValue);
			NumberBaseHelper<double>.CreateTruncating(MinValue).Should().Be(Int512MinValueAsDouble);
		}

		[Fact]
		public static void IsCanonicalTest()
		{
			NumberBaseHelper<Int>.IsCanonical(default(Int)).Should().BeTrue();
		}

		[Fact]
		public static void IsComplexNumberTest()
		{
			NumberBaseHelper<Int>.IsComplexNumber(default(Int)).Should().BeFalse();
		}

		[Fact]
		public static void IsEvenIntegerTest()
		{
			NumberBaseHelper<Int>.IsEvenInteger(One).Should().BeFalse();
			NumberBaseHelper<Int>.IsEvenInteger(Two).Should().BeTrue();
		}

		[Fact]
		public static void IsFiniteTest()
		{
			NumberBaseHelper<Int>.IsFinite(default(Int)).Should().BeTrue();
		}

		[Fact]
		public static void IsImaginaryNumberTest()
		{
			NumberBaseHelper<Int>.IsImaginaryNumber(default(Int)).Should().BeFalse();
		}

		[Fact]
		public static void IsInfinityTest()
		{
			NumberBaseHelper<Int>.IsInfinity(default(Int)).Should().BeFalse();
		}

		[Fact]
		public static void IsIntegerTest()
		{
			NumberBaseHelper<Int>.IsInteger(default(Int)).Should().BeTrue();
		}

		[Fact]
		public static void IsNaNTest()
		{
			NumberBaseHelper<Int>.IsNaN(default(Int)).Should().BeFalse();
		}

		[Fact]
		public static void IsNegativeTest()
		{
			NumberBaseHelper<Int>.IsNegative(One).Should().BeFalse();
			NumberBaseHelper<Int>.IsNegative(NegativeOne).Should().BeTrue();
		}

		[Fact]
		public static void IsNegativeInfinityTest()
		{
			NumberBaseHelper<Int>.IsNegativeInfinity(One).Should().BeFalse();
		}

		[Fact]
		public static void IsNormalTest()
		{
			NumberBaseHelper<Int>.IsNormal(Zero).Should().BeFalse();
			NumberBaseHelper<Int>.IsNormal(One).Should().BeTrue();
		}

		[Fact]
		public static void IsOddIntegerTest()
		{
			NumberBaseHelper<Int>.IsOddInteger(One).Should().BeTrue();
			NumberBaseHelper<Int>.IsOddInteger(Two).Should().BeFalse();
		}

		[Fact]
		public static void IsPositiveTest()
		{
			NumberBaseHelper<Int>.IsPositive(One).Should().BeTrue();
			NumberBaseHelper<Int>.IsPositive(NegativeOne).Should().BeFalse();
		}

		[Fact]
		public static void IsPositiveInfinityTest()
		{
			NumberBaseHelper<Int>.IsPositiveInfinity(One).Should().BeFalse();
		}

		[Fact]
		public static void IsRealNumberTest()
		{
			NumberBaseHelper<Int>.IsRealNumber(default).Should().BeTrue();
		}

		[Fact]
		public static void IsSubnormalTest()
		{
			NumberBaseHelper<Int>.IsSubnormal(default).Should().BeFalse();
		}

		[Fact]
		public static void IsZeroTest()
		{
			NumberBaseHelper<Int>.IsZero(default).Should().BeTrue();
			NumberBaseHelper<Int>.IsZero(Zero).Should().BeTrue();
			NumberBaseHelper<Int>.IsZero(One).Should().BeFalse();
			NumberBaseHelper<Int>.IsZero(NegativeOne).Should().BeFalse();
		}

		[Fact]
		public static void MaxMagnitudeTest()
		{
			NumberBaseHelper<Int>.MaxMagnitude(MaxValue, MinValue).Should().Be(MinValue);
			NumberBaseHelper<Int>.MaxMagnitude(One, NegativeTwo).Should().Be(NegativeTwo);
			NumberBaseHelper<Int>.MaxMagnitude(Two, NegativeOne).Should().Be(Two);
			NumberBaseHelper<Int>.MaxMagnitude(NegativeOne, NegativeTwo).Should().Be(NegativeTwo);
		}

		[Fact]
		public static void MaxMagnitudeNumberTest()
		{
			NumberBaseHelper<Int>.MaxMagnitudeNumber(MaxValue, MinValue).Should().Be(MinValue);
			NumberBaseHelper<Int>.MaxMagnitudeNumber(One, NegativeTwo).Should().Be(NegativeTwo);
			NumberBaseHelper<Int>.MaxMagnitudeNumber(Two, NegativeOne).Should().Be(Two);
			NumberBaseHelper<Int>.MaxMagnitudeNumber(NegativeOne, NegativeTwo).Should().Be(NegativeTwo);
		}

		[Fact]
		public static void MinMagnitudeTest()
		{
			NumberBaseHelper<Int>.MinMagnitude(MaxValue, MinValue).Should().Be(MaxValue);
			NumberBaseHelper<Int>.MinMagnitude(One, NegativeTwo).Should().Be(One);
			NumberBaseHelper<Int>.MinMagnitude(Two, NegativeOne).Should().Be(NegativeOne);
			NumberBaseHelper<Int>.MinMagnitude(NegativeOne, NegativeTwo).Should().Be(NegativeOne);
		}

		[Fact]
		public static void MinMagnitudeNumberTest()
		{
			NumberBaseHelper<Int>.MinMagnitudeNumber(MaxValue, MinValue).Should().Be(MaxValue);
			NumberBaseHelper<Int>.MinMagnitudeNumber(One, NegativeTwo).Should().Be(One);
			NumberBaseHelper<Int>.MinMagnitudeNumber(Two, NegativeOne).Should().Be(NegativeOne);
			NumberBaseHelper<Int>.MinMagnitudeNumber(NegativeOne, NegativeTwo).Should().Be(NegativeOne);
		}

		[Fact]
		public void ParseTest()
		{
			NumberBaseHelper<Int>.Parse("6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042047", System.Globalization.NumberStyles.Integer, CultureInfo.CurrentCulture)
				.Should().Be(Int512MaxValue)
				.And.BeRankedEquallyTo(Int512MaxValue);
			NumberBaseHelper<Int>.Parse("7FFFFFFFFFFFFFFF" +
										"FFFFFFFFFFFFFFFF" +
										"FFFFFFFFFFFFFFFF" +
										"FFFFFFFFFFFFFFFF" +
										"FFFFFFFFFFFFFFFF" +
										"FFFFFFFFFFFFFFFF" +
										"FFFFFFFFFFFFFFFF" +
										"FFFFFFFFFFFFFFFF", System.Globalization.NumberStyles.HexNumber, CultureInfo.CurrentCulture)
				.Should().Be(Int512MaxValue)
				.And.BeRankedEquallyTo(Int512MaxValue);
			NumberBaseHelper<Int>.Parse("-6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042048", System.Globalization.NumberStyles.Integer, CultureInfo.CurrentCulture)
				.Should().Be(Int512MinValue)
				.And.BeRankedEquallyTo(Int512MinValue);
			NumberBaseHelper<Int>.Parse("8000000000000000" +
										"0000000000000000" +
										"0000000000000000" +
										"0000000000000000" +
										"0000000000000000" +
										"0000000000000000" +
										"0000000000000000" +
										"0000000000000000", System.Globalization.NumberStyles.HexNumber, CultureInfo.CurrentCulture)
				.Should().Be(Int512MinValue)
				.And.BeRankedEquallyTo(Int512MinValue);

			Assert.Throws<OverflowException>(() => NumberBaseHelper<Int>.Parse("13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084096", System.Globalization.NumberStyles.Integer, CultureInfo.CurrentCulture));
		}

		[Fact]
		public void TryParseTest()
		{
			NumberBaseHelper<Int>.TryParse("6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042047", System.Globalization.NumberStyles.Integer, CultureInfo.CurrentCulture, out Int parsedValue)
				.Should().BeTrue();
			parsedValue.Should().Be(Int512MaxValue)
				.And.BeRankedEquallyTo(Int512MaxValue);
			NumberBaseHelper<Int>.TryParse("7FFFFFFFFFFFFFFF" +
										"FFFFFFFFFFFFFFFF" +
										"FFFFFFFFFFFFFFFF" +
										"FFFFFFFFFFFFFFFF" +
										"FFFFFFFFFFFFFFFF" +
										"FFFFFFFFFFFFFFFF" +
										"FFFFFFFFFFFFFFFF" +
										"FFFFFFFFFFFFFFFF", System.Globalization.NumberStyles.HexNumber, CultureInfo.CurrentCulture, out parsedValue)
				.Should().BeTrue();
			parsedValue.Should().Be(Int512MaxValue)
				.And.BeRankedEquallyTo(Int512MaxValue);
			NumberBaseHelper<Int>.TryParse("-6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042048", System.Globalization.NumberStyles.Integer, CultureInfo.CurrentCulture, out parsedValue)
				.Should().BeTrue();
			parsedValue.Should().Be(Int512MinValue)
				.And.BeRankedEquallyTo(Int512MinValue);
			NumberBaseHelper<Int>.TryParse("8000000000000000" +
										"0000000000000000" +
										"0000000000000000" +
										"0000000000000000" +
										"0000000000000000" +
										"0000000000000000" +
										"0000000000000000" +
										"0000000000000000", System.Globalization.NumberStyles.HexNumber, CultureInfo.CurrentCulture, out parsedValue)
				.Should().BeTrue();
			parsedValue.Should().Be(Int512MinValue)
				.And.BeRankedEquallyTo(Int512MinValue);

			NumberBaseHelper<Int>.TryParse("13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084096", System.Globalization.NumberStyles.Integer, CultureInfo.CurrentCulture, out parsedValue)
				.Should().BeFalse();
			parsedValue.Should().Be(default);
		}

		[Fact]
		public void ParseUtf8Test()
		{
			NumberBaseHelper<Int>.Parse("6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042047"u8, System.Globalization.NumberStyles.Integer, CultureInfo.CurrentCulture)
				.Should().Be(Int512MaxValue)
				.And.BeRankedEquallyTo(Int512MaxValue);
			NumberBaseHelper<Int>.Parse("7FFFFFFFFFFFFFFF" +
										"FFFFFFFFFFFFFFFF" +
										"FFFFFFFFFFFFFFFF" +
										"FFFFFFFFFFFFFFFF" +
										"FFFFFFFFFFFFFFFF" +
										"FFFFFFFFFFFFFFFF" +
										"FFFFFFFFFFFFFFFF" +
										"FFFFFFFFFFFFFFFF", System.Globalization.NumberStyles.HexNumber, CultureInfo.CurrentCulture)
				.Should().Be(Int512MaxValue)
				.And.BeRankedEquallyTo(Int512MaxValue);
			NumberBaseHelper<Int>.Parse("-6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042048"u8, System.Globalization.NumberStyles.Integer, CultureInfo.CurrentCulture)
				.Should().Be(Int512MinValue)
				.And.BeRankedEquallyTo(Int512MinValue);
			NumberBaseHelper<Int>.Parse("8000000000000000"u8 +
										"0000000000000000"u8 +
										"0000000000000000"u8 +
										"0000000000000000"u8 +
										"0000000000000000"u8 +
										"0000000000000000"u8 +
										"0000000000000000"u8 +
										"0000000000000000"u8, System.Globalization.NumberStyles.HexNumber, CultureInfo.CurrentCulture)
				.Should().Be(Int512MinValue)
				.And.BeRankedEquallyTo(Int512MinValue);

			Assert.Throws<OverflowException>(() => NumberBaseHelper<Int>.Parse("13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084096"u8, System.Globalization.NumberStyles.Integer, CultureInfo.CurrentCulture));
		}

		[Fact]
		public void TryParseUtf8Test()
		{
			NumberBaseHelper<Int>.TryParse("6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042047"u8, System.Globalization.NumberStyles.Integer, CultureInfo.CurrentCulture, out Int parsedValue)
				.Should().BeTrue();
			parsedValue.Should().Be(Int512MaxValue)
				.And.BeRankedEquallyTo(Int512MaxValue);
			NumberBaseHelper<Int>.TryParse("7FFFFFFFFFFFFFFF"u8 +
										"FFFFFFFFFFFFFFFF"u8 +
										"FFFFFFFFFFFFFFFF"u8 +
										"FFFFFFFFFFFFFFFF"u8 +
										"FFFFFFFFFFFFFFFF"u8 +
										"FFFFFFFFFFFFFFFF"u8 +
										"FFFFFFFFFFFFFFFF"u8 +
										"FFFFFFFFFFFFFFFF"u8, System.Globalization.NumberStyles.HexNumber, CultureInfo.CurrentCulture, out parsedValue)
				.Should().BeTrue();
			parsedValue.Should().Be(Int512MaxValue)
				.And.BeRankedEquallyTo(Int512MaxValue);
			NumberBaseHelper<Int>.TryParse("-6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042048"u8, System.Globalization.NumberStyles.Integer, CultureInfo.CurrentCulture, out parsedValue)
				.Should().BeTrue();
			parsedValue.Should().Be(Int512MinValue)
				.And.BeRankedEquallyTo(Int512MinValue);
			NumberBaseHelper<Int>.TryParse("8000000000000000"u8 +
										"0000000000000000"u8 +
										"0000000000000000"u8 +
										"0000000000000000"u8 +
										"0000000000000000"u8 +
										"0000000000000000"u8 +
										"0000000000000000"u8 +
										"0000000000000000"u8, System.Globalization.NumberStyles.HexNumber, CultureInfo.CurrentCulture, out parsedValue)
				.Should().BeTrue();
			parsedValue.Should().Be(Int512MinValue)
				.And.BeRankedEquallyTo(Int512MinValue);

			NumberBaseHelper<Int>.TryParse("13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084096"u8, System.Globalization.NumberStyles.Integer, CultureInfo.CurrentCulture, out parsedValue)
				.Should().BeFalse();
			parsedValue.Should().Be(default);
		}
		#endregion

		#region ISignedNumber
		[Fact]
		public static void NegativeOneTest()
		{
			MathConstantsHelper.NegativeOne<Int>().Should().Be(NegativeOne);	
		}
		#endregion
	}
}
