using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using Xunit;

using Int = MissingValues.Int512;

namespace MissingValues.Tests.Core
{
	public partial class Int512Test
	{
		private static readonly Int Int512MaxValue = new(
			new UInt256(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF),
			new UInt256(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)
			);
		private static readonly Int Int512MinValue = new(
			new UInt256(0x8000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000),
			new UInt256(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000)
			);

		[Fact]
		public void Ctor_Empty()
		{
			var i = new Int();
			Assert.Equal(0, i);
		}

		[Fact]
		public void Ctor_Value()
		{
			var i = new Int(7);
			Assert.Equal(7, i);
		}

		[Fact]
		public void Cast_ToByte()
		{
			byte.MinValue.Should().Be((byte)Zero);
			byte.MaxValue.Should().Be((byte)ByteMaxValue);
		}
		
		[Fact]
		public void Cast_ToInt16()
		{
			short.MinValue.Should().Be((short)Int16MinValue);
			short.MaxValue.Should().Be((short)Int16MaxValue);
		}
		
		[Fact]
		public void Cast_ToInt32()
		{
			int.MinValue.Should().Be((int)Int32MinValue);
			int.MaxValue.Should().Be((int)Int32MaxValue);
		}
		
		[Fact]
		public void Cast_ToInt64()
		{
			long.MinValue.Should().Be((long)Int64MinValue);
			long.MaxValue.Should().Be((long)Int64MaxValue);
		}
		
		[Fact]
		public void Cast_ToInt128()
		{
			Int128.MinValue.Should().Be((Int128)Int128MinValue);
			Int128.MaxValue.Should().Be((Int128)Int128MaxValue);
		}

		[Fact]
		public void Cast_ToInt256()
		{
			Int256.MinValue.Should().Be((Int256)Int256MinValue);
			Int256.MaxValue.Should().Be((Int256)Int256MaxValue);
		}

		[Fact]
		public void Cast_ToBigInteger()
		{
			BigInteger.One.Should().Be((BigInteger)One);
			BigInteger.Parse("6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042047")
				.Should().Be((BigInteger)MaxValue);
			BigInteger.Parse("-6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042048")
				.Should().Be((BigInteger)MinValue);
		}

		[Fact]
		public void Cast_ToDouble()
		{
			const double max = 6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042047.0;
			const double min = -6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042048.0;

			max.Should().Be((double)Int512MaxValue);
			min.Should().Be((double)Int512MinValue);
		}

		[Fact]
		public void ToDecStringTest()
		{
			MaxValue.ToString("D", CultureInfo.CurrentCulture)
				.Should().Be("6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042047");
			MinValue.ToString("D", CultureInfo.CurrentCulture)
				.Should().Be("-6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042048");
		}
		[Fact]
		public void ToHexStringTest()
		{
			One.ToString("X", CultureInfo.CurrentCulture)
				.Should().Be("1");
			((Int)sbyte.MaxValue).ToString("X", CultureInfo.CurrentCulture)
				.Should().Be("7F");
			((Int)short.MaxValue).ToString("X", CultureInfo.CurrentCulture)
				.Should().Be("7FFF");
			((Int)int.MaxValue).ToString("X", CultureInfo.CurrentCulture)
				.Should().Be("7FFFFFFF");
			((Int)long.MaxValue).ToString("X", CultureInfo.CurrentCulture)
				.Should().Be("7FFFFFFFFFFFFFFF");
			((Int)Int128.MaxValue).ToString("X", CultureInfo.CurrentCulture)
				.Should().Be("7FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF");

			MaxValue.ToString("X128", CultureInfo.CurrentCulture)
				.Should()
				.Be("7FFFFFFFFFFFFFFF" +
							"FFFFFFFFFFFFFFFF" +
							"FFFFFFFFFFFFFFFF" +
							"FFFFFFFFFFFFFFFF" +
							"FFFFFFFFFFFFFFFF" +
							"FFFFFFFFFFFFFFFF" +
							"FFFFFFFFFFFFFFFF" +
							"FFFFFFFFFFFFFFFF");
			MinValue.ToString("X128", CultureInfo.CurrentCulture)
				.Should()
				.Be("8000000000000000" +
							"0000000000000000" +
							"0000000000000000" +
							"0000000000000000" +
							"0000000000000000" +
							"0000000000000000" +
							"0000000000000000" +
							"0000000000000000");
		}
		[Fact]
		public void ToBinStringTest()
		{
			One.ToString("B", CultureInfo.CurrentCulture)
				.Should().Be("1");
			((Int)sbyte.MaxValue).ToString("B", CultureInfo.CurrentCulture)
				.Should().Be("1111111");
			((Int)short.MaxValue).ToString("B", CultureInfo.CurrentCulture)
				.Should().Be("111111111111111");
			((Int)int.MaxValue).ToString("B", CultureInfo.CurrentCulture)
				.Should().Be("1111111111111111111111111111111");
			((Int)long.MaxValue).ToString("B", CultureInfo.CurrentCulture)
				.Should().Be("111111111111111111111111111111111111111111111111111111111111111");
			((Int)Int128.MaxValue).ToString("B", CultureInfo.CurrentCulture)
				.Should().Be("1111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111");

			MaxValue.ToString("B512", CultureInfo.CurrentCulture)
				.Should()
				.Be("011111111111111111111111111111111" +
							"111111111111111111111111111111111" +
							"111111111111111111111111111111111" +
							"111111111111111111111111111111111" +
							"111111111111111111111111111111111" +
							"111111111111111111111111111111111" +
							"111111111111111111111111111111111" +
							"111111111111111111111111111111111" +
							"111111111111111111111111111111111" +
							"111111111111111111111111111111111" +
							"111111111111111111111111111111111" +
							"111111111111111111111111111111111" +
							"111111111111111111111111111111111" +
							"111111111111111111111111111111111" +
							"11111111111111111111111111111111111111111111111111");
			MinValue.ToString("B512", CultureInfo.CurrentCulture)
				.Should()
				.Be("100000000000000000000000000000000" +
							"000000000000000000000000000000000" +
							"000000000000000000000000000000000" +
							"000000000000000000000000000000000" +
							"000000000000000000000000000000000" +
							"000000000000000000000000000000000" +
							"000000000000000000000000000000000" +
							"000000000000000000000000000000000" +
							"000000000000000000000000000000000" +
							"000000000000000000000000000000000" +
							"000000000000000000000000000000000" +
							"000000000000000000000000000000000" +
							"000000000000000000000000000000000" +
							"000000000000000000000000000000000" +
							"00000000000000000000000000000000000000000000000000");
		}

