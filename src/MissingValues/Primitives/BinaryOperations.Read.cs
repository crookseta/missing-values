using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MissingValues.Internals;

namespace MissingValues.Primitives;

public static partial class BinaryOperations
{
    /// <summary>
    /// Returns a quadruple-precision floating-point value converted from 16 bytes at a specified position in a byte array.
    /// </summary>
    /// <param name="value">An array of bytes.</param>
    /// <param name="startIndex">The starting position within <paramref name="value"/>.</param>
    /// <returns>A quadruple-precision floating-point value formed by 16 bytes beginning at <paramref name="startIndex"/>.</returns>
    public static Quad ToQuad(byte[] value, int startIndex) => To<Quad>(value, startIndex);
    /// <summary>
    /// Converts a read-only byte span into a quadruple-precision floating-point value.
    /// </summary>
    /// <param name="value">A read-only span containing the bytes to convert.</param>
    /// <returns>A quadruple-precision floating-point value representing the converted bytes.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad ToQuad(ReadOnlySpan<byte> value) => To<Quad>(value);
    
    /// <summary>
    /// Returns an octuple-precision floating-point value converted from 32 bytes at a specified position in a byte array.
    /// </summary>
    /// <param name="value">An array of bytes.</param>
    /// <param name="startIndex">The starting position within <paramref name="value"/>.</param>
    /// <returns>An octuple-precision floating-point value formed by 32 bytes beginning at <paramref name="startIndex"/>.</returns>
    public static Octo ToOcto(byte[] value, int startIndex) => To<Octo>(value, startIndex);
    /// <summary>
    /// Converts a read-only byte span into an octuple-precision floating-point value.
    /// </summary>
    /// <param name="value">A read-only span containing the bytes to convert.</param>
    /// <returns>An octuple-precision floating-point value representing the converted bytes.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Octo ToOcto(ReadOnlySpan<byte> value) => To<Octo>(value);
    
    /// <summary>
    /// Returns a 256-bit unsigned integer converted from 32 bytes at a specified position in a byte array.
    /// </summary>
    /// <param name="value">An array of bytes.</param>
    /// <param name="startIndex">The starting position within <paramref name="value"/>.</param>
    /// <returns>A 256-bit unsigned integer formed by 32 bytes beginning at <paramref name="startIndex"/>.</returns>
    public static UInt256 ToUInt256(byte[] value, int startIndex) => To<UInt256>(value, startIndex);
    /// <summary>
    /// Converts a read-only byte span into a 256-bit unsigned integer.
    /// </summary>
    /// <param name="value">A read-only span containing the bytes to convert.</param>
    /// <returns>A 256-bit unsigned integer representing the converted bytes.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static UInt256 ToUInt256(ReadOnlySpan<byte> value) => To<UInt256>(value);
    
    /// <summary>
    /// Returns a 256-bit integer converted from 32 bytes at a specified position in a byte array.
    /// </summary>
    /// <param name="value">An array of bytes.</param>
    /// <param name="startIndex">The starting position within <paramref name="value"/>.</param>
    /// <returns>A 256-bit integer formed by 32 bytes beginning at <paramref name="startIndex"/>.</returns>
    public static Int256 ToInt256(byte[] value, int startIndex) => To<Int256>(value, startIndex);
    /// <summary>
    /// Converts a read-only byte span into a 256-bit integer.
    /// </summary>
    /// <param name="value">A read-only span containing the bytes to convert.</param>
    /// <returns>A 256-bit integer representing the converted bytes.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Int256 ToInt256(ReadOnlySpan<byte> value) => To<Int256>(value);
    
    /// <summary>
    /// Returns a 512-bit unsigned integer converted from 64 bytes at a specified position in a byte array.
    /// </summary>
    /// <param name="value">An array of bytes.</param>
    /// <param name="startIndex">The starting position within <paramref name="value"/>.</param>
    /// <returns>A 512-bit unsigned integer formed by 64 bytes beginning at <paramref name="startIndex"/>.</returns>
    public static UInt512 ToUInt512(byte[] value, int startIndex) => To<UInt512>(value, startIndex);
    /// <summary>
    /// Converts a read-only byte span into a 512-bit unsigned integer.
    /// </summary>
    /// <param name="value">A read-only span containing the bytes to convert.</param>
    /// <returns>A 512-bit unsigned integer representing the converted bytes.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static UInt512 ToUInt512(ReadOnlySpan<byte> value) => To<UInt512>(value);
    
