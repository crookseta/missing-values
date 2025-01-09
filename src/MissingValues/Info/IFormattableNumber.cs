using System.Globalization;
using System.Numerics;

namespace MissingValues.Info
{
	internal interface IFormattableNumber<TSelf> : INumber<TSelf>
		where TSelf : IFormattableNumber<TSelf>?
	{
		/// <summary>
		/// Converts the specified numeric Unicode character to a <typeparamref name="TSelf"/>.
		/// </summary>
		/// <param name="value">A numeric Unicode character.</param>
		/// <returns>The numeric value of <paramref name="value"/> if it represents a number; otherwise, 0</returns>
		static virtual TSelf GetDecimalValue(char value) => TSelf.CreateChecked(CharUnicodeInfo.GetDecimalDigitValue(value));

		abstract static bool IsBinaryInteger();
	}
}
