using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues.Benchmarks.Core
{
	public class OctoBenchmarks
	{
		private static readonly Random rnd = new Random(7);

		[HideColumns("Job", "Error", "StdDev")]
		[BenchmarkCategory("Octo", "Floating")]
		public class MathOperators
		{
			private Octo _o;

			private static readonly Octo Two = new Octo(0x4000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

			[Params(10L, 100L, 1000L)]
			public long E { get; set; }

			[GlobalSetup]
			public void Setup()
			{
				_o = (Octo)rnd.NextDouble() * E * E;
			}

			[Benchmark]
			[BenchmarkCategory("Addition")]
			public Octo Addition_Op__Octo()
			{
				Octo sum = _o;
				for (int i = 0, length = (int)E; i < length; i++)
				{
					sum += _o;
				}
				return sum;
			}

			[Benchmark]
			[BenchmarkCategory("Subtraction")]
			public Octo Subtraction_Op__Octo()
			{
				Octo sum = _o;
				for (int i = 0, length = (int)E; i < length; i++)
				{
					sum -= _o;
				}
				return sum;
			}

			[Benchmark]
			[BenchmarkCategory("Multiplication")]
			public Octo Multiplication_Op__Octo()
			{
				Octo sum = _o;
				for (int i = 0, length = (int)E; i < length; i++)
				{
					sum *= _o;
				}
				return sum;
			}

			[Benchmark]
			[BenchmarkCategory("Division")]
			public Octo Division_Op__Octo()
			{
				Octo sum = _o;
				for (int i = 0, length = (int)E; i < length; i++)
				{
					sum /= Two;
				}
				return sum;
			}
		}
		[MemoryDiagnoser]
		[HideColumns("Job", "Error", "StdDev")]
		[BenchmarkCategory("Octo", "Floating")]
		public class Parsing
		{
			private string[] _lines;
			private Octo[] _octos;

			[Params(@"Data/canada.txt", @"Data/mesh.txt", @"Data/synthetic.txt")]
			public string FileName { get; set; }
			public int LineCount { get; set; }
			public ReadOnlySpan<string> Lines => new(_lines);
			private readonly NumberFormatInfo _numberFormatInfo = NumberFormatInfo.InvariantInfo;

			[GlobalSetup]
			public void Setup()
			{
				_lines = File.ReadAllLines(FileName);
				LineCount = _lines.Length;
				_octos = new Octo[LineCount];
				for (int i = 0; i < LineCount; i++)
				{
					_octos[i] = Octo.Parse(_lines[i]);
				}
			}

			[Benchmark]
			[BenchmarkCategory("Formatting")]
			public Span<string> OctoToString()
			{
				Span<string> result = new string[_lines.Length];

				for (int i = 0; i < LineCount; i++)
				{
					result[i] = _octos[i].ToString();
				}

				return result;
			}

			[Benchmark]
			[BenchmarkCategory("Parsing")]
			public Span<Octo> OctoParse()
			{
				Span<Octo> result = new Octo[_lines.Length];

				for (int i = 0; i < LineCount; i++)
				{
					result[i] = Octo.Parse(_lines[i], _numberFormatInfo);
				}

				return result;
			}
		}
	}
}