    /// <summary>
    /// Returns a 512-bit integer converted from 64 bytes at a specified position in a byte array.
    /// </summary>
    /// <param name="value">An array of bytes.</param>
    /// <param name="startIndex">The starting position within <paramref name="value"/>.</param>
    /// <returns>A 512-bit integer formed by 64 bytes beginning at <paramref name="startIndex"/>.</returns>
    public static Int512 ToInt512(byte[] value, int startIndex) => To<Int512>(value, startIndex);
    /// <summary>
    /// Converts a read-only byte span into a 512-bit integer.
    /// </summary>
    /// <param name="value">A read-only span containing the bytes to convert.</param>
    /// <returns>A 512-bit integer representing the converted bytes.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Int512 ToInt512(ReadOnlySpan<byte> value) => To<Int512>(value);

    /// <summary>
    /// Reads a <see cref="Quad"/> from the beginning of a read-only span of bytes, as big endian.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>The big endian value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">source is too small to contain a <see cref="Quad"/></exception>
    /// <remarks>Reads exactly 16 bytes from the beginning of the span.</remarks>
    public static Quad ReadQuadBigEndian(ReadOnlySpan<byte> source)
    {
        if (BitConverter.IsLittleEndian)
        {
            UInt128 value = MemoryMarshal.Read<UInt128>(source);
            return UInt128BitsToQuad(BinaryPrimitives.ReverseEndianness(value));
        }

        return MemoryMarshal.Read<Quad>(source);
    }
    
    /// <summary>
    /// Reads a <see cref="Octo"/> from the beginning of a read-only span of bytes, as big endian.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>The big endian value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">source is too small to contain a <see cref="Octo"/></exception>
    /// <remarks>Reads exactly 32 bytes from the beginning of the span.</remarks>
    public static Octo ReadOctoBigEndian(ReadOnlySpan<byte> source)
    {
        if (BitConverter.IsLittleEndian)
        {
            UInt256 value = MemoryMarshal.Read<UInt256>(source);
            return UInt256BitsToOcto(ReverseEndianness(in value));
        }

        return MemoryMarshal.Read<Octo>(source);
    }
    
    /// <summary>
    /// Reads a <see cref="UInt256"/> from the beginning of a read-only span of bytes, as big endian.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>The big endian value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">source is too small to contain a <see cref="UInt256"/></exception>
    /// <remarks>Reads exactly 32 bytes from the beginning of the span.</remarks>
    public static UInt256 ReadUInt256BigEndian(ReadOnlySpan<byte> source)
    {
        UInt256 value = MemoryMarshal.Read<UInt256>(source);
        return BitConverter.IsLittleEndian
            ? ReverseEndianness(in value)
            : value;
    }
    
    /// <summary>
    /// Reads a <see cref="Int256"/> from the beginning of a read-only span of bytes, as big endian.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>The big endian value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">source is too small to contain a <see cref="Int256"/></exception>
    /// <remarks>Reads exactly 32 bytes from the beginning of the span.</remarks>
    public static Int256 ReadInt256BigEndian(ReadOnlySpan<byte> source)
    {
        Int256 value = MemoryMarshal.Read<Int256>(source);
        return BitConverter.IsLittleEndian
            ? ReverseEndianness(in value)
            : value;
    }
    
    /// <summary>
    /// Reads a <see cref="UInt512"/> from the beginning of a read-only span of bytes, as big endian.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>The big endian value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">source is too small to contain a <see cref="UInt512"/></exception>
    /// <remarks>Reads exactly 64 bytes from the beginning of the span.</remarks>
    public static UInt512 ReadUInt512BigEndian(ReadOnlySpan<byte> source)
    {
        UInt512 value = MemoryMarshal.Read<UInt512>(source);
        return BitConverter.IsLittleEndian
            ? ReverseEndianness(in value)
            : value;
    }
    
    /// <summary>
    /// Reads a <see cref="Int512"/> from the beginning of a read-only span of bytes, as big endian.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>The big endian value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">source is too small to contain a <see cref="Int512"/></exception>
    /// <remarks>Reads exactly 64 bytes from the beginning of the span.</remarks>
    public static Int512 ReadInt512BigEndian(ReadOnlySpan<byte> source)
    {
        Int512 value = MemoryMarshal.Read<Int512>(source);
        return BitConverter.IsLittleEndian
            ? ReverseEndianness(in value)
            : value;
    }
    
