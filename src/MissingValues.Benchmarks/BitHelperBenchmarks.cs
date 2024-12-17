using BenchmarkDotNet.Attributes;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues.Benchmarks;

[MarkdownExporter]
public class ReverseEndiannessBenchmarks
{
	private static readonly UInt256 _256 = UInt256.MaxValue;
	private static readonly UInt512 _512 = UInt512.MaxValue;

	[Benchmark(Baseline = true)]
	public UInt256 ReverseEndianness_UInt256_BitHelper()
	{
		return BitHelper.ReverseEndianness(in _256);
	}
	[Benchmark]
	public UInt512 ReverseEndianness_UInt512_BitHelper()
	{
		return BitHelper.ReverseEndianness(in _512);
	}
	
	[Benchmark]
	public UInt256 ReverseEndianness_UInt256_Span()
	{
		return ReverseEndianness(in _256);
	}
	[Benchmark]
	public UInt512 ReverseEndianness_UInt512_Span()
	{
		return ReverseEndianness(in _512);
	}


	private static UInt256 ReverseEndianness(in UInt256 value)
	{
		Span<ulong> resultSpan = stackalloc ulong[4];
		Span<ulong> valueSpan = stackalloc ulong[4];
		Unsafe.WriteUnaligned(ref Unsafe.As<ulong, byte>(ref MemoryMarshal.GetReference(valueSpan)), value);

		BinaryPrimitives.ReverseEndianness(valueSpan, resultSpan);

		return Unsafe.ReadUnaligned<UInt256>(ref Unsafe.As<ulong, byte>(ref MemoryMarshal.GetReference(resultSpan)));
	}
	private static UInt512 ReverseEndianness(in UInt512 value)
	{
		Span<ulong> resultSpan = stackalloc ulong[8];
		Span<ulong> valueSpan = stackalloc ulong[8];
		Unsafe.WriteUnaligned(ref Unsafe.As<ulong, byte>(ref MemoryMarshal.GetReference(valueSpan)), value);

		BinaryPrimitives.ReverseEndianness(valueSpan, resultSpan);

		return Unsafe.ReadUnaligned<UInt512>(ref Unsafe.As<ulong, byte>(ref MemoryMarshal.GetReference(resultSpan)));
	}
}


[MarkdownExporter]
[MemoryDiagnoser]
public class FloatingBitCastBenchmarks
{
	private static Quad _quad = Quad.Pi;
	private static Octo _octo = Octo.Pi;
	

	[Benchmark]
	public unsafe UInt128 UnsafeCast_QuadToUInt128()
	{
		var value = _quad;
		return *((UInt128*)&value);
	}
	[Benchmark]
	public unsafe Int128 UnsafeCast_QuadToInt128()
	{
		var value = _quad;
		return *((Int128*)&value);
	}
	[Benchmark]
	public unsafe UInt256 UnsafeCast_OctoToUInt256()
	{
		var value = _octo;
		return *((UInt256*)&value);
	}
	[Benchmark]
	public unsafe Int256 UnsafeCast_OctoToInt256()
	{
		var value = _octo;
		return *((Int256*)&value);
	}

	[Benchmark]
	public UInt128 UnsafeAs_QuadToUInt128()
	{
		return Unsafe.As<Quad, UInt128>(ref _quad);
	}
	[Benchmark]
	public Int128 UnsafeAs_QuadToInt128()
	{
		return Unsafe.As<Quad, Int128>(ref _quad);
	}
	[Benchmark]
	public UInt256 UnsafeAs_OctoToUInt256()
	{
		return Unsafe.As<Octo, UInt256>(ref _octo);
	}
	[Benchmark]
	public Int256 UnsafeAs_OctoToInt256()
	{
		return Unsafe.As<Octo, Int256>(ref _octo);
	}

	[Benchmark]
	public UInt128 UnsafeBitCast_QuadToUInt128()
	{
		return Unsafe.BitCast<Quad, UInt128>(_quad);
	}
	[Benchmark]
	public Int128 UnsafeBitCast_QuadToInt128()
	{
		return Unsafe.BitCast<Quad, Int128>(_quad);
	}
	[Benchmark]
	public UInt256 UnsafeBitCast_OctoToUInt256()
	{
		return Unsafe.BitCast<Octo, UInt256>(_octo);
	}
	[Benchmark]
	public Int256 UnsafeBitCast_OctoToInt256()
	{
		return Unsafe.BitCast<Octo, Int256>(_octo);
	}

	[Benchmark]
	public UInt128 Ctor_QuadToUInt128()
	{
		return new UInt128(_quad._upper, _quad._lower);
	}
	[Benchmark]
	public Int128 Ctor_QuadToInt128()
	{
		return new Int128(_quad._upper, _quad._lower);
	}
	[Benchmark]
	public UInt256 Ctor_OctoToUInt256()
	{
		return new UInt256(_octo.Upper, _octo.Lower);
	}
	[Benchmark]
	public Int256 Ctor_OctoToInt256()
	{
		return new Int256(_octo.Upper, _octo.Lower);
	}
}

[MarkdownExporter]
[MemoryDiagnoser]
public class IntegerCastBenchmarks
{
	private static UInt256 _u256 = UInt256.MaxValue;
	private static UInt512 _u512 = UInt512.MaxValue;

	[Benchmark(Baseline = true)]
	public Int256 Ctor_256()
	{
		return new Int256(_u256.Part3, _u256.Part2, _u256.Part1, _u256.Part0);
	}
	[Benchmark]
	public Int512 Ctor_512()
	{
		return new Int512(_u512.Part7, _u512.Part6, _u512.Part5, _u512.Part4, _u512.Part3, _u512.Part2, _u512.Part1, _u512.Part0);
	}
	[Benchmark]
	public Int256 UnsafeBitCast_256()
	{
		return Unsafe.BitCast<UInt256, Int256>(_u256);
	}
	[Benchmark]
	public Int512 UnsafeBitCast_512()
	{
		return Unsafe.BitCast<UInt512, Int512>(_u512);
	}
	[Benchmark]
	public Int256 UnsafeAs_256()
	{
		return Unsafe.As<UInt256, Int256>(ref _u256);
	}
	[Benchmark]
	public Int512 UnsafeAs_512()
	{
		return Unsafe.As<UInt512, Int512>(ref _u512);
	}
}