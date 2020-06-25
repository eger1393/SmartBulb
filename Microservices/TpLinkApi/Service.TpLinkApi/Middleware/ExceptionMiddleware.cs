using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TpLinkApi.Implementation.Exceptions;

namespace Service.TpLinkApi.Middleware
{
	public static class ExceptionMiddleware
	{
		public static async Task UnauthorizedExceptionMiddleware(HttpContext context, Func<Task> next)
		{
			try
			{
				await next.Invoke();
			}
			catch (BaseBusinessException ex)
			{
				switch (ex)
				{
					case TpLinkAuthorizeException authorizeException:
					{
						context.Response.StatusCode = 401;
						await context.Response.WriteAsync(ex.Message);
						break;
					}
					default:
					{
						context.Response.StatusCode = 500;
						await context.Response.WriteAsync(ex.Message);
						break;
					}
				}
			}
		}
	}
}