namespace Common.Logging.Simple
{
    /// <summary>
    /// A null-functionality implementation of <see cref="IVariablesContext" />
    /// </summary>
    public class NoOpVariablesContext : IVariablesContext
    {
        /// <summary>
        /// Sets the value of a new or existing variable within the global context
        /// </summary>
        /// <param name="key">The key of the variable that is to be added</param>
        /// <param name="value">The value to add</param>
        public void Set(string key, object value)
        {
        }

        /// <summary>
        /// Gets the value of a variable within the global context
        /// </summary>
        /// <param name="key">The key of the variable to get</param>
        /// <returns>The value or null if not found</returns>
        public object Get(string key)
        {
            return null;
        }

        /// <summary>
        /// Checks if a variable is set within the global context
        /// </summary>
        /// <param name="key">The key of the variable to check for</param>
        /// <returns>True if the variable is set</returns>
        public bool Contains(string key)
        {
            return false;
        }

        /// <summary>
        /// Removes a variable from the global context by key
        /// </summary>
        /// <param name="key">The key of the variable to remove</param>
        public void Remove(string key)
        {
        }

        /// <summary>
        /// Clears the global context variables
        /// </summary>
        public void Clear()
        {
        }
    }
}
