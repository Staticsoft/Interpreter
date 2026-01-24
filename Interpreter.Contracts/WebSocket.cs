using Staticsoft.Contracts.Abstractions;
using Staticsoft.WsCommunication.Server.Abstractions;

namespace Staticsoft.Interpreter.Contracts;

public class WebSocket(
    HttpEndpoint<WsConnectRequest, WebSocket.ConnectResponse> connect,
    HttpEndpoint<WsDisconnectRequest, WebSocket.DisconnectResponse> disconnect
)
{
    [Endpoint(HttpMethod.Post)]
    public HttpEndpoint<WsConnectRequest, ConnectResponse> Connect { get; } = connect;

    [Endpoint(HttpMethod.Post)]
    public HttpEndpoint<WsDisconnectRequest, DisconnectResponse> Disconnect { get; } = disconnect;


    public class ConnectResponse { }

    public class DisconnectResponse { }
}
