namespace Common.Logging
{
    /// <summary>
    /// A context for logger variables
    /// </summary>
    public interface IVariablesContext
    {
        /// <summary>
        /// Sets the value of a new or existing variable within the global context
        /// </summary>
        /// <param name="key">The key of the variable that is to be added</param>
        /// <param name="value">The value to add</param>
        void Set(string key, object value);

        /// <summary>
        /// Gets the value of a variable within the global context
        /// </summary>
        /// <param name="key">The key of the variable to get</param>
        /// <returns>The value or null if not found</returns>
        object Get(string key);

        /// <summary>
        /// Checks if a variable is set within the global context
        /// </summary>
        /// <param name="key">The key of the variable to check for</param>
        /// <returns>True if the variable is set</returns>
        bool Contains(string key);

        /// <summary>
        /// Removes a variable from the global context by key
        /// </summary>
        /// <param name="key">The key of the variable to remove</param>
        void Remove(string key);

        /// <summary>
        /// Clears the global context variables
        /// </summary>
        void Clear();
    }
}
