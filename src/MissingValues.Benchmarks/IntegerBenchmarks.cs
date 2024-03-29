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
	[MemoryDiagnoser]
	public class IntegerBenchmarks
	{
		private readonly Random _rng = new Random(7);

		[Params(1, 100, 10_000, 1_000_000)]
		public int Length;

		private UInt512[] _values, _destination;

		[GlobalSetup]
		public void Setup()
		{
			_values = new UInt512[Length];

			for (int i = 0; i < Length; i++)
			{
				_values[i] = new UInt512(
						 (ulong)_rng.NextInt64(), (ulong)_rng.NextInt64(), (ulong)_rng.NextInt64(), (ulong)_rng.NextInt64(), 
						 (ulong)_rng.NextInt64(), (ulong)_rng.NextInt64(), (ulong)_rng.NextInt64(), (ulong)_rng.NextInt64());
			}

			_destination = new UInt512[Length];
		}

		public IEnumerable<UInt512> Numbers()
		{
			yield return new UInt512(0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0xFFFF_FFFF_FFFF_FFFF);
			yield return new UInt512(0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);
			yield return new UInt512(0x0, 0x0, 0x0, 0x0, 0x0, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);
			yield return new UInt512(0x0, 0x0, 0x0, 0x0, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);
			yield return new UInt512(0x0, 0x0, 0x0, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);
			yield return new UInt512(0x0, 0x0, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);
			yield return new UInt512(0x0, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);
			yield return new UInt512(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);
		}

		[Benchmark(Baseline = true)]
		public UInt512[] Integers_ArithmeticOp()
		{
			for (int i = 0; i < Length; i++)
			{
				_destination[i] = _values[i] * _values[i];
				var temp = _destination[i] + _values[i];
				_destination[i] = _destination[i] - temp;
			}

			return _destination;
		}
		[Benchmark]
		public UInt512[] Integers_ArithmeticOp_ByRef()
		{
			for (int i = 0; i < Length; i++)
			{
				_destination[i] = MultiplicationOp(in _values[i], in _values[i]);
				var temp = AdditionOp(in _destination[i], in _values[i]);
				_destination[i] = SubtractionOp(in _destination[i], in temp);
			}

			return _destination;
		}

		private static UInt512 AdditionOp(in UInt512 left, in UInt512 right)
		{
			UInt256 lower = left.Lower + right.Lower;
			UInt256 carry = (lower < left.Lower) ? UInt256.One : UInt256.Zero;

			UInt256 upper = left.Upper + right.Upper + carry;
			return new UInt512(upper, lower);
		}
		private static UInt512 SubtractionOp(in UInt512 left, in UInt512 right)
		{
			UInt256 lower = left.Lower - right.Lower;
			UInt256 borrow = (lower > left.Lower) ? UInt256.One : UInt256.Zero;

			UInt256 upper = left.Upper - right.Upper - borrow;
			return new UInt512(upper, lower);
		}
		private static UInt512 MultiplicationOp(in UInt512 left, in UInt512 right)
		{
			UInt256 upper = UInt256.BigMul(left.Lower, right.Lower, out UInt256 lower);
			upper += (left.Upper * right.Lower) + (left.Lower * right.Upper);
			return new UInt512(upper, lower);
		}
	}
}
