using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues.Tests.Helpers
{
	internal static class FloatingPoint<TSelf>
		where TSelf : IFloatingPoint<TSelf>
	{
		/// <inheritdoc cref="IFloatingPoint{TSelf}.Ceiling(TSelf)"/>
		public static TSelf Ceiling(TSelf x) => TSelf.Ceiling(x);
		/// <inheritdoc cref="IFloatingPoint{TSelf}.Floor(TSelf)"/>
		public static TSelf Floor(TSelf x) => TSelf.Floor(x);
		/// <inheritdoc cref="IFloatingPoint{TSelf}.Round(TSelf)"/>
		public static TSelf Round(TSelf x) => TSelf.Round(x);
		/// <inheritdoc cref="IFloatingPoint{TSelf}.Round(TSelf, int)"/>
		public static TSelf Round(TSelf x, int digits) => TSelf.Round(x, digits);
		/// <inheritdoc cref="IFloatingPoint{TSelf}.Round(TSelf, MidpointRounding)"/>
		public static TSelf Round(TSelf x, MidpointRounding mode) => TSelf.Round(x, mode);
		/// <inheritdoc cref="IFloatingPoint{TSelf}.Round(TSelf, int, MidpointRounding)"/>
		public static TSelf Round(TSelf x, int digits, MidpointRounding mode) => TSelf.Round(x, digits, mode);
		/// <inheritdoc cref="IFloatingPoint{TSelf}.Truncate(TSelf)"/>
		public static TSelf Truncate(TSelf x) => TSelf.Truncate(x);
		/// <inheritdoc cref="IFloatingPoint{TSelf}.GetExponentByteCount()"/>
		public static int GetExponentByteCount(TSelf x) => x.GetExponentByteCount();
		/// <inheritdoc cref="IFloatingPoint{TSelf}.GetExponentShortestBitLength"/>
		public static int GetExponentShortestBitLength(TSelf x) => x.GetExponentShortestBitLength();
		/// <inheritdoc cref="IFloatingPoint{TSelf}.GetSignificandBitLength"/>
		public static int GetSignificandBitLength(TSelf x) => x.GetSignificandBitLength();
		/// <inheritdoc cref="IFloatingPoint{TSelf}.GetSignificandByteCount"/>
		public static int GetSignificandByteCount(TSelf x) => x.GetSignificandByteCount();
		public static bool TryWriteExponentBigEndian(TSelf x, Span<byte> destination, out int bytesWritten) => x.TryWriteExponentBigEndian(destination, out bytesWritten);
		public static bool TryWriteExponentLittleEndian(TSelf x, Span<byte> destination, out int bytesWritten) => x.TryWriteExponentLittleEndian(destination, out bytesWritten);
		public static bool TryWriteSignificandBigEndian(TSelf x, Span<byte> destination, out int bytesWritten) => x.TryWriteSignificandBigEndian(destination, out bytesWritten);
		public static bool TryWriteSignificandLittleEndian(TSelf x, Span<byte> destination, out int bytesWritten) => x.TryWriteSignificandLittleEndian(destination, out bytesWritten);
		public static int WriteExponentBigEndian(TSelf x, byte[] destination) => x.WriteExponentBigEndian(destination);
		public static int WriteExponentBigEndian(TSelf x, byte[] destination, int startIndex) => x.WriteExponentBigEndian(destination, startIndex);
		public static int WriteExponentBigEndian(TSelf x, Span<byte> destination) => x.WriteExponentBigEndian(destination);
		public static int WriteExponentLittleEndian(TSelf x, byte[] destination) => x.WriteExponentLittleEndian(destination);
		public static int WriteExponentLittleEndian(TSelf x, byte[] destination, int startIndex) => x.WriteExponentLittleEndian(destination, startIndex);
		public static int WriteExponentLittleEndian(TSelf x, Span<byte> destination) => x.WriteExponentLittleEndian(destination);
		public static int WriteSignificandBigEndian(TSelf x, byte[] destination) => x.WriteSignificandBigEndian(destination);
		public static int WriteSignificandBigEndian(TSelf x, byte[] destination, int startIndex) => x.WriteSignificandBigEndian(destination, startIndex);
		public static int WriteSignificandBigEndian(TSelf x, Span<byte> destination) => x.WriteSignificandBigEndian(destination);
		public static int WriteSignificandLittleEndian(TSelf x, byte[] destination) => x.WriteSignificandLittleEndian(destination);
		public static int WriteSignificandLittleEndian(TSelf x, byte[] destination, int startIndex) => x.WriteSignificandLittleEndian(destination, startIndex);
		public static int WriteSignificandLittleEndian(TSelf x, Span<byte> destination) => x.WriteSignificandLittleEndian(destination);
	}
}
