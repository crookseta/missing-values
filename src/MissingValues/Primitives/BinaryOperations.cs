using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

namespace MissingValues.Primitives;

/// <summary>
/// Provides methods for reading and writing bytes as <see cref="MissingValues"/> primitive types.
/// </summary>
public static partial class BinaryOperations
{
    /// <summary>
    /// Reinterprets the specified 128-bit unsigned integer to a quadruple-precision floating point number.
    /// </summary>
    /// <param name="bits">The number to convert.</param>
    /// <returns>A quadruple-precision floating point number whose bits are identical to <paramref name="bits"/>.</returns>º
    public static Quad UInt128BitsToQuad(UInt128 bits) => Unsafe.BitCast<UInt128, Quad>(bits);
    
    /// <summary>
    /// Reinterprets the specified 128-bit signed integer to a quadruple-precision floating point number.
    /// </summary>
    /// <param name="bits">The number to convert.</param>
    /// <returns>A quadruple-precision floating point number whose bits are identical to <paramref name="bits"/>.</returns>
    public static Quad Int128BitsToQuad(Int128 bits) => Unsafe.BitCast<Int128, Quad>(bits);

    /// <summary>
    /// Converts the specified quadruple-precision floating point number to a 128-bit unsigned integer.
    /// </summary>
    /// <param name="value">The number to convert.</param>
    /// <returns>A 128-bit unsigned integer whose value is equivalent to <paramref name="value"/>.</returns>
    public static UInt128 QuadToUInt128Bits(Quad value) => new UInt128(value._upper, value._lower);
    
    /// <summary>
    /// Converts the specified quadruple-precision floating point number to a 128-bit signed integer.
    /// </summary>
    /// <param name="value">The number to convert.</param>
    /// <returns>A 128-bit signed integer whose value is equivalent to <paramref name="value"/>.</returns>
    public static Int128 QuadToInt128Bits(Quad value) => new Int128(value._upper, value._lower);
    
    /// <summary>
    /// Reinterprets the specified 256-bit unsigned integer to an octuple-precision floating point number.
    /// </summary>
    /// <param name="bits">The number to convert.</param>
    /// <returns>An octuple-precision floating point number whose bits are identical to <paramref name="bits"/>.</returns>
    public static Octo UInt256BitsToOcto(UInt256 bits) => Unsafe.BitCast<UInt256, Octo>(bits);
    
    /// <summary>
    /// Reinterprets the specified 256-bit signed integer to an octuple-precision floating point number.
    /// </summary>
    /// <param name="bits">The number to convert.</param>
    /// <returns>An octuple-precision floating point number whose bits are identical to <paramref name="bits"/>.</returns>
    public static Octo Int256BitsToOcto(Int256 bits) => Unsafe.BitCast<Int256, Octo>(bits);
    
    /// <summary>
    /// Converts the specified octuple-precision floating point number to a 256-bit unsigned integer.
    /// </summary>
    /// <param name="value">The number to convert.</param>
    /// <returns>A 256-bit unsigned integer whose value is equivalent to <paramref name="value"/>.</returns>
    public static UInt256 OctoToUInt256Bits(Octo value) => Unsafe.BitCast<Octo, UInt256>(value);
    
    /// <summary>
    /// Converts the specified octuple-precision floating point number to a 256-bit signed integer.
    /// </summary>
    /// <param name="value">The number to convert.</param>
    /// <returns>A 256-bit signed integer whose value is equivalent to <paramref name="value"/>.</returns>
    public static Int256 OctoToInt256Bits(Octo value) => Unsafe.BitCast<Octo, Int256>(value);
    
