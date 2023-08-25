using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
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

		private (UInt256, UInt256) _u256;
		private (UInt512, UInt512) _u512;
		private (BigInteger, BigInteger) _uBig;
		private (Int256, Int256) _i256;
		private (Int512, Int512) _i512;
		private (BigInteger, BigInteger) _iBig;


		[GlobalSetup]
		public void Setup()
		{
			Span<byte> bigIntBytes = stackalloc byte[64];
			_rng.NextBytes(bigIntBytes);

			unchecked
			{
				_u256 = (new UInt256((ulong)_rng.NextInt64(), (ulong)_rng.NextInt64(), (ulong)_rng.NextInt64(), (ulong)_rng.NextInt64()), new UInt256((ulong)_rng.NextInt64(), (ulong)_rng.NextInt64(), (ulong)_rng.NextInt64(), (ulong)_rng.NextInt64()));
				_u512 = (new UInt512((ulong)_rng.NextInt64(), (ulong)_rng.NextInt64(), (ulong)_rng.NextInt64(), (ulong)_rng.NextInt64(), (ulong)_rng.NextInt64(), (ulong)_rng.NextInt64(), (ulong)_rng.NextInt64(), (ulong)_rng.NextInt64()), new UInt512((ulong)_rng.NextInt64(), (ulong)_rng.NextInt64(), (ulong)_rng.NextInt64(), (ulong)_rng.NextInt64(), (ulong)_rng.NextInt64(), (ulong)_rng.NextInt64(), (ulong)_rng.NextInt64(), (ulong)_rng.NextInt64()));
				_uBig = (new BigInteger(bigIntBytes, true), new BigInteger(bigIntBytes, true));
			
				_i256 = ((Int256)_u256.Item1, (Int256)_u256.Item2);
				_i512 = ((Int512)_u512.Item1, (Int512)_u512.Item2);
				_iBig = (new BigInteger(bigIntBytes), new BigInteger(bigIntBytes));
			}
		}

		[Benchmark]
		public BigInteger UnsignedBigInt_Addition()
		{
			return _uBig.Item1 + _uBig.Item2;
		}
		[Benchmark]
		public UInt256 UInt256_Addition()
		{
			return _u256.Item1 + _u256.Item2;
		}
		[Benchmark]
		public UInt512 UInt512_Addition()
		{
			return _u512.Item1 + _u512.Item2;
		}

		[Benchmark]
		public BigInteger BigInt_Addition()
		{
			return _iBig.Item1 + _iBig.Item2;
		}
		[Benchmark]
		public Int256 Int256_Addition()
		{
			return _i256.Item1 + _i256.Item2;
		}
		[Benchmark]
		public Int512 Int512_Addition()
		{
			return _i512.Item1 + _i512.Item2;
		}


		[Benchmark]
		public BigInteger UnsignedBigInt_Multiplication()
		{
			return _uBig.Item1 * _uBig.Item2;
		}
		[Benchmark]
		public UInt256 UInt256_Multiplication()
		{
			return _u256.Item1 * _u256.Item2;
		}
		[Benchmark]
		public UInt512 UInt512_Multiplication()
		{
			return _u512.Item1 * _u512.Item2;
		}

		[Benchmark]
		public BigInteger BigInt_Multiplication()
		{
			return _iBig.Item1 * _iBig.Item2;
		}
		[Benchmark]
		public Int256 Int256_Multiplication()
		{
			return _i256.Item1 * _i256.Item2;
		}
		[Benchmark]
		public Int512 Int512_Multiplication()
		{
			return _i512.Item1 * _i512.Item2;
		}


		[Benchmark]
		public BigInteger UnsignedBigInt_Division()
		{
			return _uBig.Item1 / _uBig.Item2;
		}
		[Benchmark]
		public UInt256 UInt256_Division()
		{
			return _u256.Item1 / _u256.Item2;
		}
		[Benchmark]
		public UInt512 UInt512_Division()
		{
			return _u512.Item1 / _u512.Item2;
		}

		[Benchmark]
		public BigInteger BigInt_Division()
		{
			return _iBig.Item1 / _iBig.Item2;
		}
		[Benchmark]
		public Int256 Int256_Division()
		{
			return _i256.Item1 / _i256.Item2;
		}
		[Benchmark]
		public Int512 Int512_Division()
		{
			return _i512.Item1 / _i512.Item2;
		}
	}
}
