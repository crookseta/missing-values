using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues.Benchmarks.Core
{
	public class UInt256Benchmarks
	{
		[HideColumns("Job", "Error", "StdDev")]
		[BenchmarkCategory("UInt256", "Unsigned", "Integer")]
		public class MathOperators
		{
			[Params(100, 10_000, 100_000)]
			public int Length;
			private UInt256[] a, b, c;

			[GlobalSetup]
			public void Setup()
			{
				a = new UInt256[Length];
				b = new UInt256[Length];

				for (int i = 0; i < Length; i++)
				{
					a[i] = new((ulong)Random.Shared.NextInt64(), (ulong)Random.Shared.NextInt64(), (ulong)Random.Shared.NextInt64(), (ulong)Random.Shared.NextInt64());
					b[i] = new((ulong)Random.Shared.NextInt64(), (ulong)Random.Shared.NextInt64(), (ulong)Random.Shared.NextInt64(), (ulong)Random.Shared.NextInt64());
				}

				c = new UInt256[Length];
			}

			[Benchmark]
			[BenchmarkCategory("Addition")]
			public UInt256[] Add_UInt256()
			{
				for (int i = 0; i < Length; i++)
				{
					c[i] = a[i] + b[i];
				}
				return c;
			}

			[Benchmark]
			[BenchmarkCategory("Subtraction")]
			public UInt256[] Subtract_UInt256()
			{
				for (int i = 0; i < Length; i++)
				{
					c[i] = a[i] - b[i];
				}
				return c;
			}

			[Benchmark]
			[BenchmarkCategory("Multiplication")]
			public UInt256[] Multiply_UInt256()
			{
				for (int i = 0; i < Length; i++)
				{
					c[i] = a[i] * b[i];
				}
				return c;
			}

			[Benchmark]
			[BenchmarkCategory("Division")]
			public UInt256[] Divide_UInt256()
			{
				for (int i = 0; i < Length; i++)
				{
					c[i] = a[i] / b[i];
				}
				return c;
			}

			[Benchmark]
			[BenchmarkCategory("Modulus")]
			public UInt256[] Modulus_UInt256()
			{
				for (int i = 0; i < Length; i++)
				{
					c[i] = a[i] % b[i];
				}
				return c;
			}

			[Benchmark]
			[BenchmarkCategory("BitwiseAnd")]
			public UInt256[] BitwiseAnd_UInt256()
			{
				for (int i = 0; i < Length; i++)
				{
					c[i] = a[i] & b[i];
				}
				return c;
			}

			[Benchmark]
			[Arguments(16)]
			[Arguments(32)]
			[Arguments(64)]
			[Arguments(100)]
			[Arguments(128)]
			[Arguments(192)]
			[Arguments(200)]
			[BenchmarkCategory("ShiftLeft")]
			public UInt256[] ShiftLeft_UInt256(int shiftAmount)
			{
				for (int i = 0; i < Length; i++)
				{
					c[i] = a[i] << shiftAmount;
				}
				return c;
			}

			[Benchmark]
			[Arguments(16)]
			[Arguments(32)]
			[Arguments(64)]
			[Arguments(100)]
			[Arguments(128)]
			[Arguments(192)]
			[Arguments(200)]
			[BenchmarkCategory("ShiftRight")]
			public UInt256[] ShiftRight_UInt256(int shiftAmount)
			{
				for (int i = 0; i < Length; i++)
				{
					c[i] = a[i] >> shiftAmount;
				}
				return c;
			}
		}

		[MemoryDiagnoser]
		[HideColumns("Job", "Error", "StdDev")]
		[BenchmarkCategory("UInt256", "Unsigned", "Integer")]
		public class ParsingAndFormatting
		{
			[Benchmark]
			[BenchmarkCategory("Formatting")]
			[ArgumentsSource(nameof(ValuesToFormat))]
			public string ToString_UInt256(UInt256 value, string? fmt)
			{
				return value.ToString(fmt, CultureInfo.CurrentCulture);
			}
			[Benchmark]
			[BenchmarkCategory("Parsing")]
			[ArgumentsSource(nameof(ValuesToParse))]
			public UInt256 Parse_UInt256(string s, NumberStyles style, IFormatProvider provider)
			{
				return UInt256.Parse(s, style, provider);
			}

			public IEnumerable<object[]> ValuesToParse()
			{
				yield return ["9223372036854775808", NumberStyles.Integer, CultureInfo.CurrentCulture];
				yield return ["170141183460469231731687303715884105728", NumberStyles.Integer, CultureInfo.CurrentCulture];
				yield return ["57896044618658097711785492504343953926634992332820282019728792003956564819967", NumberStyles.Integer, CultureInfo.CurrentCulture];
				yield return ["11111111111111111111111111111111111111111111111111111111111111111111111111111", NumberStyles.BinaryNumber, CultureInfo.CurrentCulture];
				yield return ["FEDCBA0987654321", NumberStyles.HexNumber, CultureInfo.CurrentCulture];
			}
			public IEnumerable<object[]> ValuesToFormat()
			{
				yield return [UInt256.MaxValue, "D"];
				yield return [UInt256.MaxValue, "X64"];
				yield return [UInt256.MaxValue, "B256"];
				yield return [UInt256.MaxValue, "C"];
				yield return [UInt256.MaxValue, "E"];
				yield return [UInt256.MaxValue, "N"];
			}
		}
	}
}
