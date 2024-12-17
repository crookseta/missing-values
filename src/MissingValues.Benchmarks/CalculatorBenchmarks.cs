using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using MissingValues.Internals;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues.Benchmarks;

[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net90)]
[HideColumns("Job", "Error", "StdDev")]
public class CalculatorDivisionBenchmarks
{
	[Benchmark]
	[ArgumentsSource(nameof(Arguments256))]
	public unsafe (UInt256, uint) DivRem_UInt256_Old(UInt256 dividend, uint divisor)
	{
		const int UIntCount = 32 / sizeof(uint);

		Span<uint> quotientSpan = stackalloc uint[UIntCount];
		quotientSpan.Clear();
		Unsafe.WriteUnaligned(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(quotientSpan)), dividend);

		Span<uint> rawBits = stackalloc uint[UIntCount];
		rawBits.Clear();

		Calculator.DivRem(quotientSpan[..((UIntCount) - (BitHelper.LeadingZeroCount(in dividend) / 32))], divisor, rawBits, out uint remainder);

		return (Unsafe.ReadUnaligned<UInt256>(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(rawBits))), remainder);
	}
	[Benchmark]
	[ArgumentsSource(nameof(Arguments256))]
	public unsafe (UInt256, uint) DivRem_UInt256_New(UInt256 dividend, uint divisor)
	{
		Calculator.DivRem(in dividend, divisor, out var quo, out uint r);
		return (quo, r);
	}

	[Benchmark]
	[ArgumentsSource(nameof(Arguments256))]
	public unsafe uint Remainder_UInt256_Old(UInt256 dividend, uint divisor)
	{
		const int UIntCount = 32 / sizeof(uint);

		Span<uint> quotientSpan = stackalloc uint[UIntCount];
		quotientSpan.Clear();
		Unsafe.WriteUnaligned(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(quotientSpan)), dividend);

		return Calculator.Remainder(quotientSpan[..((UIntCount) - (BitHelper.LeadingZeroCount(in dividend) / 32))], divisor);
	}
	[Benchmark]
	[ArgumentsSource(nameof(Arguments256))]
	public unsafe uint Remainder_UInt256_New(UInt256 dividend, uint divisor)
	{
		return Calculator.Remainder(in dividend, divisor);
	}

	[Benchmark]
	[ArgumentsSource(nameof(Arguments256))]
	public unsafe UInt256 Division_UInt256_Old(UInt256 dividend, uint divisor)
	{
		const int UIntCount = 32 / sizeof(uint);

		Span<uint> quotientSpan = stackalloc uint[UIntCount];
		quotientSpan.Clear();
		Unsafe.WriteUnaligned(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(quotientSpan)), dividend);

		Span<uint> rawBits = stackalloc uint[UIntCount];
		rawBits.Clear();

		Calculator.Divide(quotientSpan[..((UIntCount) - (BitHelper.LeadingZeroCount(in dividend) / 32))], divisor, rawBits);

		return Unsafe.ReadUnaligned<UInt256>(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(rawBits)));
	}
	[Benchmark]
	[ArgumentsSource(nameof(Arguments256))]
	public unsafe UInt256 Division_UInt256_New(UInt256 dividend, uint divisor)
	{
		return Calculator.Divide(in dividend, divisor);
	}


	[Benchmark]
	[ArgumentsSource(nameof(Arguments512))]
	public unsafe (UInt512, UInt512) DivRem_UInt512_Old(UInt512 dividend, uint divisor)
	{
		const int UIntCount = 64 / sizeof(uint);

		Span<uint> quotientSpan = stackalloc uint[UIntCount];
		quotientSpan.Clear();
		Unsafe.WriteUnaligned(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(quotientSpan)), dividend);

		Span<uint> rawBits = stackalloc uint[UIntCount];
		rawBits.Clear();

		Calculator.DivRem(quotientSpan[..((UIntCount) - (BitHelper.LeadingZeroCount(in dividend) / 32))], divisor, rawBits, out uint remainder);

		return (Unsafe.ReadUnaligned<UInt512>(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(rawBits))), remainder);
	}
	[Benchmark]
	[ArgumentsSource(nameof(Arguments512))]
	public unsafe (UInt512, UInt512) DivRem_UInt512_New(UInt512 dividend, uint divisor)
	{
		Calculator.DivRem(in dividend, divisor, out var quo, out uint r);
		return (quo, r);
	}

	[Benchmark]
	[ArgumentsSource(nameof(Arguments512))]
	public unsafe uint Remainder_UInt512_Old(UInt512 dividend, uint divisor)
	{
		const int UIntCount = 64 / sizeof(uint);

		Span<uint> quotientSpan = stackalloc uint[UIntCount];
		quotientSpan.Clear();
		Unsafe.WriteUnaligned(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(quotientSpan)), dividend);

		return Calculator.Remainder(quotientSpan[..((UIntCount) - (BitHelper.LeadingZeroCount(in dividend) / 32))], divisor);
	}
	[Benchmark]
	[ArgumentsSource(nameof(Arguments512))]
	public unsafe uint Remainder_UInt512_New(UInt512 dividend, uint divisor)
	{
		return Calculator.Remainder(in dividend, divisor);
	}

	[Benchmark]
	[ArgumentsSource(nameof(Arguments512))]
	public unsafe UInt512 Division_UInt512_Old(UInt512 dividend, uint divisor)
	{
		const int UIntCount = 64 / sizeof(uint);

		Span<uint> quotientSpan = stackalloc uint[UIntCount];
		quotientSpan.Clear();
		Unsafe.WriteUnaligned(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(quotientSpan)), dividend);

		Span<uint> rawBits = stackalloc uint[UIntCount];
		rawBits.Clear();

		Calculator.Divide(quotientSpan[..((UIntCount) - (BitHelper.LeadingZeroCount(in dividend) / 32))], divisor, rawBits);

		return Unsafe.ReadUnaligned<UInt256>(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(rawBits)));
	}
	[Benchmark]
	[ArgumentsSource(nameof(Arguments512))]
	public unsafe UInt512 Division_UInt512_New(UInt512 dividend, uint divisor)
	{
		return Calculator.Divide(in dividend, divisor);
	}


	public static IEnumerable<object[]> Arguments256()
	{
		yield return [UInt256.MaxValue, 10U];
		yield return [(UInt256)UInt128.MaxValue, 10U];
		yield return [(UInt256)ulong.MaxValue, 10U];
		yield return [(UInt256)uint.MaxValue, 10U];
	}
	public static IEnumerable<object[]> Arguments512()
	{
		yield return [UInt512.MaxValue, 10U];
		yield return [(UInt512)UInt256.MaxValue, 10U];
		yield return [(UInt512)UInt128.MaxValue, 10U];
		yield return [(UInt512)ulong.MaxValue, 10U];
		yield return [(UInt512)uint.MaxValue, 10U];
	}
}
