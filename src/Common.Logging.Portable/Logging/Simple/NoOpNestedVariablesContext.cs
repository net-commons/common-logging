using System;

namespace Common.Logging.Simple
{
	/// <summary>
	/// A null-functionality implementation of <see cref="INestedVariablesContext" />
	/// </summary>
	public class NoOpNestedVariablesContext : INestedVariablesContext
	{
		/// <summary>
		/// Ignore the push
		/// </summary>
		/// <param name="text"></param>
		/// <returns>A NoOpDisposable</returns>
		public IDisposable Push(string text)
		{
			return new NoOpDisposable();
		}

		/// <summary>
		/// Nothing to pop
		/// </summary>
		/// <returns>a Null string value</returns>
		public string Pop()
		{
			return null;
		}

		/// <summary>
		/// Clears the context variables
		/// </summary>
		public void Clear()
		{}

		/// <summary>
		/// 
		/// </summary>
		public bool HasItems { get; private set; }

		private class NoOpDisposable : IDisposable
		{
			public void Dispose()
			{}
		}
	}
}