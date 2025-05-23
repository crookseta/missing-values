using MissingValues.Tests.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnit.Assertions.AssertConditions.Throws;

namespace MissingValues.Tests.Numerics;

public class UInt256GenericMathTests
{
	#region Operators
	[Test]
	[MethodDataSource(typeof(UInt256DataSources), nameof(UInt256DataSources.op_AdditionTestData))]
	public async Task op_AdditionTest(UInt256 left, UInt256 right, UInt256 expected)
	{
		var result = left + right;

		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(UInt256DataSources), nameof(UInt256DataSources.op_CheckedAdditionTestData))]
	public async Task op_CheckedAdditionTest(UInt256 left, UInt256 right, UInt256 expected, bool overflows)
	{
		if (overflows)
		{
			await Assert.That(() => checked(left + right)).Throws<OverflowException>();
		}
		else
		{
			var result = checked(left + right);
			await Assert.That(result).IsEqualTo(expected);
		}
	}
	[Test]
	[MethodDataSource(typeof(UInt256DataSources), nameof(UInt256DataSources.op_IncrementTestData))]
	public async Task op_IncrementTest(UInt256 value, UInt256 expected)
	{
		var result = ++value;

		await Assert.That(result).IsEqualTo(expected).And.IsEqualTo(value);
	}
	[Test]
	[MethodDataSource(typeof(UInt256DataSources), nameof(UInt256DataSources.op_CheckedIncrementTestData))]
	public async Task op_CheckedIncrementTest(UInt256 value, UInt256 expected, bool overflows)
	{
		if (overflows)
		{
			await Assert.That(() => checked(++value)).Throws<OverflowException>();
		}
		else
		{
			var result = checked(++value);
			await Assert.That(result).IsEqualTo(expected).And.IsEqualTo(value);
		}
	}
	[Test]
	[MethodDataSource(typeof(UInt256DataSources), nameof(UInt256DataSources.op_SubtractionTestData))]
	public async Task op_SubtractionTest(UInt256 left, UInt256 right, UInt256 expected)
	{
		var result = left - right;

		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(UInt256DataSources), nameof(UInt256DataSources.op_CheckedSubtractionTestData))]
	public async Task op_CheckedSubtractionTest(UInt256 left, UInt256 right, UInt256 expected, bool overflows)
	{
		if (overflows)
		{
			await Assert.That(() => checked(left - right)).Throws<OverflowException>();
		}
		else
		{
			var result = checked(left - right);
			await Assert.That(result).IsEqualTo(expected);
		}
	}
	[Test]
	[MethodDataSource(typeof(UInt256DataSources), nameof(UInt256DataSources.op_DecrementTestData))]
	public async Task op_DecrementTest(UInt256 value, UInt256 expected)
	{
		var result = --value;

		await Assert.That(result).IsEqualTo(expected).And.IsEqualTo(value);
	}
	[Test]
	[MethodDataSource(typeof(UInt256DataSources), nameof(UInt256DataSources.op_CheckedDecrementTestData))]
	public async Task op_CheckedDecrementTest(UInt256 value, UInt256 expected, bool overflows)
	{
		if (overflows)
		{
			await Assert.That(() => checked(--value)).Throws<OverflowException>();
		}
		else
		{
			var result = checked(--value);
			await Assert.That(result).IsEqualTo(expected).And.IsEqualTo(value);
		}
	}
	[Test]
	[MethodDataSource(typeof(UInt256DataSources), nameof(UInt256DataSources.op_MultiplyTestData))]
	public async Task op_MultiplyTest(UInt256 left, UInt256 right, UInt256 expected)
	{
		var result = left * right;

		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(UInt256DataSources), nameof(UInt256DataSources.op_CheckedMultiplyTestData))]
	public async Task op_CheckedMultiplyTest(UInt256 left, UInt256 right, UInt256 expected, bool overflows)
	{
		if (overflows)
		{
			await Assert.That(() => checked(left * right)).Throws<OverflowException>();
		}
		else
		{
			var result = checked(left * right);
			await Assert.That(result).IsEqualTo(expected);
		}
	}
	[Test]
	[MethodDataSource(typeof(UInt256DataSources), nameof(UInt256DataSources.op_DivisionTestData))]
	public async Task op_DivisionTest(UInt256 left, UInt256 right, UInt256 expected)
	{
		if (right == UInt256.Zero)
		{
			await Assert.That(() => left / right).Throws<DivideByZeroException>();
		}
		else
		{
			var result = left / right;

			await Assert.That(result).IsEqualTo(expected);
		}
	}
	[Test]
	[MethodDataSource(typeof(UInt256DataSources), nameof(UInt256DataSources.op_ModulusTestData))]
	public async Task op_ModulusTest(UInt256 left, UInt256 right, UInt256 expected)
	{
		if (right == UInt256.Zero)
		{
			await Assert.That(() => left % right).Throws<DivideByZeroException>();
		}
		else
		{
			var result = left % right;

			await Assert.That(result).IsEqualTo(expected);
		}
	}
	[Test]
	[MethodDataSource(typeof(UInt256DataSources), nameof(UInt256DataSources.op_OnesComplementTestData))]
	public async Task op_OnesComplementTest(UInt256 value, UInt256 expected)
	{
		var result = ~value;

		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(UInt256DataSources), nameof(UInt256DataSources.op_BitwiseAndTestData))]
	public async Task op_BitwiseAndTest(UInt256 left, UInt256 right, UInt256 expected)
	{
		var result = left & right;

		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(UInt256DataSources), nameof(UInt256DataSources.op_BitwiseOrTestData))]
	public async Task op_BitwiseOrTest(UInt256 left, UInt256 right, UInt256 expected)
	{
		var result = left | right;

		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(UInt256DataSources), nameof(UInt256DataSources.op_BitwiseXorTestData))]
	public async Task op_BitwiseXorTest(UInt256 left, UInt256 right, UInt256 expected)
	{
		var result = left ^ right;

		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(UInt256DataSources), nameof(UInt256DataSources.op_ShiftLeftTestData))]
	public async Task op_ShiftLeftTest(UInt256 value, int shiftAmount, UInt256 expected)
	{
		var result = value << shiftAmount;

		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(UInt256DataSources), nameof(UInt256DataSources.op_ShiftRightTestData))]
	public async Task op_ShiftRightTest(UInt256 value, int shiftAmount, UInt256 expected)
	{
		var result = value >> shiftAmount;

		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(UInt256DataSources), nameof(UInt256DataSources.op_UnsignedShiftRightTestData))]
	public async Task op_UnsignedShiftRightTest(UInt256 value, int shiftAmount, UInt256 expected)
	{
		var result = value >>> shiftAmount;

		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(UInt256DataSources), nameof(UInt256DataSources.op_EqualityTestData))]
	public async Task op_EqualityTest(UInt256 left, UInt256 right, bool expected)
	{
		var result = left == right;

		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(UInt256DataSources), nameof(UInt256DataSources.op_InequalityTestData))]
	public async Task op_InequalityTest(UInt256 left, UInt256 right, bool expected)
	{
		var result = left != right;

		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(UInt256DataSources), nameof(UInt256DataSources.op_LessThanTestData))]
	public async Task op_LessThanTest(UInt256 left, UInt256 right, bool expected)
	{
		var result = left < right;

		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(UInt256DataSources), nameof(UInt256DataSources.op_LessThanOrEqualTestData))]
	public async Task op_LessThanOrEqualTest(UInt256 left, UInt256 right, bool expected)
	{
		var result = left <= right;

		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(UInt256DataSources), nameof(UInt256DataSources.op_GreaterThanTestData))]
	public async Task op_GreaterThanTest(UInt256 left, UInt256 right, bool expected)
	{
		var result = left > right;

		await Assert.That(result).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource(typeof(UInt256DataSources), nameof(UInt256DataSources.op_GreaterThanOrEqualTestData))]
	public async Task op_GreaterThanOrEqualTest(UInt256 left, UInt256 right, bool expected)
	{
		var result = left >= right;

		await Assert.That(result).IsEqualTo(expected);
	}
	#endregion
}
