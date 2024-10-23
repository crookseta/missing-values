using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using UInt = MissingValues.UInt512;

namespace MissingValues.Tests.Core
{
	public partial class UInt512Test
	{
		#region Readonly Variables
		private const double MaxValueAsDouble = 13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084095.0;

		private static readonly UInt ByteMaxValue = new(
			0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000,
			0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_00FF);
		private static readonly UInt UInt16MaxValue = new(
			0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000,
			0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_FFFF);
		private static readonly UInt UInt32MaxValue = new(
			0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000,
			0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_FFFF_FFFF);
		private static readonly UInt UInt64MaxValue = new(
			0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000,
			0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0xFFFF_FFFF_FFFF_FFFF);
		private static readonly UInt UInt128MaxValue = new(
			0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000,
			0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);
		private static readonly UInt UInt256MaxValue = new(
			0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000,
			0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);

		private static readonly UInt Zero = new(
			0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000,
			0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		private static readonly UInt One = new(
			0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000,
			0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0001);
		private static readonly UInt Two = new(
			0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000,
			0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0002);

		private static readonly UInt MaxValue = new(
			0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF,
			0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);
		private static readonly UInt MaxValueMinusOne = new(
			0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF,
			0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFE);
		private static readonly UInt MaxValueMinusTwo = new(
			0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF,
			0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFD);
		private static readonly UInt HalfMaxValue = new(
			0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF,
			0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);

		private static readonly UInt E40 = new(
			0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000,
			0x0000_0000_0000_0000, 0x0000_0000_0000_001D, 0x6329_F1C3_5CA4_BFAB, 0xB9F5_6100_0000_0000);
		#endregion

		#region Generic Math Operators
		[Fact]
		public static void op_AdditionTest()
		{
			Assert.Equal(One, MathOperatorsHelper.AdditionOperation<UInt, UInt, UInt>(Zero, One));
			Assert.Equal(Two, MathOperatorsHelper.AdditionOperation<UInt, UInt, UInt>(One, One));
			Assert.Equal(Zero, MathOperatorsHelper.AdditionOperation<UInt, UInt, UInt>(MaxValue, One));
		}
		[Fact]
		public static void op_CheckedAdditionTest()
		{
			Assert.Equal(One, MathOperatorsHelper.CheckedAdditionOperation<UInt, UInt, UInt>(Zero, One));
			Assert.Equal(Two, MathOperatorsHelper.CheckedAdditionOperation<UInt, UInt, UInt>(One, One));

			Assert.Throws<OverflowException>(() => MathOperatorsHelper.CheckedAdditionOperation<UInt, UInt, UInt>(MaxValue, 1));
		}
		[Fact]	
		public static void op_IncrementTest()
		{
			MathOperatorsHelper.IncrementOperation(Zero).Should().Be(One);
			MathOperatorsHelper.IncrementOperation(One).Should().Be(Two);
			MathOperatorsHelper.IncrementOperation(MaxValueMinusTwo).Should().Be(MaxValueMinusOne);
			MathOperatorsHelper.IncrementOperation(MaxValueMinusOne).Should().Be(MaxValue);
			MathOperatorsHelper.IncrementOperation(MaxValue).Should().Be(Zero);
		}
		[Fact]	
		public static void op_CheckedIncrementTest()
		{
			MathOperatorsHelper.CheckedIncrementOperation(Zero).Should().Be(One);
			MathOperatorsHelper.CheckedIncrementOperation(One).Should().Be(Two);
			MathOperatorsHelper.CheckedIncrementOperation(MaxValueMinusTwo).Should().Be(MaxValueMinusOne);
			MathOperatorsHelper.CheckedIncrementOperation(MaxValueMinusOne).Should().Be(MaxValue);

			Assert.Throws<OverflowException>(() => MathOperatorsHelper.CheckedIncrementOperation(MaxValue));
		}
		[Fact]
		public static void op_SubtractionTest()
		{
			Assert.Equal(One, MathOperatorsHelper.SubtractionOperation<UInt, UInt, UInt>(Two, One));
			Assert.Equal(MaxValueMinusOne, MathOperatorsHelper.SubtractionOperation<UInt, UInt, UInt>(MaxValue, 1));
		}
		[Fact]
		public static void op_CheckedSubtractionTest()
		{
			Assert.Equal(One, MathOperatorsHelper.CheckedSubtractionOperation<UInt, UInt, UInt>(Two, One));
			Assert.Equal(MaxValueMinusOne, MathOperatorsHelper.CheckedSubtractionOperation<UInt, UInt, UInt>(MaxValue, 1));

			Assert.Throws<OverflowException>(() => MathOperatorsHelper.CheckedSubtractionOperation<UInt, UInt, UInt>(Zero, 1));
		}
		[Fact]
		public static void op_DecrementTest()
		{
			MathOperatorsHelper.DecrementOperation(Two).Should().Be(One);
			MathOperatorsHelper.DecrementOperation(One).Should().Be(Zero);
			MathOperatorsHelper.DecrementOperation(MaxValueMinusOne).Should().Be(MaxValueMinusTwo);
			MathOperatorsHelper.DecrementOperation(MaxValue).Should().Be(MaxValueMinusOne);
			MathOperatorsHelper.DecrementOperation(Zero).Should().Be(MaxValue);
		}
		[Fact]
		public static void op_CheckedDecrementTest()
		{
			MathOperatorsHelper.CheckedDecrementOperation(Two).Should().Be(One);
			MathOperatorsHelper.CheckedDecrementOperation(One).Should().Be(Zero);
			MathOperatorsHelper.CheckedDecrementOperation(MaxValueMinusOne).Should().Be(MaxValueMinusTwo);
			MathOperatorsHelper.CheckedDecrementOperation(MaxValue).Should().Be(MaxValueMinusOne);

			Assert.Throws<OverflowException>(() => MathOperatorsHelper.CheckedDecrementOperation(Zero));
		}
		[Fact]
		public static void op_MultiplyTest()
		{
			Assert.Equal(One, MathOperatorsHelper.MultiplicationOperation<UInt, UInt, UInt>(One, One));
			Assert.Equal(Two, MathOperatorsHelper.MultiplicationOperation<UInt, UInt, UInt>(Two, One));
			Assert.Equal(MaxValueMinusOne, MathOperatorsHelper.MultiplicationOperation<UInt, UInt, UInt>(Two, HalfMaxValue));
		}
		[Fact]
		public static void op_CheckedMultiplyTest()
		{
			Assert.Equal(One, MathOperatorsHelper.CheckedMultiplicationOperation<UInt, UInt, UInt>(One, One));
			Assert.Equal(Two, MathOperatorsHelper.CheckedMultiplicationOperation<UInt, UInt, UInt>(Two, One));

			Assert.Throws<OverflowException>(() => MathOperatorsHelper.CheckedMultiplicationOperation<UInt, UInt, UInt>(MaxValue, Two));
		}
		[Fact]
		public static void op_DivisionTest()
		{
			Assert.Equal(
				  HalfMaxValue,
				  MathOperatorsHelper.DivisionOperation<UInt, UInt, UInt>(MaxValue, Two));
			Assert.Equal(
				  new(0x028F_5C28_F5C2_8F5C, 0x28F5_C28F_5C28_F5C2, 0x8F5C_28F5_C28F_5C28, 0xF5C2_8F5C_28F5_C28F,
				  0x5C28_F5C2_8F5C_28F5, 0xC28F_5C28_F5C2_8F5C, 0x28F5_C28F_5C28_F5C2, 0x8F5C_28F5_C28F_5C28),
				  MaxValue / 100);
			Assert.Equal(
				  new(0x0006_8DB8_BAC7_10CB, 0x295E_9E1B_089A_0275, 0x2546_0AA6_4C2F_837B, 0x4A23_39C0_EBED_FA43,
				  0xFE5C_91D1_4E3B_CD35, 0xA858_793D_D97F_62B6, 0xAE7D_566C_F41F_212D, 0x7731_8FC5_0481_6F00),
				  (MaxValue / 100) / 100);
			Assert.Equal(
				new UInt(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x08B6_1313_BBAB_CE2C, 0x6232_3AC4_B3B3_DA01, 
				0x53B6_2BE7_BC1A_0042, 0xB443_E18A_C4E7_0AFD, 0xB897_7684_D802_7110, 0x5B47_424E_B16F_CC18), 
				MathOperatorsHelper.DivisionOperation<UInt, UInt, UInt>(MaxValue, E40));
			Assert.Equal(
				new UInt(
					0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000,
					0x1DA4_8CE4_68E7_C702, 0x6520_247D_3556_476D, 0x1469_CAF6_DB22_4CF9, 0x7D7D_B189_5F2A_4679
				), 
				MathOperatorsHelper.DivisionOperation<UInt, UInt, UInt>(new UInt(
					0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x08B6_1313_BBAB_CE2C, 0x6232_3AC4_B3B3_DA01,
				0x53B6_2BE7_BC1A_0042, 0xB443_E18A_C4E7_0AFD, 0xB897_7684_D802_7110, 0x5B47_424E_B16F_CC18
					), 
					new UInt(
						0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000,
				0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x4B3B_4CA8_5A86_C47A, 0x098A_2240_0000_0000
						)));
			Assert.Equal(One, MathOperatorsHelper.DivisionOperation<UInt, UInt, UInt>(MaxValue, MaxValue));

			Assert.Throws<DivideByZeroException>(() => MathOperatorsHelper.DivisionOperation<UInt, UInt, UInt>(One, Zero));
		}
		[Fact]
		public static void op_CheckedDivisionTest()
		{
			Assert.Equal(
				  HalfMaxValue,
				  MathOperatorsHelper.CheckedDivisionOperation<UInt, UInt, UInt>(MaxValue, Two));
			Assert.Equal(One, MathOperatorsHelper.CheckedDivisionOperation<UInt, UInt, UInt>(MaxValue, MaxValue));
			Assert.Equal(
				new UInt(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x08B6_1313_BBAB_CE2C, 0x6232_3AC4_B3B3_DA01,
				0x53B6_2BE7_BC1A_0042, 0xB443_E18A_C4E7_0AFD, 0xB897_7684_D802_7110, 0x5B47_424E_B16F_CC18),
				MathOperatorsHelper.CheckedDivisionOperation<UInt, UInt, UInt>(MaxValue, E40));

			Assert.Throws<DivideByZeroException>(() => MathOperatorsHelper.CheckedDivisionOperation<UInt, UInt, UInt>(One, Zero));
		}
		[Fact]
		public static void op_ModulusTest()
		{
			MathOperatorsHelper.ModulusOperation<UInt, UInt, UInt>(Two, Two).Should().Be(Zero);
			MathOperatorsHelper.ModulusOperation<UInt, UInt, UInt>(One, Two).Should().NotBe(Zero);
			MathOperatorsHelper.ModulusOperation<UInt, UInt, UInt>(MaxValue, new(10U)).Should().Be(5U);
			MathOperatorsHelper.ModulusOperation<UInt, UInt, UInt>(MaxValue, E40)
				.Should().Be(
				new UInt(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000,
					0x0000_0000_0000_0000, 0x0000_0000_0000_0011, 0xC828_0B1A_5E03_5840, 0xF8B2_E7FF_FFFF_FFFF));

			Assert.Throws<DivideByZeroException>(() => MathOperatorsHelper.ModulusOperation<UInt, UInt, UInt>(One, Zero));
		}

		[Fact]
		public static void op_BitwiseAndTest()
		{
			BitwiseOperatorsHelper<UInt, UInt, UInt>.BitwiseAndOperation(Zero, 1U).Should().Be(Zero);
			BitwiseOperatorsHelper<UInt, UInt, UInt>.BitwiseAndOperation(One, 1U).Should().Be(One);
			BitwiseOperatorsHelper<UInt, UInt, UInt>.BitwiseAndOperation(MaxValue, 1U).Should().Be(One);
		}
		[Fact]
		public static void op_BitwiseOrTest()
		{
			BitwiseOperatorsHelper<UInt, UInt, UInt>.BitwiseOrOperation(Zero, 1U)
				.Should().Be(One);
			BitwiseOperatorsHelper<UInt, UInt, UInt>.BitwiseOrOperation(One, 1U)
				.Should().Be(One);
			BitwiseOperatorsHelper<UInt, UInt, UInt>.BitwiseOrOperation(MaxValue, 1U)
				.Should().Be(MaxValue);
		}
		[Fact]
		public static void op_ExclusiveOrTest()
		{
			BitwiseOperatorsHelper<UInt, UInt, UInt>.ExclusiveOrOperation(Zero, 1U)
				.Should().Be(One);
			BitwiseOperatorsHelper<UInt, UInt, UInt>.ExclusiveOrOperation(One, 1U)
				.Should().Be(Zero);
			BitwiseOperatorsHelper<UInt, UInt, UInt>.ExclusiveOrOperation(MaxValue, 1U)
				.Should().Be(MaxValueMinusOne);
		}
		[Fact]
		public static void op_OnesComplementTest()
		{
			BitwiseOperatorsHelper<UInt, UInt, UInt>.OnesComplementOperation(Zero)
				.Should().Be(MaxValue);
			BitwiseOperatorsHelper<UInt, UInt, UInt>.OnesComplementOperation(One)
				.Should().Be(MaxValueMinusOne);
			BitwiseOperatorsHelper<UInt, UInt, UInt>.OnesComplementOperation(MaxValue)
				.Should().Be(Zero);
		}

		[Fact]
		public static void op_LeftShiftTest()
		{
			ShiftOperatorsHelper<UInt, int, UInt>.LeftShiftOperation(Zero, 1)
				.Should().Be(Zero);
			ShiftOperatorsHelper<UInt, int, UInt>.LeftShiftOperation(One, 1)
				.Should().Be(Two);
			ShiftOperatorsHelper<UInt, int, UInt>.LeftShiftOperation(MaxValue, 1)
				.Should().Be(MaxValueMinusOne);

			UInt actual = new(
				0x028F_5C28_F5C2_8F5C, 0x28F5_C28F_5C28_F5C2, 0x8F5C_28F5_C28F_5C28, 0xF5C2_8F5C_28F5_C28F, 
				0x5C28_F5C2_8F5C_28F5, 0xC28F_5C28_F5C2_8F5C, 0x28F5_C28F_5C28_F5C2, 0x8F5C_28F5_C28F_5C28);

			ShiftOperatorsHelper<UInt, int, UInt>.LeftShiftOperation(actual, 0)
				.Should().Be(actual);
			ShiftOperatorsHelper<UInt, int, UInt>.LeftShiftOperation(actual, 64)
				.Should().Be(new(
					0x28F5_C28F_5C28_F5C2, 0x8F5C_28F5_C28F_5C28, 0xF5C2_8F5C_28F5_C28F, 0x5C28_F5C2_8F5C_28F5, 
				0xC28F_5C28_F5C2_8F5C, 0x28F5_C28F_5C28_F5C2, 0x8F5C_28F5_C28F_5C28, 0));
			ShiftOperatorsHelper<UInt, int, UInt>.LeftShiftOperation(actual, 64 * 2)
				.Should().Be(new(
					0x8F5C_28F5_C28F_5C28, 0xF5C2_8F5C_28F5_C28F, 0x5C28_F5C2_8F5C_28F5, 0xC28F_5C28_F5C2_8F5C,
				 0x28F5_C28F_5C28_F5C2, 0x8F5C_28F5_C28F_5C28, 0, 0));
			ShiftOperatorsHelper<UInt, int, UInt>.LeftShiftOperation(actual, 64 * 3)
				.Should().Be(new(
					0xF5C2_8F5C_28F5_C28F, 0x5C28_F5C2_8F5C_28F5, 0xC28F_5C28_F5C2_8F5C, 0x28F5_C28F_5C28_F5C2,
				  0x8F5C_28F5_C28F_5C28, 0, 0, 0));
			ShiftOperatorsHelper<UInt, int, UInt>.LeftShiftOperation(actual, 64 * 4)
				.Should().Be(new(
					0x5C28_F5C2_8F5C_28F5, 0xC28F_5C28_F5C2_8F5C, 0x28F5_C28F_5C28_F5C2, 0x8F5C_28F5_C28F_5C28,
				  0, 0, 0, 0 ));
			ShiftOperatorsHelper<UInt, int, UInt>.LeftShiftOperation(actual, 64 * 5)
				.Should().Be(new(
					0xC28F_5C28_F5C2_8F5C, 0x28F5_C28F_5C28_F5C2, 0x8F5C_28F5_C28F_5C28, 0,
				  0, 0, 0, 0));
			ShiftOperatorsHelper<UInt, int, UInt>.LeftShiftOperation(actual, 64 * 6)
				.Should().Be(new(
					0x28F5_C28F_5C28_F5C2, 0x8F5C_28F5_C28F_5C28, 0, 0,
				  0, 0, 0, 0));
			ShiftOperatorsHelper<UInt, int, UInt>.LeftShiftOperation(actual, 64 * 7)
				.Should().Be(new(
					0x8F5C_28F5_C28F_5C28, 0, 0, 0,
				  0, 0, 0, 0));
		}
		[Fact]
		public static void op_RightShiftTest()
		{
			ShiftOperatorsHelper<UInt, int, UInt>.RightShiftOperation(Zero, 1)
				.Should().Be(Zero);
			ShiftOperatorsHelper<UInt, int, UInt>.RightShiftOperation(One, 1)
				.Should().Be(Zero);
			ShiftOperatorsHelper<UInt, int, UInt>.RightShiftOperation(MaxValue, 1)
				.Should().Be(HalfMaxValue);

			UInt actual = new(
				0x028F_5C28_F5C2_8F5C, 0x28F5_C28F_5C28_F5C2, 0x8F5C_28F5_C28F_5C28, 0xF5C2_8F5C_28F5_C28F,
				0x5C28_F5C2_8F5C_28F5, 0xC28F_5C28_F5C2_8F5C, 0x28F5_C28F_5C28_F5C2, 0x8F5C_28F5_C28F_5C28);

			ShiftOperatorsHelper<UInt, int, UInt>.RightShiftOperation(actual, 64 * 0)
				.Should().Be(actual);
			ShiftOperatorsHelper<UInt, int, UInt>.RightShiftOperation(actual, 64 * 1)
				.Should().Be(new(
					0, 0x028F_5C28_F5C2_8F5C, 0x28F5_C28F_5C28_F5C2, 0x8F5C_28F5_C28F_5C28,
					0xF5C2_8F5C_28F5_C28F, 0x5C28_F5C2_8F5C_28F5, 0xC28F_5C28_F5C2_8F5C, 0x28F5_C28F_5C28_F5C2));
			ShiftOperatorsHelper<UInt, int, UInt>.RightShiftOperation(actual, 64 * 2)
				.Should().Be(new(
					0, 0, 0x028F_5C28_F5C2_8F5C, 0x28F5_C28F_5C28_F5C2, 
					0x8F5C_28F5_C28F_5C28, 0xF5C2_8F5C_28F5_C28F, 0x5C28_F5C2_8F5C_28F5, 0xC28F_5C28_F5C2_8F5C));
			ShiftOperatorsHelper<UInt, int, UInt>.RightShiftOperation(actual, 64 * 3)
				.Should().Be(new(
					0, 0, 0, 0x028F_5C28_F5C2_8F5C, 
					0x28F5_C28F_5C28_F5C2, 0x8F5C_28F5_C28F_5C28, 0xF5C2_8F5C_28F5_C28F, 0x5C28_F5C2_8F5C_28F5));
			ShiftOperatorsHelper<UInt, int, UInt>.RightShiftOperation(actual, 64 * 4)
				.Should().Be(new(
					0, 0, 0, 0, 
					0x028F_5C28_F5C2_8F5C, 0x28F5_C28F_5C28_F5C2, 0x8F5C_28F5_C28F_5C28, 0xF5C2_8F5C_28F5_C28F));
			ShiftOperatorsHelper<UInt, int, UInt>.RightShiftOperation(actual, 64 * 5)
				.Should().Be(new(
					0, 0, 0, 0, 
					0, 0x028F_5C28_F5C2_8F5C, 0x28F5_C28F_5C28_F5C2, 0x8F5C_28F5_C28F_5C28));
			ShiftOperatorsHelper<UInt, int, UInt>.RightShiftOperation(actual, 64 * 6)
				.Should().Be(new(
					0, 0, 0, 0, 
					0, 0, 0x028F_5C28_F5C2_8F5C, 0x28F5_C28F_5C28_F5C2));
			ShiftOperatorsHelper<UInt, int, UInt>.RightShiftOperation(actual, 64 * 7)
				.Should().Be(new(
					0, 0, 0, 0, 
					0, 0, 0, 0x028F_5C28_F5C2_8F5C));
		}
		[Fact]
		public static void op_UnsignedRightShiftTest()
		{
			ShiftOperatorsHelper<UInt, int, UInt>.UnsignedRightShiftOperation(Zero, 1)
				.Should().Be(Zero);
			ShiftOperatorsHelper<UInt, int, UInt>.UnsignedRightShiftOperation(One, 1)
				.Should().Be(Zero);
			ShiftOperatorsHelper<UInt, int, UInt>.UnsignedRightShiftOperation(MaxValue, 1)
				.Should().Be(HalfMaxValue);

			UInt actual = new(
				0x028F_5C28_F5C2_8F5C, 0x28F5_C28F_5C28_F5C2, 0x8F5C_28F5_C28F_5C28, 0xF5C2_8F5C_28F5_C28F,
				0x5C28_F5C2_8F5C_28F5, 0xC28F_5C28_F5C2_8F5C, 0x28F5_C28F_5C28_F5C2, 0x8F5C_28F5_C28F_5C28);

			ShiftOperatorsHelper<UInt, int, UInt>.UnsignedRightShiftOperation(actual, 64 * 0)
				.Should().Be(actual);
			ShiftOperatorsHelper<UInt, int, UInt>.UnsignedRightShiftOperation(actual, 64 * 1)
				.Should().Be(new(
					0, 0x028F_5C28_F5C2_8F5C, 0x28F5_C28F_5C28_F5C2, 0x8F5C_28F5_C28F_5C28,
					0xF5C2_8F5C_28F5_C28F, 0x5C28_F5C2_8F5C_28F5, 0xC28F_5C28_F5C2_8F5C, 0x28F5_C28F_5C28_F5C2));
			ShiftOperatorsHelper<UInt, int, UInt>.UnsignedRightShiftOperation(actual, 64 * 2)
				.Should().Be(new(
					0, 0, 0x028F_5C28_F5C2_8F5C, 0x28F5_C28F_5C28_F5C2,
					0x8F5C_28F5_C28F_5C28, 0xF5C2_8F5C_28F5_C28F, 0x5C28_F5C2_8F5C_28F5, 0xC28F_5C28_F5C2_8F5C));
			ShiftOperatorsHelper<UInt, int, UInt>.UnsignedRightShiftOperation(actual, 64 * 3)
				.Should().Be(new(
					0, 0, 0, 0x028F_5C28_F5C2_8F5C,
					0x28F5_C28F_5C28_F5C2, 0x8F5C_28F5_C28F_5C28, 0xF5C2_8F5C_28F5_C28F, 0x5C28_F5C2_8F5C_28F5));
			ShiftOperatorsHelper<UInt, int, UInt>.UnsignedRightShiftOperation(actual, 64 * 4)
				.Should().Be(new(
					0, 0, 0, 0,
					0x028F_5C28_F5C2_8F5C, 0x28F5_C28F_5C28_F5C2, 0x8F5C_28F5_C28F_5C28, 0xF5C2_8F5C_28F5_C28F));
			ShiftOperatorsHelper<UInt, int, UInt>.UnsignedRightShiftOperation(actual, 64 * 5)
				.Should().Be(new(
					0, 0, 0, 0,
					0, 0x028F_5C28_F5C2_8F5C, 0x28F5_C28F_5C28_F5C2, 0x8F5C_28F5_C28F_5C28));
			ShiftOperatorsHelper<UInt, int, UInt>.UnsignedRightShiftOperation(actual, 64 * 6)
				.Should().Be(new(
					0, 0, 0, 0,
					0, 0, 0x028F_5C28_F5C2_8F5C, 0x28F5_C28F_5C28_F5C2));
			ShiftOperatorsHelper<UInt, int, UInt>.UnsignedRightShiftOperation(actual, 64 * 7)
				.Should().Be(new(
					0, 0, 0, 0,
					0, 0, 0, 0x028F_5C28_F5C2_8F5C));
		}

		[Fact]
		public static void op_EqualityTest()
		{
			EqualityOperatorsHelper<UInt, UInt, bool>.EqualityOperation(Zero, 1U).Should().BeFalse();
			EqualityOperatorsHelper<UInt, UInt, bool>.EqualityOperation(One, 1U).Should().BeTrue();
			EqualityOperatorsHelper<UInt, UInt, bool>.EqualityOperation(MaxValue, 1U).Should().BeFalse();
		}
		[Fact]
		public static void op_InequalityTest()
		{
			EqualityOperatorsHelper<UInt, UInt, bool>.InequalityOperation(Zero, 1U).Should().BeTrue();
			EqualityOperatorsHelper<UInt, UInt, bool>.InequalityOperation(One, 1U).Should().BeFalse();
			EqualityOperatorsHelper<UInt, UInt, bool>.InequalityOperation(MaxValue, 1U).Should().BeTrue();
		}

		[Fact]
		public static void op_GreaterThanTest()
		{
			ComparisonOperatorsHelper<UInt, UInt, bool>.GreaterThanOperation(Zero, 1U).Should().BeFalse();
			ComparisonOperatorsHelper<UInt, UInt, bool>.GreaterThanOperation(One, 1U).Should().BeFalse();
			ComparisonOperatorsHelper<UInt, UInt, bool>.GreaterThanOperation(MaxValue, 1U).Should().BeTrue();

			ComparisonOperatorsHelper<UInt, UInt, bool>.GreaterThanOperation(
				new(0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1), new(0x0, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1))
				.Should().BeTrue();
			ComparisonOperatorsHelper<UInt, UInt, bool>.GreaterThanOperation(
				new(0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1), new(0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x0))
				.Should().BeTrue();
			ComparisonOperatorsHelper<UInt, UInt, bool>.GreaterThanOperation(
				new(0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1), new(0x1, 0x1, 0x1, 0x1, 0x1, 0x0, 0x1, 0x1))
				.Should().BeTrue();
		}
		[Fact]
		public static void op_GreaterThanOrEqualTest()
		{
			ComparisonOperatorsHelper<UInt, UInt, bool>.GreaterThanOrEqualOperation(Zero, 1U).Should().BeFalse();
			ComparisonOperatorsHelper<UInt, UInt, bool>.GreaterThanOrEqualOperation(One, 1U).Should().BeTrue();
			ComparisonOperatorsHelper<UInt, UInt, bool>.GreaterThanOrEqualOperation(MaxValue, 1U).Should().BeTrue();
		}
		[Fact]
		public static void op_LessThanTest()
		{
			ComparisonOperatorsHelper<UInt, UInt, bool>.LessThanOperation(Zero, 1U).Should().BeTrue();
			ComparisonOperatorsHelper<UInt, UInt, bool>.LessThanOperation(One, 1U).Should().BeFalse();
			ComparisonOperatorsHelper<UInt, UInt, bool>.LessThanOperation(MaxValue, 1U).Should().BeFalse();
		}
		[Fact]
		public static void op_LessThanOrEqualTest()
		{
			ComparisonOperatorsHelper<UInt, UInt, bool>.LessThanOrEqualOperation(Zero, 1U).Should().BeTrue();
			ComparisonOperatorsHelper<UInt, UInt, bool>.LessThanOrEqualOperation(One, 1U).Should().BeTrue();
			ComparisonOperatorsHelper<UInt, UInt, bool>.LessThanOrEqualOperation(MaxValue, 1U).Should().BeFalse();
		}
		#endregion

		#region Identities
		[Fact]
		public static void AdditiveIdentityTest()
		{
			Assert.Equal(Zero, MathConstantsHelper.AdditiveIdentityHelper<UInt, UInt>());
		}

		[Fact]
		public static void MultiplicativeIdentityTest()
		{
			Assert.Equal(One, MathConstantsHelper.MultiplicativeIdentityHelper<UInt, UInt>());
		}
		#endregion

		#region IBinaryInteger
		[Fact]
		public static void DivRemTest()
		{
			Assert.Equal((Zero, Zero), BinaryIntegerHelper<UInt>.DivRem(Zero, Two));
			Assert.Equal((Zero, One), BinaryIntegerHelper<UInt>.DivRem(One, Two));
			Assert.Equal((HalfMaxValue, One), BinaryIntegerHelper<UInt>.DivRem(MaxValue, 2));
			Assert.Equal((
				new UInt(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x08B6_1313_BBAB_CE2C, 0x6232_3AC4_B3B3_DA01,
				0x53B6_2BE7_BC1A_0042, 0xB443_E18A_C4E7_0AFD, 0xB897_7684_D802_7110, 0x5B47_424E_B16F_CC18),
				new UInt(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000,
					0x0000_0000_0000_0000, 0x0000_0000_0000_0011, 0xC828_0B1A_5E03_5840, 0xF8B2_E7FF_FFFF_FFFF)),
				BinaryIntegerHelper<UInt>.DivRem(MaxValue, E40));
		}

		[Fact]
		public static void LeadingZeroCountTest()
		{
			Assert.Equal(512U, BinaryIntegerHelper<UInt>.LeadingZeroCount(Zero));
			Assert.Equal(511U, BinaryIntegerHelper<UInt>.LeadingZeroCount(One));
			Assert.Equal(448U, BinaryIntegerHelper<UInt>.LeadingZeroCount(ulong.MaxValue));
			Assert.Equal(384U, BinaryIntegerHelper<UInt>.LeadingZeroCount(UInt128.MaxValue));
			Assert.Equal(256U, BinaryIntegerHelper<UInt>.LeadingZeroCount(UInt256.MaxValue));
			Assert.Equal(192U, BinaryIntegerHelper<UInt>.LeadingZeroCount(MaxValue >>> 192));
			Assert.Equal(128U, BinaryIntegerHelper<UInt>.LeadingZeroCount(MaxValue >>> 128));
			Assert.Equal(64U, BinaryIntegerHelper<UInt>.LeadingZeroCount(MaxValue >>> 64));
			Assert.Equal(0U, BinaryIntegerHelper<UInt>.LeadingZeroCount(MaxValue));
		}

		[Fact]
		public static void PopCountTest()
		{
			Assert.Equal(0U, BinaryIntegerHelper<UInt>.PopCount(Zero));
			Assert.Equal(1U, BinaryIntegerHelper<UInt>.PopCount(One));
			Assert.Equal(512U, BinaryIntegerHelper<UInt>.PopCount(MaxValue));
		}

		[Fact]
		public static void RotateLeftTest()
		{
			Assert.Equal(Zero, BinaryIntegerHelper<UInt>.RotateLeft(Zero, 1));
			Assert.Equal(Two, BinaryIntegerHelper<UInt>.RotateLeft(One, 1));
			Assert.Equal(MaxValue, BinaryIntegerHelper<UInt>.RotateLeft(MaxValue, 1));
		}

		[Fact]
		public static void RotateRightTest()
		{
			Assert.Equal(new(
				0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000,
				0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), BinaryIntegerHelper<UInt>.RotateRight(Zero, 1));
			Assert.Equal(new(
				  0x8000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000,
				  0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), BinaryIntegerHelper<UInt>.RotateRight(One, 1));
			Assert.Equal(new(
				  0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF,
				  0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), BinaryIntegerHelper<UInt>.RotateRight(MaxValue, 1));
		}

		[Fact]
		public static void TrailingZeroCountTest()
		{
			Assert.Equal(512U, BinaryIntegerHelper<UInt>.TrailingZeroCount(Zero));
			Assert.Equal(0U, BinaryIntegerHelper<UInt>.TrailingZeroCount(One));
			Assert.Equal(64U, BinaryIntegerHelper<UInt>.TrailingZeroCount(new UInt(0, 0, 0, 0, 0, 0, 1, 0)));
			Assert.Equal(128U, BinaryIntegerHelper<UInt>.TrailingZeroCount(new UInt(0, 0, 0, 0, 0, 1, 0, 0)));
			Assert.Equal(192U, BinaryIntegerHelper<UInt>.TrailingZeroCount(new UInt(0, 0, 0, 0, 1, 0, 0, 0)));
			Assert.Equal(256U, BinaryIntegerHelper<UInt>.TrailingZeroCount(new UInt(0, 0, 0, 1, 0, 0, 0, 0)));
			Assert.Equal(320U, BinaryIntegerHelper<UInt>.TrailingZeroCount(new UInt(0, 0, 1, 0, 0, 0, 0, 0)));
			Assert.Equal(384U, BinaryIntegerHelper<UInt>.TrailingZeroCount(new UInt(0, 1, 0, 0, 0, 0, 0, 0)));
			Assert.Equal(448U, BinaryIntegerHelper<UInt>.TrailingZeroCount(new UInt(1, 0, 0, 0, 0, 0, 0, 0)));
			Assert.Equal(0U, BinaryIntegerHelper<UInt>.TrailingZeroCount(MaxValue));
		}
		[Fact]
		public static void TryReadLittleEndianInt256Test()
		{
			UInt result;

			Assert.True(BinaryIntegerHelper<UInt>.TryReadLittleEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: false, out result));
			Assert.Equal(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), result);