    /// <summary>
    /// Reads a <see cref="Quad"/> from the beginning of a read-only span of bytes, as little endian.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>The little endian value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">source is too small to contain a <see cref="Quad"/></exception>
    /// <remarks>Reads exactly 16 bytes from the beginning of the span.</remarks>
    public static Quad ReadQuadLittleEndian(ReadOnlySpan<byte> source)
    {
        if (!BitConverter.IsLittleEndian)
        {
            UInt128 value = MemoryMarshal.Read<UInt128>(source);
            return UInt128BitsToQuad(BinaryPrimitives.ReverseEndianness(value));
        }
        else
        {
            return MemoryMarshal.Read<Quad>(source);
        }
    }
    
    /// <summary>
    /// Reads a <see cref="Octo"/> from the beginning of a read-only span of bytes, as little endian.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>The little endian value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">source is too small to contain a <see cref="Octo"/></exception>
    /// <remarks>Reads exactly 32 bytes from the beginning of the span.</remarks>
    public static Octo ReadOctoLittleEndian(ReadOnlySpan<byte> source)
    {
        if (!BitConverter.IsLittleEndian)
        {
            UInt256 value = MemoryMarshal.Read<UInt256>(source);
            return UInt256BitsToOcto(ReverseEndianness(in value));
        }
        else
        {
            return MemoryMarshal.Read<Octo>(source);
        }
    }
    
    /// <summary>
    /// Reads a <see cref="UInt256"/> from the beginning of a read-only span of bytes, as little endian.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>The little endian value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">source is too small to contain a <see cref="UInt256"/></exception>
    /// <remarks>Reads exactly 32 bytes from the beginning of the span.</remarks>
    public static UInt256 ReadUInt256LittleEndian(ReadOnlySpan<byte> source)
    {
        UInt256 value = MemoryMarshal.Read<UInt256>(source);
        return !BitConverter.IsLittleEndian
            ? ReverseEndianness(in value)
            : value;
    }
    
    /// <summary>
    /// Reads a <see cref="Int256"/> from the beginning of a read-only span of bytes, as little endian.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>The little endian value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">source is too small to contain a <see cref="Int256"/></exception>
    /// <remarks>Reads exactly 32 bytes from the beginning of the span.</remarks>
    public static Int256 ReadInt256LittleEndian(ReadOnlySpan<byte> source)
    {
        Int256 value = MemoryMarshal.Read<Int256>(source);
        return !BitConverter.IsLittleEndian
            ? ReverseEndianness(in value)
            : value;
    }
    
    /// <summary>
    /// Reads a <see cref="UInt512"/> from the beginning of a read-only span of bytes, as little endian.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>The little endian value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">source is too small to contain a <see cref="UInt512"/></exception>
    /// <remarks>Reads exactly 64 bytes from the beginning of the span.</remarks>
    public static UInt512 ReadUInt512LittleEndian(ReadOnlySpan<byte> source)
    {
        UInt512 value = MemoryMarshal.Read<UInt512>(source);
        return !BitConverter.IsLittleEndian
            ? ReverseEndianness(in value)
            : value;
    }
    
    /// <summary>
    /// Reads a <see cref="Int512"/> from the beginning of a read-only span of bytes, as little endian.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <returns>The little endian value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">source is too small to contain a <see cref="Int512"/></exception>
    /// <remarks>Reads exactly 64 bytes from the beginning of the span.</remarks>
    public static Int512 ReadInt512LittleEndian(ReadOnlySpan<byte> source)
    {
        Int512 value = MemoryMarshal.Read<Int512>(source);
        return !BitConverter.IsLittleEndian
            ? ReverseEndianness(in value)
            : value;
    }
    
    /// <summary>
    /// Reads a <see cref="Quad"/> from the beginning of a read-only span of bytes, as big endian.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <param name="value">When this method returns, contains the value read out of the read-only span of bytes, as big endian.</param>
    /// <returns>true if the span is large enough to contain a <see cref="Quad"/>; otherwise, false.</returns>
    /// <remarks>Reads exactly 16 bytes from the beginning of the span.</remarks>
    public static bool TryReadQuadBigEndian(ReadOnlySpan<byte> source, out Quad value)
    {
        if (BitConverter.IsLittleEndian)
        {
            bool success = MemoryMarshal.TryRead(source, out UInt128 tmp);
            value = UInt128BitsToQuad(BinaryPrimitives.ReverseEndianness(tmp));
            return success;
        }

        return MemoryMarshal.TryRead(source, out value);
    }
    
