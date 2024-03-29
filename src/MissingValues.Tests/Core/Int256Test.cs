using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using Xunit;

using Int = MissingValues.Int256;

namespace MissingValues.Tests.Core
{
	public partial class Int256Test
	{
		private static readonly Int Int256MaxValue = new(
			new UInt128(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF),
			new UInt128(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)
			);
		private static readonly Int Int256MinValue = new(
			new UInt128(0x8000_0000_0000_0000, 0x0000_0000_0000_0000),
			new UInt128(0x0000_0000_0000_0000, 0x0000_0000_0000_0000)
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
			var i = new Int(UInt128.Zero, new(0, 7));
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
		public void Cast_ToDouble()
		{
			const double max = 57896044618658097711785492504343953926634992332820282019728792003956564819967.0;
			const double min = -57896044618658097711785492504343953926634992332820282019728792003956564819968.0;

			max.Should().Be((double)Int256MaxValue);
			min.Should().Be((double)Int256MinValue);
		}

		[Fact]
		public void ToDecStringTest()
		{
			Zero.ToString("D", CultureInfo.CurrentCulture)
				.Should().Be("0");
			One.ToString("D", CultureInfo.CurrentCulture)
				.Should().Be("1");
			MaxValue.ToString("D", CultureInfo.CurrentCulture)
				.Should().Be("57896044618658097711785492504343953926634992332820282019728792003956564819967");
			MinValue.ToString("D", CultureInfo.CurrentCulture)
				.Should().Be("-57896044618658097711785492504343953926634992332820282019728792003956564819968");
		}
		[Fact]
		public void ToHexStringTest()
		{
			Zero.ToString("X64", CultureInfo.CurrentCulture)
				.Should().Be("0000000000000000000000000000000000000000000000000000000000000000");
			MaxValue.ToString("x64", CultureInfo.CurrentCulture)
				.Should().Be("7fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff");
			MaxValue.ToString("X64", CultureInfo.CurrentCulture)
				.Should().Be("7FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF");
			MinValue.ToString("X64", CultureInfo.CurrentCulture)
				.Should().Be("8000000000000000000000000000000000000000000000000000000000000000");
		}
		[Fact]
		public void ToBinStringTest()
		{
			Zero.ToString("B256", CultureInfo.CurrentCulture)
				.Should().Be("0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
			MaxValue.ToString("B256", CultureInfo.CurrentCulture)
				.Should().Be("0111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111");
			MinValue.ToString("B256", CultureInfo.CurrentCulture)
				.Should().Be("1000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
		}

		[Fact]
		public void FormatCurrencyTest()
		{

		}
		[Fact]
		public void FormatEngineeringTest()
		{
			MaxValue.ToString("E", CultureInfo.CurrentCulture)
				.Should().Be("5,789604E+76");
			MaxValue.ToString("e25", CultureInfo.CurrentCulture)
				.Should().Be("5,7896044618658097711785493e+76");

			MinValue.ToString("E", CultureInfo.CurrentCulture)
				.Should().Be("-5,789604E+76");
			MinValue.ToString("e25", CultureInfo.CurrentCulture)
				.Should().Be("-5,7896044618658097711785493e+76");
		}
		[Fact]
		public void FormatFixedPointTest()
		{

		}
		[Fact]
		public void FormatGeneralTest()
		{

		}
		[Fact]
		public void FormatNumberTest()
		{

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
			MaxValue.ToString("X64", CultureInfo.CurrentCulture).Should().Be($"{MaxValue:X64}");
			MinValue.ToString("X64", CultureInfo.CurrentCulture).Should().Be($"{MinValue:X64}");
		}
		[Fact]
		public void ToBinFormatStringTest()
		{
			MaxValue.ToString("B256", CultureInfo.CurrentCulture).Should().Be($"{MaxValue:B256}");
			MinValue.ToString("B256", CultureInfo.CurrentCulture).Should().Be($"{MinValue:B256}");
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
			ReadOnlySpan<byte> toString = Encoding.UTF8.GetBytes(MaxValue.ToString("X64", CultureInfo.CurrentCulture)!);

			Span<byte> format = stackalloc byte[toString.Length];
			bool success = Utf8.TryWrite(format, CultureInfo.CurrentCulture, $"{MaxValue:X64}", out _);
			Assert.Equal(toString, format);
			success.Should().BeTrue();


			toString = Encoding.UTF8.GetBytes(MinValue.ToString("X64", CultureInfo.CurrentCulture)!);

			format = stackalloc byte[toString.Length];
			success = Utf8.TryWrite(format, CultureInfo.CurrentCulture, $"{MinValue:X64}", out _);
			Assert.Equal(toString, format);
			success.Should().BeTrue();
		}
		[Fact]
		public void ToBinFormatUtf8StringTest()
		{
			ReadOnlySpan<byte> toString = Encoding.UTF8.GetBytes(MaxValue.ToString("B256", CultureInfo.CurrentCulture)!);

			Span<byte> format = stackalloc byte[toString.Length];
			bool success = Utf8.TryWrite(format, CultureInfo.CurrentCulture, $"{MaxValue:B256}", out _);
			Assert.Equal(toString, format);
			success.Should().BeTrue();


			toString = Encoding.UTF8.GetBytes(MinValue.ToString("B256", CultureInfo.CurrentCulture)!);

			format = stackalloc byte[toString.Length];
			success = Utf8.TryWrite(format, CultureInfo.CurrentCulture, $"{MinValue:B256}", out _);
			Assert.Equal(toString, format);
			success.Should().BeTrue();
		}

		[Fact]
		public void JsonWriteTest()
		{
			JsonSerializer.Serialize(new object[] { MaxValue, MinValue, One, NegativeOne })
				.Should().Be("[57896044618658097711785492504343953926634992332820282019728792003956564819967,-57896044618658097711785492504343953926634992332820282019728792003956564819968,1,-1]");
		}
		[Fact]
		public void JsonReadTest()
		{
			JsonSerializer.Deserialize<Int>("57896044618658097711785492504343953926634992332820282019728792003956564819967")
				.Should().Be(MaxValue);
			JsonSerializer.Deserialize<Int>("57896044618658097711785492504343953926634992332820282019728792003956564819967"u8)
				.Should().Be(MaxValue);
			JsonSerializer.Deserialize<Int>("-57896044618658097711785492504343953926634992332820282019728792003956564819968")
				.Should().Be(MinValue);
			JsonSerializer.Deserialize<Int>("-57896044618658097711785492504343953926634992332820282019728792003956564819968"u8)
				.Should().Be(MinValue);
		}
	}
}
