namespace EventHorizon
{
    using System;

    public static class ObjectExtensions
    {
        public static T ValidateForNull<T>(
            this T? obj
        )
        {
#if DEBUG
            if (obj == null)
            {
                throw new NullReferenceException(
                    "Found a Null"
                );
            }
#endif

            return obj;
        }
    }
}
