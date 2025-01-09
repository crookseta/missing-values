using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace MissingValues.Internals;

/*
 * This is modified from the internal BigInteger implementation in the .Net core library
 * https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/Number.BigInteger.cs
 */
[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal unsafe ref partial struct BigNumber
{
	// The longest binary mantissa requires: explicit mantissa bits + abs(min exponent)
	// * Half:     10 +    14 =    24
	// * Single:   23 +   126 =   149
	// * Double:   52 +  1022 =  1074
	// * Quad:    112 + 16382 = 16494
	// * Octo:    236 + 262142 = 262378
	private const int BitsForLongestBinaryMantissa = 262378;

	// The longest digit sequence requires: ceil(log2(pow(10, max significant digits + 1 rounding digit)))
	// * Half:    ceil(log2(pow(10,    21 + 1))) =    74
	// * Single:  ceil(log2(pow(10,   112 + 1))) =   376
	// * Double:  ceil(log2(pow(10,   767 + 1))) =  2552
	// * Quad:    ceil(log2(pow(10, 11563 + 1))) = 38415
	// * Octo:    ceil(log2(pow(10, 183466 + 1))) = 609465
	private const int BitsForLongestDigitSequence = 609465;

	// We require BitsPerBlock additional bits for shift space used during the pre-division preparation
	private const int MaxBits = BitsForLongestBinaryMantissa + BitsForLongestDigitSequence + BitsPerBlock;

	private const int BitsPerBlock = sizeof(long) * 8;
	private const int MaxBlockCount = (MaxBits + (BitsPerBlock - 1)) / BitsPerBlock; // 13624
	private const int MaxUInt64Pow10 = 19;
	private long _length;
	private fixed ulong _blocks[MaxBlockCount];

	public void Add(ulong value)
	{
		int length = (int)_length;
		if (length == 0)
		{
			SetUInt64(out this, value);
			return;
		}

		_blocks[0] += value;
		if (_blocks[0] >= value)
		{
			// No carry
			return;
		}

		for (int index = 1; index < length; index++)
		{
			_blocks[index]++;
			if (_blocks[index] > 0)
			{
				// No carry
				return;
			}
		}

		Debug.Assert(unchecked((uint)(length)) + 1 <= MaxBlockCount);
		_blocks[length] = 1;
		_length = length + 1;
	}

	public static void Add(scoped ref BigNumber lhs, scoped ref BigNumber rhs, out BigNumber result)
	{
		// determine which operand has the smaller length
		ref BigNumber large = ref (lhs._length < rhs._length) ? ref rhs : ref lhs;
		ref BigNumber small = ref (lhs._length < rhs._length) ? ref lhs : ref rhs;

		int largeLength = (int)large._length;
		int smallLength = (int)small._length;

		// The output will be at least as long as the largest input
		result._length = largeLength;

		// Add each block and add carry the overflow to the next block
		UInt128 carry = 0;

		int largeIndex = 0;
		int smallIndex = 0;
		int resultIndex = 0;

		while (smallIndex < smallLength)
		{
			UInt128 sum = carry + large._blocks[largeIndex] + small._blocks[smallIndex];
			carry = sum >> 64;
			result._blocks[resultIndex] = (ulong)(sum);

			largeIndex++;
			smallIndex++;
			resultIndex++;
		}

		// Add the carry to any blocks that only exist in the large operand
		while (largeIndex < largeLength)
		{
			UInt128 sum = carry + large._blocks[largeIndex];
			carry = sum >> 64;
			result._blocks[resultIndex] = (ulong)(sum);

			largeIndex++;
			resultIndex++;
		}

		// If there's still a carry, append a new block
		if (carry != 0)
		{
			Debug.Assert(carry == 1);
			Debug.Assert((resultIndex == largeLength) && (largeLength < MaxBlockCount));

			result._blocks[resultIndex] = 1;
			result._length++;
		}
	}

	public static int Compare(scoped ref BigNumber lhs, scoped ref BigNumber rhs)
	{
		Debug.Assert(unchecked((uint)(lhs._length)) <= MaxBlockCount);
		Debug.Assert(unchecked((uint)(rhs._length)) <= MaxBlockCount);

		int lhsLength = (int)lhs._length;
		int rhsLength = (int)rhs._length;

		int lengthDelta = (lhsLength - rhsLength);

		if (lengthDelta != 0)
		{
			return lengthDelta;
		}

		if (lhsLength == 0)
		{
			Debug.Assert(rhsLength == 0);
			return 0;
		}

		for (int index = (lhsLength - 1); index >= 0; index--)
		{
			Int128 delta = (Int128)(lhs._blocks[index]) - rhs._blocks[index];

			if (delta != 0)
			{
				return delta > 0 ? 1 : -1;
			}
		}

		return 0;
	}

	public static uint CountSignificantBits(uint value)
	{
		return 32 - (uint)BitOperations.LeadingZeroCount(value);
	}

	public static uint CountSignificantBits(ulong value)
	{
		return 64 - (uint)BitOperations.LeadingZeroCount(value);
	}

	public static uint CountSignificantBits(UInt128 value)
	{
		return 128 - (uint)UInt128.LeadingZeroCount(value);
	}

	public static uint CountSignificantBits(UInt256 value)
	{
		return 256 - (uint)BitHelper.LeadingZeroCount(in value);
	}

	public static uint CountSignificantBits<T>(T value)
		where T : unmanaged, IBinaryInteger<T>, IUnsignedNumber<T>
	{
		return (uint)(sizeof(T) * 8) - uint.CreateChecked(T.LeadingZeroCount(value));
	}

	public static uint CountSignificantBits(ref BigNumber value)
	{
		if (value.IsZero())
		{
			return 0;
		}

		// We don't track any unused blocks, so we only need to do a BSR on the
		// last index and add that to the number of bits we skipped.

		uint lastIndex = (uint)(value._length - 1);
		return (lastIndex * BitsPerBlock) + CountSignificantBits(value._blocks[lastIndex]);
	}

	public static void DivRem(scoped ref BigNumber lhs, scoped ref BigNumber rhs, out BigNumber quo, out BigNumber rem)
	{
		// This is modified from the libraries BigIntegerCalculator.DivRem.cs implementation:
		// https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Numerics/src/System/Numerics/BigIntegerCalculator.DivRem.cs

		Debug.Assert(!rhs.IsZero());

		if (lhs.IsZero())
		{
			SetZero(out quo);
			SetZero(out rem);
			return;
		}

		int lhsLength = (int)lhs._length;
		int rhsLength = (int)rhs._length;

		if ((lhsLength == 1) && (rhsLength == 1))
		{
			(ulong quotient, ulong remainder) = Math.DivRem(lhs._blocks[0], rhs._blocks[0]);
			SetUInt64(out quo, quotient);
			SetUInt64(out rem, remainder);
			return;
		}

		if (rhsLength == 1)
		{
			// We can make the computation much simpler if the rhs is only one block

			int quoLength = lhsLength;

			UInt128 rhsValue = rhs._blocks[0];
			UInt128 carry = 0;

			for (int i = quoLength - 1; i >= 0; i--)
			{
				UInt128 value = new UInt128((ulong)carry, lhs._blocks[i]);
				UInt128 digit;
				(digit, carry) = UInt128.DivRem(value, rhsValue);

				if ((digit == 0) && (i == (quoLength - 1)))
				{
					quoLength--;
				}
				else
				{
					quo._blocks[i] = (ulong)(digit);
				}
			}

			quo._length = quoLength;
			SetUInt64(out rem, (ulong)(carry));

			return;
		}
		else if (rhsLength > lhsLength)
		{
			// Handle the case where we have no quotient
			SetZero(out quo);
			SetValue(out rem, ref lhs);
			return;
		}
		else
		{
			int quoLength = lhsLength - rhsLength + 1;
			SetValue(out rem, ref lhs);
			int remLength = lhsLength;

			// Executes the "grammar-school" algorithm for computing q = a / b.
			// Before calculating q_i, we get more bits into the highest bit
			// block of the divisor. Thus, guessing digits of the quotient
			// will be more precise. Additionally we'll get r = a % b.

			ulong divHi = rhs._blocks[rhsLength - 1];
			ulong divLo = rhs._blocks[rhsLength - 2];

			// We measure the leading zeros of the divisor
			int shiftLeft = BitOperations.LeadingZeroCount(divHi);
			int shiftRight = 64 - shiftLeft;

			// And, we make sure the most significant bit is set
			if (shiftLeft > 0)
			{
				divHi = (divHi << shiftLeft) | (divLo >> shiftRight);
				divLo <<= shiftLeft;

				if (rhsLength > 2)
				{
					divLo |= (rhs._blocks[rhsLength - 3] >> shiftRight);
				}
			}

			// Then, we divide all of the bits as we would do it using
			// pen and paper: guessing the next digit, subtracting, ...
			for (int i = lhsLength; i >= rhsLength; i--)
			{
				int n = i - rhsLength;
				ulong t = i < lhsLength ? rem._blocks[i] : 0;

				UInt128 valHi = new UInt128(t, rem._blocks[i - 1]);

				ulong valLo = i > 1 ? rem._blocks[i - 2] : 0;

				// We shifted the divisor, we shift the dividend too
				if (shiftLeft > 0)
				{
					valHi = (valHi << shiftLeft) | (valLo >> shiftRight);
					valLo <<= shiftLeft;

					if (i > 2)
					{
						valLo |= (rem._blocks[i - 3] >> shiftRight);
					}
				}

				// First guess for the current digit of the quotient,
				// which naturally must have only 64 bits...
				UInt128 digit = valHi / divHi;

				if (digit > ulong.MaxValue)
				{
					digit = ulong.MaxValue;
				}

				// Our first guess may be a little bit to big
				while (DivideGuessTooBig(digit, valHi, valLo, divHi, divLo))
				{
					digit--;
				}

				if (digit > 0)
				{
					// rem and rhs have different lifetimes here and compiler is warning
					// about potential for one to copy into the other. This is a place
					// ref scoped parameters would alleviate.
					// https://github.com/dotnet/roslyn/issues/64393

					// Now it's time to subtract our current quotient
					ulong carry = SubtractDivisor(ref rem, n, ref rhs, digit);

					if (carry != t)
					{
						Debug.Assert(carry == t + 1);

						// Our guess was still exactly one too high
						carry = AddDivisor(ref rem, n, ref rhs);
						digit--;

						Debug.Assert(carry == 1);
					}

				}

				// We have the digit!
				if (quoLength != 0)
				{
					if ((digit == 0) && (n == (quoLength - 1)))
					{
						quoLength--;
					}
					else
					{
						quo._blocks[n] = (ulong)(digit);
					}
				}

				if (i < remLength)
				{
					remLength--;
				}
			}

			quo._length = quoLength;

			// We need to check for the case where remainder is zero

			for (int i = remLength - 1; i >= 0; i--)
			{
				if (rem._blocks[i] == 0)
				{
					remLength--;
				}
				else
				{
					// As soon as we find a non-zero block, the rest of remainder is significant
					break;
				}
			}

			rem._length = remLength;
		}
	}
	public static ulong HeuristicDivide(ref BigNumber dividend, ref BigNumber divisor)
	{
		int divisorLength = (int)divisor._length;

		if (dividend._length < divisorLength)
		{
			return 0;
		}

		// This is an estimated quotient. Its error should be less than 2.
		// Reference inequality:
		// a/b - floor(floor(a)/(floor(b) + 1)) < 2
		int lastIndex = (divisorLength - 1);
		ulong quotient = dividend._blocks[lastIndex] / (divisor._blocks[lastIndex] + 1);

		if (quotient != 0)
		{
			// Now we use our estimated quotient to update each block of dividend.
			// dividend = dividend - divisor * quotient
			int index = 0;

			UInt128 borrow = 0;
			UInt128 carry = 0;

			do
			{
				UInt128 product = ((UInt128)(divisor._blocks[index]) * quotient) + carry;
				carry = product >> 64;

				UInt128 difference = (UInt128)(dividend._blocks[index]) - (uint)(product) - borrow;
				borrow = (difference >> 64) & 1;

				dividend._blocks[index] = (uint)(difference);

				index++;
			}
			while (index < divisorLength);

			// Remove all leading zero blocks from dividend
			while ((divisorLength > 0) && (dividend._blocks[divisorLength - 1] == 0))
			{
				divisorLength--;
			}

			dividend._length = divisorLength;
		}

		// If the dividend is still larger than the divisor, we overshot our estimate quotient. To correct,
		// we increment the quotient and subtract one more divisor from the dividend (Because we guaranteed the error range).
		if (Compare(ref dividend, ref divisor) >= 0)
		{
			quotient++;

			// dividend = dividend - divisor
			int index = 0;
			UInt128 borrow = 0;

			do
			{
				UInt128 difference = (UInt128)(dividend._blocks[index]) - divisor._blocks[index] - borrow;
				borrow = (difference >> 64) & 1;

				dividend._blocks[index] = (uint)(difference);

				index++;
			}
			while (index < divisorLength);

			// Remove all leading zero blocks from dividend
			while ((divisorLength > 0) && (dividend._blocks[divisorLength - 1] == 0))
			{
				divisorLength--;
			}

			dividend._length = divisorLength;
		}

		return quotient;
	}

	public static void Multiply(scoped ref BigNumber lhs, ulong value, out BigNumber result)
	{
		if (lhs._length <= 1)
		{
			SetUInt128(out result, (UInt128)lhs.ToUInt64() * value);
			return;
		}

		if (value <= 1)
		{
			if (value == 0)
			{
				SetZero(out result);
			}
			else
			{
				SetValue(out result, ref lhs);
			}
			return;
		}

		int lhsLength = (int)lhs._length;
		int index = 0;
		ulong carry = 0;

		while (index < lhsLength)
		{
			UInt128 product = ((UInt128)(lhs._blocks[index]) * value) + carry;
			result._blocks[index] = (ulong)(product);
			carry = (ulong)(product >> 64);

			index++;
		}

		if (carry != 0)
		{
			Debug.Assert(unchecked((uint)(lhsLength)) + 1 <= MaxBlockCount);
			result._blocks[index] = carry;
			result._length = (lhsLength + 1);
		}
		else
		{
			result._length = lhsLength;
		}
	}

	public static void Multiply(scoped ref BigNumber lhs, scoped ref BigNumber rhs, out BigNumber result)
	{
		if (lhs._length <= 1)
		{
			Multiply(ref rhs, lhs.ToUInt64(), out result);
			return;
		}

		if (rhs._length <= 1)
		{
			Multiply(ref lhs, rhs.ToUInt64(), out result);
			return;
		}

		ref readonly BigNumber large = ref lhs;
		int largeLength = (int)lhs._length;

		ref readonly BigNumber small = ref rhs;
		int smallLength = (int)rhs._length;

		if (largeLength < smallLength)
		{
			large = ref rhs;
			largeLength = (int)rhs._length;

			small = ref lhs;
			smallLength = (int)lhs._length;
		}

		int maxResultLength = smallLength + largeLength;
		Debug.Assert(unchecked((uint)(maxResultLength)) <= MaxBlockCount);

		// Zero out result internal blocks.
		result._length = maxResultLength;
		result.Clear((uint)maxResultLength);

		int smallIndex = 0;
		int resultStartIndex = 0;

		while (smallIndex < smallLength)
		{
			// Multiply each block of large BigNum.
			if (small._blocks[smallIndex] != 0)
			{
				int largeIndex = 0;
				int resultIndex = resultStartIndex;

				UInt128 carry = 0;

				do
				{
					UInt128 product = result._blocks[resultIndex] + ((UInt128)(small._blocks[smallIndex]) * large._blocks[largeIndex]) + carry;
					carry = product >> 64;
					result._blocks[resultIndex] = (ulong)(product);

					resultIndex++;
					largeIndex++;
				}
				while (largeIndex < largeLength);

				result._blocks[resultIndex] = (ulong)(carry);
			}

			smallIndex++;
			resultStartIndex++;
		}

		if ((maxResultLength > 0) && (result._blocks[maxResultLength - 1] == 0))
		{
			result._length--;
		}
	}

	public static void Pow2(uint exponent, out BigNumber result)
	{
		uint blocksToShift = DivRem64(exponent, out uint remainingBitsToShift);
		result._length = (int)blocksToShift + 1;
		Debug.Assert(unchecked((uint)result._length) <= MaxBlockCount);
		if (blocksToShift > 0)
		{
			result.Clear(blocksToShift);
		}
		result._blocks[blocksToShift] = 1UL << (int)(remainingBitsToShift);
	}

	public static void Pow10(uint exponent, out BigNumber result)
	{
		// We leverage two arrays - Pow10UInt64Table and Pow10BigNumTable to speed up the Pow10 calculation.
		//
		// Pow10UInt64Table stores the results of 10^0 to 10^15.
		// Pow10BigNumTable stores the results of 10^16, 10^32, 10^64, 10^128, 10^256, 10^512, 10^1024, 10^2048, 10^4096, 10^8192, and 10^16384
		//
		// For example, let's say exp = 0b111111. We can split the exp to two parts, one is small exp,
		// which 10^smallExp can be represented as ulong, another part is 10^bigExp, which must be represented as BigNum.
		// So the result should be 10^smallExp * 10^bigExp.
		//
		// Calculating 10^smallExp is simple, we just lookup the 10^smallExp from Pow10UInt32Table.
		// But here's a bad news: although ulong can represent 10^19, exp 19's binary representation is 10011.
		// That means 10^(10100) all the way to 10^(11111) cannot be stored as ulong, we cannot easily say something like:
		// "Any bits <= 4 is small exp, any bits > 4 is big exp". So instead of involving 10^16, 10^17, 10^18, 10^19 to Pow10UInt64Table,
		// consider them as a bigNum, so they fall into Pow10BigNumTable. Now we can have a simple rule:
		// "Any bits <= 4 is small exp, any bits > 4 is big exp".
		//
		// For 0b111111, we first calculate 10^(smallExp), which is 10^(15), now we can shift right 4 bits, prepare to calculate the bigExp part,
		// the exp now becomes 0b000011.
		//
		// Apparently the lowest bit of bigExp should represent 10^16 because we have already shifted 4 bits for smallExp, so Pow10BigNumTable[0] = 10^16.
		// Now let's shift exp right 1 bit, the lowest bit should represent 10^(16 * 2) = 10^32, and so on...
		//
		// That's why we just need the values of Pow10BigNumTable be power of 2.
		//
		// More details of this implementation can be found at: https://github.com/dotnet/coreclr/pull/12894#discussion_r128890596

		// Validate that `Pow10BigNumTable` has exactly enough trailing elements to fill a BigInteger (which contains MaxBlockCount + 1 elements)
		// We validate here, since this is the only current consumer of the array
		Debug.Assert((Pow10BigNumTableIndices[^1] + MaxBlockCount + 2) == Pow10BigNumTable.Length);

		SetUInt64(out BigNumber temp1, Pow10UInt64Table[(int)(exponent & 0xF)]);
		ref BigNumber lhs = ref temp1;

		SetZero(out BigNumber temp2);
		ref BigNumber product = ref temp2;

		exponent >>= 4;
		uint index = 0;

		while (exponent != 0)
		{
			// If the current bit is set, multiply it with the corresponding power of 10
			if ((exponent & 1) != 0)
			{
				// Multiply into the next temporary
				fixed (ulong* pBigNumEntry = &Pow10BigNumTable[Pow10BigNumTableIndices[(int)index]])
				{
					ref BigNumber rhs = ref *(BigNumber*)(pBigNumEntry);
					Multiply(ref lhs, ref rhs, out product);
				}

				// Swap to the next temporary
				ref BigNumber temp = ref product;
				product = ref lhs;
				lhs = ref temp;
			}

			// Advance to the next bit
			++index;
			exponent >>= 1;
		}

		SetValue(out result, ref lhs);
	}

	private static ulong AddDivisor(ref BigNumber lhs, int lhsStartIndex, ref BigNumber rhs)
	{
		int lhsLength = (int)lhs._length;
		int rhsLength = (int)rhs._length;

		Debug.Assert(lhsLength >= 0);
		Debug.Assert(rhsLength >= 0);
		Debug.Assert(lhsLength >= rhsLength);

		// Repairs the dividend, if the last subtract was too much

		UInt128 carry = 0UL;

		for (int i = 0; i < rhsLength; i++)
		{
			ref ulong lhsValue = ref lhs._blocks[lhsStartIndex + i];

			UInt128 digit = lhsValue + carry + rhs._blocks[i];
			lhsValue = unchecked((ulong)digit);
			carry = digit >> 64;
		}

		return (ulong)(carry);
	}

	private static bool DivideGuessTooBig(UInt128 q, UInt128 valHi, ulong valLo, ulong divHi, ulong divLo)
	{
		Debug.Assert(q <= 0xFFFF_FFFF_FFFF_FFFF);

		// We multiply the two most significant limbs of the divisor
		// with the current guess for the quotient. If those are bigger
		// than the three most significant limbs of the current dividend
		// we return true, which means the current guess is still too big.

		UInt128 chkHi = divHi * q;
		UInt128 chkLo = divLo * q;

		chkHi += (chkLo >> 64);
		chkLo &= ulong.MaxValue;

		if (chkHi < valHi)
			return false;

		if (chkHi > valHi)
			return true;

		if (chkLo < valLo)
			return false;

		if (chkLo > valLo)
			return true;

		return false;
	}

	private static ulong SubtractDivisor(ref BigNumber lhs, int lhsStartIndex, ref BigNumber rhs, UInt128 q)
	{
		int lhsLength = (int)lhs._length - lhsStartIndex;
		int rhsLength = (int)rhs._length;

		Debug.Assert(lhsLength >= 0);
		Debug.Assert(rhsLength >= 0);
		Debug.Assert(lhsLength >= rhsLength);
		Debug.Assert(q <= ulong.MaxValue);

		// Combines a subtract and a multiply operation, which is naturally
		// more efficient than multiplying and then subtracting...

		UInt128 carry = 0;

		for (int i = 0; i < rhsLength; i++)
		{
			carry += rhs._blocks[i] * q;
			ulong digit = unchecked((ulong)carry);
			carry >>= 64;

			ref ulong lhsValue = ref lhs._blocks[lhsStartIndex + i];

			if (lhsValue < digit)
			{
				carry++;
			}

			lhsValue = unchecked(lhsValue - digit);
		}

		return (ulong)(carry);
	}

	public ulong GetBlock(uint index)
	{
		Debug.Assert(index < _length);
		return _blocks[index];
	}

	public readonly int GetLength()
	{
		return (int)_length;
	}

	public readonly bool IsZero()
	{
		return _length == 0;
	}

	public void Multiply(ulong value)
	{
		Multiply(ref this, value, out this);
	}

	public void Multiply(scoped ref BigNumber value)
	{
		if (value._length <= 1)
		{
			Multiply(ref this, value.ToUInt32(), out this);
		}
		else
		{
			SetValue(out BigNumber temp, ref this);
			Multiply(ref temp, ref value, out this);
		}
	}

	public void Multiply10()
	{
		if (IsZero())
		{
			return;
		}

		int index = 0;
		int length = (int)_length;
		UInt128 carry = UInt128.Zero;

		do
		{
			UInt128 block = (UInt128)(_blocks[index]);
			UInt128 product = (block << 3) + (block << 1) + carry;
			carry = product >> 64;
			_blocks[index] = (ulong)(product);

			index++;
		} while (index < length);

		if (carry != UInt128.Zero)
		{
			Debug.Assert(unchecked((uint)(_length)) + 1 <= MaxBlockCount);
			_blocks[index] = (ulong)carry;
			_length++;
		}
	}

	public void MultiplyPow10(uint exponent)
	{
		if (exponent <= MaxUInt64Pow10)
		{
			Multiply(Pow10UInt64Table[(int)exponent]);
		}
		else if (!IsZero())
		{
			Pow10(exponent, out BigNumber poweredValue);
			Multiply(ref poweredValue);
		}
	}

	public static void SetUInt32(out BigNumber result, uint value)
	{
		if (value == 0)
		{
			SetZero(out result);
		}
		else
		{
			result._blocks[0] = value;
			result._length = 1;
		}
	}

	public static void SetUInt64(out BigNumber result, ulong value)
	{
		if (value == 0)
		{
			SetZero(out result);
		}
		else
		{
			result._blocks[0] = value;
			result._length = 1;
		}
	}

	public static void SetUInt128(out BigNumber result, UInt128 value)
	{
		if (value <= ulong.MaxValue)
		{
			SetUInt64(out result, (ulong)(value));
		}
		else
		{
			result._blocks[0] = (ulong)(value);
			result._blocks[1] = (ulong)(value >> 64);

			result._length = 2;
		}
	}

	public static void SetUInt256(out BigNumber result, UInt256 value)
	{
		if (value <= ulong.MaxValue)
		{
			SetUInt64(out result, (ulong)(value));
		}
		else if (value <= UInt128.MaxValue)
		{
			SetUInt128(out result, (UInt128)(value));
		}
		else
		{
			result._blocks[0] = value.Part0;
			result._blocks[1] = value.Part1;
			result._blocks[2] = value.Part2;
			result._blocks[3] = value.Part3;

			result._length = 4;
		}
	}

	public static void SetValue<T>(out BigNumber result, T value)
		where T : unmanaged, IBinaryInteger<T>, IUnsignedNumber<T>
	{
		Debug.Assert(typeof(T) == typeof(ulong) || typeof(T) == typeof(UInt128) || typeof(T) == typeof(UInt256));
		if (typeof(T) == typeof(UInt256))
		{
			SetUInt256(out result, UInt256.CreateChecked(value));
		}
		else if (typeof(T) == typeof(UInt128))
		{
			SetUInt128(out result, UInt128.CreateChecked(value));
		}
		else
		{
			SetUInt64(out result, ulong.CreateChecked(value));
		}
	}

	public static void SetValue(out BigNumber result, scoped ref BigNumber value)
	{
		int rhsLength = (int)value._length;
		result._length = rhsLength;

		Span<ulong> r = MemoryMarshal.CreateSpan(ref result._blocks[0], rhsLength);
		Span<ulong> v = MemoryMarshal.CreateSpan(ref value._blocks[0], rhsLength);

		v.CopyTo(r);
	}

	public static void SetZero(out BigNumber result)
	{
		result._length = 0;
	}

	public void ShiftLeft(uint shift)
	{
		// Process blocks high to low so that we can safely process in place
		int length = (int)this._length;

		if ((length == 0) || (shift == 0))
		{
			return;
		}

		uint blocksToShift = DivRem64(shift, out uint remainingBitsToShift);

		// Copy blocks from high to low
		int readIndex = (length - 1);
		int writeIndex = readIndex + (int)(blocksToShift);

		// Check if the shift is block aligned
		if (remainingBitsToShift == 0)
		{
			Debug.Assert(writeIndex < MaxBlockCount);

			while (readIndex >= 0)
			{
				this._blocks[writeIndex] = this._blocks[readIndex];
				readIndex--;
				writeIndex--;
			}

			this._length += (int)(blocksToShift);

			// Zero the remaining low blocks
			Clear(blocksToShift);
		}
		else
		{
			// We need an extra block for the partial shift
			writeIndex++;
			Debug.Assert(writeIndex < MaxBlockCount);

			// Set the length to hold the shifted blocks
			this._length = writeIndex + 1;

			// Output the initial blocks
			ulong lowBitsShift = (64 - remainingBitsToShift);
			ulong highBits = 0;
			ulong block = this._blocks[readIndex];
			ulong lowBits = block >> (int)(lowBitsShift);
			while (readIndex > 0)
			{
				_blocks[writeIndex] = highBits | lowBits;
				highBits = block << (int)(remainingBitsToShift);

				--readIndex;
				--writeIndex;

				block = _blocks[readIndex];
				lowBits = block >> (int)lowBitsShift;
			}

			// Output the final blocks
			this._blocks[writeIndex] = highBits | lowBits;
			this._blocks[writeIndex - 1] = block << (int)(remainingBitsToShift);

			// Zero the remaining low blocks
			Clear(blocksToShift);

			// Check if the terminating block has no set bits
			if (_blocks[_length - 1] == 0)
			{
				_length--;
			}
		}
	}

	public uint ToUInt32()
	{
		if (_length > 0)
		{
			return (uint)_blocks[0];
		}

		return 0;
	}

	public ulong ToUInt64()
	{
		if (_length > 0)
		{
			return _blocks[0];
		}

		return 0;
	}

	public UInt128 ToUInt128()
	{
		if (_length > 1)
		{
			return new UInt128(_blocks[1], _blocks[0]);
		}

		if (_length > 0)
		{
			return _blocks[0];
		}

		return UInt128.Zero;
	}

	public UInt256 ToUInt256()
	{
		if (_length > 3)
		{
			return new UInt256(_blocks[3], _blocks[2], _blocks[1], _blocks[0]);
		}
		if (_length > 2)
		{
			return new UInt256(0, _blocks[2], _blocks[1], _blocks[0]);
		}
		if (_length > 1)
		{
			return new UInt256(0, 0, _blocks[1], _blocks[0]);
		}
		if (_length > 0)
		{
			return new UInt256(0, 0, 0, _blocks[0]);
		}

		return UInt256.Zero;
	}

	public T ToUInt<T>() where T : unmanaged, IBinaryInteger<T>, IUnsignedNumber<T>
	{
		Debug.Assert(typeof(T) == typeof(uint) || typeof(T) == typeof(ulong) || typeof(T) == typeof(UInt128) || typeof(T) == typeof(UInt256));
		if (typeof(T) == typeof(UInt256))
		{
			return T.CreateChecked(ToUInt256());
		}
		else if (typeof(T) == typeof(UInt128))
		{
			return T.CreateChecked(ToUInt128());
		}
		else if (typeof(T) == typeof(ulong))
		{
			return T.CreateChecked(ToUInt64());
		}
		else
		{
			return T.CreateChecked(ToUInt32());
		}
	}

	public override string ToString()
	{
		if (IsZero())
		{
			return "0";
		}
		StringBuilder stringBuilder = new StringBuilder(_blocks[0].ToString("X16"), (int)_length * 16);

		for (int i = 1; i < _length; i++)
		{
			stringBuilder.Insert(0, _blocks[i].ToString("X16"));
		}

		return stringBuilder.ToString();
	}

	private void Clear(uint length) =>
		NativeMemory.Clear(
			(byte*)Unsafe.AsPointer(ref _blocks[0]), // This is safe to do since we are a ref struct
			length * sizeof(ulong));

	private static uint DivRem32(uint value, out uint remainder)
	{
		remainder = value & 31;
		return value >> 5;
	}
	private static uint DivRem64(uint value, out uint remainder)
	{
		remainder = value & 63;
		return value >> 6;
	}
}