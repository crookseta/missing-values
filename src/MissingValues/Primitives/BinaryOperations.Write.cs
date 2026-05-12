using System.Buffers.Binary;
using System.Runtime.InteropServices;

namespace MissingValues.Primitives;

public static partial class BinaryOperations
{
    /// <summary>
    /// Write a <see cref="Quad"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException">destination is too small to contain a <see cref="Quad"/></exception>
    /// <remarks>Writes exactly 16 bytes to the beginning of the span.</remarks>
    public static void WriteQuadBigEndian(Span<byte> destination, in Quad value)
    {
        if (BitConverter.IsLittleEndian)
        {
            UInt128 tmp = BinaryPrimitives.ReverseEndianness(QuadToUInt128Bits(value));
            MemoryMarshal.Write(destination, in tmp);
        }
        else
        {
            MemoryMarshal.Write(destination, in value);
        }
    }
    
    /// <summary>
    /// Write a <see cref="Octo"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException">destination is too small to contain a <see cref="Octo"/></exception>
    /// <remarks>Writes exactly 32 bytes to the beginning of the span.</remarks>
    public static void WriteOctoBigEndian(Span<byte> destination, in Octo value)
    {
        if (BitConverter.IsLittleEndian)
        {
            UInt256 tmp = ReverseEndianness(OctoToUInt256Bits(value));
            MemoryMarshal.Write(destination, in tmp);
        }
        else
        {
            MemoryMarshal.Write(destination, in value);
        }
    }
    
    /// <summary>
    /// Write a <see cref="UInt256"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException">destination is too small to contain a <see cref="UInt256"/></exception>
    /// <remarks>Writes exactly 32 bytes to the beginning of the span.</remarks>
    public static void WriteUInt256BigEndian(Span<byte> destination, in UInt256 value)
    {
        if (BitConverter.IsLittleEndian)
        {
            UInt256 tmp = ReverseEndianness(in value);
            MemoryMarshal.Write(destination, in tmp);
        }
        else
        {
            MemoryMarshal.Write(destination, in value);
        }
    }
    
    /// <summary>
    /// Write a <see cref="Int256"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException">destination is too small to contain a <see cref="Int256"/></exception>
    /// <remarks>Writes exactly 32 bytes to the beginning of the span.</remarks>
    public static void WriteInt256BigEndian(Span<byte> destination, in Int256 value)
    {
        if (BitConverter.IsLittleEndian)
        {
            Int256 tmp = ReverseEndianness(in value);
            MemoryMarshal.Write(destination, in tmp);
        }
        else
        {
            MemoryMarshal.Write(destination, in value);
        }
    }
    
    /// <summary>
    /// Write a <see cref="UInt512"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException">destination is too small to contain a <see cref="UInt512"/></exception>
    /// <remarks>Writes exactly 64 bytes to the beginning of the span.</remarks>
    public static void WriteUInt512BigEndian(Span<byte> destination, in UInt512 value)
    {
        if (BitConverter.IsLittleEndian)
        {
            UInt512 tmp = ReverseEndianness(in value);
            MemoryMarshal.Write(destination, in tmp);
        }
        else
        {
            MemoryMarshal.Write(destination, in value);
        }
    }
    
    /// <summary>
    /// Write a <see cref="Int512"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException">destination is too small to contain a <see cref="Int512"/></exception>
    /// <remarks>Writes exactly 64 bytes to the beginning of the span.</remarks>
    public static void WriteInt512BigEndian(Span<byte> destination, in Int512 value)
    {
        if (BitConverter.IsLittleEndian)
        {
            Int512 tmp = ReverseEndianness(in value);
            MemoryMarshal.Write(destination, in tmp);
        }
        else
        {
            MemoryMarshal.Write(destination, in value);
        }
    }
    
    /// <summary>
    /// Write a <see cref="Quad"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException">destination is too small to contain a <see cref="Quad"/></exception>
    /// <remarks>Writes exactly 16 bytes to the beginning of the span.</remarks>
    public static void WriteQuadLittleEndian(Span<byte> destination, in Quad value)
    {
        if (!BitConverter.IsLittleEndian)
        {
            UInt128 tmp = BinaryPrimitives.ReverseEndianness(QuadToUInt128Bits(value));
            MemoryMarshal.Write(destination, in tmp);
        }
        else
        {
            MemoryMarshal.Write(destination, in value);
        }
    }
    
    /// <summary>
    /// Write a <see cref="Octo"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException">destination is too small to contain a <see cref="Octo"/></exception>
    /// <remarks>Writes exactly 32 bytes to the beginning of the span.</remarks>
    public static void WriteOctoLittleEndian(Span<byte> destination, in Octo value)
    {
        if (!BitConverter.IsLittleEndian)
        {
            UInt256 tmp = ReverseEndianness(OctoToUInt256Bits(value));
            MemoryMarshal.Write(destination, in tmp);
        }
        else
        {
            MemoryMarshal.Write(destination, in value);
        }
    }
    