		[Fact]
		public void ToDecFormatStringTest()
		{
			MaxValue.ToString().Should().Be($"{MaxValue:D}");
			MinValue.ToString().Should().Be($"{MinValue:D}");
		}
		[Fact]
		public void ToHexFormatStringTest()
		{
			MaxValue.ToString("X128", CultureInfo.CurrentCulture).Should().Be($"{MaxValue:X128}");
			MinValue.ToString("X128", CultureInfo.CurrentCulture).Should().Be($"{MinValue:X128}");
		}
		[Fact]
		public void ToBinFormatStringTest()
		{
			MaxValue.ToString("B512", CultureInfo.CurrentCulture).Should().Be($"{MaxValue:B512}");
			MinValue.ToString("B512", CultureInfo.CurrentCulture).Should().Be($"{MinValue:B512}");
		}

		[Fact]
		public void ToDecFormatUtf8StringTest()
		{
			ReadOnlySpan<byte> toString = Encoding.UTF8.GetBytes(MaxValue.ToString()!);

			Span<byte> format = stackalloc byte[toString.Length];
			bool success = Utf8.TryWrite(format, CultureInfo.CurrentCulture, $"{MaxValue:D}", out _);
			Assert.Equal(toString, format);
			success.Should().BeTrue();


			toString = Encoding.UTF8.GetBytes(MinValue.ToString()!);

			format = stackalloc byte[toString.Length];
			success = Utf8.TryWrite(format, CultureInfo.CurrentCulture, $"{MinValue:D}", out _);
			Assert.Equal(toString, format);
			success.Should().BeTrue();
		}
		[Fact]
		public void ToHexFormatUtf8StringTest()
		{
			ReadOnlySpan<byte> toString = Encoding.UTF8.GetBytes(MaxValue.ToString("X128", CultureInfo.CurrentCulture)!);

			Span<byte> format = stackalloc byte[toString.Length];
			bool success = Utf8.TryWrite(format, CultureInfo.CurrentCulture, $"{MaxValue:X128}", out _);
			Assert.Equal(toString, format);
			success.Should().BeTrue();


			toString = Encoding.UTF8.GetBytes(MinValue.ToString("X128", CultureInfo.CurrentCulture)!);

			format = stackalloc byte[toString.Length];
			success = Utf8.TryWrite(format, CultureInfo.CurrentCulture, $"{MinValue:X128}", out _);
			Assert.Equal(toString, format);
			success.Should().BeTrue();
		}
		[Fact]
		public void ToBinFormatUtf8StringTest()
		{
			ReadOnlySpan<byte> toString = Encoding.UTF8.GetBytes(MaxValue.ToString("B512", CultureInfo.CurrentCulture)!);

			Span<byte> format = stackalloc byte[toString.Length];
			bool success = Utf8.TryWrite(format, CultureInfo.CurrentCulture, $"{MaxValue:B512}", out _);
			Assert.Equal(toString, format);
			success.Should().BeTrue();


			toString = Encoding.UTF8.GetBytes(MinValue.ToString("B512", CultureInfo.CurrentCulture)!);

			format = stackalloc byte[toString.Length];
			success = Utf8.TryWrite(format, CultureInfo.CurrentCulture, $"{MinValue:B512}", out _);
			Assert.Equal(toString, format);
			success.Should().BeTrue();
		}

		[Fact]
		public void JsonWriteTest()
		{
			JsonSerializer.Serialize(new object[] { MaxValue, MinValue, One, NegativeOne })
				.Should().Be("[6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042047,-6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042048,1,-1]");
		}
		[Fact]
		public void JsonReadTest()
		{
			JsonSerializer.Deserialize<Int>("6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042047")
				.Should().Be(MaxValue);
			JsonSerializer.Deserialize<Int>("6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042047"u8)
				.Should().Be(MaxValue);
			JsonSerializer.Deserialize<Int>("-6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042048")
				.Should().Be(MinValue);
			JsonSerializer.Deserialize<Int>("-6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042048"u8)
				.Should().Be(MinValue);
		}
	}
}
