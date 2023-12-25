using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues.Tests.Helpers
{
	internal static class GenericFloatingPointFunctions
	{
		public static T Acos<T>(T x) where T : ITrigonometricFunctions<T> => T.Acos(x);
		public static T Acosh<T>(T x) where T : IHyperbolicFunctions<T> => T.Acosh(x);
		public static T AcosPi<T>(T x) where T : ITrigonometricFunctions<T> => T.AcosPi(x);
		public static T Asin<T>(T x) where T : ITrigonometricFunctions<T> => T.Asin(x);
		public static T Asinh<T>(T x) where T : IHyperbolicFunctions<T> => T.Asinh(x);
		public static T AsinPi<T>(T x) where T : ITrigonometricFunctions<T> => T.AsinPi(x);
		public static T Atan<T>(T x) where T : ITrigonometricFunctions<T> => T.Atan(x);
		public static T AtanPi<T>(T x) where T : ITrigonometricFunctions<T> => T.AtanPi(x);
		public static T Atanh<T>(T x) where T : IHyperbolicFunctions<T> => T.Atanh(x);
		public static T Cbrt<T>(T x) where T : IRootFunctions<T> => T.Cbrt(x);
		public static T Cos<T>(T x) where T : ITrigonometricFunctions<T> => T.Cos(x);
		public static T Cosh<T>(T x) where T : IHyperbolicFunctions<T> => T.Cosh(x);
		public static T CosPi<T>(T x) where T : ITrigonometricFunctions<T> => T.CosPi(x);
		public static T Exp<T>(T x) where T : IExponentialFunctions<T> => T.Exp(x);
		public static T ExpM1<T>(T x) where T : IExponentialFunctions<T> => T.ExpM1(x);
		public static T Exp2<T>(T x) where T : IExponentialFunctions<T> => T.Exp2(x);
		public static T Exp2M1<T>(T x) where T : IExponentialFunctions<T> => T.Exp2M1(x);
		public static T Exp10<T>(T x) where T : IExponentialFunctions<T> => T.Exp10(x);
		public static T Exp10M1<T>(T x) where T : IExponentialFunctions<T> => T.Exp10M1(x);
		public static T Hypot<T>(T x, T y) where T : IRootFunctions<T> => T.Hypot(x, y);
		public static T Log<T>(T x) where T : ILogarithmicFunctions<T> => T.Log(x);
		public static T Log<T>(T x, T newBase) where T : ILogarithmicFunctions<T> => T.Log(x, newBase);
		public static T LogP1<T>(T x) where T : ILogarithmicFunctions<T> => T.LogP1(x);
		public static T Log2<T>(T x) where T : ILogarithmicFunctions<T> => T.Log2(x);
		public static T Log2P1<T>(T x) where T : ILogarithmicFunctions<T> => T.Log2P1(x);
		public static T Log10<T>(T x) where T : ILogarithmicFunctions<T> => T.Log10(x);
		public static T Log10P1<T>(T x) where T : ILogarithmicFunctions<T> => T.Log10P1(x);
		public static T Pow<T>(T x, T y) where T : IPowerFunctions<T> => T.Pow(x, y);
		public static T RootN<T>(T x, int y) where T : IRootFunctions<T> => T.RootN(x, y);
		public static T Sin<T>(T x) where T : ITrigonometricFunctions<T> => T.Sin(x);
		public static (T Sin, T Cos) SinCos<T>(T x) where T : ITrigonometricFunctions<T> => T.SinCos(x);
		public static (T Sin, T Cos) SinCosPi<T>(T x) where T : ITrigonometricFunctions<T> => T.SinCosPi(x);
		public static T Sinh<T>(T x) where T : IHyperbolicFunctions<T> => T.Sinh(x);
		public static T SinPi<T>(T x) where T : ITrigonometricFunctions<T> => T.SinPi(x);
		public static T Sqrt<T>(T x) where T : IRootFunctions<T> => T.Sqrt(x);
		public static T Tan<T>(T x) where T : ITrigonometricFunctions<T> => T.Tan(x);
		public static T Tanh<T>(T x) where T : IHyperbolicFunctions<T> => T.Tanh(x);
		public static T TanPi<T>(T x) where T : ITrigonometricFunctions<T> => T.TanPi(x);
	}
}
