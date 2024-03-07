using MissingValues.Internals.Operators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace MissingValues.Internals
{
	internal static class GenericMath
	{
		public static TFloat Acos<TOperation, TFloat>(TFloat x)
			where TOperation : IAcosOperator<TFloat>
			where TFloat : struct, IBinaryFloatingPointIeee754<TFloat>
		{
			TFloat y;

			if (TFloat.IsZero(x))
			{
				return TOperation.PiOver2;
			}
			if (x >= TFloat.One)
			{
				if (x == TFloat.One)
				{
					return TFloat.Zero;
				}
				return TFloat.NaN;
			}
			if (x <= TFloat.NegativeOne)
			{
				if (x == TFloat.NegativeOne)
				{
					return TFloat.Pi;
				}
				return TFloat.NaN;
			}

			y = TFloat.Atan(TFloat.Sqrt(TFloat.One - (x * x)) / x);

			if (x > TFloat.Zero)
			{
				return y;
			}

			return y + TFloat.Pi;
		}
		public static TFloat Acosh<TOperation, TFloat>(TFloat x)
			where TOperation : IAcoshOperator<TFloat>
			where TFloat : struct, IBinaryFloatingPointIeee754<TFloat>, INumberConstants<TFloat>, IShapeIeee754
		{
			var se = x.SignExponent;
			if (se < 0x3FFF || (se & 0x8000) != 0)
			{
				return TFloat.NaN;
			}
			else if (se >= 0x401D)
			{
				if (se >= 0x7FFF)
				{
					return x;
				}
				else 
				{ 
					return TFloat.Log(x) + TOperation.LN2; 
				}
			}
			else if (x == TFloat.One)
			{
				return TFloat.Zero;
			}
			else if (se > 0x4000)
			{
				var t = x * x;
				return TFloat.Log(TFloat.Two * x - TFloat.One / (x + TFloat.Sqrt(t - TFloat.One)));
			}
			else
			{
				var t = x - TFloat.One;
				return TFloat.LogP1(t + TFloat.Sqrt(TFloat.Two * t + t * t));
			}
		}
		public static TFloat Asin<TOperation, TFloat>(TFloat x)
			where TOperation : IAsinOperator<TFloat>
			where TFloat : struct, IBinaryFloatingPointIeee754<TFloat>, INumberConstants<TFloat>, IShapeIeee754
		{
			var e = x.SignExponent & 0x7FFF;
			var sign = TFloat.IsNegative(x);

            if (e >= 0x3FFF) /* |x| >= 1 or nan */
			{
				/* asin(+-1)=+-pi/2 with inexact */
				if (x == TFloat.One || x == TFloat.NegativeOne)
				{
					return x * TOperation.PiOver2Hi + TOperation.Small;
				}
				return TFloat.NaN;
            }
			if (e < 0x3FFF - 1)
			{ /* |x| < 0.5 */
				if (e < 0x3FFF - (x.GetSignificandBitLength() + 1) / 2)
				{
					return x;
				}
				return x + x * TOperation.RationalApproximation(x * x);
			}
			/* 1 > |x| >= 0.5 */
			var z = (TFloat.One - TFloat.Abs(x)) * TFloat.Half;
			var s = TFloat.Sqrt(z);
			var r = TOperation.RationalApproximation(z);
			if (TOperation.CloseToOne(x))
			{
				x = TOperation.PiOver2Hi - (TFloat.Two * (s + s * r) - TOperation.PiOver2Lo);
			}
			else
			{
				var f = TOperation.ClearBottom(x);
				var c = (z - f * f) / (s + f);
				x = TFloat.Half * TOperation.PiOver2Hi - (TFloat.Two * s * r - (TOperation.PiOver2Lo - TFloat.Two * c) - (TFloat.Half * TOperation.PiOver2Hi - TFloat.Two * f));
			}
			return sign ? -x : x;
		}
		public static TFloat Asinh<TOperation, TFloat>(TFloat x)
			where TOperation : IAsinhOperator<TFloat>
			where TFloat : struct, IBinaryFloatingPointIeee754<TFloat>, INumberConstants<TFloat>, IShapeIeee754
		{
			const uint IEEE854_LONG_DOUBLE_MAXEXP = 0x7FFF;
			var hx = x.SignExponent;
			var ix = hx & IEEE854_LONG_DOUBLE_MAXEXP;
			TFloat w;

			if (ix < 0x3FDE)
			{
				if (TFloat.HugeValue + x > TFloat.One)
				{
					return x;
				}
			}
			if (ix > 0x4020)
			{
				if (ix == IEEE854_LONG_DOUBLE_MAXEXP)
				{
					return x;
				}
				w = TFloat.Log(TFloat.Abs(x)) + TOperation.LN2;
			}
			else
			{
				var xa = TFloat.Abs(x);

				if (ix > 0x4000)
				{
					w = TFloat.LogP1(TFloat.Two * xa + TFloat.One / (TFloat.Sqrt(xa * xa + TFloat.One) + xa));
				}
				else
				{
					var t = xa * xa;
					w = TFloat.LogP1(xa + t / (TFloat.One + TFloat.Sqrt(TFloat.One + t)));
				}
			}
			if (hx > 0)
				return w;
			return -w;
		}
		public static TFloat Atan<TOperation, TFloat>(TFloat x)
			where TOperation : IAtanOperator<TFloat>
			where TFloat : struct, IBinaryFloatingPointIeee754<TFloat>, INumberConstants<TFloat>, IShapeIeee754
		{
			TFloat w, s1, s2, z;
			int id;
			var exponent = x.SignExponent & 0x7FFF;
			var sign = TFloat.IsNegative(x);

			if (exponent >= 0x3FFF + (x.GetSignificandBitLength() + 1))
			{
				// if |x| is large, atan(x)~=pi/2
				if (TFloat.IsNaN(x))
				{
					return x;
				}
				return sign ? -TOperation.AtanHi[3] : TOperation.AtanHi[3];
			}

			// Extract the exponent and the first few bits of the mantissa.
			uint expman = x.ExpMantissa;
			if (expman < ((0x3FFF - 2) << 8) + 0xC0) // |x| < 0.4375
			{
				if (exponent < 0x3FFF - (x.GetSignificandBitLength() + 1) / 2)
				{
					// if |x| is small, atan(x)~=x
					if (TFloat.IsSubnormal(x))
					{
						x = TFloat.NegativeInfinity;
					}
					return x;
				}

				id = -1;
			}
			else
			{
				x = TFloat.Abs(x);

				if (expman < ((0x3fff << 8) + 0x30))
				{
					// |x| < 1.1875
					if (expman < ((0x3fff - 1) << 8) + 0x60)
					{
						// 7/16 <= |x| < 11/16 
						id = 0;
						x = (TFloat.Two * x - TFloat.One) / (TFloat.Two + x);
					}
					else
					{
						id = 1;
						x = (x - TFloat.One) / (x + TFloat.One);
					}
				}
				else
				{
					if (expman < ((0x3fff + 1) << 8) + 0x38)
					{
						id = 2;
						x = (x - TFloat.OneHalf) / (TFloat.One + TFloat.OneHalf * x);
					}
					else
					{
						id = 3;
						x = TFloat.NegativeOne / x;
					}
				}
			}

			// end of argument reduction
			z = x * x;
			w = z * z;
			// break sum aT[i]z**(i+1) into odd and even poly
			s1 = z * TOperation.Even(w);
			s2 = w * TOperation.Odd(w);
			if (id < 0)
				return x - x * (s1 + s2);
			z = TOperation.AtanHi[id] - ((x * (s1 + s2) - TOperation.AtanLo[id]) - x);
			return sign ? -z : z;
		}
		public static TFloat Atan2<TOperation, TFloat>(TFloat y, TFloat x)
			where TOperation : IAtan2Operator<TFloat>
			where TFloat : struct, IBinaryFloatingPointIeee754<TFloat>, INumberConstants<TFloat>, IShapeIeee754
		{
			TFloat z;
			int m, ex, ey;

			if (TFloat.IsNaN(x))
			{
				return x;
			}
			if (TFloat.IsNaN(y))
			{
				return y;
			}

			ex = (int)(x.SignExponent & 0x7FFF);
			ey = (int)(y.SignExponent & 0x7FFF);
			m = (int)(2 * (x.SignExponent >> 15) | (y.SignExponent >> 15));

			if (y == TFloat.Zero)
			{
				switch (m)
				{
					case 0:
					case 1:
						return y;
					case 2:
						return TFloat.Two * TOperation.PiOver2Hi;
					case 3:
						return -TFloat.Two * TOperation.PiOver2Hi;
					default:
						break;
				}
			}
			if (x == TFloat.Zero)
			{
				return ((m & 1) != 0) ? -TOperation.PiOver2Hi : TOperation.PiOver2Hi;
			}
			if (ex == 0x7FFF)
			{
				if (ey == 0x7FFF)
				{
					TFloat oneHalf = TFloat.OneHalf;
					switch (m)
					{
						case 0: // atan(+INF,+INF)
							return TOperation.PiOver2Hi / TFloat.Two;
						case 1: // atan(-INF,+INF)
							return -TOperation.PiOver2Hi / TFloat.Two;
						case 2: // atan(+INF,-INF)
							return oneHalf * TOperation.PiOver2Hi;
						case 3: // atan(-INF,-INF)
							return -oneHalf * TOperation.PiOver2Hi;
						default:
							break;
					}
				}
				else
				{
					switch (m)
					{
						case 0: // atan(+...,+INF)
							return TFloat.Zero;
						case 1: // atan(-...,+INF)
							return TFloat.NegativeZero;
						case 2: // atan(+...,-INF)
							return TFloat.Two * TOperation.PiOver2Hi;
						case 3: // atan(-...,-INF)
							return -TFloat.Two * TOperation.PiOver2Hi;
						default:
							break;
					}
				}
			}
			if (ex + 120 < ey || ey == 0x7FFF)
			{
				return ((m & 1) != 0) ? -TOperation.PiOver2Hi : TOperation.PiOver2Hi;
			}
			// z = atan(|y/x|) without spurious underflow
			if (((m & 2) != 0) && ey + 120 < ex)
			{
				z = TFloat.Zero;
			}
			else
			{
				z = TFloat.Atan(TFloat.Abs(y / x));
			}

			switch (m)
			{
				case 0: // atan(+,+)
					return z;
				case 1: // atan(-,+)
					return -z;
				case 2: // atan(+,-)
					return TFloat.Two * TOperation.PiOver2Hi - (z - TFloat.Two * TOperation.PiOver2Lo);
				default: // atan(-,-)
					return (z - TFloat.Two * TOperation.PiOver2Lo) - TFloat.Two * TOperation.PiOver2Hi;
			}
		}
		public static TFloat Atanh<TFloat>(TFloat x)
			where TFloat : struct, IBinaryFloatingPointIeee754<TFloat>, INumberConstants<TFloat>, IShapeIeee754
		{
			uint e = x.SignExponent & 0x7FFF;
			bool s = TFloat.IsNegative(x);

			/* |x| */
			x = TFloat.Abs(x);

			if (e < 0x3FF - 1)
			{
				if (e >= 0x3FF - x.GetSignificandBitLength() / 2)
				{
					x = TFloat.Half * TFloat.LogP1(TFloat.Two * x + TFloat.Two * x * x / (TFloat.One - x));
				}
			}
			else
			{
				x = TFloat.Half * TFloat.LogP1(TFloat.Two * (x / (TFloat.One - x)));
			}

			return s ? -x : x;
		}
		public static TFloat Cbrt<TOperation, TFloat>(TFloat x)
			where TOperation : ICbrtOperator<TFloat>
			where TFloat : struct, IBinaryFloatingPointIeee754<TFloat>, INumberConstants<TFloat>, IShapeIeee754
		{
			// B1 = (127-127.0/3-0.03306235651)*2**23
			const uint B1 = 709958130;

			TFloat r, s, t, w;
			double dr, dt, dx;
			float ft;
			int e = (int)(x.SignExponent & 0x7FFF);
			int sign = (int)(x.SignExponent & 0x8000);
			TFloat u = x;

			/*
			 * If x = +-Inf, then cbrt(x) = +-Inf.
			 * If x = NaN, then cbrt(x) = NaN.
			 */
			if (e == 0x7fff)
				return x;
			if (e == 0)
			{
				/* Adjust subnormal numbers. */
				u *= TOperation.Big;
				e = (int)(u.SignExponent & 0x7fff);
				/* If x = +-0, then cbrt(x) = +-0. */
				if (e == 0)
					return x;
				e -= 120;
			}
			e -= 0x3FFF;
			x = TOperation.Ctor(false, 0x3FFF, u.Mantissa);

			switch (e % 3)
			{
				case 1:
				case -2:
					x *= TFloat.Two;
					e--;
					break;
				case 2:
				case -1:
					x *= TFloat.Four; // x *= 4
					e -= 2;
					break;
			}

			TFloat v = TOperation.Ctor(sign != 0, 0x3FFF + e / 3, TFloat.One.Mantissa);

			/*
			 * The following is the guts of s_cbrtf, with the handling of
			 * special values removed and extra care for accuracy not taken,
			 * but with most of the extra accuracy not discarded.
			 */

			/* ~5-bit estimate: */
			ft = BitConverter.UInt32BitsToSingle((BitConverter.SingleToUInt32Bits(TOperation.ToSingle(x)) & 0x7FFF_FFFF) / 3 + B1);

			/* ~16-bit estimate: */
			dx = TOperation.ToDouble(x);
			dt = ft;
			dr = dt * dt * dt;
			dt = dt * (dx + dx + dr) / (dx + dr + dr);

			/* ~47-bit estimate: */
			dr = dt * dt * dt;
			dt = dt * (dx + dx + dr) / (dx + dr + dr);

			t = TOperation.RoundAwayFromZeroOrNBits(TOperation.FromDouble(dt));

			/*
			 * Final step Newton iteration to 64 or 113 bits with
			 * error < 0.667 ulps
			 */
			s = t * t;         /* t*t is exact */
			r = x / s;         /* error <= 0.5 ulps; |r| < |t| */
			w = t + t;         /* t+t is exact */
			r = (r - t) / (w + r); /* r-t is exact; w+r ~= 3*t */
			t = t + t * r;       /* error <= 0.5 + 0.5/3 + epsilon */

			t *= v;
			return t;
		}
		public static TFloat Ceiling<TOperation, TFloat>(TFloat x)
			where TOperation : IRoundOperator<TFloat>
			where TFloat : struct, IBinaryFloatingPointIeee754<TFloat>, INumberConstants<TFloat>, IShapeIeee754
		{
			var exponent = x.SignExponent & 0x7FFF;
			bool sign = TFloat.IsNegative(x);
			TFloat y;
			if (exponent >= 0x3FFF + x.GetSignificandBitLength() - 1 || x == TFloat.Zero)
			{
				return x;
			}
			// newBase = int(x) - x, where int(x) is an integer neighbor of x
			TFloat toint = TOperation.ToInt;
			if (sign)
			{
				y = x - toint + toint - x;
			}
			else
			{
				y = x + toint - toint - x;
			}
			// special case because of non-nearest rounding modes
			if (exponent <= 0x3FFF - 1)
			{
				return sign ? TFloat.NegativeZero : TFloat.One;
			}
			if (y < TFloat.Zero)
			{
				return x + y + TFloat.One;
			}
			return x + y;
		}
		public static TFloat Cos<TOperation, TFloat>(TFloat x)
			where TOperation : ICosOperator<TFloat>, ISinOperator<TFloat>
			where TFloat : unmanaged, IBinaryFloatingPointIeee754<TFloat>, INumberConstants<TFloat>, IShapeIeee754
		{
			uint n;
			TFloat hi, lo;
			Span<TFloat> y = stackalloc TFloat[2];

			if ((x.SignExponent & 0x7FFF) == 0x7FFF)
			{
				return x - x;
			}
			x = TFloat.Abs(x);
			if (x < TOperation.PiOver4)
			{
				if (x.SignExponent < 0x3FFF - x.GetSignificandBitLength())
				{
					return TFloat.One + x;
				}
				return TOperation.CosCore(x, TFloat.Zero);
			}
			n = (uint)TOperation.RemPiOver2(x, y);
			hi = y[0];
			lo = y[1];

			switch (n & 3)
			{
				case 0:
					return TOperation.CosCore(hi, lo);
				case 1:
					return -TOperation.SinCore(hi, lo, 1);
				case 2:
					return -TOperation.CosCore(hi, lo);
				case 3:
				default:
					return TOperation.SinCore(hi, lo, 1);
			}
		}
	}
}
