using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues.Benchmarks
{
	public class UInt256Benchmarks
	{
		static readonly Random rnd = new Random(7);

		[MemoryDiagnoser]
		public class MathOperators
		{
			private UInt256 a, b;

			[GlobalSetup]
			public void Setup()
			{
				a = new UInt256(new(0, (ulong)rnd.NextInt64()), new((ulong)rnd.NextInt64(), (ulong)rnd.NextInt64()));
				b = new UInt256(new(0, (ulong)rnd.NextInt64()), new((ulong)rnd.NextInt64(), (ulong)rnd.NextInt64()));
			}

			[Benchmark]
			public UInt256 Add_UInt256()
			{
				return a + b;
			}

			[Benchmark]
			public UInt256 Subtract_UInt256()
			{
				return a - b;
			}

			[Benchmark]
			public UInt256 Multiply_UInt256()
			{
				return a * b;
			}

			[Benchmark]
			public UInt256 Divide_UInt256()
			{
				return a / b;
			}

			[Benchmark]
			public UInt256 Modulus_UInt256()
			{
				return a % b;
			}
		}

		[MemoryDiagnoser]
		public class Formatting
		{
			private UInt256 value;

			[Params("57896044618658097711785492504343953926634992332820282019728792003956564819967", "1", "115792089237316195423570985008687907853269984665640564039457584007913129639935")]
			public string Value { get; set; }
			[Params(null, "X64", "B256")]
			public string Format { get; set; }

			[GlobalSetup]
			public void Setup()
			{
				value = UInt256.Parse(Value);
			}

			[Benchmark]
			public string ToString_UInt256()
			{
				return value.ToString(Format, CultureInfo.CurrentCulture);
			}
		}
		
		[MemoryDiagnoser]
		public class Parsing
		{
			private string _bin, _hex;

			[Params("57896044618658097711785492504343953926634992332820282019728792003956564819967", "1", "115792089237316195423570985008687907853269984665640564039457584007913129639935")]
			public string Value { get; set; }

			[GlobalSetup]
			public void Setup()
			{
				_bin = UInt256.Parse(Value).ToString("B256", CultureInfo.CurrentCulture);
				_hex = UInt256.Parse(Value).ToString("X64", CultureInfo.CurrentCulture);
			}

			[Benchmark(Baseline = true)]
			public UInt256 Parse_DecimalUInt256()
			{
				return UInt256.Parse(Value);
			}
			[Benchmark]
			public UInt256 Parse_HexadecimalUInt256()
			{
				return UInt256.Parse(_hex, NumberStyles.HexNumber, CultureInfo.CurrentCulture);
			}
		}

		[MemoryDiagnoser]
		public class Operations
		{
			private UInt256 a, b;

			[GlobalSetup]
			public void Setup()
			{
				a = new UInt256(new(0, (ulong)rnd.NextInt64()), new((ulong)rnd.NextInt64(), (ulong)rnd.NextInt64()));
				b = new UInt256(new(0, (ulong)rnd.NextInt64()), new((ulong)rnd.NextInt64(), (ulong)rnd.NextInt64()));
			}

			[Benchmark]
			public UInt256 Clamp_UInt256()
			{
				return UInt256.Clamp(a, UInt256.Zero, b);
			}

			[Benchmark]
			public (UInt256, UInt256) DivRem_UInt256()
			{
				return UInt256.DivRem(a, b);
			}
		}
	}
}
