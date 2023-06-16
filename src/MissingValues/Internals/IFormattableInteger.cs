using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues
{
	internal interface IFormattableInteger<TSelf> : IBinaryInteger<TSelf>
		where TSelf : IFormattableInteger<TSelf>?
	{
		/// <summary>
		/// Converts a <typeparamref name="TSelf"/> to a <see cref="char"/>.
		/// </summary>
		/// <returns></returns>
		char ToChar();
		/// <summary>
		/// Converts a <typeparamref name="TSelf"/> to a <see cref="int"/>.
		/// </summary>
		/// <returns></returns>
		int ToInt32();

		/// <summary>
		/// Converts the specified numeric Unicode character to a <typeparamref name="TSelf"/>.
		/// </summary>
		/// <param name="value">A numeric Unicode character.</param>
		/// <returns>The numeric value of <paramref name="value"/> if it represents a number; otherwise, 0</returns>
		abstract static TSelf GetDecimalValue(char value);
		/// <summary>
		/// Converts the specified hexadecimal character to a <typeparamref name="TSelf"/>.
		/// </summary>
		/// <param name="value">A hexadecimal character.</param>
		/// <returns>The hexadecimal value of <paramref name="value"/> if it represents a number; otherwise, 0</returns>
		abstract static TSelf GetHexValue(char value);

		/// <summary>
		/// Gets the value <c>2</c> of the type.
		/// </summary>
		abstract static TSelf Two { get; }
		/// <summary>
		/// Gets the value <c>16</c> of the type.
		/// </summary>
		abstract static TSelf Sixteen { get; }
		/// <summary>
		/// Gets the value <c>10</c> of the type.
		/// </summary>
		abstract static TSelf Ten { get; }

		/// <summary>
		/// Gets the value <c>4</c> of the type.
		/// </summary>
		abstract static TSelf TwoPow2 { get; }
		/// <summary>
		/// Gets the value <c>256</c> of the type.
		/// </summary>
		abstract static TSelf SixteenPow2 { get; }
		/// <summary>
		/// Gets the value <c>100</c> of the type.
		/// </summary>
		abstract static TSelf TenPow2 { get; }
		
		/// <summary>
		/// Gets the value <c>8</c> of the type.
		/// </summary>
		abstract static TSelf TwoPow3 { get; }
		/// <summary>
		/// Gets the value <c>4096</c> of the type.
		/// </summary>
		abstract static TSelf SixteenPow3 { get; }
		/// <summary>
		/// Gets the value <c>1000</c> of the type.
		/// </summary>
		abstract static TSelf TenPow3 { get; }

		/// <summary>
		/// Gets the left-most digit of the maximum value of <typeparamref name="TSelf"/>.
		/// </summary>
		abstract static char LastDecimalDigitOfMaxValue { get; }
		/// <summary>
		/// Gets the number of digits of the maximum decimal value of <typeparamref name="TSelf"/>.
		/// </summary>
		abstract static int MaxDecimalDigits { get; }
		/// <summary>
		/// Gets the number of digits of the maximum hexadecimal value of <typeparamref name="TSelf"/>.
		/// </summary>
		abstract static int MaxHexDigits { get; }
		/// <summary>
		/// Gets the number of digits of the maximum binary value of <typeparamref name="TSelf"/>.
		/// </summary>
		abstract static int MaxBinaryDigits { get; }
	}

	internal interface IFormattableSignedInteger<TSigned, TUnsigned> : IFormattableInteger<TSigned>, ISignedNumber<TSigned>
		where TSigned : IFormattableSignedInteger<TSigned, TUnsigned>
		where TUnsigned : IUnsignedNumber<TUnsigned>, IFormattableInteger<TUnsigned>
	{
		/// <summary>
		/// Returns the unsigned representation of the signed integer.
		/// </summary>
		/// <returns>The unsigned representation of the signed integer.</returns>
		TUnsigned ToUnsigned();
	}

	internal interface IFormattableUnsignedInteger<TUnsigned, TSigned> : IFormattableInteger<TUnsigned>, IUnsignedNumber<TUnsigned>
		where TUnsigned : IFormattableUnsignedInteger<TUnsigned, TSigned>
		where TSigned : IFormattableInteger<TSigned>
	{
		/// <summary>
		/// Gets the absolute representation of the maximum representable value of <typeparamref name="TSigned"/>.
		/// </summary>
		abstract static TUnsigned SignedMaxMagnitude { get; }

		/// <summary>
		/// Returns the signed representation of the unsigned integer.
		/// </summary>
		/// <returns>The signed representation of the unsigned integer.</returns>
		TSigned ToSigned();
	}
}
