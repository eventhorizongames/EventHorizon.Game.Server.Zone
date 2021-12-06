namespace EventHorizon.Test.Common.Attributes;

using AutoFixture.Xunit2;

using Xunit;

public class InlineAutoMoqDataAttribute
    : CompositeDataAttribute
{
    public InlineAutoMoqDataAttribute(params object[] values)
        : base(new InlineDataAttribute(values), new AutoMoqDataAttribute())
    {
    }
}
