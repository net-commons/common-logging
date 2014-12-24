using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Logging.Configuration
{

#if !PORTABLE || NET20
    /// <summary>
    /// Helper class for working with NameValueCollection
    /// </summary>
    public static class NameValueCollectionHelper
    {
        /// <summary>
        /// Convert a <see cref="System.Collections.Specialized.NameValueCollection"/> into the corresponding 
        /// common logging equivalent <see cref="Common.Logging.Configuration.NameValueCollection"/>
        /// </summary>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        public static NameValueCollection ToCommonLoggingCollection(System.Collections.Specialized.NameValueCollection properties)
        {
            var result = new NameValueCollection();
            foreach (var key in properties.AllKeys)
                result.Add(key, properties.Get(key));
            return result;
        }
    }
#endif
}
