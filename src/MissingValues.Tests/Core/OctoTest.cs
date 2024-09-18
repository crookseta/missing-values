using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
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

		public static readonly Octo GreaterThanOneSmallest = Values.CreateFloat<Octo>(0x3FFF_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0001);
		public static readonly Octo LessThanOneLargest = Values.CreateFloat<Octo>(0x3FFF_EFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);

		public static readonly Octo SmallestSubnormal = Values.CreateFloat<Octo>(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0001);
		public static readonly Octo GreatestSubnormal = Values.CreateFloat<Octo>(0x0000_0FFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);

		public static readonly Octo MaxValue = Octo.MaxValue;
		public static readonly Octo MinValue = Octo.MinValue;

		public static readonly Octo ByteMaxValue = Values.CreateFloat<Octo>(0x4000_6FE0_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Octo UInt16MaxValue = Values.CreateFloat<Octo>(0x4000_EFFF_E000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Octo UInt32MaxValue = Values.CreateFloat<Octo>(0x4001_EFFF_FFFF_E000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Octo UInt64MaxValue = Values.CreateFloat<Octo>(0x4003_EFFF_FFFF_FFFF, 0xFFFF_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Octo UInt128MaxValue = Values.CreateFloat<Octo>(0x4007_EFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_E000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Octo TwoOver255 = Values.CreateFloat<Octo>(0x400F_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Octo TwoOver511 = Values.CreateFloat<Octo>(0x401F_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

		public static readonly Octo SByteMaxValue = Values.CreateFloat<Octo>(0x4000_5FC0_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Octo SByteMinValue = Values.CreateFloat<Octo>(0xC000_6000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Octo Int16MaxValue = Values.CreateFloat<Octo>(0x4000_DFFF_C000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Octo Int16MinValue = Values.CreateFloat<Octo>(0xC000_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Octo Int32MaxValue = Values.CreateFloat<Octo>(0x4001_DFFF_FFFF_C000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Octo Int32MinValue = Values.CreateFloat<Octo>(0xC001_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Octo Int64MaxValue = Values.CreateFloat<Octo>(0x4003_DFFF_FFFF_FFFF, 0xFFFF_C000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Octo Int64MinValue = Values.CreateFloat<Octo>(0xC003_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Octo Int128MaxValue = Values.CreateFloat<Octo>(0x4007_DFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_C000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Octo Int128MinValue = Values.CreateFloat<Octo>(0xC007_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

		public static readonly Octo Delta = Values.CreateFloat<Octo>(0x400E_B000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

		public static readonly int Radix = 2;

		[Fact]
		public void Ctor_Empty()
		{
			var i = new Octo();
			Assert.Equal(Zero, i);
		}

		[Theory]
		[MemberData(nameof(CastFromByteTheoryData))]
		public void Cast_FromByte(byte from, Octo to)
		{
			((Octo)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastFromUInt16TheoryData))]
		public void Cast_FromUInt16(ushort from, Octo to)
		{
			((Octo)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastFromUInt32TheoryData))]
		public void Cast_FromUInt32(uint from, Octo to)
		{
			((Octo)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastFromUInt64TheoryData))]
		public void Cast_FromUInt64(ulong from, Octo to)
		{
			((Octo)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastFromUInt128TheoryData))]
		public void Cast_FromUInt128(UInt128 from, Octo to)
		{
			((Octo)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastFromUInt256TheoryData))]
		public void Cast_FromUInt256(UInt256 from, Octo to)
		{
			((Octo)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastFromUInt512TheoryData))]
		public void Cast_FromUInt512(UInt512 from, Octo to)
		{
			((Octo)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastFromSByteTheoryData))]
		public void Cast_FromSByte(sbyte from, Octo to)
		{
			((Octo)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastFromInt16TheoryData))]
		public void Cast_FromInt16(short from, Octo to)
		{
			((Octo)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastFromInt32TheoryData))]
		public void Cast_FromInt32(int from, Octo to)
		{
			((Octo)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastFromInt64TheoryData))]
		public void Cast_FromInt64(long from, Octo to)
		{
			((Octo)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastFromInt128TheoryData))]
		public void Cast_FromInt128(Int128 from, Octo to)
		{
			((Octo)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastFromInt256TheoryData))]
		public void Cast_FromInt256(Int256 from, Octo to)
		{
			((Octo)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastFromInt512TheoryData))]
		public void Cast_FromInt512(Int512 from, Octo to)
		{
			((Octo)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastFromHalfTheoryData))]
		public void Cast_FromHalf(Half from, Octo to)
		{
			((Octo)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastFromSingleTheoryData))]
		public void Cast_FromSingle(float from, Octo to)
		{
			((Octo)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastFromDoubleTheoryData))]
		public void Cast_FromDouble(double from, Octo to)
		{
			((Octo)from).Should().Be(to);
		}

		[Theory]
		[MemberData(nameof(CastToByteTheoryData))]
		public void Cast_ToByte(Octo from, byte to)
		{
			((byte)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastToUInt16TheoryData))]
		public void Cast_ToUInt16(Octo from, ushort to)
		{
			((ushort)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastToUInt32TheoryData))]
		public void Cast_ToUInt32(Octo from, uint to)
		{
			((uint)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastToUInt64TheoryData))]
		public void Cast_ToUInt64(Octo from, ulong to)
		{
			((ulong)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastToUInt128TheoryData))]
		public void Cast_ToUInt128(Octo from, UInt128 to)
		{
			((UInt128)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastToUInt256TheoryData))]
		public void Cast_ToUInt256(Octo from, UInt256 to)
		{
			((UInt256)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastToUInt512TheoryData))]
		public void Cast_ToUInt512(Octo from, UInt512 to)
		{
			((UInt512)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastToSByteTheoryData))]
		public void Cast_ToSByte(Octo from, sbyte to)
		{
			((sbyte)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastToInt16TheoryData))]
		public void Cast_ToInt16(Octo from, short to)
		{
			((short)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastToInt32TheoryData))]
		public void Cast_ToInt32(Octo from, int to)
		{
			((int)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastToInt64TheoryData))]
		public void Cast_ToInt64(Octo from, long to)
		{
			((long)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastToInt128TheoryData))]
		public void Cast_ToInt128(Octo from, Int128 to)
		{
			((Int128)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastToInt256TheoryData))]
		public void Cast_ToInt256(Octo from, Int256 to)
		{
			((Int256)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastToInt512TheoryData))]
		public void Cast_ToInt512(Octo from, Int512 to)
		{
			((Int512)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastToHalfTheoryData))]
		public void Cast_ToHalf(Octo from, Half to)
		{
			((Half)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastToSingleTheoryData))]
		public void Cast_ToSingle(Octo from, float to)
		{
			((float)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastToDoubleTheoryData))]
		public void Cast_ToDouble(Octo from, double to)
		{
			((double)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastToQuadTheoryData))]
		public void Cast_ToQuad(Octo from, Quad to)
		{
			((Quad)from).Should().Be(to);
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


		[Fact]
		public void JsonWriteTest()
		{
			JsonSerializer.Serialize(new Octo[] { MaxValue, One, Half, Quarter, Zero })
				.Should().Be($"[{MaxValue.ToString(null, CultureInfo.InvariantCulture)},1,0.5,0.25,0]");
		}
		[Fact]
		public void JsonReadTest()
		{
			string toString = Quarter.ToString(null, CultureInfo.InvariantCulture);

			JsonSerializer.Deserialize<Octo>(toString)
				.Should().Be(Quarter);
			JsonSerializer.Deserialize<Octo>(Encoding.UTF8.GetBytes(toString))
				.Should().Be(Quarter);
		}
	}
}
