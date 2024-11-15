using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MissingValues.Tests.Helpers
{
	public class NumberTheoryData<TSelf> : TheoryData<TSelf>
		where TSelf : INumberBase<TSelf>
	{
        public NumberTheoryData(IEnumerable<TSelf> data)
        {
			Contract.Assert(data is not null && data.Any());

			foreach (var dat in data)
			{
				Add(dat);
			}
        }
    }

	public class OperationTheoryData<TSelf, TOther, TResult> : TheoryData<TSelf, TOther, TResult>
	{
		public OperationTheoryData(IEnumerable<(TSelf, TOther, TResult)> data)
		{
			Contract.Assert(data is not null && data.Any());

			foreach (var dat in data)
			{
				Add(dat.Item1, dat.Item2, dat.Item3);
			}
		}
	}

	public class FusedMultiplyAddTheoryData<TSelf> : TheoryData<TSelf, TSelf, TSelf, TSelf>
		where TSelf : IFloatingPointIeee754<TSelf>
	{
        public FusedMultiplyAddTheoryData(IEnumerable<(TSelf, TSelf, TSelf, TSelf)> data)
        {
			Contract.Assert(data is not null && data.Any());

			foreach (var dat in data)
			{
				Add(dat.Item1, dat.Item2, dat.Item3, dat.Item4);
			}
		}
    }

	public class RoundTheoryData<TFloat> : TheoryData<TFloat, int, MidpointRounding, TFloat>
	{
        public RoundTheoryData(IEnumerable<(TFloat, int, MidpointRounding, TFloat)> data)
        {
			Contract.Assert(data is not null && data.Any());

			foreach (var dat in data)
			{
				Add(dat.Item1, dat.Item2, dat.Item3, dat.Item4);
			}
		}
    }

	public class UnaryTheoryData<TSelf, TResult> : TheoryData<TSelf, TResult>
	{
		public UnaryTheoryData(IEnumerable<(TSelf, TResult)> data)
		{
			Contract.Assert(data is not null && data.Any());

			foreach (var dat in data)
			{
				Add(dat.Item1, dat.Item2);
			}
		}
	}

	public class CastingTheoryData<TFrom, TTo> : TheoryData<TFrom, TTo>
	{
        public CastingTheoryData(IEnumerable<(TFrom, TTo)> data)
        {
			Contract.Assert(data is not null && data.Any());

			foreach (var dat in data)
			{
				Add(dat.Item1, dat.Item2);
			}
		}
    }

	public class ComparisonOperatorsTheoryData<TSelf, TOther> : TheoryData<TSelf, TOther, bool>
	{
        public ComparisonOperatorsTheoryData(IEnumerable<(TSelf, TOther, bool)> data)
        {
			Contract.Assert(data is not null && data.Any());

			foreach (var dat in data)
			{
				Add(dat.Item1, dat.Item2, dat.Item3);
			}
		}
    }

	public class TryParseTheoryData<T> : TheoryData<string, bool, T>
		where T : IParsable<T>
	{
		public TryParseTheoryData(IEnumerable<(string, bool, T)> data)
		{
			Contract.Assert(data is not null && data.Any());

			foreach (var dat in data)
			{
				Add(dat.Item1, dat.Item2, dat.Item3);
			}
		}
	}

	public class FormatStringTheoryData : TheoryData<IFormattable, string, NumberFormatInfo?, string>
	{
        public FormatStringTheoryData(IEnumerable<(IFormattable, string, NumberFormatInfo?, string)> data)
        {
			Contract.Assert(data is not null && data.Any());

            foreach (var dat in data)
            {
				Add(dat.Item1, dat.Item2, dat.Item3, dat.Item4);
            }
        }
    }
	public class FormatParsingTheoryData<TNumber> : TheoryData<string, NumberStyles, NumberFormatInfo?, TNumber, bool>
		where TNumber : INumber<TNumber>
	{
        public FormatParsingTheoryData(IEnumerable<(string, NumberStyles, NumberFormatInfo?, TNumber, bool)> data)
        {
			Contract.Assert(data is not null && data.Any());

            foreach (var dat in data)
            {
				Add(dat.Item1, dat.Item2, dat.Item3, dat.Item4, dat.Item5);
            }
        }
    }
}
