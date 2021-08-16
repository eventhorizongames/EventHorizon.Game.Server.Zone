namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Tests.Runner
{
    using EventHorizon.Game.I18n;
    using EventHorizon.Game.I18n.Model;
    using EventHorizon.Zone.Core.Events.Client.Generic;
    using EventHorizon.Zone.Core.Events.Entity.Find;
    using EventHorizon.Zone.Core.Model.DateTimeService;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.Combat.Model.Client.Messsage;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Events.Runner;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Find;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Model.Entity;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Runner.Effect;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Validation;
    using EventHorizon.Zone.System.Combat.Skill.Runner;

    using FluentAssertions;

    using global::System.Collections.Concurrent;
    using global::System.Collections.Generic;
    using global::System.Numerics;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    using Microsoft.Extensions.Logging;

    using Moq;

    using Xunit;

    public class RunSkillWithTargetOfEntityHandlerTests
    {
        [Fact]
        public async Task ShouldRunSkillEffectWithTargetOfEntityEventWhenValidationOfSkillIsSuccessful()
        {
            // Given
            var connectionId = "connection-id";
            var casterId = 1L;
            var targetId = 12L;
            var skillId = "skill-id";
            var targetPosition = new Vector3(3, 3, 3);
            var i18n_default_skillExceptionMessage = "default_skillExpcetionMessage";
            var messageData = new MessageFromCombatSystemData
            {
                MessageCode = "skill_exception",
                Message = i18n_default_skillExceptionMessage,
            };
            var skillEffect = new SkillEffect
            {

            };

            var skillState = new SkillState
            {
                SkillMap = new SkillStateMap
                {
                    List = new List<SkillStateDetails>
                    {
                        new SkillStateDetails
                        {
                            Id = skillId,
                        }
                    }
                }
            };
            var caster = new DefaultEntity(
                new ConcurrentDictionary<string, object>()
            )
            {
                Id = casterId,
            };
            caster.SetProperty(
                SkillState.PROPERTY_NAME,
                skillState
            );
            var target = new DefaultEntity(
                new ConcurrentDictionary<string, object>()
            )
            {
                Id = targetId,
            };
            var validatorList = new List<SkillValidator>();
            var skill = new SkillInstance
            {
                Id = skillId,
                Next = new List<SkillEffect>
                {
                    skillEffect,
                }.ToArray(),
                ValidatorList = validatorList.ToArray(),
            };

            var expected = new RunSkillEffectWithTargetOfEntityEvent(
                connectionId,
                skillEffect,
                caster,
                target,
                skill,
                targetPosition,
                null
            );

            var loggerMock = new Mock<ILogger<RunSkillWithTargetOfEntityHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var i18nResolverMock = new Mock<I18nResolver>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new GetEntityByIdEvent(
                        casterId
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                caster
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new GetEntityByIdEvent(
                        targetId
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                target
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new FindSkillByIdEvent(
                        skillId
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                skill
            );

            i18nResolverMock.Setup(
                mock => mock.Resolve(
                    "default",
                    "skillExceptionMessage"
                )
            ).Returns(
                i18n_default_skillExceptionMessage
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new RunSkillValidation(
                        skill,
                        validatorList,
                        caster,
                        target,
                        targetPosition
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new List<SkillValidatorResponse>
                {
                    new SkillValidatorResponse
                    {
                        Success = true
                    }
                }
            );

            // When
            var handler = new RunSkillWithTargetOfEntityHandler(
                loggerMock.Object,
                mediatorMock.Object,
                i18nResolverMock.Object
            );
            await handler.Handle(
                new RunSkillWithTargetOfEntityEvent
                {
                    ConnectionId = connectionId,
                    CasterId = casterId,
                    TargetId = targetId,
                    SkillId = skillId,
                    TargetPosition = targetPosition,
                },
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Publish(
                    expected,
                    CancellationToken.None
                )
            );
        }

        [Fact]
        public async Task ShouldRunSkillEffectWithTargetOfEntityEventForFailedListWhenValidationOfSkillIsNotSuccessful()
        {
            // Given
            var connectionId = "connection-id";
            var casterId = 1L;
            var targetId = 12L;
            var skillId = "skill-id";
            var targetPosition = new Vector3(3, 3, 3);
            var i18n_default_skillExceptionMessage = "default_skillExpcetionMessage";
            var messageData = new MessageFromCombatSystemData
            {
                MessageCode = "skill_exception",
                Message = i18n_default_skillExceptionMessage,
            };
            var failedSkillEffect = new SkillEffect
            {

            };

            var skillState = new SkillState
            {
                SkillMap = new SkillStateMap
                {
                    List = new List<SkillStateDetails>
                    {
                        new SkillStateDetails
                        {
                            Id = skillId,
                        }
                    }
                }
            };
            var caster = new DefaultEntity(
                new ConcurrentDictionary<string, object>()
            )
            {
                Id = casterId,
            };
            caster.SetProperty(
                SkillState.PROPERTY_NAME,
                skillState
            );
            var target = new DefaultEntity(
                new ConcurrentDictionary<string, object>()
            )
            {
                Id = targetId,
            };

            var validatorList = new List<SkillValidator>();
            var skill = new SkillInstance
            {
                Id = skillId,
                FailedList = new List<SkillEffect>
                {
                    failedSkillEffect,
                }.ToArray(),
                ValidatorList = validatorList,
            };
            var failedVaildationResponse = new SkillValidatorResponse
            {
                Success = false,
                ErrorCode = "error-code",
            };

            var expectedConnectionId = connectionId;
            var expectedSkillEffect = failedSkillEffect;
            var expectedCaster = caster;
            var expectedTarget = target;
            var expectedSkill = skill;
            var expectedTargetPosition = targetPosition;
            var expectedState = new Dictionary<string, object>
            {
                {
                    "Code",
                    "skill_effect_validation_failed"
                },
                {
                    "ValidationResponse",
                    new List<SkillValidatorResponse>
                    {
                        failedVaildationResponse
                    }
                }
            };

            var loggerMock = new Mock<ILogger<RunSkillWithTargetOfEntityHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var i18nResolverMock = new Mock<I18nResolver>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new GetEntityByIdEvent(
                        casterId
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                caster
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new GetEntityByIdEvent(
                        targetId
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                target
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new FindSkillByIdEvent(
                        skillId
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                skill
            );

            i18nResolverMock.Setup(
                mock => mock.Resolve(
                    "default",
                    "skillExceptionMessage"
                )
            ).Returns(
                i18n_default_skillExceptionMessage
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new RunSkillValidation(
                        skill,
                        validatorList,
                        caster,
                        target,
                        targetPosition
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new List<SkillValidatorResponse>
                {
                    failedVaildationResponse
                }
            );

            // When
            var handler = new RunSkillWithTargetOfEntityHandler(
                loggerMock.Object,
                mediatorMock.Object,
                i18nResolverMock.Object
            );
            var actual = default(RunSkillEffectWithTargetOfEntityEvent);
            mediatorMock.Setup(
                mock => mock.Publish(
                    It.IsAny<RunSkillEffectWithTargetOfEntityEvent>(),
                    CancellationToken.None
                )
            ).Callback<RunSkillEffectWithTargetOfEntityEvent, CancellationToken>(
                (notification, _) =>
                {
                    actual = notification;
                }
            );
            await handler.Handle(
                new RunSkillWithTargetOfEntityEvent
                {
                    ConnectionId = connectionId,
                    CasterId = casterId,
                    TargetId = targetId,
                    SkillId = skillId,
                    TargetPosition = targetPosition,
                },
                CancellationToken.None
            );

            // Then
            actual.ConnectionId
                .Should().Be(connectionId);
            actual.SkillEffect
                .Should().Be(expectedSkillEffect);
            actual.Caster
                .Should().Be(expectedCaster);
            actual.Target
                .Should().Be(expectedTarget);
            actual.Skill
                .Should().Be(expectedSkill);
            actual.TargetPosition
                .Should().Be(expectedTargetPosition);
            actual.State
                .Should().BeEquivalentTo(expectedState);
        }

        [Fact]
        public async Task ShouldNotPublishRunSkillEffectWithTargetOfEntityEventWhenValidationOfSkillIsNotSuccessfulAndFailedListIsNull()
        {
            // Given
            var connectionId = "connection-id";
            var casterId = 1L;
            var targetId = 12L;
            var skillId = "skill-id";
            var targetPosition = new Vector3(3, 3, 3);
            var i18n_default_skillExceptionMessage = "default_skillExpcetionMessage";
            var messageData = new MessageFromCombatSystemData
            {
                MessageCode = "skill_exception",
                Message = i18n_default_skillExceptionMessage,
            };

            var skillState = new SkillState
            {
                SkillMap = new SkillStateMap
                {
                    List = new List<SkillStateDetails>
                    {
                        new SkillStateDetails
                        {
                            Id = skillId,
                        }
                    }
                }
            };
            var caster = new DefaultEntity(
                new ConcurrentDictionary<string, object>()
            )
            {
                Id = casterId,
            };
            caster.SetProperty(
                SkillState.PROPERTY_NAME,
                skillState
            );
            var target = new DefaultEntity(
                new ConcurrentDictionary<string, object>()
            )
            {
                Id = targetId,
            };
            var validatorList = new List<SkillValidator>();
            var skill = new SkillInstance
            {
                Id = skillId,
                FailedList = null,
                ValidatorList = validatorList.ToArray(),
            };
            var failedVaildationResponse = new SkillValidatorResponse
            {
                Success = false,
                ErrorCode = "error-code",
            };

            var loggerMock = new Mock<ILogger<RunSkillWithTargetOfEntityHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var i18nResolverMock = new Mock<I18nResolver>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new GetEntityByIdEvent(
                        casterId
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                caster
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new GetEntityByIdEvent(
                        targetId
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                target
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new FindSkillByIdEvent(
                        skillId
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                skill
            );

            i18nResolverMock.Setup(
                mock => mock.Resolve(
                    "default",
                    "skillExceptionMessage"
                )
            ).Returns(
                i18n_default_skillExceptionMessage
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new RunSkillValidation(
                        skill,
                        validatorList,
                        caster,
                        target,
                        targetPosition
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new List<SkillValidatorResponse>
                {
                    failedVaildationResponse
                }
            );

            // When
            var handler = new RunSkillWithTargetOfEntityHandler(
                loggerMock.Object,
                mediatorMock.Object,
                i18nResolverMock.Object
            );
            await handler.Handle(
                new RunSkillWithTargetOfEntityEvent
                {
                    ConnectionId = connectionId,
                    CasterId = casterId,
                    TargetId = targetId,
                    SkillId = skillId,
                    TargetPosition = targetPosition,
                },
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    It.IsAny<RunSkillEffectWithTargetOfEntityEvent>(),
                    CancellationToken.None
                ),
                Times.Never()
            );
        }

        [Fact]
        public async Task ShouldPublishClientActionGenericToSingleEventWhenCasterDoesNotHaveSkill()
        {
            // Given
            var connectionId = "connection-id";
            var casterId = 1L;
            var casterName = "caster-name";
            var targetId = 12L;
            var skillId = "skill-id";
            var skillName = "skill-name";
            var targetPosition = new Vector3(3, 3, 3);
            var i18n_default_casterDoesNotHaveSkill = "default_casterDoesNotHaveSkill";

            var skillState = new SkillState
            {
                SkillMap = new SkillStateMap
                {
                    List = new List<SkillStateDetails>(),
                }
            };
            var caster = new DefaultEntity(
                new ConcurrentDictionary<string, object>()
            )
            {
                Id = casterId,
                Name = casterName,
            };
            caster.SetProperty(
                SkillState.PROPERTY_NAME,
                skillState
            );
            var target = new DefaultEntity(
                new ConcurrentDictionary<string, object>()
            )
            {
                Id = targetId,
            };
            var validatorList = new List<SkillValidator>();
            var skill = new SkillInstance
            {
                Id = skillId,
                Name = skillName,
                ValidatorList = validatorList.ToArray(),
            };

            var expected = new ClientActionGenericToSingleEvent(
                connectionId,
                "MessageFromCombatSystem",
                new MessageFromCombatSystemData
                {
                    MessageCode = "does_not_have_skill",
                    Message = i18n_default_casterDoesNotHaveSkill,
                }
            );

            var loggerMock = new Mock<ILogger<RunSkillWithTargetOfEntityHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var i18nResolverMock = new Mock<I18nResolver>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new GetEntityByIdEvent(
                        casterId
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                caster
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new GetEntityByIdEvent(
                        targetId
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                target
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new FindSkillByIdEvent(
                        skillId
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                skill
            );

            i18nResolverMock.Setup(
                mock => mock.Resolve(
                    "default",
                    "casterDoesNotHaveSkill",
                    new I18nTokenValue
                    {
                        Token = "casterName",
                        Value = casterName,
                    },
                    new I18nTokenValue
                    {
                        Token = "skillName",
                        Value = skillName,
                    }
                )
            ).Returns(
                i18n_default_casterDoesNotHaveSkill
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new RunSkillValidation(
                        skill,
                        validatorList,
                        caster,
                        target,
                        targetPosition
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new List<SkillValidatorResponse>
                {
                    new SkillValidatorResponse
                    {
                        Success = true,
                    }
                }
            );

            // When
            var handler = new RunSkillWithTargetOfEntityHandler(
                loggerMock.Object,
                mediatorMock.Object,
                i18nResolverMock.Object
            );
            await handler.Handle(
                new RunSkillWithTargetOfEntityEvent
                {
                    ConnectionId = connectionId,
                    CasterId = casterId,
                    TargetId = targetId,
                    SkillId = skillId,
                    TargetPosition = targetPosition,
                },
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Publish(
                    expected,
                    CancellationToken.None
                )
            );
        }

        [Fact]
        public async Task ShouldPublishSkillExceptionClientEventWhenCasterIsNotFound()
        {
            // Given
            var connectionId = "connection-id";
            var casterId = 1L;
            var targetId = 12L;
            var skillId = "skill-id";
            var targetPosition = new Vector3(3, 3, 3);
            var i18n_default_skillExceptionMessage = "default_skillExpcetionMessage";
            var messageData = new MessageFromCombatSystemData
            {
                MessageCode = "skill_exception",
                Message = i18n_default_skillExceptionMessage,
            };

            var caster = default(DefaultEntity);
            var target = default(DefaultEntity);
            var skill = default(SkillInstance);

            var expected = new ClientActionGenericToSingleEvent(
                connectionId,
                "MessageFromCombatSystem",
                messageData
            );

            var loggerMock = new Mock<ILogger<RunSkillWithTargetOfEntityHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var i18nResolverMock = new Mock<I18nResolver>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new GetEntityByIdEvent(
                        casterId
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                caster
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new GetEntityByIdEvent(
                        targetId
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                target
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new FindSkillByIdEvent(
                        skillId
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                skill
            );

            i18nResolverMock.Setup(
                mock => mock.Resolve(
                    "default",
                    "skillExceptionMessage"
                )
            ).Returns(
                i18n_default_skillExceptionMessage
            );

            // When
            var handler = new RunSkillWithTargetOfEntityHandler(
                loggerMock.Object,
                mediatorMock.Object,
                i18nResolverMock.Object
            );
            await handler.Handle(
                new RunSkillWithTargetOfEntityEvent
                {
                    ConnectionId = connectionId,
                    CasterId = casterId,
                    TargetId = targetId,
                    SkillId = skillId,
                    TargetPosition = targetPosition,
                },
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Publish(
                    expected,
                    CancellationToken.None
                )
            );
        }

        [Fact]
        public async Task ShouldPublishSkillExceptionClientEventWhenTargetIsNotFound()
        {
            // Given
            var connectionId = "connection-id";
            var casterId = 1L;
            var targetId = 12L;
            var skillId = "skill-id";
            var targetPosition = new Vector3(3, 3, 3);
            var i18n_default_skillExceptionMessage = "default_skillExpcetionMessage";
            var messageData = new MessageFromCombatSystemData
            {
                MessageCode = "skill_exception",
                Message = i18n_default_skillExceptionMessage,
            };

            var caster = new DefaultEntity(
                new ConcurrentDictionary<string, object>()
            )
            {
                Id = casterId,
            };
            var target = default(DefaultEntity);
            var skill = default(SkillInstance);

            var expected = new ClientActionGenericToSingleEvent(
                connectionId,
                "MessageFromCombatSystem",
                messageData
            );

            var loggerMock = new Mock<ILogger<RunSkillWithTargetOfEntityHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var i18nResolverMock = new Mock<I18nResolver>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new GetEntityByIdEvent(
                        casterId
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                caster
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new GetEntityByIdEvent(
                        targetId
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                target
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new FindSkillByIdEvent(
                        skillId
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                skill
            );

            i18nResolverMock.Setup(
                mock => mock.Resolve(
                    "default",
                    "skillExceptionMessage"
                )
            ).Returns(
                i18n_default_skillExceptionMessage
            );

            // When
            var handler = new RunSkillWithTargetOfEntityHandler(
                loggerMock.Object,
                mediatorMock.Object,
                i18nResolverMock.Object
            );
            await handler.Handle(
                new RunSkillWithTargetOfEntityEvent
                {
                    ConnectionId = connectionId,
                    CasterId = casterId,
                    TargetId = targetId,
                    SkillId = skillId,
                    TargetPosition = targetPosition,
                },
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Publish(
                    expected,
                    CancellationToken.None
                )
            );
        }

        [Fact]
        public async Task ShouldPublishSkillExceptionClientEventWhenSkillIsNotFound()
        {
            // Given
            var connectionId = "connection-id";
            var casterId = 1L;
            var targetId = 12L;
            var skillId = "skill-id";
            var targetPosition = new Vector3(3, 3, 3);
            var i18n_default_skillExceptionMessage = "default_skillExpcetionMessage";
            var messageData = new MessageFromCombatSystemData
            {
                MessageCode = "skill_exception",
                Message = i18n_default_skillExceptionMessage,
            };

            var caster = new DefaultEntity(
                new ConcurrentDictionary<string, object>()
            )
            {
                Id = casterId,
            };
            var target = new DefaultEntity(
                new ConcurrentDictionary<string, object>()
            )
            {
                Id = targetId,
            };
            var skill = default(SkillInstance);

            var expected = new ClientActionGenericToSingleEvent(
                connectionId,
                "MessageFromCombatSystem",
                messageData
            );

            var loggerMock = new Mock<ILogger<RunSkillWithTargetOfEntityHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var i18nResolverMock = new Mock<I18nResolver>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new GetEntityByIdEvent(
                        casterId
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                caster
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new GetEntityByIdEvent(
                        targetId
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                target
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new FindSkillByIdEvent(
                        skillId
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                skill
            );

            i18nResolverMock.Setup(
                mock => mock.Resolve(
                    "default",
                    "skillExceptionMessage"
                )
            ).Returns(
                i18n_default_skillExceptionMessage
            );

            // When
            var handler = new RunSkillWithTargetOfEntityHandler(
                loggerMock.Object,
                mediatorMock.Object,
                i18nResolverMock.Object
            );
            await handler.Handle(
                new RunSkillWithTargetOfEntityEvent
                {
                    ConnectionId = connectionId,
                    CasterId = casterId,
                    TargetId = targetId,
                    SkillId = skillId,
                    TargetPosition = targetPosition,
                },
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Publish(
                    expected,
                    CancellationToken.None
                )
            );
        }
    }
}
