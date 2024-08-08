using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues.Benchmarks
{
	[MemoryDiagnoser]
	public class ParsingBenchmarks
	{
		const int MaxUInt256HexDigits = 64;
		const int MaxUInt512HexDigits = 128;
		const int MaxUInt256BinDigits = 256;
		const int MaxUInt512BinDigits = 512;

		[Benchmark]
		public UInt256 NextHexDigit_ShiftLeft_UInt256()
		{
			UInt256 result = UInt256.One;

			for (int i = 1; i < MaxUInt256HexDigits; i++)
			{
				result <<= 4;
			}

			return result;
		}
		[Benchmark]
		public UInt256 NextHexDigit_Multiply_UInt256()
		{
			UInt256 result = UInt256.One;

			for (int i = 1; i < MaxUInt256HexDigits; i++)
			{
				result *= new UInt256(0, 0, 0, 16);
			}

			return result;
		}
		[Benchmark]
		public UInt256 NextBinDigit_ShiftLeft_UInt256()
		{
			UInt256 result = UInt256.One;

			for (int i = 1; i < MaxUInt256BinDigits; i++)
			{
				result <<= 1;
			}

			return result;
		}
		[Benchmark]
		public UInt256 NextBinDigit_Multiply_UInt256()
		{
			UInt256 result = UInt256.One;

			for (int i = 1; i < MaxUInt256BinDigits; i++)
			{
				result *= new UInt256(0, 0, 0, 2);
			}

			return result;
		}
		[Benchmark]
		public UInt512 NextHexDigit_ShiftLeft_UInt512()
		{
			UInt512 result = UInt512.One;

			for (int i = 1; i < MaxUInt512HexDigits; i++)
			{
				result <<= 4;
			}

			return result;
		}
		[Benchmark]
		public UInt512 NextHexDigit_Multiply_UInt512()
		{
			UInt512 result = UInt512.One;

			for (int i = 1; i < MaxUInt512HexDigits; i++)
			{
				result *= new UInt512(0,0,0,0,0,0,0,16);
			}

			return result;
		}
		[Benchmark]
		public UInt512 NextBinDigit_ShiftLeft_UInt512()
		{
			UInt512 result = UInt512.One;

			for (int i = 1; i < MaxUInt512BinDigits; i++)
			{
				result <<= 1;
			}

			return result;
		}
		[Benchmark]
		public UInt512 NextBinDigit_Multiply_UInt512()
		{
			UInt512 result = UInt512.One;

			for (int i = 1; i < MaxUInt512BinDigits; i++)
			{
				result *= new UInt512(0, 0, 0, 0, 0, 0, 0, 2);
			}

			return result;
		}
	}
}