    /// <summary>
    /// Reads a <see cref="Octo"/> from the beginning of a read-only span of bytes, as big endian.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <param name="value">When this method returns, contains the value read out of the read-only span of bytes, as big endian.</param>
    /// <returns>true if the span is large enough to contain a <see cref="Octo"/>; otherwise, false.</returns>
    /// <remarks>Reads exactly 32 bytes from the beginning of the span.</remarks>
    public static bool TryReadOctoBigEndian(ReadOnlySpan<byte> source, out Octo value)
    {
        if (BitConverter.IsLittleEndian)
        {
            bool success = MemoryMarshal.TryRead(source, out UInt256 tmp);
            value = UInt256BitsToOcto(ReverseEndianness(in tmp));
            return success;
        }

        return MemoryMarshal.TryRead(source, out value);
    }
    
    /// <summary>
    /// Reads a <see cref="UInt256"/> from the beginning of a read-only span of bytes, as big endian.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <param name="value">When this method returns, contains the value read out of the read-only span of bytes, as big endian.</param>
    /// <returns>true if the span is large enough to contain a <see cref="UInt256"/>; otherwise, false.</returns>
    /// <remarks>Reads exactly 32 bytes from the beginning of the span.</remarks>
    public static bool TryReadUInt256BigEndian(ReadOnlySpan<byte> source, out UInt256 value)
    {
        if (BitConverter.IsLittleEndian)
        {
            bool success = MemoryMarshal.TryRead(source, out UInt256 tmp);
            value = ReverseEndianness(in tmp);
            return success;
        }

        return MemoryMarshal.TryRead(source, out value);
    }
    
    /// <summary>
    /// Reads a <see cref="Int256"/> from the beginning of a read-only span of bytes, as big endian.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <param name="value">When this method returns, contains the value read out of the read-only span of bytes, as big endian.</param>
    /// <returns>true if the span is large enough to contain a <see cref="Int256"/>; otherwise, false.</returns>
    /// <remarks>Reads exactly 32 bytes from the beginning of the span.</remarks>
    public static bool TryReadInt256BigEndian(ReadOnlySpan<byte> source, out Int256 value)
    {
        if (BitConverter.IsLittleEndian)
        {
            bool success = MemoryMarshal.TryRead(source, out Int256 tmp);
            value = ReverseEndianness(in tmp);
            return success;
        }

        return MemoryMarshal.TryRead(source, out value);
    }
    
    /// <summary>
    /// Reads a <see cref="UInt512"/> from the beginning of a read-only span of bytes, as big endian.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <param name="value">When this method returns, contains the value read out of the read-only span of bytes, as big endian.</param>
    /// <returns>true if the span is large enough to contain a <see cref="UInt512"/>; otherwise, false.</returns>
    /// <remarks>Reads exactly 64 bytes from the beginning of the span.</remarks>
    public static bool TryReadUInt512BigEndian(ReadOnlySpan<byte> source, out UInt512 value)
    {
        if (BitConverter.IsLittleEndian)
        {
            bool success = MemoryMarshal.TryRead(source, out UInt512 tmp);
            value = ReverseEndianness(in tmp);
            return success;
        }

        return MemoryMarshal.TryRead(source, out value);
    }
    
    /// <summary>
    /// Reads a <see cref="Int512"/> from the beginning of a read-only span of bytes, as big endian.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <param name="value">When this method returns, contains the value read out of the read-only span of bytes, as big endian.</param>
    /// <returns>true if the span is large enough to contain a <see cref="Int512"/>; otherwise, false.</returns>
    /// <remarks>Reads exactly 64 bytes from the beginning of the span.</remarks>
    public static bool TryReadInt512BigEndian(ReadOnlySpan<byte> source, out Int512 value)
    {
        if (BitConverter.IsLittleEndian)
        {
            bool success = MemoryMarshal.TryRead(source, out Int512 tmp);
            value = ReverseEndianness(in tmp);
            return success;
        }

        return MemoryMarshal.TryRead(source, out value);
    }
    
    /// <summary>
    /// Reads a <see cref="Quad"/> from the beginning of a read-only span of bytes, as little endian.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <param name="value">When this method returns, contains the value read out of the read-only span of bytes, as little endian.</param>
    /// <returns>true if the span is large enough to contain a <see cref="Quad"/>; otherwise, false.</returns>
    /// <remarks>Reads exactly 16 bytes from the beginning of the span.</remarks>
    public static bool TryReadQuadLittleEndian(ReadOnlySpan<byte> source, out Quad value)
    {
        if (!BitConverter.IsLittleEndian)
        {
            bool success = MemoryMarshal.TryRead(source, out UInt128 tmp);
            value = UInt128BitsToQuad(BinaryPrimitives.ReverseEndianness(tmp));
            return success;
        }

        return MemoryMarshal.TryRead(source, out value);
    }
    
