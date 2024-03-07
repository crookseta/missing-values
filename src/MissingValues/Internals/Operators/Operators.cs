using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues.Internals.Operators
{
	internal interface IAcosOperator<T>
		where T : struct, ITrigonometricFunctions<T>
	{
		abstract static T PiOver2 { get; }
	}
	internal interface IAcoshOperator<T>
		where T : struct, ITrigonometricFunctions<T>
	{
		abstract static T LN2 { get; }
	}
	internal interface IAsinOperator<T>
		where T : struct, ITrigonometricFunctions<T>
	{
		abstract static T Big { get; }
		abstract static T Small { get; }
		abstract static T PiOver2Lo { get; }
		abstract static T PiOver2Hi { get; }

		abstract static bool CloseToOne(T value);
		abstract static T ClearBottom(T value);
		abstract static T RationalApproximation(T value);
	}
	internal interface IAsinhOperator<T>
		where T : struct, ITrigonometricFunctions<T>
	{
		abstract static T LN2 { get; }
	}
	internal interface IAtanOperator<T>
		where T : struct, IFloatingPointIeee754<T>
	{
		abstract static ReadOnlySpan<T> AtanHi { get; }
		abstract static ReadOnlySpan<T> AtanLo { get; }
		abstract static ReadOnlySpan<T> AT { get; }

		abstract static T ExponentMantissa(T value);
		abstract static T Even(T x);
		abstract static T Odd(T x);
	}
	internal interface IAtan2Operator<T>
		where T : struct, IFloatingPointIeee754<T>
	{
		abstract static T PiOver2Lo { get; }
		abstract static T PiOver2Hi { get; }

		abstract static T RationalApproximation(T value);
	}
	internal interface ICbrtOperator<T>
		where T : struct, IRootFunctions<T>
	{
		abstract static T Big { get; }
		abstract static T Small { get; }
		abstract static T RoundAwayFromZeroOrNBits(T value);
		abstract static T FromSingle(float value);
		abstract static float ToSingle(T value);
		abstract static T FromDouble(double value);
		abstract static double ToDouble(T value);
		abstract static T FromBits(UInt128 bits);
		abstract static T Ctor(bool sign, int exponent, UInt128 significand);
	}
	internal interface IRoundOperator<T>
		where T : struct, IFloatingPoint<T>
	{
		abstract static T ToInt { get; }
	}
	internal interface IExp10Operator<T>
		where T : struct, IExponentialFunctions<T>
	{
		abstract static ReadOnlySpan<T> P10 { get; }
		abstract static T ModF(T x, ref T n);
	}
	internal interface IExp2Operator<T>
		where T : struct, IExponentialFunctions<T>
	{
		abstract static int TableSize { get; }
		abstract static ReadOnlySpan<T> Table { get; }
		abstract static T Redux { get; }
		abstract static ulong GetLowBits(T x);
		/// <summary>
		/// Compute r = exp2l(y) = exp2lt[i0] * p(z).
		/// </summary>
		/// <param name="hi"></param>
		/// <param name="lo"></param>
		/// <param name="z"></param>
		/// <returns></returns>
		abstract static T Compute(T hi, T lo, T z);
	}
	internal interface IHypotOperator<T>
		where T : struct, IRootFunctions<T>
	{
		abstract static T Split { get; }

		abstract static void Sq(ref T hi, ref T lo, T x);
	}
	internal interface ICosOperator<T>
		where T : struct, ITrigonometricFunctions<T>
	{
		abstract static T PiOver4 { get; }
		abstract static int RemPiOver2(T x, Span<T> y);
		abstract static T RationalApproximation(T value);
		abstract static T CosCore(T x, T y);
	}
	internal interface ISinOperator<T>
		where T : struct, ITrigonometricFunctions<T>
	{
		abstract static T RemPiOver2(T x, T y);
		abstract static T RationalApproximation(T value);
		abstract static T SinCore(T x, T y, int iy);
	}
	internal interface ITanOperator<T>
		where T : struct, ITrigonometricFunctions<T>
	{
		abstract static T RemPiOver2(T x, T y);
		abstract static void TanCore(T x, T y, int iy);
	}
}
