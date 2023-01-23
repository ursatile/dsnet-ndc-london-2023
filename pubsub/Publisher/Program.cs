using EasyNetQ;
using Messages;

const string AMQP = "amqps://spjuzoxw:mMT6Y4tSsLW6I6jX3FTIcjRDUodd7JaD@feisty-maroon-kiwi.rmq5.cloudamqp.com/spjuzoxw";

var bus = RabbitHutch.CreateBus(AMQP);
Console.WriteLine("Press any key to send a message!");
var number = 1;
while (true) {
	Console.ReadKey(true);
	var greeting = new Greeting {
		Content = "NDC London",
		CreatedAt = DateTimeOffset.UtcNow,
		MachineName = Environment.MachineName,
		Number = number++
	};
	Console.WriteLine($"Publishing {greeting}");
	bus.PubSub.Publish(greeting);
}
