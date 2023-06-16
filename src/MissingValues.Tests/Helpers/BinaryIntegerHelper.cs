using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues.Tests.Helpers
{
	internal static class BinaryIntegerHelper<TSelf>
		where TSelf : IBinaryInteger<TSelf>
	{
		public static (TSelf Quotient, TSelf Remainder) DivRem(TSelf left, TSelf right)
		{
			return TSelf.DivRem(left, right);
		}

		public static TSelf LeadingZeroCount(TSelf value)
		{
			return TSelf.LeadingZeroCount(value);
		}

		public static TSelf PopCount(TSelf value)
		{
			return TSelf.PopCount(value);
		}
		
		public static TSelf RotateLeft(TSelf value, int rotateAmount)
		{
			return TSelf.RotateLeft(value, rotateAmount);
		}
		
		public static TSelf RotateRight(TSelf value, int rotateAmount)
		{
			return TSelf.RotateRight(value, rotateAmount);
		}
		
		public static TSelf TrailingZeroCount(TSelf value)
		{
			return TSelf.TrailingZeroCount(value);
		}
		
		public static bool TryReadBigEndian(ReadOnlySpan<byte> source, bool isUnsigned, out TSelf value)
		{
			return TSelf.TryReadBigEndian(source, isUnsigned, out value);
		}
		
		public static bool TryReadLittleEndian(ReadOnlySpan<byte> source, bool isUnsigned, out TSelf value)
		{
			return TSelf.TryReadLittleEndian(source, isUnsigned, out value);
		}

		public static int GetByteCount(TSelf value)
		{
			return value.GetByteCount();
		}
		
		public static int GetShortestBitLength(TSelf value)
		{
			return value.GetShortestBitLength();
		}

		public static bool TryWriteBigEndian(TSelf value, Span<byte> source, out int bytesWritten)
		{
			return value.TryWriteBigEndian(source, out bytesWritten);
		}
		
		public static bool TryWriteLittleEndian(TSelf value, Span<byte> source, out int bytesWritten)
		{
			return value.TryWriteLittleEndian(source, out bytesWritten);
		}

	}
}
