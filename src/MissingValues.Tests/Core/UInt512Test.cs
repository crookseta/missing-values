using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UInt = MissingValues.UInt512;

namespace MissingValues.Tests.Core
{
	public partial class UInt512Test
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
		public void Cast_ToUInt256()
		{
			UInt256.MinValue.Should().Be((UInt256)Zero);
			UInt256.MaxValue.Should().Be((UInt256)UInt256MaxValue);
		}

		[Fact]
		public void Cast_ToDouble()
		{
			// Test a UInt256Converter value where _upper is 0
			UInt value1 = new UInt(UInt256.MaxValue);
			double exp1 = System.Math.Round((double)UInt256.MaxValue, 5);
			double act1 = System.Math.Round((double)value1, 5);
			double diff1 = System.Math.Abs(exp1 * 0.00000000001);
			Assert.True(System.Math.Abs(exp1 - act1) <= diff1);


			// Test a UInt256Converter value where _upper is not 0
			UInt value2 = new UInt(UInt256.MaxValue, UInt256.MaxValue);
			double exp2 = System.Math.Round(((double)UInt256.MaxValue) * System.Math.Pow(2.0, 256), 5);
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
				.Be(new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFE));
		}

		[Fact]
		public void BasicParseTest()
		{
			UInt.Parse("13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084095")
				.Should().Be(MaxValue)
				.And.BeRankedEquallyTo(MaxValue);
		}

		[Fact]
		public void BasicTryParseTest()
		{
			UInt.TryParse("13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084095", out UInt parsedValue).Should().BeTrue();
			parsedValue.Should().Be(MaxValue)
				.And.BeRankedEquallyTo(MaxValue);
		}

		[Fact]
		public void ToDecStringTest()
		{
			MaxValue.ToString("D", CultureInfo.CurrentCulture)
				.Should().Be("13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084095");
		}
		[Fact]
		public void ToHexStringTest()
		{
			MaxValue.ToString("X128", CultureInfo.CurrentCulture)
				.Should().Be("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF");
		}
		[Fact]
		public void ToBinStringTest()
		{
			MaxValue.ToString("B512", CultureInfo.CurrentCulture)
				.Should().Be("11111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111");
		}

		[Fact]
		public void ToDecFormatStringTest()
		{
			MaxValue.ToString().Should().Be($"{MaxValue:D}");
		}
		[Fact]
		public void ToHexFormatStringTest()
		{
			MaxValue.ToString("X128", CultureInfo.CurrentCulture).Should().Be($"{MaxValue:X128}");
		}
		[Fact]
		public void ToBinFormatStringTest()
		{
			MaxValue.ToString("B512", CultureInfo.CurrentCulture).Should().Be($"{MaxValue:B512}");
		}
	}
}
