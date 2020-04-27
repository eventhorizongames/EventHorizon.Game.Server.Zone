namespace EventHorizon.Zone.System.ClientEntities.State
{
    using EventHorizon.Zone.System.ClientEntities.Model;
    using global::System.Collections.Generic;

    public interface ClientEntityRepository
    {
        ClientEntity Find(
            string id
        );
        void Add(
            ClientEntity clientEntityInstance
        );
        IEnumerable<ClientEntity> All();
        void Remove(
            string id
        );
    }
}