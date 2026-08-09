using System.Numerics;
using MissingValues.Primitives;

namespace MissingValues;

public partial struct Quad
{
	/// <inheritdoc/>
	public static Quad operator +(Quad value) => value;

	/// <inheritdoc/>
	public static Quad operator +(Quad left, Quad right) => MathQ.Add(left, right);

	/// <inheritdoc/>
	public static Quad operator -(Quad value)
	{
		// Invert the sign bit
		return BinaryOperations.UInt128BitsToQuad(BinaryOperations.QuadToUInt128Bits(value) ^ new UInt128(0x8000_0000_0000_0000, 0x0000_0000_0000_0000));
	}

	/// <inheritdoc/>
	public static Quad operator -(Quad left, Quad right) => MathQ.Sub(left, right);

	/// <inheritdoc/>
	static Quad IBitwiseOperators<Quad, Quad, Quad>.operator ~(Quad value) => new Quad(~value._upper, ~value._lower);

	/// <inheritdoc/>
	public static Quad operator ++(Quad value) => MathQ.Add(value, One);

	/// <inheritdoc/>
	public static Quad operator --(Quad value) => MathQ.Sub(value, One);

	/// <inheritdoc/>
	public static Quad operator *(Quad left, Quad right) => MathQ.Mul(left, right);

	/// <inheritdoc/>
	public static Quad operator /(Quad left, Quad right) => MathQ.Div(left, right);

	/// <inheritdoc/>
	public static Quad operator %(Quad left, Quad right)
	{
		return (MathQ.Abs(left) - (MathQ.Abs(right) * (MathQ.Floor(MathQ.Abs(left) / MathQ.Abs(right))))) * MathQ.Sign(left);
	}

	/// <inheritdoc/>
	static Quad IBitwiseOperators<Quad, Quad, Quad>.operator &(Quad left, Quad right) => new Quad(left._upper & right._upper, left._lower & right._lower);

	/// <inheritdoc/>
	static Quad IBitwiseOperators<Quad, Quad, Quad>.operator |(Quad left, Quad right) => new Quad(left._upper | right._upper, left._lower | right._lower);

	/// <inheritdoc/>
	static Quad IBitwiseOperators<Quad, Quad, Quad>.operator ^(Quad left, Quad right) => new Quad(left._upper ^ right._upper, left._lower ^ right._lower);

	/// <inheritdoc/>
	public static bool operator ==(Quad left, Quad right)
	{
		if (IsNaN(left) || IsNaN(right))
		{
			// IEEE defines that NaN is not equal to anything, including itself.
			return false;
		}

		// IEEE defines that positive and negative zero are equivalent.
		return (left._upper == right._upper && left._lower == right._lower) || AreZero(left, right);
	}

	/// <inheritdoc/>
	public static bool operator !=(Quad left, Quad right) => !(left == right);

	/// <inheritdoc/>
	public static bool operator <(Quad left, Quad right)
	{
		if (IsNaN(left) || IsNaN(right))
		{
			// IEEE defines that NaN is unordered with respect to everything, including itself.
			return false;
		}

		bool leftIsNegative = IsNegative(left);

		if (leftIsNegative != IsNegative(right))
		{
			// When the signs of right and left differ, we know that right is less than left if it is
			// the negative value. The exception to this is if both values are zero, in which case IEEE
			// says they should be equal, even if the signs differ.
			return leftIsNegative && !AreZero(left, right);
		}

		UInt128 leftBits = BinaryOperations.QuadToUInt128Bits(left);
		UInt128 rightBits = BinaryOperations.QuadToUInt128Bits(right);

		return (leftBits != rightBits) && ((leftBits < rightBits) ^ leftIsNegative);
	}

	/// <inheritdoc/>
	public static bool operator >(Quad left, Quad right)
	{
		if (IsNaN(right) || IsNaN(left))
		{
			// IEEE defines that NaN is unordered with respect to everything, including itself.
			return false;
		}

		bool rightIsNegative = IsNegative(right);

		if (rightIsNegative != IsNegative(left))
		{
			return rightIsNegative && !AreZero(right, left);
		}

		UInt128 leftBits = BinaryOperations.QuadToUInt128Bits(left);
		UInt128 rightBits = BinaryOperations.QuadToUInt128Bits(right);

		return (rightBits != leftBits) && ((rightBits < leftBits) ^ rightIsNegative);
	}

	/// <inheritdoc/>
	public static bool operator <=(Quad left, Quad right)
	{
		if (IsNaN(left) || IsNaN(right))
		{
			// IEEE defines that NaN is unordered with respect to everything, including itself.
			return false;
		}

		bool leftIsNegative = IsNegative(left);

		if (leftIsNegative != IsNegative(right))
		{
			// When the signs of right and left differ, we know that right is less than left if it is
			// the negative value. The exception to this is if both values are zero, in which case IEEE
			// says they should be equal, even if the signs differ.
			return leftIsNegative || AreZero(left, right);
		}

		UInt128 leftBits = BinaryOperations.QuadToUInt128Bits(left);
		UInt128 rightBits = BinaryOperations.QuadToUInt128Bits(right);

		return (leftBits == rightBits) || ((leftBits < rightBits) ^ leftIsNegative);
	}

	/// <inheritdoc/>
	public static bool operator >=(Quad left, Quad right)
	{
		if (IsNaN(right) || IsNaN(left))
		{
			// IEEE defines that NaN is unordered with respect to everything, including itself.
			return false;
		}

		bool rightIsNegative = IsNegative(right);

		if (rightIsNegative != IsNegative(left))
		{
			return rightIsNegative || AreZero(right, left);
		}

		UInt128 leftBits = BinaryOperations.QuadToUInt128Bits(left);
		UInt128 rightBits = BinaryOperations.QuadToUInt128Bits(right);

		return (rightBits == leftBits) || ((rightBits < leftBits) ^ rightIsNegative);
	}
}