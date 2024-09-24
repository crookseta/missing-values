using BenchmarkDotNet.Attributes;
using Microsoft.CodeAnalysis.Text;
using MissingValues;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[MinColumn, MaxColumn, MeanColumn, MedianColumn]
[ShortRunJob]
public class WriteUnalignedBenchmarks
{
	private int _index = 1;

	[Benchmark]
	[ArgumentsSource(nameof(UInt256ParamsSource))]
	public unsafe uint UInt256_UnsafePointer_WriteByUInt32(UInt256 u)
	{
		const int UIntCount = 32 / sizeof(uint);

		uint* p = stackalloc uint[UIntCount];

		Unsafe.WriteUnaligned(ref *(byte*)(p + 0), (uint)(u.Part0 >> 00));
		Unsafe.WriteUnaligned(ref *(byte*)(p + 1), (uint)(u.Part0 >> 32));
		
		Unsafe.WriteUnaligned(ref *(byte*)(p + 2), (uint)(u.Part1 >> 00));
		Unsafe.WriteUnaligned(ref *(byte*)(p + 3), (uint)(u.Part1 >> 32));
		
		Unsafe.WriteUnaligned(ref *(byte*)(p + 4), (uint)(u.Part2 >> 00));
		Unsafe.WriteUnaligned(ref *(byte*)(p + 5), (uint)(u.Part2 >> 32));
		
		Unsafe.WriteUnaligned(ref *(byte*)(p + 6), (uint)(u.Part3 >> 00));
		Unsafe.WriteUnaligned(ref *(byte*)(p + 7), (uint)(u.Part3 >> 32));

		Span<uint> left = new Span<uint>(p, (UIntCount) - (BitHelper.LeadingZeroCount(in u) / 32));

		return GetItem(left);
	}
	[Benchmark]
	[ArgumentsSource(nameof(UInt256ParamsSource))]
	public unsafe uint UInt256_UnsafePointer_WriteByUInt256(UInt256 u)
	{
		const int UIntCount = 32 / sizeof(uint);

		uint* p = stackalloc uint[UIntCount];
		Unsafe.WriteUnaligned(ref *(byte*)(p), u);
		Span<uint> left = new Span<uint>(p, (UIntCount) - (BitHelper.LeadingZeroCount(in u) / 32));

		return GetItem(left);
	}
	[Benchmark]
	[ArgumentsSource(nameof(UInt256ParamsSource))]
	public uint UInt256_Ref_WriteByUInt32(UInt256 u)
	{
		const int UIntCount = 32 / sizeof(uint);
		Span<uint> p = stackalloc uint[UIntCount];

		Unsafe.WriteUnaligned(ref Unsafe.Add(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(p)), 0), (uint)(u.Part0 >> 00));
		Unsafe.WriteUnaligned(ref Unsafe.Add(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(p)), 1), (uint)(u.Part0 >> 32));
		
		Unsafe.WriteUnaligned(ref Unsafe.Add(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(p)), 2), (uint)(u.Part1 >> 00));
		Unsafe.WriteUnaligned(ref Unsafe.Add(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(p)), 3), (uint)(u.Part1 >> 32));
		
		Unsafe.WriteUnaligned(ref Unsafe.Add(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(p)), 4), (uint)(u.Part2 >> 00));
		Unsafe.WriteUnaligned(ref Unsafe.Add(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(p)), 5), (uint)(u.Part2 >> 32));
		
		Unsafe.WriteUnaligned(ref Unsafe.Add(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(p)), 6), (uint)(u.Part3 >> 00));
		Unsafe.WriteUnaligned(ref Unsafe.Add(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(p)), 7), (uint)(u.Part3 >> 32));

		return GetItem(p[..((UIntCount) - (BitHelper.LeadingZeroCount(in u) / 32))]);
	}
	[Benchmark]
	[ArgumentsSource(nameof(UInt256ParamsSource))]
	public uint UInt256_Ref_WriteByUInt256(UInt256 u)
	{
		const int UIntCount = 32 / sizeof(uint);
		Span<uint> p = stackalloc uint[UIntCount];

		Unsafe.WriteUnaligned(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(p)), u);

		return GetItem(p[..((UIntCount) - (BitHelper.LeadingZeroCount(in u) / 32))]);
	}

	[Benchmark]
	[ArgumentsSource(nameof(UInt512ParamsSource))]
	public unsafe uint UInt512_UnsafePointer_WriteByUInt512(UInt512 u)
	{
		const int UIntCount = 64 / sizeof(uint);

		uint* p = stackalloc uint[UIntCount];
		Unsafe.WriteUnaligned(ref *(byte*)(p), u);
		Span<uint> left = new Span<uint>(p, (UIntCount) - (BitHelper.LeadingZeroCount(in u) / 32));

		return GetItem(left);
	}
	[Benchmark]
	[ArgumentsSource(nameof(UInt512ParamsSource))]
	public unsafe uint UInt512_UnsafePointer_WriteByUInt32(UInt512 u)
	{
		const int UIntCount = 64 / sizeof(uint);

		uint* p = stackalloc uint[UIntCount];

		Unsafe.WriteUnaligned(ref *(byte*)(p + 0), (uint)(u.Part0 >> 00));
		Unsafe.WriteUnaligned(ref *(byte*)(p + 1), (uint)(u.Part0 >> 32));
		
		Unsafe.WriteUnaligned(ref *(byte*)(p + 2), (uint)(u.Part1 >> 00));
		Unsafe.WriteUnaligned(ref *(byte*)(p + 3), (uint)(u.Part1 >> 32));
		
		Unsafe.WriteUnaligned(ref *(byte*)(p + 4), (uint)(u.Part2 >> 00));
		Unsafe.WriteUnaligned(ref *(byte*)(p + 5), (uint)(u.Part2 >> 32));
		
		Unsafe.WriteUnaligned(ref *(byte*)(p + 6), (uint)(u.Part3 >> 00));
		Unsafe.WriteUnaligned(ref *(byte*)(p + 7), (uint)(u.Part3 >> 32));
		
		Unsafe.WriteUnaligned(ref *(byte*)(p + 8), (uint)(u.Part4 >> 00));
		Unsafe.WriteUnaligned(ref *(byte*)(p + 9), (uint)(u.Part4 >> 32));
		
		Unsafe.WriteUnaligned(ref *(byte*)(p + 10), (uint)(u.Part5 >> 00));
		Unsafe.WriteUnaligned(ref *(byte*)(p + 11), (uint)(u.Part5 >> 32));
		
		Unsafe.WriteUnaligned(ref *(byte*)(p + 12), (uint)(u.Part6 >> 00));
		Unsafe.WriteUnaligned(ref *(byte*)(p + 13), (uint)(u.Part6 >> 32));
		
		Unsafe.WriteUnaligned(ref *(byte*)(p + 14), (uint)(u.Part7 >> 00));
		Unsafe.WriteUnaligned(ref *(byte*)(p + 15), (uint)(u.Part7 >> 32));

		Span<uint> left = new Span<uint>(p, (UIntCount) - (BitHelper.LeadingZeroCount(in u) / 32));

		return GetItem(left);
	}
	[Benchmark]
	[ArgumentsSource(nameof(UInt512ParamsSource))]
	public uint UInt512_Ref_WriteByUInt512(UInt512 u)
	{
		const int UIntCount = 64 / sizeof(uint);

		Span<uint> p = stackalloc uint[UIntCount];
		Unsafe.WriteUnaligned(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(p)), u);

		return GetItem(p[..((UIntCount) - (BitHelper.LeadingZeroCount(in u) / 32))]);
	}
	[Benchmark]
	[ArgumentsSource(nameof(UInt512ParamsSource))]
	public uint UInt512_Ref_WriteByUInt32(UInt512 u)
	{
		const int UIntCount = 64 / sizeof(uint);

		Span<uint> p = stackalloc uint[UIntCount];

		Unsafe.WriteUnaligned(ref Unsafe.Add(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(p)), 0), (uint)(u.Part0 >> 00));
		Unsafe.WriteUnaligned(ref Unsafe.Add(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(p)), 1), (uint)(u.Part0 >> 32));
		
		Unsafe.WriteUnaligned(ref Unsafe.Add(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(p)), 2), (uint)(u.Part1 >> 00));
		Unsafe.WriteUnaligned(ref Unsafe.Add(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(p)), 3), (uint)(u.Part1 >> 32));
		
		Unsafe.WriteUnaligned(ref Unsafe.Add(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(p)), 4), (uint)(u.Part2 >> 00));
		Unsafe.WriteUnaligned(ref Unsafe.Add(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(p)), 5), (uint)(u.Part2 >> 32));
		
		Unsafe.WriteUnaligned(ref Unsafe.Add(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(p)), 6), (uint)(u.Part3 >> 00));
		Unsafe.WriteUnaligned(ref Unsafe.Add(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(p)), 7), (uint)(u.Part3 >> 32));
		
		Unsafe.WriteUnaligned(ref Unsafe.Add(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(p)), 8), (uint)(u.Part4 >> 00));
		Unsafe.WriteUnaligned(ref Unsafe.Add(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(p)), 9), (uint)(u.Part4 >> 32));
		
		Unsafe.WriteUnaligned(ref Unsafe.Add(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(p)), 10), (uint)(u.Part5 >> 00));
		Unsafe.WriteUnaligned(ref Unsafe.Add(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(p)), 11), (uint)(u.Part5 >> 32));
		
		Unsafe.WriteUnaligned(ref Unsafe.Add(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(p)), 12), (uint)(u.Part6 >> 00));
		Unsafe.WriteUnaligned(ref Unsafe.Add(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(p)), 13), (uint)(u.Part6 >> 32));
		
		Unsafe.WriteUnaligned(ref Unsafe.Add(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(p)), 14), (uint)(u.Part7 >> 00));
		Unsafe.WriteUnaligned(ref Unsafe.Add(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(p)), 15), (uint)(u.Part7 >> 32));

		return GetItem(p[..((UIntCount) - (BitHelper.LeadingZeroCount(in u) / 32))]);
	}
	private uint GetItem(ReadOnlySpan<uint> span)
	{
		return span.IsEmpty ? span[_index] : 0;
	}
	public IEnumerable<UInt256> UInt256ParamsSource()
	{
		yield return (UInt256)uint.MaxValue;
		yield return (UInt256)ulong.MaxValue;
		yield return (UInt256)UInt128.MaxValue;
		yield return (UInt256)UInt256.MaxValue;
	}
	public IEnumerable<UInt512> UInt512ParamsSource()
	{
		yield return (UInt512)uint.MaxValue;
		yield return (UInt512)ulong.MaxValue;
		yield return (UInt512)UInt128.MaxValue;
		yield return (UInt512)UInt256.MaxValue;
		yield return (UInt512)UInt512.MaxValue;
	}
}

