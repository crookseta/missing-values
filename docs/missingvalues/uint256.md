[`< Back`](../)

---

# UInt256

Namespace: MissingValues

Represents a 256-bit unsigned integer.

```csharp
public readonly struct UInt256
```

Inheritance [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) → [ValueType](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype) → [UInt256](./missingvalues/uint256.md)<br>
Implements [IBigInteger&lt;UInt256&gt;](./missingvalues/internals/ibiginteger-1.md), [IBigBinaryNumber&lt;UInt256&gt;](./missingvalues/internals/ibigbinarynumber-1.md), [IBigNumber&lt;UInt256&gt;](./missingvalues/internals/ibignumber-1.md), [INumber&lt;UInt256&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.inumber-1), [IComparable](https://learn.microsoft.com/en-us/dotnet/api/system.icomparable), [IComparable&lt;UInt256&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.icomparable-1), [IComparisonOperators&lt;UInt256, UInt256, Boolean&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.icomparisonoperators-3), [IEqualityOperators&lt;UInt256, UInt256, Boolean&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.iequalityoperators-3), [IModulusOperators&lt;UInt256, UInt256, UInt256&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.imodulusoperators-3), [INumberBase&lt;UInt256&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.inumberbase-1), [IAdditionOperators&lt;UInt256, UInt256, UInt256&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.iadditionoperators-3), [IAdditiveIdentity&lt;UInt256, UInt256&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.iadditiveidentity-2), [IDecrementOperators&lt;UInt256&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.idecrementoperators-1), [IDivisionOperators&lt;UInt256, UInt256, UInt256&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.idivisionoperators-3), [IEquatable&lt;UInt256&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1), [IIncrementOperators&lt;UInt256&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.iincrementoperators-1), [IMultiplicativeIdentity&lt;UInt256, UInt256&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.imultiplicativeidentity-2), [IMultiplyOperators&lt;UInt256, UInt256, UInt256&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.imultiplyoperators-3), [ISpanFormattable](https://learn.microsoft.com/en-us/dotnet/api/system.ispanformattable), [IFormattable](https://learn.microsoft.com/en-us/dotnet/api/system.iformattable), [ISpanParsable&lt;UInt256&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.ispanparsable-1), [IParsable&lt;UInt256&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iparsable-1), [ISubtractionOperators&lt;UInt256, UInt256, UInt256&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.isubtractionoperators-3), [IUnaryPlusOperators&lt;UInt256, UInt256&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.iunaryplusoperators-2), [IUnaryNegationOperators&lt;UInt256, UInt256&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.iunarynegationoperators-2), [IUtf8SpanFormattable](https://learn.microsoft.com/en-us/dotnet/api/system.iutf8spanformattable), [IUtf8SpanParsable&lt;UInt256&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iutf8spanparsable-1), [IBinaryNumber&lt;UInt256&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.ibinarynumber-1), [IBitwiseOperators&lt;UInt256, UInt256, UInt256&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.ibitwiseoperators-3), [IBinaryInteger&lt;UInt256&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.ibinaryinteger-1), [IShiftOperators&lt;UInt256, Int32, UInt256&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.ishiftoperators-3), [IMinMaxValue&lt;UInt256&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.iminmaxvalue-1), [IUnsignedNumber&lt;UInt256&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.iunsignednumber-1), [IPowerFunctions&lt;UInt256&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.ipowerfunctions-1), [IFormattableUnsignedInteger&lt;UInt256&gt;](./missingvalues/info/iformattableunsignedinteger-1.md), [IFormattableInteger&lt;UInt256&gt;](./missingvalues/info/iformattableinteger-1.md), [IFormattableNumber&lt;UInt256&gt;](./missingvalues/info/iformattablenumber-1.md)<br>
Attributes [IsReadOnlyAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.isreadonlyattribute), [JsonConverterAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.serialization.jsonconverterattribute), [DebuggerDisplayAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.debuggerdisplayattribute), [DebuggerTypeProxyAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.debuggertypeproxyattribute)

## Fields

### **One**

Represents the value `1` of the type.

```csharp
public static UInt256 One;
```

### **MaxValue**

Represents the largest possible value of the type.

```csharp
public static UInt256 MaxValue;
```

### **MinValue**

Represents the smallest possible value of the type.

```csharp
public static UInt256 MinValue;
```

### **Zero**

Represents the value `0` of the type.

```csharp
public static UInt256 Zero;
```

## Constructors

### **UInt256(UInt64, UInt64, UInt64, UInt64)**

Initializes a new instance of the [UInt256](./missingvalues/uint256.md) struct.

```csharp
public UInt256(ulong u1, ulong u2, ulong l1, ulong l2)
```

#### Parameters

`u1` [UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64)<br>
The first 64-bits of the 256-bit value.

`u2` [UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64)<br>
The second 64-bits of the 256-bit value.

`l1` [UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64)<br>
The third 64-bits of the 256-bit value.

`l2` [UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64)<br>
The fourth 64-bits of the 256-bit value.

### **UInt256(UInt128)**

Initializes a new instance of the [UInt256](./missingvalues/uint256.md) struct.

```csharp
public UInt256(UInt128 lower)
```

#### Parameters

`lower` [UInt128](https://learn.microsoft.com/en-us/dotnet/api/system.uint128)<br>
The lower 128-bits of the 256-bit value.

### **UInt256(UInt128, UInt128)**

Initializes a new instance of the [UInt256](./missingvalues/uint256.md) struct.

```csharp
public UInt256(UInt128 upper, UInt128 lower)
```

#### Parameters

`upper` [UInt128](https://learn.microsoft.com/en-us/dotnet/api/system.uint128)<br>
The upper 128-bits of the 256-bit value.

`lower` [UInt128](https://learn.microsoft.com/en-us/dotnet/api/system.uint128)<br>
The lower 128-bits of the 256-bit value.

## Methods

### **CreateChecked&lt;TOther&gt;(TOther)**

```csharp
public static UInt256 CreateChecked<TOther>(TOther value) where TOther : INumberBase<TOther>
```

#### Type Parameters

`TOther`<br>

#### Parameters

`value` TOther<br>

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **CreateSaturating&lt;TOther&gt;(TOther)**

```csharp
public static UInt256 CreateSaturating<TOther>(TOther value) where TOther : INumberBase<TOther>
```

#### Type Parameters

`TOther`<br>

#### Parameters

`value` TOther<br>

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **CreateTruncating&lt;TOther&gt;(TOther)**

```csharp
public static UInt256 CreateTruncating<TOther>(TOther value) where TOther : INumberBase<TOther>
```

#### Type Parameters

`TOther`<br>

#### Parameters

`value` TOther<br>

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **ToString()**

```csharp
public override string ToString()
```

#### Returns

[String](https://learn.microsoft.com/en-us/dotnet/api/system.string)<br>

### **Equals(Object)**

```csharp
public override bool Equals(object? obj)
```

#### Parameters

`obj` [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object)?<br>

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **GetHashCode()**

```csharp
public override int GetHashCode()
```

#### Returns

[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>

### **BigMul(UInt256, UInt256, out UInt256)**

Produces the full product of two unsigned 256-bit numbers.

```csharp
public static UInt256 BigMul(UInt256 left, UInt256 right, out UInt256 lower)
```

#### Parameters

`left` [UInt256](./missingvalues/uint256.md)<br>
First number to multiply.

`right` [UInt256](./missingvalues/uint256.md)<br>
Second number to multiply.

`out` `lower` [UInt256](./missingvalues/uint256.md)<br>
The low 256-bit of the product of the specified numbers.

#### Returns

[UInt256](./missingvalues/uint256.md)<br>
The high 256-bit of the product of the specified numbers.

### **Log10(UInt256)**

Computes the base-10 logarithm of a [UInt256](./missingvalues/uint256.md).

```csharp
public static UInt256 Log10(UInt256 value)
```

#### Parameters

`value` [UInt256](./missingvalues/uint256.md)<br>
The value whose base-10 logarithm is to be computed.

#### Returns

[UInt256](./missingvalues/uint256.md)<br>
The base-10 logarithm of `value`

### **Pow(UInt256, Int32)**

Raises a [UInt256](./missingvalues/uint256.md) value to the power of a specified value.

```csharp
public static UInt256 Pow(UInt256 value, int exponent)
```

#### Parameters

`value` [UInt256](./missingvalues/uint256.md)<br>
The number to raise to the `exponent` power.

`exponent` [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>
The exponent to raise `value` by.

#### Returns

[UInt256](./missingvalues/uint256.md)<br>
The result of raising `value` to the `exponent` power.

#### Exceptions

[ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception)<br>
`exponent` is negative.

[OverflowException](https://learn.microsoft.com/en-us/dotnet/api/system.overflowexception)<br>
The result of raising `value` to the `exponent` power is less than [UInt256.MinValue](./missingvalues/uint256.md#minvalue) or greater than [UInt256.MaxValue](./missingvalues/uint256.md#maxvalue).

### **Parse(ReadOnlySpan&lt;Char&gt;)**

Parses a span of characters into a value.

```csharp
public static UInt256 Parse(ReadOnlySpan<char> s)
```

#### Parameters

`s` [ReadOnlySpan&lt;Char&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<br>
The span of characters to parse.

#### Returns

[UInt256](./missingvalues/uint256.md)<br>
The result of parsing `s`.

#### Exceptions

[FormatException](https://learn.microsoft.com/en-us/dotnet/api/system.formatexception)<br>
`s` is not in the correct format.

[OverflowException](https://learn.microsoft.com/en-us/dotnet/api/system.overflowexception)<br>
`s` is not representable by [UInt256](./missingvalues/uint256.md).

### **TryParse(ReadOnlySpan&lt;Char&gt;, out UInt256)**

Tries to parse a span of characters into a value.

```csharp
public static bool TryParse(ReadOnlySpan<char> s, out UInt256 result)
```

#### Parameters

`s` [ReadOnlySpan&lt;Char&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<br>
The span of characters to parse.

`out` `result` [UInt256](./missingvalues/uint256.md)<br>
On return, contains the result of successfully parsing `s` or an undefined value on failure.

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>
`true` if `s` was successfully parsed; otherwise, `false`.

### **Clamp(UInt256, UInt256, UInt256)**

```csharp
public static UInt256 Clamp(UInt256 value, UInt256 min, UInt256 max)
```

#### Parameters

`value` [UInt256](./missingvalues/uint256.md)<br>

`min` [UInt256](./missingvalues/uint256.md)<br>

`max` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **CompareTo(UInt256)**

```csharp
public int CompareTo(UInt256 other)
```

#### Parameters

`other` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>

### **CompareTo(Object)**

```csharp
public int CompareTo(object? obj)
```

#### Parameters

`obj` [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object)?<br>

#### Returns

[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>

### **DivRem(UInt256, UInt256)**

```csharp
public static (UInt256 Quotient, UInt256 Remainder) DivRem(UInt256 left, UInt256 right)
```

#### Parameters

`left` [UInt256](./missingvalues/uint256.md)<br>

`right` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

`(UInt256 Quotient, UInt256 Remainder)`<br>

### **Equals(UInt256)**

```csharp
public new bool Equals(UInt256 other)
```

#### Parameters

`other` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **IsEvenInteger(UInt256)**

```csharp
public static bool IsEvenInteger(UInt256 value)
```

#### Parameters

`value` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **IsOddInteger(UInt256)**

```csharp
public static bool IsOddInteger(UInt256 value)
```

#### Parameters

`value` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **IsPow2(UInt256)**

```csharp
public static bool IsPow2(UInt256 value)
```

#### Parameters

`value` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **LeadingZeroCount(UInt256)**

```csharp
public static UInt256 LeadingZeroCount(UInt256 value)
```

#### Parameters

`value` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **Log2(UInt256)**

```csharp
public static UInt256 Log2(UInt256 value)
```

#### Parameters

`value` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **Max(UInt256, UInt256)**

```csharp
public static UInt256 Max(UInt256 x, UInt256 y)
```

#### Parameters

`x` [UInt256](./missingvalues/uint256.md)<br>

`y` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **Min(UInt256, UInt256)**

```csharp
public static UInt256 Min(UInt256 x, UInt256 y)
```

#### Parameters

`x` [UInt256](./missingvalues/uint256.md)<br>

`y` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **Parse(ReadOnlySpan&lt;Char&gt;, NumberStyles, IFormatProvider)**

```csharp
public static UInt256 Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
```

#### Parameters

`s` [ReadOnlySpan&lt;Char&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<br>

`style` [NumberStyles](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.numberstyles)<br>

`provider` [IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider)?<br>

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **Parse(String, NumberStyles, IFormatProvider)**

```csharp
public static UInt256 Parse(string s, NumberStyles style, IFormatProvider? provider)
```

#### Parameters

`s` [String](https://learn.microsoft.com/en-us/dotnet/api/system.string)<br>

`style` [NumberStyles](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.numberstyles)<br>

`provider` [IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider)?<br>

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **Parse(ReadOnlySpan&lt;Char&gt;, IFormatProvider)**

```csharp
public static UInt256 Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
```

#### Parameters

`s` [ReadOnlySpan&lt;Char&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<br>

`provider` [IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider)?<br>

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **Parse(String, IFormatProvider)**

```csharp
public static UInt256 Parse(string s, IFormatProvider? provider)
```

#### Parameters

`s` [String](https://learn.microsoft.com/en-us/dotnet/api/system.string)<br>

`provider` [IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider)?<br>

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **Parse(ReadOnlySpan&lt;Byte&gt;, NumberStyles, IFormatProvider)**

```csharp
public static UInt256 Parse(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider)
```

#### Parameters

`utf8Text` [ReadOnlySpan&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<br>

`style` [NumberStyles](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.numberstyles)<br>

`provider` [IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider)?<br>

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **Parse(ReadOnlySpan&lt;Byte&gt;, IFormatProvider)**

```csharp
public static UInt256 Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
```

#### Parameters

`utf8Text` [ReadOnlySpan&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<br>

`provider` [IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider)?<br>

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **PopCount(UInt256)**

```csharp
public static UInt256 PopCount(UInt256 value)
```

#### Parameters

`value` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **RotateLeft(UInt256, Int32)**

```csharp
public static UInt256 RotateLeft(UInt256 value, int rotateAmount)
```

#### Parameters

`value` [UInt256](./missingvalues/uint256.md)<br>

`rotateAmount` [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **RotateRight(UInt256, Int32)**

```csharp
public static UInt256 RotateRight(UInt256 value, int rotateAmount)
```

#### Parameters

`value` [UInt256](./missingvalues/uint256.md)<br>

`rotateAmount` [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **TrailingZeroCount(UInt256)**

```csharp
public static UInt256 TrailingZeroCount(UInt256 value)
```

#### Parameters

`value` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **TryParse(ReadOnlySpan&lt;Char&gt;, NumberStyles, IFormatProvider, out UInt256)**

```csharp
public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out UInt256 result)
```

#### Parameters

`s` [ReadOnlySpan&lt;Char&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<br>

`style` [NumberStyles](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.numberstyles)<br>

`provider` [IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider)?<br>

`out` `result` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **TryParse(String, NumberStyles, IFormatProvider, out UInt256)**

```csharp
public static bool TryParse(string? s, NumberStyles style, IFormatProvider? provider, out UInt256 result)
```

#### Parameters

`s` [String](https://learn.microsoft.com/en-us/dotnet/api/system.string)?<br>

`style` [NumberStyles](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.numberstyles)<br>

`provider` [IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider)?<br>

`out` `result` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **TryParse(ReadOnlySpan&lt;Char&gt;, IFormatProvider, out UInt256)**

```csharp
public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out UInt256 result)
```

#### Parameters

`s` [ReadOnlySpan&lt;Char&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<br>

`provider` [IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider)?<br>

`out` `result` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **TryParse(String, IFormatProvider, out UInt256)**

```csharp
public static bool TryParse(string? s, IFormatProvider? provider, out UInt256 result)
```

#### Parameters

`s` [String](https://learn.microsoft.com/en-us/dotnet/api/system.string)?<br>

`provider` [IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider)?<br>

`out` `result` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **TryParse(ReadOnlySpan&lt;Byte&gt;, NumberStyles, IFormatProvider, out UInt256)**

```csharp
public static bool TryParse(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider, out UInt256 result)
```

#### Parameters

`utf8Text` [ReadOnlySpan&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<br>

`style` [NumberStyles](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.numberstyles)<br>

`provider` [IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider)?<br>

`out` `result` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **TryParse(ReadOnlySpan&lt;Byte&gt;, IFormatProvider, out UInt256)**

```csharp
public static bool TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, out UInt256 result)
```

#### Parameters

`utf8Text` [ReadOnlySpan&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<br>

`provider` [IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider)?<br>

`out` `result` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **ToString(String, IFormatProvider)**

```csharp
public string ToString(string? format, IFormatProvider? formatProvider)
```

#### Parameters

`format` [String](https://learn.microsoft.com/en-us/dotnet/api/system.string)?<br>

`formatProvider` [IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider)?<br>

#### Returns

[String](https://learn.microsoft.com/en-us/dotnet/api/system.string)<br>

### **TryFormat(Span&lt;Char&gt;, out Int32, ReadOnlySpan&lt;Char&gt;, IFormatProvider)**

```csharp
public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
```

#### Parameters

`destination` [Span&lt;Char&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.span-1)<br>

`out` `charsWritten` [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>

`format` [ReadOnlySpan&lt;Char&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<br>

`provider` [IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider)?<br>

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **TryFormat(Span&lt;Byte&gt;, out Int32, ReadOnlySpan&lt;Char&gt;, IFormatProvider)**

```csharp
public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
```

#### Parameters

`utf8Destination` [Span&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.span-1)<br>

`out` `bytesWritten` [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>

`format` [ReadOnlySpan&lt;Char&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<br>

`provider` [IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider)?<br>

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>

## Operators

### **explicit operator char(in UInt256)**

Explicitly converts a [UInt256](./missingvalues/uint256.md) value to a [Char](https://learn.microsoft.com/en-us/dotnet/api/system.char).

```csharp
public static explicit operator char(in UInt256 value)
```

#### Parameters

`in` `value` [UInt256](./missingvalues/uint256.md)<br>
The value to convert.

#### Returns

[Char](https://learn.microsoft.com/en-us/dotnet/api/system.char)<br>

### **operator op_CheckedExplicit(in UInt256)**

```csharp
public static char operator op_CheckedExplicit(in UInt256 value)
```

#### Parameters

`in` `value` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[Char](https://learn.microsoft.com/en-us/dotnet/api/system.char)<br>

### **explicit operator byte(in UInt256)**

Explicitly converts a [UInt256](./missingvalues/uint256.md) value to a [Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte).

```csharp
public static explicit operator byte(in UInt256 value)
```

#### Parameters

`in` `value` [UInt256](./missingvalues/uint256.md)<br>
The value to convert.

#### Returns

[Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte)<br>

### **operator op_CheckedExplicit(in UInt256)**

```csharp
public static byte operator op_CheckedExplicit(in UInt256 value)
```

#### Parameters

`in` `value` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte)<br>

### **explicit operator ushort(in UInt256)**

Explicitly converts a [UInt256](./missingvalues/uint256.md) value to a [UInt16](https://learn.microsoft.com/en-us/dotnet/api/system.uint16).

```csharp
public static explicit operator ushort(in UInt256 value)
```

#### Parameters

`in` `value` [UInt256](./missingvalues/uint256.md)<br>
The value to convert.

#### Returns

[UInt16](https://learn.microsoft.com/en-us/dotnet/api/system.uint16)<br>

### **operator op_CheckedExplicit(in UInt256)**

```csharp
public static ushort operator op_CheckedExplicit(in UInt256 value)
```

#### Parameters

`in` `value` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[UInt16](https://learn.microsoft.com/en-us/dotnet/api/system.uint16)<br>

### **explicit operator uint(in UInt256)**

Explicitly converts a [UInt256](./missingvalues/uint256.md) value to a [UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32).

```csharp
public static explicit operator uint(in UInt256 value)
```

#### Parameters

`in` `value` [UInt256](./missingvalues/uint256.md)<br>
The value to convert.

#### Returns

[UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32)<br>

### **operator op_CheckedExplicit(in UInt256)**

```csharp
public static uint operator op_CheckedExplicit(in UInt256 value)
```

#### Parameters

`in` `value` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32)<br>

### **explicit operator ulong(in UInt256)**

Explicitly converts a [UInt256](./missingvalues/uint256.md) value to a [UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64).

```csharp
public static explicit operator ulong(in UInt256 value)
```

#### Parameters

`in` `value` [UInt256](./missingvalues/uint256.md)<br>
The value to convert.

#### Returns

[UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64)<br>

### **operator op_CheckedExplicit(in UInt256)**

```csharp
public static ulong operator op_CheckedExplicit(in UInt256 value)
```

#### Parameters

`in` `value` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64)<br>

### **explicit operator UInt128(in UInt256)**

Explicitly converts a [UInt256](./missingvalues/uint256.md) value to a [UInt128](https://learn.microsoft.com/en-us/dotnet/api/system.uint128).

```csharp
public static explicit operator UInt128(in UInt256 value)
```

#### Parameters

`in` `value` [UInt256](./missingvalues/uint256.md)<br>
The value to convert.

#### Returns

[UInt128](https://learn.microsoft.com/en-us/dotnet/api/system.uint128)<br>

### **operator op_CheckedExplicit(in UInt256)**

```csharp
public static UInt128 operator op_CheckedExplicit(in UInt256 value)
```

#### Parameters

`in` `value` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[UInt128](https://learn.microsoft.com/en-us/dotnet/api/system.uint128)<br>

### **implicit operator UInt512(in UInt256)**

Implicitly converts a [UInt256](./missingvalues/uint256.md) value to a [UInt512](./missingvalues/uint512.md).

```csharp
public static implicit operator UInt512(in UInt256 value)
```

#### Parameters

`in` `value` [UInt256](./missingvalues/uint256.md)<br>
The value to convert.

#### Returns

[UInt512](./missingvalues/uint512.md)<br>

### **explicit operator UIntPtr(in UInt256)**

Explicitly converts a [UInt256](./missingvalues/uint256.md) value to a [UIntPtr](https://learn.microsoft.com/en-us/dotnet/api/system.uintptr).

```csharp
public static explicit operator UIntPtr(in UInt256 value)
```

#### Parameters

`in` `value` [UInt256](./missingvalues/uint256.md)<br>
The value to convert.

#### Returns

[UIntPtr](https://learn.microsoft.com/en-us/dotnet/api/system.uintptr)<br>

### **operator op_CheckedExplicit(in UInt256)**

```csharp
public static UIntPtr operator op_CheckedExplicit(in UInt256 value)
```

#### Parameters

`in` `value` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[UIntPtr](https://learn.microsoft.com/en-us/dotnet/api/system.uintptr)<br>

### **explicit operator sbyte(in UInt256)**

Explicitly converts a [UInt256](./missingvalues/uint256.md) value to a [SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte).

```csharp
public static explicit operator sbyte(in UInt256 value)
```

#### Parameters

`in` `value` [UInt256](./missingvalues/uint256.md)<br>
The value to convert.

#### Returns

[SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte)<br>

### **operator op_CheckedExplicit(in UInt256)**

```csharp
public static sbyte operator op_CheckedExplicit(in UInt256 value)
```

#### Parameters

`in` `value` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte)<br>

### **explicit operator short(in UInt256)**

Explicitly converts a [UInt256](./missingvalues/uint256.md) value to a [Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16).

```csharp
public static explicit operator short(in UInt256 value)
```

#### Parameters

`in` `value` [UInt256](./missingvalues/uint256.md)<br>
The value to convert.

#### Returns

[Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16)<br>

### **operator op_CheckedExplicit(in UInt256)**

```csharp
public static short operator op_CheckedExplicit(in UInt256 value)
```

#### Parameters

`in` `value` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16)<br>

### **explicit operator int(in UInt256)**

Explicitly converts a [UInt256](./missingvalues/uint256.md) value to a [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32).

```csharp
public static explicit operator int(in UInt256 value)
```

#### Parameters

`in` `value` [UInt256](./missingvalues/uint256.md)<br>
The value to convert.

#### Returns

[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>

### **operator op_CheckedExplicit(in UInt256)**

```csharp
public static int operator op_CheckedExplicit(in UInt256 value)
```

#### Parameters

`in` `value` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>

### **explicit operator long(in UInt256)**

Explicitly converts a [UInt256](./missingvalues/uint256.md) value to a [Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64).

```csharp
public static explicit operator long(in UInt256 value)
```

#### Parameters

`in` `value` [UInt256](./missingvalues/uint256.md)<br>
The value to convert.

#### Returns

[Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64)<br>

### **operator op_CheckedExplicit(in UInt256)**

```csharp
public static long operator op_CheckedExplicit(in UInt256 value)
```

#### Parameters

`in` `value` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64)<br>

### **explicit operator Int128(in UInt256)**

Explicitly converts a [UInt256](./missingvalues/uint256.md) value to a [Int128](https://learn.microsoft.com/en-us/dotnet/api/system.int128).

```csharp
public static explicit operator Int128(in UInt256 value)
```

#### Parameters

`in` `value` [UInt256](./missingvalues/uint256.md)<br>
The value to convert.

#### Returns

[Int128](https://learn.microsoft.com/en-us/dotnet/api/system.int128)<br>

### **operator op_CheckedExplicit(in UInt256)**

```csharp
public static Int128 operator op_CheckedExplicit(in UInt256 value)
```

#### Parameters

`in` `value` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[Int128](https://learn.microsoft.com/en-us/dotnet/api/system.int128)<br>

### **explicit operator Int256(in UInt256)**

Explicitly converts a [UInt256](./missingvalues/uint256.md) value to a [Int256](./missingvalues/int256.md).

```csharp
public static explicit operator Int256(in UInt256 value)
```

#### Parameters

`in` `value` [UInt256](./missingvalues/uint256.md)<br>
The value to convert.

#### Returns

[Int256](./missingvalues/int256.md)<br>

### **operator op_CheckedExplicit(in UInt256)**

```csharp
public static Int256 operator op_CheckedExplicit(in UInt256 value)
```

#### Parameters

`in` `value` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[Int256](./missingvalues/int256.md)<br>

### **explicit operator Int512(in UInt256)**

Explicitly converts a [UInt256](./missingvalues/uint256.md) value to a [Int512](./missingvalues/int512.md).

```csharp
public static explicit operator Int512(in UInt256 value)
```

#### Parameters

`in` `value` [UInt256](./missingvalues/uint256.md)<br>
The value to convert.

#### Returns

[Int512](./missingvalues/int512.md)<br>

### **operator op_CheckedExplicit(in UInt256)**

```csharp
public static Int512 operator op_CheckedExplicit(in UInt256 value)
```

#### Parameters

`in` `value` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[Int512](./missingvalues/int512.md)<br>

### **explicit operator IntPtr(in UInt256)**

Explicitly converts a [UInt256](./missingvalues/uint256.md) value to a [IntPtr](https://learn.microsoft.com/en-us/dotnet/api/system.intptr).

```csharp
public static explicit operator IntPtr(in UInt256 value)
```

#### Parameters

`in` `value` [UInt256](./missingvalues/uint256.md)<br>
The value to convert.

#### Returns

[IntPtr](https://learn.microsoft.com/en-us/dotnet/api/system.intptr)<br>

### **operator op_CheckedExplicit(in UInt256)**

```csharp
public static IntPtr operator op_CheckedExplicit(in UInt256 value)
```

#### Parameters

`in` `value` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[IntPtr](https://learn.microsoft.com/en-us/dotnet/api/system.intptr)<br>

### **explicit operator BigInteger(in UInt256)**

Explicitly converts a [UInt256](./missingvalues/uint256.md) value to a [System.Numerics.BigInteger](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.biginteger).

```csharp
public static explicit operator BigInteger(in UInt256 value)
```

#### Parameters

`in` `value` [UInt256](./missingvalues/uint256.md)<br>
The value to convert.

#### Returns

[BigInteger](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.biginteger)<br>

### **explicit operator decimal(in UInt256)**

Explicitly converts a [UInt256](./missingvalues/uint256.md) value to a [Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal).

```csharp
public static explicit operator decimal(in UInt256 value)
```

#### Parameters

`in` `value` [UInt256](./missingvalues/uint256.md)<br>
The value to convert.

#### Returns

[Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal)<br>

#### Exceptions

[OverflowException](https://learn.microsoft.com/en-us/dotnet/api/system.overflowexception)<br>
`value` is outside the range of [Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal).

### **explicit operator Octo(in UInt256)**

Explicitly converts a [UInt256](./missingvalues/uint256.md) value to a [Octo](./missingvalues/octo.md).

```csharp
public static explicit operator Octo(in UInt256 value)
```

#### Parameters

`in` `value` [UInt256](./missingvalues/uint256.md)<br>
The value to convert.

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **explicit operator Quad(in UInt256)**

Explicitly converts a [UInt256](./missingvalues/uint256.md) value to a [Quad](./missingvalues/quad.md).

```csharp
public static explicit operator Quad(in UInt256 value)
```

#### Parameters

`in` `value` [UInt256](./missingvalues/uint256.md)<br>
The value to convert.

#### Returns

[Quad](./missingvalues/quad.md)<br>

### **explicit operator double(in UInt256)**

Explicitly converts a [UInt256](./missingvalues/uint256.md) value to a [Double](https://learn.microsoft.com/en-us/dotnet/api/system.double).

```csharp
public static explicit operator double(in UInt256 value)
```

#### Parameters

`in` `value` [UInt256](./missingvalues/uint256.md)<br>
The value to convert.

#### Returns

[Double](https://learn.microsoft.com/en-us/dotnet/api/system.double)<br>

### **explicit operator float(in UInt256)**

Explicitly converts a [UInt256](./missingvalues/uint256.md) value to a [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single).

```csharp
public static explicit operator float(in UInt256 value)
```

#### Parameters

`in` `value` [UInt256](./missingvalues/uint256.md)<br>
The value to convert.

#### Returns

[Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)<br>

### **explicit operator Half(in UInt256)**

Explicitly converts a [UInt256](./missingvalues/uint256.md) value to a [Half](https://learn.microsoft.com/en-us/dotnet/api/system.half).

```csharp
public static explicit operator Half(in UInt256 value)
```

#### Parameters

`in` `value` [UInt256](./missingvalues/uint256.md)<br>
The value to convert.

#### Returns

[Half](https://learn.microsoft.com/en-us/dotnet/api/system.half)<br>

### **implicit operator UInt256(Char)**

Implicitly converts a [Char](https://learn.microsoft.com/en-us/dotnet/api/system.char) value to a [UInt256](./missingvalues/uint256.md).

```csharp
public static implicit operator UInt256(char value)
```

#### Parameters

`value` [Char](https://learn.microsoft.com/en-us/dotnet/api/system.char)<br>
The value to convert.

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **explicit operator UInt256(Half)**

Explicitly converts a [Half](https://learn.microsoft.com/en-us/dotnet/api/system.half) value to a [UInt256](./missingvalues/uint256.md).

```csharp
public static explicit operator UInt256(Half value)
```

#### Parameters

`value` [Half](https://learn.microsoft.com/en-us/dotnet/api/system.half)<br>
The value to convert.

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **operator op_CheckedExplicit(Half)**

```csharp
public static UInt256 operator op_CheckedExplicit(Half value)
```

#### Parameters

`value` [Half](https://learn.microsoft.com/en-us/dotnet/api/system.half)<br>

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **explicit operator UInt256(Single)**

Explicitly converts a [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single) value to a [UInt256](./missingvalues/uint256.md).

```csharp
public static explicit operator UInt256(float value)
```

#### Parameters

`value` [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)<br>
The value to convert.

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **operator op_CheckedExplicit(Single)**

```csharp
public static UInt256 operator op_CheckedExplicit(float value)
```

#### Parameters

`value` [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)<br>

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **explicit operator UInt256(Double)**

Explicitly converts a [Double](https://learn.microsoft.com/en-us/dotnet/api/system.double) value to a [UInt256](./missingvalues/uint256.md).

```csharp
public static explicit operator UInt256(double value)
```

#### Parameters

`value` [Double](https://learn.microsoft.com/en-us/dotnet/api/system.double)<br>
The value to convert.

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **operator op_CheckedExplicit(Double)**

```csharp
public static UInt256 operator op_CheckedExplicit(double value)
```

#### Parameters

`value` [Double](https://learn.microsoft.com/en-us/dotnet/api/system.double)<br>

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **explicit operator UInt256(Decimal)**

Explicitly converts a [Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal) value to a [UInt256](./missingvalues/uint256.md).

```csharp
public static explicit operator UInt256(decimal value)
```

#### Parameters

`value` [Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal)<br>
The value to convert.

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **operator op_CheckedExplicit(Decimal)**

```csharp
public static UInt256 operator op_CheckedExplicit(decimal value)
```

#### Parameters

`value` [Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal)<br>

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **implicit operator UInt256(Byte)**

Implicitly converts a [Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte) value to a [UInt256](./missingvalues/uint256.md).

```csharp
public static implicit operator UInt256(byte value)
```

#### Parameters

`value` [Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte)<br>
The value to convert.

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **implicit operator UInt256(UInt16)**

Implicitly converts a [UInt16](https://learn.microsoft.com/en-us/dotnet/api/system.uint16) value to a [UInt256](./missingvalues/uint256.md).

```csharp
public static implicit operator UInt256(ushort value)
```

#### Parameters

`value` [UInt16](https://learn.microsoft.com/en-us/dotnet/api/system.uint16)<br>
The value to convert.

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **implicit operator UInt256(UInt32)**

Implicitly converts a [UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32) value to a [UInt256](./missingvalues/uint256.md).

```csharp
public static implicit operator UInt256(uint value)
```

#### Parameters

`value` [UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32)<br>
The value to convert.

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **implicit operator UInt256(UInt64)**

Implicitly converts a [UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64) value to a [UInt256](./missingvalues/uint256.md).

```csharp
public static implicit operator UInt256(ulong value)
```

#### Parameters

`value` [UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64)<br>
The value to convert.

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **implicit operator UInt256(UInt128)**

Implicitly converts a [UInt128](https://learn.microsoft.com/en-us/dotnet/api/system.uint128) value to a [UInt256](./missingvalues/uint256.md).

```csharp
public static implicit operator UInt256(UInt128 value)
```

#### Parameters

`value` [UInt128](https://learn.microsoft.com/en-us/dotnet/api/system.uint128)<br>
The value to convert.

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **implicit operator UInt256(UIntPtr)**

Implicitly converts a [UIntPtr](https://learn.microsoft.com/en-us/dotnet/api/system.uintptr) value to a [UInt256](./missingvalues/uint256.md).

```csharp
public static implicit operator UInt256(UIntPtr value)
```

#### Parameters

`value` [UIntPtr](https://learn.microsoft.com/en-us/dotnet/api/system.uintptr)<br>
The value to convert.

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **explicit operator UInt256(SByte)**

Explicitly converts a [SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte) value to a [UInt256](./missingvalues/uint256.md).

```csharp
public static explicit operator UInt256(sbyte value)
```

#### Parameters

`value` [SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte)<br>
The value to convert.

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **operator op_CheckedExplicit(SByte)**

```csharp
public static UInt256 operator op_CheckedExplicit(sbyte value)
```

#### Parameters

`value` [SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte)<br>

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **explicit operator UInt256(Int16)**

Explicitly converts a [Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16) value to a [UInt256](./missingvalues/uint256.md).

```csharp
public static explicit operator UInt256(short value)
```

#### Parameters

`value` [Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16)<br>
The value to convert.

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **operator op_CheckedExplicit(Int16)**

```csharp
public static UInt256 operator op_CheckedExplicit(short value)
```

#### Parameters

`value` [Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16)<br>

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **explicit operator UInt256(Int32)**

Explicitly converts a [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) value to a [UInt256](./missingvalues/uint256.md).

```csharp
public static explicit operator UInt256(int value)
```

#### Parameters

`value` [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>
The value to convert.

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **operator op_CheckedExplicit(Int32)**

```csharp
public static UInt256 operator op_CheckedExplicit(int value)
```

#### Parameters

`value` [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **explicit operator UInt256(Int64)**

Explicitly converts a [Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) value to a [UInt256](./missingvalues/uint256.md).

```csharp
public static explicit operator UInt256(long value)
```

#### Parameters

`value` [Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64)<br>
The value to convert.

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **operator op_CheckedExplicit(Int64)**

```csharp
public static UInt256 operator op_CheckedExplicit(long value)
```

#### Parameters

`value` [Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64)<br>

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **explicit operator UInt256(Int128)**

Explicitly converts a [Int128](https://learn.microsoft.com/en-us/dotnet/api/system.int128) value to a [UInt256](./missingvalues/uint256.md).

```csharp
public static explicit operator UInt256(Int128 value)
```

#### Parameters

`value` [Int128](https://learn.microsoft.com/en-us/dotnet/api/system.int128)<br>
The value to convert.

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **operator op_CheckedExplicit(Int128)**

```csharp
public static UInt256 operator op_CheckedExplicit(Int128 value)
```

#### Parameters

`value` [Int128](https://learn.microsoft.com/en-us/dotnet/api/system.int128)<br>

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **explicit operator UInt256(IntPtr)**

Explicitly converts a [IntPtr](https://learn.microsoft.com/en-us/dotnet/api/system.intptr) value to a [UInt256](./missingvalues/uint256.md).

```csharp
public static explicit operator UInt256(IntPtr value)
```

#### Parameters

`value` [IntPtr](https://learn.microsoft.com/en-us/dotnet/api/system.intptr)<br>
The value to convert.

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **operator op_CheckedExplicit(IntPtr)**

```csharp
public static UInt256 operator op_CheckedExplicit(IntPtr value)
```

#### Parameters

`value` [IntPtr](https://learn.microsoft.com/en-us/dotnet/api/system.intptr)<br>

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **explicit operator UInt256(BigInteger)**

Explicitly converts a [System.Numerics.BigInteger](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.biginteger) value to a [UInt256](./missingvalues/uint256.md).

```csharp
public static explicit operator UInt256(BigInteger value)
```

#### Parameters

`value` [BigInteger](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.biginteger)<br>
The value to convert.

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **operator op_CheckedExplicit(BigInteger)**

```csharp
public static UInt256 operator op_CheckedExplicit(BigInteger value)
```

#### Parameters

`value` [BigInteger](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.biginteger)<br>

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **operator +(in UInt256)**

```csharp
public static UInt256 operator +(in UInt256 value)
```

#### Parameters

`in` `value` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **operator +(in UInt256, in UInt256)**

```csharp
public static UInt256 operator +(in UInt256 left, in UInt256 right)
```

#### Parameters

`in` `left` [UInt256](./missingvalues/uint256.md)<br>

`in` `right` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **operator op_CheckedAddition(in UInt256, in UInt256)**

```csharp
public static UInt256 operator op_CheckedAddition(in UInt256 left, in UInt256 right)
```

#### Parameters

`in` `left` [UInt256](./missingvalues/uint256.md)<br>

`in` `right` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **operator -(in UInt256)**

```csharp
public static UInt256 operator -(in UInt256 value)
```

#### Parameters

`in` `value` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **operator op_CheckedUnaryNegation(in UInt256)**

```csharp
public static UInt256 operator op_CheckedUnaryNegation(in UInt256 value)
```

#### Parameters

`in` `value` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **operator -(in UInt256, in UInt256)**

```csharp
public static UInt256 operator -(in UInt256 left, in UInt256 right)
```

#### Parameters

`in` `left` [UInt256](./missingvalues/uint256.md)<br>

`in` `right` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **operator op_CheckedSubtraction(in UInt256, in UInt256)**

```csharp
public static UInt256 operator op_CheckedSubtraction(in UInt256 left, in UInt256 right)
```

#### Parameters

`in` `left` [UInt256](./missingvalues/uint256.md)<br>

`in` `right` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **operator ~(in UInt256)**

```csharp
public static UInt256 operator ~(in UInt256 value)
```

#### Parameters

`in` `value` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **operator ++(in UInt256)**

```csharp
public static UInt256 operator ++(in UInt256 value)
```

#### Parameters

`in` `value` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **operator op_CheckedIncrement(in UInt256)**

```csharp
public static UInt256 operator op_CheckedIncrement(in UInt256 value)
```

#### Parameters

`in` `value` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **operator --(in UInt256)**

```csharp
public static UInt256 operator --(in UInt256 value)
```

#### Parameters

`in` `value` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **operator op_CheckedDecrement(in UInt256)**

```csharp
public static UInt256 operator op_CheckedDecrement(in UInt256 value)
```

#### Parameters

`in` `value` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **operator *(in UInt256, in UInt256)**

```csharp
public static UInt256 operator *(in UInt256 left, in UInt256 right)
```

#### Parameters

`in` `left` [UInt256](./missingvalues/uint256.md)<br>

`in` `right` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **operator op_CheckedMultiply(in UInt256, in UInt256)**

```csharp
public static UInt256 operator op_CheckedMultiply(in UInt256 left, in UInt256 right)
```

#### Parameters

`in` `left` [UInt256](./missingvalues/uint256.md)<br>

`in` `right` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **operator /(in UInt256, in UInt256)**

```csharp
public static UInt256 operator /(in UInt256 left, in UInt256 right)
```

#### Parameters

`in` `left` [UInt256](./missingvalues/uint256.md)<br>

`in` `right` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **operator op_CheckedDivision(in UInt256, in UInt256)**

```csharp
public static UInt256 operator op_CheckedDivision(in UInt256 left, in UInt256 right)
```

#### Parameters

`in` `left` [UInt256](./missingvalues/uint256.md)<br>

`in` `right` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **operator %(in UInt256, in UInt256)**

```csharp
public static UInt256 operator %(in UInt256 left, in UInt256 right)
```

#### Parameters

`in` `left` [UInt256](./missingvalues/uint256.md)<br>

`in` `right` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **operator &(in UInt256, in UInt256)**

```csharp
public static UInt256 operator &(in UInt256 left, in UInt256 right)
```

#### Parameters

`in` `left` [UInt256](./missingvalues/uint256.md)<br>

`in` `right` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **operator |(in UInt256, in UInt256)**

```csharp
public static UInt256 operator |(in UInt256 left, in UInt256 right)
```

#### Parameters

`in` `left` [UInt256](./missingvalues/uint256.md)<br>

`in` `right` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **operator ^(in UInt256, in UInt256)**

```csharp
public static UInt256 operator ^(in UInt256 left, in UInt256 right)
```

#### Parameters

`in` `left` [UInt256](./missingvalues/uint256.md)<br>

`in` `right` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **operator &lt;&lt;(in UInt256, Int32)**

```csharp
public static UInt256 operator <<(in UInt256 value, int shiftAmount)
```

#### Parameters

`in` `value` [UInt256](./missingvalues/uint256.md)<br>

`shiftAmount` [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **operator &gt;&gt;(in UInt256, Int32)**

```csharp
public static UInt256 operator >>(in UInt256 value, int shiftAmount)
```

#### Parameters

`in` `value` [UInt256](./missingvalues/uint256.md)<br>

`shiftAmount` [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **operator ==(in UInt256, in UInt256)**

```csharp
public static bool operator ==(in UInt256 left, in UInt256 right)
```

#### Parameters

`in` `left` [UInt256](./missingvalues/uint256.md)<br>

`in` `right` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **operator !=(in UInt256, in UInt256)**

```csharp
public static bool operator !=(in UInt256 left, in UInt256 right)
```

#### Parameters

`in` `left` [UInt256](./missingvalues/uint256.md)<br>

`in` `right` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **operator &lt;(in UInt256, in UInt256)**

```csharp
public static bool operator <(in UInt256 left, in UInt256 right)
```

#### Parameters

`in` `left` [UInt256](./missingvalues/uint256.md)<br>

`in` `right` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **operator &gt;(in UInt256, in UInt256)**

```csharp
public static bool operator >(in UInt256 left, in UInt256 right)
```

#### Parameters

`in` `left` [UInt256](./missingvalues/uint256.md)<br>

`in` `right` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **operator &lt;=(in UInt256, in UInt256)**

```csharp
public static bool operator <=(in UInt256 left, in UInt256 right)
```

#### Parameters

`in` `left` [UInt256](./missingvalues/uint256.md)<br>

`in` `right` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **operator &gt;=(in UInt256, in UInt256)**

```csharp
public static bool operator >=(in UInt256 left, in UInt256 right)
```

#### Parameters

`in` `left` [UInt256](./missingvalues/uint256.md)<br>

`in` `right` [UInt256](./missingvalues/uint256.md)<br>

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **operator &gt;&gt;&gt;(in UInt256, Int32)**

```csharp
public static UInt256 operator >>>(in UInt256 value, int shiftAmount)
```

#### Parameters

`in` `value` [UInt256](./missingvalues/uint256.md)<br>

`shiftAmount` [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

---

[`< Back`](../)
