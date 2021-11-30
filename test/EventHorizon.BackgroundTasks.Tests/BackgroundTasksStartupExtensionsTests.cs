namespace EventHorizon.BackgroundTasks.Tests;

using System.Linq;

using EventHorizon.BackgroundTasks;
using EventHorizon.BackgroundTasks.Api;
using EventHorizon.BackgroundTasks.HostedService;
using EventHorizon.BackgroundTasks.State;
using EventHorizon.Test.Common.Utils;
using EventHorizon.Zone.Core.Model.DateTimeService;
using EventHorizon.Zone.Core.Model.DirectoryService;

using FluentAssertions;

using Microsoft.Extensions.Hosting;

using Xunit;

public class BackgroundTasksStartupExtensionsTests
{
    [Fact]
    public void TestAddI18n_ShouldReturnExpectedTypedImplementations()
    {
        //Given
        var serviceCollectionMock = new ServiceCollectionMock();

        //When
        serviceCollectionMock.AddBackgroundTasksServices();
        var actual = serviceCollectionMock.Services;

        //Then
        actual.Should().SatisfyRespectively(
            service =>
            {
                service.ServiceType.Should().BeAssignableTo<IHostedService>();
                service.ImplementationType.Should().BeAssignableTo<BackgroundTasksHostedService>();
            },
            service =>
            {
                service.ServiceType.Should().BeAssignableTo<BackgroundJobs>();
                service.ImplementationType.Should().BeAssignableTo<StandardBackgroundJobs>();
            }
        );
    }
}
