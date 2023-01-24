// See https://aka.ms/new-console-template for more information

using Grpc.Net.Client;
using GrpcServer;

using var channel = GrpcChannel.ForAddress("https://workshop.ursatile.com:5003");
var grpcClient = new Greeter.GreeterClient(channel);
Console.WriteLine("Connected to gRPC! press a key to send a message...");
var codes = new [] {
    "en-GB",
    "en-AU",
    "pt-BR",
    "de-DE",
    "mk-MK",
    "is-IS",
    "ar-DZ"
};

for(var i = 0; i < codes.Length; i++) {
    Console.WriteLine($"{i}: {codes[i]}");
}

while (true) {
    var index  = GetIntFromKey(Console.ReadKey(true));
    var code = (index < codes.Length ? codes[index]: String.Empty);    
	var request = new HelloRequest {
		Name = Environment.MachineName,
        LanguageCode = code
	};
    Console.WriteLine(request);
	var reply = grpcClient.SayHello(request);
	Console.WriteLine(reply.Message);
    Console.WriteLine($"The weather is: {reply.Weather}");
}

int GetIntFromKey(ConsoleKeyInfo key) {
    return key.KeyChar & 15;
}

