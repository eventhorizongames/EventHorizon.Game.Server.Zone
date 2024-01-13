namespace EventHorizon;

public static class ObjectExtensions
{
    public static T ValidateForNull<T>(
        this T? obj
    )
    {
#if DEBUG
        // We are disabling this check in release builds
        if (obj == null)
        {
            throw new System.NullReferenceException(
                "Found a Null"
            );
        }
#endif

        return obj!;
    }
}
