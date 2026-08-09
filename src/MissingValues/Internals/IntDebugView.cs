using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MissingValues.Internals
{
	internal sealed class IntDebugView<T>
		where T : unmanaged, IBigInteger<T>
	{
		private readonly UInt64Wrapper[] _array;

		internal IntDebugView(T integer)
		{
			_array = MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<T, UInt64Wrapper>(ref integer), Unsafe.SizeOf<T>() / sizeof(ulong)).ToArray();
		}

		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		internal UInt64Wrapper[] Segments => _array;

		[DebuggerDisplay($"{{{nameof(Display)}(),nq}}")]
		internal readonly struct UInt64Wrapper
		{
			private readonly ulong _value;

			internal string Display()
			{
				return _value.ToString("X16");
			}
		}
	}
}
