using Staticsoft.Contracts.Abstractions;
using Staticsoft.Interpreter.Contracts;
using Staticsoft.WsCommunication.Server.Abstractions;

namespace Staticsoft.Interpreter.Server.Tests;

public class GracefulDisconnectEndpoint(
	HttpEndpoint<WsDisconnectRequest, WebSocket.DisconnectResponse> decorated,
	GracefullDisconnect disconnect
) : HttpEndpoint<WsDisconnectRequest, WebSocket.DisconnectResponse>
{
	readonly HttpEndpoint<WsDisconnectRequest, WebSocket.DisconnectResponse> Decorated = decorated;
	readonly GracefullDisconnect Disconnect = disconnect;


	public async Task<WebSocket.DisconnectResponse> Execute(WsDisconnectRequest request)
	{
		var response = await Decorated.Execute(request);
		Disconnect.NotifyDisconnected();
		return response;
	}
}