    /// <summary>
    /// Write a <see cref="UInt256"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException">destination is too small to contain a <see cref="UInt256"/></exception>
    /// <remarks>Writes exactly 32 bytes to the beginning of the span.</remarks>
    public static void WriteUInt256LittleEndian(Span<byte> destination, in UInt256 value)
    {
        if (!BitConverter.IsLittleEndian)
        {
            UInt256 tmp = ReverseEndianness(in value);
            MemoryMarshal.Write(destination, in tmp);
        }
        else
        {
            MemoryMarshal.Write(destination, in value);
        }
    }
    
    /// <summary>
    /// Write a <see cref="Int256"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException">destination is too small to contain a <see cref="Int256"/></exception>
    /// <remarks>Writes exactly 32 bytes to the beginning of the span.</remarks>
    public static void WriteInt256LittleEndian(Span<byte> destination, in Int256 value)
    {
        if (!BitConverter.IsLittleEndian)
        {
            Int256 tmp = ReverseEndianness(in value);
            MemoryMarshal.Write(destination, in tmp);
        }
        else
        {
            MemoryMarshal.Write(destination, in value);
        }
    }
    
    /// <summary>
    /// Write a <see cref="UInt512"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException">destination is too small to contain a <see cref="UInt512"/></exception>
    /// <remarks>Writes exactly 64 bytes to the beginning of the span.</remarks>
    public static void WriteUInt512LittleEndian(Span<byte> destination, in UInt512 value)
    {
        if (!BitConverter.IsLittleEndian)
        {
            UInt512 tmp = ReverseEndianness(in value);
            MemoryMarshal.Write(destination, in tmp);
        }
        else
        {
            MemoryMarshal.Write(destination, in value);
        }
    }
    
    /// <summary>
    /// Write a <see cref="Int512"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException">destination is too small to contain a <see cref="Int512"/></exception>
    /// <remarks>Writes exactly 64 bytes to the beginning of the span.</remarks>
    public static void WriteInt512LittleEndian(Span<byte> destination, in Int512 value)
    {
        if (!BitConverter.IsLittleEndian)
        {
            Int512 tmp = ReverseEndianness(in value);
            MemoryMarshal.Write(destination, in tmp);
        }
        else
        {
            MemoryMarshal.Write(destination, in value);
        }
    }
    
    /// <summary>
    /// Write a <see cref="Quad"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <returns>true if the span is large enough to contain a <see cref="Quad"/>; otherwise, false.</returns>
    /// <remarks>Writes exactly 16 bytes to the beginning of the span.</remarks>
    public static bool TryWriteQuadBigEndian(Span<byte> destination, in Quad value)
    {
        if (BitConverter.IsLittleEndian)
        {
            UInt128 tmp = BinaryPrimitives.ReverseEndianness(QuadToUInt128Bits(value));
            return MemoryMarshal.TryWrite(destination, in tmp);
        }

        return MemoryMarshal.TryWrite(destination, in value);
    }
    
    /// <summary>
    /// Write a <see cref="Octo"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <returns>true if the span is large enough to contain a <see cref="Octo"/>; otherwise, false.</returns>
    /// <remarks>Writes exactly 32 bytes to the beginning of the span.</remarks>
    public static bool TryWriteOctoBigEndian(Span<byte> destination, in Octo value)
    {
        if (BitConverter.IsLittleEndian)
        {
            UInt256 tmp = ReverseEndianness(OctoToUInt256Bits(value));
            return MemoryMarshal.TryWrite(destination, in tmp);
        }

        return MemoryMarshal.TryWrite(destination, in value);
    }
    
    /// <summary>
    /// Write a <see cref="UInt256"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <returns>true if the span is large enough to contain a <see cref="UInt256"/>; otherwise, false.</returns>
    /// <remarks>Writes exactly 32 bytes to the beginning of the span.</remarks>
    public static bool TryWriteUInt256BigEndian(Span<byte> destination, in UInt256 value)
    {
        if (BitConverter.IsLittleEndian)
        {
            UInt256 tmp = ReverseEndianness(in value);
            return MemoryMarshal.TryWrite(destination, in tmp);
        }

        return MemoryMarshal.TryWrite(destination, in value);
    }
    
    /// <summary>
    /// Write a <see cref="Int256"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <returns>true if the span is large enough to contain a <see cref="Int256"/>; otherwise, false.</returns>
    /// <remarks>Writes exactly 32 bytes to the beginning of the span.</remarks>
    public static bool TryWriteInt256BigEndian(Span<byte> destination, in Int256 value)
    {
        if (BitConverter.IsLittleEndian)
        {
            Int256 tmp = ReverseEndianness(in value);
            return MemoryMarshal.TryWrite(destination, in tmp);
        }

        return MemoryMarshal.TryWrite(destination, in value);
    }
    
