using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Gate.Services
{
	public interface IForwardRequestService
	{
		Task<HttpResponseMessage> ForwardRequest(HttpContext context, Uri targetUri);
		Task ForwardResponse(HttpContext context, HttpResponseMessage response);
	}

	public class ForwardRequestService : IForwardRequestService
	{
		private readonly HttpClient _httpClient;

		public ForwardRequestService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<HttpResponseMessage> ForwardRequest(HttpContext context, Uri targetUri)
		{
			var targetRequestMessage = CreateTargetMessage(context, new Uri(targetUri, context.Request.Path));
			return await _httpClient.SendAsync(targetRequestMessage, HttpCompletionOption.ResponseHeadersRead,
				context.RequestAborted);
		}

		public async Task ForwardResponse(HttpContext context, HttpResponseMessage response)
		{
			context.Response.StatusCode = (int)response.StatusCode;
			CopyFromTargetResponseHeaders(context, response);
			await response.Content.CopyToAsync(context.Response.Body);
		}

		private HttpRequestMessage CreateTargetMessage(HttpContext context, Uri targetUri)
		{
			var requestMessage = new HttpRequestMessage();
			CopyFromOriginalRequestContentAndHeaders(context, requestMessage);

			requestMessage.RequestUri = targetUri;
			requestMessage.Headers.Host = targetUri.Host;
			requestMessage.Method = GetMethod(context.Request.Method);

			return requestMessage;
		}

		private void CopyFromOriginalRequestContentAndHeaders(HttpContext context, HttpRequestMessage requestMessage)
		{
			var requestMethod = context.Request.Method;

			if (!HttpMethods.IsGet(requestMethod) &&
				!HttpMethods.IsHead(requestMethod) &&
				!HttpMethods.IsDelete(requestMethod) &&
				!HttpMethods.IsTrace(requestMethod))
			{
				var streamContent = new StreamContent(context.Request.Body);
				requestMessage.Content = streamContent;
			}

			foreach (var header in context.Request.Headers)
			{
				requestMessage.Content?.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
			}

			if (requestMessage.Content == null)
			{
				foreach (var header in context.Request.Headers)
				{
					requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
				}
			}
		}

		private void CopyFromTargetResponseHeaders(HttpContext context, HttpResponseMessage responseMessage)
		{
			foreach (var header in responseMessage.Headers)
			{
				context.Response.Headers[header.Key] = header.Value.ToArray();
			}

			foreach (var header in responseMessage.Content.Headers)
			{
				context.Response.Headers[header.Key] = header.Value.ToArray();
			}

			context.Response.Headers.Remove("transfer-encoding");
		}

		private static HttpMethod GetMethod(string method)
		{
			if (HttpMethods.IsDelete(method)) return HttpMethod.Delete;
			if (HttpMethods.IsGet(method)) return HttpMethod.Get;
			if (HttpMethods.IsHead(method)) return HttpMethod.Head;
			if (HttpMethods.IsOptions(method)) return HttpMethod.Options;
			if (HttpMethods.IsPost(method)) return HttpMethod.Post;
			if (HttpMethods.IsPut(method)) return HttpMethod.Put;
			if (HttpMethods.IsTrace(method)) return HttpMethod.Trace;
			return new HttpMethod(method);
		}

	}
}