    /// <summary>
    /// Reverses a primitive value by performing an endianness swap of the specified <see cref="UInt256"/> value.
    /// </summary>
    /// <param name="value">The value to reverse.</param>
    /// <returns>The reversed value.</returns>
    public static UInt256 ReverseEndianness(in UInt256 value)
    {
        if (Vector256.IsHardwareAccelerated)
        {
#if NET11_0_OR_GREATER
            return UInt256.Create(Vector256.Reverse(value.AsVector<byte>()));
#else
            return Unsafe.BitCast<Vector256<byte>, UInt256>(
                Vector256.Shuffle(
                    Unsafe.BitCast<UInt256, Vector256<byte>>(value),
                    Vector256.Create((byte)31, 30, 29, 28, 27, 26, 25, 24, 23, 22, 21, 20, 19, 18, 17, 16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0)
                )
            );
#endif
        }
        return new UInt256(BinaryPrimitives.ReverseEndianness(value.Part0), BinaryPrimitives.ReverseEndianness(value.Part1), BinaryPrimitives.ReverseEndianness(value.Part2), BinaryPrimitives.ReverseEndianness(value.Part3));
    }
    
    /// <summary>
    /// Reverses a primitive value by performing an endianness swap of the specified <see cref="Int256"/> value.
    /// </summary>
    /// <param name="value">The value to reverse.</param>
    /// <returns>The reversed value.</returns>
    public static Int256 ReverseEndianness(in Int256 value)
    {
        if (Vector256.IsHardwareAccelerated)
        {
#if NET11_0_OR_GREATER
            return Int256.Create(Vector256.Reverse(value.AsVector<byte>()));
#else
            return Unsafe.BitCast<Vector256<byte>, Int256>(
                Vector256.Shuffle(
                    Unsafe.BitCast<Int256, Vector256<byte>>(value),
                    Vector256.Create((byte)31, 30, 29, 28, 27, 26, 25, 24, 23, 22, 21, 20, 19, 18, 17, 16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0)
                )
            );
#endif
        }
        return new Int256(BinaryPrimitives.ReverseEndianness(value.Part0), BinaryPrimitives.ReverseEndianness(value.Part1), BinaryPrimitives.ReverseEndianness(value.Part2), BinaryPrimitives.ReverseEndianness(value.Part3));
    }
    
    /// <summary>
    /// Reverses a primitive value by performing an endianness swap of the specified <see cref="UInt512"/> value.
    /// </summary>
    /// <param name="value">The value to reverse.</param>
    /// <returns>The reversed value.</returns>
    public static UInt512 ReverseEndianness(in UInt512 value)
    {
        if (Vector512.IsHardwareAccelerated)
        {
#if NET11_0_OR_GREATER
            return UInt512.Create(Vector512.Reverse(value.AsVector<byte>()));
#else
            return Unsafe.BitCast<Vector512<byte>, UInt512>(
                Vector512.Shuffle(
                    Unsafe.BitCast<UInt512, Vector512<byte>>(value),
                    Vector512.Create(
                        (byte)63, 62, 61, 60, 59, 58, 57, 56, 55, 54, 53, 52, 51, 50, 49, 48, 47, 46, 45, 44, 43, 42, 41, 40, 39, 38, 37, 36, 35, 34, 33, 32,
                        31, 30, 29, 28, 27, 26, 25, 24, 23, 22, 21, 20, 19, 18, 17, 16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0
                    )
                )
            );
#endif
        }
        if (Vector256.IsHardwareAccelerated)
        {
            return new UInt512(ReverseEndianness(value.Lower), ReverseEndianness(value.Upper));
        }
        return new UInt512(
            BinaryPrimitives.ReverseEndianness(value.Part0), BinaryPrimitives.ReverseEndianness(value.Part1), BinaryPrimitives.ReverseEndianness(value.Part2), BinaryPrimitives.ReverseEndianness(value.Part3),
            BinaryPrimitives.ReverseEndianness(value.Part4), BinaryPrimitives.ReverseEndianness(value.Part5), BinaryPrimitives.ReverseEndianness(value.Part6), BinaryPrimitives.ReverseEndianness(value.Part7)
        );
    }
    
