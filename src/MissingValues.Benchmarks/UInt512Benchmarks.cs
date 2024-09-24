using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues.Benchmarks
{
	public class UInt512Benchmarks
	{
		[MinColumn, MaxColumn, MeanColumn, MedianColumn]
		public class MathOperators
		{
			[Params(100, 10_000, 500_000)]
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
			public UInt512[] Add_UInt512()
			{
				for (int i = 0; i < Length; i++)
				{
					c[i] = a[i] + b[i];
				}
				return c;
			}

			[Benchmark]
			public UInt512[] Subtract_UInt512()
			{
				for (int i = 0; i < Length; i++)
				{
					c[i] = a[i] - b[i];
				}
				return c;
			}

			[Benchmark]
			public UInt512[] Multiply_UInt512()
			{
				for (int i = 0; i < Length; i++)
				{
					c[i] = a[i] * b[i];
				}
				return c;
			}

			[Benchmark]
			public UInt512[] Divide_UInt512()
			{
				for (int i = 0; i < Length; i++)
				{
					c[i] = a[i] / b[i];
				}
				return c;
			}

			[Benchmark]
			public UInt512[] Modulus_UInt512()
			{
				for (int i = 0; i < Length; i++)
				{
					c[i] = a[i] % b[i];
				}
				return c;
			}

			[Benchmark]
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
			public UInt512[] ShiftRight_UInt512(int shiftAmount)
			{
				for (int i = 0; i < Length; i++)
				{
					c[i] = a[i] >> shiftAmount;
				}
				return c;
			}
		}

		[MemoryDiagnoser]
		public class ParsingAndFormatting
		{
			[Benchmark]
			[ArgumentsSource(nameof(ValuesToFormat))]
			public string ToString_UInt512(UInt512 value, string? fmt)
			{
				return value.ToString(fmt, CultureInfo.CurrentCulture);
			}
			[Benchmark]
			[ArgumentsSource(nameof(ValuesToParse))]
			public UInt512 Parse_UInt512(string s, NumberStyles style, IFormatProvider provider)
			{
				return UInt512.Parse(s, style, provider);
			}

			public IEnumerable<object[]> ValuesToParse()
			{
				yield return ["6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042047", NumberStyles.Integer, CultureInfo.CurrentCulture];
				yield return ["1111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111", NumberStyles.BinaryNumber, CultureInfo.CurrentCulture];
				yield return ["FEDCBA09876543210123456789ABCDEF", NumberStyles.HexNumber, CultureInfo.CurrentCulture];
			}
			public IEnumerable<object[]> ValuesToFormat()
			{
				yield return [UInt512.MaxValue, "D"];
				yield return [UInt512.MaxValue, "X128"];
				yield return [UInt512.MaxValue, "B512"];
				yield return [UInt512.MaxValue, "C"];
				yield return [UInt512.MaxValue, "E"];
				yield return [UInt512.MaxValue, "N"];
			}
		}
	}
}
