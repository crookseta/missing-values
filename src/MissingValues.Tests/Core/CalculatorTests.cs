using MissingValues.Internals;
using TUnit.Assertions.AssertConditions.Throws;

namespace MissingValues.Tests.Core;

public class CalculatorTests
{
	[Test]
	public async Task AddWithCarry_NoOverflow_ReturnsCorrectSumAndZeroCarry()
	{
		ulong a = 10;
		ulong b = 20;

		ulong result = Calculator.AddWithCarry(a, b, out ulong carry);

		await Assert.That(result).IsEqualTo(30UL);
		await Assert.That(carry).IsEqualTo(0UL);
	}
	[Test]
	public async Task AddWithCarry_MaxValueAndOne_ReturnsZeroAndCarry()
	{
		ulong a = ulong.MaxValue;
		ulong b = 1;

		ulong result = Calculator.AddWithCarry(a, b, out ulong carry);

		await Assert.That(result).IsEqualTo(0UL);
		await Assert.That(carry).IsEqualTo(1UL);
	}
	[Test]
	public async Task AddWithCarry_OverflowCase_ReturnsCorrectResultAndCarry()
	{
		ulong a = ulong.MaxValue - 1;
		ulong b = 5;

		ulong result = Calculator.AddWithCarry(a, b, out ulong carry);

		await Assert.That(result).IsEqualTo(3UL);
		await Assert.That(carry).IsEqualTo(1UL);
	}
	[Test]
	public async Task AddWithCarry_AddZero_ReturnsSameValueAndZeroCarry()
	{
		ulong a = 123456789;
		ulong b = 0;

		ulong result = Calculator.AddWithCarry(a, b, out ulong carry);

		await Assert.That(result).IsEqualTo(a);
		await Assert.That(carry).IsEqualTo(0UL);
	}
	[Test]
	public async Task AddWithCarry_BothZero_ReturnsZeroAndZeroCarry()
	{
		ulong a = 0;
		ulong b = 0;

		ulong result = Calculator.AddWithCarry(a, b, out ulong carry);

		await Assert.That(result).IsEqualTo(0UL);
		await Assert.That(carry).IsEqualTo(0UL);
	}
	[Test]
	public async Task AddWithCarry_ExactMaxValueSum_ReturnsMaxValueAndZeroCarry()
	{
		ulong a = ulong.MaxValue / 2;
		ulong b = ulong.MaxValue / 2;

		ulong result = Calculator.AddWithCarry(a, b, out ulong carry);

		await Assert.That(result).IsEqualTo(ulong.MaxValue - 1);
		await Assert.That(carry).IsEqualTo(0UL);
	}
	[Test]
	public async Task AddWithCarry_CarryOnlyTriggeredWhenOverflow()
	{
		ulong a = ulong.MaxValue;
		ulong b = 0;

		ulong result = Calculator.AddWithCarry(a, b, out ulong carry);

		await Assert.That(result).IsEqualTo(ulong.MaxValue);
		await Assert.That(carry).IsEqualTo(0UL);
	}

	[Test]
	public async Task BigMulAdd_SimpleMultiplicationAndAddition()
	{
		ulong a = 2;
		ulong b = 3;
		ulong c = 5;

		var (hi, lo) = Calculator.BigMulAdd(a, b, c);

		await Assert.That(hi).IsEqualTo(0UL);
		await Assert.That(lo).IsEqualTo(11UL);
	}
	[Test]
	public async Task BigMulAdd_WithOverflowFromLowToHigh()
	{
		ulong a = ulong.MaxValue;
		ulong b = 2;
		ulong c = 1;

		var (hi, lo) = Calculator.BigMulAdd(a, b, c);

		await Assert.That(hi).IsEqualTo(1UL);
		await Assert.That(lo).IsEqualTo(0xFFFFFFFFFFFFFFFFUL);
	}
	[Test]
	public async Task BigMulAdd_MaxValues()
	{
		ulong a = ulong.MaxValue;
		ulong b = ulong.MaxValue;
		ulong c = ulong.MaxValue;

		var (hi, lo) = Calculator.BigMulAdd(a, b, c);

		await Assert.That(hi).IsEqualTo(0xFFFFFFFFFFFFFFFFUL);
		await Assert.That(lo).IsEqualTo(0UL);
	}
	[Test]
	public async Task BigMulAdd_NoCarryOrOverflow()
	{
		ulong a = 1000;
		ulong b = 1000;
		ulong c = 1;

		var (hi, lo) = Calculator.BigMulAdd(a, b, c);

		await Assert.That(hi).IsEqualTo(0UL);
		await Assert.That(lo).IsEqualTo(1_000_001UL);
	}
	[Test]
	public async Task BigMulAdd_CausesCarryFromAdditionOnly()
	{
		ulong a = ulong.MaxValue;
		ulong b = 1;
		ulong c = 1;

		var (hi, lo) = Calculator.BigMulAdd(a, b, c);

		await Assert.That(hi).IsEqualTo(1UL);
		await Assert.That(lo).IsEqualTo(0UL);
	}
	[Test]
	public async Task BigMulAdd_CausesNoCarryWithZeroInputs()
	{
		ulong a = 0;
		ulong b = 0;
		ulong c = 0;

		var (hi, lo) = Calculator.BigMulAdd(a, b, c);

		await Assert.That(hi).IsEqualTo(0UL);
		await Assert.That(lo).IsEqualTo(0UL);
	}

