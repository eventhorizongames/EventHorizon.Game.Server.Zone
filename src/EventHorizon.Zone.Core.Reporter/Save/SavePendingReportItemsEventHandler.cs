namespace EventHorizon.Zone.Core.Reporter.Save
{
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Zone.Core.Reporter.Model;
    using EventHorizon.Zone.Core.Reporter.Writer;

    using MediatR;

    public class SavePendingReportItemsEventHandler
        : INotificationHandler<SavePendingReportItemsEvent>
    {
        private readonly IMediator _mediator;
        private readonly ReportRepository _repository;

        public SavePendingReportItemsEventHandler(
            IMediator mediator,
            ReportRepository repository
        )
        {
            _mediator = mediator;
            _repository = repository;
        }

        public async Task Handle(
            SavePendingReportItemsEvent notification,
            CancellationToken cancellationToken
        )
        {
            var reports = _repository.TakeAll();
            foreach (var report in reports)
            {
                await _mediator.Send(
                    new WriteReport(
                        report
                    ),
                    cancellationToken
                );
            }
        }
    }
}
