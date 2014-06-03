using ContextLog4Net = log4net.ThreadContext;

namespace Common.Logging.Log4Net
{
    /// <summary>
    /// A global context for logger variables
    /// </summary>
    public class Log4NetThreadVariablesContext : IVariablesContext
    {
        /// <summary>
        /// Sets the value of a new or existing variable within the global context
        /// </summary>
        /// <param name="key">The key of the variable that is to be added</param>
        /// <param name="value">The value to add</param>
        public void Set(string key, object value)
        {
            ContextLog4Net.Properties[key] = value;
        }

        /// <summary>
        /// Gets the value of a variable within the global context
        /// </summary>
        /// <param name="key">The key of the variable to get</param>
        /// <returns>The value or null if not found</returns>
        public object Get(string key)
        {
            return ContextLog4Net.Properties[key];
        }

        /// <summary>
        /// Checks if a variable is set within the global context
        /// </summary>
        /// <param name="key">The key of the variable to check for</param>
        /// <returns>True if the variable is set</returns>
        public bool Contains(string key)
        {
            return ContextLog4Net.Properties[key] != null;
        }

        /// <summary>
        /// Removes a variable from the global context by key
        /// </summary>
        /// <param name="key">The key of the variable to remove</param>
        public void Remove(string key)
        {
            ContextLog4Net.Properties.Remove(key);
        }

        /// <summary>
        /// Clears the global context variables
        /// </summary>
        public void Clear()
        {
            ContextLog4Net.Properties.Clear();
        }
    }
}
