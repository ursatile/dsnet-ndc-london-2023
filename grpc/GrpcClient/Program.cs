// See https://aka.ms/new-console-template for more information

using Grpc.Net.Client;
using GrpcServer;

using var channel = GrpcChannel.ForAddress("https://workshop.ursatile.com:5003");
var grpcClient = new Greeter.GreeterClient(channel);
Console.WriteLine("Connected to gRPC! press a key to send a message...");
while (true) {
	Console.ReadKey(true);
	var request = new HelloRequest {
		Name = "NDC London"
	};
	var reply = grpcClient.SayHello(request);
	Console.WriteLine(reply.Message);
}

