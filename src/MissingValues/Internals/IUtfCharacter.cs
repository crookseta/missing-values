using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Unicode;

namespace MissingValues.Internals
{
	internal interface IUtfCharacter<TSelf> :
		IEquatable<TSelf>,
		IEqualityOperators<TSelf, TSelf, bool>
		where TSelf : unmanaged, IUtfCharacter<TSelf>?
	{
		static abstract TSelf NullCharacter { get; }
		static abstract TSelf WhiteSpaceCharacter { get; }
		static abstract ReadOnlySpan<TSelf> Digits { get; }
		static abstract ReadOnlySpan<byte> TwoDigitsAsBytes { get; }

		static abstract ReadOnlySpan<char> CastToCharSpan(ReadOnlySpan<TSelf> chars);
		static abstract Span<char> CastToCharSpan(Span<TSelf> chars);
		static abstract ReadOnlySpan<byte> CastToByteSpan(ReadOnlySpan<TSelf> chars);
		static abstract Span<byte> CastToByteSpan(Span<TSelf> chars);

		static abstract ReadOnlySpan<TSelf> CastFromCharSpan(ReadOnlySpan<char> chars);
		static abstract Span<TSelf> CastFromCharSpan(Span<char> chars);
		static abstract ReadOnlySpan<TSelf> CastFromByteSpan(ReadOnlySpan<byte> chars);
		static abstract Span<TSelf> CastFromByteSpan(Span<byte> chars);

		static abstract void Copy(ReadOnlySpan<char> origin, Span<TSelf> destination);
		static abstract void Copy(ReadOnlySpan<byte> origin, Span<TSelf> destination);

		static abstract bool TryParseInteger<T>(ReadOnlySpan<TSelf> s, NumberStyles style, IFormatProvider? provider, out T result) where T : struct, IBinaryInteger<T>;

		static virtual bool TryParsePartialInteger<T>(ReadOnlySpan<TSelf> s, NumberStyles style, IFormatProvider? provider, out T result, out int charsConsumed) 
			where T : struct, IBinaryInteger<T>
		{
			if (TSelf.TryParseInteger(s, style, provider, out result))
			{
				charsConsumed = s.Length;
				return true;
			}

			charsConsumed = 0;
			return false;
		}

		static abstract int GetLength(ReadOnlySpan<char> s);
		static abstract int GetLength(ReadOnlySpan<byte> utf8Text);

		static abstract TSelf ToUpper(TSelf value);
		static abstract TSelf ToLower(TSelf value);

		static virtual TSelf ToCharUpper(uint value)
		{
			value &= 0xF;
			value += '0';

			if (value > '9')
			{
				value += ('A' - ('9' + 1));
			}

			return (TSelf)value;
		}

		static virtual TSelf ToCharLower(uint value)
		{
			value &= 0xF;
			value += '0';

			if (value > '9')
			{
				value += ('a' - ('9' + 1));
			}

			return (TSelf)value;
		}
		static abstract bool IsWhiteSpace(TSelf value);
		static abstract bool IsDigit(TSelf value);
		static abstract bool IsHexDigit(TSelf value);

		static abstract bool Constains(ReadOnlySpan<TSelf> v1, ReadOnlySpan<TSelf> v2, StringComparison comparisonType);
		static abstract bool EndsWith(ReadOnlySpan<TSelf> v1, ReadOnlySpan<TSelf> v2, StringComparison comparisonType);
		static abstract bool StartsWith(ReadOnlySpan<TSelf> v1, ReadOnlySpan<TSelf> v2, StringComparison comparisonType);
		static abstract bool Equals(ReadOnlySpan<TSelf> v1, ReadOnlySpan<TSelf> v2, StringComparison comparisonType);

		static virtual ReadOnlySpan<TSelf> Trim(ReadOnlySpan<TSelf> s)
		{
			return TSelf.TrimEnd(TSelf.TrimStart(s));
		}
		static abstract ReadOnlySpan<TSelf> TrimStart(ReadOnlySpan<TSelf> s);
		static abstract ReadOnlySpan<TSelf> TrimEnd(ReadOnlySpan<TSelf> s);

		static abstract explicit operator TSelf(uint value);
		static abstract explicit operator TSelf(char value);
		static abstract explicit operator TSelf(byte value);

		static abstract explicit operator uint(TSelf value);
		static abstract explicit operator char(TSelf value);
		static abstract explicit operator byte(TSelf value);
	}

