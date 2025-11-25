namespace PLCompliant.Utilities
{
    /// <summary>
    /// Static class for thread helpers
    /// </summary>
    public static class ThreadUtilities
    {
        /// <summary>
        /// Create a background thread. This is basically a wrapper for "new Thread"
        /// </summary>
        /// <param name="start">The threadstart delegate</param>
        /// <returns>The created thread</returns>
        public static Thread CreateBackgroundThread(ThreadStart start)
        {
            Thread t = new Thread(start);
            t.IsBackground = true;
            return t;
        }
    }
}
