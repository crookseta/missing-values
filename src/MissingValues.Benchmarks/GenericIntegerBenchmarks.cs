using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using MissingValues.Internals;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MissingValues.Benchmarks.Helpers;

namespace MissingValues.Benchmarks
{
	[SimpleJob(RuntimeMoniker.Net80)]
	[SimpleJob(RuntimeMoniker.Net90)]
	[HideColumns("Job", "Error", "StdDev")]
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

	[ShortRunJob]
	[HideColumns("Job", "Error", "StdDev")]
	[GenericTypeArguments(typeof(Int256))]
	[GenericTypeArguments(typeof(UInt256))]
	[GenericTypeArguments(typeof(Int512))]
	[GenericTypeArguments(typeof(UInt512))]
	[GenericTypeArguments(typeof(Quad))]
	[GenericTypeArguments(typeof(Octo))]
	public class GenericCastingBenchmarks<T>
		where T : INumber<T>
	{
		private T _value;
		
		[GlobalSetup]
		public void Setup()
		{
			_value = Randomize();
		}

		[Benchmark]
		public T FromByte()
		{
			return T.CreateTruncating(byte.MaxValue);
		}
		
		[Benchmark]
		public T FromUInt16()
		{
			return T.CreateTruncating(ushort.MaxValue);
		}
		
		[Benchmark]
		public T FromUInt32()
		{
			return T.CreateTruncating(uint.MaxValue);
		}
		
		[Benchmark]
		public T FromUInt64()
		{
			return T.CreateTruncating(ulong.MaxValue);
		}
		
		[Benchmark]
		public T FromUInt128()
		{
			return T.CreateTruncating(UInt128.MaxValue);
		}
		
		[Benchmark]
		public T FromUInt256()
		{
			return T.CreateTruncating(UInt256.MaxValue);
		}
		
		[Benchmark]
		public T FromUInt512()
		{
			return T.CreateTruncating(UInt512.MaxValue);
		}
		
		[Benchmark]
		public T FromSByte()
		{
			return T.CreateTruncating(sbyte.MaxValue);
		}
		
		[Benchmark]
		public T FromInt16()
		{
			return T.CreateTruncating(short.MaxValue);
		}
		
		[Benchmark]
		public T FromInt32()
		{
			return T.CreateTruncating(int.MaxValue);
		}
		
		[Benchmark]
		public T FromInt64()
		{
			return T.CreateTruncating(long.MaxValue);
		}
		
		[Benchmark]
		public T FromInt128()
		{
			return T.CreateTruncating(Int128.MaxValue);
		}
		
		[Benchmark]
		public T FromInt256()
		{
			return T.CreateTruncating(Int256.MaxValue);
		}
		
		[Benchmark]
		public T FromInt512()
		{
			return T.CreateTruncating(Int512.MaxValue);
		}
		
		[Benchmark]
		public T FromHalf()
		{
			return T.CreateTruncating(Half.One);
		}
		
		[Benchmark]
		public T FromSingle()
		{
			return T.CreateTruncating(1f);
		}
		
		[Benchmark]
		public T FromDouble()
		{
			return T.CreateTruncating(1d);
		}
		
		[Benchmark]
		public T FromQuad()
		{
			return T.CreateTruncating(Quad.One);
		}
		
		[Benchmark]
		public T FromOcto()
		{
			return T.CreateTruncating(Octo.One);
		}
		
		[Benchmark]
		public byte ToByte()
		{
			return byte.CreateTruncating(_value);
		}
		
		[Benchmark]
		public ushort ToUInt16()
		{
			return ushort.CreateTruncating(_value);
		}
		
		[Benchmark]
		public uint ToUInt32()
		{
			return uint.CreateTruncating(_value);
		}
		
		[Benchmark]
		public ulong ToUInt64()
		{
			return ulong.CreateTruncating(_value);
		}
		[Benchmark]
		public UInt128 ToUInt128()
		{
			return UInt128.CreateTruncating(_value);
		}
		
		[Benchmark]
		public UInt256 ToUInt256()
		{
			return UInt256.CreateTruncating(_value);
		}
		
		[Benchmark]
		public UInt512 ToUInt512()
		{
			return UInt512.CreateTruncating(_value);
		}

		[Benchmark]
		public sbyte ToSByte()
		{
			return sbyte.CreateTruncating(_value);
		}
		
		[Benchmark]
		public short ToInt16()
		{
			return short.CreateTruncating(_value);
		}
		
		[Benchmark]
		public int ToInt32()
		{
			return int.CreateTruncating(_value);
		}
		
		[Benchmark]
		public long ToInt64()
		{
			return long.CreateTruncating(_value);
		}
		[Benchmark]
		public Int128 ToInt128()
		{
			return Int128.CreateTruncating(_value);
		}
		
		[Benchmark]
		public Int256 ToInt256()
		{
			return Int256.CreateTruncating(_value);
		}
		
		[Benchmark]
		public Int512 ToInt512()
		{
			return Int512.CreateTruncating(_value);
		}

		[Benchmark]
		public Half ToHalf()
		{
			return Half.CreateTruncating(_value);
		}

		[Benchmark]
		public float ToSingle()
		{
			return float.CreateTruncating(_value);
		}

		[Benchmark]
		public double ToDouble()
		{
			return double.CreateTruncating(_value);
		}
		
		[Benchmark]
		public Quad ToQuad()
		{
			return Quad.CreateTruncating(_value);
		}

		[Benchmark]
		public Octo ToOcto()
		{
			return Octo.CreateTruncating(_value);
		}

		private static T Randomize()
		{
			scoped Span<byte> bytes;
			if (T.IsNegative(-T.One))
			{
				if (T.One / (T.One + T.One) != T.Zero)
				{
					return T.CreateTruncating(Random.Shared.NextDouble() * 100);
				}
				else
				{
					bytes = stackalloc byte[Unsafe.SizeOf<T>()];
					Random.Shared.NextBytes(bytes);

					return Unsafe.ReadUnaligned<T>(ref MemoryMarshal.GetReference(bytes));
				}
			}
			else
			{
				bytes = stackalloc byte[Unsafe.SizeOf<T>()];
				Random.Shared.NextBytes(bytes);

				return Unsafe.ReadUnaligned<T>(ref MemoryMarshal.GetReference(bytes));
			}
		}
	}
}
