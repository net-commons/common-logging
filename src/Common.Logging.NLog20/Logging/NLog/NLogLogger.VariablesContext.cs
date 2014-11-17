namespace Common.Logging.NLog
{
    public partial class NLogLogger
    {
        /// <summary>
        /// Returns the global context for variables
        /// </summary>
        public override IVariablesContext GlobalVariablesContext
        {
            get { return new NLogGlobalVariablesContext(); }
        }

        /// <summary>
        /// Returns the thread-specific context for variables
        /// </summary>
        public override IVariablesContext ThreadVariablesContext
        {
            get { return new NLogThreadVariablesContext(); }
        }
    }
}
