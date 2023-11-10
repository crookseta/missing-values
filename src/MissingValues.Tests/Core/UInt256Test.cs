using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UInt = MissingValues.UInt256;

namespace MissingValues.Tests.Core
{
	public partial class UInt256Test
	{
		[Fact]
		public void Cast_ToByte()
		{
			byte.MinValue.Should().Be((byte)Zero);
			byte.MaxValue.Should().Be((byte)ByteMaxValue);
		}

		[Fact]
		public void Cast_ToUInt16()
		{
			ushort.MinValue.Should().Be((ushort)Zero);
			ushort.MaxValue.Should().Be((ushort)UInt16MaxValue);
		}

		[Fact]
		public void Cast_ToUInt32()
		{
			uint.MinValue.Should().Be((uint)Zero);
			uint.MaxValue.Should().Be((uint)UInt32MaxValue);
		}

		[Fact]
		public void Cast_ToUInt64()
		{
			ulong.MinValue.Should().Be((ulong)Zero);
			ulong.MaxValue.Should().Be((ulong)UInt64MaxValue);
		}

		[Fact]
		public void Cast_ToUInt128()
		{
			UInt128.MinValue.Should().Be((UInt128)Zero);
			UInt128.MaxValue.Should().Be((UInt128)UInt128MaxValue);
		}

		[Fact]
		public void Cast_ToDouble()
		{
			// Test a UInt256Converter value where _upper is 0
			UInt value1 = new UInt(UInt128.Zero, UInt128.MaxValue);
			double exp1 = System.Math.Round((double)UInt128.MaxValue, 5);
			double act1 = System.Math.Round((double)value1, 5);
			double diff1 = System.Math.Abs(exp1 * 0.00000000001);
			Assert.True(System.Math.Abs(exp1 - act1) <= diff1);


			// Test a UInt256Converter value where _upper is not 0
			UInt value2 = new UInt(UInt128.MaxValue, UInt128.MaxValue);
			double exp2 = System.Math.Round(((double)UInt128.MaxValue) * System.Math.Pow(2.0, 128), 5);
			double act2 = System.Math.Round((double)value2, 5);
			double diff2 = System.Math.Abs(exp2 * 0.00000000001);
			Assert.True(System.Math.Abs(exp2 - act2) <= diff2);
		}

		[Fact]
		public void BigMulTest()
		{
			UInt upper = UInt.BigMul(MaxValue, Two, out UInt lower);

			upper
				.Should()
				.Be(new(0x1));
			lower
				.Should()
				.Be(new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFE));
		}

		[Fact]
		public void BasicParseTest()
		{
			UInt.Parse("115792089237316195423570985008687907853269984665640564039457584007913129639935")
				.Should().Be(MaxValue)
				.And.BeRankedEquallyTo(MaxValue);
		}

		[Fact]
		public void BasicTryParseTest()
		{
			UInt.TryParse("115792089237316195423570985008687907853269984665640564039457584007913129639935", out UInt parsedValue).Should().BeTrue();
			parsedValue.Should().Be(MaxValue)
				.And.BeRankedEquallyTo(MaxValue);
		}

		[Fact]
		public void ToDecStringTest()
		{
			MaxValue.ToString("D", CultureInfo.CurrentCulture)
				.Should().Be("115792089237316195423570985008687907853269984665640564039457584007913129639935");
		}
		[Fact]
		public void ToHexStringTest()
		{
			MaxValue.ToString("X64", CultureInfo.CurrentCulture)
				.Should().Be("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF");
		}
		[Fact]
		public void ToBinStringTest()
		{
			MaxValue.ToString("B256", CultureInfo.CurrentCulture)
				.Should().Be("1111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111");
		}

		[Fact]
		public void ToDecFormatStringTest()
		{
			MaxValue.ToString().Should().Be($"{MaxValue:D}");
		}
		[Fact]
		public void ToHexFormatStringTest()
		{
			MaxValue.ToString("X64", CultureInfo.CurrentCulture).Should().Be($"{MaxValue:X64}");
		}
		[Fact]
		public void ToBinFormatStringTest()
		{
			MaxValue.ToString("B256", CultureInfo.CurrentCulture).Should().Be($"{MaxValue:B256}");
		}
	}
}
