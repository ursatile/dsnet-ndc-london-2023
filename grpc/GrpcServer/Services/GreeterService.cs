using Grpc.Core;
using GrpcServer;

namespace GrpcServer.Services;


public class GreeterService : Greeter.GreeterBase {
    static int requestCount = 0;
	private readonly ILogger<GreeterService> logger;

	public GreeterService(ILogger<GreeterService> logger) {
		this.logger = logger;
	}

	public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context) {
        Interlocked.Increment(ref requestCount);       
        logger.LogInformation($"Request {requestCount}: {request}");

        var greeting = (request.LanguageCode) switch {
            "pt-BR" => $"Olá {request.Name}",
            "mk-MK" => $"Здраво {request.Name}",
            "de-de" => $"Hallo  {request.Name}",
            "is-IS" => $"Halló, {request.Name}. Klukkan er {DateTime.Now:O}",
            "ar-DZ" => $"{request.Name} مرحبا",
            "en-GB" => $"Good morning, {request.Name}",
            "en-AU" => $"G'day, {request.Name}",
            _ => "Greetings, {request.Name}"
        };
        var weather = "Cloudy";
        return Task.FromResult(new HelloReply {
            Message = greeting,
            Weather = weather
        });
	}
}
