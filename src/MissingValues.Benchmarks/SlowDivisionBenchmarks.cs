using BenchmarkDotNet.Attributes;
using MissingValues.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues.Benchmarks
{
	[MeanColumn, MaxColumn, MinColumn]
	public class SlowDivisionBenchmarks
	{
		[Params(100, 10_000, 500_000)]
		public int Length;
		private ulong[][] a64, b64;
		private ulong[][] c64;
		private uint[][] a32, b32;
		private uint[][] c32;

		[GlobalSetup]
		public void Setup()
		{
			a64 = new ulong[Length][];
			b64 = new ulong[Length][];
			a32 = new uint[Length][];
			b32 = new uint[Length][];

			c64 = new ulong[Length][];
			c32 = new uint[Length][];

			var ua = new UInt512();
			var ub = new UInt512();

			for (int i = 0; i < Length; i++)
			{
				ua = new(
					(ulong)Random.Shared.NextInt64(), (ulong)Random.Shared.NextInt64(), (ulong)Random.Shared.NextInt64(), (ulong)Random.Shared.NextInt64(),
					(ulong)Random.Shared.NextInt64(), (ulong)Random.Shared.NextInt64(), (ulong)Random.Shared.NextInt64(), (ulong)Random.Shared.NextInt64());
				a64[i] = MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<UInt512, ulong>(ref ua), 8).ToArray();
				a32[i] = MemoryMarshal.Cast<ulong, uint>(a64[i]).ToArray();
				ub = new(
					(ulong)Random.Shared.NextInt64(), (ulong)Random.Shared.NextInt64(), (ulong)Random.Shared.NextInt64(), (ulong)Random.Shared.NextInt64(),
					(ulong)Random.Shared.NextInt64(), (ulong)Random.Shared.NextInt64(), (ulong)Random.Shared.NextInt64(), (ulong)Random.Shared.NextInt64());
				b64[i] = MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<UInt512, ulong>(ref ub), 8).ToArray();
				b32[i] = MemoryMarshal.Cast<ulong, uint>(b64[i]).ToArray();

				c64[i] = new ulong[int.Max(a64[i].Length, b64[i].Length)];
				c32[i] = new uint[int.Max(a32[i].Length, b32[i].Length)];
			}

			
		}

		[Benchmark]
		public ulong[][] SlowDivision_UInt64()
		{
			for (int i = 0; i < Length; i++)
			{
				Calculator.Divide(a64[i], b64[i], c64[i]);
			}
			return c64;
		}
		[Benchmark]
		public uint[][] SlowDivision_UInt32()
		{
			for (int i = 0; i < Length; i++)
			{
				Calculator.Divide(a32[i], b32[i], c32[i]);
			}
			return c32;
		}
	}
}
