namespace EventHorizon.Zone.System.ClientEntities.State
{
    using global::System.Collections.Generic;
    using EventHorizon.Zone.System.ClientEntities.Model;

    public interface ClientEntityRepository
    {
        void Add(
            ClientEntity clientEntityInstance
        );
        IEnumerable<ClientEntity> All();
    }
}