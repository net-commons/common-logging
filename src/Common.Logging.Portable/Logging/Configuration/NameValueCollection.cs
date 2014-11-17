using System;
using System.Collections.Generic;

namespace Common.Logging.Configuration
{
    /// <summary>
    /// Substitute NameValueCollection in System.Collections.Specialized.
    /// </summary>
    public class NameValueCollection : Dictionary<string, string>
    {
        /// <summary>
        /// Creates a new instance of <seealso cref="NameValueCollection">NameValueCollection</seealso>.
        /// </summary>
        public NameValueCollection()
            : base(StringComparer.OrdinalIgnoreCase)
        {

        }

        /// <summary>
        /// Gets the values (only a single one) for the specified key (configuration name)
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>an array with one value, or null if no value exist</returns>
        public string[] GetValues(string key)
        {
            string value;
            if (this.TryGetValue(key, out value) && value != null)
            {
                return new[] { value };
            }
            return null;
        }

        /// <summary>
        /// Gets or sets the value with the specified key.
        /// </summary>
        /// <value>
        /// The value corrsponding to the key, or null if no value exist
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns>value store for the key</returns>
        public new string this[string key]
        {
            get
            {
                string value;
                if (base.TryGetValue(key, out value))
                    return value;
                else
                    return null;
            }
            set
            {
                base[key] = value;
            }
        }
    }
}