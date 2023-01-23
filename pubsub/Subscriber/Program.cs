using EasyNetQ;
using Messages;

const string AMQP = "amqps://spjuzoxw:mMT6Y4tSsLW6I6jX3FTIcjRDUodd7JaD@feisty-maroon-kiwi.rmq5.cloudamqp.com/spjuzoxw";

var bus = RabbitHutch.CreateBus(AMQP);
bus.PubSub.Subscribe<Greeting>("SUBSCRIPTION_ID", greeting => {
    Console.WriteLine(greeting);
});

Console.WriteLine("Listening for messages...");
Console.ReadKey(true);