using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Staticsoft.Interpreter.Server;

public class HttpContextIdentity(
	IHttpContextAccessor accessor
) : Identity
{
	readonly IHttpContextAccessor Accessor = accessor;

	public string UserId
		=> Sub.Value;

	Claim Sub
		=> Context.User.FindFirst("sub") ?? throw new NotSupportedException($"'sub' claim is missing");

	HttpContext Context
		=> Accessor.HttpContext ?? throw new NotSupportedException("Cannot access HttpContext");
}
