using ContextNLog = NLog.GlobalDiagnosticsContext;

namespace Common.Logging.NLog
{
    /// <summary>
    /// A global context for logger variables
    /// </summary>
    public class NLogGlobalVariablesContext : IVariablesContext
    {
        /// <summary>
        /// Sets the value of a new or existing variable within the global context
        /// </summary>
        /// <param name="key">The key of the variable that is to be added</param>
        /// <param name="value">The value to add</param>
        public void Set(string key, object value)
        {
            ContextNLog.Set(key, value != null ? value.ToString() : null);
        }

        /// <summary>
        /// Gets the value of a variable within the global context
        /// </summary>
        /// <param name="key">The key of the variable to get</param>
        /// <returns>The value or null if not found</returns>
        public object Get(string key)
        {
            return ContextNLog.Get(key);
        }

        /// <summary>
        /// Checks if a variable is set within the global context
        /// </summary>
        /// <param name="key">The key of the variable to check for</param>
        /// <returns>True if the variable is set</returns>
        public bool Contains(string key)
        {
            return ContextNLog.Contains(key);
        }

        /// <summary>
        /// Removes a variable from the global context by key
        /// </summary>
        /// <param name="key">The key of the variable to remove</param>
        public void Remove(string key)
        {
            ContextNLog.Remove(key);
        }

        /// <summary>
        /// Clears the global context variables
        /// </summary>
        public void Clear()
        {
            ContextNLog.Clear();
        }
    }
}
