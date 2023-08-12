using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using Xunit;

namespace MySpot.Tests.Unit.Framework;

public class ServiceCollectionTests
{
    [Fact]
    public void test()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddScoped<IMessenger, Messenger>();
        serviceCollection.AddScoped<IMessenger, Messenger2>();
        
        var serviceProvider = serviceCollection.BuildServiceProvider();
        
        var messengers = serviceProvider.GetService<IEnumerable<IMessenger>>();
        var messengers2 = serviceProvider.GetServices<IMessenger>();

        messengers.Count().ShouldBe(2);
        messengers.First().ShouldBeOfType<Messenger>();
        messengers.Skip(1).First().ShouldBeOfType<Messenger2>();

        messengers2.Count().ShouldBe(2);
        messengers2.First().ShouldBeOfType<Messenger>();
        messengers2.Skip(1).First().ShouldBeOfType<Messenger2>();
    }
    
    private interface IMessenger
    {
        void Send();
        Guid GetId();
    }

    private class Messenger : IMessenger
    {
        private readonly Guid _id = Guid.NewGuid();

        public void Send()
            => Console.WriteLine($"Sending a message... [{_id}");

        public Guid GetId() => _id;
    }
    
    private class Messenger2 : IMessenger
    {
        private readonly Guid _id = Guid.NewGuid();

        public void Send()
            => Console.WriteLine($"Sending a message... [{_id}");

        public Guid GetId() => _id;
    }
}