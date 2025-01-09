using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MissingValues.Internals
{
	internal sealed class IntDebugView<T>
		where T : unmanaged, IBigInteger<T>
	{
		private readonly UInt64Wrapper[] _array;

		public IntDebugView(T integer)
		{
			_array = MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<T, UInt64Wrapper>(ref integer), Unsafe.SizeOf<T>() / sizeof(ulong)).ToArray();
		}

		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public UInt64Wrapper[] Segments => _array;

		[DebuggerDisplay($"{{{nameof(ToString)}(),nq}}")]
		public readonly struct UInt64Wrapper
		{
			private readonly ulong _value;

			public override string ToString()
			{
				return _value.ToString("X16");
			}
		}
	}
}
