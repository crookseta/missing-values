[`< Back`](../)

---

# MathQ

Namespace: MissingValues

Provides constants and static methods for trigonometric, logarithmic, and other common mathematical functions.

```csharp
public static class MathQ
```

Inheritance [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) → [MathQ](./missingvalues/mathq.md)

## Methods

### **BigMul(UInt128, UInt128)**

Produces the full product of two unsigned 128-bit numbers.

```csharp
public static UInt256 BigMul(UInt128 a, UInt128 b)
```

#### Parameters

`a` [UInt128](https://learn.microsoft.com/en-us/dotnet/api/system.uint128)<br>
The first number to multiply.

`b` [UInt128](https://learn.microsoft.com/en-us/dotnet/api/system.uint128)<br>
The second number to multiply.

#### Returns

[UInt256](./missingvalues/uint256.md)<br>
The full product of the specified numbers.

### **BigMul(Int128, Int128)**

Produces the full product of two 128-bit numbers.

```csharp
public static Int256 BigMul(Int128 a, Int128 b)
```

#### Parameters

`a` [Int128](https://learn.microsoft.com/en-us/dotnet/api/system.int128)<br>
The first number to multiply.

`b` [Int128](https://learn.microsoft.com/en-us/dotnet/api/system.int128)<br>
The second number to multiply.

#### Returns

[Int256](./missingvalues/int256.md)<br>
The full product of the specified numbers.

### **BigMul(UInt256, UInt256)**

Produces the full product of two unsigned 256-bit numbers.

```csharp
public static UInt512 BigMul(UInt256 a, UInt256 b)
```

#### Parameters

`a` [UInt256](./missingvalues/uint256.md)<br>
The first number to multiply.

`b` [UInt256](./missingvalues/uint256.md)<br>
The second number to multiply.

#### Returns

[UInt512](./missingvalues/uint512.md)<br>
The full product of the specified numbers.

### **BigMul(Int256, Int256)**

Produces the full product of two 256-bit numbers.

```csharp
public static Int512 BigMul(Int256 a, Int256 b)
```

#### Parameters

`a` [Int256](./missingvalues/int256.md)<br>
The first number to multiply.

`b` [Int256](./missingvalues/int256.md)<br>
The second number to multiply.

#### Returns

[Int512](./missingvalues/int512.md)<br>
The full product of the specified numbers.

### **Abs(Quad)**

Returns the absolute value of a quadruple-precision floating-point number.

```csharp
public static Quad Abs(Quad x)
```

#### Parameters

`x` [Quad](./missingvalues/quad.md)<br>
A number that is greater than or equal to [Quad.MinValue](./missingvalues/quad.md#minvalue), but less than or equal to [Quad.MaxValue](./missingvalues/quad.md#maxvalue).

#### Returns

[Quad](./missingvalues/quad.md)<br>
A quadruple-precision floating-point number, x, such that 0 ≤ x ≤ [Quad.MaxValue](./missingvalues/quad.md#maxvalue).

### **Acos(Quad)**

Returns the angle whose cosine is the specified number.

```csharp
public static Quad Acos(Quad x)
```

#### Parameters

`x` [Quad](./missingvalues/quad.md)<br>
A number representing a cosine, where `x` must be greater than or equal to -1, but less than or equal to 1.

#### Returns

[Quad](./missingvalues/quad.md)<br>
An angle, θ, measured in radians, such that 0 ≤ θ ≤ π.

### **Acosh(Quad)**

Returns the angle whose hyperbolic cosine is the specified number.

```csharp
public static Quad Acosh(Quad x)
```

#### Parameters

`x` [Quad](./missingvalues/quad.md)<br>
A number representing a hyperbolic cosine, where `x` must be greater than or equal to 1, but less than or equal to [Quad.PositiveInfinity](./missingvalues/quad.md#positiveinfinity).

#### Returns

[Quad](./missingvalues/quad.md)<br>
An angle, θ, measured in radians, such that 0 ≤ θ ≤ ∞.

### **Asin(Quad)**

Returns the angle whose sine is the specified number.

```csharp
public static Quad Asin(Quad x)
```

#### Parameters

`x` [Quad](./missingvalues/quad.md)<br>
A number representing a sine, where `x` must be greater than or equal to -1, but less than or equal to 1.

#### Returns

[Quad](./missingvalues/quad.md)<br>
An angle, θ, measured in radians, such that -π/2 ≤ θ ≤ π/2.

### **Asinh(Quad)**

Returns the angle whose hyperbolic sine is the specified number.

```csharp
public static Quad Asinh(Quad x)
```

#### Parameters

`x` [Quad](./missingvalues/quad.md)<br>
A number representing a hyperbolic sine, where `x` must be greater than or equal to [Quad.NegativeInfinity](./missingvalues/quad.md#negativeinfinity), but less than or equal to [Quad.PositiveInfinity](./missingvalues/quad.md#positiveinfinity).

#### Returns

[Quad](./missingvalues/quad.md)<br>
An angle, θ, measured in radians.

### **Atan(Quad)**

Returns the angle whose tangent is the specified number.

```csharp
public static Quad Atan(Quad x)
```

#### Parameters

`x` [Quad](./missingvalues/quad.md)<br>
A number representing a tangent.

#### Returns

[Quad](./missingvalues/quad.md)<br>
An angle, θ, measured in radians, such that -π/2 ≤ θ ≤ π/2.

### **Atan2(Quad, Quad)**

Returns the angle whose tangent is the quotient of two specified numbers.

```csharp
public static Quad Atan2(Quad y, Quad x)
```

#### Parameters

`y` [Quad](./missingvalues/quad.md)<br>
The y coordinate of a point.

`x` [Quad](./missingvalues/quad.md)<br>
The x coordinate of a point.

#### Returns

[Quad](./missingvalues/quad.md)<br>
An angle, θ, measured in radians, such that -π ≤ θ ≤ π, and tan(θ) = `y` / `x`, where (`x`, `y`) is a point in the Cartesian plane.

### **Atanh(Quad)**

Returns the angle whose hyperbolic tangent is the specified number.

```csharp
public static Quad Atanh(Quad x)
```

#### Parameters

`x` [Quad](./missingvalues/quad.md)<br>
A number representing a hyperbolic tangent, where `x` must be greater than or equal to -1, but less than or equal to 1.

#### Returns

[Quad](./missingvalues/quad.md)<br>
An angle, θ, measured in radians.

### **BitDecrement(Quad)**

Returns the largest value that compares less than a specified value.

```csharp
public static Quad BitDecrement(Quad x)
```

#### Parameters

`x` [Quad](./missingvalues/quad.md)<br>
The value to decrement.

#### Returns

[Quad](./missingvalues/quad.md)<br>
The largest value that compares less than `x`.

### **BitIncrement(Quad)**

Returns the smallest value that compares greater than a specified value.

```csharp
public static Quad BitIncrement(Quad x)
```

#### Parameters

`x` [Quad](./missingvalues/quad.md)<br>
The value to increment.

#### Returns

[Quad](./missingvalues/quad.md)<br>
The smallest value that compares greater than `x`.

### **Cbrt(Quad)**

Returns the cube root of a specified number.

```csharp
public static Quad Cbrt(Quad x)
```

#### Parameters

`x` [Quad](./missingvalues/quad.md)<br>
The number whose cube root is to be found.

#### Returns

[Quad](./missingvalues/quad.md)<br>
The cube root of `x`.

### **Ceiling(Quad)**

Returns the smallest integral value that is greater than or equal to the specified quadruple-precision floating-point number.

```csharp
public static Quad Ceiling(Quad x)
```

#### Parameters

`x` [Quad](./missingvalues/quad.md)<br>
A quadruple-precision floating-point

#### Returns

[Quad](./missingvalues/quad.md)<br>
The smallest integral value that is greater than or equal to `x`. If `x` is equal to [Quad.NaN](./missingvalues/quad.md#nan), [Quad.NegativeInfinity](./missingvalues/quad.md#negativeinfinity), or [Quad.PositiveInfinity](./missingvalues/quad.md#positiveinfinity), that value is returned. Note that this method returns a [Quad](./missingvalues/quad.md) instead of an integral type.

### **Clamp(Quad, Quad, Quad)**

Returns `value` clamped to the inclusive range of `min` and `max`.

```csharp
public static Quad Clamp(Quad value, Quad min, Quad max)
```

#### Parameters

`value` [Quad](./missingvalues/quad.md)<br>
The value to be clamped

`min` [Quad](./missingvalues/quad.md)<br>
The lower bound of the result

`max` [Quad](./missingvalues/quad.md)<br>
The upper bound of the result

#### Returns

[Quad](./missingvalues/quad.md)<br>
`value` if `min` ≤ `value` ≤ `max`.

### **CopySign(Quad, Quad)**

Returns a value with the magnitude of `x` and the sign of `y`.

```csharp
public static Quad CopySign(Quad x, Quad y)
```

#### Parameters

`x` [Quad](./missingvalues/quad.md)<br>
A number whose magnitude is used in the result.

`y` [Quad](./missingvalues/quad.md)<br>
A number whose sign is the used in the result.

#### Returns

[Quad](./missingvalues/quad.md)<br>
A value with the magnitude of `x` and the sign of `y`.

### **Cos(Quad)**

Returns the cosine of the specified angle.

```csharp
public static Quad Cos(Quad x)
```

#### Parameters

`x` [Quad](./missingvalues/quad.md)<br>
An angle, measured in radians.

#### Returns

[Quad](./missingvalues/quad.md)<br>
The cosine of `x`. If `x` is equal to [Quad.NaN](./missingvalues/quad.md#nan), [Quad.NegativeInfinity](./missingvalues/quad.md#negativeinfinity), or [Quad.PositiveInfinity](./missingvalues/quad.md#positiveinfinity), this method returns [Quad.NaN](./missingvalues/quad.md#nan).

### **Cosh(Quad)**

Returns the hyperbolic cosine of the specified angle.

```csharp
public static Quad Cosh(Quad x)
```

#### Parameters

`x` [Quad](./missingvalues/quad.md)<br>
An angle, measured in radians.

#### Returns

[Quad](./missingvalues/quad.md)<br>
The hyperbolic cosine of `x`. If `x` is equal to [Quad.NegativeInfinity](./missingvalues/quad.md#negativeinfinity) or [Quad.PositiveInfinity](./missingvalues/quad.md#positiveinfinity), [Quad.PositiveInfinity](./missingvalues/quad.md#positiveinfinity) is returned. If `x` is equal to [Quad.NaN](./missingvalues/quad.md#nan), [Quad.NaN](./missingvalues/quad.md#nan) is returned.

### **Exp(Quad)**

Returns [Quad.E](./missingvalues/quad.md#e) raised to the specified power.

```csharp
public static Quad Exp(Quad x)
```

#### Parameters

`x` [Quad](./missingvalues/quad.md)<br>
A number specifying a power.

#### Returns

[Quad](./missingvalues/quad.md)<br>
The number [Quad.E](./missingvalues/quad.md#e) raised to the power `x`. If `x` equals [Quad.NaN](./missingvalues/quad.md#nan) or [Quad.PositiveInfinity](./missingvalues/quad.md#positiveinfinity), that value is returned. If `x` equals [Quad.NegativeInfinity](./missingvalues/quad.md#negativeinfinity), 0 is returned.

### **Floor(Quad)**

Returns the largest integral value less than or equal to the specified quadruple-precision floating-point number.

```csharp
public static Quad Floor(Quad x)
```

#### Parameters

`x` [Quad](./missingvalues/quad.md)<br>
A quadruple-precision floating-point number

#### Returns

[Quad](./missingvalues/quad.md)<br>
The largest integral value less than or equal to `x`. If `x` is equal to [Quad.NaN](./missingvalues/quad.md#nan), [Quad.NegativeInfinity](./missingvalues/quad.md#negativeinfinity), or [Quad.PositiveInfinity](./missingvalues/quad.md#positiveinfinity), that value is returned.

### **FusedMultiplyAdd(Quad, Quad, Quad)**

Returns (x * y) + z, rounded as one ternary operation.

```csharp
public static Quad FusedMultiplyAdd(Quad x, Quad y, Quad z)
```

#### Parameters

`x` [Quad](./missingvalues/quad.md)<br>
The number to be multiplied with `y`.

`y` [Quad](./missingvalues/quad.md)<br>
The number to be multiplied with `x`.

`z` [Quad](./missingvalues/quad.md)<br>
The number to be added to the result of `x` multiplied by `y`.

#### Returns

[Quad](./missingvalues/quad.md)<br>
(x * y) + z, rounded as one ternary operation.

### **IEEERemainder(Quad, Quad)**

Returns the remainder resulting from the division of a specified number by another specified number.

```csharp
public static Quad IEEERemainder(Quad x, Quad y)
```

#### Parameters

`x` [Quad](./missingvalues/quad.md)<br>
A dividend.

`y` [Quad](./missingvalues/quad.md)<br>
A divisor.

#### Returns

[Quad](./missingvalues/quad.md)<br>
A number equal to `x` - (`y` Q), where Q is the quotient of `x` / `y` rounded to the nearest integer

### **ILogB(Quad)**

Returns the base 2 integer logarithm of a specified number.

```csharp
public static int ILogB(Quad x)
```

#### Parameters

`x` [Quad](./missingvalues/quad.md)<br>
The number whose logarithm is to be found.

#### Returns

[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>
The base 2 integer log of `x`; that is, (int)log2(`x`).

### **Log(Quad)**

Returns the natural (base `e`) logarithm of a specified number.

```csharp
public static Quad Log(Quad x)
```

#### Parameters

`x` [Quad](./missingvalues/quad.md)<br>
The number whose logarithm is to be found.

#### Returns

[Quad](./missingvalues/quad.md)<br>
If positive, 	The natural logarithm of `x`; that is, ln `x`, or log e `x`

### **Log(Quad, Quad)**

Returns the logarithm of a specified number in a specified base.

```csharp
public static Quad Log(Quad a, Quad newBase)
```

#### Parameters

`a` [Quad](./missingvalues/quad.md)<br>
The number whose logarithm is to be found.

`newBase` [Quad](./missingvalues/quad.md)<br>
The base.

#### Returns

[Quad](./missingvalues/quad.md)<br>

### **Log10(Quad)**

Returns the base 10 logarithm of a specified number.

```csharp
public static Quad Log10(Quad x)
```

#### Parameters

`x` [Quad](./missingvalues/quad.md)<br>
A number whose logarithm is to be found.

#### Returns

[Quad](./missingvalues/quad.md)<br>
The base 10 log of `x`; that is, log 10`x`.

### **Log2(Quad)**

Returns the base 2 logarithm of a specified number.

```csharp
public static Quad Log2(Quad x)
```

#### Parameters

`x` [Quad](./missingvalues/quad.md)<br>
A number whose logarithm is to be found.

#### Returns

[Quad](./missingvalues/quad.md)<br>
The base 2 log of `x`; that is, log 2`x`.

### **Max(Quad, Quad)**

Returns the larger of two quadruple-precision floating-point numbers.

```csharp
public static Quad Max(Quad val1, Quad val2)
```

#### Parameters

`val1` [Quad](./missingvalues/quad.md)<br>
The first of two quadruple-precision floating-point numbers to compare.

`val2` [Quad](./missingvalues/quad.md)<br>
The second of two quadruple-precision floating-point numbers to compare.

#### Returns

[Quad](./missingvalues/quad.md)<br>
Parameter `val1` or `val2`, whichever is larger. If `val1`, or `val2`, or both `val1` and `val2` are equal to [Quad.NaN](./missingvalues/quad.md#nan), [Quad.NaN](./missingvalues/quad.md#nan) is returned.

### **MaxMagnitude(Quad, Quad)**

Returns the larger magnitude of two quadruple-precision floating-point numbers.

```csharp
public static Quad MaxMagnitude(Quad x, Quad y)
```

#### Parameters

`x` [Quad](./missingvalues/quad.md)<br>
The first of two quadruple-precision floating-point numbers to compare.

`y` [Quad](./missingvalues/quad.md)<br>
The second of two quadruple-precision floating-point numbers to compare.

#### Returns

[Quad](./missingvalues/quad.md)<br>
Parameter `x` or `y`, whichever has the larger magnitude. If `x`, or `y`, or both `x` and `y` are equal to [Quad.NaN](./missingvalues/quad.md#nan), [Quad.NaN](./missingvalues/quad.md#nan) is returned.

### **Min(Quad, Quad)**

Returns the smaller of two quadruple-precision floating-point numbers.

```csharp
public static Quad Min(Quad val1, Quad val2)
```

#### Parameters

`val1` [Quad](./missingvalues/quad.md)<br>
The first of two quadruple-precision floating-point numbers to compare.

`val2` [Quad](./missingvalues/quad.md)<br>
The second of two quadruple-precision floating-point numbers to compare.

#### Returns

[Quad](./missingvalues/quad.md)<br>
Parameter `val1` or `val2`, whichever is smaller. If `val1`, `val2`, or both `val1` and `val2` are equal to [Quad.NaN](./missingvalues/quad.md#nan), [Quad.NaN](./missingvalues/quad.md#nan) is returned.

### **MinMagnitude(Quad, Quad)**

Returns the smaller magnitude of two quadruple-precision floating-point numbers.

```csharp
public static Quad MinMagnitude(Quad x, Quad y)
```

#### Parameters

`x` [Quad](./missingvalues/quad.md)<br>
The first of two quadruple-precision floating-point numbers to compare.

`y` [Quad](./missingvalues/quad.md)<br>
The second of two quadruple-precision floating-point numbers to compare.

#### Returns

[Quad](./missingvalues/quad.md)<br>
Parameter `x` or `y`, whichever has the smaller magnitude. If `x`, or `y`, or both `x` and `y` are equal to [Quad.NaN](./missingvalues/quad.md#nan), [Quad.NaN](./missingvalues/quad.md#nan) is returned.

### **Pow(Quad, Quad)**

Returns a specified number raised to the specified power.

```csharp
public static Quad Pow(Quad x, Quad y)
```

#### Parameters

`x` [Quad](./missingvalues/quad.md)<br>
A quadruple-precision floating-point number to be raised to a power.

`y` [Quad](./missingvalues/quad.md)<br>
A quadruple-precision floating-point number that specifies a power.

#### Returns

[Quad](./missingvalues/quad.md)<br>
The number `x` raised to the power `y`.

### **ReciprocalEstimate(Quad)**

Returns an estimate of the reciprocal of a specified number.

```csharp
public static Quad ReciprocalEstimate(Quad x)
```

#### Parameters

`x` [Quad](./missingvalues/quad.md)<br>
The number whose reciprocal is to be estimated.

#### Returns

[Quad](./missingvalues/quad.md)<br>
An estimate of the reciprocal of `x`.

### **ReciprocalSqrtEstimate(Quad)**

Returns an estimate of the reciprocal square root of a specified number.

```csharp
public static Quad ReciprocalSqrtEstimate(Quad x)
```

#### Parameters

`x` [Quad](./missingvalues/quad.md)<br>
The number whose reciprocal square root is to be estimated.

#### Returns

[Quad](./missingvalues/quad.md)<br>
An estimate of the reciprocal square root `x`.

### **Round(Quad)**

Rounds a quadruple-precision floating-point value to the nearest integral value, and rounds midpoint values to the nearest even number.

```csharp
public static Quad Round(Quad x)
```

#### Parameters

`x` [Quad](./missingvalues/quad.md)<br>
A quadruple-precision floating-point number to be rounded.

#### Returns

[Quad](./missingvalues/quad.md)<br>
The integer nearest `x`. If the fractional component of `x` is halfway between two integers, one of which is even and the other odd, then the even number is returned.

### **Round(Quad, Int32)**

Rounds a quadruple-precision floating-point value to a specified number of fractional digits, and rounds midpoint values to the nearest even number.

```csharp
public static Quad Round(Quad x, int digits)
```

#### Parameters

`x` [Quad](./missingvalues/quad.md)<br>
A quadruple-precision floating-point number to be rounded.

`digits` [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>
The number of fractional digits in the return value.

#### Returns

[Quad](./missingvalues/quad.md)<br>
The number nearest to `x` that contains a number of fractional digits equal to `digits`.

#### Exceptions

[ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception)<br>
`digits` is less than 0 or greater than 34.

### **Round(Quad, MidpointRounding)**

Rounds a quadruple-precision floating-point value to an integer using the specified rounding convention.

```csharp
public static Quad Round(Quad x, MidpointRounding mode)
```

#### Parameters

`x` [Quad](./missingvalues/quad.md)<br>
A quadruple-precision floating-point number to be rounded.

`mode` [MidpointRounding](https://learn.microsoft.com/en-us/dotnet/api/system.midpointrounding)<br>
One of the enumeration values that specifies which rounding strategy to use.

#### Returns

[Quad](./missingvalues/quad.md)<br>
The integer that `x` is rounded to using the `mode` rounding convention. This method returns a [Quad](./missingvalues/quad.md) instead of an integral type.

#### Exceptions

[ArgumentException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentexception)<br>
`mode` is not a valid value of [MidpointRounding](https://learn.microsoft.com/en-us/dotnet/api/system.midpointrounding).

### **Round(Quad, Int32, MidpointRounding)**

Rounds a quadruple-precision floating-point value to a specified number of fractional digits using the specified rounding convention.

```csharp
public static Quad Round(Quad x, int digits, MidpointRounding mode)
```

#### Parameters

`x` [Quad](./missingvalues/quad.md)<br>
A quadruple-precision floating-point number to be rounded.

`digits` [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>
The number of fractional digits in the return value.

`mode` [MidpointRounding](https://learn.microsoft.com/en-us/dotnet/api/system.midpointrounding)<br>
One of the enumeration values that specifies which rounding strategy to use.

#### Returns

[Quad](./missingvalues/quad.md)<br>
The number that `x` is rounded to that has `digits` fractional digits. If `x` has fewer fractional digits than `digits`, `x` is returned unchanged.

#### Exceptions

[ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception)<br>
`digits` is less than 0 or greater than 34.

[ArgumentException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentexception)<br>
`mode` is not a valid value of [MidpointRounding](https://learn.microsoft.com/en-us/dotnet/api/system.midpointrounding).

### **ScaleB(Quad, Int32)**

Returns x * 2^n computed efficiently.

```csharp
public static Quad ScaleB(Quad x, int n)
```

#### Parameters

`x` [Quad](./missingvalues/quad.md)<br>
A quadruple-precision floating-point number that specifies the base value.

`n` [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>
A number that specifies the power.

#### Returns

[Quad](./missingvalues/quad.md)<br>
x * 2^n computed efficiently.

### **Sign(Quad)**

Returns an integer that indicates the sign of a quadruple-precision floating-point number.

```csharp
public static int Sign(Quad x)
```

#### Parameters

`x` [Quad](./missingvalues/quad.md)<br>
A signed number.

#### Returns

[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>
A number that indicates the sign of `x`.

#### Exceptions

[ArithmeticException](https://learn.microsoft.com/en-us/dotnet/api/system.arithmeticexception)<br>
`x` is equal to [Quad.NaN](./missingvalues/quad.md#nan).

### **Sin(Quad)**

Returns the sine of the specified angle.

```csharp
public static Quad Sin(Quad x)
```

#### Parameters

`x` [Quad](./missingvalues/quad.md)<br>
An angle, measured in radians.

#### Returns

[Quad](./missingvalues/quad.md)<br>
The sine of `x`. If `x` is equal to [Quad.NaN](./missingvalues/quad.md#nan), [Quad.NegativeInfinity](./missingvalues/quad.md#negativeinfinity), or [Quad.PositiveInfinity](./missingvalues/quad.md#positiveinfinity), this method returns [Quad.NaN](./missingvalues/quad.md#nan).

### **SinCos(Quad)**

Returns the sine and cosine of the specified angle.

```csharp
public static (Quad Sin, Quad Cos) SinCos(Quad x)
```

#### Parameters

`x` [Quad](./missingvalues/quad.md)<br>
An angle, measured in radians.

#### Returns

`(Quad Sin, Quad Cos)`<br>
The sine and cosine of `x`. If `x` is equal to [Quad.NaN](./missingvalues/quad.md#nan), [Quad.NegativeInfinity](./missingvalues/quad.md#negativeinfinity), or [Quad.PositiveInfinity](./missingvalues/quad.md#positiveinfinity), this method returns [Quad.NaN](./missingvalues/quad.md#nan).

### **Sinh(Quad)**

Returns the hyperbolic sine of the specified angle.

```csharp
public static Quad Sinh(Quad x)
```

#### Parameters

`x` [Quad](./missingvalues/quad.md)<br>
An angle, measured in radians.

#### Returns

[Quad](./missingvalues/quad.md)<br>
The hyperbolic sine of `x`. If `x` is equal to [Quad.NaN](./missingvalues/quad.md#nan), [Quad.NegativeInfinity](./missingvalues/quad.md#negativeinfinity), or [Quad.PositiveInfinity](./missingvalues/quad.md#positiveinfinity), this method returns a [Quad](./missingvalues/quad.md) equal to `x`.

### **Sqrt(Quad)**

Returns the square root of a specified number.

```csharp
public static Quad Sqrt(Quad x)
```

#### Parameters

`x` [Quad](./missingvalues/quad.md)<br>
The number whose square root is to be found.

#### Returns

[Quad](./missingvalues/quad.md)<br>
The positive square root of `x`.

### **Tan(Quad)**

Returns the tangent of the specified angle.

```csharp
public static Quad Tan(Quad x)
```

#### Parameters

`x` [Quad](./missingvalues/quad.md)<br>
An angle, measured in radians.

#### Returns

[Quad](./missingvalues/quad.md)<br>
The tangent of `x`. If `x` is equal to [Quad.NaN](./missingvalues/quad.md#nan), [Quad.NegativeInfinity](./missingvalues/quad.md#negativeinfinity), or [Quad.PositiveInfinity](./missingvalues/quad.md#positiveinfinity), this method returns [Quad.NaN](./missingvalues/quad.md#nan).

### **Tanh(Quad)**

Returns the hyperbolic tangent of the specified angle.

```csharp
public static Quad Tanh(Quad x)
```

#### Parameters

`x` [Quad](./missingvalues/quad.md)<br>
An angle, measured in radians.

#### Returns

[Quad](./missingvalues/quad.md)<br>
The hyperbolic tangent of `x`. If `x` is equal to [Quad.NegativeInfinity](./missingvalues/quad.md#negativeinfinity), this method returns -1. If value is equal to [Quad.PositiveInfinity](./missingvalues/quad.md#positiveinfinity), this method returns 1. If `x` is equal to [Quad.NaN](./missingvalues/quad.md#nan), this method returns [Quad.NaN](./missingvalues/quad.md#nan).

### **Truncate(Quad)**

Calculates the integral part of a specified quadruple-precision floating-point number.

```csharp
public static Quad Truncate(Quad x)
```

#### Parameters

`x` [Quad](./missingvalues/quad.md)<br>
A number to truncate.

#### Returns

[Quad](./missingvalues/quad.md)<br>
The integral part of `x`; that is, the number that remains after any fractional digits have been discarded, or one of the values listed in the following table.

---

[`< Back`](../)
