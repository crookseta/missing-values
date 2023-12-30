using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace MissingValues.Internals
{
	internal interface IUtfCharacter<TSelf> :
		IEquatable<TSelf>,
		IEqualityOperators<TSelf, TSelf, bool>
		where TSelf : unmanaged, IUtfCharacter<TSelf>?
	{
		abstract static TSelf NullCharacter { get; }
		abstract static TSelf WhiteSpaceCharacter { get; }
		abstract static ReadOnlySpan<TSelf> Digits { get; }

		abstract static ReadOnlySpan<char> CastToCharSpan(ReadOnlySpan<TSelf> chars);
		abstract static Span<char> CastToCharSpan(Span<TSelf> chars);
		abstract static ReadOnlySpan<byte> CastToByteSpan(ReadOnlySpan<TSelf> chars);
		abstract static Span<byte> CastToByteSpan(Span<TSelf> chars);

		abstract static ReadOnlySpan<TSelf> CastFromCharSpan(ReadOnlySpan<char> chars);
		abstract static Span<TSelf> CastFromCharSpan(Span<char> chars);
		abstract static ReadOnlySpan<TSelf> CastFromByteSpan(ReadOnlySpan<byte> chars);
		abstract static Span<TSelf> CastFromByteSpan(Span<byte> chars);

		abstract static void Copy(ReadOnlySpan<char> origin, Span<TSelf> destination);
		abstract static void Copy(ReadOnlySpan<byte> origin, Span<TSelf> destination);

		abstract static bool TryParseInteger<T>(ReadOnlySpan<TSelf> s, out T result) where T : struct, IBinaryInteger<T>;

		abstract static int GetLength(ReadOnlySpan<char> s);
		abstract static int GetLength(ReadOnlySpan<byte> utf8Text);

		abstract static TSelf ToUpper(TSelf value);
		abstract static TSelf ToLower(TSelf value);
		abstract static bool IsWhiteSpace(TSelf value);
		abstract static bool IsDigit(TSelf value);
		abstract static bool IsHexDigit(TSelf value);

		abstract static bool Constains(ReadOnlySpan<TSelf> v1, ReadOnlySpan<TSelf> v2, StringComparison comparisonType);
		abstract static bool EndsWith(ReadOnlySpan<TSelf> v1, ReadOnlySpan<TSelf> v2, StringComparison comparisonType);
		abstract static bool StartsWith(ReadOnlySpan<TSelf> v1, ReadOnlySpan<TSelf> v2, StringComparison comparisonType);
		abstract static bool Equals(ReadOnlySpan<TSelf> v1, ReadOnlySpan<TSelf> v2, StringComparison comparisonType);

		abstract static explicit operator TSelf(char value);  
		abstract static explicit operator TSelf(byte value);

		abstract static explicit operator char(TSelf value);
		abstract static explicit operator byte(TSelf value);
	}

	internal readonly struct Utf16Char : IUtfCharacter<Utf16Char>
	{
		private readonly char _char;

		public static Utf16Char NullCharacter => (Utf16Char)'\0';

		public static ReadOnlySpan<Utf16Char> Digits => new Utf16Char[] 
		{
			(Utf16Char)'0',
			(Utf16Char)'1',
			(Utf16Char)'2',
			(Utf16Char)'3',
			(Utf16Char)'4',
			(Utf16Char)'5',
			(Utf16Char)'6',
			(Utf16Char)'7',
			(Utf16Char)'8',
			(Utf16Char)'9',
			(Utf16Char)'A',
			(Utf16Char)'B',
			(Utf16Char)'C',
			(Utf16Char)'D',
			(Utf16Char)'E',
			(Utf16Char)'F',
		};

		public static Utf16Char WhiteSpaceCharacter => (Utf16Char)' ';

		private Utf16Char(char @char)
		{
			_char = @char;
		}

		static ReadOnlySpan<Utf16Char> IUtfCharacter<Utf16Char>.CastFromByteSpan(ReadOnlySpan<byte> chars)
		{
			throw new InvalidCastException();
		}

		public static ReadOnlySpan<Utf16Char> CastFromCharSpan(ReadOnlySpan<char> chars)
		{
			return MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<char, Utf16Char>(ref MemoryMarshal.GetReference(chars)), chars.Length);
		}

		static ReadOnlySpan<byte> IUtfCharacter<Utf16Char>.CastToByteSpan(ReadOnlySpan<Utf16Char> chars)
		{
			throw new InvalidCastException();
		}

		public static ReadOnlySpan<char> CastToCharSpan(ReadOnlySpan<Utf16Char> chars)
		{
			return MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<Utf16Char, char>(ref MemoryMarshal.GetReference(chars)), chars.Length);
		}

		public static bool Constains(ReadOnlySpan<Utf16Char> v1, ReadOnlySpan<Utf16Char> v2, StringComparison comparisonType)
		{
			return CastToCharSpan(v1).Contains(CastToCharSpan(v2), comparisonType);
		}

		public static bool EndsWith(ReadOnlySpan<Utf16Char> v1, ReadOnlySpan<Utf16Char> v2, StringComparison comparisonType)
		{
			return CastToCharSpan(v1).EndsWith(CastToCharSpan(v2), comparisonType);
		}

		public static bool Equals(ReadOnlySpan<Utf16Char> v1, ReadOnlySpan<Utf16Char> v2, StringComparison comparisonType)
		{
			return CastToCharSpan(v1).Equals(CastToCharSpan(v2), comparisonType);
		}

		public static bool IsWhiteSpace(Utf16Char value)
		{
			return char.IsWhiteSpace(value._char);
		}

		public static bool StartsWith(ReadOnlySpan<Utf16Char> v1, ReadOnlySpan<Utf16Char> v2, StringComparison comparisonType)
		{
			return CastToCharSpan(v1).StartsWith(CastToCharSpan(v2), comparisonType);
		}

		public static Utf16Char ToLower(Utf16Char value)
		{
			return new(char.ToLower(value._char));
		}

		public static Utf16Char ToUpper(Utf16Char value)
		{
			return new(char.ToUpper(value._char));
		}

		public bool Equals(Utf16Char other)
		{
			return _char.Equals(other._char);
		}

		public static bool IsDigit(Utf16Char value)
		{
			return char.IsDigit(value._char);
		}

		public static bool IsHexDigit(Utf16Char value)
		{
			return char.IsAsciiHexDigit(value._char);
		}

		public static bool TryParseInteger<T>(ReadOnlySpan<Utf16Char> s, out T result) where T : struct, IBinaryInteger<T>
		{
			return T.TryParse(CastToCharSpan(s), NumberStyles.Integer, CultureInfo.CurrentCulture, out result);
		}

		public static Span<char> CastToCharSpan(Span<Utf16Char> chars)
		{
			return MemoryMarshal.CreateSpan(ref Unsafe.As<Utf16Char, char>(ref MemoryMarshal.GetReference(chars)), chars.Length);
		}

		public static Span<byte> CastToByteSpan(Span<Utf16Char> chars)
		{
			throw new NotImplementedException();
		}

		public static Span<Utf16Char> CastFromCharSpan(Span<char> chars)
		{
			return MemoryMarshal.CreateSpan(ref Unsafe.As<char, Utf16Char>(ref MemoryMarshal.GetReference(chars)), chars.Length);
		}

		public static Span<Utf16Char> CastFromByteSpan(Span<byte> chars)
		{
			throw new NotImplementedException();
		}

		public static int GetLength(ReadOnlySpan<char> s)
		{
			return s.Length;
		}

		public static int GetLength(ReadOnlySpan<byte> utf8Text)
		{
			return Encoding.UTF8.GetCharCount(utf8Text);
		}

		public static void Copy(ReadOnlySpan<char> origin, Span<Utf16Char> destination)
		{
			CastFromCharSpan(origin).CopyTo(destination);
		}

		public static void Copy(ReadOnlySpan<byte> origin, Span<Utf16Char> destination)
		{
			Encoding.UTF8.GetChars(origin, CastToCharSpan(destination));
		}

		public override string? ToString()
		{
			return _char.ToString();
		}

		public static bool operator ==(Utf16Char left, Utf16Char right)
		{
			return left._char == right._char;
		}

		public static bool operator !=(Utf16Char left, Utf16Char right)
		{
			return left._char != right._char;
		}

		public static explicit operator Utf16Char(char value)
		{
			return new(value);
		}

		public static explicit operator Utf16Char(byte value)
		{
			return new((char)value);
		}

		public static explicit operator char(Utf16Char value)
		{
			return value._char;
		}

		public static explicit operator byte(Utf16Char value)
		{
			return (byte)value._char;
		}
	}

