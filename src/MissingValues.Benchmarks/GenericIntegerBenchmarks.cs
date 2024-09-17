using BenchmarkDotNet.Attributes;
using MissingValues.Internals;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues.Benchmarks
{
	[MinColumn, MaxColumn, MeanColumn, MedianColumn]
	[GenericTypeArguments(typeof(int))]
	[GenericTypeArguments(typeof(long))]
	[GenericTypeArguments(typeof(Int128))]
	[GenericTypeArguments(typeof(Int256))]
	[GenericTypeArguments(typeof(Int512))]
	public class GenericIntegerBenchmarks<T>
		where T : INumber<T>
	{
		[Params(100, 10_000, 250_000, 750_000)]
		public int Length;

		private T[] _v1, _v2;
		private T[] _destination;
		private string[] _toString;

		[GlobalSetup]
		public void Setup()
		{
			_v1 = new T[Length];
			_v2 = new T[Length];

			Span<byte> bytes = stackalloc byte[Unsafe.SizeOf<T>()];

			for (int i = 0; i < Length; i++)
			{
				Random.Shared.NextBytes(bytes);
				_v1[i] = Unsafe.ReadUnaligned<T>(ref bytes[0]);
			}
			Random.Shared.GetItems(_v1, _v2.AsSpan());

			_destination = new T[Length];
			_toString = new string[Length];
		}

		[Benchmark]
		public T[] Add_Integer()
		{
			for (int i = 0; i < Length; i++)
			{
				_destination[i] = _v1[i] + _v2[i];
			}
			return _destination;
		}

		[Benchmark]
		public T[] Subtract_Integer()
		{
			for (int i = 0; i < Length; i++)
			{
				_destination[i] = _v1[i] - _v2[i];
			}
			return _destination;
		}

		[Benchmark]
		public T[] Multiply_Integer()
		{
			for (int i = 0; i < Length; i++)
			{
				_destination[i] = _v1[i] * _v2[i];
			}
			return _destination;
		}

		[Benchmark]
		public T[] Divide_Integer()
		{
			for (int i = 0; i < Length; i++)
			{
				_destination[i] = _v1[i] / _v2[i];
			}
			return _destination;
		}

		[Benchmark]
		public string[] ToString_Integer()
		{
			for (int i = 0; i < Length; i++)
			{
				_toString[i] = _v1[i].ToString()!;
			}
			return _toString;
		}
	}
}
