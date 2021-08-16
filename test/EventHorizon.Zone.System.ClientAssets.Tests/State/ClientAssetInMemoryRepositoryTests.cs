namespace EventHorizon.Zone.System.ClientAssets.Tests.State
{
    using EventHorizon.Test.Common.Attributes;
    using EventHorizon.Zone.System.ClientAssets.Model;
    using EventHorizon.Zone.System.ClientAssets.State;

    using FluentAssertions;

    using global::System.Collections.Generic;

    using Xunit;

    public class ClientAssetInMemoryRepositoryTests
    {
        [Theory, AutoMoqData]
        public void ShouldSupportCreateReadUpdateDeleteWhenRepositoryIsUsed(
            // Given
            ClientAsset clientAsset,
            ClientAsset setClientAsset
        )
        {
            setClientAsset.Id = clientAsset.Id;

            // When
            var repository = new ClientAssetInMemoryRepository();

            // Then
            // Add to Repository
            repository.Add(
                clientAsset
            );
            // Read by Id
            var actual = repository.Get(
                clientAsset.Id
            );
            actual.HasValue.Should().BeTrue();
            actual.Value.Should().Be(clientAsset);
            // Update with same Id
            repository.Set(
                setClientAsset
            );
            // Get by Id
            actual = repository.Get(
                clientAsset.Id
            );
            // Validate Id the same, but instance is updated
            actual.Value.Should().NotBe(clientAsset);
            actual.Value.Should().Be(setClientAsset);
            // Validate Get All
            repository.All().Should().BeEquivalentTo(
                new List<ClientAsset>
                {
                    setClientAsset
                }
            );
            // Validate Delete
            repository.Delete(
                setClientAsset.Id
            );
            // Validate All is Empty
            repository.All().Should().BeEmpty();
            // Get by Id returns empty Option
            actual = repository.Get(
                setClientAsset.Id
            );
            actual.HasValue.Should().BeFalse();
        }
    }
}
