using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MissingValues.Benchmarks.Helpers;

namespace MissingValues.Benchmarks.Core
{
	public class UInt512Benchmarks
	{
		[HideColumns("Job", "Error", "StdDev")]
		[BenchmarkCategory("UInt512", "Unsigned", "Integer")]
		public class MathOperators
		{
			private static readonly Random rng = new Random(7);
			[Params(100, 10_000, 100_000)]
			public int Length;
			private UInt512[] a, b, c;

			[GlobalSetup]
			public void Setup()
			{
				a = new UInt512[Length];
				b = new UInt512[Length];

				for (int i = 0; i < Length; i++)
				{
					a[i] = new(
						(ulong)Random.Shared.NextInt64(), (ulong)Random.Shared.NextInt64(), (ulong)Random.Shared.NextInt64(), (ulong)Random.Shared.NextInt64(),
						(ulong)Random.Shared.NextInt64(), (ulong)Random.Shared.NextInt64(), (ulong)Random.Shared.NextInt64(), (ulong)Random.Shared.NextInt64());
					b[i] = new(
						(ulong)Random.Shared.NextInt64(), (ulong)Random.Shared.NextInt64(), (ulong)Random.Shared.NextInt64(), (ulong)Random.Shared.NextInt64(),
						(ulong)Random.Shared.NextInt64(), (ulong)Random.Shared.NextInt64(), (ulong)Random.Shared.NextInt64(), (ulong)Random.Shared.NextInt64());
				}

				c = new UInt512[Length];
			}

			[Benchmark]
			[BenchmarkCategory("Addition")]
			public UInt512[] Add_UInt512()
			{
				for (int i = 0; i < Length; i++)
				{
					c[i] = a[i] + b[i];
				}
				return c;
			}

			[Benchmark]
			[BenchmarkCategory("Subtraction")]
			public UInt512[] Subtract_UInt512()
			{
				for (int i = 0; i < Length; i++)
				{
					c[i] = a[i] - b[i];
				}
				return c;
			}

			[Benchmark]
			[BenchmarkCategory("Multiplication")]
			public UInt512[] Multiply_UInt512()
			{
				for (int i = 0; i < Length; i++)
				{
					c[i] = a[i] * b[i];
				}
				return c;
			}
			
			[Benchmark]
			[BenchmarkCategory("Multiplication")]
			[ArgumentsSource(nameof(Arguments64x512))]
			public UInt512[] Multiply64x512_UInt512(UInt512[] left, UInt512[] right)
			{
				for (int i = 0; i < Length; i++)
				{
					c[i] = left[i] * right[i];
				}
				return c;
			}
			[Benchmark]
			[BenchmarkCategory("Multiplication")]
			[ArgumentsSource(nameof(Arguments128x512))]
			public UInt512[] Multiply128x512_UInt512(UInt512[] left, UInt512[] right)
			{
				for (int i = 0; i < Length; i++)
				{
					c[i] = left[i] * right[i];
				}
				return c;
			}
			[Benchmark]
			[BenchmarkCategory("Multiplication")]
			[ArgumentsSource(nameof(Arguments256x512))]
			public UInt512[] Multiply256x512_UInt512(UInt512[] left, UInt512[] right)
			{
				for (int i = 0; i < Length; i++)
				{
					c[i] = left[i] * right[i];
				}
				return c;
			}
			[Benchmark]
			[BenchmarkCategory("Multiplication")]
			[ArgumentsSource(nameof(Arguments512x512))]
			public UInt512[] Multiply512x512_UInt512(UInt512[] left, UInt512[] right)
			{
				for (int i = 0; i < Length; i++)
				{
					c[i] = left[i] * right[i];
				}
				return c;
			}
			[Benchmark]
			[BenchmarkCategory("Multiplication")]
			[ArgumentsSource(nameof(Arguments512x256))]
			public UInt512[] Multiply512x256_UInt512(UInt512[] left, UInt512[] right)
			{
				for (int i = 0; i < Length; i++)
				{
					c[i] = left[i] * right[i];
				}
				return c;
			}
			[Benchmark]
			[BenchmarkCategory("Multiplication")]
			[ArgumentsSource(nameof(Arguments512x128))]
			public UInt512[] Multiply512x128_UInt512(UInt512[] left, UInt512[] right)
			{
				for (int i = 0; i < Length; i++)
				{
					c[i] = left[i] * right[i];
				}
				return c;
			}
			[Benchmark]
			[BenchmarkCategory("Multiplication")]
			[ArgumentsSource(nameof(Arguments512x64))]
			public UInt512[] Multiply512x64_UInt512(UInt512[] left, UInt512[] right)
			{
				for (int i = 0; i < Length; i++)
				{
					c[i] = left[i] * right[i];
				}
				return c;
			}

			[Benchmark]
			[BenchmarkCategory("Division")]
			public UInt512[] Divide_UInt512()
			{
				for (int i = 0; i < Length; i++)
				{
					c[i] = a[i] / b[i];
				}
				return c;
			}
			