public class ReadUnalignedBenchmarks
{
	private uint[] _array = Enumerable.Range(-2147483648, 64 / sizeof(uint)).Select(v => unchecked((uint)v)).ToArray();

	[Benchmark]
	public UInt256 UInt256_ReadUnaligned()
	{
		return Unsafe.ReadUnaligned<UInt256>(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetArrayDataReference(_array)));
	}
	[Benchmark]
	public UInt256 UInt256_UnsafeAs()
	{
		return Unsafe.As<uint, UInt256>(ref MemoryMarshal.GetArrayDataReference(_array));
	}
	[Benchmark]
	public UInt256 UInt256_Ctor_UseArray()
	{
		return new UInt256(
			((ulong)_array[7] << 32) | _array[6], ((ulong)_array[5] << 32) | _array[4], ((ulong)_array[3] << 32) | _array[2], ((ulong)_array[1] << 32) | _array[0]
			);
	}
	[Benchmark]
	public UInt256 UInt256_Ctor_UseSpan()
	{
		ReadOnlySpan<uint> span = _array;
		return new UInt256(
			((ulong)span[7] << 32) | span[6], ((ulong)span[5] << 32) | span[4], ((ulong)span[3] << 32) | span[2], ((ulong)span[1] << 32) | span[0]
			);
	}
	[Benchmark]
	public UInt256 UInt256_Ctor_UseRef()
	{
		ref uint ptr = ref MemoryMarshal.GetArrayDataReference(_array);
		return new UInt256(
			((ulong)Unsafe.Add(ref ptr, 7) << 32) | Unsafe.Add(ref ptr, 6),
			((ulong)Unsafe.Add(ref ptr, 5) << 32) | Unsafe.Add(ref ptr, 4),
			((ulong)Unsafe.Add(ref ptr, 3) << 32) | Unsafe.Add(ref ptr, 2),
			((ulong)Unsafe.Add(ref ptr, 1) << 32) | Unsafe.Add(ref ptr, 0)
			);
	}

	[Benchmark]
	public UInt512 UInt512_ReadUnaligned()
	{
		return Unsafe.ReadUnaligned<UInt512>(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetArrayDataReference(_array)));
	}
	[Benchmark]
	public UInt512 UInt512_UnsafeAs()
	{
		return Unsafe.As<uint, UInt512>(ref MemoryMarshal.GetArrayDataReference(_array));
	}
	[Benchmark]
	public UInt512 UInt512_Ctor_UseArray()
	{
		return new UInt512(
			((ulong)_array[15] << 32) | _array[14], ((ulong)_array[13] << 32) | _array[12], ((ulong)_array[11] << 32) | _array[10], ((ulong)_array[9] << 32) | _array[8],
			((ulong)_array[7] << 32) | _array[6], ((ulong)_array[5] << 32) | _array[4], ((ulong)_array[3] << 32) | _array[2], ((ulong)_array[1] << 32) | _array[0]
			);
	}
	[Benchmark]
	public UInt512 UInt512_Ctor_UseSpan()
	{
		ReadOnlySpan<uint> span = _array;
		return new UInt512(
			((ulong)span[15] << 32) | span[14], ((ulong)span[13] << 32) | span[12], ((ulong)span[11] << 32) | span[10], ((ulong)span[9] << 32) | span[8],
			((ulong)span[7] << 32) | span[6], ((ulong)span[5] << 32) | span[4], ((ulong)span[3] << 32) | span[2], ((ulong)span[1] << 32) | span[0]
			);
	}
	[Benchmark]
	public UInt512 UInt512_Ctor_UseRef()
	{
		ref uint ptr = ref MemoryMarshal.GetArrayDataReference(_array);
		return new UInt512(
			((ulong)Unsafe.Add(ref ptr, 15) << 32) | Unsafe.Add(ref ptr, 14),
			((ulong)Unsafe.Add(ref ptr, 13) << 32) | Unsafe.Add(ref ptr, 12),
			((ulong)Unsafe.Add(ref ptr, 11) << 32) | Unsafe.Add(ref ptr, 10),
			((ulong)Unsafe.Add(ref ptr, 9) << 32) | Unsafe.Add(ref ptr, 8),
			((ulong)Unsafe.Add(ref ptr, 7) << 32) | Unsafe.Add(ref ptr, 6),
			((ulong)Unsafe.Add(ref ptr, 5) << 32) | Unsafe.Add(ref ptr, 4),
			((ulong)Unsafe.Add(ref ptr, 3) << 32) | Unsafe.Add(ref ptr, 2),
			((ulong)Unsafe.Add(ref ptr, 1) << 32) | Unsafe.Add(ref ptr, 0)
			);
	}
}