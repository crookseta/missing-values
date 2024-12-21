using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues.Benchmarks
{
	public class QuadBenchmarks
	{
		private static readonly Random rnd = new Random(7);

		[SimpleJob(RuntimeMoniker.Net80)]
		[SimpleJob(RuntimeMoniker.Net90)]
		[HideColumns("Job", "Error", "StdDev")]
		public class MathOperators
		{
			private Quad q;
			private double d;
			private float f;

			private static readonly Quad Two = new Quad(0x4000_0000_0000_0000, 0x0000_0000_0000_0000);

			[Params(10L, 100L, 1000L)]
            public long E { get; set; }

            [GlobalSetup]
			public void Setup()
			{
				f = rnd.NextSingle() * E;
				d = rnd.NextDouble() * E;
				q = (Quad)rnd.NextDouble() * E * E;
			}

			[Benchmark]
			public float Addition_Op__Single()
			{
				float sum = f;
				for (int i = 0, length = (int)E; i < length; i++)
				{
					sum += f;
				}
				return sum;
			}
			[Benchmark]
			public double Addition_Op__Double()
			{
				double sum = d;
				for (int i = 0, length = (int)E; i < length; i++)
				{
					sum += d;
				}
				return sum;
			}
			[Benchmark]
			public Quad Addition_Op__Quad()
			{
				Quad sum = q;
				for (int i = 0, length = (int)E; i < length; i++)
				{
					sum += q;
				}
				return sum;
			}

			[Benchmark]
			public float Multiplication_Op__Single()
			{
				float sum = f;
				for (int i = 0, length = (int)E; i < length; i++)
				{
					sum *= f;
				}
				return sum;
			}
			[Benchmark]
			public double Multiplication_Op__Double()
			{
				double sum = d;
				for (int i = 0, length = (int)E; i < length; i++)
				{
					sum *= d;
				}
				return sum;
			}
			[Benchmark]
			public Quad Multiplication_Op__Quad()
			{
				Quad sum = q;
				for (int i = 0, length = (int)E; i < length; i++)
				{
					sum *= q;
				}
				return sum;
			}

			[Benchmark]
			public float Division_Op__Single()
			{
				float sum = f;
				for (int i = 0, length = (int)E; i < length; i++)
				{
					sum /= 2F;
				}
				return sum;
			}
			[Benchmark]
			public double Division_Op__Double()
			{
				double sum = d;
				for (int i = 0, length = (int)E; i < length; i++)
				{
					sum /= 2d;
				}
				return sum;
			}
			[Benchmark]
			public Quad Division_Op__Quad()
			{
				Quad sum = q;
				for (int i = 0, length = (int)E; i < length; i++)
				{
					sum /= Two;
				}
				return sum;
			}
		}
		[MemoryDiagnoser]
		[SimpleJob(RuntimeMoniker.Net80)]
		[SimpleJob(RuntimeMoniker.Net90)]
		[HideColumns("Job", "Error", "StdDev")]
		public class Parsing
		{
			private string[] _lines;

			[Params(@"Data/canada.txt", @"Data/mesh.txt", @"Data/synthetic.txt")]
			public string FileName { get; set; }
			public int LineCount { get; set; }
			public ReadOnlySpan<string> Lines => new(_lines);
			private NumberFormatInfo _numberFormatInfo = NumberFormatInfo.InvariantInfo;

			[GlobalSetup]
			public void Setup()
			{
				_lines = File.ReadAllLines(FileName);
				LineCount = _lines.Length;
			}

			[Benchmark]
			public Span<float> SingleParse()
			{
				Span<float> result = new float[_lines.Length];

                for (int i = 0; i < LineCount; i++)
                {
					result[i] = float.Parse(_lines[i], _numberFormatInfo);
                }

				return result;
            }

			[Benchmark]
			public Span<double> DoubleParse()
			{
				Span<double> result = new double[_lines.Length];

                for (int i = 0; i < LineCount; i++)
                {
					result[i] = double.Parse(_lines[i], _numberFormatInfo);
                }

				return result;
            }

			[Benchmark]
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
