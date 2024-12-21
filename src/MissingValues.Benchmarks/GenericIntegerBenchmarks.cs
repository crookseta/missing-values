using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
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
	[SimpleJob(RuntimeMoniker.Net80)]
	[SimpleJob(RuntimeMoniker.Net90)]
	[HideColumns("Job", "Error", "StdDev")]
	[MinColumn, MaxColumn, MeanColumn, MedianColumn]
	[GenericTypeArguments(typeof(Int256))]
	[GenericTypeArguments(typeof(UInt256))]
	[GenericTypeArguments(typeof(Int512))]
	[GenericTypeArguments(typeof(UInt512))]
	public class GenericAritmethicBenchmarks<T>
		where T : IBinaryInteger<T>
	{
		[Params(100, 10_000, 250_000, 750_000)]
		public int Length;

		private T[] _v1, _v2;
		private T[] _destination;

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
		}

		[Benchmark]
		public T[] Add_Integer()
		{
			for (int i = 0; i < Length; i++)
			{
				_destination[i] = unchecked(_v1[i] + _v2[i]);
			}
			return _destination;
		}

		[Benchmark]
		public T[] Subtract_Integer()
		{
			for (int i = 0; i < Length; i++)
			{
				_destination[i] = unchecked(_v1[i] - _v2[i]);
			}
			return _destination;
		}

		[Benchmark]
		public T[] Multiply_Integer()
		{
			for (int i = 0; i < Length; i++)
			{
				_destination[i] = unchecked(_v1[i] * _v2[i]);
			}
			return _destination;
		}

		[Benchmark]
		public T[] Divide_Integer()
		{
			for (int i = 0; i < Length; i++)
			{
				_destination[i] = unchecked(_v1[i] / _v2[i]);
			}
			return _destination;
		}

		[Benchmark]
		public T[] Remainder_Integer()
		{
			for (int i = 0; i < Length; i++)
			{
				_destination[i] = unchecked(_v1[i] % _v2[i]);
			}
			return _destination;
		}

		[Benchmark]
		public T[] DivRem_Integer()
		{
			for (int i = 0; i < Length; i++)
			{
				(_destination[i], _) = unchecked(T.DivRem(_v1[i], _v2[i]));
			}
			return _destination;
		}

		[Benchmark]
		public T[] ShiftLeft_Integer()
		{
			int length = Length / 4;
			for (int i = 0; i < length; i += 2)
			{
				_destination[i] = _v1[i] << 32;
				_destination[i + 1] = _v2[i] << 64;
				_destination[i + 2] = _v1[i] << 96;
				_destination[i + 3] = _v2[i] << 128;
			}
			return _destination;
		}

		[Benchmark]
		public T[] ShiftRight_Integer()
		{
			int length = Length / 4;
			for (int i = 0; i < length; i += 2)
			{
				_destination[i] = _v1[i] >>> 32;
				_destination[i + 1] = _v2[i] >>> 64;
				_destination[i + 2] = _v1[i] >>> 96;
				_destination[i + 3] = _v2[i] >>> 128;
			}
			return _destination;
		}
	}
}
