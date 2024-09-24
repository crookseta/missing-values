using System.Numerics;

namespace MissingValues.Internals;

internal interface IBigNumber<TSelf> : INumber<TSelf>
	where TSelf : IBigNumber<TSelf>?
{
	/// <summary>Computes the unary plus of a value.</summary>
	/// <param name="value">The value for which to compute its unary plus.</param>
	/// <returns>The unary plus of <paramref name="value" />.</returns>
	static abstract TSelf operator +(in TSelf value);
	/// <summary>Adds two values together to compute their sum.</summary>
	/// <param name="left">The value to which <paramref name="right" /> is added.</param>
	/// <param name="right">The value which is added to <paramref name="left" />.</param>
	/// <returns>The sum of <paramref name="left" /> and <paramref name="right" />.</returns>
	static abstract TSelf operator +(in TSelf left, in TSelf right);
	/// <summary>Adds two values together to compute their sum.</summary>
	/// <param name="left">The value to which <paramref name="right" /> is added.</param>
	/// <param name="right">The value which is added to <paramref name="left" />.</param>
	/// <returns>The sum of <paramref name="left" /> and <paramref name="right" />.</returns>
	/// <exception cref="OverflowException">The sum of <paramref name="left" /> and <paramref name="right" /> is not representable by <typeparamref name="TSelf" />.</exception>
	static virtual TSelf operator checked +(in TSelf left, in TSelf right) => left + right;
	/// <summary>
	/// Increments a value.
	/// </summary>
	/// <param name="value">The value to increment.</param>
	/// <returns>The result of incrementing <paramref name="value"/>.</returns>
	static abstract TSelf operator ++(in TSelf value);
	/// <summary>
	/// Increments a value.
	/// </summary>
	/// <param name="value">The value to increment.</param>
	/// <exception cref="OverflowException">The result of incrementing <paramref name="value" /> is not representable by <typeparamref name="TSelf" />.</exception>
	/// <returns>The result of incrementing <paramref name="value"/>.</returns>
	static virtual TSelf operator checked ++(in TSelf value) => value - TSelf.One;
	/// <summary>Computes the unary negation of a value.</summary>
	/// <param name="value">The value for which to compute its unary negation.</param>
	/// <returns>The unary negation of <paramref name="value" />.</returns>
	static abstract TSelf operator -(in TSelf value);
	/// <summary>Computes the unary negation of a value.</summary>
	/// <param name="value">The value for which to compute its unary negation.</param>
	/// <returns>The unary negation of <paramref name="value" />.</returns>
	/// <exception cref="OverflowException">The unary negation of <paramref name="value" /> is not representable by <typeparamref name="TSelf" />.</exception>
	static virtual TSelf operator checked -(in TSelf value) => -value;
	/// <summary>Subtracts two values to compute their difference.</summary>
	/// <param name="left">The value from which <paramref name="right" /> is subtracted.</param>
	/// <param name="right">The value which is subtracted from <paramref name="left" />.</param>
	/// <returns>The difference of <paramref name="right" /> subtracted from <paramref name="left" />.</returns>
	static abstract TSelf operator -(in TSelf left, in TSelf right);
	/// <summary>
	/// Decrements a value.
	/// </summary>
	/// <param name="value">The value to decrement.</param>
	/// <returns>The result of decrementing <paramref name="value"/>.</returns>
	static abstract TSelf operator --(in TSelf value);
	/// <summary>
	/// Decrements a value.
	/// </summary>
	/// <param name="value">The value to decrement.</param>
	/// <exception cref="OverflowException">The result of decrementing <paramref name="value" /> is not representable by <typeparamref name="TSelf" />.</exception>
	/// <returns>The result of decrementing <paramref name="value"/>.</returns>
	static virtual TSelf operator checked --(in TSelf value) => value - TSelf.One;
	/// <summary>Subtracts two values to compute their difference.</summary>
	/// <param name="left">The value from which <paramref name="right" /> is subtracted.</param>
	/// <param name="right">The value which is subtracted from <paramref name="left" />.</param>
	/// <returns>The difference of <paramref name="right" /> subtracted from <paramref name="left" />.</returns>
	/// <exception cref="OverflowException">The difference of <paramref name="right" /> subtracted from <paramref name="left" /> is not representable by <typeparamref name="TSelf" />.</exception>
	static virtual TSelf operator checked -(in TSelf left, in TSelf right) => left - right;
	/// <summary>Multiplies two values together to compute their product.</summary>
	/// <param name="left">The value which <paramref name="right" /> multiplies.</param>
	/// <param name="right">The value which multiplies <paramref name="left" />.</param>
	/// <returns>The product of <paramref name="left" /> multiplied-by <paramref name="right" />.</returns>
	static abstract TSelf operator *(in TSelf left, in TSelf right);
	/// <summary>Multiplies two values together to compute their product.</summary>
	/// <param name="left">The value which <paramref name="right" /> multiplies.</param>
	/// <param name="right">The value which multiplies <paramref name="left" />.</param>
	/// <returns>The product of <paramref name="left" /> multiplied-by <paramref name="right" />.</returns>
	/// <exception cref="OverflowException">The product of <paramref name="left" /> multiplied-by <paramref name="right" /> is not representable by <typeparamref name="TSelf" />.</exception>
	static virtual TSelf operator checked *(in TSelf left, in TSelf right) => left * right;
	/// <summary>Divides two values together to compute their quotient.</summary>
	/// <param name="left">The value which <paramref name="right" /> divides.</param>
	/// <param name="right">The value which divides <paramref name="left" />.</param>
	/// <returns>The quotient of <paramref name="left" /> divided-by <paramref name="right" />.</returns>
	static abstract TSelf operator /(in TSelf left, in TSelf right);
	/// <summary>Divides two values together to compute their quotient.</summary>
	/// <param name="left">The value which <paramref name="right" /> divides.</param>
	/// <param name="right">The value which divides <paramref name="left" />.</param>
	/// <returns>The quotient of <paramref name="left" /> divided-by <paramref name="right" />.</returns>
	/// <exception cref="OverflowException">The quotient of <paramref name="left" /> divided-by <paramref name="right" /> is not representable by <typeparamref name="TSelf" />.</exception>
	static virtual TSelf operator checked /(in TSelf left, in TSelf right) => left / right;
	/// <summary>Divides two values together to compute their modulus or remainder.</summary>
	/// <param name="left">The value which <paramref name="right" /> divides.</param>
	/// <param name="right">The value which divides <paramref name="left" />.</param>
	/// <returns>The modulus or remainder of <paramref name="left" /> divided-by <paramref name="right" />.</returns>
	static abstract TSelf operator %(in TSelf left, in TSelf right);
	/// <summary>Compares two values to determine equality.</summary>
	/// <param name="left">The value to compare with <paramref name="right" />.</param>
	/// <param name="right">The value to compare with <paramref name="left" />.</param>
	/// <returns><c>true</c> if <paramref name="left" /> is equal to <paramref name="right" />; otherwise, <c>false</c>.</returns>
	static abstract bool operator ==(in TSelf left, in TSelf right);
	/// <summary>Compares two values to determine inequality.</summary>
	/// <param name="left">The value to compare with <paramref name="right" />.</param>
	/// <param name="right">The value to compare with <paramref name="left" />.</param>
	/// <returns><c>true</c> if <paramref name="left" /> is not equal to <paramref name="right" />; otherwise, <c>false</c>.</returns>
	static abstract bool operator !=(in TSelf left, in TSelf right);
	/// <summary>Compares two values to determine which is less.</summary>
	/// <param name="left">The value to compare with <paramref name="right" />.</param>
	/// <param name="right">The value to compare with <paramref name="left" />.</param>
	/// <returns><c>true</c> if <paramref name="left" /> is less than <paramref name="right" />; otherwise, <c>false</c>.</returns>
	static abstract bool operator <(in TSelf left, in TSelf right);
	/// <summary>Compares two values to determine which is less or equal.</summary>
	/// <param name="left">The value to compare with <paramref name="right" />.</param>
	/// <param name="right">The value to compare with <paramref name="left" />.</param>
	/// <returns><c>true</c> if <paramref name="left" /> is less than or equal to <paramref name="right" />; otherwise, <c>false</c>.</returns>
	static abstract bool operator <=(in TSelf left, in TSelf right);
	/// <summary>Compares two values to determine which is greater.</summary>
	/// <param name="left">The value to compare with <paramref name="right" />.</param>
	/// <param name="right">The value to compare with <paramref name="left" />.</param>
	/// <returns><c>true</c> if <paramref name="left" /> is greater than <paramref name="right" />; otherwise, <c>false</c>.</returns>
	static abstract bool operator >(in TSelf left, in TSelf right);
	/// <summary>Compares two values to determine which is greater or equal.</summary>
	/// <param name="left">The value to compare with <paramref name="right" />.</param>
	/// <param name="right">The value to compare with <paramref name="left" />.</param>
	/// <returns><c>true</c> if <paramref name="left" /> is greater than or equal to <paramref name="right" />; otherwise, <c>false</c>.</returns>
	static abstract bool operator >=(in TSelf left, in TSelf right);


	static TSelf IUnaryPlusOperators<TSelf, TSelf>.operator +(TSelf value) => +value;
	static TSelf IUnaryNegationOperators<TSelf, TSelf>.operator -(TSelf value) => -value;
	static TSelf IUnaryNegationOperators<TSelf, TSelf>.operator checked -(TSelf value) => checked(-value);
	static TSelf IAdditionOperators<TSelf, TSelf, TSelf>.operator +(TSelf left, TSelf right) => left + right;
	static TSelf IAdditionOperators<TSelf, TSelf, TSelf>.operator checked +(TSelf left, TSelf right) => checked(left + right);
	static TSelf IIncrementOperators<TSelf>.operator ++(TSelf value) => ++value;
	static TSelf IIncrementOperators<TSelf>.operator checked ++(TSelf value) => checked(++value);
	static TSelf ISubtractionOperators<TSelf, TSelf, TSelf>.operator -(TSelf left, TSelf right) => left - right;
	static TSelf ISubtractionOperators<TSelf, TSelf, TSelf>.operator checked -(TSelf left, TSelf right) => checked(left - right);
	static TSelf IDecrementOperators<TSelf>.operator --(TSelf value) => --value;
	static TSelf IDecrementOperators<TSelf>.operator checked --(TSelf value) => checked(--value);
	static TSelf IMultiplyOperators<TSelf, TSelf, TSelf>.operator *(TSelf left, TSelf right) => left * right;
	static TSelf IMultiplyOperators<TSelf, TSelf, TSelf>.operator checked *(TSelf left, TSelf right) => checked(left * right);
	static TSelf IDivisionOperators<TSelf, TSelf, TSelf>.operator /(TSelf left, TSelf right) => left / right;
	static TSelf IDivisionOperators<TSelf, TSelf, TSelf>.operator checked /(TSelf left, TSelf right) => checked(left / right);
	static TSelf IModulusOperators<TSelf, TSelf, TSelf>.operator %(TSelf left, TSelf right) => left % right;
	static bool IEqualityOperators<TSelf, TSelf, bool>.operator ==(TSelf? left, TSelf? right) => left == right;
	static bool IEqualityOperators<TSelf, TSelf, bool>.operator !=(TSelf? left, TSelf? right) => left != right;
	static bool IComparisonOperators<TSelf, TSelf, bool>.operator <(TSelf left, TSelf right) => left < right;
	static bool IComparisonOperators<TSelf, TSelf, bool>.operator <=(TSelf left, TSelf right) => left <= right;
	static bool IComparisonOperators<TSelf, TSelf, bool>.operator >(TSelf left, TSelf right) => left > right;
	static bool IComparisonOperators<TSelf, TSelf, bool>.operator >=(TSelf left, TSelf right) => left >= right;
}
internal interface IBigBinaryNumber<TSelf> : IBigNumber<TSelf>, IBinaryNumber<TSelf>
	where TSelf : IBigBinaryNumber<TSelf>?
{
	/// <summary>Computes the bitwise-and of two values.</summary>
	/// <param name="left">The value to and with <paramref name="right" />.</param>
	/// <param name="right">The value to and with <paramref name="left" />.</param>
	/// <returns>The bitwise-and of <paramref name="left" /> and <paramref name="right" />.</returns>
	static abstract TSelf operator &(in TSelf left, in TSelf right);
	/// <summary>Computes the bitwise-or of two values.</summary>
	/// <param name="left">The value to or with <paramref name="right" />.</param>
	/// <param name="right">The value to or with <paramref name="left" />.</param>
	/// <returns>The bitwise-or of <paramref name="left" /> and <paramref name="right" />.</returns>
	static abstract TSelf operator |(in TSelf left, in TSelf right);
	/// <summary>Computes the exclusive-or of two values.</summary>
	/// <param name="left">The value to xor with <paramref name="right" />.</param>
	/// <param name="right">The value to xorwith <paramref name="left" />.</param>
	/// <returns>The exclusive-or of <paramref name="left" /> and <paramref name="right" />.</returns>
	static abstract TSelf operator ^(in TSelf left, in TSelf right);
	/// <summary>Computes the ones-complement representation of a given value.</summary>
	/// <param name="value">The value for which to compute its ones-complement.</param>
	/// <returns>The ones-complement of <paramref name="value" />.</returns>
	static abstract TSelf operator ~(in TSelf value);

	static TSelf IBitwiseOperators<TSelf, TSelf, TSelf>.operator &(TSelf left, TSelf right) => left & right;
	static TSelf IBitwiseOperators<TSelf, TSelf, TSelf>.operator |(TSelf left, TSelf right) => left | right;
	static TSelf IBitwiseOperators<TSelf, TSelf, TSelf>.operator ^(TSelf left, TSelf right) => left ^ right;
	static TSelf IBitwiseOperators<TSelf, TSelf, TSelf>.operator ~(TSelf value) => ~value;
}
internal interface IBigInteger<TSelf> : IBigBinaryNumber<TSelf>, IBinaryInteger<TSelf>
	where TSelf : IBigInteger<TSelf>?
{
	/// <summary>Shifts a value left by a given amount.</summary>
	/// <param name="value">The value which is shifted left by <paramref name="shiftAmount" />.</param>
	/// <param name="shiftAmount">The amount by which <paramref name="value" /> is shifted left.</param>
	/// <returns>The result of shifting <paramref name="value" /> left by <paramref name="shiftAmount" />.</returns>
	static abstract TSelf operator <<(in TSelf value, int shiftAmount);
	/// <summary>Shifts a value right by a given amount.</summary>
	/// <param name="value">The value which is shifted right by <paramref name="shiftAmount" />.</param>
	/// <param name="shiftAmount">The amount by which <paramref name="value" /> is shifted right.</param>
	/// <returns>The result of shifting <paramref name="value" /> right by <paramref name="shiftAmount" />.</returns>
	/// <remarks>This operation is meant to perform a signed (otherwise known as an arithmetic) right shift on signed types.</remarks>
	static abstract TSelf operator >>(in TSelf value, int shiftAmount);
	/// <summary>Shifts a value right by a given amount.</summary>
	/// <param name="value">The value which is shifted right by <paramref name="shiftAmount" />.</param>
	/// <param name="shiftAmount">The amount by which <paramref name="value" /> is shifted right.</param>
	/// <returns>The result of shifting <paramref name="value" /> right by <paramref name="shiftAmount" />.</returns>
	/// <remarks>This operation is meant to perform n unsigned (otherwise known as a logical) right shift on all types.</remarks>
	static abstract TSelf operator >>>(in TSelf value, int shiftAmount);

	static TSelf IShiftOperators<TSelf, int, TSelf>.operator <<(TSelf value, int shiftAmount) => value << shiftAmount;
	static TSelf IShiftOperators<TSelf, int, TSelf>.operator >>(TSelf value, int shiftAmount) => value >> shiftAmount;
	static TSelf IShiftOperators<TSelf, int, TSelf>.operator >>>(TSelf value, int shiftAmount) => value >>> shiftAmount;
}
