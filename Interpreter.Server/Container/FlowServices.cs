using Microsoft.Extensions.DependencyInjection;
using Staticsoft.Flow;

namespace Staticsoft.Interpreter.Server;

static class FlowServices
{
	public static IServiceCollection UseFlowServices(this IServiceCollection services)
		=> services
			.UseJob<ProcessMessageJob, ProcessMessageJob.Input, ProcessMessageJob.Output>()
			.UseOperation<FindConversationOperation, FindConversationOperation.Input, FindConversationOperation.Output>()
			.UseOperation<PrintOperation, PrintOperation.Input, PrintOperation.Output>()
			.UseOperation<CurrentDateTimeOperation, CurrentDateTimeOperation.Input, CurrentDateTimeOperation.Output>()
			.UseOperation<CreateProgramOperation, CreateProgramOperation.Input, CreateProgramOperation.Output>()
			.UseOperation<ShowTableOperation, ShowTableOperation.Input, ShowTableOperation.Output>();
}