			Assert.True(BinaryIntegerHelper<UInt>.TryReadLittleEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 }, isUnsigned: false, out result));
			Assert.Equal(new(0x0100_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), result);

			Assert.False(BinaryIntegerHelper<UInt>.TryReadLittleEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80 }, isUnsigned: false, out result));
			Assert.Equal(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), result);

			Assert.True(BinaryIntegerHelper<UInt>.TryReadLittleEndian(new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: false, out result));
			Assert.Equal(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0001), result);

			Assert.False(BinaryIntegerHelper<UInt>.TryReadLittleEndian(new byte[] { 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, isUnsigned: false, out result));
			Assert.Equal(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), result);

			Assert.True(BinaryIntegerHelper<UInt>.TryReadLittleEndian(new byte[] { 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: false, out result));
			Assert.Equal(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0080), result);

			Assert.True(BinaryIntegerHelper<UInt>.TryReadLittleEndian(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F }, isUnsigned: false, out result));
			Assert.Equal(new(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), result);

			Assert.False(BinaryIntegerHelper<UInt>.TryReadLittleEndian(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, isUnsigned: false, out result));
			Assert.Equal(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), result);
		}

		[Fact]
		public static void TryReadLittleEndianUInt256Test()
		{
			UInt result;

			Assert.True(BinaryIntegerHelper<UInt>.TryReadLittleEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: true, out result));
			Assert.Equal(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), result);

			Assert.True(BinaryIntegerHelper<UInt>.TryReadLittleEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 }, isUnsigned: true, out result));
			Assert.Equal(new(0x0100_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), result);

			Assert.True(BinaryIntegerHelper<UInt>.TryReadLittleEndian(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80 }, isUnsigned: true, out result));
			Assert.Equal(new(0x8000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), result);

			Assert.True(BinaryIntegerHelper<UInt>.TryReadLittleEndian(new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: true, out result));
			Assert.Equal(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0001), result);

			Assert.True(BinaryIntegerHelper<UInt>.TryReadLittleEndian(new byte[] { 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, isUnsigned: true, out result));
			Assert.Equal(new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FF7F), result);

			Assert.True(BinaryIntegerHelper<UInt>.TryReadLittleEndian(new byte[] { 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isUnsigned: true, out result));
			Assert.Equal(new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0080), result);

			Assert.True(BinaryIntegerHelper<UInt>.TryReadLittleEndian(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F }, isUnsigned: true, out result));
			Assert.Equal(new(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), result);

			Assert.True(BinaryIntegerHelper<UInt>.TryReadLittleEndian(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, isUnsigned: true, out result));
			Assert.Equal(new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), result);
		}

		[Fact]
		public static void GetByteCountTest()
		{
			Assert.Equal(64, BinaryIntegerHelper<UInt>.GetByteCount(Zero));
			Assert.Equal(64, BinaryIntegerHelper<UInt>.GetByteCount(One));
			Assert.Equal(64, BinaryIntegerHelper<UInt>.GetByteCount(MaxValue));
		}

		[Fact]
		public static void GetShortestBitLengthTest()
		{
			Assert.Equal(0x00, BinaryIntegerHelper<UInt>.GetShortestBitLength(Zero));
			Assert.Equal(0x01, BinaryIntegerHelper<UInt>.GetShortestBitLength(One));
			Assert.Equal(0x200, BinaryIntegerHelper<UInt>.GetShortestBitLength(MaxValue));
		}

		[Fact]
		public static void TryWriteBigEndianTest()
		{
			Span<byte> destination = stackalloc byte[64];
			int bytesWritten = 0;

			Assert.True(BinaryIntegerHelper<UInt>.TryWriteBigEndian(Zero, destination, out bytesWritten));
			Assert.Equal(64, bytesWritten);
			Assert.Equal(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, destination.ToArray());

			Assert.True(BinaryIntegerHelper<UInt>.TryWriteBigEndian(One, destination, out bytesWritten));
			Assert.Equal(64, bytesWritten);
			Assert.Equal(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 }, destination.ToArray());

			Assert.True(BinaryIntegerHelper<UInt>.TryWriteBigEndian(MaxValue, destination, out bytesWritten));
			Assert.Equal(64, bytesWritten);
			Assert.Equal(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, destination.ToArray());

			Assert.False(BinaryIntegerHelper<UInt>.TryWriteBigEndian(default, Span<byte>.Empty, out bytesWritten));
			Assert.Equal(0, bytesWritten);
			Assert.Equal(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, destination.ToArray());
		}

		[Fact]
		public static void TryWriteLittleEndianTest()
		{
			Span<byte> destination = stackalloc byte[64];
			int bytesWritten = 0;

			Assert.True(BinaryIntegerHelper<UInt>.TryWriteLittleEndian(Zero, destination, out bytesWritten));
			Assert.Equal(64, bytesWritten);
			Assert.Equal(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, destination.ToArray());

			Assert.True(BinaryIntegerHelper<UInt>.TryWriteLittleEndian(One, destination, out bytesWritten));
			Assert.Equal(64, bytesWritten);
			Assert.Equal(new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, destination.ToArray());

			Assert.True(BinaryIntegerHelper<UInt>.TryWriteLittleEndian(MaxValue, destination, out bytesWritten));
			Assert.Equal(64, bytesWritten);
			Assert.Equal(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, destination.ToArray());

			Assert.False(BinaryIntegerHelper<UInt>.TryWriteLittleEndian(default, Span<byte>.Empty, out bytesWritten));
			Assert.Equal(0, bytesWritten);
			Assert.Equal(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, destination.ToArray());
		}
		#endregion

		#region IBinaryNumber
		[Fact]
		public static void AllBitsSetTest()
		{
			Assert.Equal(BinaryNumberHelper<UInt>.AllBitsSet, ~Zero);
		}
		[Fact]
		public static void IsPow2Test()
		{
			Assert.True(BinaryNumberHelper<UInt>.IsPow2(new(0x100)));
			Assert.True(BinaryNumberHelper<UInt>.IsPow2(new(0x1_0000)));
			Assert.True(BinaryNumberHelper<UInt>.IsPow2(new(0x1_0000_0000)));
			Assert.True(BinaryNumberHelper<UInt>.IsPow2(new(
				0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000,
				0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0001, 0x0000_0000_0000_0000)));
			Assert.True(BinaryNumberHelper<UInt>.IsPow2(new(
				0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000,
				0x0000_0000_0000_0000, 0x0000_0000_0000_0001, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000)));
		}
		[Fact]
		public static void Log2Test()
		{
			Assert.Equal(8U, BinaryNumberHelper<UInt>.Log2(new(0x100)));
			Assert.Equal(16U, BinaryNumberHelper<UInt>.Log2(new(0x1_0000)));
			Assert.Equal(32U, BinaryNumberHelper<UInt>.Log2(new(0x1_0000_0000)));
			Assert.Equal(64U, BinaryNumberHelper<UInt>.Log2(new(
				0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000,
				0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0001, 0x0000_0000_0000_0000)));
			Assert.Equal(128U, BinaryNumberHelper<UInt>.Log2(new(
				0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000,
				0x0000_0000_0000_0000, 0x0000_0000_0000_0001, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000)));
			Assert.Equal(256U, BinaryNumberHelper<UInt>.Log2(new(
				0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0001,
				0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000)));
		}
		#endregion

		#region IMinMaxValue
		[Fact]
		public static void MaxValueTest()
		{
			MaxValue.Should().Be(MathConstantsHelper.MaxValue<UInt>());
		}

		[Fact]
		public static void MinValueTest()
		{
			Zero.Should().Be(MathConstantsHelper.MinValue<UInt>());
		}
		#endregion

		#region INumber
		[Fact]
		public static void ClampTest()
		{
			NumberHelper<UInt>.Clamp(MaxValueMinusOne, UInt128MaxValue, MaxValue).Should().Be(MaxValueMinusOne);
			NumberHelper<UInt>.Clamp(Zero, Two, MaxValue).Should().Be(Two);
			NumberHelper<UInt>.Clamp(MaxValue, Zero, One).Should().Be(One);

			Assert.Throws<ArgumentException>(() => NumberHelper<UInt>.Clamp(Zero, MaxValue, Zero));
		}
		[Fact]
		public static void CopySignTest()
		{
			NumberHelper<UInt>.CopySign(MaxValue, One).Should().Be(MaxValue);
		}
		[Fact]
		public static void MaxTest()
		{
			NumberHelper<UInt>.Max(MaxValue, Two).Should().Be(MaxValue);
			NumberHelper<UInt>.Max(One, Zero).Should().Be(One);
			NumberHelper<UInt>.Max(Two, Zero).Should().Be(Two);
			NumberHelper<UInt>.Max(Two, One).Should().Be(Two);
		}
		[Fact]
		public static void MaxNumberTest()
		{
			NumberHelper<UInt>.MaxNumber(MaxValue, Zero).Should().Be(MaxValue);
			NumberHelper<UInt>.MaxNumber(One, Zero).Should().Be(One);
			NumberHelper<UInt>.MaxNumber(Two, Zero).Should().Be(Two);
		}
		[Fact]
		public static void MinTest()
		{
			NumberHelper<UInt>.Min(MaxValue, Zero).Should().Be(Zero);
			NumberHelper<UInt>.Min(One,	Zero).Should().Be(Zero);
			NumberHelper<UInt>.Min(Two, Zero).Should().Be(Zero);
		}
		[Fact]
		public static void MinNumberTest()
		{
			NumberHelper<UInt>.MinNumber(MaxValue, Zero).Should().Be(Zero);
			NumberHelper<UInt>.MinNumber(One,Zero).Should().Be(Zero);
			NumberHelper<UInt>.MinNumber(Two, Zero).Should().Be(Zero);
		}
		[Fact]
		public static void SignTest()
		{
			NumberHelper<UInt>.Sign(MaxValue).Should().Be(1);
			NumberHelper<UInt>.Sign(UInt.Zero).Should().Be(0);
		}
		#endregion

		#region INumberBase
		[Fact]
		public static void AbsTest()
		{
			NumberBaseHelper<UInt>.Abs(One).Should().Be(One);
		}
		[Fact]
		public static void CreateCheckedToUInt512Test()
		{
			NumberBaseHelper<UInt>.CreateChecked(byte.MaxValue).Should().Be(ByteMaxValue);
			NumberBaseHelper<UInt>.CreateChecked(ushort.MaxValue).Should().Be(UInt16MaxValue);
			NumberBaseHelper<UInt>.CreateChecked(uint.MaxValue).Should().Be(UInt32MaxValue);
			NumberBaseHelper<UInt>.CreateChecked(ulong.MaxValue).Should().Be(UInt64MaxValue);
			NumberBaseHelper<UInt>.CreateChecked(UInt128.MaxValue).Should().Be(UInt128MaxValue);
			NumberBaseHelper<UInt>.CreateChecked(UInt256.MaxValue).Should().Be(UInt256MaxValue);
			NumberBaseHelper<UInt>
				.CreateChecked(BigInteger.Parse(
					"13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084095"))
				.Should().Be(MaxValue);
			NumberBaseHelper<UInt>.CreateChecked(MaxValueAsDouble).Should().Be(MaxValue);

			NumberBaseHelper<UInt>.CreateChecked(byte.MinValue).Should().Be(Zero);
			NumberBaseHelper<UInt>.CreateChecked(ushort.MinValue).Should().Be(Zero);
			NumberBaseHelper<UInt>.CreateChecked(uint.MinValue).Should().Be(Zero);
			NumberBaseHelper<UInt>.CreateChecked(ulong.MinValue).Should().Be(Zero);
			NumberBaseHelper<UInt>.CreateChecked(UInt128.MinValue).Should().Be(Zero);
			NumberBaseHelper<UInt>.CreateChecked(UInt256.MinValue).Should().Be(Zero);
		}
		[Fact]
		public static void CreateSaturatingToUInt512Test()
		{
			NumberBaseHelper<UInt>.CreateSaturating(byte.MaxValue).Should().Be(ByteMaxValue);
			NumberBaseHelper<UInt>.CreateSaturating(ushort.MaxValue).Should().Be(UInt16MaxValue);
			NumberBaseHelper<UInt>.CreateSaturating(uint.MaxValue).Should().Be(UInt32MaxValue);
			NumberBaseHelper<UInt>.CreateSaturating(ulong.MaxValue).Should().Be(UInt64MaxValue);
			NumberBaseHelper<UInt>.CreateSaturating(UInt128.MaxValue).Should().Be(UInt128MaxValue);
			NumberBaseHelper<UInt>.CreateSaturating(UInt256.MaxValue).Should().Be(UInt256MaxValue);
			NumberBaseHelper<UInt>
				.CreateSaturating(BigInteger.Parse(
					"13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084095"))
				.Should().Be(MaxValue);
			NumberBaseHelper<UInt>.CreateSaturating(MaxValueAsDouble).Should().Be(MaxValue);

			NumberBaseHelper<UInt>.CreateSaturating(byte.MinValue).Should().Be(Zero);
			NumberBaseHelper<UInt>.CreateSaturating(ushort.MinValue).Should().Be(Zero);
			NumberBaseHelper<UInt>.CreateSaturating(uint.MinValue).Should().Be(Zero);
			NumberBaseHelper<UInt>.CreateSaturating(ulong.MinValue).Should().Be(Zero);
			NumberBaseHelper<UInt>.CreateSaturating(UInt128.MinValue).Should().Be(Zero);
			NumberBaseHelper<UInt>.CreateSaturating(UInt256.MinValue).Should().Be(Zero);
		}
		[Fact]
		public static void CreateTruncatingToUInt512Test()
		{
			NumberBaseHelper<UInt>.CreateTruncating(byte.MaxValue).Should().Be(ByteMaxValue);
			NumberBaseHelper<UInt>.CreateTruncating(ushort.MaxValue).Should().Be(UInt16MaxValue);
			NumberBaseHelper<UInt>.CreateTruncating(uint.MaxValue).Should().Be(UInt32MaxValue);
			NumberBaseHelper<UInt>.CreateTruncating(ulong.MaxValue).Should().Be(UInt64MaxValue);
			NumberBaseHelper<UInt>.CreateTruncating(UInt128.MaxValue).Should().Be(UInt128MaxValue);
			NumberBaseHelper<UInt>.CreateTruncating(UInt256.MaxValue).Should().Be(UInt256MaxValue);
			NumberBaseHelper<UInt>
				.CreateTruncating(BigInteger.Parse(
					"13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084095"))
				.Should().Be(MaxValue);
			NumberBaseHelper<UInt>.CreateTruncating(MaxValueAsDouble).Should().Be(MaxValue);

			NumberBaseHelper<UInt>.CreateTruncating(byte.MinValue).Should().Be(Zero);
			NumberBaseHelper<UInt>.CreateTruncating(ushort.MinValue).Should().Be(Zero);
			NumberBaseHelper<UInt>.CreateTruncating(uint.MinValue).Should().Be(Zero);
			NumberBaseHelper<UInt>.CreateTruncating(ulong.MinValue).Should().Be(Zero);
			NumberBaseHelper<UInt>.CreateTruncating(UInt128.MinValue).Should().Be(Zero);
			NumberBaseHelper<UInt>.CreateTruncating(UInt256.MinValue).Should().Be(Zero);
		}

		[Fact]
		public static void CreateCheckedFromUInt512Test()
		{
			NumberBaseHelper<byte>.CreateChecked(ByteMaxValue).Should().Be(byte.MaxValue);
			NumberBaseHelper<ushort>.CreateChecked(UInt16MaxValue).Should().Be(ushort.MaxValue);
			NumberBaseHelper<uint>.CreateChecked(UInt32MaxValue).Should().Be(uint.MaxValue);
			NumberBaseHelper<ulong>.CreateChecked(UInt64MaxValue).Should().Be(ulong.MaxValue);
			NumberBaseHelper<UInt128>.CreateChecked(UInt128MaxValue).Should().Be(UInt128.MaxValue);
			NumberBaseHelper<UInt256>.CreateChecked(UInt256MaxValue).Should().Be(UInt256.MaxValue);
			NumberBaseHelper<BigInteger>.CreateChecked(MaxValue).Should()
				.Be(BigInteger.Parse(
					"13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084095"));
			NumberBaseHelper<double>.CreateChecked(MaxValue).Should().Be(MaxValueAsDouble);

			NumberBaseHelper<byte>.CreateChecked(Zero).Should().Be(byte.MinValue);
			NumberBaseHelper<ushort>.CreateChecked(Zero).Should().Be(ushort.MinValue);
			NumberBaseHelper<uint>.CreateChecked(Zero).Should().Be(uint.MinValue);
			NumberBaseHelper<ulong>.CreateChecked(Zero).Should().Be(ulong.MinValue);
			NumberBaseHelper<UInt128>.CreateChecked(Zero).Should().Be(UInt128.MinValue);
			NumberBaseHelper<UInt256>.CreateChecked(Zero).Should().Be(UInt256.MinValue);
		}
		[Fact]
		public static void CreateSaturatingFromUInt512Test()
		{
			NumberBaseHelper<byte>.CreateSaturating(ByteMaxValue).Should().Be(byte.MaxValue);
			NumberBaseHelper<ushort>.CreateSaturating(UInt16MaxValue).Should().Be(ushort.MaxValue);
			NumberBaseHelper<uint>.CreateSaturating(UInt32MaxValue).Should().Be(uint.MaxValue);
			NumberBaseHelper<ulong>.CreateSaturating(UInt64MaxValue).Should().Be(ulong.MaxValue);
			NumberBaseHelper<UInt128>.CreateSaturating(UInt128MaxValue).Should().Be(UInt128.MaxValue);
			NumberBaseHelper<UInt256>.CreateSaturating(UInt256MaxValue).Should().Be(UInt256.MaxValue);
			NumberBaseHelper<BigInteger>.CreateSaturating(MaxValue).Should()
				.Be(BigInteger.Parse(
					"13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084095"));
			NumberBaseHelper<double>.CreateSaturating(MaxValue).Should().Be(MaxValueAsDouble);

			NumberBaseHelper<byte>.CreateSaturating(Zero).Should().Be(byte.MinValue);
			NumberBaseHelper<ushort>.CreateSaturating(Zero).Should().Be(ushort.MinValue);
			NumberBaseHelper<uint>.CreateSaturating(Zero).Should().Be(uint.MinValue);
			NumberBaseHelper<ulong>.CreateSaturating(Zero).Should().Be(ulong.MinValue);
			NumberBaseHelper<UInt128>.CreateSaturating(Zero).Should().Be(UInt128.MinValue);
			NumberBaseHelper<UInt256>.CreateSaturating(Zero).Should().Be(UInt256.MinValue);
		}
		[Fact]
		public static void CreateTruncatingFromUInt512Test()
		{
			NumberBaseHelper<byte>.CreateTruncating(ByteMaxValue).Should().Be(byte.MaxValue);
			NumberBaseHelper<ushort>.CreateTruncating(UInt16MaxValue).Should().Be(ushort.MaxValue);
			NumberBaseHelper<uint>.CreateTruncating(UInt32MaxValue).Should().Be(uint.MaxValue);
			NumberBaseHelper<ulong>.CreateTruncating(UInt64MaxValue).Should().Be(ulong.MaxValue);
			NumberBaseHelper<UInt128>.CreateTruncating(UInt128MaxValue).Should().Be(UInt128.MaxValue);
			NumberBaseHelper<UInt256>.CreateTruncating(UInt256MaxValue).Should().Be(UInt256.MaxValue);
			NumberBaseHelper<BigInteger>.CreateTruncating(MaxValue).Should()
				.Be(BigInteger.Parse(
					"13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084095"));
			NumberBaseHelper<double>.CreateTruncating(MaxValue).Should().Be(MaxValueAsDouble);

			NumberBaseHelper<byte>.CreateTruncating(Zero).Should().Be(byte.MinValue);
			NumberBaseHelper<ushort>.CreateTruncating(Zero).Should().Be(ushort.MinValue);
			NumberBaseHelper<uint>.CreateTruncating(Zero).Should().Be(uint.MinValue);
			NumberBaseHelper<ulong>.CreateTruncating(Zero).Should().Be(ulong.MinValue);
			NumberBaseHelper<UInt128>.CreateTruncating(Zero).Should().Be(UInt128.MinValue);
			NumberBaseHelper<UInt256>.CreateTruncating(Zero).Should().Be(UInt256.MinValue);
		}

		[Fact]
		public static void IsCanonicalTest()
		{
			NumberBaseHelper<UInt>.IsCanonical(default(UInt)).Should().BeTrue();
		}

		[Fact]
		public static void IsComplexNumberTest()
		{
			NumberBaseHelper<UInt>.IsComplexNumber(default(UInt)).Should().BeFalse();
		}

		[Fact]
		public static void IsEvenIntegerTest()
		{
			NumberBaseHelper<UInt>.IsEvenInteger(One).Should().BeFalse();
			NumberBaseHelper<UInt>.IsEvenInteger(Two).Should().BeTrue();
		}

		[Fact]
		public static void IsFiniteTest()
		{
			NumberBaseHelper<UInt>.IsFinite(default(UInt)).Should().BeTrue();
		}

		[Fact]
		public static void IsImaginaryNumberTest()
		{
			NumberBaseHelper<UInt>.IsImaginaryNumber(default(UInt)).Should().BeFalse();
		}

		[Fact]
		public static void IsInfinityTest()
		{
			NumberBaseHelper<UInt>.IsInfinity(default(UInt)).Should().BeFalse();
		}

		[Fact]
		public static void IsIntegerTest()
		{
			NumberBaseHelper<UInt>.IsInteger(default(UInt)).Should().BeTrue();
		}

		[Fact]
		public static void IsNaNTest()
		{
			NumberBaseHelper<UInt>.IsNaN(default(UInt)).Should().BeFalse();
		}

		[Fact]
		public static void IsNegativeTest()
		{
			NumberBaseHelper<UInt>.IsNegative(One).Should().BeFalse();
		}

		[Fact]
		public static void IsNegativeInfinityTest()
		{
			NumberBaseHelper<UInt>.IsNegativeInfinity(One).Should().BeFalse();
		}

		[Fact]
		public static void IsNormalTest()
		{
			NumberBaseHelper<UInt>.IsNormal(Zero).Should().BeFalse();
			NumberBaseHelper<UInt>.IsNormal(One).Should().BeTrue();
		}

		[Fact]
		public static void IsOddIntegerTest()
		{
			NumberBaseHelper<UInt>.IsOddInteger(One).Should().BeTrue();
			NumberBaseHelper<UInt>.IsOddInteger(Two).Should().BeFalse();
		}

		[Fact]
		public static void IsPositiveTest()
		{
			NumberBaseHelper<UInt>.IsPositive(One).Should().BeTrue();
		}

		[Fact]
		public static void IsPositiveInfinityTest()
		{
			NumberBaseHelper<UInt>.IsPositiveInfinity(One).Should().BeFalse();
		}

		[Fact]
		public static void IsRealNumberTest()
		{
			NumberBaseHelper<UInt>.IsRealNumber(default).Should().BeTrue();
		}

		[Fact]
		public static void IsSubnormalTest()
		{
			NumberBaseHelper<UInt>.IsSubnormal(default).Should().BeFalse();
		}

		[Fact]
		public static void IsZeroTest()
		{
			NumberBaseHelper<UInt>.IsZero(default).Should().BeTrue();
			NumberBaseHelper<UInt>.IsZero(Zero).Should().BeTrue();
			NumberBaseHelper<UInt>.IsZero(One).Should().BeFalse();
		}

		[Fact]
		public static void MaxMagnitudeTest()
		{
			NumberBaseHelper<UInt>.MaxMagnitude(MaxValue, Zero).Should().Be(MaxValue);
			NumberBaseHelper<UInt>.MaxMagnitude(One, Zero).Should().Be(One);
			NumberBaseHelper<UInt>.MaxMagnitude(Two, Zero).Should().Be(Two);
		}

		[Fact]
		public static void MaxMagnitudeNumberTest()
		{
			NumberBaseHelper<UInt>.MaxMagnitudeNumber(MaxValue, Zero).Should().Be(MaxValue);
			NumberBaseHelper<UInt>.MaxMagnitudeNumber(One, Zero).Should().Be(One);
			NumberBaseHelper<UInt>.MaxMagnitudeNumber(Two, Zero).Should().Be(Two);
		}

		[Fact]
		public static void MinMagnitudeTest()
		{
			NumberBaseHelper<UInt>.MinMagnitude(MaxValue, MaxValueMinusOne).Should().Be(MaxValueMinusOne);
			NumberBaseHelper<UInt>.MinMagnitude(One, Zero).Should().Be(Zero);
			NumberBaseHelper<UInt>.MinMagnitude(Two, Zero).Should().Be(Zero);
		}

		[Fact]
		public static void MinMagnitudeNumberTest()
		{
			NumberBaseHelper<UInt>.MinMagnitudeNumber(MaxValue, MaxValueMinusOne).Should().Be(MaxValueMinusOne);
			NumberBaseHelper<UInt>.MinMagnitudeNumber(One, Zero).Should().Be(Zero);
			NumberBaseHelper<UInt>.MinMagnitudeNumber(Two, Zero).Should().Be(Zero);
		}

		[Fact]
		public void ParseTest()
		{
			NumberBaseHelper<UInt>.Parse("13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084095", System.Globalization.NumberStyles.Integer, CultureInfo.CurrentCulture)
				.Should().Be(MaxValue)
				.And.BeRankedEquallyTo(MaxValue);
			NumberBaseHelper<UInt>.Parse("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", System.Globalization.NumberStyles.HexNumber, CultureInfo.CurrentCulture)
				.Should().Be(MaxValue)
				.And.BeRankedEquallyTo(MaxValue);

			Assert.Throws<OverflowException>(() => NumberBaseHelper<UInt>.Parse("13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084096", System.Globalization.NumberStyles.Integer, CultureInfo.CurrentCulture));
		}

		[Fact]
		public void TryParseTest()
		{
			NumberBaseHelper<UInt>.TryParse("13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084095", System.Globalization.NumberStyles.Integer, CultureInfo.CurrentCulture, out UInt parsedValue)
				.Should().BeTrue();
			parsedValue.Should().Be(MaxValue)
				.And.BeRankedEquallyTo(MaxValue);
			NumberBaseHelper<UInt>.TryParse("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", System.Globalization.NumberStyles.HexNumber, CultureInfo.CurrentCulture, out parsedValue)
				.Should().BeTrue();
			parsedValue.Should().Be(MaxValue)
				.And.BeRankedEquallyTo(MaxValue);

			NumberBaseHelper<UInt>.TryParse("13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084096", System.Globalization.NumberStyles.Integer, CultureInfo.CurrentCulture, out parsedValue)
				.Should().BeFalse();
			parsedValue.Should().Be(default);
		}

		[Fact]
		public void ParseUtf8Test()
		{
			NumberBaseHelper<UInt>.Parse("13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084095"u8, System.Globalization.NumberStyles.Integer, CultureInfo.CurrentCulture)
				.Should().Be(MaxValue)
				.And.BeRankedEquallyTo(MaxValue);
			NumberBaseHelper<UInt>.Parse("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF"u8, System.Globalization.NumberStyles.HexNumber, CultureInfo.CurrentCulture)
				.Should().Be(MaxValue)
				.And.BeRankedEquallyTo(MaxValue);

			Assert.Throws<OverflowException>(() => NumberBaseHelper<UInt>.Parse("13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084096"u8, System.Globalization.NumberStyles.Integer, CultureInfo.CurrentCulture));
		}

		[Fact]
		public void TryParseUtf8Test()
		{
			NumberBaseHelper<UInt>.TryParse("13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084095"u8, System.Globalization.NumberStyles.Integer, CultureInfo.CurrentCulture, out UInt parsedValue)
				.Should().BeTrue();
			parsedValue.Should().Be(MaxValue)
				.And.BeRankedEquallyTo(MaxValue);
			NumberBaseHelper<UInt>.TryParse("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF"u8, System.Globalization.NumberStyles.HexNumber, CultureInfo.CurrentCulture, out parsedValue)
				.Should().BeTrue();
			parsedValue.Should().Be(MaxValue)
				.And.BeRankedEquallyTo(MaxValue);

			NumberBaseHelper<UInt>.TryParse("13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084096"u8, System.Globalization.NumberStyles.Integer, CultureInfo.CurrentCulture, out parsedValue)
				.Should().BeFalse();
			parsedValue.Should().Be(default);
		}
		#endregion
	}
}
