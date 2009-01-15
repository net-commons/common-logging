namespace Common.Logging
{
    /// <summary>
    /// The type of a callback method used to obtain a message to be logged. 
    /// </summary>
    /// <remarks>
    /// Each <see cref="ILog"/> log method offers to pass in such a callback instead of the actual message.
    /// Using this methods has the advantage to defer possibly expensive message formatting until the message gets
    /// actually logged. If the message is not logged at all (e.g. due to <see cref="LogLevel"/> settings), 
    /// you won't have to pay the peformance penalty of creating the message.
    /// </remarks>
    /// <example>
    /// The example below demonstrates using callback style for creating the message:
    /// <code>
    /// Log.Debug( m=&gt;m(&quot;result is {0}&quot;, random.NextDouble()) );
    /// Log.Debug(delegate(m) { m(&quot;result is {0}&quot;, random.NextDouble()); });
    /// </code>
    /// </example>
    /// <seealso cref="ILog"/>
    public delegate string FormatMessageCallback(FormatMessageHandler fmcb);

    ///<summary>
    ///</summary>
    ///<param name="format"></param>
    ///<param name="args"></param>
    public delegate string FormatMessageHandler(string format, params object[] args);
}