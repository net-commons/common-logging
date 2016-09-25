using System;
using ContextLog4Net = log4net.ThreadContext;

namespace Common.Logging.Log4Net
{
	/// <summary>
	/// A global context for logger variables
	/// </summary>
	public class Log4NetNestedThreadVariablesContext : INestedVariablesContext
	{
		private const string NESTED_STACK_NAME = "NDC";

		/// <summary>Pushes a new context message into this stack.</summary>
		/// <param name="text">The new context message text.</param>
		/// <returns>
		/// An <see cref="T:System.IDisposable" /> that can be used to clean up the context stack.
		/// </returns>
		public IDisposable Push(string text)
		{
			return ContextLog4Net.Stacks[NESTED_STACK_NAME].Push(text);
		}

		/// <summary>Removes the top context from this stack.</summary>
		/// <returns>The message in the context that was removed from the top of this stack.</returns>
		public string Pop()
		{
			return ContextLog4Net.Stacks[NESTED_STACK_NAME].Pop();
		}

		/// <summary>
		/// Remove all items from nested context
		/// </summary>
		public void Clear()
		{
			ContextLog4Net.Stacks[NESTED_STACK_NAME].Clear();
		}

		/// <summary>
		/// Returns true if there is at least one item in the nested context; false, if empty
		/// </summary>
		public bool HasItems => ContextLog4Net.Stacks[NESTED_STACK_NAME].Count > 0;
	}
}