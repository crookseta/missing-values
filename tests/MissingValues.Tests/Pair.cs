namespace MissingValues.Tests;

public record struct Pair<T>(T First, T Second)
{
    public static implicit operator Pair<T>((T, T) p)
    {
        return new Pair<T>(p.Item1, p.Item2);
    }
}