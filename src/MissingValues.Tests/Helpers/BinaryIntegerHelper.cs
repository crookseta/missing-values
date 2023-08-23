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
		/// <inheritdoc cref="IBinaryInteger{TSelf}.DivRem(TSelf, TSelf)"/>
		public static (TSelf Quotient, TSelf Remainder) DivRem(TSelf left, TSelf right)
		{
			return TSelf.DivRem(left, right);
		}
		/// <inheritdoc cref="IBinaryInteger{TSelf}.LeadingZeroCount(TSelf)"/>
		public static TSelf LeadingZeroCount(TSelf value)
		{
			return TSelf.LeadingZeroCount(value);
		}
		/// <inheritdoc cref="IBinaryInteger{TSelf}.PopCount(TSelf)"/>
		public static TSelf PopCount(TSelf value)
		{
			return TSelf.PopCount(value);
		}
		/// <inheritdoc cref="IBinaryInteger{TSelf}.RotateLeft(TSelf, int)"/>
		public static TSelf RotateLeft(TSelf value, int rotateAmount)
		{
			return TSelf.RotateLeft(value, rotateAmount);
		}
		/// <inheritdoc cref="IBinaryInteger{TSelf}.RotateRight(TSelf, int)"/>
		public static TSelf RotateRight(TSelf value, int rotateAmount)
		{
			return TSelf.RotateRight(value, rotateAmount);
		}
		/// <inheritdoc cref="IBinaryInteger{TSelf}.TrailingZeroCount(TSelf)"/>
		public static TSelf TrailingZeroCount(TSelf value)
		{
			return TSelf.TrailingZeroCount(value);
		}
		/// <inheritdoc cref="IBinaryInteger{TSelf}.TryReadBigEndian(ReadOnlySpan{byte}, bool, out TSelf)"/>
		public static bool TryReadBigEndian(ReadOnlySpan<byte> source, bool isUnsigned, out TSelf value)
		{
			return TSelf.TryReadBigEndian(source, isUnsigned, out value);
		}
		/// <inheritdoc cref="IBinaryInteger{TSelf}.TryReadLittleEndian(ReadOnlySpan{byte}, bool, out TSelf)"/>
		public static bool TryReadLittleEndian(ReadOnlySpan<byte> source, bool isUnsigned, out TSelf value)
		{
			return TSelf.TryReadLittleEndian(source, isUnsigned, out value);
		}
		/// <inheritdoc cref="IBinaryInteger{TSelf}.GetByteCount"/>
		public static int GetByteCount(TSelf value)
		{
			return value.GetByteCount();
		}
		/// <inheritdoc cref="IBinaryInteger{TSelf}.GetShortestBitLength"/>
		public static int GetShortestBitLength(TSelf value)
		{
			return value.GetShortestBitLength();
		}
		/// <inheritdoc cref="IBinaryInteger{TSelf}.TryWriteBigEndian(Span{byte}, out int)"/>
		public static bool TryWriteBigEndian(TSelf value, Span<byte> destination, out int bytesWritten)
		{
			return value.TryWriteBigEndian(destination, out bytesWritten);
		}
		/// <inheritdoc cref="IBinaryInteger{TSelf}.TryWriteLittleEndian(Span{byte}, out int)"/>
		public static bool TryWriteLittleEndian(TSelf value, Span<byte> destination, out int bytesWritten)
		{
			return value.TryWriteLittleEndian(destination, out bytesWritten);
		}

	}
}
