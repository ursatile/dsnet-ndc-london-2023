using Grpc.Core;
using GrpcServer;

namespace GrpcServer.Services;

public class GreeterService : Greeter.GreeterBase {
	private readonly ILogger<GreeterService> logger;
	public GreeterService(ILogger<GreeterService> logger) {
		this.logger = logger;
	}

	public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context) {
        logger.LogInformation($"Received request: {request}");
		return Task.FromResult(new HelloReply {
			Message = $"Hello, {request.Name}. The time is {DateTime.Now:O}"
		});
	}
}
