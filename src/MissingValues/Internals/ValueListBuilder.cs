using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues.Internals
{
	internal ref struct ValueListBuilder<T>
		where T : unmanaged, IEquatable<T>
	{
		private Chunk _items;
		private int _count;
		private const int MaxCapacity = 512;

        public readonly int Count => _count;

        public ValueListBuilder(scoped ReadOnlySpan<T> span)
        {
			_items = default;
			span.CopyTo(_items);
			_count = span.Length;
        }

		public ref T this[int index]
		{
			get
			{
				ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual((uint)index, (uint)_count);
				return ref Unsafe.Add(ref Unsafe.As<Chunk, T>(ref Unsafe.AsRef(in _items)), (nint)(uint)index);
			}
		}

		public void Add(T item)
		{
			_items[_count++] = item;
		}
		public void Add(ReadOnlySpan<T> items)
		{
			if (items.Length == 0)
			{
				_items[_count++] = items[0];
			}
			else
			{
				items.CopyTo(_items[_count..]);
				_count += items.Length;
			}
		}

		public Span<T> AsSpan()
		{
			return MemoryMarshal.CreateSpan(ref Unsafe.As<Chunk, T>(ref _items), _count);
		}

		public readonly bool Contains(T item)
		{
			return _items[.._count].IndexOf(item) >= 0;
		}
		
		public void Insert(int index, T item)
		{
			Span<T> temp = stackalloc T[_count - index];
			_items[index..temp.Length].CopyTo(temp);

			_items[index] = item;
			temp.CopyTo(_items[(index + 1)..]);
			_count++;
		}
		public void InsertRange(int index, ReadOnlySpan<T> items)
		{
			Span<T> temp = stackalloc T[_count - index];
			_items[index..temp.Length].CopyTo(temp);

			items.CopyTo(_items[index..]);
			temp.CopyTo(_items[(index + items.Length)..]);
			_count += items.Length;
		}

		public readonly void CopyTo(Span<T> destination)
		{
			_items[.._count].CopyTo(destination);
		}

		public ref T GetFirstReference()
		{
			return ref Unsafe.As<Chunk, T>(ref Unsafe.AsRef(in _items));
		}

		[InlineArray(MaxCapacity)]
		private struct Chunk
		{
			private T _0;
		}
	}
}
