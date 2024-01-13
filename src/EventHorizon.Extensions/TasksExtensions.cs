namespace System.Threading.Tasks;

public static class TasksExtensions
{
    /// <summary>
    /// This will turn the object into a Task using the Task.FromResult.
    /// </summary>
    /// <param name="obj">The Object you want to wrap.</param>
    /// <typeparam name="T">The Type of the obj.</typeparam>
    /// <returns>The Task.FromResult of obj.</returns>
    public static Task<T> FromResult<T>(
        this T obj
    ) => Task.FromResult<T>(
        obj
    );
}
