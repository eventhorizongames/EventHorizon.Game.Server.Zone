namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Tests.Validation
{
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Validation;
    using EventHorizon.Zone.System.Server.Scripts.Events.Run;
    using FluentAssertions;
    using global::System.Collections.Generic;
    using global::System.Numerics;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;
    using Moq;
    using Xunit;

    public class RunValidateForSkillEffectHandlerTests
    {
        [Fact]
        public async Task ShouldReturnSkillValidatorResposneListWhenRequestIsHandled()
        {
            // Given
            var validator1 = "validator-1";
            var validatorResponse1 = new SkillValidatorResponse
            {
                Success = true,
            };
            var validator2 = "validator-2";
            var validatorResponse2 = new SkillValidatorResponse
            {
                Success = true,
            };
            var validator3 = "validator-3";
            var validatorResponse3 = new SkillValidatorResponse
            {
                Success = true,
            };
            var expected = new List<SkillValidatorResponse>
            {
                validatorResponse1,
                validatorResponse2,
                validatorResponse3,
            };

            var skill = new SkillInstance();
            var validatorList = new List<SkillValidator>
            {
                new SkillValidator
                {
                    Validator = validator1,
                },
                new SkillValidator
                {
                    Validator = validator2,
                },
                new SkillValidator
                {
                    Validator = validator3,
                },
            };
            var caster = new DefaultEntity();
            var target = new DefaultEntity();
            var targetPosition = Vector3.Zero;

            var mediatorMock = new Mock<IMediator>();

            mediatorMock.Setup(
                mock => mock.Send(
                    It.Is<RunServerScriptCommand>(
                        request => request.Id == validator1
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                validatorResponse1
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    It.Is<RunServerScriptCommand>(
                        request => request.Id == validator2
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                validatorResponse2
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    It.Is<RunServerScriptCommand>(
                        request => request.Id == validator3
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                validatorResponse3
            );

            // When
            var handler = new RunSkillValidationHandler(
                mediatorMock.Object
            );
            var actual = await handler.Handle(
                new RunSkillValidation(
                    skill,
                    validatorList,
                    caster,
                    target,
                    targetPosition
                ),
                CancellationToken.None
            );

            // Then
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task ShouldOnlyReturnUpToFirstFailureSkillValidatorResposneWhenRequestIsHandled()
        {
            // Given
            var validator1 = "validator-1";
            var validatorResponse1 = new SkillValidatorResponse
            {
                Success = true,
            };
            var validator2 = "validator-2";
            var validatorResponse2 = new SkillValidatorResponse
            {
                Success = false,
            };
            var validator3 = "validator-3";
            var validatorResponse3 = new SkillValidatorResponse
            {
                Success = true,
            };
            var expected = new List<SkillValidatorResponse>
            {
                validatorResponse1,
                validatorResponse2,
            };

            var skill = new SkillInstance();
            var validatorList = new List<SkillValidator>
            {
                new SkillValidator
                {
                    Validator = validator1,
                },
                new SkillValidator
                {
                    Validator = validator2,
                },
                new SkillValidator
                {
                    Validator = validator3,
                },
            };
            var caster = new DefaultEntity();
            var target = new DefaultEntity();
            var targetPosition = Vector3.Zero;

            var mediatorMock = new Mock<IMediator>();

            mediatorMock.Setup(
                mock => mock.Send(
                    It.Is<RunServerScriptCommand>(
                        request => request.Id == validator1
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                validatorResponse1
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    It.Is<RunServerScriptCommand>(
                        request => request.Id == validator2
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                validatorResponse2
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    It.Is<RunServerScriptCommand>(
                        request => request.Id == validator3
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                validatorResponse3
            );

            // When
            var handler = new RunSkillValidationHandler(
                mediatorMock.Object
            );
            var actual = await handler.Handle(
                new RunSkillValidation(
                    skill,
                    validatorList,
                    caster,
                    target,
                    targetPosition
                ),
                CancellationToken.None
            );

            // Then
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task ShouldReturnEmptyResponseWhenSkillEffectValidatorListIsEmpty()
        {
            // Given
            var expected = new List<SkillValidatorResponse>();

            var skillEffect = new SkillEffect
            {
            };
            var skill = new SkillInstance();
            var validatorList = new List<SkillValidator>();
            var caster = new DefaultEntity();
            var target = new DefaultEntity();
            var targetPosition = Vector3.Zero;

            var mediatorMock = new Mock<IMediator>();

            // When
            var handler = new RunSkillValidationHandler(
                mediatorMock.Object
            );
            var actual = await handler.Handle(
                new RunSkillValidation(
                    skill,
                    validatorList,
                    caster,
                    target,
                    targetPosition
                ),
                CancellationToken.None
            );

            // Then
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task ShouldReturnEmptyResponseWhenSkillEffectValidatorListIsNull()
        {
            // Given
            var expected = new List<SkillValidatorResponse>();

            var skill = new SkillInstance();
            List<SkillValidator> validatorList = null;
            var caster = new DefaultEntity();
            var target = new DefaultEntity();
            var targetPosition = Vector3.Zero;

            var mediatorMock = new Mock<IMediator>();

            // When
            var handler = new RunSkillValidationHandler(
                mediatorMock.Object
            );
            var actual = await handler.Handle(
                new RunSkillValidation(
                    skill,
                    validatorList,
                    caster,
                    target,
                    targetPosition
                ),
                CancellationToken.None
            );

            // Then
            actual.Should().BeEquivalentTo(expected);
        }
    }
}
