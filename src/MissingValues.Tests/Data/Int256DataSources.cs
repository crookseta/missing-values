using System.Globalization;
using MissingValues.Tests.Data.Sources;

namespace MissingValues.Tests.Data;

public class Int256DataSources
	: IMathOperatorsDataSource<Int256>,
	IShiftOperatorsDataSource<Int256>,
	IBitwiseOperatorsDataSource<Int256>,
	IEqualityOperatorsDataSource<Int256>,
	IComparisonOperatorsDataSource<Int256>,
	INumberBaseDataSource<Int256>,
	INumberDataSource<Int256>,
	IBinaryNumberDataSource<Int256>,
	IBinaryIntegerDataSource<Int256>
{
	public static IEnumerable<Func<(Int256, Int256, Int256)>> op_AdditionTestData()
	{
		yield return () => (Int256.Zero, Int256.Zero, Int256.Zero);
		yield return () => (Int256.One, Int256.Zero, Int256.One);
		yield return () => (Int256.One, Int256.One, new Int256(0, 0, 0, 2));
		yield return () => (new Int256(0, 0, 1, ulong.MaxValue), new Int256(0, 0, 1, 1), new Int256(0, 0, 3, 0));
		yield return () => (new Int256(0, 1, ulong.MaxValue, ulong.MaxValue), new Int256(0, 1, 1, 1), new Int256(0, 3, 1, 0));
		yield return () => (new Int256(1, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), new Int256(1, 1, 1, 1), new Int256(3, 1, 1, 0));
		yield return () => (Int256.MaxValue, Int256.One, Int256.MinValue);
		yield return () => (Int256.NegativeOne, Int256.One, Int256.Zero);
	}

	public static IEnumerable<Func<(Int256, Int256, Int256, bool)>> op_CheckedAdditionTestData()
	{
		yield return () => (Int256.Zero, Int256.Zero, Int256.Zero, false);
		yield return () => (Int256.One, Int256.Zero, Int256.One, false);
		yield return () => (Int256.One, Int256.One, new Int256(0, 0, 0, 2), false);
		yield return () => (new Int256(0, 0, 1, ulong.MaxValue), new Int256(0, 0, 1, 1), new Int256(0, 0, 3, 0), false);
		yield return () => (new Int256(0, 1, ulong.MaxValue, ulong.MaxValue), new Int256(0, 1, 1, 1), new Int256(0, 3, 1, 0), false);
		yield return () => (new Int256(1, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), new Int256(1, 1, 1, 1), new Int256(3, 1, 1, 0), false);
		yield return () => (Int256.MaxValue, Int256.One, Int256.MinValue, true);
		yield return () => (Int256.NegativeOne, Int256.One, Int256.Zero, false);
		yield return () => (Int256.MinValue, Int256.NegativeOne, Int256.MaxValue, true);
	}

	public static IEnumerable<Func<(Int256, Int256, bool)>> op_CheckedDecrementTestData()
	{
		yield return () => (Int256.Zero, Int256.NegativeOne, false);
		yield return () => (Int256.One, Int256.Zero, false);
		yield return () => (new Int256(0, 0, 0, 2), new Int256(0, 0, 0, 1), false);
		yield return () => (new Int256(0, 0, 1, 0), new Int256(0, 0, 0, ulong.MaxValue), false);
		yield return () => (new Int256(0, 1, 0, 0), new Int256(0, 0, ulong.MaxValue, ulong.MaxValue), false);
		yield return () => (new Int256(1, 0, 0, 0), new Int256(0, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), false);
		yield return () => (Int256.MinValue, Int256.MaxValue, true);
	}

	public static IEnumerable<Func<(Int256, Int256, bool)>> op_CheckedIncrementTestData()
	{
		yield return () => (Int256.Zero, Int256.One, false);
		yield return () => (Int256.One, new Int256(0, 0, 0, 2), false);
		yield return () => (Int256.MaxValue, Int256.Zero, true);
		yield return () => (Int256.NegativeOne, Int256.Zero, false);
		yield return () => (new Int256(0, 0, 0, ulong.MaxValue), new Int256(0, 0, 1, 0), false);
		yield return () => (new Int256(0, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), new Int256(1, 0, 0, 0), false);
		yield return () => (new Int256(unchecked((ulong)-123456789), 987654321, 555555555, 999999999), new Int256(unchecked((ulong)-123456789), 987654321, 555555555, 1000000000), false);
	}

	public static IEnumerable<Func<(Int256, Int256, Int256, bool)>> op_CheckedMultiplyTestData()
	{
		yield return () => (Int256.Zero, Int256.Zero, Int256.Zero, false);
		yield return () => (Int256.One, Int256.One, Int256.One, false);
		yield return () => (Int256.One, Int256.NegativeOne, Int256.NegativeOne, false);
		yield return () => (Int256.NegativeOne, Int256.NegativeOne, Int256.One, false);
		yield return () => (new Int256(0, 0, 0, 2), new Int256(0, 0, 0, 3), new Int256(0, 0, 0, 6), false);
		yield return () => (Int256.MaxValue, Int256.One, Int256.MaxValue, false);
		yield return () => (Int256.MaxValue, new Int256(0, 0, 0, 2), default, true);
		yield return () => (Int256.MinValue, Int256.NegativeOne, default, true);
		yield return () => (new Int256(0, 0, 0, ulong.MaxValue), new Int256(0, 0, 0, ulong.MaxValue), default, true);
		yield return () => (new Int256(ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), new Int256(2, 0, 0, 0), default, true);
	}

	public static IEnumerable<Func<(Int256, Int256, Int256, bool)>> op_CheckedSubtractionTestData()
	{
		yield return () => (Int256.Zero, Int256.Zero, Int256.Zero, false);
		yield return () => (Int256.One, Int256.Zero, Int256.One, false);
		yield return () => (Int256.One, Int256.One, Int256.Zero, false);
		yield return () => (new Int256(0, 0, 0, 2), Int256.One, Int256.One, false);
		yield return () => (new Int256(0, 0, 1, 0), new Int256(0, 0, 0, 1), new Int256(0, 0, 0, ulong.MaxValue), false);
		yield return () => (new Int256(0, 1, 0, 0), new Int256(0, 0, ulong.MaxValue, ulong.MaxValue), new Int256(0, 0, 0, 1), false);
		yield return () => (Int256.MinValue, Int256.One, Int256.MaxValue, true);
		yield return () => (Int256.MaxValue, Int256.NegativeOne, Int256.MinValue, true);
	}

	public static IEnumerable<Func<(Int256, Int256)>> op_DecrementTestData()
	{
		yield return () => (Int256.Zero, Int256.NegativeOne);
		yield return () => (Int256.One, Int256.Zero);
		yield return () => (new Int256(0, 0, 0, 2), new Int256(0, 0, 0, 1));
		yield return () => (new Int256(0, 0, 1, 0), new Int256(0, 0, 0, ulong.MaxValue));
		yield return () => (new Int256(0, 1, 0, 0), new Int256(0, 0, ulong.MaxValue, ulong.MaxValue));
		yield return () => (new Int256(1, 0, 0, 0), new Int256(0, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue));
		yield return () => (Int256.MinValue, Int256.MaxValue);
	}

	public static IEnumerable<Func<(Int256, Int256, Int256)>> op_DivisionTestData()
	{
		yield return () => (Int256.Zero, Int256.One, Int256.Zero);
		yield return () => (Int256.One, Int256.One, Int256.One);
		yield return () => (Int256.One, Int256.NegativeOne, Int256.NegativeOne);
		yield return () => (Int256.NegativeOne, Int256.One, Int256.NegativeOne);
		yield return () => (new Int256(0, 0, 0, 4), new Int256(0, 0, 0, 2), new Int256(0, 0, 0, 2));
		yield return () => (Int256.MaxValue, Int256.One, Int256.MaxValue);
		yield return () => (Int256.MinValue, Int256.One, Int256.MinValue);
		yield return () => (Int256.Zero, Int256.MaxValue, Int256.Zero);
		yield return () => (Int256.MaxValue, Int256.MaxValue, Int256.One);
		yield return () => (Int256.MinValue, Int256.MinValue, Int256.One);
		yield return () => (new Int256(0, 0, 1, 0), new Int256(0, 1, 0, 0), Int256.Zero);
		yield return () => (Int256.Parse("340282366920938463463374607431768211456"), Int256.Parse("18446744073709551616"), Int256.Parse("18446744073709551616"));
		yield return () => (Int256.Parse("340282366920938463463374607431768211456"), Int256.Parse("-18446744073709551616"), Int256.Parse("-18446744073709551616"));
		yield return () => (Int256.Parse("-340282366920938463463374607431768211456"), Int256.Parse("18446744073709551616"), Int256.Parse("-18446744073709551616"));
		yield return () => (Int256.Parse("-340282366920938463463374607431768211456"), Int256.Parse("-18446744073709551616"), Int256.Parse("18446744073709551616"));
		yield return () => (Int256.Parse("-340282366920938463463374607431768211456"), Int256.Parse("-18446744073709551616"), Int256.Parse("18446744073709551616"));
		yield return () => (Int256.Parse("57896044618658097711785492504343953926634992332820282019728792003956564819968"), new Int256(0, 0, 0, 10), Int256.Parse("5789604461865809771178549250434395392663499233282028201972879200395656481996"));
		yield return () => (Int256.Parse("170141183460469231731687303715884105728"), new Int256(0, 0, 0, 10), Int256.Parse("17014118346046923173168730371588410572"));
		yield return () => (Int256.Parse("-170141183460469231731687303715884105728"), new Int256(0, 0, 0, 10), Int256.Parse("-17014118346046923173168730371588410572"));
	}

	public static IEnumerable<Func<(Int256, Int256)>> op_IncrementTestData()
	{
		yield return () => (Int256.Zero, Int256.One);
		yield return () => (Int256.One, new Int256(0, 0, 0, 2));
		yield return () => (Int256.MaxValue, Int256.MinValue);
		yield return () => (Int256.NegativeOne, Int256.Zero);
		yield return () => (new Int256(0, 0, 0, ulong.MaxValue), new Int256(0, 0, 1, 0));
		yield return () => (new Int256(0, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), new Int256(1, 0, 0, 0));
		yield return () => (new Int256(123456789, 987654321, 555555555, 999999999), new Int256(123456789, 987654321, 555555555, 1000000000));
		yield return () => (new Int256(0x8000000000000000, 0, 0, 0),new Int256(0x8000000000000000, 0, 0, 1));
	}

	public static IEnumerable<Func<(Int256, Int256, Int256)>> op_ModulusTestData()
	{
		yield return () => (Int256.Zero, Int256.One, Int256.Zero);
		yield return () => (Int256.One, Int256.One, Int256.Zero);
		yield return () => (new Int256(0, 0, 0, 123456789), Int256.One, Int256.Zero);
		yield return () => (Int256.MaxValue, Int256.MaxValue, Int256.Zero);
		yield return () => (new Int256(0, 0, 1, 0), new Int256(0, 1, 0, 0), new Int256(0, 0, 1, 0));
		yield return () => (new Int256(0, 0, 0, 10), new Int256(0, 0, 0, 3), new Int256(0, 0, 0, 1));
		yield return () => (new Int256(0, 0, 0, 15), new Int256(0, 0, 0, 5), Int256.Zero);
		yield return () => (Int256.NegativeOne, new Int256(0, 0, 0, 2), Int256.NegativeOne);
		yield return () => (new Int256(0, 0, 0, 7), Int256.NegativeOne, Int256.Zero);
		yield return () => (Int256.MaxValue, new Int256(0, 0, 0, 123456789), new Int256(0, 0, 0, 77645365));
		yield return () => (Int256.Parse("401734511064747568885490523085290650630550748445698208825344"), Int256.Parse("100000000000000000000000000000000000000000000000000000000000"), Int256.Parse("1734511064747568885490523085290650630550748445698208825344"));
	}

	public static IEnumerable<Func<(Int256, Int256, Int256)>> op_MultiplyTestData()
	{
		yield return () => (Int256.Zero, Int256.Zero, Int256.Zero);
		yield return () => (Int256.Zero, Int256.One, Int256.Zero);
		yield return () => (Int256.One, Int256.One, Int256.One);
		yield return () => (Int256.One, Int256.NegativeOne, Int256.NegativeOne);
		yield return () => (Int256.NegativeOne, Int256.NegativeOne, Int256.One);
		yield return () => (new Int256(0, 0, 0, 2), new Int256(0, 0, 0, 3), new Int256(0, 0, 0, 6));
		yield return () => (new Int256(0, 0, 0, ulong.MaxValue), new Int256(0, 0, 0, 2), new Int256(0, 0, 1, ulong.MaxValue - 1));
		yield return () => (new Int256(0, 0, 0, ulong.MaxValue), new Int256(0, 0, 0, ulong.MaxValue), new Int256(ulong.MaxValue - 1, ulong.MaxValue, 0, 1));
	}

	public static IEnumerable<Func<(Int256, Int256, Int256)>> op_SubtractionTestData()
	{
		yield return () => (Int256.Zero, Int256.Zero, Int256.Zero);
		yield return () => (Int256.One, Int256.Zero, Int256.One);
		yield return () => (Int256.One, Int256.One, Int256.Zero);
		yield return () => (Int256.Zero, Int256.One, Int256.NegativeOne);
		yield return () => (Int256.MaxValue,Int256.One, Int256.Parse("57896044618658097711785492504343953926634992332820282019728792003956564819966"));
		yield return () => (Int256.MinValue,Int256.One,Int256.MaxValue);
		yield return () => (Int256.MinValue,Int256.NegativeOne, Int256.Parse("-57896044618658097711785492504343953926634992332820282019728792003956564819967"));
		yield return () => (new Int256(1, 2, 3, 4),new Int256(0, 1, 2, 3),new Int256(1, 1, 1, 1));
		yield return () => (new Int256(0, 0, 0, 0),new Int256(0, 0, 0, 1),new Int256(unchecked((ulong)-1), ulong.MaxValue, ulong.MaxValue, ulong.MaxValue));
	}

	public static IEnumerable<Func<(Int256, int, Int256)>> op_ShiftLeftTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, int, Int256)>> op_ShiftRightTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, int, Int256)>> op_UnsignedShiftRightTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256, Int256)>> op_BitwiseAndTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256, Int256)>> op_BitwiseOrTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256, Int256)>> op_BitwiseXorTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256)>> op_OnesComplementTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256, bool)>> op_EqualityTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256, bool)>> op_InequalityTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256, bool)>> op_GreaterThanOrEqualTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256, bool)>> op_GreaterThanTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256, bool)>> op_LessThanOrEqualTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256, bool)>> op_LessThanTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256)>> AbsTestData()
	{
		yield return () => (Int256.Zero, Int256.Zero);
		yield return () => (Int256.One, Int256.One);
		yield return () => (Int256.NegativeOne, Int256.One);
		yield return () => (Int256.MinValue + Int256.One, Int256.MaxValue);
	}

	public static IEnumerable<Func<(Int256, bool)>> IsCanonicalTestData()
	{
		yield return () => (Int256.Zero, true);
	}

	public static IEnumerable<Func<(Int256, bool)>> IsComplexNumberTestData()
	{
		yield return () => (Int256.Zero, true);
	}

	public static IEnumerable<Func<(Int256, bool)>> IsEvenIntegerTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, bool)>> IsFiniteTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, bool)>> IsImaginaryNumberTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, bool)>> IsInfinityTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, bool)>> IsIntegerTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, bool)>> IsNaNTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, bool)>> IsNegativeTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, bool)>> IsNegativeInfinityTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, bool)>> IsNormalTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, bool)>> IsOddIntegerTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, bool)>> IsPositiveTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, bool)>> IsPositiveInfinityTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, bool)>> IsRealNumberTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, bool)>> IsSubnormalTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, bool)>> IsZeroTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256, Int256)>> MaxMagnitudeTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256, Int256)>> MaxMagnitudeNumberTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256, Int256)>> MinMagnitudeTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256, Int256)>> MinMagnitudeNumberTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256, Int256, Int256)>> MultiplyAddEstimateTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(string, NumberStyles, IFormatProvider?, Int256)>> ParseTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(char[], NumberStyles, IFormatProvider?, Int256)>> ParseSpanTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(byte[], NumberStyles, IFormatProvider?, Int256)>> ParseUtf8TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(string, NumberStyles, IFormatProvider?, bool, Int256)>> TryParseTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(char[], NumberStyles, IFormatProvider?, bool, Int256)>> TryParseSpanTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(byte[], NumberStyles, IFormatProvider?, bool, Int256)>> TryParseUtf8TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256, Int256, Int256)>> ClampTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256, Int256)>> CopySignTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256, Int256)>> MaxTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256, Int256)>> MaxNumberTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256, Int256)>> MinTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256, Int256)>> MinNumberTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, int)>> SignTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, bool)>> IsPow2TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256)>> Log2TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256, (Int256, Int256))>> DivRemTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256)>> LeadingZeroCountTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256)>> PopCountTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(byte[], bool, Int256)>> ReadBigEndianTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(byte[], bool, Int256)>> ReadLittleEndianTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, int, Int256)>> RotateLeftTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, int, Int256)>> RotateRightTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256)>> TrailingZeroCountTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, int)>> GetByteCountTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, int)>> GetShortestBitLengthTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, byte[], int)>> WriteBigEndianTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, byte[], int)>> WriteLittleEndianTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int256, byte)>> ConvertToCheckedByteTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int256, byte)>> ConvertToSaturatingByteTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int256, byte)>> ConvertToTruncatingByteTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, ushort)>> ConvertToCheckedUInt16TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int256, ushort)>> ConvertToSaturatingUInt16TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int256, ushort)>> ConvertToTruncatingUInt16TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, uint)>> ConvertToCheckedUInt32TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int256, uint)>> ConvertToSaturatingUInt32TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int256, uint)>> ConvertToTruncatingUInt32TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, ulong)>> ConvertToCheckedUInt64TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int256, ulong)>> ConvertToSaturatingUInt64TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int256, ulong)>> ConvertToTruncatingUInt64TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, UInt128)>> ConvertToCheckedUInt128TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int256, UInt128)>> ConvertToSaturatingUInt128TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int256, UInt128)>> ConvertToTruncatingUInt128TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, UInt256)>> ConvertToCheckedUInt256TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int256, UInt256)>> ConvertToSaturatingUInt256TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int256, UInt256)>> ConvertToTruncatingUInt256TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, UInt512)>> ConvertToCheckedUInt512TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int256, UInt512)>> ConvertToSaturatingUInt512TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int256, UInt512)>> ConvertToTruncatingUInt512TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, sbyte)>> ConvertToCheckedSByteTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int256, sbyte)>> ConvertToSaturatingSByteTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int256, sbyte)>> ConvertToTruncatingSByteTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, short)>> ConvertToCheckedInt16TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int256, short)>> ConvertToSaturatingInt16TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int256, short)>> ConvertToTruncatingInt16TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, int)>> ConvertToCheckedInt32TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int256, int)>> ConvertToSaturatingInt32TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int256, int)>> ConvertToTruncatingInt32TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, long)>> ConvertToCheckedInt64TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int256, long)>> ConvertToSaturatingInt64TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int256, long)>> ConvertToTruncatingInt64TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int128)>> ConvertToCheckedInt128TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int256, Int128)>> ConvertToSaturatingInt128TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int256, Int128)>> ConvertToTruncatingInt128TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int512)>> ConvertToCheckedInt512TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int256, Int512)>> ConvertToSaturatingInt512TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int256, Int512)>> ConvertToTruncatingInt512TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Half)>> ConvertToCheckedHalfTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int256, Half)>> ConvertToSaturatingHalfTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int256, Half)>> ConvertToTruncatingHalfTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, float)>> ConvertToCheckedSingleTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int256, float)>> ConvertToSaturatingSingleTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int256, float)>> ConvertToTruncatingSingleTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, double)>> ConvertToCheckedDoubleTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int256, double)>> ConvertToSaturatingDoubleTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int256, double)>> ConvertToTruncatingDoubleTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Quad)>> ConvertToCheckedQuadTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int256, Quad)>> ConvertToSaturatingQuadTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int256, Quad)>> ConvertToTruncatingQuadTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Octo)>> ConvertToCheckedOctoTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int256, Octo)>> ConvertToSaturatingOctoTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int256, Octo)>> ConvertToTruncatingOctoTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(byte, Int256)>> ConvertFromCheckedByteTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(byte, Int256)>> ConvertFromSaturatingByteTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(byte, Int256)>> ConvertFromTruncatingByteTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(ushort, Int256)>> ConvertFromCheckedUInt16TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(ushort, Int256)>> ConvertFromSaturatingUInt16TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(ushort, Int256)>> ConvertFromTruncatingUInt16TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(uint, Int256)>> ConvertFromCheckedUInt32TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(uint, Int256)>> ConvertFromSaturatingUInt32TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(uint, Int256)>> ConvertFromTruncatingUInt32TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(ulong, Int256)>> ConvertFromCheckedUInt64TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(ulong, Int256)>> ConvertFromSaturatingUInt64TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(ulong, Int256)>> ConvertFromTruncatingUInt64TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt128, Int256)>> ConvertFromCheckedUInt128TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt128, Int256)>> ConvertFromSaturatingUInt128TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt128, Int256)>> ConvertFromTruncatingUInt128TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt256, Int256)>> ConvertFromCheckedUInt256TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt256, Int256)>> ConvertFromSaturatingUInt256TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt256, Int256)>> ConvertFromTruncatingUInt256TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, Int256)>> ConvertFromCheckedUInt512TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt512, Int256)>> ConvertFromSaturatingUInt512TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt512, Int256)>> ConvertFromTruncatingUInt512TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(sbyte, Int256)>> ConvertFromCheckedSByteTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(sbyte, Int256)>> ConvertFromSaturatingSByteTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(sbyte, Int256)>> ConvertFromTruncatingSByteTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(short, Int256)>> ConvertFromCheckedInt16TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(short, Int256)>> ConvertFromSaturatingInt16TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(short, Int256)>> ConvertFromTruncatingInt16TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(int, Int256)>> ConvertFromCheckedInt32TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(int, Int256)>> ConvertFromSaturatingInt32TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(int, Int256)>> ConvertFromTruncatingInt32TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(long, Int256)>> ConvertFromCheckedInt64TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(long, Int256)>> ConvertFromSaturatingInt64TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(long, Int256)>> ConvertFromTruncatingInt64TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int128, Int256)>> ConvertFromCheckedInt128TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int128, Int256)>> ConvertFromSaturatingInt128TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int128, Int256)>> ConvertFromTruncatingInt128TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, Int256)>> ConvertFromCheckedInt512TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int512, Int256)>> ConvertFromSaturatingInt512TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int512, Int256)>> ConvertFromTruncatingInt512TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Half, Int256)>> ConvertFromCheckedHalfTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Half, Int256)>> ConvertFromSaturatingHalfTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Half, Int256)>> ConvertFromTruncatingHalfTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(float, Int256)>> ConvertFromCheckedSingleTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(float, Int256)>> ConvertFromSaturatingSingleTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(float, Int256)>> ConvertFromTruncatingSingleTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(double, Int256)>> ConvertFromCheckedDoubleTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(double, Int256)>> ConvertFromSaturatingDoubleTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(double, Int256)>> ConvertFromTruncatingDoubleTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Quad, Int256)>> ConvertFromCheckedQuadTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, Int256)>> ConvertFromSaturatingQuadTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, Int256)>> ConvertFromTruncatingQuadTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Octo, Int256)>> ConvertFromCheckedOctoTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Octo, Int256)>> ConvertFromSaturatingOctoTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Octo, Int256)>> ConvertFromTruncatingOctoTestData()
	{
		throw new NotImplementedException();
	}
}
