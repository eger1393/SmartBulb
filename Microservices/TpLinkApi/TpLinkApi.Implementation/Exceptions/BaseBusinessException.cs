using System;

namespace TpLinkApi.Implementation.Exceptions
{
	public class BaseBusinessException : Exception
	{
		public BaseBusinessException()
		{
		}

		public BaseBusinessException(string message)
			: base(message)
		{
		}

		public BaseBusinessException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}