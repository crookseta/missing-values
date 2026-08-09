[`< Back`](../)

---

# Octo

Namespace: MissingValues

Represents an octuple-precision floating-point number.

```csharp
public readonly struct Octo
```

Inheritance [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) → [ValueType](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype) → [Octo](./missingvalues/octo.md)<br>
Implements [IBigBinaryNumber&lt;Octo&gt;](./missingvalues/internals/ibigbinarynumber-1.md), [IBigNumber&lt;Octo&gt;](./missingvalues/internals/ibignumber-1.md), [INumber&lt;Octo&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.inumber-1), [IComparable](https://learn.microsoft.com/en-us/dotnet/api/system.icomparable), [IComparable&lt;Octo&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.icomparable-1), [IComparisonOperators&lt;Octo, Octo, Boolean&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.icomparisonoperators-3), [IEqualityOperators&lt;Octo, Octo, Boolean&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.iequalityoperators-3), [IModulusOperators&lt;Octo, Octo, Octo&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.imodulusoperators-3), [INumberBase&lt;Octo&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.inumberbase-1), [IAdditionOperators&lt;Octo, Octo, Octo&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.iadditionoperators-3), [IAdditiveIdentity&lt;Octo, Octo&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.iadditiveidentity-2), [IDecrementOperators&lt;Octo&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.idecrementoperators-1), [IDivisionOperators&lt;Octo, Octo, Octo&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.idivisionoperators-3), [IEquatable&lt;Octo&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1), [IIncrementOperators&lt;Octo&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.iincrementoperators-1), [IMultiplicativeIdentity&lt;Octo, Octo&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.imultiplicativeidentity-2), [IMultiplyOperators&lt;Octo, Octo, Octo&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.imultiplyoperators-3), [ISpanFormattable](https://learn.microsoft.com/en-us/dotnet/api/system.ispanformattable), [IFormattable](https://learn.microsoft.com/en-us/dotnet/api/system.iformattable), [ISpanParsable&lt;Octo&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.ispanparsable-1), [IParsable&lt;Octo&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iparsable-1), [ISubtractionOperators&lt;Octo, Octo, Octo&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.isubtractionoperators-3), [IUnaryPlusOperators&lt;Octo, Octo&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.iunaryplusoperators-2), [IUnaryNegationOperators&lt;Octo, Octo&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.iunarynegationoperators-2), [IUtf8SpanFormattable](https://learn.microsoft.com/en-us/dotnet/api/system.iutf8spanformattable), [IUtf8SpanParsable&lt;Octo&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iutf8spanparsable-1), [IBinaryNumber&lt;Octo&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.ibinarynumber-1), [IBitwiseOperators&lt;Octo, Octo, Octo&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.ibitwiseoperators-3), [IBinaryFloatingPointIeee754&lt;Octo&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.ibinaryfloatingpointieee754-1), [IFloatingPointIeee754&lt;Octo&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.ifloatingpointieee754-1), [IExponentialFunctions&lt;Octo&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.iexponentialfunctions-1), [IFloatingPointConstants&lt;Octo&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.ifloatingpointconstants-1), [IFloatingPoint&lt;Octo&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.ifloatingpoint-1), [ISignedNumber&lt;Octo&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.isignednumber-1), [IHyperbolicFunctions&lt;Octo&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.ihyperbolicfunctions-1), [ILogarithmicFunctions&lt;Octo&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.ilogarithmicfunctions-1), [IPowerFunctions&lt;Octo&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.ipowerfunctions-1), [IRootFunctions&lt;Octo&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.irootfunctions-1), [ITrigonometricFunctions&lt;Octo&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.itrigonometricfunctions-1), [IBinaryFloatingPointInfo&lt;Octo, UInt256&gt;](./missingvalues/info/ibinaryfloatingpointinfo-2.md), [IFormattableFloatingPoint&lt;Octo&gt;](./missingvalues/info/iformattablefloatingpoint-1.md), [IFormattableNumber&lt;Octo&gt;](./missingvalues/info/iformattablenumber-1.md), [IMinMaxValue&lt;Octo&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.iminmaxvalue-1)<br>
Attributes [IsReadOnlyAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.isreadonlyattribute), [JsonConverterAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.serialization.jsonconverterattribute), [DebuggerDisplayAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.debuggerdisplayattribute), [DebuggerTypeProxyAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.debuggertypeproxyattribute)

## Fields

### **E**

Represents the natural logarithmic base, specified by the constant, `e`.

```csharp
public static Octo E;
```

### **Epsilon**

Represents the smallest positive [Octo](./missingvalues/octo.md) value that is greater than zero.

```csharp
public static Octo Epsilon;
```

### **MaxValue**

Represents the largest possible value of a [Octo](./missingvalues/octo.md).

```csharp
public static Octo MaxValue;
```

### **MinValue**

Represents the smallest possible value of a [Octo](./missingvalues/octo.md).

```csharp
public static Octo MinValue;
```

### **NaN**

Represents a value that is not a number (`NaN`).

```csharp
public static Octo NaN;
```

### **NegativeOne**

Represents the value `-1` of the type.

```csharp
public static Octo NegativeOne;
```

### **NegativeInfinity**

Represents negative infinity.

```csharp
public static Octo NegativeInfinity;
```

### **NegativeZero**

Represents the value `-0` of the type.

```csharp
public static Octo NegativeZero;
```

### **One**

Represents the value `1` of the type.

```csharp
public static Octo One;
```

### **Pi**

Represents the ratio of the circumference of a circle to its diameter, specified by the constant, `pi`.

```csharp
public static Octo Pi;
```

### **PositiveInfinity**

Represents positive infinity.

```csharp
public static Octo PositiveInfinity;
```

### **Tau**

Represents the number of radians in one turn, specified by the constant, `tau`.

```csharp
public static Octo Tau;
```

### **Zero**

Represents the value `0` of the type.

```csharp
public static Octo Zero;
```

## Constructors

### **Octo(Boolean, UInt32, UInt256)**

Initializes a new instance of the [Octo](./missingvalues/octo.md) struct.

```csharp
public Octo(bool sign, uint exp, UInt256 sig)
```

#### Parameters

`sign` [Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>
A [Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) indicating the sign of the number.  represents a negative number, and  represents a positive number.

`exp` [UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32)<br>
An [UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32) representing the exponent part of the floating-point number.

`sig` [UInt256](./missingvalues/uint256.md)<br>
An [UInt256](./missingvalues/uint256.md) representing the significand part of the floating-point number.

## Methods

### **CreateChecked&lt;TOther&gt;(TOther)**

```csharp
public static Octo CreateChecked<TOther>(TOther value) where TOther : INumberBase<TOther>
```

#### Type Parameters

`TOther`<br>

#### Parameters

`value` TOther<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **CreateSaturating&lt;TOther&gt;(TOther)**

```csharp
public static Octo CreateSaturating<TOther>(TOther value) where TOther : INumberBase<TOther>
```

#### Type Parameters

`TOther`<br>

#### Parameters

`value` TOther<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **CreateTruncating&lt;TOther&gt;(TOther)**

```csharp
public static Octo CreateTruncating<TOther>(TOther value) where TOther : INumberBase<TOther>
```

#### Type Parameters

`TOther`<br>

#### Parameters

`value` TOther<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

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

### **ToString()**

```csharp
public override string ToString()
```

#### Returns

[String](https://learn.microsoft.com/en-us/dotnet/api/system.string)<br>

### **Parse(ReadOnlySpan&lt;Char&gt;)**

Parses a span of characters into a value.

```csharp
public static Octo Parse(ReadOnlySpan<char> s)
```

#### Parameters

`s` [ReadOnlySpan&lt;Char&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<br>
The span of characters to parse.

#### Returns

[Octo](./missingvalues/octo.md)<br>
The result of parsing `s`.

### **TryParse(ReadOnlySpan&lt;Char&gt;, out Octo)**

tries to parse a span of characters into a value.

```csharp
public static bool TryParse(ReadOnlySpan<char> s, out Octo result)
```

#### Parameters

`s` [ReadOnlySpan&lt;Char&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<br>
The span of characters to parse.

`out` `result` [Octo](./missingvalues/octo.md)<br>
When this method returns, contains the result of successfully parsing `s`, or an undefined value on failure.

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>
if `s` was successfully parsed; otherwise, .

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

### **Abs(Octo)**

```csharp
public static Octo Abs(Octo value)
```

#### Parameters

`value` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **Acos(Octo)**

```csharp
public static Octo Acos(Octo x)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **Acosh(Octo)**

```csharp
public static Octo Acosh(Octo x)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **AcosPi(Octo)**

```csharp
public static Octo AcosPi(Octo x)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **Asin(Octo)**

```csharp
public static Octo Asin(Octo x)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **Asinh(Octo)**

```csharp
public static Octo Asinh(Octo x)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **AsinPi(Octo)**

```csharp
public static Octo AsinPi(Octo x)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **Atan(Octo)**

```csharp
public static Octo Atan(Octo x)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **Atan2(Octo, Octo)**

```csharp
public static Octo Atan2(Octo y, Octo x)
```

#### Parameters

`y` [Octo](./missingvalues/octo.md)<br>

`x` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **Atan2Pi(Octo, Octo)**

```csharp
public static Octo Atan2Pi(Octo y, Octo x)
```

#### Parameters

`y` [Octo](./missingvalues/octo.md)<br>

`x` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **Atanh(Octo)**

```csharp
public static Octo Atanh(Octo x)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **AtanPi(Octo)**

```csharp
public static Octo AtanPi(Octo x)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **BitDecrement(Octo)**

```csharp
public static Octo BitDecrement(Octo x)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **BitIncrement(Octo)**

```csharp
public static Octo BitIncrement(Octo x)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **Cbrt(Octo)**

```csharp
public static Octo Cbrt(Octo x)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **Ceiling(Octo)**

```csharp
public static Octo Ceiling(Octo x)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **Clamp(Octo, Octo, Octo)**

```csharp
public static Octo Clamp(Octo value, Octo min, Octo max)
```

#### Parameters

`value` [Octo](./missingvalues/octo.md)<br>

`min` [Octo](./missingvalues/octo.md)<br>

`max` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **CopySign(Octo, Octo)**

```csharp
public static Octo CopySign(Octo value, Octo sign)
```

#### Parameters

`value` [Octo](./missingvalues/octo.md)<br>

`sign` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **Cos(Octo)**

```csharp
public static Octo Cos(Octo x)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **Cosh(Octo)**

```csharp
public static Octo Cosh(Octo x)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **CosPi(Octo)**

```csharp
public static Octo CosPi(Octo x)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **DegreesToRadians(Octo)**

```csharp
public static Octo DegreesToRadians(Octo degrees)
```

#### Parameters

`degrees` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **Exp(Octo)**

```csharp
public static Octo Exp(Octo x)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **ExpM1(Octo)**

```csharp
public static Octo ExpM1(Octo x)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **Exp10(Octo)**

```csharp
public static Octo Exp10(Octo x)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **Exp10M1(Octo)**

```csharp
public static Octo Exp10M1(Octo x)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **Exp2(Octo)**

```csharp
public static Octo Exp2(Octo x)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **Exp2M1(Octo)**

```csharp
public static Octo Exp2M1(Octo x)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **Floor(Octo)**

```csharp
public static Octo Floor(Octo x)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **FusedMultiplyAdd(Octo, Octo, Octo)**

```csharp
public static Octo FusedMultiplyAdd(Octo left, Octo right, Octo addend)
```

#### Parameters

`left` [Octo](./missingvalues/octo.md)<br>

`right` [Octo](./missingvalues/octo.md)<br>

`addend` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **Hypot(Octo, Octo)**

```csharp
public static Octo Hypot(Octo x, Octo y)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

`y` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **Ieee754Remainder(Octo, Octo)**

```csharp
public static Octo Ieee754Remainder(Octo left, Octo right)
```

#### Parameters

`left` [Octo](./missingvalues/octo.md)<br>

`right` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **ILogB(Octo)**

```csharp
public static int ILogB(Octo x)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>

### **IsEvenInteger(Octo)**

```csharp
public static bool IsEvenInteger(Octo value)
```

#### Parameters

`value` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **IsFinite(Octo)**

```csharp
public static bool IsFinite(Octo value)
```

#### Parameters

`value` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **IsInfinity(Octo)**

```csharp
public static bool IsInfinity(Octo value)
```

#### Parameters

`value` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **IsInteger(Octo)**

```csharp
public static bool IsInteger(Octo value)
```

#### Parameters

`value` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **IsNaN(Octo)**

```csharp
public static bool IsNaN(Octo value)
```

#### Parameters

`value` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **IsNegative(Octo)**

```csharp
public static bool IsNegative(Octo value)
```

#### Parameters

`value` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **IsNegativeInfinity(Octo)**

```csharp
public static bool IsNegativeInfinity(Octo value)
```

#### Parameters

`value` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **IsNormal(Octo)**

```csharp
public static bool IsNormal(Octo value)
```

#### Parameters

`value` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **IsOddInteger(Octo)**

```csharp
public static bool IsOddInteger(Octo value)
```

#### Parameters

`value` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **IsPositive(Octo)**

```csharp
public static bool IsPositive(Octo value)
```

#### Parameters

`value` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **IsPositiveInfinity(Octo)**

```csharp
public static bool IsPositiveInfinity(Octo value)
```

#### Parameters

`value` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **IsPow2(Octo)**

```csharp
public static bool IsPow2(Octo value)
```

#### Parameters

`value` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **IsRealNumber(Octo)**

```csharp
public static bool IsRealNumber(Octo value)
```

#### Parameters

`value` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **IsSubnormal(Octo)**

```csharp
public static bool IsSubnormal(Octo value)
```

#### Parameters

`value` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **Lerp(Octo, Octo, Octo)**

```csharp
public static Octo Lerp(Octo value1, Octo value2, Octo amount)
```

#### Parameters

`value1` [Octo](./missingvalues/octo.md)<br>

`value2` [Octo](./missingvalues/octo.md)<br>

`amount` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **Log(Octo)**

```csharp
public static Octo Log(Octo x)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **Log(Octo, Octo)**

```csharp
public static Octo Log(Octo x, Octo newBase)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

`newBase` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **LogP1(Octo)**

```csharp
public static Octo LogP1(Octo x)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **Log10(Octo)**

```csharp
public static Octo Log10(Octo x)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **Log10P1(Octo)**

```csharp
public static Octo Log10P1(Octo x)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **Log2(Octo)**

```csharp
public static Octo Log2(Octo x)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **Log2P1(Octo)**

```csharp
public static Octo Log2P1(Octo x)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **Max(Octo, Octo)**

```csharp
public static Octo Max(Octo x, Octo y)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

`y` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **MaxNumber(Octo, Octo)**

```csharp
public static Octo MaxNumber(Octo x, Octo y)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

`y` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **MaxMagnitude(Octo, Octo)**

```csharp
public static Octo MaxMagnitude(Octo x, Octo y)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

`y` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **MaxMagnitudeNumber(Octo, Octo)**

```csharp
public static Octo MaxMagnitudeNumber(Octo x, Octo y)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

`y` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **Min(Octo, Octo)**

```csharp
public static Octo Min(Octo x, Octo y)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

`y` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **MinNumber(Octo, Octo)**

```csharp
public static Octo MinNumber(Octo x, Octo y)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

`y` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **MinMagnitude(Octo, Octo)**

```csharp
public static Octo MinMagnitude(Octo x, Octo y)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

`y` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **MinMagnitudeNumber(Octo, Octo)**

```csharp
public static Octo MinMagnitudeNumber(Octo x, Octo y)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

`y` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **Parse(ReadOnlySpan&lt;Char&gt;, NumberStyles, IFormatProvider)**

```csharp
public static Octo Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
```

#### Parameters

`s` [ReadOnlySpan&lt;Char&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<br>

`style` [NumberStyles](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.numberstyles)<br>

`provider` [IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider)?<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **Parse(String, NumberStyles, IFormatProvider)**

```csharp
public static Octo Parse(string s, NumberStyles style, IFormatProvider? provider)
```

#### Parameters

`s` [String](https://learn.microsoft.com/en-us/dotnet/api/system.string)<br>

`style` [NumberStyles](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.numberstyles)<br>

`provider` [IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider)?<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **Parse(ReadOnlySpan&lt;Char&gt;, IFormatProvider)**

```csharp
public static Octo Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
```

#### Parameters

`s` [ReadOnlySpan&lt;Char&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<br>

`provider` [IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider)?<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **Parse(String, IFormatProvider)**

```csharp
public static Octo Parse(string s, IFormatProvider? provider)
```

#### Parameters

`s` [String](https://learn.microsoft.com/en-us/dotnet/api/system.string)<br>

`provider` [IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider)?<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **Parse(ReadOnlySpan&lt;Byte&gt;, NumberStyles, IFormatProvider)**

```csharp
public static Octo Parse(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider)
```

#### Parameters

`utf8Text` [ReadOnlySpan&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<br>

`style` [NumberStyles](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.numberstyles)<br>

`provider` [IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider)?<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **Parse(ReadOnlySpan&lt;Byte&gt;, IFormatProvider)**

```csharp
public static Octo Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
```

#### Parameters

`utf8Text` [ReadOnlySpan&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<br>

`provider` [IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider)?<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **Pow(Octo, Octo)**

```csharp
public static Octo Pow(Octo x, Octo y)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

`y` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **RadiansToDegrees(Octo)**

```csharp
public static Octo RadiansToDegrees(Octo radians)
```

#### Parameters

`radians` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **ReciprocalEstimate(Octo)**

```csharp
public static Octo ReciprocalEstimate(Octo x)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **ReciprocalSqrtEstimate(Octo)**

```csharp
public static Octo ReciprocalSqrtEstimate(Octo x)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **RootN(Octo, Int32)**

```csharp
public static Octo RootN(Octo x, int n)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

`n` [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **Round(Octo)**

```csharp
public static Octo Round(Octo x)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **Round(Octo, Int32)**

```csharp
public static Octo Round(Octo x, int digits)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

`digits` [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **Round(Octo, MidpointRounding)**

```csharp
public static Octo Round(Octo x, MidpointRounding mode)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

`mode` [MidpointRounding](https://learn.microsoft.com/en-us/dotnet/api/system.midpointrounding)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **Round(Octo, Int32, MidpointRounding)**

```csharp
public static Octo Round(Octo x, int digits, MidpointRounding mode)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

`digits` [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>

`mode` [MidpointRounding](https://learn.microsoft.com/en-us/dotnet/api/system.midpointrounding)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **ScaleB(Octo, Int32)**

```csharp
public static Octo ScaleB(Octo x, int n)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

`n` [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **Sign(Octo)**

```csharp
public static int Sign(Octo value)
```

#### Parameters

`value` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>

### **Sin(Octo)**

```csharp
public static Octo Sin(Octo x)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **SinCos(Octo)**

```csharp
public static (Octo Sin, Octo Cos) SinCos(Octo x)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

#### Returns

`(Octo Sin, Octo Cos)`<br>

### **SinCosPi(Octo)**

```csharp
public static (Octo SinPi, Octo CosPi) SinCosPi(Octo x)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

#### Returns

`(Octo SinPi, Octo CosPi)`<br>

### **Sinh(Octo)**

```csharp
public static Octo Sinh(Octo x)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **SinPi(Octo)**

```csharp
public static Octo SinPi(Octo x)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **Sqrt(Octo)**

```csharp
public static Octo Sqrt(Octo x)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **Tan(Octo)**

```csharp
public static Octo Tan(Octo x)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **Tanh(Octo)**

```csharp
public static Octo Tanh(Octo x)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **TanPi(Octo)**

```csharp
public static Octo TanPi(Octo x)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **Truncate(Octo)**

```csharp
public static Octo Truncate(Octo x)
```

#### Parameters

`x` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **TryParse(ReadOnlySpan&lt;Char&gt;, NumberStyles, IFormatProvider, out Octo)**

```csharp
public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out Octo result)
```

#### Parameters

`s` [ReadOnlySpan&lt;Char&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<br>

`style` [NumberStyles](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.numberstyles)<br>

`provider` [IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider)?<br>

`out` `result` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **TryParse(String, NumberStyles, IFormatProvider, out Octo)**

```csharp
public static bool TryParse(string? s, NumberStyles style, IFormatProvider? provider, out Octo result)
```

#### Parameters

`s` [String](https://learn.microsoft.com/en-us/dotnet/api/system.string)?<br>

`style` [NumberStyles](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.numberstyles)<br>

`provider` [IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider)?<br>

`out` `result` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **TryParse(ReadOnlySpan&lt;Char&gt;, IFormatProvider, out Octo)**

```csharp
public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Octo result)
```

#### Parameters

`s` [ReadOnlySpan&lt;Char&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<br>

`provider` [IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider)?<br>

`out` `result` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **TryParse(String, IFormatProvider, out Octo)**

```csharp
public static bool TryParse(string? s, IFormatProvider? provider, out Octo result)
```

#### Parameters

`s` [String](https://learn.microsoft.com/en-us/dotnet/api/system.string)?<br>

`provider` [IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider)?<br>

`out` `result` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **TryParse(ReadOnlySpan&lt;Byte&gt;, NumberStyles, IFormatProvider, out Octo)**

```csharp
public static bool TryParse(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider, out Octo result)
```

#### Parameters

`utf8Text` [ReadOnlySpan&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<br>

`style` [NumberStyles](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.numberstyles)<br>

`provider` [IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider)?<br>

`out` `result` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **TryParse(ReadOnlySpan&lt;Byte&gt;, IFormatProvider, out Octo)**

```csharp
public static bool TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, out Octo result)
```

#### Parameters

`utf8Text` [ReadOnlySpan&lt;Byte&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)<br>

`provider` [IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider)?<br>

`out` `result` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **CompareTo(Object)**

```csharp
public int CompareTo(object? obj)
```

#### Parameters

`obj` [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object)?<br>

#### Returns

[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>

### **CompareTo(Octo)**

```csharp
public int CompareTo(Octo other)
```

#### Parameters

`other` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>

### **Equals(Octo)**

```csharp
public new bool Equals(Octo other)
```

#### Parameters

`other` [Octo](./missingvalues/octo.md)<br>

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

### **explicit operator byte(in Octo)**

Explicitly converts a [Octo](./missingvalues/octo.md) value to a [Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte).

```csharp
public static explicit operator byte(in Octo value)
```

#### Parameters

`in` `value` [Octo](./missingvalues/octo.md)<br>
The value to convert.

#### Returns

[Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte)<br>

### **operator op_CheckedExplicit(in Octo)**

```csharp
public static byte operator op_CheckedExplicit(in Octo value)
```

#### Parameters

`in` `value` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte)<br>

### **explicit operator ushort(in Octo)**

Explicitly converts a [Octo](./missingvalues/octo.md) value to a [UInt16](https://learn.microsoft.com/en-us/dotnet/api/system.uint16).

```csharp
public static explicit operator ushort(in Octo value)
```

#### Parameters

`in` `value` [Octo](./missingvalues/octo.md)<br>
The value to convert.

#### Returns

[UInt16](https://learn.microsoft.com/en-us/dotnet/api/system.uint16)<br>

### **operator op_CheckedExplicit(in Octo)**

```csharp
public static ushort operator op_CheckedExplicit(in Octo value)
```

#### Parameters

`in` `value` [Octo](./missingvalues/octo.md)<br>

#### Returns

[UInt16](https://learn.microsoft.com/en-us/dotnet/api/system.uint16)<br>

### **explicit operator uint(in Octo)**

Explicitly converts a [Octo](./missingvalues/octo.md) value to a [UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32).

```csharp
public static explicit operator uint(in Octo value)
```

#### Parameters

`in` `value` [Octo](./missingvalues/octo.md)<br>
The value to convert.

#### Returns

[UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32)<br>

### **operator op_CheckedExplicit(in Octo)**

```csharp
public static uint operator op_CheckedExplicit(in Octo value)
```

#### Parameters

`in` `value` [Octo](./missingvalues/octo.md)<br>

#### Returns

[UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32)<br>

### **explicit operator ulong(in Octo)**

Explicitly converts a [Octo](./missingvalues/octo.md) value to a [UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64).

```csharp
public static explicit operator ulong(in Octo value)
```

#### Parameters

`in` `value` [Octo](./missingvalues/octo.md)<br>
The value to convert.

#### Returns

[UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64)<br>

### **operator op_CheckedExplicit(in Octo)**

```csharp
public static ulong operator op_CheckedExplicit(in Octo value)
```

#### Parameters

`in` `value` [Octo](./missingvalues/octo.md)<br>

#### Returns

[UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64)<br>

### **explicit operator UInt128(in Octo)**

Explicitly converts a [Octo](./missingvalues/octo.md) value to a [UInt128](https://learn.microsoft.com/en-us/dotnet/api/system.uint128).

```csharp
public static explicit operator UInt128(in Octo value)
```

#### Parameters

`in` `value` [Octo](./missingvalues/octo.md)<br>
The value to convert.

#### Returns

[UInt128](https://learn.microsoft.com/en-us/dotnet/api/system.uint128)<br>

### **operator op_CheckedExplicit(in Octo)**

```csharp
public static UInt128 operator op_CheckedExplicit(in Octo value)
```

#### Parameters

`in` `value` [Octo](./missingvalues/octo.md)<br>

#### Returns

[UInt128](https://learn.microsoft.com/en-us/dotnet/api/system.uint128)<br>

### **explicit operator UInt256(in Octo)**

Explicitly converts a [Octo](./missingvalues/octo.md) value to a [UInt256](./missingvalues/uint256.md).

```csharp
public static explicit operator UInt256(in Octo value)
```

#### Parameters

`in` `value` [Octo](./missingvalues/octo.md)<br>
The value to convert.

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **operator op_CheckedExplicit(in Octo)**

```csharp
public static UInt256 operator op_CheckedExplicit(in Octo value)
```

#### Parameters

`in` `value` [Octo](./missingvalues/octo.md)<br>

#### Returns

[UInt256](./missingvalues/uint256.md)<br>

### **explicit operator UInt512(in Octo)**

Explicitly converts a [Octo](./missingvalues/octo.md) value to a [UInt512](./missingvalues/uint512.md).

```csharp
public static explicit operator UInt512(in Octo value)
```

#### Parameters

`in` `value` [Octo](./missingvalues/octo.md)<br>
The value to convert.

#### Returns

[UInt512](./missingvalues/uint512.md)<br>

### **operator op_CheckedExplicit(in Octo)**

```csharp
public static UInt512 operator op_CheckedExplicit(in Octo value)
```

#### Parameters

`in` `value` [Octo](./missingvalues/octo.md)<br>

#### Returns

[UInt512](./missingvalues/uint512.md)<br>

### **explicit operator sbyte(in Octo)**

Explicitly converts a [Octo](./missingvalues/octo.md) value to a [SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte).

```csharp
public static explicit operator sbyte(in Octo value)
```

#### Parameters

`in` `value` [Octo](./missingvalues/octo.md)<br>
The value to convert.

#### Returns

[SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte)<br>

### **operator op_CheckedExplicit(in Octo)**

```csharp
public static sbyte operator op_CheckedExplicit(in Octo value)
```

#### Parameters

`in` `value` [Octo](./missingvalues/octo.md)<br>

#### Returns

[SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte)<br>

### **explicit operator short(in Octo)**

Explicitly converts a [Octo](./missingvalues/octo.md) value to a [Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16).

```csharp
public static explicit operator short(in Octo value)
```

#### Parameters

`in` `value` [Octo](./missingvalues/octo.md)<br>
The value to convert.

#### Returns

[Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16)<br>

### **operator op_CheckedExplicit(in Octo)**

```csharp
public static short operator op_CheckedExplicit(in Octo value)
```

#### Parameters

`in` `value` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16)<br>

### **explicit operator int(in Octo)**

Explicitly converts a [Octo](./missingvalues/octo.md) value to a [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32).

```csharp
public static explicit operator int(in Octo value)
```

#### Parameters

`in` `value` [Octo](./missingvalues/octo.md)<br>
The value to convert.

#### Returns

[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>

### **operator op_CheckedExplicit(in Octo)**

```csharp
public static int operator op_CheckedExplicit(in Octo value)
```

#### Parameters

`in` `value` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>

### **explicit operator long(in Octo)**

Explicitly converts a [Octo](./missingvalues/octo.md) value to a [Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64).

```csharp
public static explicit operator long(in Octo value)
```

#### Parameters

`in` `value` [Octo](./missingvalues/octo.md)<br>
The value to convert.

#### Returns

[Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64)<br>

### **operator op_CheckedExplicit(in Octo)**

```csharp
public static long operator op_CheckedExplicit(in Octo value)
```

#### Parameters

`in` `value` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64)<br>

### **explicit operator Int128(in Octo)**

Explicitly converts a [Octo](./missingvalues/octo.md) value to a [Int128](https://learn.microsoft.com/en-us/dotnet/api/system.int128).

```csharp
public static explicit operator Int128(in Octo value)
```

#### Parameters

`in` `value` [Octo](./missingvalues/octo.md)<br>
The value to convert.

#### Returns

[Int128](https://learn.microsoft.com/en-us/dotnet/api/system.int128)<br>

### **operator op_CheckedExplicit(in Octo)**

```csharp
public static Int128 operator op_CheckedExplicit(in Octo value)
```

#### Parameters

`in` `value` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Int128](https://learn.microsoft.com/en-us/dotnet/api/system.int128)<br>

### **explicit operator Int256(in Octo)**

Explicitly converts a [Octo](./missingvalues/octo.md) value to a [Int256](./missingvalues/int256.md).

```csharp
public static explicit operator Int256(in Octo value)
```

#### Parameters

`in` `value` [Octo](./missingvalues/octo.md)<br>
The value to convert.

#### Returns

[Int256](./missingvalues/int256.md)<br>

### **operator op_CheckedExplicit(in Octo)**

```csharp
public static Int256 operator op_CheckedExplicit(in Octo value)
```

#### Parameters

`in` `value` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Int256](./missingvalues/int256.md)<br>

### **explicit operator Int512(in Octo)**

Explicitly converts a [Octo](./missingvalues/octo.md) value to a [Int512](./missingvalues/int512.md).

```csharp
public static explicit operator Int512(in Octo value)
```

#### Parameters

`in` `value` [Octo](./missingvalues/octo.md)<br>
The value to convert.

#### Returns

[Int512](./missingvalues/int512.md)<br>

### **operator op_CheckedExplicit(in Octo)**

```csharp
public static Int512 operator op_CheckedExplicit(in Octo value)
```

#### Parameters

`in` `value` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Int512](./missingvalues/int512.md)<br>

### **explicit operator BigInteger(in Octo)**

Explicitly converts a [Octo](./missingvalues/octo.md) value to a [System.Numerics.BigInteger](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.biginteger).

```csharp
public static explicit operator BigInteger(in Octo value)
```

#### Parameters

`in` `value` [Octo](./missingvalues/octo.md)<br>
The value to convert.

#### Returns

[BigInteger](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.biginteger)<br>

#### Exceptions

[OverflowException](https://learn.microsoft.com/en-us/dotnet/api/system.overflowexception)<br>
`value` is not finite.

### **explicit operator decimal(in Octo)**

Explicitly converts a [Octo](./missingvalues/octo.md) value to a [Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal).

```csharp
public static explicit operator decimal(in Octo value)
```

#### Parameters

`in` `value` [Octo](./missingvalues/octo.md)<br>
The value to convert.

#### Returns

[Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal)<br>

### **explicit operator Quad(in Octo)**

Explicitly converts a [Octo](./missingvalues/octo.md) value to a [Quad](./missingvalues/quad.md).

```csharp
public static explicit operator Quad(in Octo value)
```

#### Parameters

`in` `value` [Octo](./missingvalues/octo.md)<br>
The value to convert.

#### Returns

[Quad](./missingvalues/quad.md)<br>

### **explicit operator double(in Octo)**

Explicitly converts a [Octo](./missingvalues/octo.md) value to a [Double](https://learn.microsoft.com/en-us/dotnet/api/system.double).

```csharp
public static explicit operator double(in Octo value)
```

#### Parameters

`in` `value` [Octo](./missingvalues/octo.md)<br>
The value to convert.

#### Returns

[Double](https://learn.microsoft.com/en-us/dotnet/api/system.double)<br>

### **explicit operator float(in Octo)**

Explicitly converts a [Octo](./missingvalues/octo.md) value to a [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single).

```csharp
public static explicit operator float(in Octo value)
```

#### Parameters

`in` `value` [Octo](./missingvalues/octo.md)<br>
The value to convert.

#### Returns

[Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)<br>

### **explicit operator Half(in Octo)**

Explicitly converts a [Octo](./missingvalues/octo.md) value to a [Half](https://learn.microsoft.com/en-us/dotnet/api/system.half).

```csharp
public static explicit operator Half(in Octo value)
```

#### Parameters

`in` `value` [Octo](./missingvalues/octo.md)<br>
The value to convert.

#### Returns

[Half](https://learn.microsoft.com/en-us/dotnet/api/system.half)<br>

### **implicit operator Octo(Byte)**

Implicitly converts a [Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte) value to a [Octo](./missingvalues/octo.md).

```csharp
public static implicit operator Octo(byte value)
```

#### Parameters

`value` [Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte)<br>
The value to convert.

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **implicit operator Octo(UInt16)**

Implicitly converts a [UInt16](https://learn.microsoft.com/en-us/dotnet/api/system.uint16) value to a [Octo](./missingvalues/octo.md).

```csharp
public static implicit operator Octo(ushort value)
```

#### Parameters

`value` [UInt16](https://learn.microsoft.com/en-us/dotnet/api/system.uint16)<br>
The value to convert.

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **implicit operator Octo(UInt32)**

Implicitly converts a [UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32) value to a [Octo](./missingvalues/octo.md).

```csharp
public static implicit operator Octo(uint value)
```

#### Parameters

`value` [UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32)<br>
The value to convert.

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **implicit operator Octo(UInt64)**

Implicitly converts a [UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64) value to a [Octo](./missingvalues/octo.md).

```csharp
public static implicit operator Octo(ulong value)
```

#### Parameters

`value` [UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64)<br>
The value to convert.

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **implicit operator Octo(UInt128)**

Implicitly converts a [UInt128](https://learn.microsoft.com/en-us/dotnet/api/system.uint128) value to a [Octo](./missingvalues/octo.md).

```csharp
public static implicit operator Octo(UInt128 value)
```

#### Parameters

`value` [UInt128](https://learn.microsoft.com/en-us/dotnet/api/system.uint128)<br>
The value to convert.

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **implicit operator Octo(SByte)**

Implicitly converts a [SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte) value to a [Octo](./missingvalues/octo.md).

```csharp
public static implicit operator Octo(sbyte value)
```

#### Parameters

`value` [SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte)<br>
The value to convert.

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **implicit operator Octo(Int16)**

Implicitly converts a [Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16) value to a [Octo](./missingvalues/octo.md).

```csharp
public static implicit operator Octo(short value)
```

#### Parameters

`value` [Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16)<br>
The value to convert.

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **implicit operator Octo(Int32)**

Implicitly converts a [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) value to a [Octo](./missingvalues/octo.md).

```csharp
public static implicit operator Octo(int value)
```

#### Parameters

`value` [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>
The value to convert.

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **implicit operator Octo(Int64)**

Implicitly converts a [Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) value to a [Octo](./missingvalues/octo.md).

```csharp
public static implicit operator Octo(long value)
```

#### Parameters

`value` [Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64)<br>
The value to convert.

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **implicit operator Octo(Int128)**

Implicitly converts a [Int128](https://learn.microsoft.com/en-us/dotnet/api/system.int128) value to a [Octo](./missingvalues/octo.md).

```csharp
public static implicit operator Octo(Int128 value)
```

#### Parameters

`value` [Int128](https://learn.microsoft.com/en-us/dotnet/api/system.int128)<br>
The value to convert.

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **explicit operator Octo(BigInteger)**

Explicitly converts a [System.Numerics.BigInteger](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.biginteger) value to a [Octo](./missingvalues/octo.md).

```csharp
public static explicit operator Octo(BigInteger value)
```

#### Parameters

`value` [BigInteger](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.biginteger)<br>
The value to convert.

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **implicit operator Octo(Decimal)**

Implicitly converts a [Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal) value to a [Octo](./missingvalues/octo.md).

```csharp
public static implicit operator Octo(decimal value)
```

#### Parameters

`value` [Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal)<br>
The value to convert.

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **implicit operator Octo(Double)**

Implicitly converts a [Double](https://learn.microsoft.com/en-us/dotnet/api/system.double) value to a [Octo](./missingvalues/octo.md).

```csharp
public static implicit operator Octo(double value)
```

#### Parameters

`value` [Double](https://learn.microsoft.com/en-us/dotnet/api/system.double)<br>
The value to convert.

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **implicit operator Octo(Single)**

Implicitly converts a [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single) value to a [Octo](./missingvalues/octo.md).

```csharp
public static implicit operator Octo(float value)
```

#### Parameters

`value` [Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)<br>
The value to convert.

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **implicit operator Octo(Half)**

Implicitly converts a [Half](https://learn.microsoft.com/en-us/dotnet/api/system.half) value to a [Octo](./missingvalues/octo.md).

```csharp
public static implicit operator Octo(Half value)
```

#### Parameters

`value` [Half](https://learn.microsoft.com/en-us/dotnet/api/system.half)<br>
The value to convert.

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **operator +(in Octo)**

```csharp
public static Octo operator +(in Octo value)
```

#### Parameters

`in` `value` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **operator +(in Octo, in Octo)**

```csharp
public static Octo operator +(in Octo left, in Octo right)
```

#### Parameters

`in` `left` [Octo](./missingvalues/octo.md)<br>

`in` `right` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **operator -(in Octo)**

```csharp
public static Octo operator -(in Octo value)
```

#### Parameters

`in` `value` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **operator -(in Octo, in Octo)**

```csharp
public static Octo operator -(in Octo left, in Octo right)
```

#### Parameters

`in` `left` [Octo](./missingvalues/octo.md)<br>

`in` `right` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **operator ++(in Octo)**

```csharp
public static Octo operator ++(in Octo value)
```

#### Parameters

`in` `value` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **operator --(in Octo)**

```csharp
public static Octo operator --(in Octo value)
```

#### Parameters

`in` `value` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **operator *(in Octo, in Octo)**

```csharp
public static Octo operator *(in Octo left, in Octo right)
```

#### Parameters

`in` `left` [Octo](./missingvalues/octo.md)<br>

`in` `right` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **operator /(in Octo, in Octo)**

```csharp
public static Octo operator /(in Octo left, in Octo right)
```

#### Parameters

`in` `left` [Octo](./missingvalues/octo.md)<br>

`in` `right` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **operator %(in Octo, in Octo)**

```csharp
public static Octo operator %(in Octo left, in Octo right)
```

#### Parameters

`in` `left` [Octo](./missingvalues/octo.md)<br>

`in` `right` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Octo](./missingvalues/octo.md)<br>

### **operator ==(in Octo, in Octo)**

```csharp
public static bool operator ==(in Octo left, in Octo right)
```

#### Parameters

`in` `left` [Octo](./missingvalues/octo.md)<br>

`in` `right` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **operator !=(in Octo, in Octo)**

```csharp
public static bool operator !=(in Octo left, in Octo right)
```

#### Parameters

`in` `left` [Octo](./missingvalues/octo.md)<br>

`in` `right` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **operator &lt;(in Octo, in Octo)**

```csharp
public static bool operator <(in Octo left, in Octo right)
```

#### Parameters

`in` `left` [Octo](./missingvalues/octo.md)<br>

`in` `right` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **operator &gt;(in Octo, in Octo)**

```csharp
public static bool operator >(in Octo left, in Octo right)
```

#### Parameters

`in` `left` [Octo](./missingvalues/octo.md)<br>

`in` `right` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **operator &lt;=(in Octo, in Octo)**

```csharp
public static bool operator <=(in Octo left, in Octo right)
```

#### Parameters

`in` `left` [Octo](./missingvalues/octo.md)<br>

`in` `right` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **operator &gt;=(in Octo, in Octo)**

```csharp
public static bool operator >=(in Octo left, in Octo right)
```

#### Parameters

`in` `left` [Octo](./missingvalues/octo.md)<br>

`in` `right` [Octo](./missingvalues/octo.md)<br>

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>

---

[`< Back`](../)
