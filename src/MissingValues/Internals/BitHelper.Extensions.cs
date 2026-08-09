using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MissingValues.Internals
{
	internal static partial class BitHelper
	{
		extension<TChar>(ref ValueListBuilder<TChar> builder)
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal void AppendUtf16(ReadOnlySpan<char> source)
			{
				Span<TChar> span = builder.AppendSpan(TChar.GetLength(source));
				TChar.Copy(source, span);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal void AppendUtf8(ReadOnlySpan<byte> source)
			{
				Span<TChar> span = builder.AppendSpan(TChar.GetLength(source));
				TChar.Copy(source, span);
			}
		}
		extension(UInt128 uInt128)
		{
			internal ulong Upper
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get => GetUpper(in uInt128);
			}

			internal ulong Lower
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get => GetLower(in uInt128);
			}

			internal static int CountDigits(UInt128 value)
			{
				var x = value | UInt128.One;
				int num1 = (int)UInt128.Log2(x) + 1;
				int num2 = (num1 * 1233) >> 12;
				return (x < Unsafe.Add(ref Unsafe.As<ulong, UInt512>(ref MemoryMarshal.GetReference(Tables.Pow10Table)), num2) ? num2 - 1 : num2) + 1;
			}
		}
		extension(Int128 int128)
		{
			internal ulong Upper
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get => GetUpper(in int128);
			}

			internal ulong Lower
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get => GetLower(in int128);
			}
		}
		
		internal static void GetUpperAndLowerBits(UInt128 value, out ulong upper, out ulong lower)
		{
			lower = value.Lower;
			upper = value.Upper;
		}
		internal static void GetUpperAndLowerBits(Int128 value, out ulong upper, out ulong lower)
		{
			lower = value.Lower;
			upper = value.Upper;
		}
		
		[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_upper")]
		private static extern ref ulong GetUpper(in UInt128 value);
		[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_lower")]
		private static extern ref ulong GetLower(in UInt128 value);
		
		[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_upper")]
		private static extern ref ulong GetUpper(in Int128 value);
		[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_lower")]
		private static extern ref ulong GetLower(in Int128 value);

		internal static T DefaultConvert<T>(out bool result)
		{
			result = false;
			return default;
		}
	}
}
