using System;
using System.Collections.Generic;

namespace Common.Logging
{
#if PORTABLE

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
    }

#else
#endif
}