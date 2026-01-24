using Staticsoft.Contracts.Abstractions;
using Staticsoft.Interpreter.Contracts;
using Staticsoft.PartitionedStorage.Abstractions;
using Staticsoft.WsCommunication.Server.Abstractions;

namespace Staticsoft.Interpreter.Server;

public class WebSocketConnectEndpoint(
	Identity identity,
	PartitionUsersByConnections users,
	PartitionConnectionsByUsers connections
) : HttpEndpoint<WsConnectRequest, WebSocket.ConnectResponse>
{
	readonly Identity Identity = identity;
	readonly PartitionUsersByConnections Users = users;
	readonly PartitionConnectionsByUsers Connections = connections;

	public async Task<WebSocket.ConnectResponse> Execute(WsConnectRequest request)
	{
		try
		{
			await UpdateExistingUserConnectionMapping(request);
		}
		catch (PartitionedStorageItemNotFoundException)
		{
			await SaveNewUserConnectionMapping(request);
		}

		return new();
	}

	async Task UpdateExistingUserConnectionMapping(WsConnectRequest request)
	{
		var existingConnection = await Connections.Get(Identity.UserId);

		await Task.WhenAll(
			Users.Save(request.ConnectionId, new() { Id = Identity.UserId }),
			Connections.Save(Identity.UserId, new() { Id = request.ConnectionId }, existingConnection.Version)
		);

		await Users.Remove(existingConnection.Data.Id);

	}

	Task SaveNewUserConnectionMapping(WsConnectRequest request)
		=> Task.WhenAll(
			Users.Save(request.ConnectionId, new() { Id = Identity.UserId }),
			Connections.Save(Identity.UserId, new() { Id = request.ConnectionId })
		);
}