	internal readonly struct Utf16Char : IUtfCharacter<Utf16Char>
	{
		private readonly char _char;

		static Utf16Char IUtfCharacter<Utf16Char>.NullCharacter => new Utf16Char('\0');

		static ReadOnlySpan<Utf16Char> IUtfCharacter<Utf16Char>.Digits => new Utf16Char[]
		{
			new Utf16Char('0'),
			new Utf16Char('1'),
			new Utf16Char('2'),
			new Utf16Char('3'),
			new Utf16Char('4'),
			new Utf16Char('5'),
			new Utf16Char('6'),
			new Utf16Char('7'),
			new Utf16Char('8'),
			new Utf16Char('9'),
			new Utf16Char('A'),
			new Utf16Char('B'),
			new Utf16Char('C'),
			new Utf16Char('D'),
			new Utf16Char('E'),
			new Utf16Char('F'),
		};

		static Utf16Char IUtfCharacter<Utf16Char>.WhiteSpaceCharacter => new Utf16Char(' ');

		static ReadOnlySpan<byte> IUtfCharacter<Utf16Char>.TwoDigitsAsBytes =>
			MemoryMarshal.AsBytes<char>("00010203040506070809" +
										"10111213141516171819" +
										"20212223242526272829" +
										"30313233343536373839" +
										"40414243444546474849" +
										"50515253545556575859" +
										"60616263646566676869" +
										"70717273747576777879" +
										"80818283848586878889" +
										"90919293949596979899");

		private Utf16Char(char @char)
		{
			_char = @char;
		}

		static ReadOnlySpan<Utf16Char> IUtfCharacter<Utf16Char>.CastFromByteSpan(ReadOnlySpan<byte> chars)
		{
			throw new InvalidCastException();
		}

		internal static ReadOnlySpan<Utf16Char> CastFromCharSpan(ReadOnlySpan<char> chars)
		{
			return MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<char, Utf16Char>(ref MemoryMarshal.GetReference(chars)), chars.Length);
		}

		static ReadOnlySpan<Utf16Char> IUtfCharacter<Utf16Char>.CastFromCharSpan(ReadOnlySpan<char> chars)
		{
			return CastFromCharSpan(chars);
		}

		static ReadOnlySpan<byte> IUtfCharacter<Utf16Char>.CastToByteSpan(ReadOnlySpan<Utf16Char> chars)
		{
			throw new InvalidCastException();
		}

