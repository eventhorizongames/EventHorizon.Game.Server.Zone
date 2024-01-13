namespace EventHorizon.Zone.Core.Reporter.Tests.Writer;

using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Zone.Core.Reporter.Model;
using EventHorizon.Zone.Core.Reporter.Writer;

using MediatR;

using Moq;

using Xunit;

using static EventHorizon.Zone.Core.Reporter.Model.ReporterSettings;

public class WriteReportHandlerTests
{
    [Theory]
    [ClassData(typeof(TestDataGenerator))]
    public async Task ShouldTestBaseOnGeneratedData(
        TestObject testObject
    )
    {
        // Given
        var elasticsearchSettingsMock = new Mock<ElasticsearchReporterSettings>();

        var settingsMock = new Mock<ReporterSettings>();
        var mediatorMock = new Mock<IMediator>();

        settingsMock.Setup(
            mock => mock.Elasticsearch
        ).Returns(
            elasticsearchSettingsMock.Object
        );

        elasticsearchSettingsMock.Setup(
            mock => mock.IsEnabled
        ).Returns(
            testObject.IsElasticSearchEnabled
        );

        settingsMock.Setup(
            mock => mock.IsWriteToFileEnabled
        ).Returns(
            testObject.IsWriteToFileEnabled
        );

        // When
        var handler = new WriteReportHandler(
            mediatorMock.Object,
            settingsMock.Object
        );
        await handler.Handle(
            new WriteReport(
                testObject.Report
            ),
            CancellationToken.None
        );

        // Then
        foreach (var expected in testObject.Expected)
        {
            mediatorMock.Verify(
                mock => mock.Send(
                    expected,
                    CancellationToken.None
                )
            );
        }
    }

    public class TestDataGenerator : IEnumerable<object[]>
    {
        private static Report REPORT = default;
        private readonly List<object[]> _data = new()
        {
            new object[]
            {
                new TestObject
                {
                    IsElasticSearchEnabled = false,
                    IsWriteToFileEnabled = false,
                    Report = REPORT,
                    Expected = new List<IRequest>(),
                },
            },
            new object[]
            {
                new TestObject
                {
                    IsElasticSearchEnabled = true,
                    IsWriteToFileEnabled = false,
                    Report = REPORT,
                    Expected = new List<IRequest>
                    {
                        new WriteReportToElasticsearch(
                            REPORT
                        ),
                    }
                },
            },
            new object[]
            {
                new TestObject
                {
                    IsElasticSearchEnabled = false,
                    IsWriteToFileEnabled = true,
                    Report = REPORT,
                    Expected = new List<IRequest>
                    {
                        new WriteReportToFile(
                            REPORT
                        ),
                    },
                },
            },
            new object[]
            {
                new TestObject
                {
                    IsElasticSearchEnabled = true,
                    IsWriteToFileEnabled = true,
                    Report = REPORT,
                    Expected = new List<IRequest>
                    {
                        new WriteReportToElasticsearch(
                            REPORT
                        ),
                        new WriteReportToFile(
                            REPORT
                        ),
                    },
                },
            },
        };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class TestObject
    {
        public bool IsElasticSearchEnabled { get; set; }
        public bool IsWriteToFileEnabled { get; set; }
        public Report Report { get; set; }
        public IList<IRequest> Expected { get; set; }
    }
}
