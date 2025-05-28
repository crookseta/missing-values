using System.Globalization;
using MissingValues.Tests.Data.Sources;

namespace MissingValues.Tests.Data;

public class QuadDataSources
    : IMathOperatorsDataSource<Quad>,
        IBitwiseOperatorsDataSource<Quad>,
        IEqualityOperatorsDataSource<Quad>,
        IComparisonOperatorsDataSource<Quad>,
        INumberBaseDataSource<Quad>,
        INumberDataSource<Quad>,
        IBinaryNumberDataSource<Quad>,
        IFloatingPointDataSource<Quad>,
        IFloatingPointIeee754DataSource<Quad>
{
    public static IEnumerable<Func<(Quad, Quad, Quad)>> op_AdditionTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad, Quad, bool)>> op_CheckedAdditionTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad, bool)>> op_CheckedDecrementTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad, bool)>> op_CheckedIncrementTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad, Quad, bool)>> op_CheckedMultiplyTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad, Quad, bool)>> op_CheckedSubtractionTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad)>> op_DecrementTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad, Quad)>> op_DivisionTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad)>> op_IncrementTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad, Quad)>> op_ModulusTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad, Quad)>> op_MultiplyTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad, Quad)>> op_SubtractionTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad, Quad)>> op_BitwiseAndTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad, Quad)>> op_BitwiseOrTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad, Quad)>> op_BitwiseXorTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad)>> op_OnesComplementTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad, bool)>> op_EqualityTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad, bool)>> op_InequalityTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad, bool)>> op_GreaterThanOrEqualTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad, bool)>> op_GreaterThanTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad, bool)>> op_LessThanOrEqualTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad, bool)>> op_LessThanTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad)>> AbsTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, bool)>> IsCanonicalTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, bool)>> IsComplexNumberTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, bool)>> IsEvenIntegerTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, bool)>> IsFiniteTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, bool)>> IsImaginaryNumberTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, bool)>> IsInfinityTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, bool)>> IsIntegerTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, bool)>> IsNaNTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, bool)>> IsNegativeTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, bool)>> IsNegativeInfinityTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, bool)>> IsNormalTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, bool)>> IsOddIntegerTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, bool)>> IsPositiveTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, bool)>> IsPositiveInfinityTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, bool)>> IsRealNumberTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, bool)>> IsSubnormalTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, bool)>> IsZeroTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad, Quad)>> MaxMagnitudeTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad, Quad)>> MaxMagnitudeNumberTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad, Quad)>> MinMagnitudeTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad, Quad)>> MinMagnitudeNumberTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad, Quad, Quad)>> MultiplyAddEstimateTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(string, NumberStyles, IFormatProvider?, Quad)>> ParseTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(char[], NumberStyles, IFormatProvider?, Quad)>> ParseSpanTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(byte[], NumberStyles, IFormatProvider?, Quad)>> ParseUtf8TestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(string, NumberStyles, IFormatProvider?, bool, Quad)>> TryParseTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(char[], NumberStyles, IFormatProvider?, bool, Quad)>> TryParseSpanTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(byte[], NumberStyles, IFormatProvider?, bool, Quad)>> TryParseUtf8TestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad, Quad, Quad)>> ClampTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad, Quad)>> CopySignTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad, Quad)>> MaxTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad, Quad)>> MaxNumberTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad, Quad)>> MinTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad, Quad)>> MinNumberTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, int)>> SignTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, bool)>> IsPow2TestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad)>> Log2TestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad)>> CeilingTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad)>> FloorTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, int, MidpointRounding, Quad)>> RoundTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad)>> TruncateTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, int)>> GetExponentByteCountTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, int)>> GetExponentShortestBitLengthTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, int)>> GetSignificandBitLengthTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, int)>> GetSignificandByteCountTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, byte[], bool, int)>> TryWriteExponentBigEndianTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, byte[], bool, int)>> TryWriteExponentLittleEndianTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, byte[], bool, int)>> TryWriteSignificandBigEndianTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, byte[], bool, int)>> TryWriteSignificandLittleEndianTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad, Quad)>> Atan2TestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad, Quad)>> Atan2PiTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad)>> BitDecrementTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad)>> BitIncrementTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad, Quad, Quad)>> FusedMultiplyAddTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad, Quad)>> Ieee754RemainderTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad, Quad)>> ILogBTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad, Quad, Quad)>> LerpTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad)>> ReciprocalEstimateTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, Quad)>> ReciprocalSqrtEstimateTestData()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Func<(Quad, int, Quad)>> ScaleBTestData()
    {
        throw new NotImplementedException();
    }
    
	public static IEnumerable<Func<(Quad, byte)>> ConvertToCheckedByteTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, byte)>> ConvertToSaturatingByteTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, byte)>> ConvertToTruncatingByteTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Quad, ushort)>> ConvertToCheckedUInt16TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, ushort)>> ConvertToSaturatingUInt16TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, ushort)>> ConvertToTruncatingUInt16TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Quad, uint)>> ConvertToCheckedUInt32TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, uint)>> ConvertToSaturatingUInt32TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, uint)>> ConvertToTruncatingUInt32TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Quad, ulong)>> ConvertToCheckedUInt64TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, ulong)>> ConvertToSaturatingUInt64TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, ulong)>> ConvertToTruncatingUInt64TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Quad, UInt128)>> ConvertToCheckedUInt128TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, UInt128)>> ConvertToSaturatingUInt128TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, UInt128)>> ConvertToTruncatingUInt128TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Quad, UInt256)>> ConvertToCheckedUInt256TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, UInt256)>> ConvertToSaturatingUInt256TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, UInt256)>> ConvertToTruncatingUInt256TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Quad, UInt512)>> ConvertToCheckedUInt512TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, UInt512)>> ConvertToSaturatingUInt512TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, UInt512)>> ConvertToTruncatingUInt512TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Quad, sbyte)>> ConvertToCheckedSByteTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, sbyte)>> ConvertToSaturatingSByteTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, sbyte)>> ConvertToTruncatingSByteTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Quad, short)>> ConvertToCheckedInt16TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, short)>> ConvertToSaturatingInt16TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, short)>> ConvertToTruncatingInt16TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Quad, int)>> ConvertToCheckedInt32TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, int)>> ConvertToSaturatingInt32TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, int)>> ConvertToTruncatingInt32TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Quad, long)>> ConvertToCheckedInt64TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, long)>> ConvertToSaturatingInt64TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, long)>> ConvertToTruncatingInt64TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Quad, Int128)>> ConvertToCheckedInt128TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, Int128)>> ConvertToSaturatingInt128TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, Int128)>> ConvertToTruncatingInt128TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Quad, Int256)>> ConvertToCheckedInt256TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, Int256)>> ConvertToSaturatingInt256TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, Int256)>> ConvertToTruncatingInt256TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Quad, Int512)>> ConvertToCheckedInt512TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, Int512)>> ConvertToSaturatingInt512TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, Int512)>> ConvertToTruncatingInt512TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Quad, Half)>> ConvertToCheckedHalfTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, Half)>> ConvertToSaturatingHalfTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, Half)>> ConvertToTruncatingHalfTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Quad, float)>> ConvertToCheckedSingleTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, float)>> ConvertToSaturatingSingleTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, float)>> ConvertToTruncatingSingleTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Quad, double)>> ConvertToCheckedDoubleTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, double)>> ConvertToSaturatingDoubleTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, double)>> ConvertToTruncatingDoubleTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Quad, Octo)>> ConvertToCheckedOctoTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, Octo)>> ConvertToSaturatingOctoTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, Octo)>> ConvertToTruncatingOctoTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(byte, Quad)>> ConvertFromCheckedByteTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(byte, Quad)>> ConvertFromSaturatingByteTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(byte, Quad)>> ConvertFromTruncatingByteTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(ushort, Quad)>> ConvertFromCheckedUInt16TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(ushort, Quad)>> ConvertFromSaturatingUInt16TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(ushort, Quad)>> ConvertFromTruncatingUInt16TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(uint, Quad)>> ConvertFromCheckedUInt32TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(uint, Quad)>> ConvertFromSaturatingUInt32TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(uint, Quad)>> ConvertFromTruncatingUInt32TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(ulong, Quad)>> ConvertFromCheckedUInt64TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(ulong, Quad)>> ConvertFromSaturatingUInt64TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(ulong, Quad)>> ConvertFromTruncatingUInt64TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt128, Quad)>> ConvertFromCheckedUInt128TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt128, Quad)>> ConvertFromSaturatingUInt128TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt128, Quad)>> ConvertFromTruncatingUInt128TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt256, Quad)>> ConvertFromCheckedUInt256TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt256, Quad)>> ConvertFromSaturatingUInt256TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt256, Quad)>> ConvertFromTruncatingUInt256TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, Quad)>> ConvertFromCheckedUInt512TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt512, Quad)>> ConvertFromSaturatingUInt512TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt512, Quad)>> ConvertFromTruncatingUInt512TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(sbyte, Quad)>> ConvertFromCheckedSByteTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(sbyte, Quad)>> ConvertFromSaturatingSByteTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(sbyte, Quad)>> ConvertFromTruncatingSByteTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(short, Quad)>> ConvertFromCheckedInt16TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(short, Quad)>> ConvertFromSaturatingInt16TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(short, Quad)>> ConvertFromTruncatingInt16TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(int, Quad)>> ConvertFromCheckedInt32TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(int, Quad)>> ConvertFromSaturatingInt32TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(int, Quad)>> ConvertFromTruncatingInt32TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(long, Quad)>> ConvertFromCheckedInt64TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(long, Quad)>> ConvertFromSaturatingInt64TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(long, Quad)>> ConvertFromTruncatingInt64TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int128, Quad)>> ConvertFromCheckedInt128TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int128, Quad)>> ConvertFromSaturatingInt128TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int128, Quad)>> ConvertFromTruncatingInt128TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Quad)>> ConvertFromCheckedInt256TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int256, Quad)>> ConvertFromSaturatingInt256TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int256, Quad)>> ConvertFromTruncatingInt256TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, Quad)>> ConvertFromCheckedInt512TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int512, Quad)>> ConvertFromSaturatingInt512TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int512, Quad)>> ConvertFromTruncatingInt512TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Half, Quad)>> ConvertFromCheckedHalfTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Half, Quad)>> ConvertFromSaturatingHalfTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Half, Quad)>> ConvertFromTruncatingHalfTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(float, Quad)>> ConvertFromCheckedSingleTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(float, Quad)>> ConvertFromSaturatingSingleTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(float, Quad)>> ConvertFromTruncatingSingleTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(double, Quad)>> ConvertFromCheckedDoubleTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(double, Quad)>> ConvertFromSaturatingDoubleTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(double, Quad)>> ConvertFromTruncatingDoubleTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Octo, Quad)>> ConvertFromCheckedOctoTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Octo, Quad)>> ConvertFromSaturatingOctoTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Octo, Quad)>> ConvertFromTruncatingOctoTestData()
	{
		throw new NotImplementedException();
	}
}