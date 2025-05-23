using MissingValues.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues.Tests.Core
{
	public class NumberFormatTest
	{
		private static readonly Quad _decimalSampleValue = Values.CreateFloat<Quad>(0x400C_81CD_6E63_1F8A, 0x0902_DE00_D1B7_1759); // 12345.6789
		private static readonly NumberFormatInfo CustomInfo = new()
		{
			PositiveSign = "+",
			NegativeSign = "-",
			CurrencyPositivePattern = 0,
			CurrencyNegativePattern = 2,
			CurrencySymbol = "$",
			CurrencyDecimalDigits = 2,
			CurrencyDecimalSeparator = ".",
			CurrencyGroupSeparator = ",",
			CurrencyGroupSizes = [3],
			NumberGroupSeparator = "_",
			NumberGroupSizes = [2],
			NumberDecimalDigits = 5,
			NumberDecimalSeparator = ".",
			NumberNegativePattern = 1,
			PositiveInfinitySymbol = "+Inf",
			NegativeInfinitySymbol = "-Inf",
			NaNSymbol = "NaN",
		};

		private static readonly (IFormattable, string, NumberFormatInfo?, string)[] _formats =
		[
			(Int256.MaxValue, "E", NumberFormatInfo.CurrentInfo, "5,789604E+76"),
			(Int256.MaxValue, "e25", NumberFormatInfo.CurrentInfo, "5,7896044618658097711785493e+76"),
			(Int256.MinValue, "E", NumberFormatInfo.CurrentInfo, "-5,789604E+76"),
			(Int256.MinValue, "e25", NumberFormatInfo.CurrentInfo, "-5,7896044618658097711785493e+76"),
			(Int512.MaxValue, "E", NumberFormatInfo.CurrentInfo, "6,703904E+153"),
			(Int512.MaxValue, "e25", NumberFormatInfo.CurrentInfo, "6,7039039649712985497870125e+153"),
			(Int512.MinValue, "E", NumberFormatInfo.CurrentInfo, "-6,703904E+153"),
			(Int512.MinValue, "e25", NumberFormatInfo.CurrentInfo, "-6,7039039649712985497870125e+153"),
			(Int512.MinValue, "F3", NumberFormatInfo.InvariantInfo, "-6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042048.000"),
			(_decimalSampleValue, "F", NumberFormatInfo.InvariantInfo, "12345.68"),
			(_decimalSampleValue, "F", CustomInfo, "12345.67890"),
			(Int512.MinValue, "N", NumberFormatInfo.InvariantInfo, "-6,703,903,964,971,298,549,787,012,499,102,923,063,739,682,910,296,196,688,861,780,721,860,882,015,036,773,488,400,937,149,083,451,713,845,015,929,093,243,025,426,876,941,405,973,284,973,216,824,503,042,048.00"),
			(_decimalSampleValue, "N3", NumberFormatInfo.InvariantInfo, "12,345.679"),
			(_decimalSampleValue, "N", CustomInfo, "1_23_45.67890"),
			(Int512.MinValue, "C", CustomInfo, "$-6,703,903,964,971,298,549,787,012,499,102,923,063,739,682,910,296,196,688,861,780,721,860,882,015,036,773,488,400,937,149,083,451,713,845,015,929,093,243,025,426,876,941,405,973,284,973,216,824,503,042,048.00"),
			(_decimalSampleValue, "C", CustomInfo, "$12,345.68"),
		];
		private static readonly (string, NumberStyles, NumberFormatInfo?, Int512, bool)[] _parseInt512 =
		[
			("1E200", NumberStyles.Number | NumberStyles.AllowExponent, NumberFormatInfo.CurrentInfo, default, false),
			("2,5E10", NumberStyles.Number | NumberStyles.AllowExponent, NumberFormatInfo.CurrentInfo, 25_000_000_000, true),
			("1E10", NumberStyles.Number | NumberStyles.AllowExponent, NumberFormatInfo.CurrentInfo, 10_000_000_000, true),
			("1,000", NumberStyles.Number, NumberFormatInfo.CurrentInfo, Int512.One, true),
			("1.000,0", NumberStyles.Number, NumberFormatInfo.CurrentInfo, 1_000, true),
			("1.000.000", NumberStyles.Number, NumberFormatInfo.CurrentInfo, 1_000_000, true),
			("1.000.000.000,00", NumberStyles.Number, NumberFormatInfo.CurrentInfo, 1_000_000_000, true),
			("-6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042048.000", NumberStyles.Number, NumberFormatInfo.InvariantInfo, Int512.MinValue, true),
			("-6,703,903,964,971,298,549,787,012,499,102,923,063,739,682,910,296,196,688,861,780,721,860,882,015,036,773,488,400,937,149,083,451,713,845,015,929,093,243,025,426,876,941,405,973,284,973,216,824,503,042,048", NumberStyles.Number, NumberFormatInfo.InvariantInfo, Int512.MinValue, true),
			("$-6,703,903,964,971,298,549,787,012,499,102,923,063,739,682,910,296,196,688,861,780,721,860,882,015,036,773,488,400,937,149,083,451,713,845,015,929,093,243,025,426,876,941,405,973,284,973,216,824,503,042,048.00", NumberStyles.Currency, CustomInfo, Int512.MinValue, true),
		];
		private static readonly (string, NumberStyles, NumberFormatInfo?, Quad, bool)[] _parseQuad =
		[
			("2,5E-1", NumberStyles.Float, NumberFormatInfo.CurrentInfo, QuadTest.Quarter, true),
			("0,250", NumberStyles.Float, NumberFormatInfo.CurrentInfo, QuadTest.Quarter, true),
			("$-0.25", NumberStyles.Currency, CustomInfo, QuadTest.NegativeQuarter, true),
			("1,000", NumberStyles.Float, NumberFormatInfo.CurrentInfo, Quad.One, true),
			("1.000,0", NumberStyles.Float | NumberStyles.AllowThousands, NumberFormatInfo.CurrentInfo, QuadTest.Thousand, true),
			("-1.000,0", NumberStyles.Float | NumberStyles.AllowThousands, NumberFormatInfo.CurrentInfo, QuadTest.NegativeThousand, true),
		];

		public static readonly FormatStringTheoryData FormatsTheoryData = new(_formats);

		public static readonly FormatParsingTheoryData<Int512> ParseInt512TheoryData = new(_parseInt512);
		public static readonly FormatParsingTheoryData<Quad> ParseQuadTheoryData = new(_parseQuad);

		[Theory]
		[MemberData(nameof(FormatsTheoryData))]
		public void FormattingTest(IFormattable value, string fmt, NumberFormatInfo? info, string expected)
		{
			string actual = value.ToString(fmt, info);
			actual.Should().Be(expected);
		}
		[Theory]
		[MemberData(nameof(ParseInt512TheoryData))]
		public void IntegerParsingTest(string s, NumberStyles style, NumberFormatInfo? info, Int512 expected, bool success)
		{
			Int512.TryParse(s, style, info, out Int512 actual).Should().Be(success);
			actual.Should().Be(expected);
		}
		[Theory]
		[MemberData(nameof(ParseQuadTheoryData))]
		public void FloatingPointParsingTest(string s, NumberStyles style, NumberFormatInfo? info, Quad expected, bool success)
		{
			Quad.TryParse(s, style, info, out Quad actual).Should().Be(success);
			actual.Should().Be(expected);
		}
	}
}
