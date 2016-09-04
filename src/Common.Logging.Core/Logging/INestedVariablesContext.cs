using System;

namespace Common.Logging
{
	/// <summary>
	/// A context for logger variables
	/// </summary>
	public interface INestedVariablesContext
	{
		/// <summary>Pushes a new context message into this stack.</summary>
		/// <param name="text">The new context message text.</param>
		/// <returns>
		/// An <see cref="T:System.IDisposable" /> that can be used to clean up the context stack.
		/// </returns>
		IDisposable Push(string text);

		/// <summary>Removes the top context from this stack.</summary>
		/// <returns>The message in the context that was removed from the top of this stack.</returns>
		string Pop();

		/// <summary>
		/// Remove all items from nested context
		/// </summary>
		void Clear();

		/// <summary>
		/// Returns true if there is at least one item in the nested context; false, if empty
		/// </summary>
		bool HasItems { get; }
	}
}