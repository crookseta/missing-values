using System.Buffers.Binary;
using System.Runtime.InteropServices;
using MissingValues.Internals;

namespace MissingValues.Primitives;

public static partial class BinaryOperations
{
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
}