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
	public class QuadBenchmarks
	{
		private static readonly Random rnd = new Random(7);

		[HideColumns("Job", "Error", "StdDev")]
		[BenchmarkCategory("Quad", "Floating")]
		public class MathOperators
		{
			private Quad _q;

			private static readonly Quad Two = new Quad(0x4000_0000_0000_0000, 0x0000_0000_0000_0000);

			[Params(10L, 100L, 1000L)]
            public long E { get; set; }

            [GlobalSetup]
			public void Setup()
			{
				_q = (Quad)rnd.NextDouble() * E * E;
			}

			[Benchmark]
			[BenchmarkCategory("Addition")]
			public Quad Addition_Op__Quad()
			{
				Quad sum = _q;
				for (int i = 0, length = (int)E; i < length; i++)
				{
					sum += _q;
				}
				return sum;
			}

			[Benchmark]
			[BenchmarkCategory("Subtraction")]
			public Quad Subtraction_Op__Quad()
			{
				Quad sum = _q;
				for (int i = 0, length = (int)E; i < length; i++)
				{
					sum -= _q;
				}
				return sum;
			}

			[Benchmark]
			[BenchmarkCategory("Multiplication")]
			public Quad Multiplication_Op__Quad()
			{
				Quad sum = _q;
				for (int i = 0, length = (int)E; i < length; i++)
				{
					sum *= _q;
				}
				return sum;
			}

			[Benchmark]
			[BenchmarkCategory("Division")]
			public Quad Division_Op__Quad()
			{
				Quad sum = _q;
				for (int i = 0, length = (int)E; i < length; i++)
				{
					sum /= Two;
				}
				return sum;
			}
		}
		[MemoryDiagnoser]
		[HideColumns("Job", "Error", "StdDev")]
		[BenchmarkCategory("Quad", "Floating")]
		public class Parsing
		{
			private string[] _lines;
			private Quad[] _quads;

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
				_quads = new Quad[LineCount];
				for (int i = 0; i < LineCount; i++)
				{
					_quads[i] = Quad.Parse(_lines[i]);
				}
			}

			[Benchmark]
			[BenchmarkCategory("Formatting")]
			public Span<string> QuadToString()
			{
				Span<string> result = new string[_lines.Length];

                for (int i = 0; i < LineCount; i++)
                {
					result[i] = _quads[i].ToString();
                }

				return result;
            }

			[Benchmark]
			[BenchmarkCategory("Parsing")]
			public Span<Quad> QuadParse()
			{
				Span<Quad> result = new Quad[_lines.Length];

                for (int i = 0; i < LineCount; i++)
                {
					result[i] = Quad.Parse(_lines[i], _numberFormatInfo);
                }

				return result;
            }
		}
	}
}