    /// <summary>
    /// Reads a <see cref="Octo"/> from the beginning of a read-only span of bytes, as little endian.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <param name="value">When this method returns, contains the value read out of the read-only span of bytes, as little endian.</param>
    /// <returns>true if the span is large enough to contain a <see cref="Octo"/>; otherwise, false.</returns>
    /// <remarks>Reads exactly 32 bytes from the beginning of the span.</remarks>
    public static bool TryReadOctoLittleEndian(ReadOnlySpan<byte> source, out Octo value)
    {
        if (!BitConverter.IsLittleEndian)
        {
            bool success = MemoryMarshal.TryRead(source, out UInt256 tmp);
            value = UInt256BitsToOcto(ReverseEndianness(in tmp));
            return success;
        }

        return MemoryMarshal.TryRead(source, out value);
    }
    
    /// <summary>
    /// Reads a <see cref="UInt256"/> from the beginning of a read-only span of bytes, as little endian.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <param name="value">When this method returns, contains the value read out of the read-only span of bytes, as little endian.</param>
    /// <returns>true if the span is large enough to contain a <see cref="UInt256"/>; otherwise, false.</returns>
    /// <remarks>Reads exactly 32 bytes from the beginning of the span.</remarks>
    public static bool TryReadUInt256LittleEndian(ReadOnlySpan<byte> source, out UInt256 value)
    {
        if (BitConverter.IsLittleEndian)
        {
            return MemoryMarshal.TryRead(source, out value);
        }

        bool success = MemoryMarshal.TryRead(source, out UInt256 tmp);
        value = ReverseEndianness(in tmp);
        return success;
    }
    
    /// <summary>
    /// Reads a <see cref="Int256"/> from the beginning of a read-only span of bytes, as little endian.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <param name="value">When this method returns, contains the value read out of the read-only span of bytes, as little endian.</param>
    /// <returns>true if the span is large enough to contain a <see cref="Int256"/>; otherwise, false.</returns>
    /// <remarks>Reads exactly 32 bytes from the beginning of the span.</remarks>
    public static bool TryReadInt256LittleEndian(ReadOnlySpan<byte> source, out Int256 value)
    {
        if (BitConverter.IsLittleEndian)
        {
            return MemoryMarshal.TryRead(source, out value);
        }

        bool success = MemoryMarshal.TryRead(source, out Int256 tmp);
        value = ReverseEndianness(in tmp);
        return success;
    }
    
    /// <summary>
    /// Reads a <see cref="UInt512"/> from the beginning of a read-only span of bytes, as little endian.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <param name="value">When this method returns, contains the value read out of the read-only span of bytes, as little endian.</param>
    /// <returns>true if the span is large enough to contain a <see cref="UInt512"/>; otherwise, false.</returns>
    /// <remarks>Reads exactly 64 bytes from the beginning of the span.</remarks>
    public static bool TryReadUInt512LittleEndian(ReadOnlySpan<byte> source, out UInt512 value)
    {
        if (BitConverter.IsLittleEndian)
        {
            return MemoryMarshal.TryRead(source, out value);
        }

        bool success = MemoryMarshal.TryRead(source, out UInt512 tmp);
        value = ReverseEndianness(in tmp);
        return success;
    }
    
    /// <summary>
    /// Reads a <see cref="Int512"/> from the beginning of a read-only span of bytes, as little endian.
    /// </summary>
    /// <param name="source">The read-only span to read.</param>
    /// <param name="value">When this method returns, contains the value read out of the read-only span of bytes, as little endian.</param>
    /// <returns>true if the span is large enough to contain a <see cref="Int512"/>; otherwise, false.</returns>
    /// <remarks>Reads exactly 64 bytes from the beginning of the span.</remarks>
    public static bool TryReadInt512LittleEndian(ReadOnlySpan<byte> source, out Int512 value)
    {
        if (BitConverter.IsLittleEndian)
        {
            return MemoryMarshal.TryRead(source, out value);
        }

        bool success = MemoryMarshal.TryRead(source, out Int512 tmp);
        value = ReverseEndianness(in tmp);
        return success;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static T To<T>(byte[] value, int startIndex)
        where T : unmanaged
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual((uint)startIndex, (uint)value.Length);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(startIndex, value.Length - 16);
        
        return Unsafe.ReadUnaligned<T>(ref value[startIndex]);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static T To<T>(ReadOnlySpan<byte> value)
        where T : unmanaged
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(value.Length, Unsafe.SizeOf<T>());
        return Unsafe.ReadUnaligned<T>(ref MemoryMarshal.GetReference(value));
    }
}