    /// <summary>
    /// Write a <see cref="UInt512"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <returns>true if the span is large enough to contain a <see cref="UInt512"/>; otherwise, false.</returns>
    /// <remarks>Writes exactly 64 bytes to the beginning of the span.</remarks>
    public static bool TryWriteUInt512BigEndian(Span<byte> destination, in UInt512 value)
    {
        if (BitConverter.IsLittleEndian)
        {
            UInt512 tmp = ReverseEndianness(in value);
            return MemoryMarshal.TryWrite(destination, in tmp);
        }

        return MemoryMarshal.TryWrite(destination, in value);
    }
    
    /// <summary>
    /// Write a <see cref="Int512"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <returns>true if the span is large enough to contain a <see cref="Int512"/>; otherwise, false.</returns>
    /// <remarks>Writes exactly 64 bytes to the beginning of the span.</remarks>
    public static bool TryWriteInt512BigEndian(Span<byte> destination, in Int512 value)
    {
        if (BitConverter.IsLittleEndian)
        {
            Int512 tmp = ReverseEndianness(in value);
            return MemoryMarshal.TryWrite(destination, in tmp);
        }

        return MemoryMarshal.TryWrite(destination, in value);
    }
    
    /// <summary>
    /// Write a <see cref="Quad"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <returns>true if the span is large enough to contain a <see cref="Quad"/>; otherwise, false.</returns>
    /// <remarks>Writes exactly 16 bytes to the beginning of the span.</remarks>
    public static bool TryWriteQuadLittleEndian(Span<byte> destination, in Quad value)
    {
        if (!BitConverter.IsLittleEndian)
        {
            UInt128 tmp = BinaryPrimitives.ReverseEndianness(QuadToUInt128Bits(value));
            return MemoryMarshal.TryWrite(destination, in tmp);
        }

        return MemoryMarshal.TryWrite(destination, in value);
    }
    
    /// <summary>
    /// Write a <see cref="Octo"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <returns>true if the span is large enough to contain a <see cref="Octo"/>; otherwise, false.</returns>
    /// <remarks>Writes exactly 32 bytes to the beginning of the span.</remarks>
    public static bool TryWriteOctoLittleEndian(Span<byte> destination, in Octo value)
    {
        if (!BitConverter.IsLittleEndian)
        {
            UInt256 tmp = ReverseEndianness(OctoToUInt256Bits(value));
            return MemoryMarshal.TryWrite(destination, in tmp);
        }

        return MemoryMarshal.TryWrite(destination, in value);
    }
    
    /// <summary>
    /// Write a <see cref="UInt256"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <returns>true if the span is large enough to contain a <see cref="UInt256"/>; otherwise, false.</returns>
    /// <remarks>Writes exactly 32 bytes to the beginning of the span.</remarks>
    public static bool TryWriteUInt256LittleEndian(Span<byte> destination, in UInt256 value)
    {
        if (!BitConverter.IsLittleEndian)
        {
            UInt256 tmp = ReverseEndianness(in value);
            return MemoryMarshal.TryWrite(destination, in tmp);
        }

        return MemoryMarshal.TryWrite(destination, in value);
    }
    
    /// <summary>
    /// Write a <see cref="Int256"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <returns>true if the span is large enough to contain a <see cref="Int256"/>; otherwise, false.</returns>
    /// <remarks>Writes exactly 32 bytes to the beginning of the span.</remarks>
    public static bool TryWriteInt256LittleEndian(Span<byte> destination, in Int256 value)
    {
        if (!BitConverter.IsLittleEndian)
        {
            Int256 tmp = ReverseEndianness(in value);
            return MemoryMarshal.TryWrite(destination, in tmp);
        }

        return MemoryMarshal.TryWrite(destination, in value);
    }
    
    /// <summary>
    /// Write a <see cref="UInt512"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <returns>true if the span is large enough to contain a <see cref="UInt512"/>; otherwise, false.</returns>
    /// <remarks>Writes exactly 64 bytes to the beginning of the span.</remarks>
    public static bool TryWriteUInt512LittleEndian(Span<byte> destination, in UInt512 value)
    {
        if (!BitConverter.IsLittleEndian)
        {
            UInt512 tmp = ReverseEndianness(in value);
            return MemoryMarshal.TryWrite(destination, in tmp);
        }

        return MemoryMarshal.TryWrite(destination, in value);
    }
    
    /// <summary>
    /// Write a <see cref="Int512"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <returns>true if the span is large enough to contain a <see cref="Int512"/>; otherwise, false.</returns>
    /// <remarks>Writes exactly 64 bytes to the beginning of the span.</remarks>
    public static bool TryWriteInt512LittleEndian(Span<byte> destination, in Int512 value)
    {
        if (!BitConverter.IsLittleEndian)
        {
            Int512 tmp = ReverseEndianness(in value);
            return MemoryMarshal.TryWrite(destination, in tmp);
        }

        return MemoryMarshal.TryWrite(destination, in value);
    }
}