    /// <summary>
    /// Reverses a primitive value by performing an endianness swap of the specified <see cref="Int512"/> value.
    /// </summary>
    /// <param name="value">The value to reverse.</param>
    /// <returns>The reversed value.</returns>
    public static Int512 ReverseEndianness(in Int512 value)
    {
        if (Vector512.IsHardwareAccelerated)
        {
#if NET11_0_OR_GREATER
            return Int512.Create(Vector512.Reverse(value.AsVector<byte>()));
#else
            return Unsafe.BitCast<Vector512<byte>, Int512>(
                Vector512.Shuffle(
                    Unsafe.BitCast<Int512, Vector512<byte>>(value),
                    Vector512.Create(
                        (byte)63, 62, 61, 60, 59, 58, 57, 56, 55, 54, 53, 52, 51, 50, 49, 48, 47, 46, 45, 44, 43, 42, 41, 40, 39, 38, 37, 36, 35, 34, 33, 32,
                        31, 30, 29, 28, 27, 26, 25, 24, 23, 22, 21, 20, 19, 18, 17, 16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0
                    )
                )
            );
#endif
        }
        if (Vector256.IsHardwareAccelerated)
        {
            return new Int512(ReverseEndianness(value.Lower), ReverseEndianness(value.Upper));
        }
        return new Int512(
            BinaryPrimitives.ReverseEndianness(value.Part0), BinaryPrimitives.ReverseEndianness(value.Part1), BinaryPrimitives.ReverseEndianness(value.Part2), BinaryPrimitives.ReverseEndianness(value.Part3),
            BinaryPrimitives.ReverseEndianness(value.Part4), BinaryPrimitives.ReverseEndianness(value.Part5), BinaryPrimitives.ReverseEndianness(value.Part6), BinaryPrimitives.ReverseEndianness(value.Part7)
        );
    }

