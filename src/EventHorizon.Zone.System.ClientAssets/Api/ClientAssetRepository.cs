namespace EventHorizon.Zone.System.ClientAssets.State.Api
{
    using EventHorizon.Zone.System.ClientAssets.Model;

    using global::System.Collections.Generic;

    public interface ClientAssetRepository
    {
        IEnumerable<ClientAsset> All();

        Option<ClientAsset> Get(
            string id
        );

        void Add(
            ClientAsset clientAsset
        );

        void Set(
            ClientAsset clientAsset
        );

        void Delete(
            string id
        );
    }
}
