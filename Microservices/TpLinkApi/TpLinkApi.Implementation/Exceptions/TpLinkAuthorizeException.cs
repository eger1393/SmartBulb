using System;

namespace TpLinkApi.Implementation.Exceptions
{
	public class TpLinkAuthorizeException: BaseBusinessException
	{
		public TpLinkAuthorizeException()
		{
		}

		public TpLinkAuthorizeException(string message)
			: base(message)
		{
		}

		public TpLinkAuthorizeException(string message, Exception inner)
			: base(message, inner)
		{
		}
    }
}