#if NET8_0_OR_GREATER
	internal readonly struct Utf8Char : IUtfCharacter<Utf8Char>
	{
		public static Utf8Char NullCharacter => (Utf8Char)(byte)'\0';

		public static Utf8Char WhiteSpaceCharacter => (Utf8Char)(byte)' ';

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
		public static ReadOnlySpan<Utf8Char> Digits => CastFromByteSpan(DigitsUtf8);
		
		private readonly byte _char;

		private Utf8Char(byte @char)
		{
			_char = @char;
		}

		public static ReadOnlySpan<Utf8Char> CastFromByteSpan(ReadOnlySpan<byte> chars)
		{
			return MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<byte, Utf8Char>(ref MemoryMarshal.GetReference(chars)), chars.Length);
		}

		static ReadOnlySpan<Utf8Char> IUtfCharacter<Utf8Char>.CastFromCharSpan(ReadOnlySpan<char> chars)
		{
			throw new InvalidCastException();
		}

		public static ReadOnlySpan<byte> CastToByteSpan(ReadOnlySpan<Utf8Char> chars)
		{
			return MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<Utf8Char, byte>(ref MemoryMarshal.GetReference(chars)), chars.Length);
		}

		static ReadOnlySpan<char> IUtfCharacter<Utf8Char>.CastToCharSpan(ReadOnlySpan<Utf8Char> chars)
		{
			throw new InvalidCastException();
		}

		public static bool Constains(ReadOnlySpan<Utf8Char> v1, ReadOnlySpan<Utf8Char> v2, StringComparison comparisonType)
		{
			Span<char> left = stackalloc char[Encoding.UTF8.GetCharCount(CastToByteSpan(v1))];
			Utf8.ToUtf16(CastToByteSpan(v1), left, out _, out _);
			Span<char> right = stackalloc char[Encoding.UTF8.GetCharCount(CastToByteSpan(v2))];
			Utf8.ToUtf16(CastToByteSpan(v2), right, out _, out _);

			return ((ReadOnlySpan<char>)left).Contains(right, comparisonType);
		}

		public static bool EndsWith(ReadOnlySpan<Utf8Char> v1, ReadOnlySpan<Utf8Char> v2, StringComparison comparisonType)
		{
			Span<char> left = stackalloc char[Encoding.UTF8.GetCharCount(CastToByteSpan(v1))];
			Utf8.ToUtf16(CastToByteSpan(v1), left, out _, out _);
			Span<char> right = stackalloc char[Encoding.UTF8.GetCharCount(CastToByteSpan(v2))];
			Utf8.ToUtf16(CastToByteSpan(v2), right, out _, out _);

			return ((ReadOnlySpan<char>)left).EndsWith(right, comparisonType);
		}

		public static bool Equals(ReadOnlySpan<Utf8Char> v1, ReadOnlySpan<Utf8Char> v2, StringComparison comparisonType)
		{
			Span<char> left = stackalloc char[Encoding.UTF8.GetCharCount(CastToByteSpan(v1))];
			Utf8.ToUtf16(CastToByteSpan(v1), left, out _, out _);
			Span<char> right = stackalloc char[Encoding.UTF8.GetCharCount(CastToByteSpan(v2))];
			Utf8.ToUtf16(CastToByteSpan(v2), right, out _, out _);

			return ((ReadOnlySpan<char>)left).Equals(right, comparisonType);
		}

		public static bool IsDigit(Utf8Char value)
		{
			return char.IsDigit((char)value);
		}

		public static bool IsHexDigit(Utf8Char value)
		{
			return char.IsAsciiHexDigit((char)value);
		}

		public static bool IsWhiteSpace(Utf8Char value)
		{
			return char.IsWhiteSpace((char)value);
		}

		public static bool StartsWith(ReadOnlySpan<Utf8Char> v1, ReadOnlySpan<Utf8Char> v2, StringComparison comparisonType)
		{
			Span<char> left = stackalloc char[Encoding.UTF8.GetCharCount(CastToByteSpan(v1))];
			Utf8.ToUtf16(CastToByteSpan(v1), left, out _, out _);
			Span<char> right = stackalloc char[Encoding.UTF8.GetCharCount(CastToByteSpan(v2))];
			Utf8.ToUtf16(CastToByteSpan(v2), right, out _, out _);

			return ((ReadOnlySpan<char>)left).StartsWith(right, comparisonType);
		}

		public static Utf8Char ToLower(Utf8Char value)
		{
			return (Utf8Char)char.ToLower((char)value);
		}

		public static Utf8Char ToUpper(Utf8Char value)
		{
			return (Utf8Char)char.ToUpper((char)value);
		}

		public bool Equals(Utf8Char other)
		{
			return _char.Equals(other._char);
		}

		public static bool TryParseInteger<T>(ReadOnlySpan<Utf8Char> s, out T result) where T : struct, IBinaryInteger<T>
		{
			return T.TryParse(CastToByteSpan(s), NumberStyles.Integer, CultureInfo.CurrentCulture, out result);
		}

		static Span<char> IUtfCharacter<Utf8Char>.CastToCharSpan(Span<Utf8Char> chars)
		{
			throw new InvalidCastException();
		}

		public static Span<byte> CastToByteSpan(Span<Utf8Char> chars)
		{
			return MemoryMarshal.CreateSpan(ref Unsafe.As<Utf8Char, byte>(ref MemoryMarshal.GetReference(chars)), chars.Length);
		}

		static Span<Utf8Char> IUtfCharacter<Utf8Char>.CastFromCharSpan(Span<char> chars)
		{
			throw new InvalidCastException();
		}

		public static Span<Utf8Char> CastFromByteSpan(Span<byte> chars)
		{
			return MemoryMarshal.CreateSpan(ref Unsafe.As<byte, Utf8Char>(ref MemoryMarshal.GetReference(chars)), chars.Length);
		}

		public static int GetLength(ReadOnlySpan<char> s)
		{
			return Encoding.UTF8.GetByteCount(s);
		}

		public static int GetLength(ReadOnlySpan<byte> utf8Text)
		{
			return utf8Text.Length;
		}

		public static void Copy(ReadOnlySpan<char> origin, Span<Utf8Char> destination)
		{
			Encoding.UTF8.GetBytes(origin, CastToByteSpan(destination));
		}

		public static void Copy(ReadOnlySpan<byte> origin, Span<Utf8Char> destination)
		{
			CastFromByteSpan(origin).CopyTo(destination);
		}

		public override string? ToString()
		{
			return ((char)_char).ToString();
		}

		public static bool operator ==(Utf8Char left, Utf8Char right)
		{
			return left._char == right._char;
		}

		public static bool operator !=(Utf8Char left, Utf8Char right)
		{
			return left._char != right._char;
		}

		public static explicit operator Utf8Char(char value)
		{
			return new((byte)value);
		}

		public static explicit operator Utf8Char(byte value)
		{
			return new(value);
		}

		public static explicit operator char(Utf8Char value)
		{
			return (char)value._char;
		}

		public static explicit operator byte(Utf8Char value)
		{
			return value._char;
		}
	}
#endif
}
