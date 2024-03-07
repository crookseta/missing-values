using System.Numerics;

namespace MissingValues.Internals
{
	internal interface INumberConstants<TSelf>
		: IFloatingPoint<TSelf>
		where TSelf : INumberConstants<TSelf>
	{
		/// <summary>
		/// Represents the number <c>1.5</c>
		/// </summary>
		static abstract TSelf OneHalf {  get; }
		/// <summary>
		/// Represents the number <c>0.5</c>
		/// </summary>
		static abstract TSelf Half {  get; }
		/// <summary>
		/// Represents the number <c>2</c>
		/// </summary>
		static abstract TSelf Two { get; }
		/// <summary>
		/// Represents the number <c>3</c>
		/// </summary>
		static abstract TSelf Three { get; }
		/// <summary>
		/// Represents the number <c>4</c>
		/// </summary>
		static abstract TSelf Four { get; }
		/// <summary>
		/// 
		/// </summary>
		static abstract TSelf HugeValue { get; }
		static abstract TSelf TinyValue { get; }
	}
}
