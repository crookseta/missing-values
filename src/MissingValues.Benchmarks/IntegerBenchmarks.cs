using BenchmarkDotNet.Attributes;
using MissingValues.Internals;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues.Benchmarks
{
	public class IntegerBenchmarks
	{
		private readonly Random _rng = new Random(7);

		[Params(100, 10_000, 300_000, 850_000)]
		public int Length;

		private UInt512[] _v1, _v2, _destination;

		[GlobalSetup]
		public void Setup()
		{
			_v1 = new UInt512[Length];
			_v2 = new UInt512[Length];

			for (int i = 0; i < Length; i++)
			{
				_v1[i] = new UInt512(
						 (ulong)_rng.NextInt64(), (ulong)_rng.NextInt64(), (ulong)_rng.NextInt64(), (ulong)_rng.NextInt64(), 
						 (ulong)_rng.NextInt64(), (ulong)_rng.NextInt64(), (ulong)_rng.NextInt64(), (ulong)_rng.NextInt64());
			}
			_rng.GetItems(_v1, _v2.AsSpan());

			_destination = new UInt512[Length];
		}

		[Benchmark(Baseline = true)]
		public UInt512[] Integers_ArithmeticOp_New()
		{
			for (int i = 0; i < Length; i++)
			{
				_destination[i] = _v1[i] - _v2[i];
			}

			return _destination;
		}
		[Benchmark]
		public UInt512[] Integers_ArithmeticOp_Old()
		{
			for (int i = 0; i < Length; i++)
			{
				_destination[i] = OldSubtractionOp(in _v1[i], in _v2[i]);
			}

			return _destination;
		}

		private static UInt512 OldAdditionOp(in UInt512 left, in UInt512 right)
		{
			UInt256 lower = left.Lower + right.Lower;
			UInt256 carry = (lower < left.Lower) ? UInt256.One : UInt256.Zero;

			UInt256 upper = left.Upper + right.Upper + carry;
			return new UInt512(upper, lower);
		}
		private static UInt512 OldSubtractionOp(in UInt512 left, in UInt512 right)
		{
			UInt256 lower = left.Lower - right.Lower;
			UInt256 borrow = (lower > left.Lower) ? UInt256.One : UInt256.Zero;

			UInt256 upper = left.Upper - right.Upper - borrow;
			return new UInt512(upper, lower);
		}
		private static UInt512 OldMultiplicationOp(in UInt512 left, in UInt512 right)
		{
			UInt256 upper = UInt256.BigMul(left.Lower, right.Lower, out UInt256 lower);
			upper += (left.Upper * right.Lower) + (left.Lower * right.Upper);
			return new UInt512(upper, lower);
		}
	}
}
