using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues.Tests.Core
{
	public partial class QuadTest
	{
		public static readonly Quad NegativeThousand = Values.CreateQuad(0xC008_F400_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Quad NegativeHundred = Values.CreateQuad(0xC005_9000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Quad NegativeTen = Values.CreateQuad(0xC002_4000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Quad NegativeSix = Values.CreateQuad(0xC001_8000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Quad NegativeFive = Values.CreateQuad(0xC001_4000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Quad NegativeFour = Values.CreateQuad(0xC001_0000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Quad NegativeThree = Values.CreateQuad(0xC000_8000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Quad NegativeTwo = Values.CreateQuad(0xC000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Quad NegativeOne = Quad.NegativeOne;
		public static readonly Quad NegativeHalf = Values.CreateQuad(0xBFFE_0000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Quad NegativeQuarter = Values.CreateQuad(0xBFFD_0000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Quad NegativeZero = Quad.NegativeZero;
		public static readonly Quad Zero = Quad.Zero;
		public static readonly Quad Quarter = Values.CreateQuad(0x3FFD_0000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Quad Half = Values.CreateQuad(0x3FFE_0000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Quad One = Quad.One;
		public static readonly Quad Two = Values.CreateQuad(0x4000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Quad Three = Values.CreateQuad(0x4000_8000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Quad Four = Values.CreateQuad(0x4001_0000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Quad Five = Values.CreateQuad(0x4001_4000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Quad Six = Values.CreateQuad(0x4001_8000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Quad Ten = Values.CreateQuad(0x4002_4000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Quad Hundred = Values.CreateQuad(0x4005_9000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Quad Thousand = Values.CreateQuad(0x4008_F400_0000_0000, 0x0000_0000_0000_0000);

		public static readonly Quad GreaterThanOneSmallest = Values.CreateQuad(0x3FFF_0000_0000_0000, 0x0000_0000_0000_0001);
		public static readonly Quad LessThanOneLargest = Values.CreateQuad(0x3FFE_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);

		public static readonly Quad SmallestSubnormal = Values.CreateQuad(0x0000_0000_0000_0000, 0x0000_0000_0000_0001);
		public static readonly Quad GreatestSubnormal = Values.CreateQuad(0x0000_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);

		public static readonly Quad MaxValue = Quad.MaxValue;
		public static readonly Quad MinValue = Quad.MinValue;

		public static readonly Quad ByteMaxValue = Values.CreateQuad(0x4006_FE00_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Quad UInt16MaxValue = Values.CreateQuad(0x400E_FFFE_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Quad UInt32MaxValue = Values.CreateQuad(0x401E_FFFF_FFFE_0000, 0x0000_0000_0000_0000);
		public static readonly Quad UInt64MaxValue = Values.CreateQuad(0x403E_FFFF_FFFF_FFFF, 0xFFFE_0000_0000_0000);
		public static readonly Quad UInt128MaxValue = Values.CreateQuad(0x407F_0000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Quad UInt256MaxValue = Values.CreateQuad(0x40FF_0000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Quad UInt512MaxValue = Values.CreateQuad(0x41FF_0000_0000_0000, 0x0000_0000_0000_0000);

		public static readonly Quad SByteMaxValue = Values.CreateQuad(0x4005_FC00_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Quad SByteMinValue = Values.CreateQuad(0xC006_0000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Quad Int16MaxValue = Values.CreateQuad(0x400D_FFFC_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Quad Int16MinValue = Values.CreateQuad(0xC00E_0000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Quad Int32MaxValue = Values.CreateQuad(0x401D_FFFF_FFFC_0000, 0x0000_0000_0000_0000);
		public static readonly Quad Int32MinValue = Values.CreateQuad(0xC01E_0000_0000_0000, 0x0000_0000_0000_0000);
		public static readonly Quad Int64MaxValue = Values.CreateQuad(0x403D_FFFF_FFFF_FFFF, 0xFFFC_0000_0000_0000);
		public static readonly Quad Int64MinValue = Values.CreateQuad(0xC03E_0000_0000_0000, 0x0000_0000_0000_0000);

		public static readonly Quad Delta = Values.CreateQuad(0x4033_0000_0000_0000, 0x0000_0000_0000_0000);

		public static readonly int Radix = 2;


		[Fact]
		public void Ctor_Empty()
		{
			var i = new Quad();
			Assert.Equal(Zero, i);
		}

		[Theory]
		[MemberData(nameof(CastFromByteTheoryData))]
		public void Cast_FromByte(byte from, Quad to)
		{
			((Quad)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastFromUInt16TheoryData))]
		public void Cast_FromUInt16(ushort from, Quad to)
		{
			((Quad)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastFromUInt32TheoryData))]
		public void Cast_FromUInt32(uint from, Quad to)
		{
			((Quad)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastFromUInt64TheoryData))]
		public void Cast_FromUInt64(ulong from, Quad to)
		{
			((Quad)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastFromUInt128TheoryData))]
		public void Cast_FromUInt128(UInt128 from, Quad to)
		{
			((Quad)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastFromUInt256TheoryData))]
		public void Cast_FromUInt256(UInt256 from, Quad to)
		{
			((Quad)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastFromUInt512TheoryData))]
		public void Cast_FromUInt512(UInt512 from, Quad to)
		{
			((Quad)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastFromSByteTheoryData))]
		public void Cast_FromSByte(sbyte from, Quad to)
		{
			((Quad)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastFromInt16TheoryData))]
		public void Cast_FromInt16(short from, Quad to)
		{
			((Quad)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastFromInt32TheoryData))]
		public void Cast_FromInt32(int from, Quad to)
		{
			((Quad)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastFromInt64TheoryData))]
		public void Cast_FromInt64(long from, Quad to)
		{
			((Quad)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastFromInt128TheoryData))]
		public void Cast_FromInt128(Int128 from, Quad to)
		{
			((Quad)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastFromInt256TheoryData))]
		public void Cast_FromInt256(Int256 from, Quad to)
		{
			((Quad)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastFromInt512TheoryData))]
		public void Cast_FromInt512(Int512 from, Quad to)
		{
			((Quad)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastFromHalfTheoryData))]
		public void Cast_FromHalf(Half from, Quad to)
		{
			((Quad)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastFromSingleTheoryData))]
		public void Cast_FromSingle(float from, Quad to)
		{
			((Quad)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastFromDoubleTheoryData))]
		public void Cast_FromDouble(double from, Quad to)
		{
			((Quad)from).Should().Be(to);
		}

		[Theory]
		[MemberData(nameof(CastToByteTheoryData))]
		public void Cast_ToByte(Quad from, byte to)
		{
			((byte)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastToUInt16TheoryData))]
		public void Cast_ToUInt16(Quad from, ushort to)
		{
			((ushort)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastToUInt32TheoryData))]
		public void Cast_ToUInt32(Quad from, uint to)
		{
			((uint)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastToUInt64TheoryData))]
		public void Cast_ToUInt64(Quad from, ulong to)
		{
			((ulong)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastToUInt128TheoryData))]
		public void Cast_ToUInt128(Quad from, UInt128 to)
		{
			((UInt128)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastToUInt256TheoryData))]
		public void Cast_ToUInt256(Quad from, UInt256 to)
		{
			((UInt256)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastToUInt512TheoryData))]
		public void Cast_ToUInt512(Quad from, UInt512 to)
		{
			((UInt512)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastToSByteTheoryData))]
		public void Cast_ToSByte(Quad from, sbyte to)
		{
			((sbyte)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastToInt16TheoryData))]
		public void Cast_ToInt16(Quad from, short to)
		{
			((short)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastToInt32TheoryData))]
		public void Cast_ToInt32(Quad from, int to)
		{
			((int)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastToInt64TheoryData))]
		public void Cast_ToInt64(Quad from, long to)
		{
			((long)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastToInt128TheoryData))]
		public void Cast_ToInt128(Quad from, Int128 to)
		{
			((Int128)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastToInt256TheoryData))]
		public void Cast_ToInt256(Quad from, Int256 to)
		{
			((Int256)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastToInt512TheoryData))]
		public void Cast_ToInt512(Quad from, Int512 to)
		{
			((Int512)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastToHalfTheoryData))]
		public void Cast_ToHalf(Quad from, Half to)
		{
			((Half)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastToSingleTheoryData))]
		public void Cast_ToSingle(Quad from, float to)
		{
			((float)from).Should().Be(to);
		}
		[Theory]
		[MemberData(nameof(CastToDoubleTheoryData))]
		public void Cast_ToDouble(Quad from, double to)
		{
			((double)from).Should().Be(to);
		}

		[Fact]
		public void ToGeneralStringTest()
		{
			NegativeQuarter.ToString("G33", CultureInfo.InvariantCulture)
				.Should().Be("-0.25");
			Half.ToString("G33", CultureInfo.InvariantCulture)
				.Should().Be("0.5");
			NegativeHalf.ToString("G33", CultureInfo.InvariantCulture)
				.Should().Be("-0.5");
			One.ToString("G33", CultureInfo.InvariantCulture)
				.Should().Be("1");
			Thousand.ToString("G33", CultureInfo.InvariantCulture)
				.Should().Be("1000");
			NegativeThousand.ToString("G33", CultureInfo.InvariantCulture)
				.Should().Be("-1000");
		}

		[Fact]
		public void ToScientificStringTest()
		{
			NegativeQuarter.ToString("E33", CultureInfo.InvariantCulture)
				.Should().Be("-2.5E-1");
			Half.ToString("E33", CultureInfo.InvariantCulture)
				.Should().Be("5E-1");
			NegativeHalf.ToString("E33", CultureInfo.InvariantCulture)
				.Should().Be("-5E-1");
			One.ToString("E33", CultureInfo.InvariantCulture)
				.Should().Be("1E+0");
			Thousand.ToString("E33", CultureInfo.InvariantCulture)
				.Should().Be("1E+3");
			NegativeThousand.ToString("E33", CultureInfo.InvariantCulture)
				.Should().Be("-1E+3");
		}

		[Fact]
		public void ToGeneralFormatStringTest()
		{
			$"{NegativeQuarter:G33}"
				.Should().Be("-0,25");
			$"{Half:G33}"
				.Should().Be("0,5");
			$"{NegativeHalf:G33}"
				.Should().Be("-0,5");
			$"{One:G33}"
				.Should().Be("1");
			$"{Thousand:G33}"
				.Should().Be("1000");
			$"{NegativeThousand:G33}"
				.Should().Be("-1000");
		}

		[Fact]
		public void ToScientificFormatStringTest()
		{
			$"{NegativeQuarter:E33}"
				.Should().Be("-2,5E-1");
			$"{Half:E33}"
				.Should().Be("5E-1");
			$"{NegativeHalf:E33}"
				.Should().Be("-5E-1");
			$"{One:E33}"
				.Should().Be("1E+0");
			$"{Thousand:E33}"
				.Should().Be("1E+3");
			$"{NegativeThousand:E33}"
				.Should().Be("-1E+3");
		}

		

		[Theory]
		[MemberData(nameof(TryParseTheoryData))]
		public void TryParseTest(string s, bool success, Quad returnValue)
		{
			Quad result;

			Quad.TryParse(s, out result).Should().Be(success);
			result.Should().Be(returnValue);
		}
	}
}
