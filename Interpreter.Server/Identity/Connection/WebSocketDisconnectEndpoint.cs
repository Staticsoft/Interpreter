using Staticsoft.Contracts.Abstractions;
using Staticsoft.Interpreter.Contracts;
using Staticsoft.WsCommunication.Server.Abstractions;

namespace Staticsoft.Interpreter.Server;

public class WebSocketDisconnectEndpoint(
    PartitionUsersByConnections users,
    PartitionConnectionsByUsers connections
) : HttpEndpoint<WsDisconnectRequest, WebSocket.DisconnectResponse>
{
    readonly PartitionUsersByConnections Users = users;
    readonly PartitionConnectionsByUsers Connections = connections;

    public async Task<WebSocket.DisconnectResponse> Execute(WsDisconnectRequest request)
    {
        var user = await Users.Get(request.ConnectionId);

        await Task.WhenAll(
            Users.Remove(request.ConnectionId),
            Connections.Remove(user.Data.Id)
        );

        return new();
    }
}