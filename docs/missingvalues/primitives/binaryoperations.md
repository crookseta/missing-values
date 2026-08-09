[`< Back`](../../)

---

# BinaryOperations

Namespace: MissingValues.Primitives

Provides methods for reading and writing bytes as `MissingValues` primitive types.

```csharp
public static class BinaryOperations
```

Inheritance [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) → [BinaryOperations](./missingvalues/primitives/binaryoperations.md)

## Methods

### **UInt128BitsToQuad(UInt128)**

Reinterprets the specified 128-bit unsigned integer to a quadruple-precision floating point number.

```csharp
public static Quad UInt128BitsToQuad(UInt128 bits)
```

#### Parameters

`bits` [UInt128](https://learn.microsoft.com/en-us/dotnet/api/system.uint128)<br>
The number to convert.

#### Returns

[Quad](./missingvalues/quad.md)<br>
A quadruple-precision floating point number whose bits are identical to `bits`.

### **Int128BitsToQuad(Int128)**

Reinterprets the specified 128-bit signed integer to a quadruple-precision floating point number.

```csharp
public static Quad Int128BitsToQuad(Int128 bits)
```

#### Parameters

`bits` [Int128](https://learn.microsoft.com/en-us/dotnet/api/system.int128)<br>
The number to convert.

#### Returns

[Quad](./missingvalues/quad.md)<br>
A quadruple-precision floating point number whose bits are identical to `bits`.

### **QuadToUInt128Bits(Quad)**

Converts the specified quadruple-precision floating point number to a 128-bit unsigned integer.

```csharp
public static UInt128 QuadToUInt128Bits(Quad value)
```

#### Parameters

`value` [Quad](./missingvalues/quad.md)<br>
The number to convert.

#### Returns

[UInt128](https://learn.microsoft.com/en-us/dotnet/api/system.uint128)<br>
A 128-bit unsigned integer whose value is equivalent to `value`.

### **QuadToInt128Bits(Quad)**

Converts the specified quadruple-precision floating point number to a 128-bit signed integer.

```csharp
public static Int128 QuadToInt128Bits(Quad value)
```

#### Parameters

`value` [Quad](./missingvalues/quad.md)<br>
The number to convert.

#### Returns

[Int128](https://learn.microsoft.com/en-us/dotnet/api/system.int128)<br>
A 128-bit signed integer whose value is equivalent to `value`.

### **UInt256BitsToOcto(UInt256)**

Reinterprets the specified 256-bit unsigned integer to an octuple-precision floating point number.

```csharp
public static Octo UInt256BitsToOcto(UInt256 bits)
```

#### Parameters

`bits` [UInt256](./missingvalues/uint256.md)<br>
The number to convert.

#### Returns

[Octo](./missingvalues/octo.md)<br>
An octuple-precision floating point number whose bits are identical to `bits`.

### **Int256BitsToOcto(Int256)**

Reinterprets the specified 256-bit signed integer to an octuple-precision floating point number.

```csharp
public static Octo Int256BitsToOcto(Int256 bits)
```

#### Parameters

`bits` [Int256](./missingvalues/int256.md)<br>
The number to convert.

#### Returns

[Octo](./missingvalues/octo.md)<br>
An octuple-precision floating point number whose bits are identical to `bits`.

### **OctoToUInt256Bits(Octo)**

Converts the specified octuple-precision floating point number to a 256-bit unsigned integer.

```csharp
public static UInt256 OctoToUInt256Bits(Octo value)
```

#### Parameters

`value` [Octo](./missingvalues/octo.md)<br>
The number to convert.

#### Returns

[UInt256](./missingvalues/uint256.md)<br>
A 256-bit unsigned integer whose value is equivalent to `value`.

### **OctoToInt256Bits(Octo)**

Converts the specified octuple-precision floating point number to a 256-bit signed integer.

```csharp
public static Int256 OctoToInt256Bits(Octo value)
```

#### Parameters

`value` [Octo](./missingvalues/octo.md)<br>
The number to convert.

#### Returns

[Int256](./missingvalues/int256.md)<br>
A 256-bit signed integer whose value is equivalent to `value`.

### **ReverseEndianness(in UInt256)**

Reverses a primitive value by performing an endianness swap of the specified [UInt256](./missingvalues/uint256.md) value.

```csharp
public static UInt256 ReverseEndianness(in UInt256 value)
```

#### Parameters

`in` `value` [UInt256](./missingvalues/uint256.md)<br>
The value to reverse.

#### Returns

[UInt256](./missingvalues/uint256.md)<br>
The reversed value.

### **ReverseEndianness(in Int256)**

Reverses a primitive value by performing an endianness swap of the specified [Int256](./missingvalues/int256.md) value.

```csharp
public static Int256 ReverseEndianness(in Int256 value)
```

#### Parameters

`in` `value` [Int256](./missingvalues/int256.md)<br>
The value to reverse.

#### Returns

[Int256](./missingvalues/int256.md)<br>
The reversed value.

### **ReverseEndianness(in UInt512)**

Reverses a primitive value by performing an endianness swap of the specified [UInt512](./missingvalues/uint512.md) value.

```csharp
public static UInt512 ReverseEndianness(in UInt512 value)
```

#### Parameters

`in` `value` [UInt512](./missingvalues/uint512.md)<br>
The value to reverse.

#### Returns

[UInt512](./missingvalues/uint512.md)<br>
The reversed value.

### **ReverseEndianness(in Int512)**

Reverses a primitive value by performing an endianness swap of the specified [Int512](./missingvalues/int512.md) value.

```csharp
public static Int512 ReverseEndianness(in Int512 value)
```

#### Parameters

`in` `value` [Int512](./missingvalues/int512.md)<br>
The value to reverse.

#### Returns

[Int512](./missingvalues/int512.md)<br>
The reversed value.

### **ToQuad(Byte[], Int32)**

Returns a quadruple-precision floating-point value converted from 16 bytes at a specified position in a byte array.

```csharp
public static Quad ToQuad(Byte[] value, int startIndex)
```

#### Parameters

`value` [Byte[]](https://learn.microsoft.com/en-us/dotnet/api/system.byte)<br>
An array of bytes.

`startIndex` [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>
The starting position within `value`.

#### Returns

[Quad](./missingvalues/quad.md)<br>
A quadruple-precision floating-point value formed by 16 bytes beginning at `startIndex`.

### **ToQuad(ReadOnlySpan&lt;Byte&gt;)**

Converts a read-only byte span into a quadruple-precision floating-point value.

```csharp
public static Quad ToQuad(ReadOnlySpan<byte> value)
```

#### Parameters

`value` [ReadOnlySpan&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<br>
A read-only span containing the bytes to convert.

#### Returns

[Quad](./missingvalues/quad.md)<br>
A quadruple-precision floating-point value representing the converted bytes.

### **ToOcto(Byte[], Int32)**

Returns an octuple-precision floating-point value converted from 32 bytes at a specified position in a byte array.

```csharp
public static Octo ToOcto(Byte[] value, int startIndex)
```

#### Parameters

`value` [Byte[]](https://learn.microsoft.com/en-us/dotnet/api/system.byte)<br>
An array of bytes.

`startIndex` [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>
The starting position within `value`.

#### Returns

[Octo](./missingvalues/octo.md)<br>
An octuple-precision floating-point value formed by 32 bytes beginning at `startIndex`.

### **ToOcto(ReadOnlySpan&lt;Byte&gt;)**

Converts a read-only byte span into an octuple-precision floating-point value.

```csharp
public static Octo ToOcto(ReadOnlySpan<byte> value)
```

#### Parameters

`value` [ReadOnlySpan&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<br>
A read-only span containing the bytes to convert.

#### Returns

[Octo](./missingvalues/octo.md)<br>
An octuple-precision floating-point value representing the converted bytes.

### **ToUInt256(Byte[], Int32)**

Returns a 256-bit unsigned integer converted from 32 bytes at a specified position in a byte array.

```csharp
public static UInt256 ToUInt256(Byte[] value, int startIndex)
```

#### Parameters

`value` [Byte[]](https://learn.microsoft.com/en-us/dotnet/api/system.byte)<br>
An array of bytes.

`startIndex` [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>
The starting position within `value`.

#### Returns

[UInt256](./missingvalues/uint256.md)<br>
A 256-bit unsigned integer formed by 32 bytes beginning at `startIndex`.

### **ToUInt256(ReadOnlySpan&lt;Byte&gt;)**

Converts a read-only byte span into a 256-bit unsigned integer.

```csharp
public static UInt256 ToUInt256(ReadOnlySpan<byte> value)
```

#### Parameters

`value` [ReadOnlySpan&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<br>
A read-only span containing the bytes to convert.

#### Returns

[UInt256](./missingvalues/uint256.md)<br>
A 256-bit unsigned integer representing the converted bytes.

### **ToInt256(Byte[], Int32)**

Returns a 256-bit integer converted from 32 bytes at a specified position in a byte array.

```csharp
public static Int256 ToInt256(Byte[] value, int startIndex)
```

#### Parameters

`value` [Byte[]](https://learn.microsoft.com/en-us/dotnet/api/system.byte)<br>
An array of bytes.

`startIndex` [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>
The starting position within `value`.

#### Returns

[Int256](./missingvalues/int256.md)<br>
A 256-bit integer formed by 32 bytes beginning at `startIndex`.

### **ToInt256(ReadOnlySpan&lt;Byte&gt;)**

Converts a read-only byte span into a 256-bit integer.

```csharp
public static Int256 ToInt256(ReadOnlySpan<byte> value)
```

#### Parameters

`value` [ReadOnlySpan&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<br>
A read-only span containing the bytes to convert.

#### Returns

[Int256](./missingvalues/int256.md)<br>
A 256-bit integer representing the converted bytes.

### **ToUInt512(Byte[], Int32)**

Returns a 512-bit unsigned integer converted from 64 bytes at a specified position in a byte array.

```csharp
public static UInt512 ToUInt512(Byte[] value, int startIndex)
```

#### Parameters

`value` [Byte[]](https://learn.microsoft.com/en-us/dotnet/api/system.byte)<br>
An array of bytes.

`startIndex` [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>
The starting position within `value`.

#### Returns

[UInt512](./missingvalues/uint512.md)<br>
A 512-bit unsigned integer formed by 64 bytes beginning at `startIndex`.

### **ToUInt512(ReadOnlySpan&lt;Byte&gt;)**

Converts a read-only byte span into a 512-bit unsigned integer.

```csharp
public static UInt512 ToUInt512(ReadOnlySpan<byte> value)
```

#### Parameters

`value` [ReadOnlySpan&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<br>
A read-only span containing the bytes to convert.

#### Returns

[UInt512](./missingvalues/uint512.md)<br>
A 512-bit unsigned integer representing the converted bytes.

### **ToInt512(Byte[], Int32)**

Returns a 512-bit integer converted from 64 bytes at a specified position in a byte array.

```csharp
public static Int512 ToInt512(Byte[] value, int startIndex)
```

#### Parameters

`value` [Byte[]](https://learn.microsoft.com/en-us/dotnet/api/system.byte)<br>
An array of bytes.

`startIndex` [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>
The starting position within `value`.

#### Returns

[Int512](./missingvalues/int512.md)<br>
A 512-bit integer formed by 64 bytes beginning at `startIndex`.

### **ToInt512(ReadOnlySpan&lt;Byte&gt;)**

Converts a read-only byte span into a 512-bit integer.

```csharp
public static Int512 ToInt512(ReadOnlySpan<byte> value)
```

#### Parameters

`value` [ReadOnlySpan&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<br>
A read-only span containing the bytes to convert.

#### Returns

[Int512](./missingvalues/int512.md)<br>
A 512-bit integer representing the converted bytes.

### **ReadQuadBigEndian(ReadOnlySpan&lt;Byte&gt;)**

Reads a [Quad](./missingvalues/quad.md) from the beginning of a read-only span of bytes, as big endian.

```csharp
public static Quad ReadQuadBigEndian(ReadOnlySpan<byte> source)
```

#### Parameters

`source` [ReadOnlySpan&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<br>
The read-only span to read.

#### Returns

[Quad](./missingvalues/quad.md)<br>
The big endian value.

#### Exceptions

[ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception)<br>
source is too small to contain a [Quad](./missingvalues/quad.md)

**Remarks:**

Reads exactly 16 bytes from the beginning of the span.

### **ReadOctoBigEndian(ReadOnlySpan&lt;Byte&gt;)**

Reads a [Octo](./missingvalues/octo.md) from the beginning of a read-only span of bytes, as big endian.

```csharp
public static Octo ReadOctoBigEndian(ReadOnlySpan<byte> source)
```

#### Parameters

`source` [ReadOnlySpan&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<br>
The read-only span to read.

#### Returns

[Octo](./missingvalues/octo.md)<br>
The big endian value.

#### Exceptions

[ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception)<br>
source is too small to contain a [Octo](./missingvalues/octo.md)

**Remarks:**

Reads exactly 32 bytes from the beginning of the span.

### **ReadUInt256BigEndian(ReadOnlySpan&lt;Byte&gt;)**

Reads a [UInt256](./missingvalues/uint256.md) from the beginning of a read-only span of bytes, as big endian.

```csharp
public static UInt256 ReadUInt256BigEndian(ReadOnlySpan<byte> source)
```

#### Parameters

`source` [ReadOnlySpan&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<br>
The read-only span to read.

#### Returns

[UInt256](./missingvalues/uint256.md)<br>
The big endian value.

#### Exceptions

[ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception)<br>
source is too small to contain a [UInt256](./missingvalues/uint256.md)

**Remarks:**

Reads exactly 32 bytes from the beginning of the span.

### **ReadInt256BigEndian(ReadOnlySpan&lt;Byte&gt;)**

Reads a [Int256](./missingvalues/int256.md) from the beginning of a read-only span of bytes, as big endian.

```csharp
public static Int256 ReadInt256BigEndian(ReadOnlySpan<byte> source)
```

#### Parameters

`source` [ReadOnlySpan&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<br>
The read-only span to read.

#### Returns

[Int256](./missingvalues/int256.md)<br>
The big endian value.

#### Exceptions

[ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception)<br>
source is too small to contain a [Int256](./missingvalues/int256.md)

**Remarks:**

Reads exactly 32 bytes from the beginning of the span.

### **ReadUInt512BigEndian(ReadOnlySpan&lt;Byte&gt;)**

Reads a [UInt512](./missingvalues/uint512.md) from the beginning of a read-only span of bytes, as big endian.

```csharp
public static UInt512 ReadUInt512BigEndian(ReadOnlySpan<byte> source)
```

#### Parameters

`source` [ReadOnlySpan&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<br>
The read-only span to read.

#### Returns

[UInt512](./missingvalues/uint512.md)<br>
The big endian value.

#### Exceptions

[ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception)<br>
source is too small to contain a [UInt512](./missingvalues/uint512.md)

**Remarks:**

Reads exactly 64 bytes from the beginning of the span.

### **ReadInt512BigEndian(ReadOnlySpan&lt;Byte&gt;)**

Reads a [Int512](./missingvalues/int512.md) from the beginning of a read-only span of bytes, as big endian.

```csharp
public static Int512 ReadInt512BigEndian(ReadOnlySpan<byte> source)
```

#### Parameters

`source` [ReadOnlySpan&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<br>
The read-only span to read.

#### Returns

[Int512](./missingvalues/int512.md)<br>
The big endian value.

#### Exceptions

[ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception)<br>
source is too small to contain a [Int512](./missingvalues/int512.md)

**Remarks:**

Reads exactly 64 bytes from the beginning of the span.

### **ReadQuadLittleEndian(ReadOnlySpan&lt;Byte&gt;)**

Reads a [Quad](./missingvalues/quad.md) from the beginning of a read-only span of bytes, as little endian.

```csharp
public static Quad ReadQuadLittleEndian(ReadOnlySpan<byte> source)
```

#### Parameters

`source` [ReadOnlySpan&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<br>
The read-only span to read.

#### Returns

[Quad](./missingvalues/quad.md)<br>
The little endian value.

#### Exceptions

[ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception)<br>
source is too small to contain a [Quad](./missingvalues/quad.md)

**Remarks:**

Reads exactly 16 bytes from the beginning of the span.

### **ReadOctoLittleEndian(ReadOnlySpan&lt;Byte&gt;)**

Reads a [Octo](./missingvalues/octo.md) from the beginning of a read-only span of bytes, as little endian.

```csharp
public static Octo ReadOctoLittleEndian(ReadOnlySpan<byte> source)
```

#### Parameters

`source` [ReadOnlySpan&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<br>
The read-only span to read.

#### Returns

[Octo](./missingvalues/octo.md)<br>
The little endian value.

#### Exceptions

[ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception)<br>
source is too small to contain a [Octo](./missingvalues/octo.md)

**Remarks:**

Reads exactly 32 bytes from the beginning of the span.

### **ReadUInt256LittleEndian(ReadOnlySpan&lt;Byte&gt;)**

Reads a [UInt256](./missingvalues/uint256.md) from the beginning of a read-only span of bytes, as little endian.

```csharp
public static UInt256 ReadUInt256LittleEndian(ReadOnlySpan<byte> source)
```

#### Parameters

`source` [ReadOnlySpan&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<br>
The read-only span to read.

#### Returns

[UInt256](./missingvalues/uint256.md)<br>
The little endian value.

#### Exceptions

[ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception)<br>
source is too small to contain a [UInt256](./missingvalues/uint256.md)

**Remarks:**

Reads exactly 32 bytes from the beginning of the span.

### **ReadInt256LittleEndian(ReadOnlySpan&lt;Byte&gt;)**

Reads a [Int256](./missingvalues/int256.md) from the beginning of a read-only span of bytes, as little endian.

```csharp
public static Int256 ReadInt256LittleEndian(ReadOnlySpan<byte> source)
```

#### Parameters

`source` [ReadOnlySpan&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<br>
The read-only span to read.

#### Returns

[Int256](./missingvalues/int256.md)<br>
The little endian value.

#### Exceptions

[ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception)<br>
source is too small to contain a [Int256](./missingvalues/int256.md)

**Remarks:**

Reads exactly 32 bytes from the beginning of the span.

### **ReadUInt512LittleEndian(ReadOnlySpan&lt;Byte&gt;)**

Reads a [UInt512](./missingvalues/uint512.md) from the beginning of a read-only span of bytes, as little endian.

```csharp
public static UInt512 ReadUInt512LittleEndian(ReadOnlySpan<byte> source)
```

#### Parameters

`source` [ReadOnlySpan&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<br>
The read-only span to read.

#### Returns

[UInt512](./missingvalues/uint512.md)<br>
The little endian value.

#### Exceptions

[ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception)<br>
source is too small to contain a [UInt512](./missingvalues/uint512.md)

**Remarks:**

Reads exactly 64 bytes from the beginning of the span.

### **ReadInt512LittleEndian(ReadOnlySpan&lt;Byte&gt;)**

Reads a [Int512](./missingvalues/int512.md) from the beginning of a read-only span of bytes, as little endian.

```csharp
public static Int512 ReadInt512LittleEndian(ReadOnlySpan<byte> source)
```

#### Parameters

`source` [ReadOnlySpan&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<br>
The read-only span to read.

#### Returns

[Int512](./missingvalues/int512.md)<br>
The little endian value.

#### Exceptions

[ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception)<br>
source is too small to contain a [Int512](./missingvalues/int512.md)

**Remarks:**

Reads exactly 64 bytes from the beginning of the span.

### **TryReadQuadBigEndian(ReadOnlySpan&lt;Byte&gt;, out Quad)**

Reads a [Quad](./missingvalues/quad.md) from the beginning of a read-only span of bytes, as big endian.

```csharp
public static bool TryReadQuadBigEndian(ReadOnlySpan<byte> source, out Quad value)
```

#### Parameters

`source` [ReadOnlySpan&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<br>
The read-only span to read.

`out` `value` [Quad](./missingvalues/quad.md)<br>
When this method returns, contains the value read out of the read-only span of bytes, as big endian.

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true if the span is large enough to contain a [Quad](./missingvalues/quad.md); otherwise, false.

**Remarks:**

Reads exactly 16 bytes from the beginning of the span.

### **TryReadOctoBigEndian(ReadOnlySpan&lt;Byte&gt;, out Octo)**

Reads a [Octo](./missingvalues/octo.md) from the beginning of a read-only span of bytes, as big endian.

```csharp
public static bool TryReadOctoBigEndian(ReadOnlySpan<byte> source, out Octo value)
```

#### Parameters

`source` [ReadOnlySpan&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<br>
The read-only span to read.

`out` `value` [Octo](./missingvalues/octo.md)<br>
When this method returns, contains the value read out of the read-only span of bytes, as big endian.

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true if the span is large enough to contain a [Octo](./missingvalues/octo.md); otherwise, false.

**Remarks:**

Reads exactly 32 bytes from the beginning of the span.

### **TryReadUInt256BigEndian(ReadOnlySpan&lt;Byte&gt;, out UInt256)**

Reads a [UInt256](./missingvalues/uint256.md) from the beginning of a read-only span of bytes, as big endian.

```csharp
public static bool TryReadUInt256BigEndian(ReadOnlySpan<byte> source, out UInt256 value)
```

#### Parameters

`source` [ReadOnlySpan&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<br>
The read-only span to read.

`out` `value` [UInt256](./missingvalues/uint256.md)<br>
When this method returns, contains the value read out of the read-only span of bytes, as big endian.

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true if the span is large enough to contain a [UInt256](./missingvalues/uint256.md); otherwise, false.

**Remarks:**

Reads exactly 32 bytes from the beginning of the span.

### **TryReadInt256BigEndian(ReadOnlySpan&lt;Byte&gt;, out Int256)**

Reads a [Int256](./missingvalues/int256.md) from the beginning of a read-only span of bytes, as big endian.

```csharp
public static bool TryReadInt256BigEndian(ReadOnlySpan<byte> source, out Int256 value)
```

#### Parameters

`source` [ReadOnlySpan&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<br>
The read-only span to read.

`out` `value` [Int256](./missingvalues/int256.md)<br>
When this method returns, contains the value read out of the read-only span of bytes, as big endian.

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true if the span is large enough to contain a [Int256](./missingvalues/int256.md); otherwise, false.

**Remarks:**

Reads exactly 32 bytes from the beginning of the span.

### **TryReadUInt512BigEndian(ReadOnlySpan&lt;Byte&gt;, out UInt512)**

Reads a [UInt512](./missingvalues/uint512.md) from the beginning of a read-only span of bytes, as big endian.

```csharp
public static bool TryReadUInt512BigEndian(ReadOnlySpan<byte> source, out UInt512 value)
```

#### Parameters

`source` [ReadOnlySpan&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<br>
The read-only span to read.

`out` `value` [UInt512](./missingvalues/uint512.md)<br>
When this method returns, contains the value read out of the read-only span of bytes, as big endian.

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true if the span is large enough to contain a [UInt512](./missingvalues/uint512.md); otherwise, false.

**Remarks:**

Reads exactly 64 bytes from the beginning of the span.

### **TryReadInt512BigEndian(ReadOnlySpan&lt;Byte&gt;, out Int512)**

Reads a [Int512](./missingvalues/int512.md) from the beginning of a read-only span of bytes, as big endian.

```csharp
public static bool TryReadInt512BigEndian(ReadOnlySpan<byte> source, out Int512 value)
```

#### Parameters

`source` [ReadOnlySpan&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<br>
The read-only span to read.

`out` `value` [Int512](./missingvalues/int512.md)<br>
When this method returns, contains the value read out of the read-only span of bytes, as big endian.

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true if the span is large enough to contain a [Int512](./missingvalues/int512.md); otherwise, false.

**Remarks:**

Reads exactly 64 bytes from the beginning of the span.

### **TryReadQuadLittleEndian(ReadOnlySpan&lt;Byte&gt;, out Quad)**

Reads a [Quad](./missingvalues/quad.md) from the beginning of a read-only span of bytes, as little endian.

```csharp
public static bool TryReadQuadLittleEndian(ReadOnlySpan<byte> source, out Quad value)
```

#### Parameters

`source` [ReadOnlySpan&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<br>
The read-only span to read.

`out` `value` [Quad](./missingvalues/quad.md)<br>
When this method returns, contains the value read out of the read-only span of bytes, as little endian.

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true if the span is large enough to contain a [Quad](./missingvalues/quad.md); otherwise, false.

**Remarks:**

Reads exactly 16 bytes from the beginning of the span.

### **TryReadOctoLittleEndian(ReadOnlySpan&lt;Byte&gt;, out Octo)**

Reads a [Octo](./missingvalues/octo.md) from the beginning of a read-only span of bytes, as little endian.

```csharp
public static bool TryReadOctoLittleEndian(ReadOnlySpan<byte> source, out Octo value)
```

#### Parameters

`source` [ReadOnlySpan&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<br>
The read-only span to read.

`out` `value` [Octo](./missingvalues/octo.md)<br>
When this method returns, contains the value read out of the read-only span of bytes, as little endian.

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true if the span is large enough to contain a [Octo](./missingvalues/octo.md); otherwise, false.

**Remarks:**

Reads exactly 32 bytes from the beginning of the span.

### **TryReadUInt256LittleEndian(ReadOnlySpan&lt;Byte&gt;, out UInt256)**

Reads a [UInt256](./missingvalues/uint256.md) from the beginning of a read-only span of bytes, as little endian.

```csharp
public static bool TryReadUInt256LittleEndian(ReadOnlySpan<byte> source, out UInt256 value)
```

#### Parameters

`source` [ReadOnlySpan&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<br>
The read-only span to read.

`out` `value` [UInt256](./missingvalues/uint256.md)<br>
When this method returns, contains the value read out of the read-only span of bytes, as little endian.

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true if the span is large enough to contain a [UInt256](./missingvalues/uint256.md); otherwise, false.

**Remarks:**

Reads exactly 32 bytes from the beginning of the span.

### **TryReadInt256LittleEndian(ReadOnlySpan&lt;Byte&gt;, out Int256)**

Reads a [Int256](./missingvalues/int256.md) from the beginning of a read-only span of bytes, as little endian.

```csharp
public static bool TryReadInt256LittleEndian(ReadOnlySpan<byte> source, out Int256 value)
```

#### Parameters

`source` [ReadOnlySpan&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<br>
The read-only span to read.

`out` `value` [Int256](./missingvalues/int256.md)<br>
When this method returns, contains the value read out of the read-only span of bytes, as little endian.

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true if the span is large enough to contain a [Int256](./missingvalues/int256.md); otherwise, false.

**Remarks:**

Reads exactly 32 bytes from the beginning of the span.

### **TryReadUInt512LittleEndian(ReadOnlySpan&lt;Byte&gt;, out UInt512)**

Reads a [UInt512](./missingvalues/uint512.md) from the beginning of a read-only span of bytes, as little endian.

```csharp
public static bool TryReadUInt512LittleEndian(ReadOnlySpan<byte> source, out UInt512 value)
```

#### Parameters

`source` [ReadOnlySpan&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<br>
The read-only span to read.

`out` `value` [UInt512](./missingvalues/uint512.md)<br>
When this method returns, contains the value read out of the read-only span of bytes, as little endian.

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true if the span is large enough to contain a [UInt512](./missingvalues/uint512.md); otherwise, false.

**Remarks:**

Reads exactly 64 bytes from the beginning of the span.

### **TryReadInt512LittleEndian(ReadOnlySpan&lt;Byte&gt;, out Int512)**

Reads a [Int512](./missingvalues/int512.md) from the beginning of a read-only span of bytes, as little endian.

```csharp
public static bool TryReadInt512LittleEndian(ReadOnlySpan<byte> source, out Int512 value)
```

#### Parameters

`source` [ReadOnlySpan&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<br>
The read-only span to read.

`out` `value` [Int512](./missingvalues/int512.md)<br>
When this method returns, contains the value read out of the read-only span of bytes, as little endian.

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true if the span is large enough to contain a [Int512](./missingvalues/int512.md); otherwise, false.

**Remarks:**

Reads exactly 64 bytes from the beginning of the span.

### **GetBytes(in Quad)**

Returns the specified quadruple-precision floating-point value as an array of bytes.

```csharp
public static Byte[] GetBytes(in Quad value)
```

#### Parameters

`in` `value` [Quad](./missingvalues/quad.md)<br>
The number to convert.

#### Returns

[Byte[]](https://learn.microsoft.com/en-us/dotnet/api/system.byte)<br>
An array of bytes with length 16.

### **GetBytes(in Octo)**

Returns the specified octuple-precision floating-point value as an array of bytes.

```csharp
public static Byte[] GetBytes(in Octo value)
```

#### Parameters

`in` `value` [Octo](./missingvalues/octo.md)<br>
The number to convert.

#### Returns

[Byte[]](https://learn.microsoft.com/en-us/dotnet/api/system.byte)<br>
An array of bytes with length 16.

### **GetBytes(in UInt256)**

Returns the specified 256-bit unsigned integer value as an array of bytes.

```csharp
public static Byte[] GetBytes(in UInt256 value)
```

#### Parameters

`in` `value` [UInt256](./missingvalues/uint256.md)<br>
The number to convert.

#### Returns

[Byte[]](https://learn.microsoft.com/en-us/dotnet/api/system.byte)<br>
An array of bytes with length 32.

### **GetBytes(in Int256)**

Returns the specified 256-bit integer value as an array of bytes.

```csharp
public static Byte[] GetBytes(in Int256 value)
```

#### Parameters

`in` `value` [Int256](./missingvalues/int256.md)<br>
The number to convert.

#### Returns

[Byte[]](https://learn.microsoft.com/en-us/dotnet/api/system.byte)<br>
An array of bytes with length 32.

### **GetBytes(in UInt512)**

Returns the specified 512-bit unsigned integer value as an array of bytes.

```csharp
public static Byte[] GetBytes(in UInt512 value)
```

#### Parameters

`in` `value` [UInt512](./missingvalues/uint512.md)<br>
The number to convert.

#### Returns

[Byte[]](https://learn.microsoft.com/en-us/dotnet/api/system.byte)<br>
An array of bytes with length 64.

### **GetBytes(in Int512)**

Returns the specified 512-bit integer value as an array of bytes.

```csharp
public static Byte[] GetBytes(in Int512 value)
```

#### Parameters

`in` `value` [Int512](./missingvalues/int512.md)<br>
The number to convert.

#### Returns

[Byte[]](https://learn.microsoft.com/en-us/dotnet/api/system.byte)<br>
An array of bytes with length 64.

### **WriteQuadBigEndian(Span&lt;Byte&gt;, in Quad)**

Write a [Quad](./missingvalues/quad.md) into a span of bytes, as big endian.

```csharp
public static void WriteQuadBigEndian(Span<byte> destination, in Quad value)
```

#### Parameters

`destination` [Span&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.span-1)<br>
The span of bytes where the value is to be written, as big endian.

`in` `value` [Quad](./missingvalues/quad.md)<br>
The value to write into the span of bytes.

#### Exceptions

[ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception)<br>
destination is too small to contain a [Quad](./missingvalues/quad.md)

**Remarks:**

Writes exactly 16 bytes to the beginning of the span.

### **WriteOctoBigEndian(Span&lt;Byte&gt;, in Octo)**

Write a [Octo](./missingvalues/octo.md) into a span of bytes, as big endian.

```csharp
public static void WriteOctoBigEndian(Span<byte> destination, in Octo value)
```

#### Parameters

`destination` [Span&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.span-1)<br>
The span of bytes where the value is to be written, as big endian.

`in` `value` [Octo](./missingvalues/octo.md)<br>
The value to write into the span of bytes.

#### Exceptions

[ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception)<br>
destination is too small to contain a [Octo](./missingvalues/octo.md)

**Remarks:**

Writes exactly 32 bytes to the beginning of the span.

### **WriteUInt256BigEndian(Span&lt;Byte&gt;, in UInt256)**

Write a [UInt256](./missingvalues/uint256.md) into a span of bytes, as big endian.

```csharp
public static void WriteUInt256BigEndian(Span<byte> destination, in UInt256 value)
```

#### Parameters

`destination` [Span&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.span-1)<br>
The span of bytes where the value is to be written, as big endian.

`in` `value` [UInt256](./missingvalues/uint256.md)<br>
The value to write into the span of bytes.

#### Exceptions

[ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception)<br>
destination is too small to contain a [UInt256](./missingvalues/uint256.md)

**Remarks:**

Writes exactly 32 bytes to the beginning of the span.

### **WriteInt256BigEndian(Span&lt;Byte&gt;, in Int256)**

Write a [Int256](./missingvalues/int256.md) into a span of bytes, as big endian.

```csharp
public static void WriteInt256BigEndian(Span<byte> destination, in Int256 value)
```

#### Parameters

`destination` [Span&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.span-1)<br>
The span of bytes where the value is to be written, as big endian.

`in` `value` [Int256](./missingvalues/int256.md)<br>
The value to write into the span of bytes.

#### Exceptions

[ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception)<br>
destination is too small to contain a [Int256](./missingvalues/int256.md)

**Remarks:**

Writes exactly 32 bytes to the beginning of the span.

### **WriteUInt512BigEndian(Span&lt;Byte&gt;, in UInt512)**

Write a [UInt512](./missingvalues/uint512.md) into a span of bytes, as big endian.

```csharp
public static void WriteUInt512BigEndian(Span<byte> destination, in UInt512 value)
```

#### Parameters

`destination` [Span&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.span-1)<br>
The span of bytes where the value is to be written, as big endian.

`in` `value` [UInt512](./missingvalues/uint512.md)<br>
The value to write into the span of bytes.

#### Exceptions

[ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception)<br>
destination is too small to contain a [UInt512](./missingvalues/uint512.md)

**Remarks:**

Writes exactly 64 bytes to the beginning of the span.

### **WriteInt512BigEndian(Span&lt;Byte&gt;, in Int512)**

Write a [Int512](./missingvalues/int512.md) into a span of bytes, as big endian.

```csharp
public static void WriteInt512BigEndian(Span<byte> destination, in Int512 value)
```

#### Parameters

`destination` [Span&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.span-1)<br>
The span of bytes where the value is to be written, as big endian.

`in` `value` [Int512](./missingvalues/int512.md)<br>
The value to write into the span of bytes.

#### Exceptions

[ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception)<br>
destination is too small to contain a [Int512](./missingvalues/int512.md)

**Remarks:**

Writes exactly 64 bytes to the beginning of the span.

### **WriteQuadLittleEndian(Span&lt;Byte&gt;, in Quad)**

Write a [Quad](./missingvalues/quad.md) into a span of bytes, as little endian.

```csharp
public static void WriteQuadLittleEndian(Span<byte> destination, in Quad value)
```

#### Parameters

`destination` [Span&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.span-1)<br>
The span of bytes where the value is to be written, as little endian.

`in` `value` [Quad](./missingvalues/quad.md)<br>
The value to write into the span of bytes.

#### Exceptions

[ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception)<br>
destination is too small to contain a [Quad](./missingvalues/quad.md)

**Remarks:**

Writes exactly 16 bytes to the beginning of the span.

### **WriteOctoLittleEndian(Span&lt;Byte&gt;, in Octo)**

Write a [Octo](./missingvalues/octo.md) into a span of bytes, as little endian.

```csharp
public static void WriteOctoLittleEndian(Span<byte> destination, in Octo value)
```

#### Parameters

`destination` [Span&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.span-1)<br>
The span of bytes where the value is to be written, as little endian.

`in` `value` [Octo](./missingvalues/octo.md)<br>
The value to write into the span of bytes.

#### Exceptions

[ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception)<br>
destination is too small to contain a [Octo](./missingvalues/octo.md)

**Remarks:**

Writes exactly 32 bytes to the beginning of the span.

### **WriteUInt256LittleEndian(Span&lt;Byte&gt;, in UInt256)**

Write a [UInt256](./missingvalues/uint256.md) into a span of bytes, as little endian.

```csharp
public static void WriteUInt256LittleEndian(Span<byte> destination, in UInt256 value)
```

#### Parameters

`destination` [Span&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.span-1)<br>
The span of bytes where the value is to be written, as little endian.

`in` `value` [UInt256](./missingvalues/uint256.md)<br>
The value to write into the span of bytes.

#### Exceptions

[ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception)<br>
destination is too small to contain a [UInt256](./missingvalues/uint256.md)

**Remarks:**

Writes exactly 32 bytes to the beginning of the span.

### **WriteInt256LittleEndian(Span&lt;Byte&gt;, in Int256)**

Write a [Int256](./missingvalues/int256.md) into a span of bytes, as little endian.

```csharp
public static void WriteInt256LittleEndian(Span<byte> destination, in Int256 value)
```

#### Parameters

`destination` [Span&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.span-1)<br>
The span of bytes where the value is to be written, as little endian.

`in` `value` [Int256](./missingvalues/int256.md)<br>
The value to write into the span of bytes.

#### Exceptions

[ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception)<br>
destination is too small to contain a [Int256](./missingvalues/int256.md)

**Remarks:**

Writes exactly 32 bytes to the beginning of the span.

### **WriteUInt512LittleEndian(Span&lt;Byte&gt;, in UInt512)**

Write a [UInt512](./missingvalues/uint512.md) into a span of bytes, as little endian.

```csharp
public static void WriteUInt512LittleEndian(Span<byte> destination, in UInt512 value)
```

#### Parameters

`destination` [Span&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.span-1)<br>
The span of bytes where the value is to be written, as little endian.

`in` `value` [UInt512](./missingvalues/uint512.md)<br>
The value to write into the span of bytes.

#### Exceptions

[ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception)<br>
destination is too small to contain a [UInt512](./missingvalues/uint512.md)

**Remarks:**

Writes exactly 64 bytes to the beginning of the span.

### **WriteInt512LittleEndian(Span&lt;Byte&gt;, in Int512)**

Write a [Int512](./missingvalues/int512.md) into a span of bytes, as little endian.

```csharp
public static void WriteInt512LittleEndian(Span<byte> destination, in Int512 value)
```

#### Parameters

`destination` [Span&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.span-1)<br>
The span of bytes where the value is to be written, as little endian.

`in` `value` [Int512](./missingvalues/int512.md)<br>
The value to write into the span of bytes.

#### Exceptions

[ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception)<br>
destination is too small to contain a [Int512](./missingvalues/int512.md)

**Remarks:**

Writes exactly 64 bytes to the beginning of the span.

### **TryWriteBytes(Span&lt;Byte&gt;, in Quad)**

Converts a quadruple-precision floating-point value into a span of bytes.

```csharp
public static bool TryWriteBytes(Span<byte> destination, in Quad value)
```

#### Parameters

`destination` [Span&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.span-1)<br>
When this method returns, the bytes representing the converted quadruple-precision floating-point value.

`in` `value` [Quad](./missingvalues/quad.md)<br>
The quadruple-precision floating-point value to convert.

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>
if the conversion was successful;  otherwise.

### **TryWriteBytes(Span&lt;Byte&gt;, in Octo)**

Converts an octuple-precision floating-point value into a span of bytes.

```csharp
public static bool TryWriteBytes(Span<byte> destination, in Octo value)
```

#### Parameters

`destination` [Span&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.span-1)<br>
When this method returns, the bytes representing the converted octuple-precision floating-point value.

`in` `value` [Octo](./missingvalues/octo.md)<br>
The octuple-precision floating-point value to convert.

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>
if the conversion was successful;  otherwise.

### **TryWriteBytes(Span&lt;Byte&gt;, in UInt256)**

Converts a 256-bit unsigned integer into a span of bytes.

```csharp
public static bool TryWriteBytes(Span<byte> destination, in UInt256 value)
```

#### Parameters

`destination` [Span&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.span-1)<br>
When this method returns, the bytes representing the converted 256-bit unsigned integer.

`in` `value` [UInt256](./missingvalues/uint256.md)<br>
The 256-bit unsigned integer to convert.

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>
if the conversion was successful;  otherwise.

### **TryWriteBytes(Span&lt;Byte&gt;, in Int256)**

Converts a 256-bit integer into a span of bytes.

```csharp
public static bool TryWriteBytes(Span<byte> destination, in Int256 value)
```

#### Parameters

`destination` [Span&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.span-1)<br>
When this method returns, the bytes representing the converted 256-bit integer.

`in` `value` [Int256](./missingvalues/int256.md)<br>
The 256-bit integer to convert.

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>
if the conversion was successful;  otherwise.

### **TryWriteBytes(Span&lt;Byte&gt;, in UInt512)**

Converts a 512-bit unsigned integer into a span of bytes.

```csharp
public static bool TryWriteBytes(Span<byte> destination, in UInt512 value)
```

#### Parameters

`destination` [Span&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.span-1)<br>
When this method returns, the bytes representing the converted 512-bit unsigned integer.

`in` `value` [UInt512](./missingvalues/uint512.md)<br>
The 512-bit unsigned integer to convert.

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>
if the conversion was successful;  otherwise.

### **TryWriteBytes(Span&lt;Byte&gt;, in Int512)**

Converts a 512-bit integer into a span of bytes.

```csharp
public static bool TryWriteBytes(Span<byte> destination, in Int512 value)
```

#### Parameters

`destination` [Span&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.span-1)<br>
When this method returns, the bytes representing the converted 512-bit integer.

`in` `value` [Int512](./missingvalues/int512.md)<br>
The 512-bit integer to convert.

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>
if the conversion was successful;  otherwise.

### **TryWriteQuadBigEndian(Span&lt;Byte&gt;, in Quad)**

Write a [Quad](./missingvalues/quad.md) into a span of bytes, as big endian.

```csharp
public static bool TryWriteQuadBigEndian(Span<byte> destination, in Quad value)
```

#### Parameters

`destination` [Span&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.span-1)<br>
The span of bytes where the value is to be written, as big endian.

`in` `value` [Quad](./missingvalues/quad.md)<br>
The value to write into the span of bytes.

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true if the span is large enough to contain a [Quad](./missingvalues/quad.md); otherwise, false.

**Remarks:**

Writes exactly 16 bytes to the beginning of the span.

### **TryWriteOctoBigEndian(Span&lt;Byte&gt;, in Octo)**

Write a [Octo](./missingvalues/octo.md) into a span of bytes, as big endian.

```csharp
public static bool TryWriteOctoBigEndian(Span<byte> destination, in Octo value)
```

#### Parameters

`destination` [Span&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.span-1)<br>
The span of bytes where the value is to be written, as big endian.

`in` `value` [Octo](./missingvalues/octo.md)<br>
The value to write into the span of bytes.

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true if the span is large enough to contain a [Octo](./missingvalues/octo.md); otherwise, false.

**Remarks:**

Writes exactly 32 bytes to the beginning of the span.

### **TryWriteUInt256BigEndian(Span&lt;Byte&gt;, in UInt256)**

Write a [UInt256](./missingvalues/uint256.md) into a span of bytes, as big endian.

```csharp
public static bool TryWriteUInt256BigEndian(Span<byte> destination, in UInt256 value)
```

#### Parameters

`destination` [Span&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.span-1)<br>
The span of bytes where the value is to be written, as big endian.

`in` `value` [UInt256](./missingvalues/uint256.md)<br>
The value to write into the span of bytes.

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true if the span is large enough to contain a [UInt256](./missingvalues/uint256.md); otherwise, false.

**Remarks:**

Writes exactly 32 bytes to the beginning of the span.

### **TryWriteInt256BigEndian(Span&lt;Byte&gt;, in Int256)**

Write a [Int256](./missingvalues/int256.md) into a span of bytes, as big endian.

```csharp
public static bool TryWriteInt256BigEndian(Span<byte> destination, in Int256 value)
```

#### Parameters

`destination` [Span&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.span-1)<br>
The span of bytes where the value is to be written, as big endian.

`in` `value` [Int256](./missingvalues/int256.md)<br>
The value to write into the span of bytes.

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true if the span is large enough to contain a [Int256](./missingvalues/int256.md); otherwise, false.

**Remarks:**

Writes exactly 32 bytes to the beginning of the span.

### **TryWriteUInt512BigEndian(Span&lt;Byte&gt;, in UInt512)**

Write a [UInt512](./missingvalues/uint512.md) into a span of bytes, as big endian.

```csharp
public static bool TryWriteUInt512BigEndian(Span<byte> destination, in UInt512 value)
```

#### Parameters

`destination` [Span&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.span-1)<br>
The span of bytes where the value is to be written, as big endian.

`in` `value` [UInt512](./missingvalues/uint512.md)<br>
The value to write into the span of bytes.

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true if the span is large enough to contain a [UInt512](./missingvalues/uint512.md); otherwise, false.

**Remarks:**

Writes exactly 64 bytes to the beginning of the span.

### **TryWriteInt512BigEndian(Span&lt;Byte&gt;, in Int512)**

Write a [Int512](./missingvalues/int512.md) into a span of bytes, as big endian.

```csharp
public static bool TryWriteInt512BigEndian(Span<byte> destination, in Int512 value)
```

#### Parameters

`destination` [Span&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.span-1)<br>
The span of bytes where the value is to be written, as big endian.

`in` `value` [Int512](./missingvalues/int512.md)<br>
The value to write into the span of bytes.

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true if the span is large enough to contain a [Int512](./missingvalues/int512.md); otherwise, false.

**Remarks:**

Writes exactly 64 bytes to the beginning of the span.

### **TryWriteQuadLittleEndian(Span&lt;Byte&gt;, in Quad)**

Write a [Quad](./missingvalues/quad.md) into a span of bytes, as little endian.

```csharp
public static bool TryWriteQuadLittleEndian(Span<byte> destination, in Quad value)
```

#### Parameters

`destination` [Span&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.span-1)<br>
The span of bytes where the value is to be written, as little endian.

`in` `value` [Quad](./missingvalues/quad.md)<br>
The value to write into the span of bytes.

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true if the span is large enough to contain a [Quad](./missingvalues/quad.md); otherwise, false.

**Remarks:**

Writes exactly 16 bytes to the beginning of the span.

### **TryWriteOctoLittleEndian(Span&lt;Byte&gt;, in Octo)**

Write a [Octo](./missingvalues/octo.md) into a span of bytes, as little endian.

```csharp
public static bool TryWriteOctoLittleEndian(Span<byte> destination, in Octo value)
```

#### Parameters

`destination` [Span&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.span-1)<br>
The span of bytes where the value is to be written, as little endian.

`in` `value` [Octo](./missingvalues/octo.md)<br>
The value to write into the span of bytes.

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true if the span is large enough to contain a [Octo](./missingvalues/octo.md); otherwise, false.

**Remarks:**

Writes exactly 32 bytes to the beginning of the span.

### **TryWriteUInt256LittleEndian(Span&lt;Byte&gt;, in UInt256)**

Write a [UInt256](./missingvalues/uint256.md) into a span of bytes, as little endian.

```csharp
public static bool TryWriteUInt256LittleEndian(Span<byte> destination, in UInt256 value)
```

#### Parameters

`destination` [Span&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.span-1)<br>
The span of bytes where the value is to be written, as little endian.

`in` `value` [UInt256](./missingvalues/uint256.md)<br>
The value to write into the span of bytes.

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true if the span is large enough to contain a [UInt256](./missingvalues/uint256.md); otherwise, false.

**Remarks:**

Writes exactly 32 bytes to the beginning of the span.

### **TryWriteInt256LittleEndian(Span&lt;Byte&gt;, in Int256)**

Write a [Int256](./missingvalues/int256.md) into a span of bytes, as little endian.

```csharp
public static bool TryWriteInt256LittleEndian(Span<byte> destination, in Int256 value)
```

#### Parameters

`destination` [Span&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.span-1)<br>
The span of bytes where the value is to be written, as little endian.

`in` `value` [Int256](./missingvalues/int256.md)<br>
The value to write into the span of bytes.

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true if the span is large enough to contain a [Int256](./missingvalues/int256.md); otherwise, false.

**Remarks:**

Writes exactly 32 bytes to the beginning of the span.

### **TryWriteUInt512LittleEndian(Span&lt;Byte&gt;, in UInt512)**

Write a [UInt512](./missingvalues/uint512.md) into a span of bytes, as little endian.

```csharp
public static bool TryWriteUInt512LittleEndian(Span<byte> destination, in UInt512 value)
```

#### Parameters

`destination` [Span&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.span-1)<br>
The span of bytes where the value is to be written, as little endian.

`in` `value` [UInt512](./missingvalues/uint512.md)<br>
The value to write into the span of bytes.

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true if the span is large enough to contain a [UInt512](./missingvalues/uint512.md); otherwise, false.

**Remarks:**

Writes exactly 64 bytes to the beginning of the span.

### **TryWriteInt512LittleEndian(Span&lt;Byte&gt;, in Int512)**

Write a [Int512](./missingvalues/int512.md) into a span of bytes, as little endian.

```csharp
public static bool TryWriteInt512LittleEndian(Span<byte> destination, in Int512 value)
```

#### Parameters

`destination` [Span&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.span-1)<br>
The span of bytes where the value is to be written, as little endian.

`in` `value` [Int512](./missingvalues/int512.md)<br>
The value to write into the span of bytes.

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true if the span is large enough to contain a [Int512](./missingvalues/int512.md); otherwise, false.

**Remarks:**

Writes exactly 64 bytes to the beginning of the span.

---

[`< Back`](../../)
