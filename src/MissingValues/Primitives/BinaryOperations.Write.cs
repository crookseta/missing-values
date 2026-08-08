using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MissingValues.Primitives;

public static partial class BinaryOperations
{
    /// <summary>
    /// Returns the specified quadruple-precision floating-point value as an array of bytes.
    /// </summary>
    /// <param name="value">The number to convert.</param>
    /// <returns>An array of bytes with length 16.</returns>
    public static byte[] GetBytes(Quad value)
    {
        byte[] destination = new byte[16];
        TryWriteBytes<Quad>((Span<byte>) destination, value);
        return destination;
    }
    
    /// <summary>
    /// Returns the specified octuple-precision floating-point value as an array of bytes.
    /// </summary>
    /// <param name="value">The number to convert.</param>
    /// <returns>An array of bytes with length 16.</returns>
    public static byte[] GetBytes(Octo value)
    {
        byte[] destination = new byte[16];
        TryWriteBytes<Octo>((Span<byte>) destination, value);
        return destination;
    }
    
    /// <summary>
    /// Returns the specified 256-bit unsigned integer value as an array of bytes.
    /// </summary>
    /// <param name="value">The number to convert.</param>
    /// <returns>An array of bytes with length 32.</returns>
    public static byte[] GetBytes(UInt256 value)
    {
        byte[] destination = new byte[UInt256.Size];
        TryWriteBytes<UInt256>((Span<byte>) destination, value);
        return destination;
    }
    
    /// <summary>
    /// Returns the specified 256-bit integer value as an array of bytes.
    /// </summary>
    /// <param name="value">The number to convert.</param>
    /// <returns>An array of bytes with length 32.</returns>
    public static byte[] GetBytes(Int256 value)
    {
        byte[] destination = new byte[Int256.Size];
        TryWriteBytes<Int256>((Span<byte>) destination, value);
        return destination;
    }
    
    /// <summary>
    /// Returns the specified 512-bit unsigned integer value as an array of bytes.
    /// </summary>
    /// <param name="value">The number to convert.</param>
    /// <returns>An array of bytes with length 64.</returns>
    public static byte[] GetBytes(UInt512 value)
    {
        byte[] destination = new byte[UInt512.Size];
        TryWriteBytes<UInt512>((Span<byte>) destination, value);
        return destination;
    }
    
    /// <summary>
    /// Returns the specified 512-bit integer value as an array of bytes.
    /// </summary>
    /// <param name="value">The number to convert.</param>
    /// <returns>An array of bytes with length 64.</returns>
    public static byte[] GetBytes(Int512 value)
    {
        byte[] destination = new byte[Int512.Size];
        TryWriteBytes<Int512>((Span<byte>) destination, value);
        return destination;
    }
    
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
    /// Converts a quadruple-precision floating-point value into a span of bytes.
    /// </summary>
    /// <param name="destination">When this method returns, the bytes representing the converted quadruple-precision floating-point value.</param>
    /// <param name="value">The quadruple-precision floating-point value to convert.</param>
    /// <returns><see langword="true" /> if the conversion was successful; <see langword="false" /> otherwise.</returns>
    public static bool TryWriteBytes(Span<byte> destination, Quad value) => TryWriteBytes<Quad>(destination, value);

    /// <summary>
    /// Converts an octuple-precision floating-point value into a span of bytes.
    /// </summary>
    /// <param name="destination">When this method returns, the bytes representing the converted octuple-precision floating-point value.</param>
    /// <param name="value">The octuple-precision floating-point value to convert.</param>
    /// <returns><see langword="true" /> if the conversion was successful; <see langword="false" /> otherwise.</returns>
    public static bool TryWriteBytes(Span<byte> destination, Octo value) => TryWriteBytes<Octo>(destination, value);
    
    /// <summary>
    /// Converts a 256-bit unsigned integer into a span of bytes.
    /// </summary>
    /// <param name="destination">When this method returns, the bytes representing the converted 256-bit unsigned integer.</param>
    /// <param name="value">The 256-bit unsigned integer to convert.</param>
    /// <returns><see langword="true" /> if the conversion was successful; <see langword="false" /> otherwise.</returns>
    public static bool TryWriteBytes(Span<byte> destination, UInt256 value) => TryWriteBytes<UInt256>(destination, value);

    /// <summary>
    /// Converts a 256-bit integer into a span of bytes.
    /// </summary>
    /// <param name="destination">When this method returns, the bytes representing the converted 256-bit integer.</param>
    /// <param name="value">The 256-bit integer to convert.</param>
    /// <returns><see langword="true" /> if the conversion was successful; <see langword="false" /> otherwise.</returns>
    public static bool TryWriteBytes(Span<byte> destination, Int256 value) => TryWriteBytes<Int256>(destination, value);

    /// <summary>
    /// Converts a 512-bit unsigned integer into a span of bytes.
    /// </summary>
    /// <param name="destination">When this method returns, the bytes representing the converted 512-bit unsigned integer.</param>
    /// <param name="value">The 512-bit unsigned integer to convert.</param>
    /// <returns><see langword="true" /> if the conversion was successful; <see langword="false" /> otherwise.</returns>
    public static bool TryWriteBytes(Span<byte> destination, UInt512 value) => TryWriteBytes<UInt512>(destination, value);

    /// <summary>
    /// Converts a 512-bit integer into a span of bytes.
    /// </summary>
    /// <param name="destination">When this method returns, the bytes representing the converted 512-bit integer.</param>
    /// <param name="value">The 512-bit integer to convert.</param>
    /// <returns><see langword="true" /> if the conversion was successful; <see langword="false" /> otherwise.</returns>
    public static bool TryWriteBytes(Span<byte> destination, Int512 value) => TryWriteBytes<Int512>(destination, value);
    
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
    
    private static bool TryWriteBytes<T>(Span<byte> destination, T value)
        where T : unmanaged
    {
        if (destination.Length < Unsafe.SizeOf<T>())
            return false;
        Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(destination), value);
        return true;
    }
}