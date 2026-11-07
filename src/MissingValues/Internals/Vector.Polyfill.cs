using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

namespace MissingValues.Internals;

internal static class Vector128Extensions
{
	extension(Vector128)
	{
#if !NET11_0_OR_GREATER
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static Vector128<sbyte> CreateAlternatingSequence(sbyte even, sbyte odd)
		{
			return Vector128.Create(
				even, odd, even, odd, even, odd, even, odd,
				even, odd, even, odd, even, odd, even, odd
			);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static Vector128<short> CreateAlternatingSequence(short even, short odd)
		{
			return Vector128.Create(
				even, odd, even, odd, even, odd, even, odd
			);
		}
#endif
	}
}
internal static class Vector256Extensions
{
	extension(Vector256)
	{
#if !NET11_0_OR_GREATER
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static Vector256<byte> Reverse(Vector256<byte> vector)
		{
			return Vector256.Shuffle(
				vector,
				Vector256.Create((byte)31, 30, 29, 28, 27, 26, 25, 24, 23, 22, 21, 20, 19, 18, 17, 16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0)
			);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static Vector256<sbyte> CreateAlternatingSequence(sbyte even, sbyte odd)
		{
			return Vector256.Create(
				even, odd, even, odd, even, odd, even, odd,
				even, odd, even, odd, even, odd, even, odd,
				even, odd, even, odd, even, odd, even, odd,
				even, odd, even, odd, even, odd, even, odd
			);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static Vector256<short> CreateAlternatingSequence(short even, short odd)
		{
			return Vector256.Create(
				even, odd, even, odd, even, odd, even, odd,
				even, odd, even, odd, even, odd, even, odd
			);
		}
#endif
	}
}
internal static class Vector512Extensions
{
	extension(Vector512)
	{
#if !NET11_0_OR_GREATER
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static Vector512<byte> Reverse(Vector512<byte> vector)
		{
			return Vector512.Shuffle(
				vector,
				Vector512.Create(
					(byte)63, 62, 61, 60, 59, 58, 57, 56, 55, 54, 53, 52, 51, 50, 49, 48, 47, 46, 45, 44, 43, 42, 41, 40, 39, 38, 37, 36, 35, 34, 33, 32,
					31, 30, 29, 28, 27, 26, 25, 24, 23, 22, 21, 20, 19, 18, 17, 16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0
				)
			);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static Vector512<sbyte> CreateAlternatingSequence(sbyte even, sbyte odd)
		{
			return Vector512.Create(
				even, odd, even, odd, even, odd, even, odd,
				even, odd, even, odd, even, odd, even, odd,
				even, odd, even, odd, even, odd, even, odd,
				even, odd, even, odd, even, odd, even, odd,
				even, odd, even, odd, even, odd, even, odd,
				even, odd, even, odd, even, odd, even, odd,
				even, odd, even, odd, even, odd, even, odd,
				even, odd, even, odd, even, odd, even, odd
			);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static Vector512<short> CreateAlternatingSequence(short even, short odd)
		{
			return Vector512.Create(
				even, odd, even, odd, even, odd, even, odd,
				even, odd, even, odd, even, odd, even, odd,
				even, odd, even, odd, even, odd, even, odd,
				even, odd, even, odd, even, odd, even, odd
			);
		}
#endif
	}
}