		internal static ReadOnlySpan<char> CastToCharSpan(ReadOnlySpan<Utf16Char> chars)
		{
			return MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<Utf16Char, char>(ref MemoryMarshal.GetReference(chars)), chars.Length);
		}

		static ReadOnlySpan<char> IUtfCharacter<Utf16Char>.CastToCharSpan(ReadOnlySpan<Utf16Char> chars)
		{
			return CastToCharSpan(chars);
		}

		static bool IUtfCharacter<Utf16Char>.Constains(ReadOnlySpan<Utf16Char> v1, ReadOnlySpan<Utf16Char> v2, StringComparison comparisonType)
		{
			return CastToCharSpan(v1).Contains(CastToCharSpan(v2), comparisonType);
		}

		static bool IUtfCharacter<Utf16Char>.EndsWith(ReadOnlySpan<Utf16Char> v1, ReadOnlySpan<Utf16Char> v2, StringComparison comparisonType)
		{
			return CastToCharSpan(v1).EndsWith(CastToCharSpan(v2), comparisonType);
		}

		static bool IUtfCharacter<Utf16Char>.Equals(ReadOnlySpan<Utf16Char> v1, ReadOnlySpan<Utf16Char> v2, StringComparison comparisonType)
		{
			return CastToCharSpan(v1).Equals(CastToCharSpan(v2), comparisonType);
		}

		public static ReadOnlySpan<Utf16Char> TrimStart(ReadOnlySpan<Utf16Char> s)
		{
			return CastFromCharSpan(CastToCharSpan(s).TrimStart());
		}

		public static ReadOnlySpan<Utf16Char> TrimEnd(ReadOnlySpan<Utf16Char> s)
		{
			return CastFromCharSpan(CastToCharSpan(s).TrimEnd());
		}

		static bool IUtfCharacter<Utf16Char>.IsWhiteSpace(Utf16Char value)
		{
			return char.IsWhiteSpace(value._char);
		}

		static bool IUtfCharacter<Utf16Char>.StartsWith(ReadOnlySpan<Utf16Char> v1, ReadOnlySpan<Utf16Char> v2, StringComparison comparisonType)
		{
			return CastToCharSpan(v1).StartsWith(CastToCharSpan(v2), comparisonType);
		}

		static Utf16Char IUtfCharacter<Utf16Char>.ToLower(Utf16Char value)
		{
			return new Utf16Char(char.ToLower(value._char));
		}

		static Utf16Char IUtfCharacter<Utf16Char>.ToUpper(Utf16Char value)
		{
			return new Utf16Char(char.ToUpper(value._char));
		}

		bool IEquatable<Utf16Char>.Equals(Utf16Char other)
		{
			return _char.Equals(other._char);
		}

		static bool IUtfCharacter<Utf16Char>.IsDigit(Utf16Char value)
		{
			return char.IsDigit(value._char);
		}

		static bool IUtfCharacter<Utf16Char>.IsHexDigit(Utf16Char value)
		{
			return char.IsAsciiHexDigit(value._char);
		}

		static bool IUtfCharacter<Utf16Char>.TryParseInteger<T>(ReadOnlySpan<Utf16Char> s, NumberStyles style, IFormatProvider? provider, out T result)
		{
			return T.TryParse(CastToCharSpan(s), style, provider, out result);
		}

		internal static Span<char> CastToCharSpan(Span<Utf16Char> chars)
		{
			return MemoryMarshal.CreateSpan(ref Unsafe.As<Utf16Char, char>(ref MemoryMarshal.GetReference(chars)), chars.Length);
		}

		static Span<char> IUtfCharacter<Utf16Char>.CastToCharSpan(Span<Utf16Char> chars)
		{
			return CastToCharSpan(chars);
		}

		static Span<byte> IUtfCharacter<Utf16Char>.CastToByteSpan(Span<Utf16Char> chars)
		{
			throw new NotImplementedException();
		}

		internal static Span<Utf16Char> CastFromCharSpan(Span<char> chars)
		{
			return MemoryMarshal.CreateSpan(ref Unsafe.As<char, Utf16Char>(ref MemoryMarshal.GetReference(chars)), chars.Length);
		}

		static Span<Utf16Char> IUtfCharacter<Utf16Char>.CastFromCharSpan(Span<char> chars)
		{
			return CastFromCharSpan(chars);
		}

		static Span<Utf16Char> IUtfCharacter<Utf16Char>.CastFromByteSpan(Span<byte> chars)
		{
			throw new NotImplementedException();
		}

		static int IUtfCharacter<Utf16Char>.GetLength(ReadOnlySpan<char> s)
		{
			return s.Length;
		}

		static int IUtfCharacter<Utf16Char>.GetLength(ReadOnlySpan<byte> utf8Text)
		{
			return Encoding.UTF8.GetCharCount(utf8Text);
		}

		static void IUtfCharacter<Utf16Char>.Copy(ReadOnlySpan<char> origin, Span<Utf16Char> destination)
		{
			CastFromCharSpan(origin).CopyTo(destination);
		}

		static void IUtfCharacter<Utf16Char>.Copy(ReadOnlySpan<byte> origin, Span<Utf16Char> destination)
		{
			Encoding.UTF8.GetChars(origin, CastToCharSpan(destination));
		}

		static bool IEqualityOperators<Utf16Char, Utf16Char, bool>.operator ==(Utf16Char left, Utf16Char right)
		{
			return left._char == right._char;
		}

		static bool IEqualityOperators<Utf16Char, Utf16Char, bool>.operator !=(Utf16Char left, Utf16Char right)
		{
			return left._char != right._char;
		}

		static explicit IUtfCharacter<Utf16Char>.operator Utf16Char(uint value) => new((char)value);

		static explicit IUtfCharacter<Utf16Char>.operator Utf16Char(char value) => new(value);

		static explicit IUtfCharacter<Utf16Char>.operator Utf16Char(byte value) => new((char)value);

		static explicit IUtfCharacter<Utf16Char>.operator uint(Utf16Char value) => value._char;

		static explicit IUtfCharacter<Utf16Char>.operator char(Utf16Char value) => value._char;

		static explicit IUtfCharacter<Utf16Char>.operator byte(Utf16Char value) => (byte)value._char;
	}

	internal readonly struct Utf8Char : IUtfCharacter<Utf8Char>
	{
		static Utf8Char IUtfCharacter<Utf8Char>.NullCharacter => new Utf8Char((byte)'\0');

		static Utf8Char IUtfCharacter<Utf8Char>.WhiteSpaceCharacter => new Utf8Char((byte)' ');

		private static ReadOnlySpan<byte> DigitsUtf8 =>
		[
			(byte)'0',
			(byte)'1',
			(byte)'2',
			(byte)'3',
			(byte)'4',
			(byte)'5',
			(byte)'6',
			(byte)'7',
			(byte)'8',
			(byte)'9',
			(byte)'A',
			(byte)'B',
			(byte)'C',
			(byte)'D',
			(byte)'E',
			(byte)'F',
		];

		static ReadOnlySpan<Utf8Char> IUtfCharacter<Utf8Char>.Digits => CastFromByteSpan("0123456789ABCDEF"u8);

		static ReadOnlySpan<byte> IUtfCharacter<Utf8Char>.TwoDigitsAsBytes =>
										"00010203040506070809"u8 +
										"10111213141516171819"u8 +
										"20212223242526272829"u8 +
										"30313233343536373839"u8 +
										"40414243444546474849"u8 +
										"50515253545556575859"u8 +
										"60616263646566676869"u8 +
										"70717273747576777879"u8 +
										"80818283848586878889"u8 +
										"90919293949596979899"u8;

		private readonly byte _char;

		private Utf8Char(byte @char)
		{
			_char = @char;
		}

		internal static ReadOnlySpan<Utf8Char> CastFromByteSpan(ReadOnlySpan<byte> chars)
		{
			return MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<byte, Utf8Char>(ref MemoryMarshal.GetReference(chars)), chars.Length);
		}

		static ReadOnlySpan<Utf8Char> IUtfCharacter<Utf8Char>.CastFromByteSpan(ReadOnlySpan<byte> chars)
		{
			return CastFromByteSpan(chars);
		}

		static ReadOnlySpan<Utf8Char> IUtfCharacter<Utf8Char>.CastFromCharSpan(ReadOnlySpan<char> chars)
		{
			throw new InvalidCastException();
		}

		internal static ReadOnlySpan<byte> CastToByteSpan(ReadOnlySpan<Utf8Char> chars)
		{
			return MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<Utf8Char, byte>(ref MemoryMarshal.GetReference(chars)), chars.Length);
		}

		static ReadOnlySpan<byte> IUtfCharacter<Utf8Char>.CastToByteSpan(ReadOnlySpan<Utf8Char> chars)
		{
			return CastToByteSpan(chars);
		}

		static ReadOnlySpan<char> IUtfCharacter<Utf8Char>.CastToCharSpan(ReadOnlySpan<Utf8Char> chars)
		{
			throw new InvalidCastException();
		}

		static bool IUtfCharacter<Utf8Char>.Constains(ReadOnlySpan<Utf8Char> v1, ReadOnlySpan<Utf8Char> v2, StringComparison comparisonType)
		{
			Span<char> left = stackalloc char[Encoding.UTF8.GetCharCount(CastToByteSpan(v1))];
			Utf8.ToUtf16(CastToByteSpan(v1), left, out _, out _);
			Span<char> right = stackalloc char[Encoding.UTF8.GetCharCount(CastToByteSpan(v2))];
			Utf8.ToUtf16(CastToByteSpan(v2), right, out _, out _);

			return ((ReadOnlySpan<char>)left).Contains(right, comparisonType);
		}

		static bool IUtfCharacter<Utf8Char>.EndsWith(ReadOnlySpan<Utf8Char> v1, ReadOnlySpan<Utf8Char> v2, StringComparison comparisonType)
		{
			Span<char> left = stackalloc char[Encoding.UTF8.GetCharCount(CastToByteSpan(v1))];
			Utf8.ToUtf16(CastToByteSpan(v1), left, out _, out _);
			Span<char> right = stackalloc char[Encoding.UTF8.GetCharCount(CastToByteSpan(v2))];
			Utf8.ToUtf16(CastToByteSpan(v2), right, out _, out _);

			return ((ReadOnlySpan<char>)left).EndsWith(right, comparisonType);
		}

		static bool IUtfCharacter<Utf8Char>.Equals(ReadOnlySpan<Utf8Char> v1, ReadOnlySpan<Utf8Char> v2, StringComparison comparisonType)
		{
			Span<char> left = stackalloc char[Encoding.UTF8.GetCharCount(CastToByteSpan(v1))];
			Utf8.ToUtf16(CastToByteSpan(v1), left, out _, out _);
			Span<char> right = stackalloc char[Encoding.UTF8.GetCharCount(CastToByteSpan(v2))];
			Utf8.ToUtf16(CastToByteSpan(v2), right, out _, out _);

			return ((ReadOnlySpan<char>)left).Equals(right, comparisonType);
		}

		public static ReadOnlySpan<Utf8Char> TrimStart(ReadOnlySpan<Utf8Char> s)
		{
			while (s.Length != 0)
			{
				_ = Rune.DecodeFromUtf8(CastToByteSpan(s), out Rune current, out int bytesConsumed);
				if (!Rune.IsWhiteSpace(current))
				{
					break;
				}
				s = s[bytesConsumed..];
			}
			return s;
		}

		public static ReadOnlySpan<Utf8Char> TrimEnd(ReadOnlySpan<Utf8Char> s)
		{
			while (s.Length != 0)
			{
				_ = Rune.DecodeLastFromUtf8(CastToByteSpan(s), out Rune current, out int bytesConsumed);
				if (!Rune.IsWhiteSpace(current))
				{
					break;
				}
				s = s[..^bytesConsumed];
			}
			return s;
		}

		static bool IUtfCharacter<Utf8Char>.IsDigit(Utf8Char value)
		{
			return char.IsDigit((char)value._char);
		}

		static bool IUtfCharacter<Utf8Char>.IsHexDigit(Utf8Char value)
		{
			return char.IsAsciiHexDigit((char)value._char);
		}

		static bool IUtfCharacter<Utf8Char>.IsWhiteSpace(Utf8Char value)
		{
			return char.IsWhiteSpace((char)value._char);
		}

		static bool IUtfCharacter<Utf8Char>.StartsWith(ReadOnlySpan<Utf8Char> v1, ReadOnlySpan<Utf8Char> v2, StringComparison comparisonType)
		{
			Span<char> left = stackalloc char[Encoding.UTF8.GetCharCount(CastToByteSpan(v1))];
			Utf8.ToUtf16(CastToByteSpan(v1), left, out _, out _);
			Span<char> right = stackalloc char[Encoding.UTF8.GetCharCount(CastToByteSpan(v2))];
			Utf8.ToUtf16(CastToByteSpan(v2), right, out _, out _);

			return ((ReadOnlySpan<char>)left).StartsWith(right, comparisonType);
		}

		static Utf8Char IUtfCharacter<Utf8Char>.ToLower(Utf8Char value)
		{
			return new Utf8Char((byte)char.ToLower((char)value._char));
		}

		static Utf8Char IUtfCharacter<Utf8Char>.ToUpper(Utf8Char value)
		{
			return new Utf8Char((byte)char.ToUpper((char)value._char));
		}

		bool IEquatable<Utf8Char>.Equals(Utf8Char other)
		{
			return _char.Equals(other._char);
		}

		static bool IUtfCharacter<Utf8Char>.TryParseInteger<T>(ReadOnlySpan<Utf8Char> s, NumberStyles style, IFormatProvider? provider, out T result)
		{
			return T.TryParse(CastToByteSpan(s), style, provider, out result);
		}

		static Span<char> IUtfCharacter<Utf8Char>.CastToCharSpan(Span<Utf8Char> chars)
		{
			throw new InvalidCastException();
		}

		internal static Span<byte> CastToByteSpan(Span<Utf8Char> chars)
		{
			return MemoryMarshal.CreateSpan(ref Unsafe.As<Utf8Char, byte>(ref MemoryMarshal.GetReference(chars)), chars.Length);
		}

		static Span<byte> IUtfCharacter<Utf8Char>.CastToByteSpan(Span<Utf8Char> chars)
		{
			return CastToByteSpan(chars);
		}

		static Span<Utf8Char> IUtfCharacter<Utf8Char>.CastFromCharSpan(Span<char> chars)
		{
			throw new InvalidCastException();
		}

		internal static Span<Utf8Char> CastFromByteSpan(Span<byte> chars)
		{
			return MemoryMarshal.CreateSpan(ref Unsafe.As<byte, Utf8Char>(ref MemoryMarshal.GetReference(chars)), chars.Length);
		}

		static Span<Utf8Char> IUtfCharacter<Utf8Char>.CastFromByteSpan(Span<byte> chars)
		{
			return CastFromByteSpan(chars);
		}

		static int IUtfCharacter<Utf8Char>.GetLength(ReadOnlySpan<char> s)
		{
			return Encoding.UTF8.GetByteCount(s);
		}

		static int IUtfCharacter<Utf8Char>.GetLength(ReadOnlySpan<byte> utf8Text)
		{
			return utf8Text.Length;
		}

		static void IUtfCharacter<Utf8Char>.Copy(ReadOnlySpan<char> origin, Span<Utf8Char> destination)
		{
			Encoding.UTF8.GetBytes(origin, CastToByteSpan(destination));
		}

		static void IUtfCharacter<Utf8Char>.Copy(ReadOnlySpan<byte> origin, Span<Utf8Char> destination)
		{
			CastFromByteSpan(origin).CopyTo(destination);
		}

		static bool IEqualityOperators<Utf8Char, Utf8Char, bool>.operator ==(Utf8Char left, Utf8Char right)
		{
			return left._char == right._char;
		}

		static bool IEqualityOperators<Utf8Char, Utf8Char, bool>.operator !=(Utf8Char left, Utf8Char right)
		{
			return left._char != right._char;
		}

		static explicit IUtfCharacter<Utf8Char>.operator Utf8Char(uint value) => new((byte)value);

		static explicit IUtfCharacter<Utf8Char>.operator Utf8Char(char value) => new((byte)value);

		static explicit IUtfCharacter<Utf8Char>.operator Utf8Char(byte value) => new(value);

		static explicit IUtfCharacter<Utf8Char>.operator uint(Utf8Char value) => value._char;

		static explicit IUtfCharacter<Utf8Char>.operator char(Utf8Char value) => (char)value._char;

		static explicit IUtfCharacter<Utf8Char>.operator byte(Utf8Char value) => value._char;
	}
}
