using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues.Internals
{
	internal sealed class FloatDebugView<T>
		where T : unmanaged, IBinaryFloatingPointIeee754<T>
	{
		private readonly bool _sign;
		private readonly UInt32Wrapper _exponent;
		private readonly UInt256Wrapper _significand;

		public FloatDebugView(T floating)
		{
			_sign = T.IsNegative(floating);
			if (floating is Quad quad)
			{
				uint e = Quad.ExtractBiasedExponentFromBits(Quad.QuadToUInt128Bits(quad));
				UInt256 s = Quad.ExtractTrailingSignificandFromBits(Quad.QuadToUInt128Bits(quad));
				_exponent =  Unsafe.As<uint, UInt32Wrapper>(ref e);
				_significand = Unsafe.As<UInt256, UInt256Wrapper>(ref s);
			}
			else if (floating is Octo octo)
			{
				uint e = Octo.ExtractBiasedExponentFromBits(Octo.OctoToUInt256Bits(octo));
				UInt256 s = Octo.ExtractTrailingSignificandFromBits(Octo.OctoToUInt256Bits(octo));
				_exponent = Unsafe.As<uint, UInt32Wrapper>(ref e);
				_significand = Unsafe.As<UInt256, UInt256Wrapper>(ref s);
			}
			else
			{
				throw new NotSupportedException();
			}
        }

		public bool Sign => _sign;
		public UInt32Wrapper Exponent => _exponent;
		public UInt256Wrapper Significand => _significand;

		[DebuggerDisplay($"{{{nameof(ToString)}(),nq}}")]
		public readonly struct UInt256Wrapper
		{
			private readonly UInt256 _value;

			public override string ToString()
			{
				return _value.ToString("X", CultureInfo.InvariantCulture);
			}
		}
		[DebuggerDisplay($"{{{nameof(ToString)}(),nq}}")]
		public readonly struct UInt32Wrapper
		{
			private readonly uint _value;

			public override string ToString()
			{
				return _value.ToString("X");
			}
		}
	}
}
