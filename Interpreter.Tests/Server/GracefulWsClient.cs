using Staticsoft.WsCommunication.Client.Abstractions;
using System.Threading.Channels;

namespace Staticsoft.Interpreter.Server.Tests;

public class GracefulWsClient(
	WsClient decorated,
	GracefullDisconnect disconnect
) : WsClient
{
	readonly WsClient Decorated = decorated;
	readonly GracefullDisconnect Disconnect = disconnect;

	public async Task<WsConnection> Connect()
	{
		var connection = await Decorated.Connect();
		return new GracefulConnection(connection, Disconnect);
	}

	class GracefulConnection(
		WsConnection decorated,
		GracefullDisconnect disconnect
	) : WsConnection
	{
		readonly WsConnection Decorated = decorated;
		readonly GracefullDisconnect Disconnect = disconnect;

		public Task Send<T>(WsClientOutMessage<T> message)
			=> Decorated.Send(message);

		public ChannelReader<T> Receive<T>()
			=> Decorated.Receive<T>();

		public async ValueTask DisposeAsync()
		{
			await Decorated.DisposeAsync();
			await Disconnect.WaitDisconnected();
		}
	}
}
