namespace EventHorizon.Zone.Core.Reporter.Writer
{
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Zone.Core.Reporter.Model;

    using MediatR;

    public class WriteReportHandler
        : IRequestHandler<WriteReport>
    {
        private readonly IMediator _mediator;
        private readonly ReporterSettings _settings;

        public WriteReportHandler(
            IMediator mediator,
            ReporterSettings settings
        )
        {
            _mediator = mediator;
            _settings = settings;
        }

        public async Task<Unit> Handle(
            WriteReport request,
            CancellationToken cancellationToken
        )
        {
            if (_settings.Elasticsearch.IsEnabled)
            {
                await _mediator.Send(
                    new WriteReportToElasticsearch(
                        request.Report
                    ),
                    cancellationToken
                );
            }
            if (_settings.IsWriteToFileEnabled)
            {
                await _mediator.Send(
                    new WriteReportToFile(
                        request.Report
                    ),
                    cancellationToken
                );
            }

            return Unit.Value;
        }
    }
}