	[Test]
	public async Task DivRemByUInt32_BasicDivision()
	{
		ulong left = 100;
		uint right = 7;

		var (quotient, remainder) = Calculator.DivRemByUInt32(left, right);

		await Assert.That(quotient).IsEqualTo(14UL);
		await Assert.That(remainder).IsEqualTo(2U);
	}
	[Test]
	public async Task DivRemByUInt32_DivideByOne()
	{
		ulong left = 1234567890123456789;
		uint right = 1;

		var (quotient, remainder) = Calculator.DivRemByUInt32(left, right);

		await Assert.That(quotient).IsEqualTo(left);
		await Assert.That(remainder).IsEqualTo(0U);
	}
	[Test]
	public async Task DivRemByUInt32_DivideByMaxUInt32()
	{
		ulong left = (ulong)uint.MaxValue + 1;
		uint right = uint.MaxValue;

		var (quotient, remainder) = Calculator.DivRemByUInt32(left, right);

		await Assert.That(quotient).IsEqualTo(1UL);
		await Assert.That(remainder).IsEqualTo(1U);
	}
	[Test]
	public async Task DivRemByUInt32_ZeroDividend()
	{
		ulong left = 0;
		uint right = uint.MaxValue;

		var (quotient, remainder) = Calculator.DivRemByUInt32(left, right);

		await Assert.That(quotient).IsEqualTo(0UL);
		await Assert.That(remainder).IsEqualTo(0U);
	}
	[Test]
	public async Task DivRemByUInt32_ExactDivision()
	{
		ulong left = 400;
		uint right = 20;

		var (quotient, remainder) = Calculator.DivRemByUInt32(left, right);

		await Assert.That(quotient).IsEqualTo(20UL);
		await Assert.That(remainder).IsEqualTo(0U);
	}
	[Test]
	public async Task DivRemByUInt32_MaxValues()
	{
		ulong left = ulong.MaxValue;
		uint right = uint.MaxValue;

		var (quotient, remainder) = Calculator.DivRemByUInt32(left, right);

		await Assert.That(quotient).IsEqualTo(ulong.MaxValue / uint.MaxValue);
		await Assert.That(remainder).IsEqualTo((uint)(ulong.MaxValue % uint.MaxValue));
	}
	[Test]
	public async Task DivRemByUInt32_DivideByZero_Throws()
	{
		ulong left = 100;
		uint right = 0;

		await Assert.That(() => Calculator.DivRemByUInt32(left, right)).Throws<DivideByZeroException>();
	}

	[Test]
	public async Task DivRemByUInt64_BasicDivision()
	{
		UInt128 left = new UInt128(0, 1000);
		ulong right = 30;

		var (quotient, remainder) = Calculator.DivRemByUInt64(left, right);

		await Assert.That(quotient).IsEqualTo(new UInt128(0, 33));
		await Assert.That(remainder).IsEqualTo(10UL);
	}
	[Test]
	public async Task DivRemByUInt64_ZeroDividend()
	{
		UInt128 left = new UInt128(0, 0);
		ulong right = 12345;

		var (quotient, remainder) = Calculator.DivRemByUInt64(left, right);

		await Assert.That(quotient).IsEqualTo(UInt128.Zero);
		await Assert.That(remainder).IsEqualTo(0UL);
	}
	[Test]
	public async Task DivRemByUInt64_DivideByOne()
	{
		UInt128 left = new UInt128(123456789, 9876543210);
		ulong right = 1;

		var (quotient, remainder) = Calculator.DivRemByUInt64(left, right);

		await Assert.That(quotient).IsEqualTo(new UInt128(123456789, 9876543210));
		await Assert.That(remainder).IsEqualTo(0UL);
	}
	[Test]
	public async Task DivRemByUInt64_ExactDivision()
	{
		UInt128 left = new UInt128(0, 1000);
		ulong right = 1000;

		var (quotient, remainder) = Calculator.DivRemByUInt64(left, right);

		await Assert.That(quotient).IsEqualTo(new UInt128(0, 1));
		await Assert.That(remainder).IsEqualTo(0UL);
	}
	[Test]
	public async Task DivRemByUInt64_UpperBitsUsed()
	{
		UInt128 left = new UInt128(1, 0);
		ulong right = 2;

		var (quotient, remainder) = Calculator.DivRemByUInt64(left, right);

		await Assert.That(quotient).IsEqualTo(new UInt128(0, 1UL << 63));
		await Assert.That(remainder).IsEqualTo(0UL);
	}
	[Test]
	public async Task DivRemByUInt64_MaxValues()
	{
		UInt128 left = UInt128.MaxValue;
		ulong right = ulong.MaxValue;

		var (quotient, remainder) = Calculator.DivRemByUInt64(left, right);

		await Assert.That(quotient).IsEqualTo(UInt128.MaxValue / ulong.MaxValue);
		await Assert.That(remainder).IsEqualTo((ulong)(UInt128.MaxValue % ulong.MaxValue));
	}
	[Test]
	public async Task DivRemByUInt64_DivideByZero_Throws()
	{
		UInt128 left = new UInt128(1, 0);
		ulong right = 0;

		await Assert.That(() => Calculator.DivRemByUInt64(left, right)).Throws<DivideByZeroException>();
	}
}
