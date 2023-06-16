using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues.Tests.Helpers
{
	internal static class MathOperatorsHelper
	{
		public static TResult AdditionOperation<TSelf, TOther, TResult>(TSelf left, TOther right)
			where TSelf : IAdditionOperators<TSelf, TOther, TResult>
		{
			return left + right;
		}
		public static TResult CheckedAdditionOperation<TSelf, TOther, TResult>(TSelf left, TOther right)
			where TSelf : IAdditionOperators<TSelf, TOther, TResult>
		{
			return checked(left + right);
		}

		public static TSelf IncrementOperation<TSelf>(TSelf value)
			where TSelf : IIncrementOperators<TSelf>
		{
			return ++value;
		}
		public static TSelf CheckedIncrementOperation<TSelf>(TSelf value)
			where TSelf : IIncrementOperators<TSelf>
		{
			return checked(++value);
		}
		
		public static TResult SubtractionOperation<TSelf, TOther, TResult>(TSelf left, TOther right)
			where TSelf : ISubtractionOperators<TSelf, TOther, TResult>
		{
			return left - right;
		}
		public static TResult CheckedSubtractionOperation<TSelf, TOther, TResult>(TSelf left, TOther right)
			where TSelf : ISubtractionOperators<TSelf, TOther, TResult>
		{
			return checked(left - right);
		}

		public static TSelf DecrementOperation<TSelf>(TSelf value)
			where TSelf : IDecrementOperators<TSelf>
		{
			return --value;
		}
		public static TSelf CheckedDecrementOperation<TSelf>(TSelf value)
			where TSelf : IDecrementOperators<TSelf>
		{
			return checked(--value);
		}

		public static TResult MultiplicationOperation<TSelf, TOther, TResult>(TSelf left, TOther right)
			where TSelf : IMultiplyOperators<TSelf, TOther, TResult>
		{
			return left * right;
		}
		public static TResult CheckedMultiplicationOperation<TSelf, TOther, TResult>(TSelf left, TOther right)
			where TSelf : IMultiplyOperators<TSelf, TOther, TResult>
		{
			return checked(left * right);
		}
		
		public static TResult DivisionOperation<TSelf, TOther, TResult>(TSelf left, TOther right)
			where TSelf : IDivisionOperators<TSelf, TOther, TResult>
		{
			return left / right;
		}
		public static TResult CheckedDivisionOperation<TSelf, TOther, TResult>(TSelf left, TOther right)
			where TSelf : IDivisionOperators<TSelf, TOther, TResult>
		{
			return checked(left / right);
		}

		public static TResult ModulusOperation<TSelf, TOther, TResult>(TSelf left, TOther right)
			where TSelf : IModulusOperators<TSelf, TOther, TResult>
		{
			return left % right;
		}
	}

    internal static class ComparisonOperatorsHelper<TSelf, TOther, TResult>
		where TSelf : IComparisonOperators<TSelf, TOther, TResult>
	{
		public static TResult GreaterThanOperation(TSelf left, TOther right)
		{
			return left > right;
		}
		public static TResult GreaterThanOrEqualOperation(TSelf left, TOther right)
		{
			return left >= right;
		}
		public static TResult LessThanOperation(TSelf left, TOther right)
		{
			return left < right;
		}
		public static TResult LessThanOrEqualOperation(TSelf left, TOther right)
		{
			return left <= right;
		}
	}

    internal static class EqualityOperatorsHelper<TSelf, TOther, TResult>
		where TSelf : IEqualityOperators<TSelf, TOther, TResult>
	{
		public static TResult EqualityOperation(TSelf left, TOther right)
		{
			return left == right;
		}
		public static TResult InequalityOperation(TSelf left, TOther right)
		{
			return left != right;
		}
	}

    internal static class BitwiseOperatorsHelper<TSelf, TOther, TResult>
		where TSelf : IBitwiseOperators<TSelf, TOther, TResult>
	{
		public static TResult BitwiseAndOperation(TSelf left, TOther right)
		{
			return left & right;
		}
		public static TResult BitwiseOrOperation(TSelf left, TOther right)
		{
			return left | right;
		}
		public static TResult ExclusiveOrOperation(TSelf left, TOther right)
		{
			return left ^ right;
		}
		public static TResult OnesComplementOperation(TSelf value)
		{
			return ~value;
		}
	}
	internal static class ShiftOperatorsHelper<TSelf, TOther, TResult>
		where TSelf : IShiftOperators<TSelf, TOther, TResult>
	{
		public static TResult LeftShiftOperation(TSelf value, TOther shiftAmount)
		{
			return value << shiftAmount;
		}
		public static TResult RightShiftOperation(TSelf value, TOther shiftAmount)
		{
			return value >> shiftAmount;
		}
		public static TResult UnsignedRightShiftOperation(TSelf value, TOther shiftAmount)
		{
			return value >>> shiftAmount;
		}
	}
	internal static class UnaryOperatorsHelper
	{
		public static TResult UnaryPlusOperation<TSelf, TResult>(TSelf value)
			where TSelf : IUnaryPlusOperators<TSelf, TResult>
		{
			return +value;
		}
		public static TResult UnaryNegationOperation<TSelf, TResult>(TSelf value)
			where TSelf : IUnaryNegationOperators<TSelf, TResult>
		{
			return -value;
		}
		public static TResult CheckedUnaryNegationOperation<TSelf, TResult>(TSelf value)
			where TSelf : IUnaryNegationOperators<TSelf, TResult>
		{
			return checked(-value);
		}
	}
}
