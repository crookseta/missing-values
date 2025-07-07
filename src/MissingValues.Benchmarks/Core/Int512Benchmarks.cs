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
	public class Int512Benchmarks
	{
		[HideColumns("Job", "Error", "StdDev")]
		[BenchmarkCategory("Int512", "Signed", "Integer")]
		public class MathOperators
		{
			[Params(100, 10_000, 100_000)]
			public int Length;
			private Int512[] a, b, c;

			[GlobalSetup]
			public void Setup()
			{
				a = new Int512[Length];
				b = new Int512[Length];

				for (int i = 0; i < Length; i++)
				{
					a[i] = new(
						(ulong)Random.Shared.NextInt64(), (ulong)Random.Shared.NextInt64(), (ulong)Random.Shared.NextInt64(), (ulong)Random.Shared.NextInt64(),
						(ulong)Random.Shared.NextInt64(), (ulong)Random.Shared.NextInt64(), (ulong)Random.Shared.NextInt64(), (ulong)Random.Shared.NextInt64());
					b[i] = new(
						(ulong)Random.Shared.NextInt64(), (ulong)Random.Shared.NextInt64(), (ulong)Random.Shared.NextInt64(), (ulong)Random.Shared.NextInt64(),
						(ulong)Random.Shared.NextInt64(), (ulong)Random.Shared.NextInt64(), (ulong)Random.Shared.NextInt64(), (ulong)Random.Shared.NextInt64());
				}

				c = new Int512[Length];
			}

			[Benchmark]
			[BenchmarkCategory("Addition")]
			public Int512[] Add_Int512()
			{
				for (int i = 0; i < Length; i++)
				{
					c[i] = a[i] + b[i];
				}
				return c;
			}

			[Benchmark]
			[BenchmarkCategory("Subtraction")]
			public Int512[] Subtract_Int512()
			{
				for (int i = 0; i < Length; i++)
				{
					c[i] = a[i] - b[i];
				}
				return c;
			}

			[Benchmark]
			[BenchmarkCategory("Multiplication")]
			public Int512[] Multiply_Int512()
			{
				for (int i = 0; i < Length; i++)
				{
					c[i] = a[i] * b[i];
				}
				return c;
			}

			[Benchmark]
			[BenchmarkCategory("Division")]
			public Int512[] Divide_Int512()
			{
				for (int i = 0; i < Length; i++)
				{
					c[i] = a[i] / b[i];
				}
				return c;
			}

			[Benchmark]
			[BenchmarkCategory("Modulus")]
			public Int512[] Modulus_Int512()
			{
				for (int i = 0; i < Length; i++)
				{
					c[i] = a[i] % b[i];
				}
				return c;
			}

			[Benchmark]
			[BenchmarkCategory("BitwiseAnd")]
			public Int512[] BitwiseAnd_Int512()
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
			[Arguments(256)]
			[Arguments(320)]
			[Arguments(384)]
			[Arguments(448)]
			[Arguments(450)]
			[BenchmarkCategory("ShiftLeft")]
			public Int512[] ShiftLeft_Int512(int shiftAmount)
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
			[Arguments(256)]
			[Arguments(320)]
			[Arguments(384)]
			[Arguments(448)]
			[Arguments(450)]
			[BenchmarkCategory("ShiftRight")]
			public Int512[] ShiftRight_Int512(int shiftAmount)
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
			[Arguments(256)]
			[Arguments(320)]
			[Arguments(384)]
			[Arguments(448)]
			[Arguments(450)]
			[BenchmarkCategory("UnsignedShiftRight")]
			public Int512[] ShiftRightUnsigned_Int512(int shiftAmount)
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
		[BenchmarkCategory("Int512", "Signed", "Integer")]
		public class ParsingAndFormatting
		{
			[Benchmark]
			[BenchmarkCategory("Formatting")]
			[ArgumentsSource(nameof(ValuesToFormat))]
			public string ToString_Int512(Int512 value, string? fmt)
			{
				return value.ToString(fmt, CultureInfo.CurrentCulture);
			}
			[Benchmark]
			[BenchmarkCategory("Parsing")]
			[ArgumentsSource(nameof(ValuesToParse))]
			public Int512 Parse_Int512(string s, NumberStyles style, IFormatProvider provider)
			{
				return Int512.Parse(s, style, provider);
			}

			public IEnumerable<object[]> ValuesToParse()
			{
				yield return ["9223372036854775808", NumberStyles.Integer, CultureInfo.CurrentCulture];
				yield return ["170141183460469231731687303715884105728", NumberStyles.Integer, CultureInfo.CurrentCulture];
				yield return ["57896044618658097711785492504343953926634992332820282019728792003956564819967", NumberStyles.Integer, CultureInfo.CurrentCulture];
				yield return ["6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042047", NumberStyles.Integer, CultureInfo.CurrentCulture];
				yield return ["-9223372036854775808", NumberStyles.Integer, CultureInfo.CurrentCulture];
				yield return ["-170141183460469231731687303715884105728", NumberStyles.Integer, CultureInfo.CurrentCulture];
				yield return ["-57896044618658097711785492504343953926634992332820282019728792003956564819966", NumberStyles.Integer, CultureInfo.CurrentCulture];
				yield return ["-6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042046", NumberStyles.Integer, CultureInfo.CurrentCulture];
				yield return ["1111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111", NumberStyles.BinaryNumber, CultureInfo.CurrentCulture];
				yield return ["FEDCBA09876543210123456789ABCDEF", NumberStyles.HexNumber, CultureInfo.CurrentCulture];
			}
			public IEnumerable<object[]> ValuesToFormat()
			{
				yield return [UInt512.MaxValue, "D"];
				yield return [UInt512.MaxValue, "X"];
				yield return [UInt512.MaxValue, "B"];
				yield return [UInt512.MaxValue, "C"];
				yield return [UInt512.MaxValue, "E"];
				yield return [UInt512.MaxValue, "N"];
				yield return [UInt512.MinValue, "D"];
				yield return [UInt512.MinValue, "X"];
				yield return [UInt512.MinValue, "B"];
				yield return [UInt512.MinValue, "C"];
				yield return [UInt512.MinValue, "E"];
				yield return [UInt512.MinValue, "N"];
			}
		}
	}
}
