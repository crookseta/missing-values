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
	public class Int256Benchmarks
	{
		[HideColumns("Job", "Error", "StdDev")]
		[BenchmarkCategory("Int256", "Signed", "Integer")]
		public class MathOperators
		{
			[Params(100, 10_000, 100_000)]
			public int Length;
			private Int256[] a, b, c;

			[GlobalSetup]
			public void Setup()
			{
				a = new Int256[Length];
				b = new Int256[Length];

				for (int i = 0; i < Length; i++)
				{
					a[i] = new((ulong)Random.Shared.NextInt64(), (ulong)Random.Shared.NextInt64(), (ulong)Random.Shared.NextInt64(), (ulong)Random.Shared.NextInt64());
					b[i] = new((ulong)Random.Shared.NextInt64(), (ulong)Random.Shared.NextInt64(), (ulong)Random.Shared.NextInt64(), (ulong)Random.Shared.NextInt64());
				}

				c = new Int256[Length];
			}

			[Benchmark]
			[BenchmarkCategory("Addition")]
			public Int256[] Add_Int256()
			{
				for (int i = 0; i < Length; i++)
				{
					c[i] = a[i] + b[i];
				}
				return c;
			}

			[Benchmark]
			[BenchmarkCategory("Subtraction")]
			public Int256[] Subtract_Int256()
			{
				for (int i = 0; i < Length; i++)
				{
					c[i] = a[i] - b[i];
				}
				return c;
			}

			[Benchmark]
			[BenchmarkCategory("Multiplication")]
			public Int256[] Multiply_Int256()
			{
				for (int i = 0; i < Length; i++)
				{
					c[i] = a[i] * b[i];
				}
				return c;
			}

			[Benchmark]
			[BenchmarkCategory("Division")]
			public Int256[] Divide_Int256()
			{
				for (int i = 0; i < Length; i++)
				{
					c[i] = a[i] / b[i];
				}
				return c;
			}

			[Benchmark]
			[BenchmarkCategory("Modulus")]
			public Int256[] Modulus_Int256()
			{
				for (int i = 0; i < Length; i++)
				{
					c[i] = a[i] % b[i];
				}
				return c;
			}

			[Benchmark]
			[BenchmarkCategory("BitwiseAnd")]
			public Int256[] BitwiseAnd_Int256()
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
			public Int256[] ShiftLeft_Int256(int shiftAmount)
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
			public Int256[] ShiftRight_Int256(int shiftAmount)
			{
				for (int i = 0; i < Length; i++)
				{
					c[i] = a[i] >> shiftAmount;
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
			[BenchmarkCategory("UnsignedShiftRight")]
			public Int256[] ShiftRightUnsigned_Int256(int shiftAmount)
			{
				for (int i = 0; i < Length; i++)
				{
					c[i] = a[i] >>> shiftAmount;
				}
				return c;
			}
		}

		[MemoryDiagnoser]
		[HideColumns("Job", "Error", "StdDev")]
		[BenchmarkCategory("Int256", "Signed", "Integer")]
		public class ParsingAndFormatting
		{
			[Benchmark]
			[BenchmarkCategory("Formatting")]
			[ArgumentsSource(nameof(ValuesToFormat))]
			public string ToString_Int256(Int256 value, string? fmt)
			{
				return value.ToString(fmt, CultureInfo.CurrentCulture);
			}
			[Benchmark]
			[BenchmarkCategory("Parsing")]
			[ArgumentsSource(nameof(ValuesToParse))]
			public Int256 Parse_Int256(string s, NumberStyles style, IFormatProvider provider)
			{
				return Int256.Parse(s, style, provider);
			}

			public IEnumerable<object[]> ValuesToParse()
			{
				yield return ["9223372036854775808", NumberStyles.Integer, CultureInfo.CurrentCulture];
				yield return ["170141183460469231731687303715884105728", NumberStyles.Integer, CultureInfo.CurrentCulture];
				yield return ["57896044618658097711785492504343953926634992332820282019728792003956564819967", NumberStyles.Integer, CultureInfo.CurrentCulture];
				yield return ["-9223372036854775808", NumberStyles.Integer, CultureInfo.CurrentCulture];
				yield return ["-170141183460469231731687303715884105728", NumberStyles.Integer, CultureInfo.CurrentCulture];
				yield return ["-57896044618658097711785492504343953926634992332820282019728792003956564819966", NumberStyles.Integer, CultureInfo.CurrentCulture];
				yield return ["11111111111111111111111111111111111111111111111111111111111111111111111111111", NumberStyles.BinaryNumber, CultureInfo.CurrentCulture];
				yield return ["FEDCBA0987654321", NumberStyles.HexNumber, CultureInfo.CurrentCulture];
			}
			public IEnumerable<object[]> ValuesToFormat()
			{
				yield return [UInt256.MaxValue, "D"];
				yield return [UInt256.MaxValue, "X"];
				yield return [UInt256.MaxValue, "B"];
				yield return [UInt256.MaxValue, "C"];
				yield return [UInt256.MaxValue, "E"];
				yield return [UInt256.MaxValue, "N"];
				yield return [UInt256.MinValue, "D"];
				yield return [UInt256.MinValue, "X"];
				yield return [UInt256.MinValue, "B"];
				yield return [UInt256.MinValue, "C"];
				yield return [UInt256.MinValue, "E"];
				yield return [UInt256.MinValue, "N"];
			}
		}
	}
}
