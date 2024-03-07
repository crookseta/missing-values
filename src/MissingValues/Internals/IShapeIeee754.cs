namespace MissingValues.Internals
{
	internal interface IShapeIeee754
	{
		uint SignExponent { get; }
		UInt128 Mantissa { get; }
		/// <summary>
		/// Extracts the exponent and the first few bits of the mantissa.
		/// </summary>
		uint ExpMantissa { get; }
	}
}
