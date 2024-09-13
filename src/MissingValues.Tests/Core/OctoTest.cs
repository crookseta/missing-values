using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues.Tests.Core
{
	public partial class OctoTest
    {
		public static readonly Octo NegativeThousand = Values.CreateFloat<Octo>(0xC000_8F40_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Octo NegativeHundred = Values.CreateFloat<Octo>(0xC000_5900_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Octo NegativeTen = Values.CreateFloat<Octo>(0xC000_2400_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Octo NegativeFive = Values.CreateFloat<Octo>(0xC000_1400_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Octo NegativeFour = Values.CreateFloat<Octo>(0xC000_1000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Octo NegativeThree = Values.CreateFloat<Octo>(0xC000_0800_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Octo NegativeTwo = Values.CreateFloat<Octo>(0xC000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Octo NegativeOne = Octo.NegativeOne;
		public static readonly Octo NegativeHalf = Values.CreateFloat<Octo>(0xBFFF_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Octo NegativeQuarter = Values.CreateFloat<Octo>(0xBFFF_D000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Octo NegativeZero = Octo.NegativeZero;
		public static readonly Octo Zero = Octo.Zero;
		public static readonly Octo Quarter = Values.CreateFloat<Octo>(0x3FFF_D000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Octo Half = Values.CreateFloat<Octo>(0x3FFF_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Octo One = Octo.One;
		public static readonly Octo Two = Values.CreateFloat<Octo>(0x4000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Octo Three = Values.CreateFloat<Octo>(0x4000_0800_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Octo Four = Values.CreateFloat<Octo>(0x4000_1000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Octo Five = Values.CreateFloat<Octo>(0x4000_1400_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Octo Ten = Values.CreateFloat<Octo>(0x4000_2400_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Octo Hundred = Values.CreateFloat<Octo>(0x4000_5900_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Octo Thousand = Values.CreateFloat<Octo>(0x4000_8F40_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

		public static readonly Octo SmallestSubnormal = Values.CreateFloat<Octo>(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0001);
		public static readonly Octo GreatestSubnormal = Values.CreateFloat<Octo>(0x0000_0FFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);

		[Fact]
		public void Ctor_Empty()
		{
			var i = new Octo();
			Assert.Equal(Zero, i);
		}

		[Fact]
		public void ToGeneralStringTest()
		{
			Zero.ToString("G70", CultureInfo.InvariantCulture)
				.Should().Be("0");
			One.ToString("G70", CultureInfo.InvariantCulture)
				.Should().Be("1");
			Two.ToString("G70", CultureInfo.InvariantCulture)
				.Should().Be("2");
			Three.ToString("G70", CultureInfo.InvariantCulture)
				.Should().Be("3");
			Five.ToString("G70", CultureInfo.InvariantCulture)
				.Should().Be("5");
			Ten.ToString("G70", CultureInfo.InvariantCulture)
				.Should().Be("10");
			Hundred.ToString("G70", CultureInfo.InvariantCulture)
				.Should().Be("100");
			Thousand.ToString("G70", CultureInfo.InvariantCulture)
				.Should().Be("1000");
			(new Octo(0x400E_ACFA_698C_9539, 0x0BA8_8C1B_7795_0E32, 0xD6C2_C314_97A4_2D00, 0x0000_0000_0000_0000)) // 1E71
				.ToString("G70", CultureInfo.InvariantCulture)
				.Should().Be("1E+71");
		}

		[Theory]
		[MemberData(nameof(TryParseTheoryData))]
		public void BasicTryParseTest(string s, bool success, Octo expected)
		{
			Octo.TryParse(s, out Octo actual).Should().Be(success);
			actual.Should().Be(expected);
		}
	}
}
