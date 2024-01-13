namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Tests.Model;

using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;

using FluentAssertions;

using Xunit;

public class SkillValidatorResponseTests
{
    [Fact]
    public void ShouldHaveExpectedValuesWhenCreated()
    {
        // Given
        var validator = "validator";
        var success = true;
        var errorCode = "error-code";
        var errorMessageTemplateKey = "error-message-template-key";
        var errorMessageTemplateData = new { };

        var expectedValidator = validator;
        var expectedSuccess = success;
        var expectedErrorCode = errorCode;
        var expectedErrorMessageTemplateKey = errorMessageTemplateKey;
        var expectedErrorMessageTemplateData = errorMessageTemplateData;
        var expectedMessage = string.Empty;

        // When
        var actual = new SkillValidatorResponse
        {
            Validator = validator,
            Success = success,
            ErrorCode = errorCode,
            ErrorMessageTemplateKey = errorMessageTemplateKey,
            ErrorMessageTemplateData = errorMessageTemplateData,
        };

        // Then
        actual.Validator
            .Should().Be(expectedValidator);
        actual.Success
            .Should().Be(expectedSuccess);
        actual.ErrorCode
            .Should().Be(expectedErrorCode);
        actual.ErrorMessageTemplateKey
            .Should().Be(expectedErrorMessageTemplateKey);
        actual.ErrorMessageTemplateData
            .Should().Be(expectedErrorMessageTemplateData);
        actual.Message
            .Should().Be(expectedMessage);

    }
}