    /// <summary>
    /// Copies every value from <paramref name="source"/> to <paramref name="destination"/>, reversing each primitive by performing an endianness swap as part of writing each.
    /// </summary>
    /// <param name="source">The source span to copy.</param>
    /// <param name="destination">The destination to which the source elements should be copied.</param>
    /// <exception cref="ArgumentOutOfRangeException">The <paramref name="destination"/>'s length is smaller than that of the <paramref name="source"/>.</exception>
    public static void ReverseEndianness(ReadOnlySpan<UInt256> source, Span<UInt256> destination)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(destination.Length, source.Length);
        if (Unsafe.AreSame(in source[0], in destination[0]) || !source.Overlaps(destination, out int elementOffset) || elementOffset < 0)
        {
            int i = 0;

            if (Vector256.IsHardwareAccelerated)
            {
                var vectorSource = MemoryMarshal.Cast<UInt256, Vector256<byte>>(source);
                var vectorDestination = MemoryMarshal.Cast<UInt256, Vector256<byte>>(destination);

                for (; i < vectorSource.Length; i++)
                {
                    vectorDestination[i] = Vector256.Shuffle(
                        vectorSource[i],
                        Vector256.Create(
                            (byte)31, 30, 29, 28, 27, 26, 25, 24, 23, 22, 21, 20, 19, 18, 17, 16, 
                            15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0)
                    );
                }
            }
            else
            {
                for (; i < source.Length; i++)
                {
                    UInt256 temp = source[i];
                    destination[i] = new UInt256(
                        BinaryPrimitives.ReverseEndianness(temp.Part0), BinaryPrimitives.ReverseEndianness(temp.Part1),
                        BinaryPrimitives.ReverseEndianness(temp.Part2), BinaryPrimitives.ReverseEndianness(temp.Part3)
                    );
                }
            }
        }
        else
        {
            int i = source.Length - 1;
            
            if (Vector256.IsHardwareAccelerated)
            {
                var vectorSource = MemoryMarshal.Cast<UInt256, Vector256<byte>>(source);
                var vectorDestination = MemoryMarshal.Cast<UInt256, Vector256<byte>>(destination);

                for (; i >= 0; i--)
                {
                    vectorDestination[i] = Vector256.Shuffle(
                        vectorSource[i],
                        Vector256.Create(
                            (byte)31, 30, 29, 28, 27, 26, 25, 24, 23, 22, 21, 20, 19, 18, 17, 16, 
                            15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0)
                    );
                }
            }
            else
            {
                for (; i >= 0; i--)
                {
                    UInt256 temp = source[i];
                    destination[i] = new UInt256(
                        BinaryPrimitives.ReverseEndianness(temp.Part0), BinaryPrimitives.ReverseEndianness(temp.Part1),
                        BinaryPrimitives.ReverseEndianness(temp.Part2), BinaryPrimitives.ReverseEndianness(temp.Part3)
                    );
                }
            }
        }
    }

    /// <summary>
    /// Copies every value from <paramref name="source"/> to <paramref name="destination"/>, reversing each primitive by performing an endianness swap as part of writing each.
    /// </summary>
    /// <param name="source">The source span to copy.</param>
    /// <param name="destination">The destination to which the source elements should be copied.</param>
    /// <exception cref="ArgumentOutOfRangeException">The <paramref name="destination"/>'s length is smaller than that of the <paramref name="source"/>.</exception>
    public static void ReverseEndianness(ReadOnlySpan<Int256> source, Span<Int256> destination)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(destination.Length, source.Length);
        if (Unsafe.AreSame(in source[0], in destination[0]) || !source.Overlaps(destination, out int elementOffset) || elementOffset < 0)
        {
            int i = 0;

            if (Vector256.IsHardwareAccelerated)
            {
                var vectorSource = MemoryMarshal.Cast<Int256, Vector256<byte>>(source);
                var vectorDestination = MemoryMarshal.Cast<Int256, Vector256<byte>>(destination);

                for (; i < vectorSource.Length; i++)
                {
                    vectorDestination[i] = Vector256.Shuffle(
                        vectorSource[i],
                        Vector256.Create(
                            (byte)31, 30, 29, 28, 27, 26, 25, 24, 23, 22, 21, 20, 19, 18, 17, 16, 
                            15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0)
                    );
                }
            }
            else
            {
                for (; i < source.Length; i++)
                {
                    Int256 temp = source[i];
                    destination[i] = new Int256(
                        BinaryPrimitives.ReverseEndianness(temp.Part0), BinaryPrimitives.ReverseEndianness(temp.Part1),
                        BinaryPrimitives.ReverseEndianness(temp.Part2), BinaryPrimitives.ReverseEndianness(temp.Part3)
                    );
                }
            }
        }
        else
        {
            int i = source.Length - 1;
            
            if (Vector256.IsHardwareAccelerated)
            {
                var vectorSource = MemoryMarshal.Cast<Int256, Vector256<byte>>(source);
                var vectorDestination = MemoryMarshal.Cast<Int256, Vector256<byte>>(destination);

                for (; i >= 0; i--)
                {
                    vectorDestination[i] = Vector256.Shuffle(
                        vectorSource[i],
                        Vector256.Create(
                            (byte)31, 30, 29, 28, 27, 26, 25, 24, 23, 22, 21, 20, 19, 18, 17, 16, 
                            15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0)
                    );
                }
            }
            else
            {
                for (; i >= 0; i--)
                {
                    Int256 temp = source[i];
                    destination[i] = new Int256(
                        BinaryPrimitives.ReverseEndianness(temp.Part0), BinaryPrimitives.ReverseEndianness(temp.Part1),
                        BinaryPrimitives.ReverseEndianness(temp.Part2), BinaryPrimitives.ReverseEndianness(temp.Part3)
                    );
                }
            }
        }
    }

    /// <summary>
    /// Copies every value from <paramref name="source"/> to <paramref name="destination"/>, reversing each primitive by performing an endianness swap as part of writing each.
    /// </summary>
    /// <param name="source">The source span to copy.</param>
    /// <param name="destination">The destination to which the source elements should be copied.</param>
    /// <exception cref="ArgumentOutOfRangeException">The <paramref name="destination"/>'s length is smaller than that of the <paramref name="source"/>.</exception>
    public static void ReverseEndianness(ReadOnlySpan<UInt512> source, Span<UInt512> destination)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(destination.Length, source.Length);
        if (Unsafe.AreSame(in source[0], in destination[0]) || !source.Overlaps(destination, out int elementOffset) || elementOffset < 0)
        {
            int i = 0;

            if (Vector512.IsHardwareAccelerated)
            {
                var vectorSource = MemoryMarshal.Cast<UInt512, Vector512<byte>>(source);
                var vectorDestination = MemoryMarshal.Cast<UInt512, Vector512<byte>>(destination);

                for (; i < vectorSource.Length; i++)
                {
                    vectorDestination[i] = Vector512.Shuffle(
                        vectorSource[i],
                        Vector512.Create(
                            (byte)63, 62, 61, 60, 59, 58, 57, 56, 55, 54, 53, 52, 51, 50, 49, 48, 
                            47, 46, 45, 44, 43, 42, 41, 40, 39, 38, 37, 36, 35, 34, 33, 32,
                            31, 30, 29, 28, 27, 26, 25, 24, 23, 22, 21, 20, 19, 18, 17, 16, 
                            15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0)
                    );
                }
            }
            else
            {
                for (; i < source.Length; i++)
                {
                    UInt512 temp = source[i];
                    destination[i] = new UInt512(
                        BinaryPrimitives.ReverseEndianness(temp.Part0), BinaryPrimitives.ReverseEndianness(temp.Part1), BinaryPrimitives.ReverseEndianness(temp.Part2), BinaryPrimitives.ReverseEndianness(temp.Part3),
                        BinaryPrimitives.ReverseEndianness(temp.Part4), BinaryPrimitives.ReverseEndianness(temp.Part5), BinaryPrimitives.ReverseEndianness(temp.Part6), BinaryPrimitives.ReverseEndianness(temp.Part7)
                    );
                }
            }
        }
        else
        {
            int i = source.Length - 1;
            
            if (Vector512.IsHardwareAccelerated)
            {
                var vectorSource = MemoryMarshal.Cast<UInt512, Vector512<byte>>(source);
                var vectorDestination = MemoryMarshal.Cast<UInt512, Vector512<byte>>(destination);

                for (; i >= 0; i--)
                {
                    vectorDestination[i] = Vector512.Shuffle(
                        vectorSource[i],
                        Vector512.Create(
                            (byte)63, 62, 61, 60, 59, 58, 57, 56, 55, 54, 53, 52, 51, 50, 49, 48, 
                            47, 46, 45, 44, 43, 42, 41, 40, 39, 38, 37, 36, 35, 34, 33, 32,
                            31, 30, 29, 28, 27, 26, 25, 24, 23, 22, 21, 20, 19, 18, 17, 16, 
                            15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0)
                    );
                }
            }
            else
            {
                for (; i >= 0; i--)
                {
                    UInt512 temp = source[i];
                    destination[i] = new UInt512(
                        BinaryPrimitives.ReverseEndianness(temp.Part0), BinaryPrimitives.ReverseEndianness(temp.Part1), BinaryPrimitives.ReverseEndianness(temp.Part2), BinaryPrimitives.ReverseEndianness(temp.Part3),
                        BinaryPrimitives.ReverseEndianness(temp.Part4), BinaryPrimitives.ReverseEndianness(temp.Part5), BinaryPrimitives.ReverseEndianness(temp.Part6), BinaryPrimitives.ReverseEndianness(temp.Part7)
                    );
                }
            }
        }
    }

    /// <summary>
    /// Copies every value from <paramref name="source"/> to <paramref name="destination"/>, reversing each primitive by performing an endianness swap as part of writing each.
    /// </summary>
    /// <param name="source">The source span to copy.</param>
    /// <param name="destination">The destination to which the source elements should be copied.</param>
    /// <exception cref="ArgumentOutOfRangeException">The <paramref name="destination"/>'s length is smaller than that of the <paramref name="source"/>.</exception>
    public static void ReverseEndianness(ReadOnlySpan<Int512> source, Span<Int512> destination)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(destination.Length, source.Length);
        if (Unsafe.AreSame(in source[0], in destination[0]) || !source.Overlaps(destination, out int elementOffset) || elementOffset < 0)
        {
            int i = 0;

            if (Vector512.IsHardwareAccelerated)
            {
                var vectorSource = MemoryMarshal.Cast<Int512, Vector512<byte>>(source);
                var vectorDestination = MemoryMarshal.Cast<Int512, Vector512<byte>>(destination);

                for (; i < vectorSource.Length; i++)
                {
                    vectorDestination[i] = Vector512.Shuffle(
                        vectorSource[i],
                        Vector512.Create(
                            (byte)63, 62, 61, 60, 59, 58, 57, 56, 55, 54, 53, 52, 51, 50, 49, 48, 
                            47, 46, 45, 44, 43, 42, 41, 40, 39, 38, 37, 36, 35, 34, 33, 32,
                            31, 30, 29, 28, 27, 26, 25, 24, 23, 22, 21, 20, 19, 18, 17, 16, 
                            15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0)
                    );
                }
            }
            else
            {
                for (; i < source.Length; i++)
                {
                    Int512 temp = source[i];
                    destination[i] = new Int512(
                        BinaryPrimitives.ReverseEndianness(temp.Part0), BinaryPrimitives.ReverseEndianness(temp.Part1), BinaryPrimitives.ReverseEndianness(temp.Part2), BinaryPrimitives.ReverseEndianness(temp.Part3),
                        BinaryPrimitives.ReverseEndianness(temp.Part4), BinaryPrimitives.ReverseEndianness(temp.Part5), BinaryPrimitives.ReverseEndianness(temp.Part6), BinaryPrimitives.ReverseEndianness(temp.Part7)
                    );
                }
            }
        }
        else
        {
            int i = source.Length - 1;
            
            if (Vector512.IsHardwareAccelerated)
            {
                var vectorSource = MemoryMarshal.Cast<Int512, Vector512<byte>>(source);
                var vectorDestination = MemoryMarshal.Cast<Int512, Vector512<byte>>(destination);

                for (; i >= 0; i--)
                {
                    vectorDestination[i] = Vector512.Shuffle(
                        vectorSource[i],
                        Vector512.Create(
                            (byte)63, 62, 61, 60, 59, 58, 57, 56, 55, 54, 53, 52, 51, 50, 49, 48, 
                            47, 46, 45, 44, 43, 42, 41, 40, 39, 38, 37, 36, 35, 34, 33, 32,
                            31, 30, 29, 28, 27, 26, 25, 24, 23, 22, 21, 20, 19, 18, 17, 16, 
                            15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0)
                    );
                }
            }
            else
            {
                for (; i >= 0; i--)
                {
                    Int512 temp = source[i];
                    destination[i] = new Int512(
                        BinaryPrimitives.ReverseEndianness(temp.Part0), BinaryPrimitives.ReverseEndianness(temp.Part1), BinaryPrimitives.ReverseEndianness(temp.Part2), BinaryPrimitives.ReverseEndianness(temp.Part3),
                        BinaryPrimitives.ReverseEndianness(temp.Part4), BinaryPrimitives.ReverseEndianness(temp.Part5), BinaryPrimitives.ReverseEndianness(temp.Part6), BinaryPrimitives.ReverseEndianness(temp.Part7)
                    );
                }
            }
        }
    }
}