			[Benchmark]
			[BenchmarkCategory("Division")]
			[ArgumentsSource(nameof(Arguments512x512))]
			public UInt512[] Divide512x512_UInt512(UInt512[] left, UInt512[] right)
			{
				for (int i = 0; i < Length; i++)
				{
					c[i] = left[i] / right[i];
				}
				return c;
			}
			[Benchmark]
			[BenchmarkCategory("Division")]
			[ArgumentsSource(nameof(Arguments512x256))]
			public UInt512[] Divide512x256_UInt512(UInt512[] left, UInt512[] right)
			{
				for (int i = 0; i < Length; i++)
				{
					c[i] = left[i] / right[i];
				}
				return c;
			}
			[Benchmark]
			[BenchmarkCategory("Division")]
			[ArgumentsSource(nameof(Arguments512x128))]
			public UInt512[] Divide512x128_UInt512(UInt512[] left, UInt512[] right)
			{
				for (int i = 0; i < Length; i++)
				{
					c[i] = left[i] / right[i];
				}
				return c;
			}
			[Benchmark]
			[BenchmarkCategory("Division")]
			[ArgumentsSource(nameof(Arguments512x64))]
			public UInt512[] Divide512x64_UInt512(UInt512[] left, UInt512[] right)
			{
				for (int i = 0; i < Length; i++)
				{
					c[i] = left[i] / right[i];
				}
				return c;
			}

			[Benchmark]
			[BenchmarkCategory("Modulus")]
			public UInt512[] Modulus_UInt512()
			{
				for (int i = 0; i < Length; i++)
				{
					c[i] = a[i] % b[i];
				}
				return c;
			}

			[Benchmark]
			[BenchmarkCategory("BitwiseAnd")]
			public UInt512[] BitwiseAnd_UInt512()
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
			public UInt512[] ShiftLeft_UInt512(int shiftAmount)
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
			public UInt512[] ShiftRight_UInt512(int shiftAmount)
			{
				for (int i = 0; i < Length; i++)
				{
					c[i] = a[i] >> shiftAmount;
				}
				return c;
			}
			
			public IEnumerable<object[]> Arguments512x64()
			{
				yield return [rng.NextIntegerArray<UInt512>(Length, new UInt512(1, 0, 0, 0, 0, 0, 0, 0), UInt512.MaxValue), rng.NextIntegerArray<UInt512>(Length, ulong.MaxValue)];
			}
			public IEnumerable<object[]> Arguments512x128()
			{
				yield return [rng.NextIntegerArray<UInt512>(Length, new UInt512(1, 0, 0, 0, 0, 0, 0, 0), UInt512.MaxValue), rng.NextIntegerArray<UInt512>(Length, new UInt256(0, 0, 1, 0), UInt128.MaxValue)];
			}
			public IEnumerable<object[]> Arguments512x256()
			{
				yield return [rng.NextIntegerArray<UInt512>(Length, new UInt512(1, 0, 0, 0, 0, 0, 0, 0), UInt512.MaxValue), rng.NextIntegerArray<UInt512>(Length, new UInt256(1, 0, 0, 0), UInt256.MaxValue)];
			}
			public IEnumerable<object[]> Arguments512x512()
			{
				yield return [rng.NextIntegerArray<UInt512>(Length, new UInt512(1, 0, 0, 0, 0, 0, 0, 0), UInt512.MaxValue), rng.NextIntegerArray<UInt512>(Length, new UInt512(1, 0, 0, 0, 0, 0, 0, 0), UInt512.MaxValue)];
			}
			public IEnumerable<object[]> Arguments256x512()
			{
				yield return [rng.NextIntegerArray<UInt512>(Length, new UInt256(1, 0, 0, 0), UInt256.MaxValue), rng.NextIntegerArray<UInt512>(Length, new UInt512(1, 0, 0, 0, 0, 0, 0, 0), UInt512.MaxValue)];
			}
			public IEnumerable<object[]> Arguments128x512()
			{
				yield return [rng.NextIntegerArray<UInt512>(Length, new UInt256(0, 0, 1, 0), UInt128.MaxValue), rng.NextIntegerArray<UInt512>(Length, new UInt512(1, 0, 0, 0, 0, 0, 0, 0), UInt512.MaxValue)];
			}
			public IEnumerable<object[]> Arguments64x512()
			{
				yield return [rng.NextIntegerArray<UInt512>(Length, ulong.MaxValue), rng.NextIntegerArray<UInt512>(Length, new UInt512(1, 0, 0, 0, 0, 0, 0, 0), UInt512.MaxValue)];
			}
		}

		[MemoryDiagnoser]
		[HideColumns("Job", "Error", "StdDev")]
		[BenchmarkCategory("UInt512", "Unsigned", "Integer")]
		public class ParsingAndFormatting
		{
			[Benchmark]
			[BenchmarkCategory("Formatting")]
			[ArgumentsSource(nameof(ValuesToFormat))]
			public string ToString_UInt512(UInt512 value, string? fmt)
			{
				return value.ToString(fmt, CultureInfo.CurrentCulture);
			}
			[Benchmark]
			[BenchmarkCategory("Parsing")]
			[ArgumentsSource(nameof(ValuesToParse))]
			public UInt512 Parse_UInt512(string s, NumberStyles style, IFormatProvider provider)
			{
				return UInt512.Parse(s, style, provider);
			}

			public IEnumerable<object[]> ValuesToParse()
			{
				yield return ["9223372036854775808", NumberStyles.Integer, CultureInfo.CurrentCulture];
				yield return ["170141183460469231731687303715884105728", NumberStyles.Integer, CultureInfo.CurrentCulture];
				yield return ["57896044618658097711785492504343953926634992332820282019728792003956564819967", NumberStyles.Integer, CultureInfo.CurrentCulture];
				yield return ["6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042047", NumberStyles.Integer, CultureInfo.CurrentCulture];
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
			}
		}
	}
}
