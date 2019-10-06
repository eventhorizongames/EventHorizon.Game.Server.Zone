using System.Reflection;
using EventHorizon.Zone.Core.Entity.State;
using EventHorizon.Zone.Core.Model.Id;
using MediatR;
using Moq;
using Xunit.Sdk;

namespace EventHorizon.Tests.TestUtils
{
    public class CleanupInMemoryEntityRepositoryAttribute : BeforeAfterTestAttribute
    {
        public override void Before(MethodInfo methodUnderTest)
        {
            var entityRepository = new InMemoryEntityRepository(
                new Mock<IMediator>().Object,
                new Mock<IdPool>().Object
            );
            foreach (var entity in entityRepository.All().GetAwaiter().GetResult())
            {
                entityRepository.Remove(
                    entity.Id
                );
            }
        }
    }
}