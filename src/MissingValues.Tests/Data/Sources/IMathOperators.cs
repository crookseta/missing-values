using System.Numerics;

namespace MissingValues.Tests.Data.Sources;

public interface IMathOperatorsDataSource<T>
	where T 
	: 
	IAdditionOperators<T, T, T>,
	IIncrementOperators<T>,
	ISubtractionOperators<T, T, T>,
	IDecrementOperators<T>,
	IMultiplyOperators<T, T, T>,
	IDivisionOperators<T, T, T>,
	IModulusOperators<T, T, T>
{
	static abstract IEnumerable<Func<(T, T, T)>> op_AdditionTestData();
	static abstract IEnumerable<Func<(T, T, T, bool)>> op_CheckedAdditionTestData();
	static abstract IEnumerable<Func<(T, T, bool)>> op_CheckedDecrementTestData();
	static abstract IEnumerable<Func<(T, T, bool)>> op_CheckedIncrementTestData();
	static abstract IEnumerable<Func<(T, T, T, bool)>> op_CheckedMultiplyTestData();
	static abstract IEnumerable<Func<(T, T, T, bool)>> op_CheckedSubtractionTestData();
	static abstract IEnumerable<Func<(T, T)>> op_DecrementTestData();
	static abstract IEnumerable<Func<(T, T, T)>> op_DivisionTestData();
	static abstract IEnumerable<Func<(T, T)>> op_IncrementTestData();
	static abstract IEnumerable<Func<(T, T, T)>> op_ModulusTestData();
	static abstract IEnumerable<Func<(T, T, T)>> op_MultiplyTestData();
	static abstract IEnumerable<Func<(T, T, T)>> op_SubtractionTestData();
}