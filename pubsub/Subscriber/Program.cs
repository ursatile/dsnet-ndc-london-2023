using EasyNetQ;
using Messages;

const string AMQP = "amqps://spjuzoxw:mMT6Y4tSsLW6I6jX3FTIcjRDUodd7JaD@feisty-maroon-kiwi.rmq5.cloudamqp.com/spjuzoxw";

var bus = RabbitHutch.CreateBus(AMQP);
bus.PubSub.Subscribe<Greeting>("dylanbeattie", greeting => {
    if (greeting.Number % 5 == 0) {
        throw new Exception("Hey! I don't like 5!");
    }
    Console.WriteLine(greeting);
});

Console.WriteLine("Listening for messages...");
Console.ReadKey(true);