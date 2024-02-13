using BenchmarkDotNet.Attributes;
using MissingValues.Internals;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues.Benchmarks
{
	[MemoryDiagnoser]
	public class IntegerBenchmarks
	{
		private readonly Random _rng = new Random(7);

		[Params("d", "x", "b", "X", "B")]
		public string Format;
		[ParamsSource(nameof(Numbers))]
		public UInt512 Number;

		[GlobalSetup]
		public void Setup()
		{

		}

		public IEnumerable<UInt512> Numbers()
		{
			yield return new UInt512(0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0xFFFF_FFFF_FFFF_FFFF);
			yield return new UInt512(0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);
			yield return new UInt512(0x0, 0x0, 0x0, 0x0, 0x0, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);
			yield return new UInt512(0x0, 0x0, 0x0, 0x0, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);
			yield return new UInt512(0x0, 0x0, 0x0, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);
			yield return new UInt512(0x0, 0x0, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);
			yield return new UInt512(0x0, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);
			yield return new UInt512(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);
		}

		[Benchmark]
		public string Integers_ToString()
		{
			return NumberFormatter.FormatUnsignedNumber<UInt512, Int512>(in Number, Format, NumberStyles.Integer, CultureInfo.CurrentCulture);
		}
	}
}
