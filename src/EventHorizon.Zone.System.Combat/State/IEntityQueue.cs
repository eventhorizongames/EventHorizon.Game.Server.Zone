namespace EventHorizon.Zone.System.Combat.State
{
    using global::System.Threading.Tasks;

    public interface IEntityQueue<T>
    {
        Task Enqueue(T entity);
        Task<T?> Dequeue();
    }
}
