namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Tests.Runner.Effect;

using EventHorizon.Zone.Core.Events.Client.Generic;
using EventHorizon.Zone.Core.Events.ServerAction;
using EventHorizon.Zone.Core.Model.Client;
using EventHorizon.Zone.Core.Model.DateTimeService;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Combat.Plugin.Skill.ClientAction;
using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;
using EventHorizon.Zone.System.Combat.Plugin.Skill.Runner.Effect;
using EventHorizon.Zone.System.Combat.Plugin.Skill.Validation;
using EventHorizon.Zone.System.Server.Scripts.Events.Run;

using FluentAssertions;

using global::System;
using global::System.Collections.Generic;
using global::System.Numerics;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Microsoft.Extensions.Logging;

using Moq;

using Xunit;

public class RunSkillEffectWithTargetOfEntityEventHandlerTests
{
    [Fact]
    public async Task ShouldAddServerActionEventForAllNextSkillEffectsWhenValidationWasSuccessful()
    {
        // Given
        var duration = 1000L;
        var nextSkillEffect = new SkillEffect();

        var connectionId = "connection-id";
        var effect = new SkillEffect
        {
            Duration = duration,
            Next = new List<SkillEffect>
            {
                nextSkillEffect,
            }.ToArray(),
        };
        var caster = new DefaultEntity();
        var target = new DefaultEntity();
        var skill = new SkillInstance();
        var targetPosition = new Vector3(2, 2, 2);

        var now = DateTime.UtcNow;
        var state = new Dictionary<string, object>();
        var effectResponse = new SkillEffectScriptResponse
        {
            ActionList = new List<ClientSkillAction>(),
            State = state,
        };

        var expected = new AddServerActionEvent(
            now.AddMilliseconds(
                duration
            ),
            new RunSkillEffectWithTargetOfEntityEvent
            {
                ConnectionId = connectionId,
                SkillEffect = nextSkillEffect,
                Caster = caster,
                Target = target,
                Skill = skill,
                State = state
            }
        );

        var loggerMock = new Mock<ILogger<RunSkillEffectWithTargetOfEntityEventHandler>>();
        var mediatorMock = new Mock<IMediator>();
        var dateTimeMock = new Mock<IDateTimeService>();

        mediatorMock.Setup(
            mock => mock.Send(
                It.IsAny<RunSkillValidation>(),
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

        mediatorMock.Setup(
            mock => mock.Send(
                It.IsAny<RunServerScriptCommand>(),
                CancellationToken.None
            )
        ).ReturnsAsync(
            effectResponse
        );

        dateTimeMock.Setup(
            mock => mock.Now
        ).Returns(
            now
        );

        // When
        var handler = new RunSkillEffectWithTargetOfEntityEventHandler(
            loggerMock.Object,
            mediatorMock.Object,
            dateTimeMock.Object
        );
        await handler.Handle(
            new RunSkillEffectWithTargetOfEntityEvent
            {
                ConnectionId = connectionId,
                SkillEffect = effect,
                Caster = caster,
                Target = target,
                Skill = skill,
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
    public async Task ShouldPublishClientActionGenericToAllEventWhenRunningEffectScript()
    {
        // Given
        var duration = 1000L;
        var nextSkillEffect = new SkillEffect();

        var connectionId = "connection-id";
        var effect = new SkillEffect
        {
            Duration = duration,
            Next = new List<SkillEffect>
            {
                nextSkillEffect,
            }.ToArray(),
        };
        var caster = new DefaultEntity();
        var target = new DefaultEntity();
        var skill = new SkillInstance();
        var targetPosition = new Vector3(2, 2, 2);

        var clientSkillAction = new ClientSkillActionEvent
        {
            Action = "action",
            Data = new { },
        };

        var expected = new ClientActionGenericToAllEvent(
            "RunSkillAction",
            clientSkillAction
        );

        var now = DateTime.UtcNow;
        var state = new Dictionary<string, object>();
        var effectResponse = new SkillEffectScriptResponse
        {
            ActionList = new List<ClientSkillAction>
            {
                clientSkillAction,
            },
            State = state,
        };

        var loggerMock = new Mock<ILogger<RunSkillEffectWithTargetOfEntityEventHandler>>();
        var mediatorMock = new Mock<IMediator>();
        var dateTimeMock = new Mock<IDateTimeService>();

        mediatorMock.Setup(
            mock => mock.Send(
                It.IsAny<RunSkillValidation>(),
                CancellationToken.None
            )
        ).ReturnsAsync(
            new List<SkillValidatorResponse>()
        );

        mediatorMock.Setup(
            mock => mock.Send(
                It.IsAny<RunServerScriptCommand>(),
                CancellationToken.None
            )
        ).ReturnsAsync(
            effectResponse
        );

        dateTimeMock.Setup(
            mock => mock.Now
        ).Returns(
            now
        );

        // When
        var handler = new RunSkillEffectWithTargetOfEntityEventHandler(
            loggerMock.Object,
            mediatorMock.Object,
            dateTimeMock.Object
        );
        await handler.Handle(
            new RunSkillEffectWithTargetOfEntityEvent
            {
                ConnectionId = connectionId,
                SkillEffect = effect,
                Caster = caster,
                Target = target,
                Skill = skill,
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
    public async Task ShouldNotErrorWhenEffectNextSkillEffectListIsNull()
    {
        // Given
        var duration = 1000L;

        var connectionId = "connection-id";
        var effect = new SkillEffect
        {
            Duration = duration,
            Next = null,
        };
        var caster = new DefaultEntity();
        var target = new DefaultEntity();
        var skill = new SkillInstance();
        var targetPosition = new Vector3(2, 2, 2);

        var clientSkillAction = new ClientSkillActionEvent
        {
            Action = "action",
            Data = new { },
        };

        var expected = new ClientActionGenericToAllEvent(
            "RunSkillAction",
            clientSkillAction
        );

        var now = DateTime.UtcNow;
        var state = new Dictionary<string, object>();
        var effectResponse = new SkillEffectScriptResponse
        {
            ActionList = new List<ClientSkillAction>
            {
                clientSkillAction,
            },
            State = state,
        };

        var loggerMock = new Mock<ILogger<RunSkillEffectWithTargetOfEntityEventHandler>>();
        var mediatorMock = new Mock<IMediator>();
        var dateTimeMock = new Mock<IDateTimeService>();

        mediatorMock.Setup(
            mock => mock.Send(
                It.IsAny<RunSkillValidation>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new List<SkillValidatorResponse>()
            );

        mediatorMock.Setup(
            mock => mock.Send(
                It.IsAny<RunServerScriptCommand>(),
                CancellationToken.None
            )
        ).ReturnsAsync(
            effectResponse
        );

        dateTimeMock.Setup(
            mock => mock.Now
            ).Returns(
                now
            );

        // When
        var handler = new RunSkillEffectWithTargetOfEntityEventHandler(
            loggerMock.Object,
            mediatorMock.Object,
            dateTimeMock.Object
        );
        await handler.Handle(
                new RunSkillEffectWithTargetOfEntityEvent
                {
                    ConnectionId = connectionId,
                    SkillEffect = effect,
                    Caster = caster,
                    Target = target,
                    Skill = skill,
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
    public async Task ShouldRunSkillEffectWithTargetOfEntityEventForAllFailedSkillEffectsWhenValidationScriptsReturnAnyFailure()
    {
        // Given
        var duration = 1000L;
        var failedSkillEffect = new SkillEffect();

        var connectionId = "connection-id";
        var effect = new SkillEffect
        {
            Duration = duration,
            FailedList = new List<SkillEffect>
            {
                failedSkillEffect,
            }.ToArray(),
        };
        var caster = new DefaultEntity();
        var target = new DefaultEntity();
        var skill = new SkillInstance();
        var targetPosition = new Vector3(2, 2, 2);

        var clientSkillAction = new ClientSkillActionEvent
        {
            Action = "action",
            Data = new { },
        };

        var now = DateTime.UtcNow;
        var state = new Dictionary<string, object>();
        var effectResponse = new SkillEffectScriptResponse
        {
            ActionList = new List<ClientSkillAction>
            {
                clientSkillAction,
            },
            State = state,
        };
        var validationResponse = new SkillValidatorResponse
        {
            Success = false,
        };

        var expectedConnectionId = connectionId;
        var expectedFailedSkillEffect = failedSkillEffect;
        var expectedCaster = caster;
        var expectedTarget = target;
        var expectedSkill = skill;
        var expectedTargetPosition = targetPosition;
        var expectedState = new Dictionary<string, object>
        {
            {
                "Code",
                "effect_validation_failed"
            },
            {
                "ValidationResponse",
                validationResponse
            },
            {
                "Effect",
                effect
            }
        };

        var loggerMock = new Mock<ILogger<RunSkillEffectWithTargetOfEntityEventHandler>>();
        var mediatorMock = new Mock<IMediator>();
        var dateTimeMock = new Mock<IDateTimeService>();

        mediatorMock.Setup(
            mock => mock.Send(
                It.IsAny<RunSkillValidation>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new List<SkillValidatorResponse>
                {
                    validationResponse,
                }
            );

        mediatorMock.Setup(
            mock => mock.Send(
                It.IsAny<RunServerScriptCommand>(),
                CancellationToken.None
            )
        ).ReturnsAsync(
            effectResponse
        );

        dateTimeMock.Setup(
            mock => mock.Now
            ).Returns(
                now
            );

        // When
        var handler = new RunSkillEffectWithTargetOfEntityEventHandler(
            loggerMock.Object,
            mediatorMock.Object,
            dateTimeMock.Object
        );
        var actual = default(RunSkillEffectWithTargetOfEntityEvent);
        mediatorMock.Setup(
            mock => mock.Publish(
                It.IsAny<RunSkillEffectWithTargetOfEntityEvent>(),
                CancellationToken.None
            )
        ).Callback<INotification, CancellationToken>(
            (notification, _) =>
            {
                actual = (RunSkillEffectWithTargetOfEntityEvent)notification;
            }
        );
        await handler.Handle(
            new RunSkillEffectWithTargetOfEntityEvent
            {
                ConnectionId = connectionId,
                SkillEffect = effect,
                Caster = caster,
                Target = target,
                Skill = skill,
                TargetPosition = targetPosition,
            },
            CancellationToken.None
        );

        // Then
        actual.ConnectionId
            .Should().Be(expectedConnectionId);
        actual.SkillEffect
            .Should().Be(expectedFailedSkillEffect);
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
    public async Task ShouldNotErrorWhenEffectFailedSkillEffectListIsNull()
    {
        // Given
        var connectionId = "connection-id";
        var effect = new SkillEffect
        {
            FailedList = null,
        };
        var caster = new DefaultEntity();
        var target = new DefaultEntity();
        var skill = new SkillInstance();
        var targetPosition = new Vector3(2, 2, 2);

        var loggerMock = new Mock<ILogger<RunSkillEffectWithTargetOfEntityEventHandler>>();
        var mediatorMock = new Mock<IMediator>();
        var dateTimeMock = new Mock<IDateTimeService>();
        mediatorMock.Setup(
            mock => mock.Send(
                It.IsAny<RunSkillValidation>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new List<SkillValidatorResponse>
                {
                    new SkillValidatorResponse
                    {
                        Success = false,
                    }
                }
            );

        // When
        var handler = new RunSkillEffectWithTargetOfEntityEventHandler(
            loggerMock.Object,
            mediatorMock.Object,
            dateTimeMock.Object
        );
        await handler.Handle(
            new RunSkillEffectWithTargetOfEntityEvent
            {
                ConnectionId = connectionId,
                SkillEffect = effect,
                Caster = caster,
                Target = target,
                Skill = skill,
                TargetPosition = targetPosition,
            },
            CancellationToken.None
        );

        // Then
        mediatorMock.Verify(
            mock => mock.Send(
                It.IsAny<RunServerScriptCommand>(),
                CancellationToken.None
            ),
            Times.Never()
        );
        mediatorMock.Verify(
            mock => mock.Publish(
                It.IsAny<AddServerActionEvent>(),
                CancellationToken.None
            ),
            Times.Never()
        );
        mediatorMock.Verify(
            mock => mock.Publish(
                It.IsAny<AddServerActionEvent>(),
                CancellationToken.None
            ),
            Times.Never()
        );
    }

    [Fact]
    public async Task ShouldPublishClientActionGenericToSingleEventWhenEffectResponseActionListContainsToConnectionActionEvent()
    {
        // Given
        var duration = 1000L;
        var failedSkillEffect = new SkillEffect();

        var connectionId = "connection-id";
        var effect = new SkillEffect
        {
            Duration = duration,
            FailedList = new List<SkillEffect>
            {
                failedSkillEffect,
            }.ToArray(),
        };
        var caster = new DefaultEntity();
        var target = new DefaultEntity();
        var skill = new SkillInstance();
        var targetPosition = new Vector3(2, 2, 2);

        var clientSkillAction = new ClientActionRunSkillActionForConnectionEvent(
            connectionId,
            "ForConnectionAction",
            new { }
        );

        var now = DateTime.UtcNow;
        var state = new Dictionary<string, object>();
        var effectResponse = new SkillEffectScriptResponse
        {
            ActionList = new List<ClientSkillAction>
            {
                clientSkillAction,
            },
            State = state,
        };
        var validationResponse = new SkillValidatorResponse
        {
            Success = true,
        };

        var expectedConnectionId = connectionId;
        var expectedAction = "RunSkillAction";
        var expectedData = clientSkillAction;

        var loggerMock = new Mock<ILogger<RunSkillEffectWithTargetOfEntityEventHandler>>();
        var mediatorMock = new Mock<IMediator>();
        var dateTimeMock = new Mock<IDateTimeService>();

        mediatorMock.Setup(
            mock => mock.Send(
                It.IsAny<RunSkillValidation>(),
                CancellationToken.None
            )
        ).ReturnsAsync(
            new List<SkillValidatorResponse>
            {
                validationResponse,
            }
        );

        mediatorMock.Setup(
            mock => mock.Send(
                It.IsAny<RunServerScriptCommand>(),
                CancellationToken.None
            )
        ).ReturnsAsync(
            effectResponse
        );

        dateTimeMock.Setup(
            mock => mock.Now
            ).Returns(
                now
            );

        // When
        var handler = new RunSkillEffectWithTargetOfEntityEventHandler(
            loggerMock.Object,
            mediatorMock.Object,
            dateTimeMock.Object
        );
        var actual = default(ClientActionGenericToSingleEvent);
        mediatorMock.Setup(
            mock => mock.Publish(
                It.IsAny<ClientActionGenericToSingleEvent>(),
                CancellationToken.None
            )
        ).Callback<INotification, CancellationToken>(
            (notification, _) =>
            {
                actual = (ClientActionGenericToSingleEvent)notification;
            }
        );
        await handler.Handle(
            new RunSkillEffectWithTargetOfEntityEvent
            {
                ConnectionId = connectionId,
                SkillEffect = effect,
                Caster = caster,
                Target = target,
                Skill = skill,
                TargetPosition = targetPosition,
            },
            CancellationToken.None
        );

        // Then
        actual.Should().NotBeNull();
        actual.ConnectionId
            .Should().Be(expectedConnectionId);
        actual.Action
            .Should().Be(expectedAction);
        actual.Data
            .Should().Be(expectedData);
    }

    [Fact]
    public async Task ShouldNotPublishAnyClientActionWhenEffectResponseActionListIsNotValidType()
    {
        // Given
        var duration = 1000L;
        var failedSkillEffect = new SkillEffect();

        var connectionId = "connection-id";
        var effect = new SkillEffect
        {
            Duration = duration,
            FailedList = new List<SkillEffect>
            {
                failedSkillEffect,
            }.ToArray(),
        };
        var caster = new DefaultEntity();
        var target = new DefaultEntity();
        var skill = new SkillInstance();
        var targetPosition = new Vector3(2, 2, 2);

        var clientSkillAction = new ClientSkillActionMock();

        var now = DateTime.UtcNow;
        var state = new Dictionary<string, object>();
        var effectResponse = new SkillEffectScriptResponse
        {
            ActionList = new List<ClientSkillAction>
            {
                clientSkillAction,
            },
            State = state,
        };
        var validationResponse = new SkillValidatorResponse
        {
            Success = true,
        };

        var expectedConnectionId = connectionId;
        var expectedData = clientSkillAction;

        var loggerMock = new Mock<ILogger<RunSkillEffectWithTargetOfEntityEventHandler>>();
        var mediatorMock = new Mock<IMediator>();
        var dateTimeMock = new Mock<IDateTimeService>();

        mediatorMock.Setup(
            mock => mock.Send(
                It.IsAny<RunSkillValidation>(),
                CancellationToken.None
            )
        ).ReturnsAsync(
            new List<SkillValidatorResponse>
            {
                validationResponse,
            }
        );

        mediatorMock.Setup(
            mock => mock.Send(
                It.IsAny<RunServerScriptCommand>(),
                CancellationToken.None
            )
        ).ReturnsAsync(
            effectResponse
        );

        dateTimeMock.Setup(
            mock => mock.Now
            ).Returns(
                now
            );

        // When
        var handler = new RunSkillEffectWithTargetOfEntityEventHandler(
            loggerMock.Object,
            mediatorMock.Object,
            dateTimeMock.Object
        );
        await handler.Handle(
            new RunSkillEffectWithTargetOfEntityEvent
            {
                ConnectionId = connectionId,
                SkillEffect = effect,
                Caster = caster,
                Target = target,
                Skill = skill,
                TargetPosition = targetPosition,
            },
            CancellationToken.None
        );

        // Then
        mediatorMock.Verify(
            mock => mock.Publish(
                It.IsAny<ClientActionGenericToAllEvent>(),
                CancellationToken.None
            ),
            Times.Never()
        );
        mediatorMock.Verify(
            mock => mock.Publish(
                It.IsAny<ClientActionGenericToSingleEvent>(),
                CancellationToken.None
            ),
            Times.Never()
        );
    }
    public struct ClientSkillActionMock : ClientSkillAction, IClientActionData
    {
        public string Action { get; set; }
        public object Data { get; set; }
    }
}
