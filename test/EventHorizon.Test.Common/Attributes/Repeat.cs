namespace EventHorizon.Test.Common.Attributes;

using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Xunit.Sdk;

[ExcludeFromCodeCoverage]
public class RepeatAttribute : DataAttribute
{
    private readonly int _count;

    public RepeatAttribute(int count)
    {
        if (count < 1)
        {
            throw new System.ArgumentOutOfRangeException(
                paramName: nameof(count),
                message: "Repeat count must be greater than 0."
                );
        }
        _count = count;
    }

    public override System.Collections.Generic.IEnumerable<object[]> GetData(System.Reflection.MethodInfo testMethod)
    {
        foreach (var iterationNumber in Enumerable.Range(start: 1, count: _count))
        {
            yield return new object[] { iterationNumber };
        }
    }
}
